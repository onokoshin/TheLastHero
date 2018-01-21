using System;
using System.Collections.Generic;
using System.Text;

namespace TheLastHero.Models
{
    //parent class of 
    class creature
    {


        string siyao;

        //title
        string name;

        //status 
        int maxHP;
        int currentHP;
        int maxMP;
        int currentMP;
        int level;


        //attributes
        int attack;
        int defense;
        int speed;
        int luck;
        int moveRange;
        int attackRange;

        bool liveStatus;
        bool isTurn;

        // creature constructor
        public creature()
        {

        }

        //Since properties are private in default, we need getters and setters
        //getters and setters for properties
        public string Name { get => name; set => name = value; }
        public int MaxHP { get => maxHP; set => maxHP = value; }
        public int CurrentHP { get => currentHP; set => currentHP = value; }
        public int MaxMP { get => maxMP; set => maxMP = value; }
        public int CurrentMP { get => currentMP; set => currentMP = value; }
        public int Level { get => level; set => level = value; }
        public int Attack { get => attack; set => attack = value; }
        public int Defense { get => defense; set => defense = value; }
        public int Speed { get => speed; set => speed = value; }
        public int Luck { get => luck; set => luck = value; }
        public int MoveRange { get => moveRange; set => moveRange = value; }
        public int AttackRange { get => attackRange; set => attackRange = value; }
        public bool LiveStatus { get => liveStatus; set => liveStatus = value; }
        public bool IsTurn { get => isTurn; set => isTurn = value; }



        /**
         * move take damage and makeDamage to business logic
         *
         **/ 
        //take damage - reduces currentHP 
        public void takeDamage(int damage)
        {
            CurrentHP -= damage;
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


