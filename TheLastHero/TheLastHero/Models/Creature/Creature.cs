using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{
    // parent class of characters and monsters 
    public class creature
    {
        // unique key to differentiate creatures with exact same attributes.
        // Like 3 “Grey Wolf level 3”
        public int id { get; set; }

        //title 
        //name indicates the name of character or monster 
        public string name { get; set; }
        //bool friendly indicates whether the units are on player-side or
        // enemy-side.This attribute will be used to ensure character wouldn’t
        // be able to attack his own teammates, vice-versa for enemy units.
        public bool friendly { get; set; }

        // status 
        // maxHP indicates how much HP character has. This attribute is mapped 
        // to the levelup map. And will be only updated when character levels
        // up.
        public int maxHP { get; set; }
        // currentHP is needed because it determines if the character is dead or
        // alive. It will decrease everytime a monster makes a successful attack
        // towards character.
        public int currentHP { get; set; }
        // maxMP shows how much Magic Power a character have. This attribute
        // will be updated during combat. Like after a magical attack and level
        // up or next battle starts.
        public int maxMP { get; set; }
        // currentMP is needed because it determine is the character has enough
        // magic power to make magical attack, for example, a mage need enough
        // mp to make multiple attack.
        public int currentMP { get; set; }
        // level is needed because it is related to the core game mechanics
        // every character could level up 
        public int level { get; set; }

        // attributes
        // atk indicates what creature attack skills are.
        public int atk { get; set; }
        // def indicates what creature defense skills are.
        public int def { get; set; }
        // spd indicates what creature speed skills are.
        public int spd { get; set; }
        // luk indicates what creature missing hits or critical hits are.
        // If we could have critical hits or missing hits, the game will have
        // more fun
        // luk could decide whether creature hit other creatures.
        public int luk { get; set; }
        // movRange indicates what creature moving range is.
        public int movRange { get; set; }
        // atkRange indicates what creature attacking range is.
        public int atkRange { get; set; }

        // liveStatus maintains the character’s live status. If return value
        // is true which means the character is alive, false means the character
        // is dead. When game starts,  this value is set to True by default.
        // During the battle, every time the character is hit by a monster,
        // we will not only decrease character’s currentHP, but also checks the
        // currentHP to determine whether the character is alive or dead.
        // This property, liveStatus, is a key property to determine whether or
        // not to display characters on the gameboard. 
        // We decide not to use “isDead()” function because we can easily access
        // liveStatus by setter and getter.
        public bool liveStatus { get; set; }


        // creature constructor
        public creature()
        {

        }

        // This method calculate the actual damage this creature will totally
        // receive. The formula include its own level, defense and item  defence
        // bonus, and if damage is more than current health, the liveStatus
        // will set to false which is DEAD.
        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {

            }
        }

        // This method calculates the actual damage this creature can deal to
        // the opponent. The formula include the character's level, skill,
        // attack power and weapon bonus. This method returns integer.
        public int DealDamage()
        {
            return -1;
        }

    }
}
