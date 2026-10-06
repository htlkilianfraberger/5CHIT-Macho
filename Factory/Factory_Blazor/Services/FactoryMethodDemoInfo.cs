namespace Factory_Blazor.Services;

public record AbstractFactoryDemoInfo(
    string SelectedFactory,
    string AbstractFactory,
    string ConcreteFactory,
    string FactoryMethod,
    string CreatedProduct,
    string ProductFamily,
    string ReturnedAs);
