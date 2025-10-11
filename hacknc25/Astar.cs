public static class Pathfinding {
	public static List<(int, int)>? AStar(Tile[,] grid, int x1, int y1, int x2, int y2) {
		int width = grid.GetLength(0);
		int height = grid.GetLength(1);

		int[,] distances = new int[width, height];
	
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				distances[x, y] = -1;
			}
		}

		distances[x1, y1] = 0;
		var unfinished = new Queue<(int, int)>();
		unfinished.Enqueue((x1, y1));

		while (unfinished.Count > 0) {
			var (cx, cy) = unfinished.Dequeue();

			if (cx == x2 && cy == y2) break; // reached target?

			var adjacents = MapFuncs.GetAdjacentSquares(grid, cx, cy);
			foreach (var (nx, ny) in adjacents) {
				if (grid[nx, ny].Type == TileType.Wall || distances[nx, ny] != -1) {
					continue;
				}

				distances[nx, ny] = distances[cx, cy] + 1;
				unfinished.Enqueue((nx, ny));
			}
		}

		if (distances[x2, y2] == -1) {
			return null;
		}

		var path = new List<(int, int)>();
		var (curx, cury) = (x2, y2);

		while (curx != x1 || cury != y1) {
			path.Add((curx, cury));

			var adjacents = MapFuncs.GetAdjacentSquares(grid, curx, cury);
			foreach (var (nx, ny) in adjacents) {
				if (distances[nx, ny] == distances[curx, cury] - 1) {
					curx = nx;
					cury = ny;
					break;
				}
			}
		}

		path.Add((x1, y1));
		path.Reverse(); // path was built backwards


		return path;
	}
}
