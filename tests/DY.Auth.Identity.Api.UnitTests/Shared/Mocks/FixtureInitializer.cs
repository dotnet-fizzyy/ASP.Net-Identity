using AutoFixture;

using DY.Auth.Identity.Api.Core.Entities;

using System;
using System.Linq;

namespace DY.Auth.Identity.Api.UnitTests.Shared.Mocks;

/// <summary>
/// Fixture initializer configuration.
/// </summary>
public static class FixtureInitializer
{
    /// <summary>
    /// Configures and initializes common solution <see cref="Fixture"/> reusable for testing.
    /// </summary>
    /// <returns>The instance of <see cref="Fixture"/>.</returns>
    public static Fixture InitializeFixture()
    {
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(behavior => fixture.Behaviors.Remove(behavior));

        fixture.Behaviors
            .Add(new OmitOnRecursionBehavior());

        ConfigureDependencies(fixture);

        return fixture;
    }

    private static void ConfigureDependencies(Fixture fixture)
    {
        fixture.Customize<AppRole>(config => config
            .Without(prop => prop.UserRoles)
            .With(prop => prop.IsDeleted, false));

        fixture.Customize<AppUserRole>(config => config
            .Without(prop => prop.AppUser)
            .Without(prop => prop.Role));

        fixture.Customize<AppUser>(config => config
            .Without(prop => prop.UserRoles)
            .With(prop => prop.UserName, Faker.Name.FullName())
            .With(prop => prop.PhoneNumber, Faker.Phone.Number())
            .With(prop => prop.IsDeleted, false)
            .With(prop => prop.AccessFailedCount, 0)
            .With(prop => prop.LockoutEnd, (DateTime?)null));

        fixture.Customize<EmailTemplate>(config => config
            .With(prop => prop.Name, Faker.Internet.DomainWord())
            .With(prop => prop.Subject, Faker.Lorem.Paragraph())
            .With(prop => prop.Layout, "<html><body><p>Hello World!</p></body></html>")
            .With(prop => prop.IsDeleted, false));
    }
}
