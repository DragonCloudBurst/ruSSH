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
	Wall
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
