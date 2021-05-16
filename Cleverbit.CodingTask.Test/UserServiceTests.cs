using Cleverbit.CodingTask.Data.Models;
using Cleverbit.CodingTask.Data.Repository;
using Cleverbit.CodingTask.Services;
using Cleverbit.CodingTask.Utilities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Test
{
    public class UserServiceTests
    {
        private Mock<IHashService> hashService;
        private Mock<IUserRepository> userRepo;
        private IUserService serviceUnderTest;

        [SetUp]
        public void Setup()
        {
            this.hashService = new Mock<IHashService>();
            this.userRepo = new Mock<IUserRepository>();
            this.serviceUnderTest = new UserService(userRepo.Object, hashService.Object);
        }


        [Test]
        public async Task Authenticate_Success()
        {
            var dummyUser = new User()
            {
                Id = 1,
                UserName = "User1",
                Password = "NSbB5VP90oAeNp4biDt10gxbWbDNdgnOFY+vJTNJqYhB45pSXNw7ZQpYEwfGMwFn6K7Fy+pPsx9MFsOqv6u8ug=="
            };
            var dummyHashPw = "NSbB5VP90oAeNp4biDt10gxbWbDNdgnOFY+vJTNJqYhB45pSXNw7ZQpYEwfGMwFn6K7Fy+pPsx9MFsOqv6u8ug==";

            this.hashService.Setup(x => x.HashText(It.IsAny<string>())).ReturnsAsync(dummyHashPw);
            this.userRepo.Setup(x => x.GetUserByUserName(It.IsAny<string>())).ReturnsAsync(dummyUser);

            var result = await this.serviceUnderTest.Authenticate("User1", "Password1");
            var token = ("User1" + ":" + "Password1").Base64Encode();

            Assert.IsTrue(result.IsAuthenticated);
            Assert.IsNotNull(result.UserData);
            Assert.IsTrue(result.UserData.Token == token);
        }

        [Test]
        public async Task Authenticate_UserNameEmpty_Error()
        {            

            var result = await this.serviceUnderTest.Authenticate(null, "Password1");

            Assert.IsFalse(result.IsAuthenticated);
            Assert.IsNull(result.UserData);
            Assert.IsTrue(result.AuthErrorMessage == "Invalid username.");
        }

        [Test]
        public async Task Authenticate_UserNameNotFound_Error()
        {

            User dummyUser = null;
            this.userRepo.Setup(x => x.GetUserByUserName(It.IsAny<string>())).ReturnsAsync(dummyUser);

            var result = await this.serviceUnderTest.Authenticate("User1", "Password1");

            Assert.IsFalse(result.IsAuthenticated);
            Assert.IsNull(result.UserData);
            Assert.IsTrue(result.AuthErrorMessage == "Invalid username.");
        }

        [Test]
        public async Task Authenticate_PasswordEmpty_Error()
        {
            var result = await this.serviceUnderTest.Authenticate("User1", null);

            Assert.IsFalse(result.IsAuthenticated);
            Assert.IsNull(result.UserData);
            Assert.IsTrue(result.AuthErrorMessage == "Invalid password.");
        }

        [Test]
        public async Task Authenticate_PasswordNotMatch_Error()
        {
            var dummyUser = new User()
            {
                Id = 1,
                UserName = "User1",
                Password = "NSbB5VP90oAeNp4biDt10gxbWbDNdgnOFY+vJTNJqYhB45pSXNw7ZQpYEwfGMwFn6K7Fy+pPsx9MFsOqv6u8ug"
            };
            var dummyHashPw = "NSbB5VP90oAeNp4biDt10gxbWbDNdgnOFY+vJTNJqYhB45pSXNw7ZQpYEwfGMwFn6K7Fy+pPsx9MFsOqv6u8ug==";

            this.hashService.Setup(x => x.HashText(It.IsAny<string>())).ReturnsAsync(dummyHashPw);
            this.userRepo.Setup(x => x.GetUserByUserName(It.IsAny<string>())).ReturnsAsync(dummyUser);

            var result = await this.serviceUnderTest.Authenticate("User1", "Password1");
            var token = ("User1" + ":" + "Password1").Base64Encode();

            Assert.IsFalse(result.IsAuthenticated);
            Assert.IsNull(result.UserData);
            Assert.IsTrue(result.AuthErrorMessage == "Invalid password.");
        }
    }
}
