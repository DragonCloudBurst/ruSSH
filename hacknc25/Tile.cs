public static class TileRenderer {
	public static string Render(Tile[,] tiles) {
		var output = new System.Text.StringBuilder();
		
		for (int y = 0; y < 40; y++) {
			for (int x = 0; x < 80; x++) {
				var tile = tiles[x, y];
				string c = tile.Symbol.ToString();
				output.Append(c);
			}
			output.AppendLine();
		}

		return output.ToString();
	}
}

public enum TileType {
	Floor,
	Wall,
	UpStair,
	DownStair,
}

public class Tile {
	public TileType Type {get; set;}
	public char Symbol {get; set;}
	// public Actor Actor {get; set;}

	public Tile(TileType type, char symbol) {
		Type = type;
		Symbol = symbol;
		// Actor = null;
	}
}

public static class MapFuncs {
	public static (int, int) RandomFreeSquare(Tile[,] grid) {
		var random = new Random();
		
		int width = grid.GetLength(0);
		int height = grid.GetLength(1);

		while (true) {
			int x = random.Next(width);
			int y = random.Next(height);

			if (grid[x, y].Type == TileType.Floor) {
				return (x, y);
			}
		}
	}

	public static (int, int)? FindTileOfType(TileType type, Tile[,] grid) {
		int width = grid.GetLength(0);
		int height = grid.GetLength(1);

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				if (grid[i, j].Type == type) {
					return (i, j);
				}
			}
		}

		return null;
	}
}
