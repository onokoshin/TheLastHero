using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TheLastHero.Models
{
    // parent class of characters and monsters 
    public class Creature
    {
        // unique key to differentiate creatures with exact same attributes.
        // Like 3 “Grey Wolf level 3”
        [PrimaryKey]
        public string Id { get; set; }
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        //title 
        //name indicates the name of character or monster 
        public string Name { get; set; }
        //bool friendly indicates whether the units are on player-side or
        // enemy-side.This attribute will be used to ensure character wouldn’t
        // be able to attack his own teammates, vice-versa for enemy units.
        public bool Friendly { get; set; }
        // store the path of the image
        public string ImgSource { get; set; }
        // status 
        // maxHP indicates how much HP character has. This attribute is mapped 
        // to the levelup map. And will be only updated when character levels
        // up.
        public int MaxHP { get; set; }
        // currentHP is needed because it determines if the character is dead or
        // alive. It will decrease everytime a monster makes a successful attack
        // towards character.
        public int CurrentHP { get; set; }
        // maxMP shows how much Magic Power a character have. This attribute
        // will be updated during combat. Like after a magical attack and level
        // up or next battle starts.
        public int MaxMP { get; set; }
        // currentMP is needed because it determine is the character has enough
        // magic power to make magical attack, for example, a mage need enough
        // mp to make multiple attack.
        public int CurrentMP { get; set; }
        // level is needed because it is related to the core game mechanics
        // every character could level up 
        public int Lvl { get; set; }
        // type indicates what image will be used for this character
        public string Type { get; set; }
        // attributes
        // atk indicates what creature attack skills are.
        public int Atk { get; set; }
        // def indicates what creature defense skills are.
        public int Def { get; set; }
        // spd indicates what creature speed skills are.
        public int Spd { get; set; }
        // luk indicates what creature missing hits or critical hits are.
        // If we could have critical hits or missing hits, the game will have
        // more fun
        // luk could decide whether creature hit other creatures.
        public int Luk { get; set; }
        // movRange indicates what creature moving range is.
        public int MoveRange { get; set; }
        // atkRange indicates what creature attacking range is.
        public int AtkRange { get; set; }

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
        public bool LiveStatus { get; set; }


        // The AttributeString will be unpacked and stored in the top level of Character as actual attributes, 
        // but it needs to go here as a string so it can be saved to the database.
        public string AttributeString { get; set; }

        // creature constructor
        public Creature()
        {

        }

        // This method calculate the actual damage this creature will totally
        // receive. The formula include its own level, defense and item  defence
        // bonus, and if damage is more than current health, the liveStatus
        // will set to false which is DEAD.
        // Take Damage
        // If the damage recived, is > health, then death occurs
        // Return the number of experience received for this attack 
        // monsters give experience to characters.  Characters don't accept expereince from monsters
        public void TakeDamage(int damage)
        {
            if (damage <= 0)
            {
                return;
            }

            CurrentHP = CurrentHP - damage;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                // Death...
                CauseDeath();
            }
        }

        // This method calculates the actual damage this creature can deal to
        // the opponent. The formula include the character's level, skill,
        // attack power and weapon bonus. This method returns integer.
        public int DealDamage()
        {
            return -1;
        }

        // Death
        // Alive turns to False
        public void CauseDeath()
        {
            LiveStatus = false;
        }

    }
}
