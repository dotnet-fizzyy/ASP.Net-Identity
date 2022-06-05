using IdentityWebApi.ApplicationLogic.Services;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.UnitTests.Shared.Constants;
using IdentityWebApi.UnitTests.Shared.Controllers;

using NUnit.Framework;

using System;

namespace IdentityWebApi.UnitTests.Tests.Services;

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
}
