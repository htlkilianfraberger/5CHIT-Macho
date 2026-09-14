using Factory.Models;

namespace Factory.Factories;

public abstract class SmartDeviceCreator
{
    public abstract string DisplayName { get; }
    public virtual string ProductName => nameof(GenericSmartDevice);

    // FACTORY METHOD:
    // This default implementation creates a generic product.
    // Concrete creators may inherit it or override it for specialized products.
    public virtual ISmartDevice CreateDevice(string name)
    {
        return new GenericSmartDevice(name);
    }

    public ISmartDevice CreateAndInitialize(string name)
    {
        ISmartDevice device = CreateDevice(name);

        // The common workflow stays in the parent creator.
        // Only the object creation step varies through CreateDevice().
        device.TurnOn();

        return device;
    }
}
