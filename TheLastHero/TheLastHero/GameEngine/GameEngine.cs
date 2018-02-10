using System;
using System.Collections;
using System.Collections.Generic;
using TheLastHero.Models;
using TheLastHero.Models.Battle;

namespace TheLastHero.GameEngine
{
    public class GameEngine
    {
        Battle battle = new Battle();

        // Every four rounds, game engine will generate a new boss monster
        const int BOSSROUND = 4;

        public GameEngine()
        {
        }
        // damage calculations are done inside this helper class. It has member function such as 
        // applyDamage(Creature attacker, Creature defender)
        DamageCalculation damageCalculation;

        // Turn manager will control all the creatures’ battle actions such as, move, attack, skip and etc.
        // usage example: turnManager.move(character_1, “forward”) and
        // turnManager.attack(character1, monster2)
        TurnManager turnManager;

        // indicates which level the player is playing, and it also tells manager to handle round related subjects.
        public int currentRound;
        // this queue tells game turn manager which creature is next to be able to move around, attack, or // equip it
        // The queue stores characters and monsters in an order of speed, in other words, they are stored in an order of which creature moves next 
        public Queue<Creature> nextOneQueue;

        // This method will be called upon beginning of the game, character movement,         
        //   monster movement, character die, monster die events. 
        public void startGame()
        {
            nextOneQueue = new Queue<Creature>();

            //TurnManager turnManager = new TurnManager();

            // game starts
            // nextOneQueue = InitializeQueue(monsters, characters);
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
                nextOneQueue.Dequeue();

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

        //This method checks existing queue that contains characters and monsters to determine
        // which character or monster goes next
        public void CheckNextTurn()
        {

            //dequeue when creature died
            var Creature = nextOneQueue.Dequeue();

            //Check whether the creature is still alive
            // if (!creature.liveStatus)
            //    return;

            //if the creature is still alive, we are going to pass that creature into 
            // method overloading to check current creature(monster or character)
            // turnMgr.moveCharacter(Creature , int x, int y, int[][] map); 

            // turnManagement component

        }


        // This method will generate monsters for each round. Initially, the game will have 
        // six monsters and each four rounds, the method will generate a boss monster to       
        // characters
        public List<Monster> GenerateMonsters(int curRound)
        {

            //Check whether the monster is the boss monster
            bool isBoss = false;

            // After every three rounds, our game will generate a new boss monster
            if (curRound % BOSSROUND == 0)
            {
                isBoss = true;
                Monster monster = new Monster(curRound, isBoss);

            }
            else
            {
                // Generate normal monsters

                for (int i = 0; i < 6; i++)
                {
                    Monster monster = new Monster(curRound, isBoss);
                    battle.monsters.Add(monster);
                }

            }
            return battle.monsters;
        }
    }
}
