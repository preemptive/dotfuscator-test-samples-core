namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

public enum Category
{
    Food,
    Transport,
    Utilities,
    Entertainment,
    Health,
    Other
}

public static class CategoryHelper
{
    public static Category[] Values => Enum.GetValues<Category>();
}