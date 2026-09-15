namespace Factory.Services;

public record FactoryMethodDemoInfo(
    string SelectedType,
    string SelectedCreator,
    string FactoryMethod,
    string Implementation,
    string CreatedProduct,
    string CreationDetails,
    string ReturnedAs);
