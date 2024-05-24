using System.Text.Json;

public static class Extensions
{
    public static T Inspect<T>(this T item)
    {
        var json = JsonSerializer.Serialize(item);
        Console.WriteLine(json);
        return item;
    }
}

