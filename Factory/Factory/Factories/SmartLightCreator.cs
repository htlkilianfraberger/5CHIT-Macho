using Factory.Models;

namespace Factory.Factories;

public class SmartLightCreator : SmartDeviceCreator
{
    public override string DisplayName => "Smart Light";
    public override string ProductName => nameof(SmartLight);

    // Overrides the Factory Method because this creator
    // needs a specialized concrete product.
    public override ISmartDevice CreateDevice(string name)
    {
        return new SmartLight(name);
    }
}
