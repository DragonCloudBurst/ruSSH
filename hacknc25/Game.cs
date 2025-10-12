using System.Dynamic;
using Spectre.Console;

public class Player {
	public PlayerData Data { get; }
	public int X {get; set;}
	public int Y {get; set;}
	public int Floor {get; set;}

	public int Health { get; private set; }
	public int MaxHealth => Data.Stats.HP;
	public int Attack => Data.Stats.Att;
	public int Defense => Data.Stats.Def;

	public Player(PlayerData data, int x, int y, int floor) {
		Data = data;
		Health = data.Stats.HP;
		X = x;
		Y = y;
		Floor = floor;
	}

	public void Move(int dx, int dy) {
		X += dx;
		Y += dy;
	}

	public void TakeDamage(int dmg) {
		Health -= dmg;
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

	public string StatUI()
    {
		return $"{player.Data.Name} HP: {player.Health} ATK: {player.Attack} DEF: {player.Defense}";
    }

	public Game(PlayerData pdata) {
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
		if (!player_pos.HasValue)
		{
			throw new Exception("should not be here: could not find upstair on first floor");
		}

		player = new Player(pdata, player_pos.Value.Item1, player_pos.Value.Item2, 0);

		var bat_pos = MapFuncs.RandomFreeSquare(levels[0].Tiles);
		levels[0].Actors.Add(MonsterFactory.NewBat(bat_pos.Item1, bat_pos.Item2));

		var goblin_pos = MapFuncs.RandomFreeSquare(levels[0].Tiles);
		levels[0].Actors.Add(MonsterFactory.NewGoblin(goblin_pos.Item1, goblin_pos.Item2));

		for (int i = 0; i < 5; i++)
        {
			var itemFact = new ItemFactory();
			var itemMade = itemFact.NewItem();
			var item_pos = MapFuncs.RandomFreeSquare(levels[0].Tiles);
			itemMade.X = item_pos.Item1;
			itemMade.Y = item_pos.Item2;
			levels[0].Items.Add(itemMade);
        }

	}

	public void Run() {
		Console.CursorVisible = false;

		// if you uncomment this it will not show exceptions and crash messages
		// AnsiConsole.AlternateScreen(() => {
        	AnsiConsole.Live(gui.Layout)
            .Start(ctx => {
                while (running) {
                    Render();
                    ctx.Refresh();  // This triggers the efficient update
                    var key = Console.ReadKey(intercept: true);
                    Update(key);
                }
            });
    	// });
	
		// clean up
		Console.CursorVisible = true;
		if (player.Health <= 0) {
			AnsiConsole.MarkupLine("[red]You died![/]");
		}
		AnsiConsole.MarkupLine("[green]Thanks for playing![/]");
	}

	public void Render() {
		var output = new System.Text.StringBuilder();

		var map = levels[player.Floor].Tiles;

		int itemCounter = 0;

		for (int y = 0; y < WindowHeight; y++) {
			for (int x = 0; x < WindowWidth; x++) {

				var actor = levels[player.Floor].ActorAt(x, y);
				var items = levels[0].Items;
				if (player.X == x && player.Y == y)
				{
					var raceColor = player.Data.Race switch
					{
						Race.Human => "yellow",
						Race.Elf => "magenta",
						Race.Orc => "green",
						Race.Dwarf => "red",
						_ => "white"
					};
					output.Append($"[{raceColor}]@[/]");
				}
				else if (actor != null)
				{
					output.Append(actor.Symbol);
				}
				else if (levels[0].Items[itemCounter].X == x &&
				levels[0].Items[itemCounter].Y == y)
				{
					output.Append(items[itemCounter].marker);
					itemCounter++;
				}
				else
				{
					output.Append(map[x, y].Symbol.ToString());
				}
				
			}
			if (y != WindowHeight-1) {
				output.AppendLine();
			}
		}

		var stats = StatUI();


		gui.Render(output.ToString(), Messages.String(), stats);
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
			var actor = levels[player.Floor].ActorAt(px+dx, py+dy);
			if (actor == null) {
				player.Move(dx, dy);
			} else if (actor.IsDead()) {
				player.Move(dx, dy);
				Messages.AddMessage("You step over the dead " + actor.Name + ".");
			} else {
				actor.TakeDamage(3);
				Messages.AddMessage("You hit the " + actor.Name + " for 3 damage.");
				if (actor.IsDead()) {
					Messages.AddMessage("You kill the " + actor.Name + "!");
				}
			}
			
		}

		updateActors:
		foreach (var actor in levels[player.Floor].Actors) {
			var (mx, my) = actor.Act(new MapSeeingObject(levels[player.Floor], player, Messages));
			actor.Move(mx, my);
		}

		if (player.Health <= 0) {
			running = false;
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
