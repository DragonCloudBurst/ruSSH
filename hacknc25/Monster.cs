public static class MonsterFactory {
	public static Actor NewBat(int x, int y) {
		var bat = new Actor(x, y, "Bat", 'b', 5, 2);
		bat.AddAI(new WanderingAI(bat));
		return bat;
	}

	public static Actor NewGoblin(int x, int y)
	{
		var goblin = new Actor(x, y, "Goblin", 'g', 12, 4);
		goblin.AddAI(new DumbAI(goblin));
		return goblin;
	}

	public static Actor NewHarpy(int x, int y)
	{
		var harpy = new Actor(x, y, "Harpy", 'h', 15, 3);
		harpy.AddAI(new WanderingAI(harpy));
		return harpy;
	}

	public static Actor NewGargoyle(int x, int y)
	{
		var gargoyle = new Actor(x, y, "Gargoyle", 'r', 20, 2);
		gargoyle.AddAI(new DumbAI(gargoyle));
		return gargoyle;
	}
	
	public static Actor NewDemon(int x, int y)
	{
		var demon = new Actor(x, y, "Demon", 'd', 30, 6);
		demon.AddAI(new DumbAI(demon));
		return demon;
	}

	public static Actor NewDangerousMonster(int x, int y) {
		var mons = new List<Actor>{
			NewGoblin(x, y), NewHarpy(x, y), NewGargoyle(x, y), NewDemon(x, y)
		};
		var r = new Random();
		var mon = mons[r.Next(0, 3)];
		return mon;
	}
}
