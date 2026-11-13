namespace Factory_Pattern;

using Factory_Pattern.Factories;
//Autor: Kilian Fraberger
// Factory Method Pattern mit SmartLight und RGBSmartLight von Shelly und Loxone
public class Program
{
    public static void Main()
    {
        ISmartHomeFactory factory = new LoxoneFactory();
        var light = factory.CreateSmartLight("Wohnzimmer");

        light.TurnOn();
        Console.WriteLine($"{factory.Vendor}-Light '{light.Name}' erstellt: {light.IsOn}");
    }
}