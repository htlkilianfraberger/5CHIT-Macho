namespace Factory.Models;

public class GenericSmartDevice : ISmartDevice
{
    public GenericSmartDevice(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string DeviceType => "Generic Smart Device";
    public bool IsOn { get; private set; }

    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;
}
