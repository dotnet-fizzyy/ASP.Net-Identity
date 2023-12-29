using IdentityWebApi.Core.Utilities;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System.Collections.Generic;
using System.Linq;

namespace IdentityWebApi.UnitTests.Tests.Utilities;

[TestFixture]
public class ExtensionsTests
{
    [TestCaseSource(nameof(EmptyCollections))]
    public void ShouldReturnTrueIfCollectionIsNullOrEmpty(IEnumerable<int> collection)
    {
        // Arrange & Act & Assert
        ClassicAssert.True(collection.IsNullOrEmpty());
    }

    [Test]
    public void ShouldReturnFalseForNonEmptyCollection()
    {
        // Arrange
        var collection = new List<int> { 1 };

        // Act & Assert
        ClassicAssert.False(collection.IsNullOrEmpty());
    }

    public static object[] EmptyCollections = { null, Enumerable.Empty<int>() };
}
