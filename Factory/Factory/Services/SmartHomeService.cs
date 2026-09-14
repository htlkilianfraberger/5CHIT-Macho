using Factory.Factories;
using Factory.Models;

namespace Factory.Services;

public class SmartHomeService
{
    private readonly Dictionary<string, SmartDeviceCreator> _creators;
    private readonly List<ISmartDevice> _devices = [];

    public SmartHomeService(IEnumerable<SmartDeviceCreator> creators)
    {
        _creators = creators.ToDictionary(creator => creator.DisplayName);
        LastDemoInfo = new FactoryMethodDemoInfo(
            "-",
            "-",
            nameof(SmartDeviceCreator.CreateDevice) + "()",
            "-",
            "-",
            nameof(ISmartDevice));
    }

    public IReadOnlyList<ISmartDevice> Devices => _devices;
    public IReadOnlyCollection<string> DeviceTypes => _creators.Keys;
    public FactoryMethodDemoInfo LastDemoInfo { get; private set; }

    public ISmartDevice AddDevice(string deviceType, string name)
    {
        if (!_creators.TryGetValue(deviceType, out SmartDeviceCreator? creator))
        {
            throw new ArgumentException($"Unknown device type: {deviceType}", nameof(deviceType));
        }

        ISmartDevice device = creator.CreateAndInitialize(name);
        _devices.Add(device);

        LastDemoInfo = new FactoryMethodDemoInfo(
            deviceType,
            creator.GetType().Name,
            nameof(SmartDeviceCreator.CreateDevice) + "()",
            GetFactoryMethodImplementation(creator),
            creator.ProductName,
            nameof(ISmartDevice));

        return device;
    }

    public void RemoveDevice(Guid id)
    {
        ISmartDevice? device = _devices.FirstOrDefault(device => device.Id == id);

        if (device is not null)
        {
            _devices.Remove(device);
        }
    }

    public IReadOnlyList<ISmartDevice> GetDevices() => Devices;

    private static string GetFactoryMethodImplementation(SmartDeviceCreator creator)
    {
        Type creatorType = creator.GetType();
        Type? declaringType = creatorType.GetMethod(nameof(SmartDeviceCreator.CreateDevice))?.DeclaringType;

        return declaringType == typeof(SmartDeviceCreator)
            ? "Inherited from SmartDeviceCreator"
            : $"Overridden in {creatorType.Name}";
    }
}
