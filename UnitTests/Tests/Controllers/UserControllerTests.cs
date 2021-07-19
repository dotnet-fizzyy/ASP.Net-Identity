using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.Services;
using IdentityWebApi.PL.Controllers;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using UnitTests.Shared.Constants;
using UnitTests.Shared.Controllers;
using UnitTests.Shared.DTO;

namespace UnitTests.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private const string ErrorMessage = "Error Test Message";
        
        [Test]
        public async Task ShouldReturnUserModelByTokenClaimsOnGetUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var userId = new Guid(UserConstants.UserId);
            var userResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.Success, userId);
            var userIdentity = UserControllerTestsDataFactory.GetUserIdentity(UserConstants.UserId);
            
            var userController = new UserController(userService.Object, claimsService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = userIdentity
                    }
                }
            };
            
            userService.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(userResult);
            
            //Act
            var result = await userController.GetUserByToken();

            //Assert
            Assert.NotNull(result.Value);
            Assert.AreEqual(userId, result.Value.Id);
            
            userService.Verify(x => x.GetUserAsync(It.IsAny<Guid>()));
        }
        
        [Test]
        public async Task ShouldReturnNotFoundStatusByTokenClaimsOnGetUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var serviceResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.NotFound);
            var userIdentity = UserControllerTestsDataFactory.GetUserIdentity(UserConstants.UserId);

            var userController = new UserController(userService.Object, claimsService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = userIdentity
                    }
                }
            };
            
            userService.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(serviceResult);
            
            //Act
            var userControllerResult = await userController.GetUserByToken();

            //Assert
            Assert.That(userControllerResult.Result, Is.TypeOf<NotFoundResult>());
            Assert.Null(userControllerResult.Value);
            
            userService.Verify(x => x.GetUserAsync(It.IsAny<Guid>()));
        }
        
        [Test]
        public async Task ShouldReturnUserModelOnGetUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var userController = new UserController(userService.Object, claimsService);

            var userId = new Guid(UserConstants.UserId);
            var userResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.Success, userId);

            userService.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(userResult);
            
            //Act
            var result = await userController.GetUser(userId);

            //Assert
            Assert.NotNull(result.Value);
            Assert.AreEqual(userId, result.Value.Id);
            
            userService.Verify(x => x.GetUserAsync(It.IsAny<Guid>()));
        }
        
        [Test]
        public async Task ShouldNotFoundStatusForMissingUserOnGetUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var userController = new UserController(userService.Object, claimsService);

            var userId = new Guid(UserConstants.UserId);
            var userResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.NotFound);

            userService.Setup(x => x.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(userResult);
            
            //Act
            var result = await userController.GetUser(userId);

            //Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
            Assert.Null(result.Value);
            
            userService.Verify(x => x.GetUserAsync(It.IsAny<Guid>()));
        }

        [Test]
        public async Task ShouldCreateUserSuccessfullyOnCreateUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var userController = new UserController(userService.Object, claimsService);

            var userDtoModel = UserDtoTestsData.GetUserActionDtoModel(new Guid(UserConstants.UserId));
            var userCreationResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.Success, userRoles: new [] { userDtoModel.UserRole });

            userService.Setup(x => x.CreateUserAsync(It.IsAny<UserActionModel>())).ReturnsAsync(userCreationResult);

            //Act
            var actionResult = await userController.CreateUser(userDtoModel);

            //Assert
            Assert.NotNull(actionResult.Value);
            Assert.AreEqual(userCreationResult.Data.Id, actionResult.Value.Id);
            Assert.AreEqual(userCreationResult.Data.UserName, actionResult.Value.UserName);
            Assert.True(userCreationResult.Data.Roles.Any(x => string.Equals(x, userDtoModel.UserRole, StringComparison.OrdinalIgnoreCase)));

            userService.Verify(x => x.CreateUserAsync(It.IsAny<UserActionModel>()));
        }

        [Test]
        public async Task ShouldUpdateUserSuccessfullyOnUpdateUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var userController = new UserController(userService.Object, claimsService);

            var userDtoModel = UserDtoTestsData.GetUserActionDtoModel(new Guid(UserConstants.UserId));
            var userUpdateResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.Success, userDtoModel.Id);

            userService.Setup(x => x.UpdateUserAsync(It.IsAny<UserActionModel>())).ReturnsAsync(userUpdateResult);

            //Act
            var actionResult = await userController.UpdateUser(userDtoModel);

            //Assert
            Assert.NotNull(actionResult.Value);
            Assert.AreEqual(userUpdateResult.Data.Id, actionResult.Value.Id);
            Assert.AreEqual(userUpdateResult.Data.UserName, actionResult.Value.UserName);

            userService.Verify(x => x.UpdateUserAsync(It.IsAny<UserActionModel>()));
        }
        
        [Test]
        public async Task ShouldReturnNotFoundForMussingUserOnUpdateUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var userController = new UserController(userService.Object, claimsService);

            var userDtoModel = UserDtoTestsData.GetUserActionDtoModel(new Guid(UserConstants.UserId));
            var userUpdateResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.NotFound, message: ErrorMessage);

            userService.Setup(x => x.UpdateUserAsync(It.IsAny<UserActionModel>())).ReturnsAsync(userUpdateResult);

            //Act
            var actionResult = await userController.UpdateUser(userDtoModel);

            //Assert
            Assert.That(actionResult.Result, Is.TypeOf<NotFoundObjectResult>());
            var notFoundResult = (NotFoundObjectResult) actionResult.Result;
            
            Assert.AreEqual(ErrorMessage, notFoundResult.Value);
            Assert.Null(actionResult.Value);

            userService.Verify(x => x.UpdateUserAsync(It.IsAny<UserActionModel>()));
        }
        
        [Test]
        public async Task ShouldRemoveUserSuccessfullyOnRemoveUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var userController = new UserController(userService.Object, claimsService);
            var userUpdateResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.Success);

            var userId = new Guid(UserConstants.UserId);

            userService.Setup(x => x.RemoveUserAsync(It.IsAny<Guid>())).ReturnsAsync(userUpdateResult);

            //Act
            var actionResult = await userController.RemoveUser(userId);

            //Assert
            Assert.That(actionResult, Is.TypeOf<NoContentResult>());
            
            userService.Verify(x => x.RemoveUserAsync(It.IsAny<Guid>()));
        }
        
        [Test]
        public async Task ShouldReturnNotFoundStatusForMissingUserOnRemoveUser()
        {
            //Arrange
            var userService = new Mock<IUserService>();
            var claimsService = new ClaimsService();

            var userController = new UserController(userService.Object, claimsService);
            var userUpdateResult = UserControllerTestsDataFactory.GetUserResult(ServiceResultType.NotFound);

            var userId = new Guid(UserConstants.UserId);

            userService.Setup(x => x.RemoveUserAsync(It.IsAny<Guid>())).ReturnsAsync(userUpdateResult);

            //Act
            var actionResult = await userController.RemoveUser(userId);

            //Assert
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
            
            userService.Verify(x => x.RemoveUserAsync(It.IsAny<Guid>()));
        }
    }
}