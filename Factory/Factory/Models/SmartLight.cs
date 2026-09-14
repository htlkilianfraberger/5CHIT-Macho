namespace Factory.Models;

public class SmartLight : ISmartDevice
{
    public SmartLight(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Brightness = 80;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string DeviceType => "Smart Light";
    public bool IsOn { get; private set; }
    public int Brightness { get; set; }

    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;
}
