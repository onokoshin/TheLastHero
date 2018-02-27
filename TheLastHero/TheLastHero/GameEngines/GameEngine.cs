using System;
using System.Collections;
using System.Collections.Generic;
using TheLastHero.Models;
using TheLastHero.Models.Battle;

namespace TheLastHero.GameEngines
{
    public class GameEngine
    {


        public string[] DialogCache { get; set; } = new string[5];
        public string ConsoleDialog1 { get; set; }

        // damage calculations are done inside this helper class. It has member function such as 
        // applyDamage(Creature attacker, Creature defender)
        // DamageCalculation damageCalculation;

        // Turn manager will control all the creatures’ battle actions such as, move, attack, skip and etc.
        // usage example: turnManager.move(character_1, “forward”) and
        // turnManager.attack(character1, monster2)
        // TurnManager turnManager;

        // indicates which level the player is playing, and it also tells manager to handle round related subjects.
        public int currentRound { get; set; }
        // this queue tells game turn manager which creature is next to be able to move around, attack, or // equip it
        // The queue stores characters and monsters in an order of speed, in other words, they are stored in an order of which creature moves next 
        public Queue<Creature> speedQueue { get; set; } = new Queue<Creature>();



        public GameEngine()
        {
        }



        // This method will be called upon beginning of the game, character movement,         
        //   monster movement, character die, monster die events. 
        /*public void startGame()
        {
            speedQueue = new Queue<Creature>();

            //TurnManager turnManager = new TurnManager();

            // game starts
            //  speedQueue = InitializeQueue(monsters, characters);
            while (true)
            {
                // If either number of characters or number of monster equals zero, the battle is over.
                if (battle.characters.Count == 0)
                {

                }
                else if (battle.monsters.Count == 0)
                {
                    // we will generate a new set of monsters for a new round 
                }
                speedQueue.Dequeue();

            }
        }*/

        public void ClearDialogCache()
        {
            for (int i = 0; i < 5; i++)
            {
                DialogCache[i] = "";
            }
        }

        // Using queue data structure to store characters and monster in an order of which creature moves next based on a creature’s speed 
        public Queue InitializeQueue(List<Character> characters, List<Monster> monsters)
        {
            //check every character and monster to determine which creature gets enqueued first
            //sort character list and monster list based on their speed 
            //then we will compare head of each list to determine whether the character or monster is
            // faster. Then we will enqueue the creature with higher speed 
            return null;
        }





    }
}
