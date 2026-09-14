using Factory.Models;

namespace Factory.Factories;

public class SmartPlugCreator : SmartDeviceCreator
{
    public override string DisplayName => "Smart Plug";
    public override string ProductName => nameof(SmartPlug);

    // Overrides the Factory Method because this creator
    // needs a specialized concrete product.
    public override ISmartDevice CreateDevice(string name)
    {
        return new SmartPlug(name);
    }
}
