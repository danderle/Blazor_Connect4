namespace Connect4Blazor.Client.Data
{
    public class Chip
    {
        public static string DefaultColor = "white";
        public int Row { get; set; }

        public int Column { get; set; }

        public string Color { get; set; } = DefaultColor;
    }
}
