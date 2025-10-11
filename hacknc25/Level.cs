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
	public Tile[,] Tiles {get; set;}

	public MapSeeingObject(Tile[,] tiles) {
		Tiles = tiles;
	}

	public (int, int) RandomFreeSquare() {
		return MapFuncs.RandomFreeSquare(Tiles);
	}
}
