using Factory.Models;

namespace Factory.Factories;

public class ThermostatCreator : SmartDeviceCreator
{
    private const decimal SimulatedCurrentTemperature = 21.4m;
    private const decimal DefaultTargetTemperature = 22.0m;
    private const string DefaultMode = "Eco";

    public override string DisplayName => "Thermostat";
    public override string ProductName => nameof(Thermostat);
    public override string CreationDetails => $"Sets current temperature to {SimulatedCurrentTemperature:0.0} °C, target to {DefaultTargetTemperature:0.0} °C, mode to {DefaultMode}.";

    // Overrides the Factory Method because this creator
    // needs a specialized concrete product with thermostat-specific defaults.
    public override ISmartDevice CreateDevice(string name)
    {
        return new Thermostat(name, SimulatedCurrentTemperature, DefaultTargetTemperature, DefaultMode);
    }
}
