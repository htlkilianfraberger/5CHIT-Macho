using Factory.Models;

namespace Factory.Factories;

public class SmartFanCreator : SmartDeviceCreator
{
    public override string DisplayName => "Smart Fan";
    public override string ProductName => nameof(SmartFan);

    // Overrides the Factory Method because this creator
    // needs a specialized concrete product.
    public override ISmartDevice CreateDevice(string name)
    {
        return new SmartFan(name);
    }
}
