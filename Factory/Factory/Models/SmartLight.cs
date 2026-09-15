namespace Factory.Models;

public class SmartLight : ISmartDevice
{
    public SmartLight(string name, int brightness, string colorTemperature)
    {
        Id = Guid.NewGuid();
        Name = name;
        Brightness = brightness;
        ColorTemperature = colorTemperature;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string DeviceType => "Smart Light";
    public bool IsOn { get; private set; }
    public int Brightness { get; set; }
    public string ColorTemperature { get; }

    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;
}
