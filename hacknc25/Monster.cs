public static class MonsterFactory {
	public static Actor NewBat(int x, int y) {
		var bat = new Actor(x, y, "Bat", 'b', 5, 2);
		bat.AddAI(new WanderingAI(bat));
		return bat;
	}

	public static Actor NewGoblin(int x, int y) {
		var goblin = new Actor(x, y, "Goblin", 'g', 12, 4);
		goblin.AddAI(new DumbAI(goblin));
		return goblin;
	}
}
