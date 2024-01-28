using IdentityWebApi.Core.Utilities;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;

namespace IdentityWebApi.UnitTests.Tests.Utilities;

[TestFixture]
public class DefaultValueAttributeTests
{
    [Test]
    public void ShouldReturnFalseIfInt32IsDefault()
    {
        // Arrange
        const int value = 0;

        var defaultValueAttribute = new DefaultValueAttribute();

        // Act
        var result = defaultValueAttribute.IsValid(value);

        // Assert
        ClassicAssert.False(result);
    }

    [Test]
    public void ShouldReturnFalseIfGuidIsDefault()
    {
        // Arrange
        var value = Guid.Empty;

        var defaultValueAttribute = new DefaultValueAttribute();

        // Act
        var result = defaultValueAttribute.IsValid(value);

        // Assert
        ClassicAssert.False(result);
    }

    [TestCase("")]
    [TestCase("       ")]
    public void ShouldReturnFalseIfStringIsEmptyOrWhiteSpace(string value)
    {
        // Arrange
        var defaultValueAttribute = new DefaultValueAttribute();

        // Act
        var result = defaultValueAttribute.IsValid(value);

        // Assert
        ClassicAssert.False(result);
    }

    [Test]
    public void ShouldReturnTrueIfTypeHasNoSupport()
    {
        // Arrange
        const decimal value = 100;

        var defaultValueAttribute = new DefaultValueAttribute();

        // Act
        var result = defaultValueAttribute.IsValid(value);

        // Assert
        ClassicAssert.True(result);
    }
}
