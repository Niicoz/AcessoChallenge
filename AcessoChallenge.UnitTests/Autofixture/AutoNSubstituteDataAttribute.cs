using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AcessoChallenge.UnitTests.Autofixture
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        public AutoNSubstituteDataAttribute()
            : base(FixtureFactory)
        {
        }

        public static IFixture FixtureFactory()
        {
            var fixture = new Fixture();

            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());

            fixture
                .Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

            return fixture;
        }
    }
}