public class Level {
	public Tile[,] Tiles {get; set;}
	public List<Actor> Actors { get; set; }
	
	public List<Item> Items { get;  set; }

	public Level(Tile[,] tiles) {
		Tiles = tiles;
		Actors = new List<Actor>();
		Items = new List<Item>();
	}

	public Actor? ActorAt(int x, int y) {
		var actor = Actors.FirstOrDefault(a => a.X == x && a.Y == y);
		if (actor == null) {
			return null;
		}
		return actor;
	}
}

// this is for letting AI's see the world and make decisions
public class MapSeeingObject {
	public Level Level {get; set;}
	public Player Player;
	public Messages Messages;

	public MapSeeingObject(Level level, Player player, Messages messages) {
		Level = level;
		Player = player;
		Messages = messages;
	}
	
	public (int, int) RandomFreeSquare() {
		var (x, y) = MapFuncs.RandomFreeSquare(Level.Tiles);
		while ((x == Player.X && y == Player.Y) || (Level.ActorAt(x, y) != null)) {
			(x, y) = MapFuncs.RandomFreeSquare(Level.Tiles);
		}
		return (x, y);
	}


	public Actor? ActorAt(int x, int y) {
		return Level.ActorAt(x, y);
	}

	public bool PlayerAt(int x, int y) {
		return (x == Player.X && y == Player.Y);
	}

	public void AddMessage(string message) {
		Messages.AddMessage(message);
	}

	public int DistanceFromPlayer(int x, int y) {
		return Math.Abs(x - Player.X) + Math.Abs(y - Player.Y);
	}

	
}
