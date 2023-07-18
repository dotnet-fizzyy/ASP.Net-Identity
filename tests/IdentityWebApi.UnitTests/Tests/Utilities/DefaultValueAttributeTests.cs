using IdentityWebApi.Core.Utilities;

using NUnit.Framework;

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
        Assert.False(result);
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
        Assert.False(result);
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
        Assert.False(result);
    }

    [Test]
    public void ShouldReturnTrueIfTypeHasNoSupport()
    {
        // Arrange
        decimal value = 100;

        var defaultValueAttribute = new DefaultValueAttribute();

        // Act
        var result = defaultValueAttribute.IsValid(value);

        // Assert
        Assert.True(result);
    }
}
