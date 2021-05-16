using Cleverbit.CodingTask.Data.Repository;
using Cleverbit.CodingTask.Services.DTO;
using Cleverbit.CodingTask.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository userRepo;
        private readonly IHashService hashService;
        private const string incorrectUserName_error_message = "Invalid username.";
        private const string incorrectPassword_error_message = "Invalid password.";

        public UserService(IUserRepository _userRepo, IHashService _hashService)
        {
            this.userRepo = _userRepo;
            this.hashService = _hashService;
        }

        public async Task<AuthResult> Authenticate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return ReturnResult(null, false, incorrectUserName_error_message);
            }
            if (string.IsNullOrEmpty(password))
            {
                return ReturnResult(null, false, incorrectPassword_error_message);
            }

            var userEntity = await this.userRepo.GetUserByUserName(userName);
            if (userEntity == null)
            {
                return ReturnResult(null, false, incorrectUserName_error_message);
            }

            var hashPassword = await this.hashService.HashText(password);
            if (hashPassword != userEntity.Password)
            {
                return ReturnResult(null, false, incorrectPassword_error_message);
            }

            var credential = userName + ":" + password;
            var base64Credentials = credential.Base64Encode();
            var user = new User()
            {
                UserName = userEntity.UserName,
                Id = userEntity.Id,
                Token = base64Credentials
            };
            return ReturnResult(user, true, null);
        }


        private AuthResult ReturnResult(User userData, bool isAuthenticated, string errMessage)
        {
            var authResult = new AuthResult()
            {
                AuthErrorMessage = errMessage,
                IsAuthenticated = isAuthenticated,
                UserData = userData
            };
            return authResult;
        }
    }
}
