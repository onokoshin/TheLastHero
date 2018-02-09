using System;
using System.Collections.Generic;
using System.Text;
using TheLastHero.Models;
using Xamarin.Forms;

namespace TheLastHero.GameEngine
{
    class TurnManager
    {

        // TurnManager will control all creatures’ actions in the battle.
        public TurnManager()
        {
        }

        //The Attack method would be invoked when there is a monster in an adjacent tile 
        //Inside of Attack method, we will conduct damage calculation 
        //Upon a successful attack and killing an enemy, we need to update the battleMap
        public int[,] Attack(Creature attacker, Creature defender, int[,] battleMap)
        {
            // After attacking, we will update the map and return it.
            return battleMap;
        }
        //The method Move will be invoked when a creature chooses to move instead of attack
        //The Move is based on creature’s moving range even though most of creature’s moving
        // range is set to one. 
        // The battleMap is also updated as a creature makes a move
        public int[,] Move(Creature creature, Point position, int[,] battleMap)
        {
            return battleMap;
        }



        // This function will be invoked when its monster’s turn. 
        // It will calculate the next move of the monster towards the closest character 
        public Point MonsterMoveAI(Monster monster, int[,] battleMap)
        {
            Point point = new Point();
            return point;
        }
    }
}


