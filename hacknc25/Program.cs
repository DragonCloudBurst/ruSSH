using System;

class Program
{
    static void Main(String[] args)
    {
        var pdata = Menu.ShowMenu();
        var game = new Game(pdata);
        game.Run();
    }
}
