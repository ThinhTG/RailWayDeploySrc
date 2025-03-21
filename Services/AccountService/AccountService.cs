using AutoMapper;
using DAO.Contracts;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Repositories.Pagging;
using Repositories.UnitOfWork;
using Repositories.WalletRepo;
using Services.DTO;
using Services.Email;
using Services.Request;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using static DAO.Contracts.UserRequestAndResponse;

namespace Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly IEmailService _emailService;
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;


        public AccountService(ITokenService tokenService, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<AccountService> logger, IEmailService emailService, IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<UserResponse> RegisterAsync(UserRegisterRequest request)
        {
            request.Validate();
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists");
            }


            var newUser = _mapper.Map<ApplicationUser>(request);

            // Generate a unique username
            newUser.UserName = GenerateUserName(request.FirstName, request.LastName);
            newUser.EmailConfirmed = false;
            var result = await _userManager.CreateAsync(newUser, request.Password);
            await CreateWalletForUserAsync(newUser.Id);
            await _userManager.AddToRoleAsync(newUser, "User");

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }
            //  Tạo token xác thực email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            await _emailService.SendConfirmationEmailAsync(newUser, token);
            await _tokenService.GenerateToken(newUser);
            newUser.CreateAt = DateTime.Now;
            newUser.UpdateAt = DateTime.Now;
            return _mapper.Map<UserResponse>(newUser);
        }

        public async Task CreateWalletForUserAsync(string accountId)
        {
            var wallet = new Models.Wallet
            {
                WalletId = Guid.NewGuid(),
                AccountId = accountId,
                Balance = 0
            };
            await _walletRepository.CreateWallet(wallet);
        }

        private string GenerateUserName(string firstName, string lastName)
        {
            var baseUsername = $"{firstName}{lastName}".ToLower();

            // Check if the username already exists
            var username = baseUsername;
            var count = 1;
            while (_userManager.Users.Any(u => u.UserName == username))
            {
                username = $"{baseUsername}{count}";
                count++;
            }
            return username;
        }



        public async Task<UserResponse> LoginAsync(UserLoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new ArgumentException("Invalid email or password");
            }

            if (!user.EmailConfirmed)
            {
                //return null;
                throw new AuthenticationException("Email is not confirmed");
            }

            // Generate access token
            var token = await _tokenService.GenerateToken(user);

            // Generate refresh token
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Hash the refresh token and store it in the database or override the existing refresh token
            using var sha256 = SHA256.Create();
            var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
            user.RefreshToken = Convert.ToBase64String(refreshTokenHash);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);

            user.CreateAt = DateTime.Now;

            // Update user information in database
            var result = await _userManager.UpdateAsync(user);
            //create wallet for user login with google
            await CreateWalletForUserAsync(user.Id);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update user: {errors}");
            }

            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
            userResponse.AccessToken = token;
            userResponse.RefreshToken = refreshToken;
            userResponse.Address = user.Address;
            userResponse.AvatarURL = user.AvatarURL;

            return userResponse;
        }

        public async Task<UserResponse> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting user by id");
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<CurrentUserResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            _logger.LogInformation("RefreshToken");

            // Hash the incoming RefreshToken and compare it with the one stored in the database
            using var sha256 = SHA256.Create();
            var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.RefreshToken));
            var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);

            // Find user based on the refresh token
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);
            if (user == null)
            {
                throw new Exception("Invalid refresh token");
            }

            // Validate the refresh token expiry time
            if (user.RefreshTokenExpiryTime < DateTime.Now)
            {
                throw new Exception("Refresh token expired");
            }

            // Generate a new access token
            var newAccessToken = await _tokenService.GenerateToken(user);
            _logger.LogInformation("Access token generated successfully");
            var currentUserResponse = _mapper.Map<CurrentUserResponse>(user);
            currentUserResponse.AccessToken = newAccessToken;
            return currentUserResponse;
        }

        public async Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest refreshTokenRemoveRequest)
        {
            try
            {
                // Hash the refresh token
                using var sha256 = SHA256.Create();
                var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshTokenRemoveRequest.RefreshToken));
                var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);

                // Find the user based on the refresh token
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);
                if (user == null)
                {
                    throw new Exception("Invalid refresh token");
                }

                // Validate the refresh token expiry time
                if (user.RefreshTokenExpiryTime < DateTime.Now)
                {
                    throw new Exception("Refresh token expired");
                }

                // Remove the refresh token
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                // Update user information in database
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return new RevokeRefreshTokenResponse
                    {
                        Message = "Failed to revoke refresh token"
                    };
                }
                return new RevokeRefreshTokenResponse
                {
                    Message = "Refresh token revoked successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to revoke refresh token");
            }
        }

        public async Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            user.UpdateAt = DateTime.Now;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Gender = request.Gender;

            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateAsync(Guid id, UpdateOrderCodeRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new Exception("User not found");
            }

            user.UpdateAt = DateTime.Now;
            user.orderCode = request.orderCode;

            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            await _userManager.DeleteAsync(user);
        }



        public async Task<bool> ResendConfirmationEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.EmailConfirmed) return false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://yourdomain.com/api/account/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendEmailAsync(user.Email, "Xác thực lại email", $"Nhấp vào link: {confirmationLink}");
            return true;
        }

        public async Task<UserResponse> LoginGoogle(GoogleLoginRequest request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token)
                          ?? throw new Exception("Invalid Google token.");

            string email = payload.Email;
            string name = payload.Name;
            string googleId = payload.Subject;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = googleId,
                    FirstName = payload.Name ?? "Unknown",
                    LastName = "",
                    Gender = "Not Specified",
                    PhoneNumber = "Unknown",
                    Address = "Not Provided",
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"User creation failed: {errors}");
                }

                await CreateWalletForUserAsync(user.Id);
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    _logger.LogError("Role assignment failed: {errors}", errors);
                    throw new Exception($"Role assignment failed: {errors}");
                }
            }

            // Generate access token
            var token = await _tokenService.GenerateToken(user);

            // Generate refresh token
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Hash the refresh token and store it in the database or override the existing refresh token
            using var sha256 = SHA256.Create();
            var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
            user.RefreshToken = Convert.ToBase64String(refreshTokenHash);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);


            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update user: {errors}", errors);
                throw new Exception($"Failed to update user: {errors}");
            }

            var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
            userResponse.AccessToken = token;
            userResponse.RefreshToken = refreshToken;
            userResponse.Address = user.Address;

            return userResponse;
        }

        public async Task<PaginatedList<UserResponseAdmin>> GetAllAccountsAsync(int pageNumber, int pageSize)
        {
            var users = await _userManager.Users.ToListAsync(); // Lấy danh sách User trước

            var listAccount = new List<UserResponseAdmin>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                string firstRole = roles.FirstOrDefault()!;
                var userDto = _mapper.Map<UserResponseAdmin>(user);
                userDto.Role = firstRole;
                listAccount.Add(userDto);
            }

            return  PaginatedList<UserResponseAdmin>.Create(listAccount, pageNumber, pageSize);
        }


        //public async Task<PaginatedList<UserDTO>> GetAllAccountsAsync(int pageNumber, int pageSize)
        //{
        //    IQueryable<ApplicationUser> usersQuery = _userManager.Users.AsQueryable();

        //    IQueryable<UserDTO> userWithRolesQuery = from user in usersQuery.AsQueryable() // Đảm bảo IQueryable
        //                                             select new UserDTO
        //                                             {
        //                                                 Email = user.Email,
        //                                                 Gender = user.Gender,
        //                                                 PhoneNumber = user.PhoneNumber,
        //                                                 CreateAt = user.CreateAt,
        //                                                 UpdateAt = user.UpdateAt,
        //                                                 Address = user.Address,
        //                                                 Role =await _userManager.GetRolesAsync(user)
        //                                             };

        //    return await PaginatedList<UserDTO>.CreateAsync(userWithRolesQuery.AsNoTracking(), pageNumber, pageSize);

        //}

        public async Task<UserResponse> AdminUpdateAsync(Guid id, AdminUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.UpdateAt = DateTime.Now;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Gender = request.Gender;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;

            // Update user role
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, request.Role);

            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserResponse>(user);
        }

        /// <summary>
        /// Xác nhận email của người dùng dựa vào token và userId.
        /// </summary>
        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        /// <summary>
        /// Gửi lại email xác nhận tài khoản.
        /// </summary>
        public async Task<bool> ResendConfirmEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
                return false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.ResendConfirmationEmailAsync(user, token);
            return true;
        }

        /// <summary>
        /// Gửi email để người dùng đặt lại mật khẩu.
        /// </summary>
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendResetPasswordEmailAsync(user, token);
            return true;
        }

        /// <summary>
        /// Đặt lại mật khẩu mới cho người dùng.
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new KeyNotFoundException("User not found");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> UpdateAvatarAsync(string userId, string avatarUrl)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.AvatarURL = avatarUrl;
            user.UpdateAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<UserResponse> UpdateAccount(UpdateProfileDTO updateProfileDTO)
        {
            var user = await _userManager.FindByIdAsync(updateProfileDTO.AccountId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

         if(!string.IsNullOrEmpty(updateProfileDTO.PhoneNumber))
            {
                user.PhoneNumber = updateProfileDTO.PhoneNumber;
            }

         if(!string.IsNullOrEmpty(updateProfileDTO.FirstName))
            {
                user.FirstName = updateProfileDTO.FirstName;
            }

         if (!string.IsNullOrEmpty(updateProfileDTO.LastName))
            {
                user.LastName = updateProfileDTO.LastName;
            }

            if (!string.IsNullOrEmpty(updateProfileDTO.Gender))
            {
                user.Gender = updateProfileDTO.Gender;
            }

            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserResponse>(user);
        }
    }

}


