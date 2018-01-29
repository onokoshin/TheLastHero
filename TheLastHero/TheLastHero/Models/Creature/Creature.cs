using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{
    //parent class of 
    public class creature
    {
        // unique key to differentiate creatures
        public int id { get; set; }

        //title
        public string name { get; set; }
        public bool friendly { get; set; }

        //status 
        public int maxHP { get; set; }
        public int currentHP { get; set; }
        public int maxMP { get; set; }
        public int currentMP { get; set; }
        public int level { get; set; }

        //attributes
        public int atk { get; set; }
        public int def { get; set; }
        public int spd { get; set; }
        public int luk { get; set; }
        public int movRange { get; set; }
        public int atkRange { get; set; }

        // this attribute tells if character is dead or not
        public bool liveStatus { get; set; }


        // creature constructor
        public creature()
        {

        }

        // This method calculate the actual damage this creature will totally
        // reveive. The formula include its own level, defense and item defence
        // bonus, and if damage is more than current health, the liveStatus
        // will set to false which is DEAD.
        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {

            }
        }

        // This method calcuate the actual damage this creature can deal to
        // the opponent. The formula include the character's level, skill,
        // attack power and weapon bonus. This method returns interger.
        public int DealDamage()
        {
            return -1;
        }

        // This method check if the character's live or dead, returns bool type
        // result.
        public bool IsDead()
        {
            return false;
        }


    }
}


