using Spectre.Console;

public class Player {
	public int X {get; set;}
	public int Y {get; set;}
	public int Floor {get; set;}

	public Player(int x, int y, int floor) {
		X = x;
		Y = y;
		Floor = floor;
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

	private Tile[][,] maps;

	public Game() {
		WindowHeight = Console.WindowHeight;
		WindowWidth = Console.WindowWidth;

		maps = new Tile[10][,];
		var gen = new RogueDungeonGenerator();
		for (int i = 0; i < 10; i++) {
			maps[i] = gen.QuickNewDungeon(WindowWidth, WindowHeight, 4);
		}

		var player_pos = MapFuncs.FindTileOfType(TileType.UpStair, maps[0]);
		if (!player_pos.HasValue) {
			throw new Exception("should not be here: could not find upstair on first floor");
		}
		player = new Player(player_pos.Value.Item1, player_pos.Value.Item2, 0);
	}

	public void Run() {
		Console.CursorVisible = false;

		AnsiConsole.AlternateScreen(() => {
			while (running) {
				Render();
				var key = Console.ReadKey(intercept: true);
				Update(key);
			}
		});

		// clean up
		Console.CursorVisible = true;
		AnsiConsole.MarkupLine("[green]Thanks for playing![/]");
	}

	public void Render() {
		var output = new System.Text.StringBuilder();

		var map = maps[player.Floor];
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

		if (key.Key == ConsoleKey.OemComma) {
			if (player.Floor > 0
				&& maps[player.Floor][player.X, player.Y].Type == TileType.UpStair) {
				player.Floor--;
				var downstair = MapFuncs.FindTileOfType(TileType.DownStair, maps[player.Floor]);
				if (!downstair.HasValue) {
					throw new Exception("should not be here: could not find downstair in level to go up");
				}
				player.X = downstair.Value.Item1;
				player.Y = downstair.Value.Item2;
			}
			return;
		}

		if (key.Key == ConsoleKey.OemPeriod) {
			if (player.Floor < 9
				&& maps[player.Floor][player.X, player.Y].Type == TileType.DownStair) {
				player.Floor++;
				var upstair = MapFuncs.FindTileOfType(TileType.UpStair, maps[player.Floor]);
				if (!upstair.HasValue) {
					throw new Exception("should not be here: could not find upstair in next level to go down");
				}
				player.X = upstair.Value.Item1;
				player.Y = upstair.Value.Item2;
			}
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

		var map = maps[player.Floor];

		if (map[px+dx, py+dy].Type == TileType.Wall) {
			return;
		}

		if ((px + dx >= 0 && px + dx < WindowWidth)
			&& (py + dy >= 0 && py + dy < WindowHeight)) {
			player.Move(dx, dy);
		}
	}


}
