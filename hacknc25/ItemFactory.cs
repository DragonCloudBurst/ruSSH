using Microsoft.VisualBasic;

public class ItemFactory
{
    public readonly Random randNums = new Random();
    public readonly Random randScrolls = new Random();
    public readonly Random randItems = new Random();

    List<Item> allItemsButAmmoAndScrolls = new List<Item>
    {
        new StoneSword(), new BronzeSword(), new SteelSword(), new StoneSpear(), new BronzeSpear(),
        new SteelSpear(), new RecurveBow(), new CompoundBow(), new Crossbow(), new StoneWand(),
        new BronzeWand(), new SteelWand(), new PlainTome(), new EmbossedTome(), new GildedTome(),
        new BasicHealthPotion(), new GreaterHealthPotion(), new BasicAttackPotion(),
        new GreaterAttackPotion(), new BasicDefensePotion(), new GreaterDefensePotion(),
        new AcidPotion(), new MagmaPotion()

    };

    public Item NewItem()
    {
        int genNumber = randNums.Next(1, 100);

        if (genNumber <= 35)
        {
            // 35% chance to be ammo
            return new Arrow();
        }
        else if (genNumber <= 45)
        {
            // 10% chance to be a scroll
            // then it's an even chance for each scroll
            int scrollNum = randScrolls.Next(1, 2);

            if (scrollNum == 1)
            {
                return new LifeScroll();
            }
            else
            {
                return new DeathScroll();
            }

        }
        else
        {
            // 55% literally everything else
            int randItem = randItems.Next(0, allItemsButAmmoAndScrolls.Count);

            return allItemsButAmmoAndScrolls[randItem];

        }
    
    }
}