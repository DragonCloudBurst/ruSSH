using Spectre.Console;

var game = new Game();
game.Run();

public class Player {
	public int X {get; set;}
	public int Y {get; set;}

	public Player(int x, int y) {
		X = x;
		Y = y;
	}

	public void Move(int dx, int dy) {
		X += dx;
		Y += dy;
	}
}

public class Game {
	public Player player;
	
	public int WindowWidth {get;}
	public int WindowHeight {get;}

	private bool running = true;

	private Tile[,] map;

	public Game() {
		WindowHeight = Console.WindowHeight;
		WindowWidth = Console.WindowWidth;
		player = new Player((WindowWidth / 2), (WindowHeight / 2));
		var gen = new RogueDungeonGenerator();
		map = gen.QuickNewDungeon(WindowWidth, WindowHeight, 3);
	}

	public void Run() {
		Console.CursorVisible = false;
		AnsiConsole.Clear();

		AnsiConsole.AlternateScreen(() => {
			while (running) {
				Render();
				var key = Console.ReadKey(intercept: true);
				Update(key);
			}
		});

		// clean up
		Console.CursorVisible = true;
		AnsiConsole.Clear();
		AnsiConsole.MarkupLine("[green]Thanks for playing![/]");
	}

	public void Render() {
		var output = new System.Text.StringBuilder();
		
		for (int y = 0; y < WindowHeight; y++) {
			for (int x = 0; x < WindowWidth; x++) {
				if (player.X == x && player.Y == y) {
					output.Append("@");
				} else {
					output.Append(map[x, y].Symbol.ToString());
				}
			}
			output.AppendLine();
		}


		Console.SetCursorPosition(0, 0);
		Console.Write(output.ToString());
	}

	public void Update(ConsoleKeyInfo key) {
		if (key.Key == ConsoleKey.Escape) {
			running = false;
			return;
		}

		var (dx, dy) = key.Key switch {
			ConsoleKey.W or ConsoleKey.UpArrow => (0, -1),
            ConsoleKey.S or ConsoleKey.DownArrow => (0, 1),
            ConsoleKey.A or ConsoleKey.LeftArrow => (-1, 0),
            ConsoleKey.D or ConsoleKey.RightArrow => (1, 0),
			ConsoleKey.Q => (-1, -1),
			ConsoleKey.E => (1, -1),
			ConsoleKey.Z => (-1, 1),
			ConsoleKey.C => (1, 1),
            _ => (0, 0)
		};

		var px = player.X;
		var py = player.Y;

		if (map[px+dx, py+dy].Type == TileType.Wall) {
			return;
		}

		if ((px + dx >= 0 && px + dx < WindowWidth)
			&& (py + dy >= 0 && py + dy < WindowHeight)) {
			player.Move(dx, dy);
		}
	}
}
