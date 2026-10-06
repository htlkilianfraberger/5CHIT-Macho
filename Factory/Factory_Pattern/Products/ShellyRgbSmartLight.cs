namespace Factory_Pattern.Products;

public sealed class ShellyRgbSmartLight : IRgbSmartLight
{
    public ShellyRgbSmartLight(string name, int red, int green, int blue)
    {
        Id = Guid.NewGuid();
        Name = name;
        SetColor(red, green, blue);
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Vendor => "Shelly";
    public string DeviceType => "RgbSmartLight";
    public bool IsOn { get; private set; }
    public int Brightness => 100;
    public int Red { get; private set; } = 255;
    public int Green { get; private set; } = 255;
    public int Blue { get; private set; } = 255;

    public void TurnOn() => IsOn = true;
    public void TurnOff() => IsOn = false;

    public void SetColor(int red, int green, int blue)
    {
        Red = Math.Clamp(red, 0, 255);
        Green = Math.Clamp(green, 0, 255);
        Blue = Math.Clamp(blue, 0, 255);
    }
}
