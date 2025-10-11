public class Actor {
	public int X {get; set;}
	public int Y {get; set;}
	public string Name {get; set;}
	public char Symbol {get; set;}
	public IAI AI {get; set;}

	public Actor(int x, int y, string name, char symbol, IAI ai) {
		X = x;
		Y = y;
		Name = name;
		Symbol = symbol;
		AI = ai;
	}

	public (int, int) Act(MapSeeingObject mso) {
		return AI.DecideAction(mso, X, Y);
	}

	public void Move(int dx, int dy) {
		X += dx;
		Y += dy;
	}
}

public interface IAI {
	(int, int) DecideAction(MapSeeingObject mso, int curX, int curY);
}

public class WanderingAI : IAI {
	private List<(int, int)> path {get; set;}
	private Actor parent;
	
	public WanderingAI() {
		path = new List<(int, int)>();
	}

	public (int, int) DecideAction(MapSeeingObject mso, int curX, int curY) {
		if (path.Count == 0) {
			var dest = mso.RandomFreeSquare();
			path = Pathfinding.AStar(mso.Tiles, curX, curY, dest.Item1, dest.Item2);
		}
		var (nextX, nextY) = path[0];
		path.RemoveAt(0);

		int dx = nextX - curX;
		int dy = nextY - curY;

		return (dx, dy);
	}
}

public class GenericMonsterAI {
}

