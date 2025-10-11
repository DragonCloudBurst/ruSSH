// using Spectre.Console;

// public class Monster
// {
//     public Player player;

//     public Tile[,] map;

//     public int X;
//     public int Y;
//     public bool isValidGen;

//     public void Move(int dx, int dy)
//     {

//         var pathToPlayer = MakePath(X, Y);

//         if (pathToPlayer != null)
//         {
//             X += dx;
//             Y += dy;
//         }
        
//     }

//     public AStarSearch MakePath(int x, int y)
//     {
//         Pair current = new Pair(x, y);
//         Pair destination = new Pair(player.X, player.Y);


//         if (AStarSearch.IsUnBlocked(map, X, Y))
//         {
//             isValidGen = true;
//         }

//         return AStarSearch.Path(map, current, destination);
//     }
    
//     public void CliTest()
//     {
//         Random randomWidth = new Random();
//         Random randomHeight = new Random();

//         var height = Console.WindowHeight;
//         var width = Console.WindowWidth;

//         bool isMonsterGen = false;


//         Monster testMonster = new Monster();

//         while (!isMonsterGen)
//         {
//             int randomX = randomWidth.Next(1, width);
//             int randomY = randomHeight.Next(1, height);

//             testMonster = new Monster();
//             var tryPath = MakePath(randomX, randomY);

//             if (testMonster.isValidGen)
//             {
//                 isMonsterGen = true;
//                 break;
//             }

//         }

//         testMonster.Move(1,1);

//     }
    
// }

