using System.Data;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

public class Item
{
    // item's name. self explanatory
    public string Name = "";
    //item's category. example, ammo
    public string Type = "";

}

public class Weapon : Item
{
    // how much damage the weapon will deal with one blow
    public int Power;
    // how many tiles away the weapon can strike (defaults to 1, right next to enemy)
    public int Range = 1;
    // does the weapon need ammo
    public bool NeedsAmmo = false;
    // name of ammo item, example arrow. defaults to "None"
    public string AmmoType = "None";
    // is the weapon magic or physical
    public string DamageType;

    public Weapon(string name, string type, int power, int range, bool needsAmmo, string ammoType,
    string damageType)
    {
        Name = name;
        Type = type;
        Power = power;
        Range = range;
        NeedsAmmo = needsAmmo;
        AmmoType = ammoType;
        DamageType = damageType;
    }

}

public class Ammo : Item
{
    // how much total of this ammo type does the player have
    public int Count;

    public Ammo(string name, string type, int count)
    {
        Name = name;
        Type = type;
        Count = count;
    }
    
}

public class Potion : Item
{
    // type of effect it provides. possible types: health, attack, defense, throwable
    public string PotionType;
    // value that says how strong the potion is. for example, how much it increases a stat by or
    // how much damage it foes
    public int Power;
    // how long the potion has effects for
    Timer? Duration;

    public Potion(string name, string type, string potionType, int power, Timer? duration)
    {
        Name = name;
        Type = type;
        PotionType = potionType;
        Power = power;
        Duration = duration;
    }

}

public class Scroll : Item
{
    // what does the scroll do
    public string Effect;
    
    public Scroll(string effect)
    {
        Effect = effect;
    }
}