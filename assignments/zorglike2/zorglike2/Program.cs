using System;
using System.Collections.Generic;

public class Room
{
    public string room_name;
    public string description;//description when viewed from outside


    public Room to_east;
    public Room to_west;
    public Room to_north;
    public Room to_south;

    public bool already_entered;

    public Collectible grabbable;

    public Room(string name, string desc)
    {
        room_name = name;
        description = desc;
        to_east = null;
        to_north = null;
        to_west = null;
        to_south = null;

        already_entered = false;

        grabbable = null;
    }

    public string to_string()
    {
        return room_name + ": " + description;
    }

    public void upon_entry(Character me)
    {
        if (!already_entered)
        {
            if (grabbable != null)
            {
                Console.WriteLine("You grabbed: " + grabbable.to_string());
                me.inventory.Add(grabbable);
            }
        }



        already_entered = true;
    }

    public string surroundings()
    {
        string s = "";
        if (to_north != null) s += "To the north: " + to_north.room_name + "\n";
        if (to_south != null) s += "To the south: " + to_south.room_name + "\n";
        if (to_east != null) s += "To the east: " + to_east.room_name + "\n";
        if (to_west != null) s += "To the west: " + to_west.room_name + "\n";

        return s;
    }
}

public class Collectible
{
    public string name;
    public string description;
    public int quantity;

    public Collectible(string item_name, string desc)
    {
        name = item_name;
        description = desc;
        quantity = 0;

    }

    public string to_string()
    {
        return name + ": " + description;
    }

}



public class Character
{
    public string name;
    public Room location;
    public List<Collectible> inventory;

    public Character(string char_name, Room loc)
    {
        name = char_name;
        location = loc;
        inventory = [];
    }

}


public class Program
{

    static Room porch = new Room("Porch", "a creaky old porch with a rocking chair");
    static Room house = new Room("House", "an old haunted house");
    static Room foyer = new Room("Foyer", "Tiny foyer");
    static Room path = new Room("Dirt path", "a winding dirt path");
    static Room forest = new Room("Forest", "a dark haunted forest. A dragon blows flame at you");
    static Room gravesite = new Room("Gravesite", "a foggy and mysterious place");
    static Room pond = new Room("Pond", "a crystal blue pond filled with fish");
    static Room orchard = new Room("Orchard", "a green orchard filled with apple trees");
    static Room cliff = new Room("Cliff", "a sheer cliff face, rocks fall away as you approach");
    static Room ledge = new Room("Ledge", "");

    static Collectible sword = new Collectible("sword", "a blade to rival excalibur");
    static Collectible skull = new Collectible("skull", "the skull of doom");
    static Collectible water = new Collectible("water", "a flask from the fountain of youth");
    static Collectible apple = new Collectible("apple", "an apple of vitality");
    static Collectible stone = new Collectible("stone", "the stone of strength");


    static Character me = new Character("Alex", porch);

    public static void Main()
    {

        porch.to_north = house;
        house.to_south = porch;

        house.to_east = foyer;
        foyer.to_west = house;

        porch.to_south = path;
        path.to_north = porch;

        path.to_south = forest;
        forest.to_north = path;

        path.to_west = gravesite;
        gravesite.to_east = path;

        porch.to_east = pond;
        pond.to_west = porch;

        pond.to_south = orchard;
        orchard.to_north = pond;

        orchard.to_south = cliff;
        cliff.to_north = orchard;

        path.to_east = orchard;
        orchard.to_west = path;

        cliff.to_south = ledge;
        //there is no north from the ledge because you die

        cliff.to_west = forest;
        forest.to_east = cliff;


        foyer.grabbable = sword;
        gravesite.grabbable = skull;
        pond.grabbable = water;
        orchard.grabbable = apple;
        cliff.grabbable = stone;

        Console.WriteLine("type 'i' for inventory");
        Console.WriteLine("type '-h' for list of acceptable commands");

        for (; ; )
        {


            Console.WriteLine("----------------------");
            Console.WriteLine("You are at: " + me.location.to_string());
            me.location.upon_entry(me);

            Console.WriteLine(me.location.surroundings());

            if (me.location == forest)
            {
                if (!me.inventory.Contains(sword))
                {
                    Console.WriteLine("You were slain by the dragon. Try bringing a sword next time");
                    break;
                }
                else if (!me.inventory.Contains(skull))
                {
                    Console.WriteLine("You were slain by the dragon. The skull of doom will protect you next time");
                    break;
                }
                else if (!me.inventory.Contains(water))
                {
                    Console.WriteLine("You were burned alive by the dragon. The water of life will protect you next time");
                    break;
                }
                else if (!me.inventory.Contains(apple))
                {
                    Console.WriteLine("You did not have anough life spirit to stand up to the dragon. The apple of spirit may protect you next time");
                    break;
                }
                else if (!me.inventory.Contains(stone))
                {
                    Console.WriteLine("You did not have the strength to fight the dragon. Next time bring a stone of strength");
                    break;
                }
                else
                {
                    Console.WriteLine("You encounter a fire-breathing dragon. He blows a tall fountain of fire at your head");
                    Console.WriteLine("Using your mighty sword, the skull of doom, the water of life, the apple of spirit, and the stone of strength, you engage in a battle for the ages");
                    Console.WriteLine("Battered and beaten, you finally slaw the dragon with one final heroic strike");
                    Console.WriteLine("You collected all the goods and slayed the dragon. You win!!");
                    Console.WriteLine("Congratulations");
                    break;
                }
            }
            if (me.location == ledge)
            {
                Console.WriteLine("You dared to walk too close to the ledge. It crumbles and falls from underneath you. You have fallen to your death");
                break;
            }

            string userinput = Console.ReadLine();
            userinput = userinput.ToLower();

            if (userinput == "i")
            {
                Console.WriteLine("your inventory: ");
                foreach (var item in me.inventory)
                {
                    Console.WriteLine(item.to_string());
                }
            }
            if (userinput == "-h")
            {
                Console.WriteLine("[i - inventory]");
                Console.WriteLine("go [north/south/east/west - go that direction]");

            }
            if (userinput == "go north")
            {
                if (me.location.to_north != null)
                {
                    Console.WriteLine("going north");
                    me.location = me.location.to_north;
                }
            }
            if (userinput == "go south")
            {
                if (me.location.to_south != null)
                {
                    Console.WriteLine("going south");
                    me.location = me.location.to_south;
                }
            }
            if (userinput == "go east")
            {
                if (me.location.to_east != null)
                {
                    Console.WriteLine("going east");
                    me.location = me.location.to_east;
                }
            }
            if (userinput == "go west")
            {
                if (me.location.to_west != null)
                {
                    Console.WriteLine("going west");
                    me.location = me.location.to_west;
                }
            }


        }


    }
}