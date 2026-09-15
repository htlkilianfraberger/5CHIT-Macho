using Factory.Models;

namespace Factory.Factories;

public class SmartLightCreator : SmartDeviceCreator
{
    private const int DefaultBrightness = 75;
    private const string DefaultColorTemperature = "Warm white";

    public override string DisplayName => "Smart Light";
    public override string ProductName => nameof(SmartLight);
    public override string CreationDetails => $"Sets brightness to {DefaultBrightness}% and color temperature to {DefaultColorTemperature}.";

    // Overrides the Factory Method because this creator
    // needs a specialized concrete product with light-specific defaults.
    public override ISmartDevice CreateDevice(string name)
    {
        return new SmartLight(name, DefaultBrightness, DefaultColorTemperature);
    }
}
