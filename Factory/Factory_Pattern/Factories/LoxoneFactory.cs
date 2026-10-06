using Factory_Pattern.Products;

namespace Factory_Pattern.Factories;

public sealed class LoxoneFactory : ISmartHomeFactory
{
    public string Vendor => "Loxone";

    public ISmartLight CreateSmartLight(string name)
    {
        return new LoxoneSmartLight(name, 45);
    }

    public IRgbSmartLight CreateRgbSmartLight(string name)
    {
        return new LoxoneRgbSmartLight(name, 255, 214, 170);
    }
}
