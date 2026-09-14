using Factory.Models;

namespace Factory.Factories;

public class ThermostatCreator : SmartDeviceCreator
{
    public override string DisplayName => "Thermostat";
    public override string ProductName => nameof(Thermostat);

    // Overrides the Factory Method because this creator
    // needs a specialized concrete product.
    public override ISmartDevice CreateDevice(string name)
    {
        return new Thermostat(name);
    }
}
