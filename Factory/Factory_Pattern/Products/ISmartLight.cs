namespace Factory_Pattern.Products;

public interface ISmartLight
{
    Guid Id { get; }
    string Name { get; }
    string Vendor { get; }
    string DeviceType { get; }
    bool IsOn { get; }
    int Brightness { get; }

    void TurnOn();
    void TurnOff();
}
