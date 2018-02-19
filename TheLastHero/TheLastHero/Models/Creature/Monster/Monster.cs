using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{
    public class Monster : Creature
    {
        //monster carry up to one item (not equipped item, it's a drop item)
        public Item UniqueDrop { get; set; }

        public bool Drop;

        //overloaded constructor 
        //monster attributes will be based on the round being passed from battle engine
        //Ex: round 1 monster - weak attributes, but round 10 monster - strong attributes
        //@param level - level will be passed in depending on which round it is 
        public Monster(int Round, bool IsBoss)
        {
            Drop = false;
            Random Rand = new Random();
            int RandNum = Rand.Next(1, 10);

            if (RandNum < 6 && !IsBoss)
            {
                Drop = true;
            }

            if (Drop)
            {
                UniqueDrop = new Item(Round);
            }
            else
            {
                UniqueDrop = null;
            }
            //3, 6, 9...rounds are boss rounds 
            // This is not for initialize 3 times for UniqueDrop 
            //every 3 rounds will be boss rounds with stronger monsters

            //if (round % 3 == 0)
            //{
            //    //this indicates that it is a boss round
            //    //attributes will be enhanced

            //    Item UniqueDrop = new Item(round);

            //}
            //else
            //{
            //    Random rand = new Random();
            //    int randNum = rand.Next(1, 10);

            //    //50% of monsters will have items and the other 50% do not have
            //    // drop items 
            //    if (randNum < 6)
            //    {
            //        Item UniqueDrop = new Item(round);
            //    }
            //    else
            //    {
            //        bool None = true;
            //        Item UniqueDrop = new Item(round, None);
            //    }
            //}
        }

        //monster drops its item when it is killed
        //Items will return to item pool
        //dropItem is a basic function, it is nice to have it so we can reuse
        // it wherever needed. Like battle engine.
        public Item DropItem()
        {
            return UniqueDrop;
        }
    }

}