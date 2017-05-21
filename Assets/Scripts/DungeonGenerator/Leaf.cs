using UnityEngine;
using System.Collections.Generic;
public class Leaf
{
    int Min_Size;
    int HallsWidth = 2;

    public int X, Y, Width, Height;

    public Leaf LeftChild;
    public Leaf RightChild;
    public Rectangle Room;
    public List<Rectangle> halls;
    System.Random rnd;

    public Leaf(int X, int Y, int Width, int Height, int minLeafSize, System.Random rand)
    {
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;
        Min_Size = (minLeafSize > 8) ? minLeafSize : 8;
        rnd = rand;
    }

    public bool Split()
    {

        if (RightChild != null || LeftChild != null)
            return false;

        bool SplitH = rnd.NextDouble() > 0.5;
        if (Width > Height && Width / Height >= 1.25)
            SplitH = false;
        else if (Height > Width && Height / Width >= 1.25)
            SplitH = true;

        int max = (SplitH ? Height : Width) - Min_Size;

        if (max <= Min_Size)
            return false;

        int split = rnd.Next(Min_Size, max);

        if (SplitH)
        {
            LeftChild = new Leaf(X, Y, Width, split, Min_Size, rnd);
            RightChild = new Leaf(X, Y + split, Width, Height - split, Min_Size, rnd);
        }
        else
        {
            LeftChild = new Leaf(X, Y, split, Height, Min_Size, rnd);
            RightChild = new Leaf(X + split, Y, Width - split, Height, Min_Size, rnd);
        }
        return true;

    }

    public void CreateRooms()
    {
        if (LeftChild != null || RightChild != null)
        {
            if (LeftChild != null)
            {
                LeftChild.CreateRooms();
            }
            if (RightChild != null)
            {
                RightChild.CreateRooms();
            }
            if (RightChild != null && LeftChild != null)
            {
                CreateHall(LeftChild.GetRoom(), RightChild.GetRoom());
            }
        }
        else
        {
            Point RoomSize = new Point(rnd.Next(5, Width - 3), rnd.Next(5, Height - 3));
            Point RoomPos = new Point(rnd.Next(1, Width - RoomSize.x - 1), rnd.Next(1, Height - RoomSize.y - 1));
            Room = new Rectangle(X + RoomPos.x, Y + RoomPos.y, RoomSize.x, RoomSize.y);
        }
    }

    public Rectangle GetRoom()
    {
        if (Room != null)
        {
            return Room;
        }
        else
        {
            Rectangle lRoom = null;
            Rectangle rRoom = null;
            if (LeftChild != null)
            {
                lRoom = LeftChild.GetRoom();
            }
            if (RightChild != null)
            {
                rRoom = RightChild.GetRoom();
            }
            if (lRoom == null && rRoom == null)
            {
                return null;
            }
            else if (rRoom == null)
            {
                return lRoom;
            }
            else if (lRoom == null)
            {
                return rRoom;
            }
            else if (rnd.NextDouble() > 0.5)
            {
                return lRoom;
            }
            else
            {
                return rRoom;
            }
        }
    }

    public void CreateHall(Rectangle L, Rectangle R)
    {
        halls = new List<Rectangle>();
        Point p1 = new Point(rnd.Next(L.left + 1, L.right - 2), rnd.Next(L.top + 1, L.bottom - 2));
        Point p2 = new Point(rnd.Next(R.left + 1, R.right - 2), rnd.Next(R.top + 1, R.bottom - 2));

        int w = p2.x - p1.x;
        int h = p2.y - p1.y;

        if (w < 0)
        {
            if (h < 0)
            {
                if (rnd.NextDouble() < 0.5)
                {
                    halls.Add(new Rectangle(p2.x, p1.y, Mathf.Abs(w), HallsWidth));
                    halls.Add(new Rectangle(p2.x, p2.y, HallsWidth, Mathf.Abs(h)));
                }
                else
                {
                    halls.Add(new Rectangle(p2.x, p2.y, Mathf.Abs(w), HallsWidth));
                    halls.Add(new Rectangle(p1.x, p2.y, HallsWidth, Mathf.Abs(h)));
                }
            }
            else if (h > 0)
            {
                if (rnd.NextDouble() < 0.5)
                {
                    halls.Add(new Rectangle(p2.x, p1.y, Mathf.Abs(w), HallsWidth));
                    halls.Add(new Rectangle(p2.x, p1.y, HallsWidth, Mathf.Abs(h)));
                }
                else
                {
                    halls.Add(new Rectangle(p2.x, p2.y, Mathf.Abs(w), HallsWidth));
                    halls.Add(new Rectangle(p1.x, p1.y, HallsWidth, Mathf.Abs(h) + HallsWidth));
                }
            }
            else
            {
                halls.Add(new Rectangle(p2.x, p2.y, Mathf.Abs(w), HallsWidth));
            }
        }
        else if (w > 0)
        {
            if (h < 0)
            {
                if (rnd.NextDouble() < 0.5)
                {
                    halls.Add(new Rectangle(p1.x, p2.y, Mathf.Abs(w), HallsWidth));
                    halls.Add(new Rectangle(p1.x, p2.y, HallsWidth, Mathf.Abs(h)));
                }
                else
                {
                    halls.Add(new Rectangle(p1.x, p1.y, Mathf.Abs(w), HallsWidth));
                    halls.Add(new Rectangle(p2.x, p2.y, HallsWidth, Mathf.Abs(h) + HallsWidth));
                }
            }
            else if (h > 0)
            {
                if (rnd.NextDouble() < 0.5)
                {
                    halls.Add(new Rectangle(p1.x, p1.y, Mathf.Abs(w), HallsWidth));
                    halls.Add(new Rectangle(p2.x, p1.y, HallsWidth, Mathf.Abs(h)));
                }
                else
                {
                    halls.Add(new Rectangle(p1.x, p2.y, Mathf.Abs(w), HallsWidth));
                    halls.Add(new Rectangle(p1.x, p1.y, HallsWidth, Mathf.Abs(h)));
                }
            }
            else
            {
                halls.Add(new Rectangle(p1.x, p1.y, Mathf.Abs(w), HallsWidth));
            }
        }
        else
        {
            if (h < 0)
            {
                halls.Add(new Rectangle(p2.x, p2.y, HallsWidth, Mathf.Abs(h)));
            }
            else if (h > 0)
            {
                halls.Add(new Rectangle(p1.x, p1.y, HallsWidth, Mathf.Abs(h)));
            }
        }
    }
}
