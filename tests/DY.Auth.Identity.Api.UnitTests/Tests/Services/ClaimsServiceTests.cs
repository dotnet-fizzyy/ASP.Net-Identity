using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Presentation.Services;
using DY.Auth.Identity.Api.UnitTests.Shared.Constants;
using DY.Auth.Identity.Api.UnitTests.Shared.Controllers;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;

namespace DY.Auth.Identity.Api.UnitTests.Tests.Services;

[TestFixture]
public class ClaimsServiceTests
{
    [Test]
    [Category("Positive")]
    public void ShouldReturnUserIdFromIdentitySuccessfully()
    {
        // Arrange
        var identityUser = UserControllerTestsDataFactory.GetUserIdentity(UserConstants.UserId);

        // Act
        var userIdResult = ClaimsService.GetUserIdFromIdentityUser(identityUser);

        // Assert
        ClassicAssert.AreEqual(ServiceResultType.Success, userIdResult.Result);
        ClassicAssert.AreEqual(new Guid(UserConstants.UserId), userIdResult.Data);
    }

    [Category("Negative")]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("123")]
    public void ShouldReturnInvalidDataFromIdentityOnAttemptToGetIncorrectUserId(string userId)
    {
        // Arrange
        var identityUser = UserControllerTestsDataFactory.GetUserIdentity(userId);

        // Act
        var userIdResult = ClaimsService.GetUserIdFromIdentityUser(identityUser);

        // Assert
        ClassicAssert.AreEqual(ServiceResultType.InvalidData, userIdResult.Result);
    }
}
