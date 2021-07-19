using System;
using System.Linq;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Services;
using NUnit.Framework;
using UnitTests.Shared.Constants;
using UnitTests.Shared.Controllers;

namespace UnitTests.Tests.Services
{
    [TestFixture]
    public class ClaimsServiceTests
    {
        [Test]
        public void ShouldReturnUserIdFromIdentitySuccessfully()
        {
            //Arrange
            var identityUser = UserControllerTestsDataFactory.GetUserIdentity(UserConstants.UserId);
            
            var claimsService = new ClaimsService();
            
            //Act
            var userIdResult = claimsService.GetUserIdFromIdentityUser(identityUser);

            //Assert
            Assert.AreEqual(ServiceResultType.Success, userIdResult.Result);
            Assert.AreEqual(new Guid(UserConstants.UserId), userIdResult.Data);
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase("123")]
        public void ShouldReturnInvalidDataFromIdentityOnAttemptToGetIncorrectUserId(string userId)
        {
            //Arrange
            var identityUser = UserControllerTestsDataFactory.GetUserIdentity(userId);
            
            var claimsService = new ClaimsService();
            
            //Act
            var userIdResult = claimsService.GetUserIdFromIdentityUser(identityUser);

            //Assert
            Assert.AreEqual(ServiceResultType.InvalidData, userIdResult.Result);
        }
        
        [Test]
        public void ShouldReturnUserEmailFromIdentitySuccessfully()
        {
            //Arrange
            var identityUser = UserControllerTestsDataFactory.GetUserIdentity(email: UserConstants.UserEmail);
            
            var claimsService = new ClaimsService();
            
            //Act
            var userEmailResult = claimsService.GetUserEmailFromIdentityUser(identityUser);

            //Assert
            Assert.AreEqual(ServiceResultType.Success, userEmailResult.Result);
            Assert.AreEqual(UserConstants.UserEmail, userEmailResult.Data);
        }
        
        [Test]
        public void ShouldReturnInvalidDataFromIdentityOnAttemptToGetIncorrectEmail()
        {
            //Arrange
            var identityUser = UserControllerTestsDataFactory.GetUserIdentity();
            
            var claimsService = new ClaimsService();
            
            //Act
            var userEmailResult = claimsService.GetUserEmailFromIdentityUser(identityUser);

            //Assert
            Assert.AreEqual(ServiceResultType.InvalidData, userEmailResult.Result);
            Assert.Null(userEmailResult.Data);
        }
        
        [Test]
        public void ShouldReturnUserRolesFromIdentitySuccessfully()
        {
            //Arrange
            var userRoles = new[] { UserConstants.UserRoleUser, UserConstants.UserRoleAdministrator };
            var identityUser = UserControllerTestsDataFactory.GetUserIdentity(roles: userRoles);
            
            var claimsService = new ClaimsService();
            
            //Act
            var userEmailResult = claimsService.GetUserRolesFromIdentityUser(identityUser);

            //Assert
            Assert.AreEqual(ServiceResultType.Success, userEmailResult.Result);
            Assert.NotNull(userEmailResult.Data);
            Assert.IsNotEmpty(userEmailResult.Data);
            Assert.True(userEmailResult.Data.All(x => userRoles.Contains(x)));
        }
        
        [Test]
        public void ShouldReturnInvalidDataFromIdentityOnAttemptToGetIncorrectRoles()
        {
            //Arrange
            var identityUser = UserControllerTestsDataFactory.GetUserIdentity();
            
            var claimsService = new ClaimsService();
            
            //Act
            var userEmailResult = claimsService.GetUserRolesFromIdentityUser(identityUser);

            //Assert
            Assert.AreEqual(ServiceResultType.InvalidData, userEmailResult.Result);
            Assert.Null(userEmailResult.Data);
        }
    }
}