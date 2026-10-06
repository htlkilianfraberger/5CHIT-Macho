using Factory_Pattern.Factories;
using Factory_Pattern.Products;

namespace Factory_Blazor.Services;

public class SmartHomeService
{
    private readonly Dictionary<string, ISmartHomeFactory> _factories;
    private readonly List<ISmartLight> _devices = [];

    public SmartHomeService(IEnumerable<ISmartHomeFactory> factories)
    {
        _factories = factories.ToDictionary(factory => factory.Vendor);
        LastDemoInfo = new AbstractFactoryDemoInfo(
            "-",
            nameof(ISmartHomeFactory),
            "-",
            "-",
            "-",
            "SmartLight / RgbSmartLight",
            nameof(ISmartLight));
    }

    public IReadOnlyList<ISmartLight> Devices => _devices;
    public IReadOnlyCollection<string> FactoryNames => _factories.Keys;
    public IReadOnlyList<string> ProductTypes { get; } = ["SmartLight", "RgbSmartLight"];
    public AbstractFactoryDemoInfo LastDemoInfo { get; private set; }

    public ISmartLight AddDevice(string factoryName, string productType, string name)
    {
        if (!_factories.TryGetValue(factoryName, out ISmartHomeFactory? factory))
        {
            throw new ArgumentException($"Unknown factory: {factoryName}", nameof(factoryName));
        }

        ISmartLight device = productType switch
        {
            "SmartLight" => factory.CreateSmartLight(name),
            "RgbSmartLight" => factory.CreateRgbSmartLight(name),
            _ => throw new ArgumentException($"Unknown product type: {productType}", nameof(productType))
        };

        device.TurnOn();
        _devices.Add(device);

        LastDemoInfo = new AbstractFactoryDemoInfo(
            factory.Vendor,
            nameof(ISmartHomeFactory),
            factory.GetType().Name,
            productType == "RgbSmartLight"
                ? nameof(ISmartHomeFactory.CreateRgbSmartLight) + "()"
                : nameof(ISmartHomeFactory.CreateSmartLight) + "()",
            device.GetType().Name,
            productType,
            productType == "RgbSmartLight" ? nameof(IRgbSmartLight) : nameof(ISmartLight));

        return device;
    }

    public void RemoveDevice(Guid id)
    {
        ISmartLight? device = _devices.FirstOrDefault(device => device.Id == id);

        if (device is not null)
        {
            _devices.Remove(device);
        }
    }
}
