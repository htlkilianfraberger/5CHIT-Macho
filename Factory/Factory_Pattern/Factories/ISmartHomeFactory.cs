using Factory_Pattern.Products;

namespace Factory_Pattern.Factories;

public interface ISmartHomeFactory
{
    string Vendor { get; }
    ISmartLight CreateSmartLight(string name);
    IRgbSmartLight CreateRgbSmartLight(string name);
}
