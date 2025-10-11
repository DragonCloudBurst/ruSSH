using System.Security.Cryptography.X509Certificates;

public class Monster
{
    public Player player;

    public int X;
    public int Y;
    

    public Monster(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Move(int dx, int dy)
    {
        X += dx;
        Y += dy;
    }

    public void MakePath(int x, int y)
    {
        Pair current = new Pair(x, y);
        Pair destination = new Pair(Player.X, Player.Y)
    }
    


}