
public class Rectangle
{
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }

    public int top { get { return y; } }

    public int bottom { get { return y + height; } }

    public int left { get { return x; } }

    public int right { get { return x + width; } }

    public Rectangle()
    {
        x = 0;
        y = 0;
        width = 0;
        height = 0;
    }

    public Rectangle(int X, int Y, int Width, int Height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }
}
