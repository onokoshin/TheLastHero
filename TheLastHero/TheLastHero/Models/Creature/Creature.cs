using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{
    //parent class of 
    class creature
    {
        //title
        public string name { get; set; }

        //TESTING!
        public string testest { get; set; }
         
        //status 
        public int maxHP { get; set; }
        public int currentHP { get; set; }
        public int maxMP { get; set; }
        public int currentMP { get; set; }
        public int level { get; set; }

        //attributes
        public int attack { get; set; }
        public int defense { get; set; }
        public int speed { get; set; }
        public int luck { get; set; }
        public int moveRange { get; set; }
        public int attackRange { get; set; }

        public bool liveStatus { get; set; }
        public bool isTurn { get; set; }

        // creature constructor
        public creature()
        {

        }

        /**
         * move take damage and makeDamage to business logic
         *
         **/ 
        //take damage - reduces currentHP 
        public void takeDamage(int damage)
        {
            currentHP -= damage;
        }

        //incur damage to enemy 
        public void makeDamage(int damage)
        {
            //incur damage to the enemy

            //this method might need to pass the damage to controller/business logic
            //to deliver the damage to the monster/enemy 
        }

    }
}


