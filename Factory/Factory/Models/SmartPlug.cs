namespace Factory.Models;

public class SmartPlug : ISmartDevice
{
    private static readonly Random Random = new();

    public SmartPlug(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        PowerConsumption = Random.Next(45, 260);
    }

    public Guid Id { get; }
    public string Name { get; }
    public string DeviceType => "Smart Plug";
    public bool IsOn { get; private set; }
    public int PowerConsumption { get; private set; }

    public void TurnOn()
    {
        IsOn = true;
        PowerConsumption = Random.Next(45, 260);
    }

    public void TurnOff()
    {
        IsOn = false;
        PowerConsumption = 0;
    }
}
