using PropertyValidator.Models;
using PropertyValidator.ValidationPack;
using Test.Maui.Models;

namespace Test.Maui.Rules;

internal class AddressRule : MultiValidationRule<Address>
{
    protected override IRuleCollection<Address> ConfigureRules(IRuleCollection<Address> ruleCollection)
    {
        return ruleCollection
            .AddRule(e => e.City, new StringRequiredRule())
            .AddRule(e => e.CountryIsoCode, new CountryIsoCodeRule())
            .AddRule(e => e.PostalCode, new PostalCodeRule())
            .AddRule(e => e.StreetAddress, new StringRequiredRule(), new MaxLengthRule(100));
    }
}
