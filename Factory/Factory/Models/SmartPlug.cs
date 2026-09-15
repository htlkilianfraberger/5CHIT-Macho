namespace Factory.Models;

public class SmartPlug : ISmartDevice
{
    private static readonly Random Random = new();

    public SmartPlug(string name, int safetyLimitWatts, int standbyThresholdWatts)
    {
        Id = Guid.NewGuid();
        Name = name;
        SafetyLimitWatts = safetyLimitWatts;
        StandbyThresholdWatts = standbyThresholdWatts;
        PowerConsumption = Random.Next(45, 260);
    }

    public Guid Id { get; }
    public string Name { get; }
    public string DeviceType => "Smart Plug";
    public bool IsOn { get; private set; }
    public int PowerConsumption { get; private set; }
    public int SafetyLimitWatts { get; }
    public int StandbyThresholdWatts { get; }

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
