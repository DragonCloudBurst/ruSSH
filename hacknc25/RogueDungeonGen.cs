public class GridCell {
	// Pos within larger grid structure
	public int GridX {get; set;}
	public int GridY {get; set;}

	// Top-left origin
	public int X {get; set;}
	public int Y {get; set;}

	
	public int Width {get; set;}
	public int Height {get; set;}

	public bool IsConnected {get; set;}

	public List<(int, int)> ConnectedTo {get; set;} = new List<(int, int)>();

	public override string ToString() {
        var connections = string.Join(", ", ConnectedTo.Select(c => $"({c.Item1},{c.Item2})"));
        return $"Cell[{GridX},{GridY}] at ({X},{Y}) size {Width}x{Height} connected:{IsConnected} -> [{connections}]";
    }
	
}

public class RogueDungeonGenerator {
	private Random random;

	public RogueDungeonGenerator() {
		random = new Random();
	}

	public Tile[,] QuickNewDungeon(int mapWidth, int mapHeight, int gridSize) {
		var grid = GenerateConnectedGrid(mapWidth, mapHeight, gridSize);
		var mg = new RogueDungeonMapConverter();
		var tiles = mg.GenerateMap(grid, gridSize, mapWidth, mapHeight);

		
		var (downstair_x, downstair_y) = MapFuncs.RandomFreeSquare(tiles);
		tiles[downstair_x, downstair_y] = new Tile(TileType.DownStair, '>');


		var (upstair_x, upstair_y) = MapFuncs.RandomFreeSquare(tiles);
		tiles[upstair_x, upstair_y] = new Tile(TileType.UpStair, '<');

		return tiles;
	}

	public GridCell[,] GenerateConnectedGrid(int mapWidth, int mapHeight, int gridSize) {
		var grid = new GridCell[gridSize, gridSize];
		int cellWidth = mapWidth / gridSize;
		int cellHeight = mapHeight / gridSize;

		// generate grid cells
		for (int y = 0; y < gridSize; y++) {
			for (int x = 0; x < gridSize; x++) {
				
				// THESE VALUES NEED TO BE REPLACED!!! TEMPORARY!!!! JUST FOR SAKE OF PUSH!!!

				int roomWidth;
				int roomHeight;
				if (cellWidth / 3 < cellWidth - 2)
				{
					roomWidth = random.Next(cellWidth / 3, cellWidth - 2);
				}
				else
				{
					// TODO CHANGE THIS
					roomWidth = 0;
				}

				if (cellHeight / 3 < cellHeight - 2)
				{
					roomHeight = random.Next(cellHeight / 3, cellHeight - 2);
				}
				else
				{
					// TODO CHANGE THIS
					roomHeight = 0;
				}


				int roomX;
				int roomY;
				if ((cellWidth - roomWidth) <= 2)
				{
					// TODO CHANGE THIS
					roomX = 0;
				}
				else
				{
					roomX = x * cellWidth + random.Next(1, cellWidth - roomWidth);
				}

				if ((cellHeight - roomHeight) <= 2)
				{
					// TODO CHANGE THIS
					roomY = 0;
				}
				else
				{
                    roomY = y * cellHeight + random.Next(1, cellHeight - roomHeight);
                }

					

				grid[x, y] = new GridCell {
					GridX = x, GridY = y,
					X = roomX, Y = roomY,
					Width = roomWidth, Height = roomHeight
				};
			}
		}

		// connecting
		var current = (random.Next(gridSize), random.Next(gridSize));
		grid[current.Item1, current.Item2].IsConnected = true;
		var stack = new Stack<(int, int)>();
		stack.Push(current);

		// connect while we have unconnected neighbors
		while (stack.Count > 0) {
			var (x, y) = stack.Peek();
			var neighbor = GetRandomUnconnectedNeighbor(grid, x, y, gridSize, random);

			if (neighbor.HasValue) {
				var (nx, ny) = neighbor.Value;
				Connect(grid, x, y, nx, ny);
				stack.Push((nx, ny));
			} else {
				stack.Pop();
			}
		}

		// connect all stragglers
		bool hasUnconnected = true;
		while (hasUnconnected) {
			hasUnconnected = false;
			for (int y = 0; y < gridSize; y++) {
				for (int x = 0; x < gridSize; x++) {
					if (!grid[x, y].IsConnected) {
						hasUnconnected = true;
						var neighbor = GetRandomConnectedNeighbor(grid, x, y, gridSize, random);
						if (neighbor.HasValue) {
							Connect(grid, x, y, neighbor.Value.Item1, neighbor.Value.Item2);
						}
					}
				}
			}
		}

		return grid;
	}

	private void Connect(GridCell[,] grid, int x1, int y1, int x2, int y2) {
		grid[x1, y1].ConnectedTo.Add((x2, y2));
		grid[x2, y2].ConnectedTo.Add((x1, y1));
		grid[x2, y2].IsConnected = true;
	}

	private List<(int, int)> GetNeighbors(int x, int y, int size) {
		var neighbors = new List<(int, int)>();

		if (x > 0) neighbors.Add((x - 1, y));
		if (x < size-1) neighbors.Add((x+1, y));
		if (y > 0) neighbors.Add((x, y-1));
		if (y < size-1) neighbors.Add((x, y + 1));

		return neighbors;
	}

	private (int, int)? GetRandomUnconnectedNeighbor(GridCell[,] grid, int x, int y, int size, Random rand) {
		var neighbors = GetNeighbors(x, y, size).Where(n => !grid[n.Item1, n.Item2].IsConnected).ToList();
		return neighbors.Count > 0 ? neighbors[rand.Next(neighbors.Count)] : null;
	}

	private (int, int)? GetRandomConnectedNeighbor(GridCell[,] grid, int x, int y, int size, Random rand) {
		var neighbors = GetNeighbors(x, y, size).Where(n => grid[n.Item1, n.Item2].IsConnected).ToList();
		return neighbors.Count > 0 ? neighbors[rand.Next(neighbors.Count)] : null;
	}
}

public class RogueDungeonMapConverter() {
	public Tile[,] GenerateMap(GridCell[,] grid, int gridSize, int mapWidth, int mapHeight) {
		var tiles = new Tile[mapWidth, mapHeight];

		// fill with walls
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				tiles[x, y] = new Tile(TileType.Wall, '#');
			}
		}

		// carve rooms
		for (int gy = 0; gy < gridSize; gy++) {
			for (int gx = 0; gx < gridSize; gx++) {
				var cell = grid[gx, gy];
				CarveRoom(tiles, cell.X, cell.Y, cell.Width, cell.Height);
			}
		}

		// carve corridors
		for (int gy = 0; gy < gridSize; gy++) {
			for (int gx = 0; gx < gridSize; gx++) {
				var cell = grid[gx, gy];
				var (x1, y1) = GetRoomCenter(cell);

				foreach (var (nx, ny) in cell.ConnectedTo) {
					var neighbor = grid[nx, ny];
					var (x2, y2) = GetRoomCenter(neighbor);
					CarveCorridor(tiles, x1, y1, x2, y2);
				}
			}
		}

		return tiles;
	}

	private void CarveRoom(Tile[,] tiles, int x, int y, int width, int height) {
		for (int dy = 0; dy < height; dy++) {
			for (int dx = 0; dx < width; dx++) {
				int px = x + dx;
				int py = y + dy;
				if (px >= 0 && px < tiles.GetLength(0) && py >= 0 && py < tiles.GetLength(1)) {
					tiles[px, py] = new Tile(TileType.Floor, ' ');
				}
			}
		}
	}

	private (int, int) GetRoomCenter(GridCell cell) {
		return (cell.X + cell.Width / 2, cell.Y + cell.Height / 2);
	}

	private void CarveCorridor(Tile[,] tiles, int x1, int y1, int x2, int y2) {
		var points = Line.Bresenham(x1, y1, x2, y2);

		foreach (var (x, y) in points) {
			tiles[x, y] = new Tile(TileType.Floor, ' ');
		}
	}
	
}
