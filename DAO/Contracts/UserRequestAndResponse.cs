using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace DAO.Contracts
{
    public class UserRequestAndResponse
    {
        public class UserRegisterRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Gender { get; set; }

            public string PhoneNumber { get; set; } // Thêm số điện thoại

            public string Address { get; set; }

            public void Validate()
            {
                // Check FirstName và LastName
                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
                    FirstName.Any(ch => !char.IsLetter(ch) && !char.IsWhiteSpace(ch)) ||
                    LastName.Any(ch => !char.IsLetter(ch) && !char.IsWhiteSpace(ch)))
                {
                    throw new BadHttpRequestException("Họ và tên không hợp lệ");
                }

                // Check Email hợp lệ
                if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    throw new BadHttpRequestException("Email không hợp lệ");
                }

                // Check số điện thoại hợp lệ (Việt Nam: 10 số, bắt đầu từ 0)
                //|| !Regex.IsMatch(PhoneNumber, @"^0\d{9}$"
                if (string.IsNullOrEmpty(PhoneNumber))
                {
                    throw new BadHttpRequestException("Số điện thoại không hợp lệ");
                }

                // Check Password: tối thiểu 6 ký tự, 1 chữ hoa, 1 số, 1 ký tự đặc biệt
                if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6 ||
                    !Regex.IsMatch(Password, @"[A-Z]") ||   // Ít nhất 1 chữ cái in hoa
                    !Regex.IsMatch(Password, @"\d") ||      // Ít nhất 1 chữ số
                    !Regex.IsMatch(Password, @"[^a-zA-Z0-9]"))  // Ít nhất 1 ký tự đặc biệt
                {
                    throw new BadHttpRequestException("Mật khẩu phải có tối thiểu 6 kí tự, 1 chữ hoa, 1 số và 1 ký tự đặc biệt");
                }

                // Check địa chỉ (nếu có)
                if (!string.IsNullOrWhiteSpace(Address) && Address.Length < 5)
                {
                    throw new BadHttpRequestException("Địa chỉ quá ngắn");
                }
            }


        }

        public class ResendConfirmEmailRequest
        {
            public string Email { get; set; }
        }


        public class ForgotPasswordRequest
        {
            public string Email { get; set; }
        }

        public class ResetPasswordRequest
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public string NewPassword { get; set; }
        }



        public class UserResponse
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }

            public string? AvatarURL { get; set; }
            public string? PhoneNumber { get; set; } // Thêm số điện thoại
            public DateTime CreateAt { get; set; }
            public DateTime UpdateAt { get; set; }
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }

            public string Address { get; set; }
            public int? orderCode { get; set; }


        }

        public class UserResponseAdmin
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }

            public string? FirstName { get; set; }
            public string? LastName { get; set; }

            public string? PhoneNumber { get; set; }
            public DateTime CreateAt { get; set; }
            public DateTime UpdateAt { get; set; }
            public string Address { get; set; }

            public string Role { get; set; }
        }

        public class UserDTO
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }

            public string FullName { get; set; }

            public string? PhoneNumber { get; set; }
            public DateTime CreateAt { get; set; }
            public DateTime UpdateAt { get; set; }
            public string Address { get; set; }

            public string Role { get; set; }
        }
        public class UserLoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CurrentUserResponse
        {

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public string AccessToken { get; set; }
            public DateTime CreateAt { get; set; }
            public DateTime UpdateAt { get; set; }

        }


        public class UpdateUserRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string Email { get; set; }
            public string Password { get; set; }
            public string Gender { get; set; }
        }


        public class RevokeRefreshTokenResponse
        {
            public string Message { get; set; }
        }


        public class RefreshTokenRequest
        {
            public string RefreshToken { get; set; }
        }

        public class UpdateOrderCodeRequest
        {
            public int? orderCode { get; set; }
        }

    }
}