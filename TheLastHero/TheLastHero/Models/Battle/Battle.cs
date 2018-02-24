﻿using System;
using System.Collections;
using System.Collections.Generic;
using TheLastHero.GameEngines;
using Xamarin.Forms;

namespace TheLastHero.Models.Battle
{
    public class Battle
    {

        // The data structure list contains selected six characters passed from    SelectCharacterPage
        public List<Character> characters;

        // The data structure list contains contains six monsters that will appear in each battle
        // The six monsters will be selected by using generateMonster() function 
        public List<Monster> monsters;

        // List of Items 
        public List<Item> itemPool;




    }

}
