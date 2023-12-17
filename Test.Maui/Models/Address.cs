using PropertyChanged;

namespace Test.Maui.Models
{
    [AddINotifyPropertyChangedInterface]
    internal class Address
    {
        public int PostalCode { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? CountryIsoCode { get; set; }
    }
}
