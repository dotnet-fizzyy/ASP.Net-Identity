using DY.Auth.Identity.Api.Core.Utilities;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System.Diagnostics;

namespace DY.Auth.Identity.Api.UnitTests.Tests.Utilities;

/// <summary>
/// Unit tests for <see cref="TracingUtilities" />.
/// </summary>
public class TracingUtilitiesTests
{
    /// <summary>
    /// Tests <see cref="TracingUtilities.SetCallerDisplayName"/> successful execution with setting expected display name.
    /// </summary>
    [Test]
    [Category("Positive")]
    public void SetCallerDisplayName_ShouldSetActivityDisplayNameByCallerInvocations()
    {
        // Arrange
        var activity = new Activity(operationName: "TestOperationName");

        const string expectedResult = "TracingUtilitiesTests.SetCallerDisplayName_ShouldSetActivityDisplayNameByCallerInvocations";

        // Act
        activity.SetCallerDisplayName();

        // Assert
        ClassicAssert.AreEqual(expectedResult, activity.DisplayName);
    }

    /// <summary>
    /// Tests <see cref="TracingUtilities.SetCallerDisplayName"/> with interrupted execution on nullable <see cref="Activity"/> instance.
    /// </summary>
    [Test]
    [Category("Negative")]
    public void SetCallerDisplayName_ShouldStopExecutionOnEmpty()
    {
        // Arrange
        Activity activity = null;

        // Act
        activity.SetCallerDisplayName();

        // Assert
        ClassicAssert.Null(activity);
    }
}
