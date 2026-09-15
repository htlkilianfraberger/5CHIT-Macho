using Factory.Models;

namespace Factory.Factories;

public class SmartFanCreator : SmartDeviceCreator
{
    private const int DefaultSpeed = 2;
    private const bool DefaultOscillation = true;
    private const string DefaultMode = "Auto";

    public override string DisplayName => "Smart Fan";
    public override string ProductName => nameof(SmartFan);
    public override string CreationDetails => $"Sets speed to {DefaultSpeed}, oscillation to on, and mode to {DefaultMode}.";

    // Overrides the Factory Method because this creator
    // needs a specialized concrete product with fan-specific defaults.
    public override ISmartDevice CreateDevice(string name)
    {
        return new SmartFan(name, DefaultSpeed, DefaultOscillation, DefaultMode);
    }
}
