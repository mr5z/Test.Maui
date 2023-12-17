using PropertyValidator.Models;

namespace Test.Maui.Rules;

internal class PostalCodeRule : ValidationRule<int>
{
    public override string ErrorMessage => "Code is wrong mate";

    public override bool IsValid(int value)
    {
        return value >= 1000 && value < 10_000;
    }
}
