namespace Factory.Models;

public class Thermostat : ISmartDevice
{
    public Thermostat(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        CurrentTemperature = 21.4m;
        TargetTemperature = 22.0m;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string DeviceType => "Thermostat";
    public bool IsOn { get; private set; }
    public decimal CurrentTemperature { get; }
    public decimal TargetTemperature { get; private set; }

    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;

    public void IncreaseTargetTemperature() => TargetTemperature += 0.5m;
    public void DecreaseTargetTemperature() => TargetTemperature -= 0.5m;
}
