using DY.Auth.Identity.Api.Core.Utilities;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;

namespace DY.Auth.Identity.Api.UnitTests.Tests.Utilities;

[TestFixture]
public class DefaultValueAttributeTests
{
    [Test]
    [Category("Negative")]
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
    [Category("Positive")]
    public void ShouldReturnTrueIfInt32IsDefault()
    {
        // Arrange
        const int value = 5;

        var defaultValueAttribute = new DefaultValueAttribute();

        // Act
        var result = defaultValueAttribute.IsValid(value);

        // Assert
        ClassicAssert.True(result);
    }

    [Test]
    [Category("Negative")]
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

    [Test]
    [Category("Positive")]
    public void ShouldReturnTrueIfGuidIsNotDefault()
    {
        // Arrange
        var value = Guid.NewGuid();

        var defaultValueAttribute = new DefaultValueAttribute();

        // Act
        var result = defaultValueAttribute.IsValid(value);

        // Assert
        ClassicAssert.True(result);
    }

    [Test]
    [Category("Negative")]
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
    [Category("Positive")]
    public void ShouldReturnTrueIfStringIsNotEmpty()
    {
        // Arrange
        const string value = "TestValue";

        var defaultValueAttribute = new DefaultValueAttribute();

        // Act
        var result = defaultValueAttribute.IsValid(value);

        // Assert
        ClassicAssert.True(result);
    }

    [Test]
    [Category("Positive")]
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
