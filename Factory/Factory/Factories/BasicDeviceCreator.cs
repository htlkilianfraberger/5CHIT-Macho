using Factory.Models;

namespace Factory.Factories;

public class BasicDeviceCreator : SmartDeviceCreator
{
    public override string DisplayName => "Basic Device";
    public override string ProductName => nameof(GenericSmartDevice);
    public override string CreationDetails => "Uses the inherited default factory method and creates a plain GenericSmartDevice.";

    // Uses the inherited Factory Method.
    // No override is necessary because the default product is sufficient.
}
