public class Level {
	public Tile[,] Tiles {get; set;}
	public List<Actor> Actors {get; set;}

	public Level(Tile[,] tiles) {
		Tiles = tiles;
		Actors = new List<Actor>();
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

	public MapSeeingObject(Level level, Player player) {
		Level = level;
		Player = player;
	}

	public (int, int) RandomFreeSquare() {
		return MapFuncs.RandomFreeSquare(Level.Tiles);
	}

	public Actor? ActorAt(int x, int y) {
		return Level.ActorAt(x, y);
	}

	public bool PlayerAt(int x, int y) {
		return (x == Player.X && y == Player.Y);
	}

	
}
