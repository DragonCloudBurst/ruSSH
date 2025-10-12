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
    public string DamageType = "";

}

public class Ammo : Item
{
    // how much total of this ammo type does the player have
    public int Count;
    
}

public class Potion : Item
{
    // value that says how strong the potion is. for example, how much it increases a stat by or
    // how much damage it foes
    public int Power;
    // how long the potion has effects for
    public TimeSpan Duration;

}

public class Scroll : Item
{
    // what does the scroll do
    public string Effect;

}


// DIFFERENT SPECIFIC ITEMS

// SWORDS

public class StoneSword : Weapon
{
    public StoneSword()
    {
        Name = "Stone Sword";
        Type = "Sword";
        Power = 3;
        Range = 1;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Physical";

    }
}

public class BronzeSword : Weapon
{
    public BronzeSword()
    {
        Name = "Bronze Sword";
        Type = "Sword";
        Power = 4;
        Range = 1;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Physical";

    }
}

public class SteelSword : Weapon
{
    public SteelSword()
    {
        Name = "Steel Sword";
        Type = "Sword";
        Power = 5;
        Range = 1;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Physical";

    }
}

// SPEARS

public class StoneSpear : Weapon
{
    public StoneSpear()
    {
        Name = "Stone Spear";
        Type = "Spear";
        Power = 2;
        Range = 3;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Physical";

    }
}

public class BronzeSpear : Weapon
{
    public BronzeSpear()
    {
        Name = "Bronze Spear";
        Type = "Spear";
        Power = 2;
        Range = 3;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Physical";

    }
}

public class SteelSpear : Weapon
{
    public SteelSpear()
    {
        Name = "Steel Spear";
        Type = "Spear";
        Power = 4;
        Range = 3;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Physical";

    }
}

// BOWS

public class RecurveBow : Weapon
{
    public RecurveBow()
    {
        Name = "Recurve Bow";
        Type = "Bow";
        Power = 4;
        Range = 6;
        NeedsAmmo = true;
        AmmoType = "Arrow";
        DamageType = "Physical";

    }
}

public class CompoundBow : Weapon
{
    public CompoundBow()
    {
        Name = "Compound Bow";
        Type = "Bow";
        Power = 3;
        Range = 10;
        NeedsAmmo = true;
        AmmoType = "Arrow";
        DamageType = "Physical";

    }
}

public class Crossbow : Weapon
{
    public Crossbow()
    {
        Name = "Crossbow";
        Type = "Bow";
        Power = 4;
        Range = 8;
        NeedsAmmo = true;
        AmmoType = "Arrow";
        DamageType = "Physical";

    }
}

// WANDS

public class StoneWand : Weapon
{
    public StoneWand()
    {
        Name = "Stone Wand";
        Type = "Wand";
        Power = 1;
        Range = 6;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Magic";

    }
}

public class BronzeWand : Weapon
{
    public BronzeWand()
    {
        Name = "Bronze Wand";
        Type = "Wand";
        Power = 2;
        Range = 6;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Magic";

    }
}

public class SteelWand : Weapon
{
    public SteelWand()
    {
        Name = "Steel Wand";
        Type = "Wand";
        Power = 4;
        Range = 6;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Magic";

    }
}

// TOMES

public class PlainTome : Weapon
{
    public PlainTome()
    {
        Name = "Plain Tome";
        Type = "Tome";
        Power = 3;
        Range = 3;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Magic";
    }
}

public class EmbossedTome : Weapon
{
    public EmbossedTome()
    {
        Name = "Embossed Tome";
        Type = "Tome";
        Power = 4;
        Range = 3;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Magic";
    }
}

public class GildedTome : Weapon
{
    public GildedTome()
    {
        Name = "Gilded Tome";
        Type = "Tome";
        Power = 5;
        Range = 3;
        NeedsAmmo = false;
        AmmoType = "None";
        DamageType = "Magic";
    }
}

// AMMO

public class Arrow : Ammo
{
    public Arrow()
    {
        Name = "Arrow";
        Type = "Ammo";
        // initialize arrows at 0, add to this as player picks up
        Count = 0;
    }
}

// POTIONS

// HEALTH POTIONS

public class BasicHealthPotion : Potion
{
    public BasicHealthPotion()
    {
        Name = "Basic Health Potion";
        Type = "Health";
        Power = 10;
        // one second since it's a one-time effect
        Duration = new TimeSpan(0, 0, 0, 1);
    }
}

public class GreaterHealthPotion : Potion
{
    public GreaterHealthPotion()
    {
        Name = "Greater Health Potion";
        Type = "Health";
        Power = 20;
        // one second since it's a one-time effect
        Duration = new TimeSpan(0, 0, 0, 1);
    }
}

// ATTACK POTIONS

public class BasicAttackPotion : Potion
{
    public BasicAttackPotion()
    {
        Name = "Basic Attack Potion";
        Type = "Attack";
        Power = 2;
        // one minute
        Duration = new TimeSpan(0, 0, 1, 0);
    }
}

public class GreaterAttackPotion : Potion
{
    public GreaterAttackPotion()
    {
        Name = "Greater Attack Potion";
        Type = "Attack";
        Power = 4;
        // one minute
        Duration = new TimeSpan(0,0,1,0);
    }
}

// DEFENSE POTIONS

public class BasicDefensePotion : Potion
{
    public BasicDefensePotion()
    {
        Name = "Basic Defense Potion";
        Type = "Defense";
        Power = 2;
        // one minute
        Duration = new TimeSpan(0, 0, 1, 0);
    }
}

public class GreaterDefensePotion : Potion
{
    public GreaterDefensePotion()
    {
        Name = "Basic Defense Potion";
        Type = "Defense";
        Power = 4;
        // one minute
        Duration = new TimeSpan(0, 0, 1, 0);
    }
}

// THROWING POTIONS

public class AcidPotion : Potion
{
    public AcidPotion()
    {
        Name = "Acid Potion";
        Type = "Throwable";
        // damage per 2 seconds?
        Power = 1;
        // 20 seconds
        Duration = new TimeSpan(0, 0, 0, 20);

    }
}

public class MagmaPotion : Potion
{
    public MagmaPotion()
    {
        Name = "Magma Potion";
        Type = "Throwable";
        // damage per 2 seconds?
        Power = 2;
        // 20 seconds
        Duration = new TimeSpan(0, 0, 0, 20);

    }
}

// SCROLLS

public class LifeScroll : Scroll
{
    public LifeScroll()
    {
        Name = "Life Scroll";
        Type = "Scroll";
        Effect = "If you are defeated in combat, this scroll will resurrect you once.";
    }
}

public class DeathScroll : Scroll
{
    public DeathScroll()
    {
        Name = "Life Scroll";
        Type = "Scroll";
        Effect = "This scroll will instantly kill the targeted enemy.";
    }
}