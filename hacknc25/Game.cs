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

	public GameUI gui;
	
	public int WindowWidth {get;}
	public int WindowHeight {get;}

	private bool running = true;

	private List<Level> levels;
	private Tile[][,] maps;

	public Messages Messages;

	public Game() {
		WindowHeight = Console.WindowHeight-10;
		WindowWidth = Console.WindowWidth-6;

		Messages = new Messages();
		
		gui = new GameUI(WindowWidth+4);

		levels = new List<Level>();
		var gen = new RogueDungeonGenerator();
		for (int i = 0; i < 10; i++) {
			var tiles = gen.QuickNewDungeon(WindowWidth, WindowHeight, 4);
			levels.Add(new Level(tiles));
		}

		var player_pos = MapFuncs.FindTileOfType(TileType.UpStair, levels[0].Tiles);
		if (!player_pos.HasValue) {
			throw new Exception("should not be here: could not find upstair on first floor");
		}
		player = new Player(player_pos.Value.Item1, player_pos.Value.Item2, 0);

		var bat_pos = MapFuncs.RandomFreeSquare(levels[0].Tiles);
		var bat = new Actor(bat_pos.Item1, bat_pos.Item2, "Bat", 'b', new WanderingAI());
		levels[0].Actors.Add(bat);
	}

	public void Run() {
		Console.CursorVisible = false;

		AnsiConsole.AlternateScreen(() => {
        	AnsiConsole.Live(gui.Layout)
            .Start(ctx => {
                while (running) {
                    Render();
                    ctx.Refresh();  // This triggers the efficient update
                    var key = Console.ReadKey(intercept: true);
                    Update(key);
                }
            });
    	});
	
		// clean up
		Console.CursorVisible = true;
		AnsiConsole.MarkupLine("[green]Thanks for playing![/]");
	}

	public void Render() {
		var output = new System.Text.StringBuilder();

		var map = levels[player.Floor].Tiles;
		
		for (int y = 0; y < WindowHeight; y++) {
			for (int x = 0; x < WindowWidth; x++) {

				var actor = levels[player.Floor].ActorAt(x, y);
				if (player.X == x && player.Y == y) {
					output.Append("@");
				} else if (actor != null) {
					output.Append(actor.Symbol);
				} else {
					output.Append(map[x, y].Symbol.ToString());
				}
			}
			if (y != WindowHeight-1) {
				output.AppendLine();
			}
		}

		

		gui.Render(output.ToString(), Messages.String());
		// Console.SetCursorPosition(0, 0);
		// Console.Write(output.ToString());
	}

	public void Update(ConsoleKeyInfo key) {

		if (key.Key == ConsoleKey.Oem1) {
			goto updateActors;
		}
		
		if (key.Key == ConsoleKey.Escape) {
			running = false;
			return;
		}

		if (key.Key == ConsoleKey.OemComma) {
			if (player.Floor > 0
				&& levels[player.Floor].Tiles[player.X, player.Y].Type == TileType.UpStair) {
				player.Floor--;
				var downstair = MapFuncs.FindTileOfType(TileType.DownStair, levels[player.Floor].Tiles);
				if (!downstair.HasValue) {
					throw new Exception("should not be here: could not find downstair in level to go up");
				}
				player.X = downstair.Value.Item1;
				player.Y = downstair.Value.Item2;
			}
			goto updateActors;
		}

		if (key.Key == ConsoleKey.OemPeriod) {
			if (player.Floor < 9
				&& levels[player.Floor].Tiles[player.X, player.Y].Type == TileType.DownStair) {
				player.Floor++;
				var upstair = MapFuncs.FindTileOfType(TileType.UpStair, levels[player.Floor].Tiles);
				if (!upstair.HasValue) {
					throw new Exception("should not be here: could not find upstair in next level to go down");
				}
				player.X = upstair.Value.Item1;
				player.Y = upstair.Value.Item2;
			}
			goto updateActors;
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

		var map = levels[player.Floor].Tiles;

		if (map[px+dx, py+dy].Type == TileType.Wall) {
			Messages.AddMessage("You bump into the wall!");
			goto updateActors;
		}

		if ((px + dx >= 0 && px + dx < WindowWidth)
			&& (py + dy >= 0 && py + dy < WindowHeight)) {
			player.Move(dx, dy);
		}

		updateActors:
		foreach (var actor in levels[player.Floor].Actors) {
			var (mx, my) = actor.Act(new MapSeeingObject(levels[player.Floor].Tiles));
			actor.Move(mx, my);
		}
	}


}

public class Messages {
	public List<string> messages;

	public Messages() {
		messages = new List<string>();
		AddMessage("Welcome to the dungeon!");
	}

	public void AddMessage(string message) {
		messages.Add(message);
		if (messages.Count > 5) {
			messages.RemoveAt(0);
		}
	}

	public string String() {
		var output = new System.Text.StringBuilder();
		for (int i = messages.Count-1; i >= 0; i--) {
			output.Append(messages[i]);
			if (i != 0) {
				output.Append("\n");
			}
		}
		return output.ToString();
	}
}
