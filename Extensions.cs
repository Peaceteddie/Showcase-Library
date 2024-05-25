using System.Text.Json;

public static class Extensions
{
    public static T Inspect<T>(this T item)
    {
        try
        {
            var json = JsonSerializer.Serialize(item);
            Console.WriteLine(json);
        }
        catch
        {
            Console.WriteLine($"Not serializable: {item}");
        }
        return item;
    }
}

