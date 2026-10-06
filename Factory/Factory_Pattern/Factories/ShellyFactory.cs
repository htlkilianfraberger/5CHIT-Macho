using Factory_Pattern.Products;

namespace Factory_Pattern.Factories;

public sealed class ShellyFactory : ISmartHomeFactory
{
    public string Vendor => "Shelly";

    public ISmartLight CreateSmartLight(string name)
    {
        return new ShellySmartLight(name, 100);
    }

    public IRgbSmartLight CreateRgbSmartLight(string name)
    {
        return new ShellyRgbSmartLight(name, 0, 90, 255);
    }
}
