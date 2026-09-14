namespace Factory.Models;

public class SmartFan : ISmartDevice
{
    public SmartFan(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Speed = 2;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string DeviceType => "Smart Fan";
    public bool IsOn { get; private set; }
    public int Speed { get; private set; }

    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;

    public void SetSpeed(int speed)
    {
        if (speed is < 1 or > 3)
        {
            return;
        }

        Speed = speed;
    }
}
