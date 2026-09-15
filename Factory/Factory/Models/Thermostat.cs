namespace Factory.Models;

public class Thermostat : ISmartDevice
{
    public Thermostat(string name, decimal currentTemperature, decimal targetTemperature, string mode)
    {
        Id = Guid.NewGuid();
        Name = name;
        CurrentTemperature = currentTemperature;
        TargetTemperature = targetTemperature;
        Mode = mode;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string DeviceType => "Thermostat";
    public bool IsOn { get; private set; }
    public decimal CurrentTemperature { get; }
    public decimal TargetTemperature { get; private set; }
    public string Mode { get; }

    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;

    public void IncreaseTargetTemperature() => TargetTemperature += 0.5m;
    public void DecreaseTargetTemperature() => TargetTemperature -= 0.5m;
}
