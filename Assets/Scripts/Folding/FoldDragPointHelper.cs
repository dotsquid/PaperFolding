public class FoldDragPointBorder
{
    public float value;

    public static implicit operator float(FoldDragPointBorder border)
    {
        return border.value;
    }
}
