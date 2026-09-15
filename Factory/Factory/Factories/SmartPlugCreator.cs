using Factory.Models;

namespace Factory.Factories;

public class SmartPlugCreator : SmartDeviceCreator
{
    private const int SafetyLimitWatts = 2400;
    private const int StandbyThresholdWatts = 5;

    public override string DisplayName => "Smart Plug";
    public override string ProductName => nameof(SmartPlug);
    public override string CreationDetails => $"Applies a {SafetyLimitWatts} W safety limit and {StandbyThresholdWatts} W standby threshold.";

    // Overrides the Factory Method because this creator
    // needs a specialized concrete product with plug-specific defaults.
    public override ISmartDevice CreateDevice(string name)
    {
        return new SmartPlug(name, SafetyLimitWatts, StandbyThresholdWatts);
    }
}
