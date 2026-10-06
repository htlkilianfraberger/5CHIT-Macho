namespace Factory_Pattern.Products;

public sealed class LoxoneSmartLight : ISmartLight
{
    public LoxoneSmartLight(string name, int brightness)
    {
        Id = Guid.NewGuid();
        Name = name;
        Brightness = Math.Clamp(brightness, 0, 100);
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Vendor => "Loxone";
    public string DeviceType => "SmartLight";
    public bool IsOn { get; private set; }
    public int Brightness { get; }

    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;
}
