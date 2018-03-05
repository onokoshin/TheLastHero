using System;
using System.Collections.Generic;
using TheLastHero.Models;
using TheLastHero.Models.Battle;
using TheLastHero.ViewModels;
using Xamarin.Forms;
using System.Linq;


/** This is our battle controller. all logical actions related to battle are 
 * written here. We will use a Queue structure for turn management, the queue 
 * takes all creatures based on their speed. Hence, the queue is in an order of 
 * which creature goes next. The queue is declared as “ speedQueue” in our code 
 * below.  In order to store creatures into a queue in a proper order, we are
 * going to sort both character list and monster list. Then we will compare head 
 * of each list’s creature’s speed to determine which creature would be enqueued
 * into our queue. This way, creatures with higher speed will get their turn 
 * first. Once, a creature is dequeued, we will first check whether the 
 * creature is alive or dead based on creature’s ‘liveStatus.’ 
 * If the creature is dead, we skip and not enqueue the creature back to
 * the queue. If the creature is alive, we will use turnManager to control 
 * its actions.   

Turn manager,turnManager, will control all the creatures’ battle actions: move, 
attack, skip and etc. In every round, every creature only be allowed to control 
action once. Character movement will be decide and input by user then
turnManager will implement users’ turn decisions. Monster movement will be 
decided by Game engine randomly. The turnManager will oftern update if any 
creature is eliminated in each round.

Grid Layout ,battleGrid, is our battle game board, we have a 4x6 size board, 
each grid element is clickable. We will capture the click event and do 
calculations and updates here. The position of monsters and characters are 
memorized by a 2 dimensional integer array. Int [][] map, 0 indicate this
tile is empty, if a monster or character is occupying this tile, we will 
register the array with its ID. Later on, we can use this map to do movement 
and attack calculation.  

There will be 6 monsters in each battle. The monster will grow stronger each 
round linearly, and our character has a cap level at 20. So eventually, all 
the monsters will become very powerful and kill all the characters. Every 4 
rounds, the characters will engage a boss fight, which there is only one 
monster. The boss monster is 6 times stronger than the normal monster. The 
BOSSROUND is a const indicate every 4 rounds there will be the boss fight.

**/
namespace TheLastHero.Views
{
    public partial class BattlePage : ContentPage
    {
        // Batstle map is a grid layout  
        //Grid battleGrid = new Grid();
        private GameEngineViewModel _viewModel;
        Script _script = new Script();
        Character curCharacter = new Character();
        Monster curMonster = new Monster();
        Creature curCreature = new Creature();
        bool atkTurn = false;
        bool endTurn = false;
        Battle battle = new Battle();


        //Constructor 
        public BattlePage()
        {
            InitializeComponent();
            _viewModel = GameEngineViewModel.Instance;

            // read sqldatabase or mockdatabase
            _viewModel.LoadDataCommand.Execute(null);
            // start with level 1 by default
            _viewModel.gameEngine.currentRound = 1;
            //create speedqueue and render map
            InitializeBattle();

            //????should we use 2 queues characterqueue and monsterqueue,
            // creature queue doesn make sense because we dont need another sets
            // of creature, we already have character and monsters

            if (_viewModel.gameEngine.characterQueue.Count > 0 && _viewModel.gameEngine.monsterQueue.Count > 0)
            {
                bool monsterTurn = true;

                // determine weather character or monster
                if (_viewModel.gameEngine.characterQueue.Peek().Spd >= _viewModel.gameEngine.monsterQueue.Peek().Spd)
                {
                    monsterTurn = false;
                }

                if (monsterTurn)
                {
                    // monster turn

                    while (_viewModel.gameEngine.monsterQueue.Count > 0 && !_viewModel.gameEngine.monsterQueue.Peek().Friendly)
                    {
                        //curMonster = _viewModel.MonsterDataset.Where(x => x.ID == _viewModel.gameEngine.monsterQueue.Peek().ID).First();

                        curMonster = _viewModel.gameEngine.monsterQueue.Dequeue();

                        // if there is character nearby attack!
                        Character target = CheckNearbyCharacter(curMonster.xPosition, curMonster.yPosition);
                        if (target != null)
                        {
                            //attack
                            applyDamageMTC(curMonster, target);
                        }
                        else
                        {
                            //move
                            //move left and right
                            if (curMonster.xPosition > 0 && _viewModel.battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition].Equals(""))
                            {


                                _viewModel.battle.battleMapTop[curMonster.xPosition, curMonster.yPosition] = "";
                                _viewModel.battle.battleMapId[curMonster.xPosition, curMonster.yPosition] = "";
                                _viewModel.battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition] = curMonster.ImgSource;
                                _viewModel.battle.battleMapId[curMonster.xPosition - 1, curMonster.yPosition] = curMonster.Id;
                                curMonster.xPosition = curMonster.xPosition - 1;
                                curMonster.yPosition = curMonster.yPosition;

                            }
                        }



                        _viewModel.gameEngine.monsterQueue.Enqueue(curMonster);
                        curMonster = null;
                    }
                }
                else
                {
                    // character turn dequeue and hold dont enqueue.
                    curCharacter = _viewModel.gameEngine.characterQueue.Dequeue();

                    _viewModel.battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                    _viewModel.battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;
                    RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);

                }



                // while monster is true
                // auto move, auto attack, no highlight

                //until character
                // highlight character, highlight move grid, highlight attack grit
                // wait for click

            }
            else if (_viewModel.gameEngine.characterQueue.Count == 0)
            {
                //game over
            }
            else if (_viewModel.gameEngine.monsterQueue.Count == 0)
            {
                // new round
            }

            //_script.scriptCounter = 1;
            //RunScript(_script, 0);
            _viewModel.battle.RefreshAllCell();
            BindingContext = _viewModel;
        }



        private void InitializeBattle()
        {

            int round = _viewModel.gameEngine.currentRound;
            _viewModel.battle.SetAllSelection(Battle.HIGHLIGHTGREY);
            _viewModel.battle.SetAllTop("");
            _viewModel.battle.SetAllId("");
            if (round <= 3)
            {
                _viewModel.battle.title = "Forest Lvl 1-3";
                _viewModel.battle.SetAllBackground(Battle.GRASS);
            }
            else if (round > 3 && round <= 6)
            {
                _viewModel.battle.title = "Mountain Lvl 4-6";
                _viewModel.battle.SetAllBackground(Battle.SAND);
            }
            else if (round > 6 && round <= 9)
            {
                _viewModel.battle.title = "Desert Lvl 7-9";
                _viewModel.battle.SetAllBackground(Battle.GRASS);
            }
            else
            {
                _viewModel.battle.title = "Lava Cave Lvl???";
                _viewModel.battle.SetAllBackground(Battle.LAVA);
            }
            _viewModel.InitMonsterQueue();
            _viewModel.InitCharacterQueue();
            RenderCharactersMonsters();
        }

        // render all characters and monsters to the map
        private void RenderCharactersMonsters()
        {
            foreach (Character c in _viewModel.CharacterDataset)
            {
                _viewModel.battle.battleMapTop[c.xPosition, c.yPosition] = c.ImgSource;
                _viewModel.battle.battleMapId[c.xPosition, c.yPosition] = c.Id;
            }
            foreach (Monster m in _viewModel.MonsterDataset)
            {
                _viewModel.battle.battleMapTop[m.xPosition, m.yPosition] = m.ImgSource;
                _viewModel.battle.battleMapId[m.xPosition, m.yPosition] = m.Id;
            }
        }

        private void RenderMoveAttackRange(int x, int y, int totalR, int atkR)
        {
            //_viewModel.battle.battleMapSelection[x, y] = Battle.HIGHLIGHTGREEN;
            if (x > 0)
            {
                if (totalR > atkR)
                {
                    _viewModel.battle.battleMapSelection[x - 1, y] = Battle.HIGHLIGHTGREEN;
                    if (totalR > 1) { RenderMoveAttackRange(x - 1, y, totalR - 1, atkR); }

                }
                //else if (_viewModel.battle.battleMapSelection[x - 1, y].Equals(""))
                else if (!_viewModel.battle.battleMapSelection[x - 1, y].Equals(Battle.HIGHLIGHTGREEN))
                {
                    _viewModel.battle.battleMapSelection[x - 1, y] = Battle.HIGHLIGHTRED;
                    if (totalR > 1) { RenderMoveAttackRange(x - 1, y, totalR - 1, atkR); }
                }
            }
            if (x < 4)
            {
                if (totalR > atkR)
                {
                    _viewModel.battle.battleMapSelection[x + 1, y] = Battle.HIGHLIGHTGREEN;
                    if (totalR > 1) { RenderMoveAttackRange(x + 1, y, totalR - 1, atkR); }
                }
                //else if (_viewModel.battle.battleMapSelection[x + 1, y].Equals(""))
                else if (!_viewModel.battle.battleMapSelection[x + 1, y].Equals(Battle.HIGHLIGHTGREEN))
                {
                    _viewModel.battle.battleMapSelection[x + 1, y] = Battle.HIGHLIGHTRED;
                    if (totalR > 1) { RenderMoveAttackRange(x + 1, y, totalR - 1, atkR); }
                }
            }
            if (y > 0)
            {
                if (totalR > atkR)
                {
                    _viewModel.battle.battleMapSelection[x, y - 1] = Battle.HIGHLIGHTGREEN;
                    if (totalR > 1) { RenderMoveAttackRange(x, y - 1, totalR - 1, atkR); }
                }
                else if (!_viewModel.battle.battleMapSelection[x, y - 1].Equals(Battle.HIGHLIGHTGREEN))

                {
                    _viewModel.battle.battleMapSelection[x, y - 1] = Battle.HIGHLIGHTRED;
                    if (totalR > 1) { RenderMoveAttackRange(x, y - 1, totalR - 1, atkR); }
                }
            }
            if (y < 5)
            {
                if (totalR > atkR)
                {
                    _viewModel.battle.battleMapSelection[x, y + 1] = Battle.HIGHLIGHTGREEN;
                    if (totalR > 1) { RenderMoveAttackRange(x, y + 1, totalR - 1, atkR); }
                }
                else if (!_viewModel.battle.battleMapSelection[x, y + 1].Equals(Battle.HIGHLIGHTGREEN))

                {
                    _viewModel.battle.battleMapSelection[x, y + 1] = Battle.HIGHLIGHTRED;
                    if (totalR > 1) { RenderMoveAttackRange(x, y + 1, totalR - 1, atkR); }
                }
            }


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;

            if (ToolbarItems.Count > 0)
            {
                ToolbarItems.RemoveAt(0);
            }

            InitializeComponent();

            if (_viewModel.CharacterDataset.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }

            BindingContext = _viewModel;
        }

        /*private void RunScript(Script s, int s_num)
        {
            string hp = "";
            bool moved = false;
            bool attacked = false;
            // dequeue current creature from speed queue
            if (_viewModel.gameEngine.speedQueue.Count > 0 && s_num != 0)
            {

                Queue<Creature> swapQueue = new Queue<Creature>();

                while (_viewModel.gameEngine.speedQueue.Count > 0)
                {
                    Creature c = _viewModel.gameEngine.speedQueue.Dequeue();
                    bool matchAndDead = false;
                    for (int i = 0; i < s.GetScripts()[s_num].Length; i = i + 7)
                    {
                        if (c.ID == s.GetScripts()[s_num][i + 4])
                        {
                            hp = s.GetScripts()[s_num][i + 3].ToString();
                            if (s.GetScripts()[s_num][i + 5] == 1)
                            {
                                moved = true;
                            }
                            if (s.GetScripts()[s_num][i + 6] == 1)
                            {
                                attacked = true;
                            }
                            // remove dead creature
                            if (s.GetScripts()[s_num][i] == 0)
                            {
                                matchAndDead = true;
                            }
                        }

                    }
                    if (!matchAndDead)
                    {
                        swapQueue.Enqueue(c);
                    }
                }


                while (swapQueue.Count > 0)
                {
                    Creature c = swapQueue.Dequeue();
                    _viewModel.gameEngine.speedQueue.Enqueue(c);
                }

                if (_viewModel.gameEngine.speedQueue.Count > 0)
                {
                    var currentCreature = _viewModel.gameEngine.speedQueue.Dequeue();
                    for (int i = 4; i > 0; i--)
                    {
                        _viewModel.gameEngine.DialogCache[i] = _viewModel.gameEngine.DialogCache[i - 1];
                    }
                    _viewModel.gameEngine.DialogCache[0] = "Turn " + _script.scriptCounter + ": " + currentCreature.Name + " " + hp + "HP ";

                    if (attacked && !moved)
                    {
                        _viewModel.gameEngine.DialogCache[0] += "is attacking";

                    }
                    else if (attacked && moved)
                    {
                        _viewModel.gameEngine.DialogCache[0] += " moved and is attacking";

                    }
                    else
                    {
                        _viewModel.gameEngine.DialogCache[0] += " moved";

                    }

                    _viewModel.gameEngine.speedQueue.Enqueue(currentCreature);
                }
                else
                {
                    _viewModel.gameEngine.DialogCache[0] = "Game Over";
                }

                for (int i = 4; i > 0; i--)
                    {
                        _viewModel.gameEngine.DialogCache[i] = _viewModel.gameEngine.DialogCache[i - 1];
                    }
                    _viewModel.gameEngine.DialogCache[0] = "Game Over";
                _viewModel.gameEngine.ConsoleDialog1 = _viewModel.gameEngine.DialogCache[0] + "\n"
                    + _viewModel.gameEngine.DialogCache[1] + "\n"
                    + _viewModel.gameEngine.DialogCache[2] + "\n"
                    + _viewModel.gameEngine.DialogCache[3] + "\n"
                    + _viewModel.gameEngine.DialogCache[4];

                //  set Dead


            }
            else
            {
                _viewModel.gameEngine.ConsoleDialog1 = "empty queue";
            }

            // update grid
            _viewModel.battle.SetAllTop("");

            if (s.scriptCounter < 34)
            {
                _viewModel.battle.SetAllBackground("Grass.png");

            }
            else
            {
                _viewModel.battle.SetAllBackground("Sand.png");

            }

            _viewModel.battle.SetAllSelection("HighlightGrey.png");
            for (int i = 0; i < s.GetScripts()[s_num].Length; i = i + 7)
            {

                if (s.GetScripts()[s_num][i] == 1)
                {
                    _viewModel.battle.battleMapTop[s.GetScripts()[s_num][i + 1], s.GetScripts()[s_num][i + 2]] = s.imgAry[s.GetScripts()[s_num][i + 4]];

                }
                else
                {

                    _viewModel.battle.battleMapTop[s.GetScripts()[s_num][i + 1], s.GetScripts()[s_num][i + 2]] = "";

                }
            }
            _viewModel.battle.RefreshAllCell();

        }*/

        public void printDialog(string str)
        {
            for (int i = 4; i > 0; i--)
            {
                _viewModel.gameEngine.DialogCache[i] = _viewModel.gameEngine.DialogCache[i - 1];
            }
            _viewModel.gameEngine.DialogCache[0] = str;
            _viewModel.gameEngine.ConsoleDialog1 = _viewModel.gameEngine.DialogCache[0] + "\n"
                + _viewModel.gameEngine.DialogCache[1] + "\n"
                + _viewModel.gameEngine.DialogCache[2] + "\n"
                + _viewModel.gameEngine.DialogCache[3] + "\n"
                + _viewModel.gameEngine.DialogCache[4];
        }

        public void Next_Clicked(object sender, EventArgs e)
        {
            //do character thing
            //if clicked on green move, set move true, check surrounding if
            // monster present render red, over
            _viewModel.battle.SetAllTop(Battle.HIGHLIGHTGREY);
            _viewModel.gameEngine.characterQueue.Enqueue(curCharacter);
            curCharacter = null;

            //do monster thing
            if (_viewModel.gameEngine.characterQueue.Count > 0 && _viewModel.gameEngine.monsterQueue.Count > 0)
            {
                bool monsterTurn = true;

                // determine weather character or monster
                if (_viewModel.gameEngine.characterQueue.Peek().Spd >= _viewModel.gameEngine.monsterQueue.Peek().Spd)
                {
                    monsterTurn = false;
                }

                if (monsterTurn)
                {
                    // monster turn
                    curMonster = _viewModel.gameEngine.monsterQueue.Dequeue();
                    while (_viewModel.gameEngine.monsterQueue.Count > 0
                           && !_viewModel.gameEngine.monsterQueue.Peek().Friendly
                           && (_viewModel.gameEngine.characterQueue.Peek().Spd < _viewModel.gameEngine.monsterQueue.Peek().Spd))
                    {
                        //curMonster = _viewModel.MonsterDataset.Where(x => x.ID == _viewModel.gameEngine.monsterQueue.Peek().ID).First();

                        curMonster = _viewModel.gameEngine.monsterQueue.Dequeue();

                        if (curMonster.LiveStatus)
                            _viewModel.gameEngine.monsterQueue.Enqueue(curMonster);
                        curMonster = null;
                    }
                }
                else
                {
                    // character turn dequeue and hold dont enqueue.

                    curCharacter = _viewModel.gameEngine.characterQueue.Dequeue();

                    _viewModel.battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                    _viewModel.battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;


                    RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);


                }



                // while monster is true
                // auto move, auto attack, no highlight

                //until character
                // highlight character, highlight move grid, highlight attack grit
                // wait for click

            }
            else
            {
                //empty characters or empty monsters, error!
            }

            //_script.scriptCounter = 1;
            //RunScript(_script, 0);
            _viewModel.battle.RefreshAllCell();
            BindingContext = null;
            BindingContext = _viewModel;

            /* _viewModel.battle.cell_00_bottom = "Sand.png";
             if (_script.scriptCounter > 49)
             {
                 Navigation.InsertPageBefore(new GameOver(), Navigation.NavigationStack[1]);
                 Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
             }
             else
             {
                 //RunScript(_script, _script.scriptCounter);
                 //_script.scriptCounter++;
                 BindingContext = null;
                 BindingContext = _viewModel;

             }*/
        }



        public void Reset_Clicked(object sender, EventArgs e)
        {
            _viewModel.InitMonsterQueue();
            _viewModel.InitCharacterQueue();
            _viewModel.LoadDataCommand.Execute(null);

            _viewModel.gameEngine.ClearDialogCache();
            _script.scriptCounter = 1;
            // do something
            //_viewModel.Data.battle.battleMapTop[0, 0] = "KnightRight.png";
            _viewModel.battle.SetAllSelection("HighlightGrey.png");
            _viewModel.battle.SetAllBackground("Grass.png");
            _viewModel.battle.SetAllTop("");

            foreach (var c in _viewModel.CharacterDataset)
            {
                _viewModel.battle.battleMapTop[c.xPosition, c.yPosition] = c.ImgSource;
            }

            foreach (var m in _viewModel.MonsterDataset)
            {
                _viewModel.battle.battleMapTop[m.xPosition, m.yPosition] = m.ImgSource;
            }

            _viewModel.battle.RefreshAllCell();
            printDialog("Reset Clicked");

            BindingContext = null;
            BindingContext = _viewModel;
        }

        public void HandleButtonClicked(int x, int y)
        {
            // if clicked within moverange
            // if creature within range, do nothing

            if (_viewModel.battle.battleMapSelection[x, y].Equals(Battle.HIGHLIGHTGREEN)
                && _viewModel.battle.battleMapTop[x, y].Equals("") || (x == curCharacter.xPosition && y == curCharacter.yPosition))
            {
                // move character
                _viewModel.battle.battleMapTop[curCharacter.xPosition, curCharacter.yPosition] = "";
                _viewModel.battle.battleMapId[curCharacter.xPosition, curCharacter.yPosition] = "";
                _viewModel.battle.battleMapTop[x, y] = curCharacter.ImgSource;
                curCharacter.xPosition = x;
                curCharacter.yPosition = y;
                _viewModel.battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                if (CheckNearbyMonster(x, y))
                {

                    RenderAttackRange(x, y, curCharacter.AtkRange);
                    atkTurn = true;
                    endTurn = false;
                }
                else
                {
                    atkTurn = false;
                    endTurn = true;
                }


            }
            else if (atkTurn)
            {
                if (_viewModel.battle.battleMapSelection[x, y].Equals(Battle.HIGHLIGHTRED) &&
                    _viewModel.gameEngine.monsterQueue.Where(z => z.Id.Equals(_viewModel.battle.battleMapId[x, y])).Count() > 0)
                {

                    // grab target from monsterqueue, calculate dmg, decresase target.HP, 
                    Monster m = _viewModel.gameEngine.monsterQueue.Where(z => z.Id.Equals(_viewModel.battle.battleMapId[x, y])).First();
                    printDialog(curCharacter + " is attacking " + m.Name);
                    // decrease target HP by = level attack + weapon attack  MIKE PLESASE READ HERE
                    applyDamageCTM(curCharacter, m);

                    //_viewModel.gameEngine.ConsoleDialog1 = m.CurrentHP.ToString();
                    if (m.CurrentHP <= 0)
                    {//remove dead monster DEATH MIKE PLESASE READ HERE
                        Queue<Monster> tmp = new Queue<Monster>();
                        while (_viewModel.gameEngine.monsterQueue.Count() > 0)
                        {
                            Monster tmpM = _viewModel.gameEngine.monsterQueue.Dequeue();
                            if (_viewModel.gameEngine.monsterQueue.Peek().Id.Equals(m.Id))
                            {

                            }
                            else
                            {
                                tmp.Enqueue(tmpM);
                            }
                        }
                        while (tmp.Count() > 0)
                        {
                            Monster tmpM = tmp.Dequeue();

                            _viewModel.gameEngine.monsterQueue.Enqueue(tmpM);

                        }
                    }
                    endTurn = true;
                }
                else
                {
                    endTurn = false;
                }


            }



            // autoattack if monster present
            // done enqueue character
            if (endTurn)
            {
                _viewModel.battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                _viewModel.gameEngine.characterQueue.Enqueue(curCharacter);
                curCharacter = null;
                if (_viewModel.gameEngine.characterQueue.Count > 0 && _viewModel.gameEngine.monsterQueue.Count > 0)
                {
                    // monster turn
                    while ((_viewModel.gameEngine.characterQueue.Peek().Spd < _viewModel.gameEngine.monsterQueue.Peek().Spd))
                    {
                        //curMonster = _viewModel.MonsterDataset.Where(x => x.ID == _viewModel.gameEngine.monsterQueue.Peek().ID).First();
                        //all monster activies
                        curMonster = _viewModel.gameEngine.monsterQueue.Dequeue();

                        if (curMonster.xPosition > 0 && _viewModel.battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition].Equals(""))
                        {
                            _viewModel.battle.battleMapId[curMonster.xPosition - 1, curMonster.yPosition] = _viewModel.battle.battleMapId[curMonster.xPosition, curMonster.yPosition];
                            _viewModel.battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition] = _viewModel.battle.battleMapTop[curMonster.xPosition, curMonster.yPosition];
                            _viewModel.battle.battleMapId[curMonster.xPosition, curMonster.yPosition] = "";
                            _viewModel.battle.battleMapTop[curMonster.xPosition, curMonster.yPosition] = "";
                            curMonster.xPosition -= 1;
                        }

                        Character target = CheckNearbyCharacter(curMonster.xPosition, curMonster.yPosition);
                        if (target != null)
                        {
                            //attack
                            applyDamageMTC(curMonster, target);

                            // update target to queue
                            if (target.CurrentHP <= 0)
                            {//remove dead monster 
                                RemoveTargetInCharacterQueue(target);
                            }
                            else
                            {
                                //update character queue with target
                                UpdateTargetInCharacterQueue(target);
                            }
                        }
                        _viewModel.gameEngine.monsterQueue.Enqueue(curMonster);
                        curMonster = null;
                    }



                }
                else if (_viewModel.gameEngine.characterQueue.Count == 0)
                {
                    //game over
                }
                else if (_viewModel.gameEngine.monsterQueue.Count == 0)
                {
                    //round over
                }
            }

            // check empty if character 
            // character turn dequeue and hold dont enqueue.
            curCharacter = _viewModel.gameEngine.characterQueue.Dequeue();

            _viewModel.battle.SetAllSelection(Battle.HIGHLIGHTGREY);
            _viewModel.battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;
            _viewModel.battle.battleMapId[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.Id;


            RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);


            // "itempool" is gloabl var
            // drop item => put in the pool
            // 



            // while monster is true
            // auto move, auto attack, no highlight

            //until character
            // highlight character, highlight move grid, highlight attack grit
            // wait for click










            printDialog(curCharacter.Name + "'s turn");
            //_script.scriptCounter = 1;
            //RunScript(_script, 0);
            _viewModel.battle.RefreshAllCell();
            BindingContext = null;
            BindingContext = _viewModel;
        }

        private void RemoveTargetInCharacterQueue(Character target)
        {
            Queue<Character> tmp = new Queue<Character>();
            while (_viewModel.gameEngine.characterQueue.Count() > 0)
            {
                Character tmpC = _viewModel.gameEngine.characterQueue.Dequeue();
                if (_viewModel.gameEngine.characterQueue.Peek().Id.Equals(target.Id))
                {
                    //discard
                }
                else
                {
                    tmp.Enqueue(tmpC);
                }
            }
            while (tmp.Count() > 0)
            {
                Character tmpC = tmp.Dequeue();

                _viewModel.gameEngine.characterQueue.Enqueue(tmpC);

            }
        }

        private void UpdateTargetInCharacterQueue(Character target)
        {
            Queue<Character> tmp = new Queue<Character>();
            while (_viewModel.gameEngine.characterQueue.Count() > 0)
            {
                Character tmpC = _viewModel.gameEngine.characterQueue.Dequeue();
                if (_viewModel.gameEngine.characterQueue.Peek().Id.Equals(target.Id))
                {
                    tmp.Enqueue(target);
                }
                else
                {
                    tmp.Enqueue(tmpC);
                }
            }
            while (tmp.Count() > 0)
            {
                Character tmpC = tmp.Dequeue();

                _viewModel.gameEngine.characterQueue.Enqueue(tmpC);

            }
        }

        private void applyDamageMTC(Monster m, Character c)
        {
            int dmg = (int)Math.Ceiling(m.Atk / 4.0);
            c.CurrentHP -= dmg;
            printDialog(m.Name + " took " + dmg + " damage!");
        }

        private void applyDamageCTM(Character c, Monster m)
        {
            int dmg = 0;
            if (c.EquippedItem.ContainsKey(Character.Locations.RightHand) && c.EquippedItem[Character.Locations.RightHand] != null)
            {
                dmg = (int)Math.Ceiling(c.Atk / 4.0) + c.EquippedItem[Character.Locations.RightHand].Atk;
            }
            else
            {
                dmg = (int)Math.Ceiling(c.Atk / 4.0);
            }
            m.CurrentHP -= dmg;
            printDialog(m.Name + " took " + dmg + " damage!");
        }

        public Item dropItem(int monsterID)
        {
            // read monster from Dataset, 
            var result = _viewModel.MonsterDataset.Where(x => x.Id == _viewModel.gameEngine.monsterQueue.Peek().Id).First();
            return _viewModel.ItemDataset.Where(i => i.Id.Equals(result.UniqueDropID)).First();
        }

        private Character CheckNearbyCharacter(int x, int y)
        {

            if (x > 0 && _viewModel.gameEngine.characterQueue.Where(c => c.Id.Equals(_viewModel.battle.battleMapId[x - 1, y])).Count() > 0)
            {
                return _viewModel.gameEngine.characterQueue.Where(c => c.Id.Equals(_viewModel.battle.battleMapId[x - 1, y])).First();

            }
            if (x < 4 && _viewModel.gameEngine.characterQueue.Where(c => c.Id.Equals(_viewModel.battle.battleMapId[x + 1, y])).Count() > 0)
            {
                return _viewModel.gameEngine.characterQueue.Where(c => c.Id.Equals(_viewModel.battle.battleMapId[x + 1, y])).First();
            }
            if (y > 0 && _viewModel.gameEngine.characterQueue.Where(c => c.Id.Equals(_viewModel.battle.battleMapId[x, y - 1])).Count() > 0)
            {
                return _viewModel.gameEngine.characterQueue.Where(c => c.Id.Equals(_viewModel.battle.battleMapId[x, y - 1])).First();
            }
            if (y < 5 && _viewModel.gameEngine.characterQueue.Where(c => c.Id.Equals(_viewModel.battle.battleMapId[x, y + 1])).Count() > 0)
            {
                return _viewModel.gameEngine.characterQueue.Where(c => c.Id.Equals(_viewModel.battle.battleMapId[x, y + 1])).First();
            }
            return null;
        }


        private bool CheckNearbyMonster(int x, int y)
        {

            if (x > 0 && _viewModel.gameEngine.monsterQueue.Where(m => m.Id.Equals(_viewModel.battle.battleMapId[x - 1, y])).Count() > 0)
            {
                return true;

            }
            if (x < 4 && _viewModel.gameEngine.monsterQueue.Where(m => m.Id.Equals(_viewModel.battle.battleMapId[x + 1, y])).Count() > 0)
            {
                return true;
            }
            if (y > 0 && _viewModel.gameEngine.monsterQueue.Where(m => m.Id.Equals(_viewModel.battle.battleMapId[x, y - 1])).Count() > 0)
            {
                return true;
            }
            if (y < 5 && _viewModel.gameEngine.monsterQueue.Where(m => m.Id.Equals(_viewModel.battle.battleMapId[x, y + 1])).Count() > 0)
            {
                return true;
            }
            return false;
        }

        private void RenderAttackRange(int x, int y, int atkRange)
        {
            //_viewModel.battle.battleMapSelection[x, y] = Battle.HIGHLIGHTGREEN;
            if (x > 0)
            {

                _viewModel.battle.battleMapSelection[x - 1, y] = Battle.HIGHLIGHTRED;
                if (atkRange > 1) { RenderAttackRange(x - 1, y, atkRange - 1); }


            }
            if (x < 4)
            {

                _viewModel.battle.battleMapSelection[x + 1, y] = Battle.HIGHLIGHTRED;
                if (atkRange > 1) { RenderAttackRange(x + 1, y, atkRange - 1); }


            }
            if (y > 0)
            {

                _viewModel.battle.battleMapSelection[x, y - 1] = Battle.HIGHLIGHTRED;
                if (atkRange > 1) { RenderAttackRange(x, y - 1, atkRange - 1); }

            }
            if (y < 5)
            {

                _viewModel.battle.battleMapSelection[x, y + 1] = Battle.HIGHLIGHTRED;
                if (atkRange > 1) { RenderAttackRange(x, y + 1, atkRange - 1); }

            }
        }

        public void Cell00Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 0); }
        public void Cell01Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 1); }
        public void Cell02Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 2); }
        public void Cell03Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 3); }
        public void Cell04Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 4); }
        public void Cell05Clicked(object sender, EventArgs e) { HandleButtonClicked(0, 5); }
        public void Cell10Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 0); }
        public void Cell11Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 1); }
        public void Cell12Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 2); }
        public void Cell13Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 3); }
        public void Cell14Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 4); }
        public void Cell15Clicked(object sender, EventArgs e) { HandleButtonClicked(1, 5); }
        public void Cell20Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 0); }
        public void Cell21Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 1); }
        public void Cell22Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 2); }
        public void Cell23Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 3); }
        public void Cell24Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 4); }
        public void Cell25Clicked(object sender, EventArgs e) { HandleButtonClicked(2, 5); }
        public void Cell30Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 0); }
        public void Cell31Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 1); }
        public void Cell32Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 2); }
        public void Cell33Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 3); }
        public void Cell34Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 4); }
        public void Cell35Clicked(object sender, EventArgs e) { HandleButtonClicked(3, 5); }
        public void Cell40Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 0); }
        public void Cell41Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 1); }
        public void Cell42Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 2); }
        public void Cell43Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 3); }
        public void Cell44Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 4); }
        public void Cell45Clicked(object sender, EventArgs e) { HandleButtonClicked(4, 5); }
    }
}