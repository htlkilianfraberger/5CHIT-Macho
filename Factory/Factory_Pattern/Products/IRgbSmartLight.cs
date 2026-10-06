namespace Factory_Pattern.Products;

public interface IRgbSmartLight : ISmartLight
{
    int Red { get; }
    int Green { get; }
    int Blue { get; }

    void SetColor(int red, int green, int blue);
}
