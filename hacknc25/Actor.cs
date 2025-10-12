public class Actor {
	public int X {get; set;}
	public int Y {get; set;}
	public string Name {get; set;}
	public char Symbol {get; set;}
	public int Health {get; set;}
	public int Strength {get; set;}
	public IAI AI {get; set;}


	public Actor(int x, int y, string name, char symbol, int health, int strength) {
		X = x;
		Y = y;
		Name = name;
		Symbol = symbol;
		Health = health;
		Strength = strength;
	}

	public void AddAI(IAI ai) {
		AI = ai;
	}

	public bool IsDead() {
		return Health <= 0;
	}

	public (int, int) Act(MapSeeingObject mso) {
		if (IsDead()) {
			return (0, 0);
		}
		return AI.DecideAction(mso, X, Y);
	}

	public void Move(int dx, int dy) {
		X += dx;
		Y += dy;
	}

	public void TakeDamage(int dmg) {
		Health -= dmg;
	}
}

public interface IAI {
	(int, int) DecideAction(MapSeeingObject mso, int curX, int curY);
}

public class WanderingAI : IAI {
	private List<(int, int)> path {get; set;}
	private Actor Parent;
	
	public WanderingAI(Actor parent) {
		path = new List<(int, int)>();
		Parent = parent;
	}

	public (int, int) DecideAction(MapSeeingObject mso, int curX, int curY) {
		if (path.Count == 0) {
			var dest = mso.RandomFreeSquare();
			path = Pathfinding.AStar(mso.Level.Tiles, curX, curY, dest.Item1, dest.Item2);
			if (path == null) {
				return (0, 0);
			}
		}
		var (nextX, nextY) = path[0];


    	if (nextX == curX && nextY == curY) {
        	path.RemoveAt(0);
			if (path.Count == 0) {
            	return (0, 0); // No more steps to take
        	}
        	(nextX, nextY) = path[0];
    	}

		var actor = mso.ActorAt(nextX, nextY);
		var player = mso.PlayerAt(nextX, nextY);
		if (actor != null) {
			return (0, 0);
		}

		if (player) {
			var player_object = mso.Player;
			Random r = new Random();
			var dmg = r.Next(0, Parent.Strength);
			player_object.TakeDamage(r.Next(0, Parent.Strength));
			mso.AddMessage("The " + Parent.Name + " hits you for " + dmg + " damage.");
		}
		
		path.RemoveAt(0);

		int dx = nextX - curX;
		int dy = nextY - curY;

		return (dx, dy);
	}
}

public class DumbAI : IAI {
	private Actor Parent;
	public DumbAI(Actor parent) {
		Parent = parent;
	}

	public (int, int) DecideAction(MapSeeingObject mso, int curX, int curY) {
		var player_object = mso.Player;
		var path = Pathfinding.AStar(mso.Level.Tiles, curX, curY, player_object.X, player_object.Y);

		var (nextX, nextY) = path[0];
		if (path == null) {
			return (0, 0);
		}
		
    	if (nextX == curX && nextY == curY) {
        	path.RemoveAt(0);
			if (path.Count == 0) {
            	return (0, 0); // No more steps to take
        	}
        	(nextX, nextY) = path[0];
    	}
		
		Random r = new Random();
		var dmg = r.Next(0, Parent.Strength);

		var actor = mso.ActorAt(nextX, nextY);
		var player = mso.PlayerAt(nextX, nextY);
		if (actor != null && !actor.IsDead()) {
			actor.TakeDamage(dmg);
			mso.AddMessage("The " + Parent.Name + " hits the " + actor.Name + " for " + dmg + " damage.");
			return (0, 0);
		}

		if (player) {
			player_object.TakeDamage(r.Next(0, Parent.Strength));
			mso.AddMessage("The " + Parent.Name + " hits you for " + dmg + " damage.");
			return (0, 0);
		}
		
		path.RemoveAt(0);

		int dx = nextX - curX;
		int dy = nextY - curY;

		return (dx, dy);
	}
}

