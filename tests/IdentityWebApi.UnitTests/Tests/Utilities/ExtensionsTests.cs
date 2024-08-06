using DY.Auth.Identity.Api.Core.Utilities;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityWebApi.UnitTests.Tests.Utilities;

[TestFixture]
public class ExtensionsTests
{
    [Test]
    [TestCaseSource(nameof(EnumerableEmptyCollections))]
    [Category("Positive")]
    public void ShouldReturnTrueIfEnumerableIsNullOrEmpty(IEnumerable<int> enumerable)
    {
        // Arrange & Act & Assert
        ClassicAssert.True(enumerable.IsNullOrEmpty());
    }

    [Test]
    [Category("Negative")]
    public void ShouldReturnFalseForNonEmptyEnumerable()
    {
        // Arrange
        var enumerable = new Stack<int>();
        enumerable.Push(1);

        // Act & Assert
        ClassicAssert.False(enumerable.IsNullOrEmpty());
    }

    [Test]
    [TestCaseSource(nameof(SetEmptyCollections))]
    [Category("Positive")]
    public void ShouldReturnTrueIfSetIsNullOrEmpty(ISet<int> set)
    {
        // Arrange & Act & Assert
        ClassicAssert.True(set.IsNullOrEmpty());
    }

    [Test]
    [Category("Negative")]
    public void ShouldReturnFalseForNonEmptySet()
    {
        // Arrange
        var set = new HashSet<int> { 1 };

        // Act & Assert
        ClassicAssert.False(set.IsNullOrEmpty());
    }

    [Test]
    [TestCaseSource(nameof(CollectionEmptyCollections))]
    [Category("Positive")]
    public void ShouldReturnTrueIfCollectionIsNullOrEmpty(ICollection<int> collection)
    {
        // Arrange & Act & Assert
        ClassicAssert.True(collection.IsNullOrEmpty());
    }

    [Test]
    [Category("Negative")]
    public void ShouldReturnFalseForNonEmptyCollection()
    {
        // Arrange
        var collection = new List<int> { 1 };

        // Act & Assert
        ClassicAssert.False(collection.IsNullOrEmpty());
    }

    public static object[] EnumerableEmptyCollections = [null, Enumerable.Empty<int>()];

    public static object[] SetEmptyCollections = [null, new HashSet<int>(), new SortedSet<int>()];

    public static object[] CollectionEmptyCollections = [null, new List<int>(), Array.Empty<int>()];
}
