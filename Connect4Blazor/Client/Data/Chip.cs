namespace Connect4Blazor.Client.Data
{
    /// <summary>
    /// The chip class
    /// </summary>
    public class Chip
    {
        /// <summary>
        /// Default color is white
        /// </summary>
        public static string DefaultColor = "white";

        /// <summary>
        /// The row of the chip
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// The column of the chip
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// The color of the chip
        /// </summary>
        public string Color { get; set; } = DefaultColor;
    }
}
