namespace Factory.Models;

public interface ISmartDevice
{
    Guid Id { get; }
    string Name { get; }
    string DeviceType { get; }
    bool IsOn { get; }

    void TurnOn();
    void TurnOff();
}
