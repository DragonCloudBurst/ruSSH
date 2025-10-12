using System;
using System.Collections.Generic;

public enum Race
{
    Human,
    Elf,
    Orc,
    Dwarf
}
public enum ClassType
{
    Warrior,
    Mage,
    Rogue
}

public sealed record StatBlock(int HP, int Att, int Def)
{
    public static StatBlock operator +(StatBlock a, StatBlock b) =>
        new StatBlock(a.HP + b.HP, a.Att + b.Att, a.Def + b.Def);
}

public sealed record PlayerData(String Name, Race Race, ClassType Class, StatBlock Stats);

public static class CharacterGenerator
{
    static readonly Dictionary<ClassType, StatBlock> ClassBase = new()
    {
        [ClassType.Warrior] = new StatBlock(10, 4, 8),
        [ClassType.Mage] = new StatBlock(6, 4, 6),
        [ClassType.Rogue] = new StatBlock(8, 6, 4)
    };

    static readonly Dictionary<Race, StatBlock> RaceModifiers = new()
    {
        [Race.Human] = new StatBlock(0, 0, 0),
        [Race.Elf] = new StatBlock(-2, 2, 0),
        [Race.Orc] = new StatBlock(2, -3, -2),
        [Race.Dwarf] = new StatBlock(-1, -1, 2)
    };

    public static PlayerData Create(string name, Race race, ClassType classType)
    {
        var baseStats = ClassBase[classType];
        var mod = RaceModifiers[race];
        var finalStats = baseStats + mod;
        return new PlayerData(name, race, classType, finalStats);
    }
}
    