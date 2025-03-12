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

            public string? PhoneNumber { get; set; } // Thêm số điện thoại
            public DateTime CreateAt { get; set; }
            public DateTime UpdateAt { get; set; }
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }

            public string Address { get; set; }


        }

        public class UserDTO
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public string? PhoneNumber { get; set; } 
            public DateTime CreateAt { get; set; }
            public DateTime UpdateAt { get; set; }
            public string Address { get; set; }
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
    }
}
