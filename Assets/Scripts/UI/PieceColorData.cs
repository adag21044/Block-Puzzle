using UnityEngine;

public class PieceColorData
{
    public static readonly Color[] Colors = new Color[]
    {
        HexToColor("#CC0000"), // Red
        HexToColor("#0099FF"), // Blue
        HexToColor("#00CC00"), // Green
        HexToColor("#660099"), // Purple
        HexToColor("#CC33CC"), // Pink
        HexToColor("#33CCCC"), // Turquoise
        HexToColor("#FFCC00"), // Yellow
        HexToColor("#FF6600"), // Orange
        HexToColor("#0000CC"), // Navy
        HexToColor("#FF6666"), // Salmon
    };

    private static Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}