using System.Collections.ObjectModel;
using Xamarin.Forms;
using TheLastHero.GameEngines;
using TheLastHero.Models;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using TheLastHero.Models.Battle;

namespace TheLastHero.ViewModels
{
    public class GameEngineViewModel : BaseViewModel
    {
        // declare varaibles
        public GameEngine gameEngine { get; set; }
        public CharactersViewModel characterViewModel { get; set; }
        public Battle battle { get; set; }
        public ObservableCollection<Character> CharacterDataset { get; set; }
        public ObservableCollection<Monster> MonsterDataset { get; set; }
        //public ObservableCollection<Creature> CreatureDataset { get; set; }
        public ObservableCollection<Item> ItemDataset { get; set; }
        public Command LoadDataCommand { get; set; }
        public Command LoadDataCommandCharacterOnly { get; set; }

        // for hackathon changes
        public int potionNum { get; set; }
        public bool allowRoundHealing { get; set; }
        public bool availablePotion { get; set; }
        public bool availableFocusAtk { get; set; }
        public bool useFocusAtk { get; set; }
        public bool magicRevive = true;
        public Random magicReviveRandom = new Random();
        public int magicReviveTarget = 6;

        //turn management variables
        public Character curCharacter { get; set; }
        Monster curMonster = new Monster();
        bool atkTurn = false;
        bool endTurn = false;
        bool endRound = false;
        public bool gameOver = false;
        private bool _needsRefresh;

        //Koshin's Addition
        //Holds the official score
        public Score BattleScore = new Score();

        public string AttackerName = string.Empty;
        public string TargetName = string.Empty;
        public string AttackStatus = string.Empty;

        public string TurnMessage = string.Empty;
        public string TurnMessageSpecial = string.Empty;

        public int DamageAmount = 0;
        public HitStatusEnum HitStatus = HitStatusEnum.Unknown;

        // CharacterViewModel is used to pass selection of characters based on CRUDi
        private CharactersViewModel charViewModel;

        // MonstersViewModel is used to pass existing monsters based on CRUDi 
        private MonstersViewModel monViewModel;

        private ItemsViewModel itemViewModel;

        // constructer of GameEngineViewModel
        // It will loads data from Datastore
        // also creates instances of gameEngine and battle class
        public GameEngineViewModel()
        {
            //all viewModel references are pointing at the appropriate instances 
            charViewModel = CharactersViewModel.Instance;
            monViewModel = MonstersViewModel.Instance;
            itemViewModel = ItemsViewModel.Instance;
            CharacterDataset = charViewModel.Dataset;
            MonsterDataset = monViewModel.Dataset;
            ItemDataset = itemViewModel.Dataset;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
            LoadDataCommandCharacterOnly = new Command(async () => await ExecuteLoadDataCommandCharacterOnly());
            LoadDataCommand.Execute(null);

            gameEngine = GameEngine.Instance;
            gameEngine.ClearDialogCache();
            battle = new Battle();
            curCharacter = new Character();
        }

        // this method automaticall move monsters if monster's speed id higher than characters
        // if character is fastest, it will select that character and render move range
        // and attack range, and wait for player to click to send command
        public void MoveFirstCreature(CharactersViewModel dataFromBattlePage)
        {
            gameEngine.currentRound = 1;
            charViewModel = dataFromBattlePage;
            monViewModel = MonstersViewModel.Instance;
            itemViewModel = ItemsViewModel.Instance;
            CharacterDataset = charViewModel.Dataset;
            MonsterDataset = monViewModel.Dataset;
            ItemDataset = itemViewModel.Dataset;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            // read sqldatabase or mockdatabase
            LoadDataCommand.Execute(null);
            // start with level 1 by default

            //create speedqueue and render map
            InitializeBattle();
            potionNum = 6;
            allowRoundHealing = true;
            availablePotion = true;
            availableFocusAtk = true;

            // determine weather character or monster
            if (gameEngine.characterQueue.Peek().Spd >= gameEngine.monsterQueue.Peek().Spd)
            {
                // character turn dequeue and hold dont enqueue.
                curCharacter = gameEngine.characterQueue.Dequeue();

                battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                //highlight current character
                battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;
                // function that takes characters move range and attack range and update to screen.
                RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);
            }
            else
            {
                // monster turn
                while (gameEngine.monsterQueue.Count > 0 && (gameEngine.characterQueue.Peek().Spd < gameEngine.monsterQueue.Peek().Spd))
                {
                    //curMonster =  MonsterDataset.Where(x => x.ID ==  gameEngine.monsterQueue.Peek().ID).First();
                    curMonster = gameEngine.monsterQueue.Dequeue();
                    // if there is character nearby attack!
                    Character target = CheckNearbyCharacter(curMonster.xPosition, curMonster.yPosition);
                    if (target != null)
                    {
                        //attack
                        ApplyDamageMTC(curMonster, target);
                        if (target.CurrentHP <= 0)
                        {
                            RemoveTargetFromQueues(target);
                            //also need to set livestatus to dead in dataset
                            battle.battleMapId[target.xPosition, target.yPosition] = "";
                            battle.battleMapTop[target.xPosition, target.yPosition] = "";
                            battle.battleMapHP[target.xPosition, target.yPosition] = "";
                        }
                        else
                        {
                            battle.battleMapHP[target.xPosition, target.yPosition] = target.CurrentHP.ToString();
                            UpdateTargetInQueues(target);
                        }
                    }
                    else
                    {
                        //move monster
                        if (curMonster.xPosition > 0 && battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition].Equals(""))
                        {
                            battle.battleMapTop[curMonster.xPosition, curMonster.yPosition] = "";
                            battle.battleMapId[curMonster.xPosition, curMonster.yPosition] = "";
                            battle.battleMapHP[curMonster.xPosition, curMonster.yPosition] = "";
                            battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition] = curMonster.ImgSource;
                            battle.battleMapId[curMonster.xPosition - 1, curMonster.yPosition] = curMonster.Id;
                            battle.battleMapHP[curMonster.xPosition - 1, curMonster.yPosition] = curMonster.CurrentHP.ToString();

                            curMonster.xPosition = curMonster.xPosition - 1;
                            curMonster.yPosition = curMonster.yPosition;

                            target = CheckNearbyCharacter(curMonster.xPosition, curMonster.yPosition);
                            if (target != null)
                            {
                                //attack
                                ApplyDamageMTC(curMonster, target);
                                if (target.CurrentHP <= 0)
                                {

                                    RemoveTargetFromQueues(target);
                                    //also need to set livestatus to dead in dataset
                                    battle.battleMapId[target.xPosition, target.yPosition] = "";
                                    battle.battleMapTop[target.xPosition, target.yPosition] = "";
                                    battle.battleMapHP[target.xPosition, target.yPosition] = "";
                                }
                                else
                                {
                                    battle.battleMapHP[target.xPosition, target.yPosition] = target.CurrentHP.ToString();
                                    UpdateTargetInQueues(target);
                                }

                            }

                        }
                    }
                    gameEngine.movedMonsters.Enqueue(curMonster);
                    curMonster = null;
                }
                // character turn dequeue and hold dont enqueue.
                curCharacter = gameEngine.characterQueue.Dequeue();

                battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                //highlight current character
                battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;
                // function that takes characters move range and attack range and update to screen.
                RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);
            }

            battle.RefreshAllCell();
        }

        private async Task ExecuteLoadDataCommandCharacterOnly()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                CharacterDataset.Clear();
                ItemDataset.Clear();
                var cdataset = await DataStore.GetAllAsync_Character(true);
                var idataset = await DataStore.GetAllAsync_Item(true);

                //CreatureDataset.Clear();
                foreach (var data in cdataset)
                {
                    if (data.LiveStatus)
                        CharacterDataset.Add(data);
                }

                foreach (var data in idataset)
                {
                    ItemDataset.Add(data);
                }
                //CreatureDataset = new ObservableCollection<Creature>(CreatureDataset.OrderByDescending(i => i.Spd));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                CharacterDataset.Clear();
                MonsterDataset.Clear();
                ItemDataset.Clear();
                var cdataset = await DataStore.GetAllAsync_Character(true);
                var mdataset = await DataStore.GetAllAsync_Monster(true);
                var idataset = await DataStore.GetAllAsync_Item(true);

                //CreatureDataset.Clear();
                foreach (var data in cdataset)
                {
                    if (data.LiveStatus)
                        CharacterDataset.Add(data);
                    //CreatureDataset.Add(data);
                }
                foreach (var data in mdataset)
                {
                    MonsterDataset.Add(data);
                    //CreatureDataset.Add(data);
                }
                foreach (var data in idataset)
                {

                    if (data.Location.Equals(ItemLocationEnum.Necklass))
                    {
                        data.EquippableLocation = "Necklass";
                        data.Atk = data.Damage;
                    }
                    else if (data.Location.Equals(ItemLocationEnum.Feet))
                    {
                        data.EquippableLocation = "Feet";
                        data.Atk = data.Damage;
                    }
                    else if (data.Location.Equals(ItemLocationEnum.PrimaryHand))
                    {
                        data.EquippableLocation = "PrimaryHand";
                        data.Atk = data.Damage;
                    }
                    else if (data.Location.Equals(ItemLocationEnum.Unknown))
                    {
                        data.EquippableLocation = "Unknown";
                        data.Atk = data.Damage;
                    }
                    else if (data.Location.Equals(ItemLocationEnum.Finger))
                    {
                        data.EquippableLocation = "Finger"; data.Atk = data.Damage;
                    }
                    else if (data.Location.Equals(ItemLocationEnum.Head))
                    {
                        data.EquippableLocation = "Head"; data.Atk = data.Damage;
                    }
                    else if (data.Location.Equals(ItemLocationEnum.LeftFinger))
                    {
                        data.EquippableLocation = "LeftFinger"; data.Atk = data.Damage;
                    }
                    else if (data.Location.Equals(ItemLocationEnum.RightFinger))
                    {
                        data.EquippableLocation = "RightFinger"; data.Atk = data.Damage;
                    }
                    else if (data.Location.Equals(ItemLocationEnum.OffHand))
                    {
                        data.EquippableLocation = "OffHand"; data.Atk = data.Damage;
                    }

                    ItemDataset.Add(data);
                }
                //CreatureDataset = new ObservableCollection<Creature>(CreatureDataset.OrderByDescending(i => i.Spd));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }



        // Return True if a refresh is needed
        // It sets the refresh flag to false
        public bool NeedsRefresh()
        {
            if (_needsRefresh)
            {
                _needsRefresh = false;
                return true;
            }
            return false;
        }

        // Sets the need to refresh
        public void SetNeedsRefresh(bool value)
        {
            _needsRefresh = value;
        }


        /*public void BuildSpeedQueue()
        {
            if (CreatureDataset.Count > 0)
            {
                gameEngine.speedQueue.Clear();
                foreach (Creature c in CreatureDataset)
                {
                    gameEngine.speedQueue.Enqueue(c);
                }
            }
        }*/

        // This method grab all characters from characterViewModel and 
        // register them to gameEngine's characterQueue
        public void InitCharacterQueue(CharactersViewModel viewModel)
        {
            characterViewModel = viewModel;

            if (characterViewModel.Party.Count > 0)
            {
                gameEngine.characterQueue.Clear();
                int y = 0;

                foreach (Character c in characterViewModel.Party)
                {
                    c.xPosition = 0;
                    c.yPosition = y;
                    gameEngine.characterQueue.Enqueue(c);
                    y++;
                }

            }
        }

        // This method grab all monsters from MonsterDataset and 
        // register them to gameEngine's MonsterQueue
        public void InitMonsterQueue()
        {
            if (monViewModel.monsterParty.Count > 0)
            {
                gameEngine.monsterQueue.Clear();

                int y = 0;
                foreach (var m in monViewModel.monsterParty)
                {
                    gameEngine.monsterQueue.Enqueue(m);
                    y++;
                }
            }
        }



        // Read item template from ItemDataset and create new item and register
        // them to gameEngine
        public Item GenerateNewItem()
        {
            //return guid of the item
            Random r = new Random();
            int index = r.Next(1, ItemDataset.Count());
            Item i = new Item();
            CopyItem(i, ItemDataset[index]);
            return i;
        }

        // Copy item helper function
        private void CopyItem(Item i, Item item)
        {
            i.Atk = item.Atk;
            i.Damage = item.Damage;
            i.Def = item.Def;
            i.Description = item.Description;
            i.EquippableLocation = item.EquippableLocation;
            i.EquippedBy = item.EquippedBy;
            i.HP = item.HP;
            i.Guid = item.Guid;
            i.ImgSource = item.ImgSource;
            i.Location = item.Location;

            i.Luk = item.Luk;
            i.Lvl = item.Lvl;
            i.MP = item.MP;
            i.Range = item.Range;
            i.Spd = item.Spd;
            i.SpecialAbility = item.SpecialAbility;
            i.Type = item.Type;
            i.Value = item.Value;

            switch (item.Location)
            {
                case ItemLocationEnum.Feet:
                    i.EquippableLocation = "Feet";
                    break;
                case ItemLocationEnum.Finger:
                    i.EquippableLocation = "LeftFinger";
                    break;
                case ItemLocationEnum.Head:
                    i.EquippableLocation = "Head";
                    break;
                case ItemLocationEnum.LeftFinger:
                    i.EquippableLocation = "LeftFinger";
                    break;
                case ItemLocationEnum.Necklass:
                    i.EquippableLocation = "Necklass";
                    break;
                case ItemLocationEnum.OffHand:
                    i.EquippableLocation = "OffHand";
                    break;
                case ItemLocationEnum.PrimaryHand:
                    i.EquippableLocation = "PrimaryHand";
                    break;
                case ItemLocationEnum.RightFinger:
                    i.EquippableLocation = "RightFinger";
                    break;
                case ItemLocationEnum.Unknown:
                    i.EquippableLocation = "Unknown";
                    break;
                default:
                    i.EquippableLocation = "Unknown";
                    break;

            }

        }



        // When a new round starts, this method will be called, and it creates
        // new monsters based on current round level and scale that monster to level.
        public void GenerateNewMonsters()
        {
            monViewModel = MonstersViewModel.Instance;
            itemViewModel = ItemsViewModel.Instance;
            MonsterDataset = monViewModel.Dataset;
            ItemDataset = itemViewModel.Dataset;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
            LoadDataCommand.Execute(null);
            monViewModel.monsterParty.Clear();

            for (int i = 0; i < 6; i++)
            {
                //decide which monster in the dataset to get
                var randIndex = HelperEngine.RollDice(1, MonsterDataset.Count() - 1);
                Monster m = new Monster();
                Monster dataMonster = MonsterDataset[randIndex];

                //generate new guid and assign all the attributes 
                m.Id = Guid.NewGuid().ToString();
                m.xPosition = 4;
                m.yPosition = i;
                m.Friendly = dataMonster.Friendly;
                m.AtkRange = dataMonster.AtkRange;
                m.LiveStatus = dataMonster.LiveStatus;
                m.CurrentHP = dataMonster.CurrentHP;
                m.MaxHP = dataMonster.MaxHP;
                m.CurrentMP = dataMonster.CurrentMP;
                m.MaxMP = dataMonster.MaxMP;
                m.MoveRange = dataMonster.MoveRange;
                m.Drop = dataMonster.Drop;
                m.Luk = dataMonster.Luk;
                m.UniqueDropID = GenerateNewItem().Guid;
                m.Type = dataMonster.Type;
                m.ImgSource = dataMonster.ImgSource;

                //Basic Attribute updated 
                m.Lvl = dataMonster.Lvl;
                m.Atk = dataMonster.Atk;
                m.Def = dataMonster.Def;
                m.Spd = dataMonster.Spd;
                m.Name = dataMonster.Name;

                //monster level gets scaled with currentRound 
                m.ScaleLevel(gameEngine.currentRound);
                m.AllocateExp();

                //add the monster to monsterParty List 
                monViewModel.monsterParty.Add(m);

            }

        }

        // Helper function for AUTO BATTLE
        // Auto create characters for player
        public void GenerateNewCharacters()
        {
            charViewModel.Party.Clear();

            for (int i = 0; i < 6; i++)
            {
                //decide which monster in the dataset to get
                var randIndex = HelperEngine.RollDice(1, CharacterDataset.Count() - 1);
                Character c = new Character();
                Character dataCharacter = CharacterDataset[randIndex];

                //generate new guid and assign all the attributes 
                c.Id = Guid.NewGuid().ToString();
                c.xPosition = 0;
                c.yPosition = i;
                c.Friendly = dataCharacter.Friendly;
                c.AtkRange = dataCharacter.AtkRange;
                c.LiveStatus = dataCharacter.LiveStatus;
                c.CurrentHP = dataCharacter.CurrentHP;
                c.MaxHP = dataCharacter.MaxHP;
                c.CurrentMP = dataCharacter.CurrentMP;
                c.MaxMP = dataCharacter.MaxMP;
                c.MoveRange = dataCharacter.MoveRange;
                c.Luk = dataCharacter.Luk;
                c.Type = dataCharacter.Type;
                c.ImgSource = dataCharacter.ImgSource;
                c.NextLevelExp = dataCharacter.NextLevelExp;
                c.CurrentExp = dataCharacter.CurrentExp;
                c.EquippedItem = new Dictionary<ItemLocationEnum, Item>();

                //Basic Attribute updated 
                c.Lvl = dataCharacter.Lvl;
                c.Atk = dataCharacter.Atk;
                c.Def = dataCharacter.Def;
                c.Spd = dataCharacter.Spd;
                c.Name = dataCharacter.Name;


                //insert these monsters into the monsterParty
                charViewModel.Party.Add(c);

            }

        }


        // This is the main functionality of the game, this method waits for player click
        // based on the clicked cell's coordinate cell 
        public void HandleButtonClicked(int x, int y)
        {
            // check if user wants to move the character
            if (battle.battleMapSelection[x, y].Equals(Battle.HIGHLIGHTGREEN)
                && battle.battleMapTop[x, y].Equals("") || (x == curCharacter.xPosition && y == curCharacter.yPosition))
            {
                // move character
                battle.battleMapTop[curCharacter.xPosition, curCharacter.yPosition] = "";
                battle.battleMapId[curCharacter.xPosition, curCharacter.yPosition] = "";
                battle.battleMapHP[curCharacter.xPosition, curCharacter.yPosition] = "";
                battle.battleMapTop[x, y] = curCharacter.ImgSource;
                battle.battleMapId[x, y] = curCharacter.Id;
                battle.battleMapHP[x, y] = curCharacter.CurrentHP.ToString();
                curCharacter.xPosition = x;
                curCharacter.yPosition = y;
                battle.SetAllSelection(Battle.HIGHLIGHTGREY);
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
                if (battle.battleMapSelection[x, y].Equals(Battle.HIGHLIGHTRED) &&
                    (gameEngine.monsterQueue.Where(z => z.Id.Equals(battle.battleMapId[x, y])).Count() > 0
                     || gameEngine.movedMonsters.Where(z => z.Id.Equals(battle.battleMapId[x, y])).Count() > 0))
                {
                    Monster m = new Monster();
                    // grab target from monsterqueue, calculate dmg, decresase target.HP, 
                    if (gameEngine.monsterQueue.Where(z => z.Id.Equals(battle.battleMapId[x, y])).Count() > 0)
                        m = gameEngine.monsterQueue.Where(z => z.Id.Equals(battle.battleMapId[x, y])).First();
                    else if (gameEngine.movedMonsters.Where(z => z.Id.Equals(battle.battleMapId[x, y])).Count() > 0)
                        m = gameEngine.movedMonsters.Where(z => z.Id.Equals(battle.battleMapId[x, y])).First();

                    PrintDialog(curCharacter.Name + " is attacking " + m.Name);
                    // decrease target HP by = level attack + weapon attack  MIKE PLESASE READ HERE
                    ApplyDamageCTM(curCharacter, m);
                    //update character's hp in case he attacks himself
                    battle.battleMapHP[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.CurrentHP.ToString();


                    // gameEngine.ConsoleDialog1 = m.CurrentHP.ToString();
                    if (m.CurrentHP <= 0)
                    {
                        if (m.UniqueDropID != null)
                        {
                            Item drop = ItemDataset.Where(item => item.Guid.Equals(m.UniqueDropID)).First();
                            //curCharacter.EquipItem(drop, drop.Location);
                            AddItemToScore(drop);

                        }

                        RemoveTargetFromQueues(m);
                        battle.battleMapId[m.xPosition, m.yPosition] = "";
                        battle.battleMapTop[m.xPosition, m.yPosition] = "";
                        battle.battleMapHP[m.xPosition, m.yPosition] = "";

                        if (gameEngine.movedMonsters.Count() == 0 && gameEngine.monsterQueue.Count() == 0)
                        {
                            endRound = true;
                        }
                    }
                    else
                    {
                        battle.battleMapHP[m.xPosition, m.yPosition] = m.CurrentHP.ToString();
                        UpdateTargetInQueues(m);

                    }



                    endTurn = true;

                    //This is for score
                    BattleScore.TurnCount++;
                }
                else
                {
                    endTurn = false;
                }


            }// end of attack turn



            // Autoattack if monster present
            // done enqueue character
            if (endTurn)
            {
                battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                gameEngine.movedCharacters.Enqueue(curCharacter);
                curCharacter = null;

                // gameEngine.characterQueue.Enqueue( curCharacter);
                // curCharacter = null;
                if (gameEngine.characterQueue.Count > 0 && gameEngine.monsterQueue.Count > 0)
                {
                    // monster turn
                    while ((gameEngine.characterQueue.Count > 0 && gameEngine.monsterQueue.Count > 0) &&
                           (gameEngine.characterQueue.Peek().Spd < gameEngine.monsterQueue.Peek().Spd))
                    {
                        curMonster = gameEngine.monsterQueue.Dequeue();
                        // gameEngine.MoveMonster( battle, curMonster.xPosition, curMonster.yPosition);
                        if (curMonster.xPosition > 0 && battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition].Equals(""))
                        {
                            battle.battleMapId[curMonster.xPosition - 1, curMonster.yPosition] = battle.battleMapId[curMonster.xPosition, curMonster.yPosition];
                            battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition] = battle.battleMapTop[curMonster.xPosition, curMonster.yPosition];
                            battle.battleMapHP[curMonster.xPosition - 1, curMonster.yPosition] = battle.battleMapHP[curMonster.xPosition, curMonster.yPosition];
                            battle.battleMapId[curMonster.xPosition, curMonster.yPosition] = "";
                            battle.battleMapTop[curMonster.xPosition, curMonster.yPosition] = "";
                            battle.battleMapHP[curMonster.xPosition, curMonster.yPosition] = "";
                            curMonster.xPosition -= 1;
                        }
                        PrintDialog(curMonster.Name + " moved");

                        Character target = CheckNearbyCharacter(curMonster.xPosition, curMonster.yPosition);
                        if (target != null)
                        {
                            //attack
                            ApplyDamageMTC(curMonster, target);
                            // update target to queue
                            if (target.CurrentHP <= 0)
                            {//remove dead monster 
                                RemoveTargetFromQueues(target);
                                battle.battleMapId[target.xPosition, target.yPosition] = "";
                                battle.battleMapTop[target.xPosition, target.yPosition] = "";
                                battle.battleMapHP[target.xPosition, target.yPosition] = "";

                                if (gameEngine.movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
                                {
                                    gameOver = true;
                                }
                            }
                            else
                            {
                                //update character queue with target
                                battle.battleMapHP[target.xPosition, target.yPosition] = target.CurrentHP.ToString();
                                UpdateTargetInQueues(target);
                            }
                        }
                        gameEngine.movedMonsters.Enqueue(curMonster);
                        curMonster = null;
                    }

                }
                else if (gameEngine.characterQueue.Count() == 0)
                {
                    if (gameEngine.monsterQueue.Count() > 0)
                    {
                        // move the rest of the monsters...
                        while (gameEngine.monsterQueue.Count() > 0)
                        {
                            curMonster = gameEngine.monsterQueue.Dequeue();
                            // gameEngine.MoveMonster( battle, curMonster.xPosition, curMonster.yPosition);
                            if (curMonster.xPosition > 0 && battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition].Equals(""))
                            {
                                battle.battleMapId[curMonster.xPosition - 1, curMonster.yPosition] = battle.battleMapId[curMonster.xPosition, curMonster.yPosition];
                                battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition] = battle.battleMapTop[curMonster.xPosition, curMonster.yPosition];
                                battle.battleMapHP[curMonster.xPosition - 1, curMonster.yPosition] = battle.battleMapHP[curMonster.xPosition, curMonster.yPosition];
                                battle.battleMapId[curMonster.xPosition, curMonster.yPosition] = "";
                                battle.battleMapTop[curMonster.xPosition, curMonster.yPosition] = "";
                                battle.battleMapHP[curMonster.xPosition, curMonster.yPosition] = "";
                                curMonster.xPosition -= 1;
                            }
                            PrintDialog(curMonster.Name + " moved");

                            Character target = CheckNearbyCharacter(curMonster.xPosition, curMonster.yPosition);
                            if (target != null)
                            {
                                //attack
                                ApplyDamageMTC(curMonster, target);
                                // update target to queue
                                if (target.CurrentHP <= 0)
                                {//remove dead monster 
                                    RemoveTargetFromQueues(target);
                                    battle.battleMapId[target.xPosition, target.yPosition] = "";
                                    battle.battleMapTop[target.xPosition, target.yPosition] = "";
                                    battle.battleMapHP[target.xPosition, target.yPosition] = "";
                                    if (gameEngine.movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
                                    {
                                        gameOver = true;
                                    }
                                }
                                else
                                {
                                    //update character queue with target
                                    battle.battleMapHP[target.xPosition, target.yPosition] = target.CurrentHP.ToString();
                                    UpdateTargetInQueues(target);
                                }
                            }
                            gameEngine.movedMonsters.Enqueue(curMonster);
                            curMonster = null;
                        }
                    }

                    if (gameEngine.movedCharacters.Count() == 0)
                    {
                        gameOver = true;
                    }
                    else
                    {
                        //transfer moved queue to speed queue
                        BattleScore.TurnCount++;
                        while (gameEngine.movedMonsters.Count() > 0)
                        {
                            gameEngine.monsterQueue.Enqueue(gameEngine.movedMonsters.Dequeue());
                        }
                        while (gameEngine.movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(gameEngine.movedCharacters.Dequeue());
                        }
                    }



                }
                else if (gameEngine.monsterQueue.Count() == 0)
                {
                    if (gameEngine.characterQueue.Count() > 0)
                    {
                        curCharacter = gameEngine.characterQueue.Dequeue();

                        //hackathon changes
                        if (curCharacter.CurrentHP < curCharacter.MaxHP && potionNum > 0)
                        {
                            availablePotion = true;
                        }
                        else
                        {
                            availablePotion = false;
                        }

                        endTurn = false;
                        battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                        battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;
                        battle.battleMapId[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.Id;
                        battle.battleMapHP[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.CurrentHP.ToString();
                        RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);
                        PrintDialog(curCharacter.Name + "'s turn");

                    }

                    if (gameEngine.movedMonsters.Count() == 0)
                    {
                        endRound = true;
                    }
                    else
                    {
                        //transfer moved queue to speed queue
                        BattleScore.TurnCount++;
                        while (gameEngine.movedMonsters.Count() > 0)
                        {
                            gameEngine.monsterQueue.Enqueue(gameEngine.movedMonsters.Dequeue());
                        }
                        while (gameEngine.movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(gameEngine.movedCharacters.Dequeue());
                        }
                    }


                }



                // characterQueue is empty
                if (endRound)
                {
                    AssignItems(battle.itemPool, characterViewModel.Party);
                    //count the scoring
                    BattleScore.RoundCount++;
                    useFocusAtk = false;
                    availableFocusAtk = true;
                    //reconstruct queue and build new battle...
                    battle = new Battle();
                    //take care of CharacterDataset
                    //take care of MonsterDataset
                    //load data

                    GenerateNewMonsters();
                    //LoadDataCommandCharacterOnly.Execute(null);
                    // start with level 1 by default
                    gameEngine.currentRound += 1;

                    InitializeBattle();

                    gameEngine.movedMonsters.Clear();
                    gameEngine.movedCharacters.Clear();


                    //create speedqueue and render map
                    potionNum = 6;
                    allowRoundHealing = true;
                    availablePotion = true;
                    availableFocusAtk = true;

                    // determine weather character or monster
                    MoveFirstCharacterMonster();
                    endRound = false;
                }
                else if (gameOver)
                {
                    PrintDialog("Game Over!");
                    // Set Score
                    BattleScore.ScoreTotal = BattleScore.ExperienceGainedTotal;

                    // Save the Score to the DataStore
                    ScoresViewModel.Instance.AddAsync(BattleScore).GetAwaiter().GetResult();

                    //To keep track of how many games have been palyed
                    GameGlobals.GameCount++;

                    //clear Dialog at the end 
                    gameEngine.ClearDialogCache();

                }
                else
                {

                    if (gameEngine.characterQueue.Count() == 0 && gameEngine.movedCharacters.Count() > 0)
                    {
                        while (gameEngine.movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(gameEngine.movedCharacters.Dequeue());
                        }
                    }

                    curCharacter = gameEngine.characterQueue.Dequeue();

                    //hackathon changes
                    if (curCharacter.CurrentHP < curCharacter.MaxHP && potionNum > 0)
                    {
                        availablePotion = true;
                    }
                    else
                    {
                        availablePotion = false;
                    }

                    endTurn = false;
                    battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                    battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;
                    battle.battleMapId[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.Id;
                    battle.battleMapHP[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.CurrentHP.ToString();

                    RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);
                    PrintDialog(curCharacter.Name + "'s turn");

                }// end of end round

            }// end of end turn

            //_script.scriptCounter = 1;
            //RunScript(_script, 0);
            battle.RefreshAllCell();
        }

        private void AddItemToScore(Item drop)
        {
            BattleScore.ItemsDroppedList += "<Name:";
            BattleScore.ItemsDroppedList += drop.Name;

            BattleScore.ItemsDroppedList += " Type:";
            BattleScore.ItemsDroppedList += drop.Type;
            BattleScore.ItemsDroppedList += " HP:";
            BattleScore.ItemsDroppedList += drop.HP;
            BattleScore.ItemsDroppedList += " MP:";
            BattleScore.ItemsDroppedList += drop.MP;
            BattleScore.ItemsDroppedList += " Lvl:";
            BattleScore.ItemsDroppedList += drop.Lvl;
            BattleScore.ItemsDroppedList += " Def:";
            BattleScore.ItemsDroppedList += drop.Def;
            BattleScore.ItemsDroppedList += " Atk:";
            BattleScore.ItemsDroppedList += drop.Atk;
            BattleScore.ItemsDroppedList += " Spd:";
            BattleScore.ItemsDroppedList += drop.Spd;
            BattleScore.ItemsDroppedList += " Luk:";
            BattleScore.ItemsDroppedList += drop.Luk;
            BattleScore.ItemsDroppedList += " SpecialAbility:";
            BattleScore.ItemsDroppedList += drop.SpecialAbility;
            BattleScore.ItemsDroppedList += " EquippableLocation:";
            BattleScore.ItemsDroppedList += drop.EquippableLocation;

            BattleScore.ItemsDroppedList += " ImgSource:";
            BattleScore.ItemsDroppedList += drop.ImgSource;
            BattleScore.ItemsDroppedList += " Range:";
            BattleScore.ItemsDroppedList += drop.Range;
            BattleScore.ItemsDroppedList += " Value:";
            BattleScore.ItemsDroppedList += drop.Value;
            BattleScore.ItemsDroppedList += ">\n";
        }

        public enum HitStatusEnum
        {
            Unknown = 0,
            Miss = 1,
            CriticalMiss = 10,
            Hit = 5,
            CriticalHit = 15
        }

        // render all characters and monsters to the map
        private void RenderCharactersMonsters()
        {
            foreach (Character c in gameEngine.characterQueue)
            {
                battle.battleMapTop[c.xPosition, c.yPosition] = c.ImgSource;
                battle.battleMapId[c.xPosition, c.yPosition] = c.Id;
                battle.battleMapHP[c.xPosition, c.yPosition] = c.CurrentHP.ToString();
            }
            foreach (Monster m in gameEngine.monsterQueue)
            {
                battle.battleMapTop[m.xPosition, m.yPosition] = m.ImgSource;
                battle.battleMapId[m.xPosition, m.yPosition] = m.Id;
                battle.battleMapHP[m.xPosition, m.yPosition] = m.CurrentHP.ToString();
            }
        }

        private void RenderMoveAttackRange(int x, int y, int totalR, int atkR)
        {
            // battle.battleMapSelection[x, y] = Battle.HIGHLIGHTGREEN;
            if (x > 0)
            {
                if (totalR > atkR)
                {
                    battle.battleMapSelection[x - 1, y] = Battle.HIGHLIGHTGREEN;
                    if (totalR > 1) { RenderMoveAttackRange(x - 1, y, totalR - 1, atkR); }

                }
                //else if ( battle.battleMapSelection[x - 1, y].Equals(""))
                else if (!battle.battleMapSelection[x - 1, y].Equals(Battle.HIGHLIGHTGREEN))
                {
                    battle.battleMapSelection[x - 1, y] = Battle.HIGHLIGHTRED;
                    if (totalR > 1) { RenderMoveAttackRange(x - 1, y, totalR - 1, atkR); }
                }
            }
            if (x < 4)
            {
                if (totalR > atkR)
                {
                    battle.battleMapSelection[x + 1, y] = Battle.HIGHLIGHTGREEN;
                    if (totalR > 1) { RenderMoveAttackRange(x + 1, y, totalR - 1, atkR); }
                }
                //else if ( battle.battleMapSelection[x + 1, y].Equals(""))
                else if (!battle.battleMapSelection[x + 1, y].Equals(Battle.HIGHLIGHTGREEN))
                {
                    battle.battleMapSelection[x + 1, y] = Battle.HIGHLIGHTRED;
                    if (totalR > 1) { RenderMoveAttackRange(x + 1, y, totalR - 1, atkR); }
                }
            }
            if (y > 0)
            {
                if (totalR > atkR)
                {
                    battle.battleMapSelection[x, y - 1] = Battle.HIGHLIGHTGREEN;
                    if (totalR > 1) { RenderMoveAttackRange(x, y - 1, totalR - 1, atkR); }
                }
                else if (!battle.battleMapSelection[x, y - 1].Equals(Battle.HIGHLIGHTGREEN))

                {
                    battle.battleMapSelection[x, y - 1] = Battle.HIGHLIGHTRED;
                    if (totalR > 1) { RenderMoveAttackRange(x, y - 1, totalR - 1, atkR); }
                }
            }
            if (y < 5)
            {
                if (totalR > atkR)
                {
                    battle.battleMapSelection[x, y + 1] = Battle.HIGHLIGHTGREEN;
                    if (totalR > 1) { RenderMoveAttackRange(x, y + 1, totalR - 1, atkR); }
                }
                else if (!battle.battleMapSelection[x, y + 1].Equals(Battle.HIGHLIGHTGREEN))

                {
                    battle.battleMapSelection[x, y + 1] = Battle.HIGHLIGHTRED;
                    if (totalR > 1) { RenderMoveAttackRange(x, y + 1, totalR - 1, atkR); }
                }
            }



        }


        public void PrintDialog(string str)
        {
            for (int i = 4; i > 0; i--)
            {
                gameEngine.DialogCache[i] = gameEngine.DialogCache[i - 1];
            }
            gameEngine.DialogCache[0] = str;
            gameEngine.ConsoleDialog1 = gameEngine.DialogCache[4]
            + "\n"
            + gameEngine.DialogCache[3] + "\n"
            + gameEngine.DialogCache[2] + "\n"
            + gameEngine.DialogCache[1] + "\n"
            + gameEngine.DialogCache[0];
        }

        public void UsePotion()
        {
            if (curCharacter != null && potionNum > 0 && curCharacter.CurrentHP < curCharacter.MaxHP)
            {
                curCharacter.CurrentHP = curCharacter.MaxHP;
                battle.battleMapHP[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.CurrentHP.ToString();
                potionNum -= 1;
                PrintDialog("Potion Used! Max HP Baby! Potion left:" + potionNum);
            }
            if (potionNum == 0)
            {
                availablePotion = false;
            }

            battle.RefreshAllCell();


        }

        public void UseFocusAtk()
        {
            if (curCharacter.EquippedItem.Count() > 0)
            {
                availableFocusAtk = false;
                useFocusAtk = true;
            }

        }

        /*public void Reset_Clicked(object sender, EventArgs e)
        {
            InitMonsterQueue();
            InitCharacterQueue();
            LoadDataCommand.Execute(null);

            gameEngine.ClearDialogCache();
            _script.scriptCounter = 1;
            // do something
            // Data.battle.battleMapTop[0, 0] = "KnightRight.png";
            battle.SetAllSelection("HighlightGrey.png");
            battle.SetAllBackground("Grass.png");
            battle.SetAllTop("");
            battle.SetAllHP("");

            foreach (var c in CharacterDataset)
            {
                battle.battleMapTop[c.xPosition, c.yPosition] = c.ImgSource;
                battle.battleMapId[c.xPosition, c.yPosition] = c.Id;
                battle.battleMapHP[c.xPosition, c.yPosition] = c.CurrentHP.ToString();
                // battle.b
            }

            foreach (var m in MonsterDataset)
            {
                battle.battleMapTop[m.xPosition, m.yPosition] = m.ImgSource;
                battle.battleMapId[m.xPosition, m.yPosition] = m.Id;
                battle.battleMapHP[m.xPosition, m.yPosition] = m.CurrentHP.ToString();
            }

            battle.RefreshAllCell();
            PrintDialog("Reset Clicked");

            BindingContext = null;
            BindingContext = _viewModel;
        }*/



        private void MoveFirstCharacterMonster()
        {
            if (gameEngine.characterQueue.Peek().Spd >= gameEngine.monsterQueue.Peek().Spd)
            {
                // character turn dequeue and hold dont enqueue.
                curCharacter = gameEngine.characterQueue.Dequeue();

                //hackathon changes
                if (curCharacter.CurrentHP < curCharacter.MaxHP && potionNum > 0)
                {
                    availablePotion = true;
                }
                else
                {
                    availablePotion = false;
                }

                battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                //highlight current character
                battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;
                // function that takes characters move range and attack range and update to screen.
                RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);
            }
            else
            {
                // monster turn
                while (gameEngine.monsterQueue.Count > 0 && (gameEngine.characterQueue.Peek().Spd < gameEngine.monsterQueue.Peek().Spd))
                {
                    //curMonster =  MonsterDataset.Where(x => x.ID ==  gameEngine.monsterQueue.Peek().ID).First();

                    curMonster = gameEngine.monsterQueue.Dequeue();

                    // if there is character nearby attack!
                    Character target = CheckNearbyCharacter(curMonster.xPosition, curMonster.yPosition);
                    if (target != null)
                    {
                        //attack
                        ApplyDamageMTC(curMonster, target);
                        if (target.CurrentHP <= 0)
                        {

                            //if(Random && maigcRevive){
                            //set to full
                            //magicRevive = false;
                            //}
                            //else{
                            RemoveTargetFromQueues(target);
                            //also need to set livestatus to dead in dataset
                            battle.battleMapId[target.xPosition, target.yPosition] = "";
                            battle.battleMapTop[target.xPosition, target.yPosition] = "";
                            battle.battleMapHP[target.xPosition, target.yPosition] = "";
                            //}
                            if (gameEngine.movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
                            {
                                gameOver = true;
                            }

                        }
                        else
                        {
                            battle.battleMapHP[target.xPosition, target.yPosition] = target.CurrentHP.ToString();
                            UpdateTargetInQueues(target);
                        }

                    }
                    else
                    {
                        //move
                        //move left and right
                        if (curMonster.xPosition > 0 && battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition].Equals(""))
                        {


                            battle.battleMapTop[curMonster.xPosition, curMonster.yPosition] = "";
                            battle.battleMapId[curMonster.xPosition, curMonster.yPosition] = "";
                            battle.battleMapHP[curMonster.xPosition, curMonster.yPosition] = "";

                            battle.battleMapTop[curMonster.xPosition - 1, curMonster.yPosition] = curMonster.ImgSource;
                            battle.battleMapId[curMonster.xPosition - 1, curMonster.yPosition] = curMonster.Id;
                            battle.battleMapHP[curMonster.xPosition - 1, curMonster.yPosition] = curMonster.CurrentHP.ToString();

                            curMonster.xPosition = curMonster.xPosition - 1;
                            curMonster.yPosition = curMonster.yPosition;

                            target = CheckNearbyCharacter(curMonster.xPosition, curMonster.yPosition);
                            if (target != null)
                            {
                                //attack
                                ApplyDamageMTC(curMonster, target);
                                if (target.CurrentHP <= 0)
                                {

                                    RemoveTargetFromQueues(target);
                                    //also need to set livestatus to dead in dataset
                                    battle.battleMapId[target.xPosition, target.yPosition] = "";
                                    battle.battleMapTop[target.xPosition, target.yPosition] = "";
                                    battle.battleMapHP[target.xPosition, target.yPosition] = "";

                                    if (gameEngine.movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
                                    {
                                        gameOver = true;
                                    }
                                }
                                else
                                {
                                    battle.battleMapHP[target.xPosition, target.yPosition] = target.CurrentHP.ToString();
                                    UpdateTargetInQueues(target);
                                }

                            }

                        }
                    }
                    gameEngine.movedMonsters.Enqueue(curMonster);
                    curMonster = null;
                }
            }
        }

        // Remove target from both speed queue and moved queue
        private void RemoveTargetFromQueues(Monster target)
        {
            Queue<Monster> tmp1 = new Queue<Monster>();
            Queue<Monster> tmp2 = new Queue<Monster>();
            Monster tmpM = new Monster();
            while (gameEngine.monsterQueue.Count() > 0)
            {
                tmpM = gameEngine.monsterQueue.Dequeue();
                if (tmpM.Id.Equals(target.Id))
                {
                    tmpM = null;
                }
                else
                {
                    tmp1.Enqueue(tmpM);
                }
            }
            while (gameEngine.movedMonsters.Count() > 0)
            {
                tmpM = gameEngine.movedMonsters.Dequeue();
                if (tmpM.Id.Equals(target.Id))
                {
                    tmpM = null;
                }
                else
                {
                    tmp2.Enqueue(tmpM);
                }
            }
            while (tmp1.Count() > 0)
            {
                tmpM = tmp1.Dequeue();
                gameEngine.monsterQueue.Enqueue(tmpM);

            }
            while (tmp2.Count() > 0)
            {
                tmpM = tmp2.Dequeue();
                gameEngine.movedMonsters.Enqueue(tmpM);
            }
        }

        // Remove target from both speed queue and moved queue
        private void RemoveTargetFromQueues(Character target)
        {
            Queue<Character> tmp1 = new Queue<Character>();
            Queue<Character> tmp2 = new Queue<Character>();
            Character tmpC = new Character();
            while (gameEngine.characterQueue.Count() > 0)
            {
                tmpC = gameEngine.characterQueue.Dequeue();
                if (tmpC.Id.Equals(target.Id))
                {
                    charViewModel.Party.Remove(tmpC);
                    tmpC = null;
                }
                else
                {
                    tmp1.Enqueue(tmpC);
                }
            }
            while (gameEngine.movedCharacters.Count() > 0)
            {
                tmpC = gameEngine.movedCharacters.Dequeue();
                if (tmpC.Id.Equals(target.Id))
                {
                    charViewModel.Party.Remove(tmpC);
                    tmpC = null;
                }
                else
                {
                    tmp2.Enqueue(tmpC);
                }
            }
            while (tmp1.Count() > 0)
            {
                tmpC = tmp1.Dequeue();
                gameEngine.characterQueue.Enqueue(tmpC);

            }
            while (tmp2.Count() > 0)
            {
                tmpC = tmp2.Dequeue();
                gameEngine.movedCharacters.Enqueue(tmpC);
            }



        }

        // Update target from both speed queue and moved queue
        private void UpdateTargetInQueues(Character target)
        {
            Queue<Character> tmp1 = new Queue<Character>();
            Queue<Character> tmp2 = new Queue<Character>();
            Character tmpC = new Character();
            while (gameEngine.characterQueue.Count() > 0)
            {
                tmpC = gameEngine.characterQueue.Dequeue();
                if (tmpC.Id.Equals(target.Id))
                {
                    tmp1.Enqueue(target);
                }
                else
                {
                    tmp1.Enqueue(tmpC);
                }
            }
            while (gameEngine.movedCharacters.Count() > 0)
            {
                tmpC = gameEngine.movedCharacters.Dequeue();
                if (tmpC.Id.Equals(target.Id))
                {
                    tmp2.Enqueue(target);
                }
                else
                {
                    tmp2.Enqueue(tmpC);
                }
            }
            while (tmp1.Count() > 0)
            {
                tmpC = tmp1.Dequeue();
                gameEngine.characterQueue.Enqueue(tmpC);

            }
            while (tmp2.Count() > 0)
            {
                tmpC = tmp2.Dequeue();
                gameEngine.movedCharacters.Enqueue(tmpC);
            }
        }

        // Update target from both speed queue and moved queue
        private void UpdateTargetInQueues(Monster target)
        {
            Queue<Monster> tmp1 = new Queue<Monster>();
            Queue<Monster> tmp2 = new Queue<Monster>();
            Monster tmpM = new Monster();
            while (gameEngine.monsterQueue.Count() > 0)
            {
                tmpM = gameEngine.monsterQueue.Dequeue();
                if (tmpM.Id.Equals(target.Id))
                {
                    tmp1.Enqueue(target);
                }
                else
                {
                    tmp1.Enqueue(tmpM);
                }
            }
            while (gameEngine.movedMonsters.Count() > 0)
            {
                tmpM = gameEngine.movedMonsters.Dequeue();
                if (tmpM.Id.Equals(target.Id))
                {
                    tmp2.Enqueue(target);
                }
                else
                {
                    tmp2.Enqueue(tmpM);
                }
            }
            while (tmp1.Count() > 0)
            {
                tmpM = tmp1.Dequeue();
                gameEngine.monsterQueue.Enqueue(tmpM);

            }
            while (tmp2.Count() > 0)
            {
                tmpM = tmp2.Dequeue();
                gameEngine.movedMonsters.Enqueue(tmpM);
            }
        }

        // Apply damage -> Assign Item if dropped
        // If creature dies, remove it
        // Update creature and print dialog
        private void ApplyDamageMTC(Monster m, Character c)
        {//def need to be in calculation
            int dmg = (int)Math.Ceiling(m.Atk / 4.0);
            c.TakeDamage(dmg);
            if (!BattleScore.AutoBattle)
            {
                PrintDialog(c.Name + " took " + dmg + " damage!");
            }



            if (c.CurrentHP <= 0 && magicRevive && magicReviveTarget == magicReviveRandom.Next(1, 7))
            {
                magicRevive = false;
                c.CurrentHP = c.MaxHP;
                if (!BattleScore.AutoBattle)
                {
                    PrintDialog(c.Name + " is so lucky he revived from dead!");
                }


                c.LiveStatus = true;
            }

            if (!c.LiveStatus)
            {
                BattleScore.AddCharacterToList(c);
                //BattleScore.CharacterAtDeathList += c.FormatOutput() + "\n";

                // Mike please read here, this are the changes for item drop 
                // when character dies
                if (c.EquippedItem.Count() > 0)
                {
                    foreach (var pair in c.EquippedItem)
                        battle.itemPool = c.RemoveItem(battle.itemPool, pair.Key);
                    c.EquippedItem.Clear();
                }
            }
        }

        // Apply damage -> Assign Item if dropped
        // If creature dies, remove it
        // Update creature and print dialog
        private void ApplyDamageCTM(Character c, Monster m)
        {
            int exp = 0;
            int dmg = c.Lvl + c.GetAttack();

            if (useFocusAtk)
            {
                ExecuteFocusAtk(c);
                dmg *= 2;
                PrintDialog("Focus Atttack! " + dmg + "Damage!");
            }
            //Calculate miss/dodge based on calculation 
            dmg = CriticalOrMissCTM(c, m, dmg);
            //calculate experience based on damage
            exp = m.CalculateExperienceEarned(dmg);
            //Add it to score at the end of each attack
            BattleScore.ExperienceGainedTotal += exp;

            //when damage is higher than 0, monster takes the damage
            if (dmg >= 0)
            {
                //Instead of calculating damage here directly, i am using m.TakeDamage function. It also changes LiveStatus from true to false if CurrentHP < 0 
                //m.CurrentHP -= dmg;
                m.TakeDamage(dmg);
                if (!BattleScore.AutoBattle)
                {
                    PrintDialog(m.Name + " took " + dmg + " damage!" + "\n"
                            + "Monster LiveStatus: " + m.LiveStatus);
                }


                //check livestatus, if the monster is dead, do all the scoring work here
                if (!m.LiveStatus)
                {
                    if (!BattleScore.AutoBattle)
                    {
                        PrintDialog(m.Name + " is dead!!!");
                    }
                    battle.itemPool.Add(dropItem(m.Id));
                    BattleScore.MonstersKilledList += m.FormatOutput() + "\n";
                    BattleScore.MonsterSlainNumber++;
                }

                // level up and experience earning functionalities
                int curLevel = c.Lvl;
                c.AddExperience(exp);
                if (!BattleScore.AutoBattle)
                {
                    PrintDialog(c.Name + " Earned " + exp + " experience!");
                }

                int updatedLevel = c.Lvl;
                if (updatedLevel > curLevel)
                {
                    if (!BattleScore.AutoBattle)
                    {
                        PrintDialog(c.Name + " has leveled up! " + c.Name + " is now Lvl:" + c.Lvl);
                    }
                }


            }
            //the dmg will be negative number in case it is critical miss 
            else
            {
                dmg = Math.Abs(dmg);
                c.TakeDamageCriticalMiss(dmg);
                if (c.CurrentHP != 1)
                {
                    if (!BattleScore.AutoBattle)
                    {
                        PrintDialog(c.Name + " took " + dmg + " damage !");
                    }

                }
                else
                {
                    if (!BattleScore.AutoBattle)
                    {
                        PrintDialog(c.Name + " took its own attack. " + c.Name + "is barely alive!");
                    }

                }


            }






        }

        private void ExecuteFocusAtk(Character c)
        {
            string minValueItemGUID = "";
            int minValue = 100;
            foreach (var i in c.EquippedItem)
            {
                if (i.Value.Value < minValue)
                {
                    minValue = i.Value.Value;
                    minValueItemGUID = i.Value.Guid;
                }
            }
            c.EquippedItem.Remove(c.EquippedItem.Where(rm => rm.Value.Guid.Equals(minValueItemGUID)).First().Value.Location);
        }

        // Calculate attack and defense and compare them, beased on result
        // execute critical or miss attak
        public int CriticalOrMissCTM(Character Attacker, Monster Defender, int damage)
        {
            //attackscore = level + base attack + level attack bonus + level item attack modifier
            var AttackFinal = Attacker.Lvl + Attacker.GetAttack();

            //DefenseScore =  
            var DefenseFinal = Defender.Def + Defender.Lvl;

            //returns whether it was critical hit or miss
            var HitSuccess = RollToHitTarget(AttackFinal, DefenseFinal);

            //if it was critical hit, the damage will be doubled 
            if (HitSuccess == HitStatusEnum.CriticalHit)
            {
                damage = damage * 2;
                if (!BattleScore.AutoBattle)
                {
                    PrintDialog("The attack is a critical hit (2x damage)!");
                }

            }
            //if it was miss, damage will be decreased to 0
            else if (HitSuccess == HitStatusEnum.Miss)
            {
                damage = 0;
                if (!BattleScore.AutoBattle)
                {
                    PrintDialog("The attack is a miss!");
                }

            }
            else if (HitSuccess == HitStatusEnum.CriticalMiss)
            {
                //critical miss = negative damage
                int var = damage * 2;
                damage -= var;
                if (!BattleScore.AutoBattle)
                {
                    PrintDialog("The attack is a critical miss!");
                }

            }

            return damage;
        }

        // Calculate attack and defense and compare them, beased on result
        // execute critical or miss attak
        public int CriticalOrMissMTC(Monster Attacker, Character Defender, int damage)
        {
            //attackscore = level + base attack + level attack bonus + level item attack modifier
            var AttackScore = Attacker.Lvl + Attacker.GetAttack();

            //DefenseScore =  
            var DefenseScore = Defender.Def + Defender.Lvl;

            //returns whether it was critical hit or miss
            var HitSuccess = RollToHitTarget(AttackScore, DefenseScore);

            //if it was critical hit, the damage will be doubled 
            if (HitSuccess == HitStatusEnum.CriticalHit)
            {
                damage = damage * 2;
                PrintDialog("The attack is a critical hit (2x damage)!");
            }
            //if it was miss, damage will be decreased to 0
            else if (HitSuccess == HitStatusEnum.Miss)
            {
                damage = 0;
                PrintDialog("The attack is a miss!");
            }
            else if (HitSuccess == HitStatusEnum.CriticalMiss)
            {
                //critical miss = negative damage
                int var = damage * 2;
                damage -= var;
                PrintDialog("The attack is a critical miss!");
            }

            return damage;
        }

        // This method roll dice and determine hit or not
        public HitStatusEnum RollToHitTarget(int AttackScore, int DefenseScore)
        {

            var d20 = HelperEngine.RollDice(1, 20);
            var tempDefenseScore = 0;

            // Turn On UnitTestingSetRoll
            if (GameGlobals.EnableCriticalHitDamage)
            {
                //set the die to 20 to force critical hit
                d20 = 20;
                GameGlobals.EnableMiss = false;
            }
            else if (GameGlobals.EnableMiss)
            {
                d20 = 1;

                //force the defense score to be high so that defense > offense happens 
                tempDefenseScore = int.MaxValue;
                var temp = 0;
                temp = DefenseScore;
                DefenseScore = tempDefenseScore;
                tempDefenseScore = temp;

                //making sure that critical boolean is false since miss is on 
                //Only one situation can happen, not both at the same time 
                GameGlobals.EnableCriticalHitDamage = false;
            }
            else if (GameGlobals.EnableCriticalMissProblems)
            {
                d20 = 1;
            }

            if (d20 == 1)
            {
                // Force Miss
                HitStatus = HitStatusEnum.CriticalMiss;
                return HitStatus;
            }

            if (d20 == 20)
            {
                // Force Hit
                HitStatus = HitStatusEnum.CriticalHit;
                return HitStatus;
            }

            var ToHitScore = d20 + AttackScore;
            if (ToHitScore < DefenseScore)
            {
                AttackStatus = " misses ";
                // Miss
                HitStatus = HitStatusEnum.Miss;
                DamageAmount = 0;
                //swap back the defensescore 
                var temp = 0;
                temp = DefenseScore;
                DefenseScore = tempDefenseScore;
                tempDefenseScore = temp;
                Console.WriteLine(tempDefenseScore + " " + DefenseScore);
            }
            else
            {
                // Hit
                HitStatus = HitStatusEnum.Hit;
            }

            return HitStatus;
        }

        // Retirns the dropped Item from monster by checking monsterID
        public Item dropItem(string monsterID)
        {
            // read monster from Dataset,
            var monster = monViewModel.monsterParty.Where(x => x.Id.Equals(monsterID)).First();
            //return itemViewModel.Where(i => i.Guid.Equals(result.UniqueDropID)).First();
            var result = itemViewModel.Dataset.Where(i => i.Guid.Equals(monster.UniqueDropID)).First();
            return result;
        }

        // Read the map and try to find nearby character 
        private Character CheckNearbyCharacter(int x, int y)
        {
            if (x > 0 && gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x - 1, y])).Count() > 0)
            {
                return gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x - 1, y])).First();
            }
            if (x > 0 && gameEngine.movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x - 1, y])).Count() > 0)
            {
                return gameEngine.movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x - 1, y])).First();
            }
            if (y > 0 && gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x, y - 1])).Count() > 0)
            {
                return gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x, y - 1])).First();
            }
            if (y > 0 && gameEngine.movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x, y - 1])).Count() > 0)
            {
                return gameEngine.movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x, y - 1])).First();
            }
            if (x < 4 && gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x + 1, y])).Count() > 0)
            {
                return gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x + 1, y])).First();
            }
            if (x < 4 && gameEngine.movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x + 1, y])).Count() > 0)
            {
                return gameEngine.movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x + 1, y])).First();
            }
            if (y < 5 && gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x, y + 1])).Count() > 0)
            {
                return gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x, y + 1])).First();
            }
            if (y < 5 && gameEngine.movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x, y + 1])).Count() > 0)
            {
                return gameEngine.movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x, y + 1])).First();
            }
            return null;
        }

        // Get any alive monster, this method is for AUTO BATTLE
        private Monster GetMonster()
        {
            if (gameEngine.monsterQueue.Count() > 0)
            {
                return gameEngine.monsterQueue.First();
            }
            if (gameEngine.movedMonsters.Count() > 0)
            {
                return gameEngine.movedMonsters.First();
            }

            return null;
        }

        // Get any alive character, this method is for AUTO BATTLE
        private Character GetCharacter()
        {
            if (gameEngine.characterQueue.Count() > 0)
            {
                return gameEngine.characterQueue.First();
            }
            if (gameEngine.movedCharacters.Count() > 0)
            {
                return gameEngine.movedCharacters.First();
            }

            return null;
        }

        // Read the map and try to find nearby monster 
        private bool CheckNearbyMonster(int x, int y)
        {

            if (x > 0 && (gameEngine.monsterQueue.Where(m => m.Id.Equals(battle.battleMapId[x - 1, y])).Count() > 0
                          || gameEngine.movedMonsters.Where(m => m.Id.Equals(battle.battleMapId[x - 1, y])).Count() > 0))
            {
                return true;

            }
            if (x < 4 && (gameEngine.monsterQueue.Where(m => m.Id.Equals(battle.battleMapId[x + 1, y])).Count() > 0
                          || gameEngine.movedMonsters.Where(m => m.Id.Equals(battle.battleMapId[x + 1, y])).Count() > 0))
            {
                return true;
            }
            if (y > 0 && (gameEngine.monsterQueue.Where(m => m.Id.Equals(battle.battleMapId[x, y - 1])).Count() > 0
                          || gameEngine.movedMonsters.Where(m => m.Id.Equals(battle.battleMapId[x, y - 1])).Count() > 0))
            {
                return true;
            }
            if (y < 5 && (gameEngine.monsterQueue.Where(m => m.Id.Equals(battle.battleMapId[x, y + 1])).Count() > 0
                          || gameEngine.movedMonsters.Where(m => m.Id.Equals(battle.battleMapId[x, y + 1])).Count() > 0))
            {
                return true;
            }
            return false;
        }

        // Simply render attack range
        private void RenderAttackRange(int x, int y, int atkRange)
        {
            // battle.battleMapSelection[x, y] = Battle.HIGHLIGHTGREEN;
            if (x > 0)
            {

                battle.battleMapSelection[x - 1, y] = Battle.HIGHLIGHTRED;
                if (atkRange > 1) { RenderAttackRange(x - 1, y, atkRange - 1); }


            }
            if (x < 4)
            {

                battle.battleMapSelection[x + 1, y] = Battle.HIGHLIGHTRED;
                if (atkRange > 1) { RenderAttackRange(x + 1, y, atkRange - 1); }


            }
            if (y > 0)
            {

                battle.battleMapSelection[x, y - 1] = Battle.HIGHLIGHTRED;
                if (atkRange > 1) { RenderAttackRange(x, y - 1, atkRange - 1); }

            }
            if (y < 5)
            {

                battle.battleMapSelection[x, y + 1] = Battle.HIGHLIGHTRED;
                if (atkRange > 1) { RenderAttackRange(x, y + 1, atkRange - 1); }

            }
        }

        // when a new round begins, reset battle map
        public void InitializeBattle()
        {

            int round = gameEngine.currentRound;
            battle.itemPool = new List<Item>();
            battle.SetAllSelection(Battle.HIGHLIGHTGREY);
            battle.SetAllTop("");
            battle.SetAllId("");
            battle.SetAllHP("");
            if (round <= 3)
            {
                battle.title = "Forest Lvl " + round.ToString();
                battle.SetAllBackground(Battle.GRASS);
            }
            else if (round > 3 && round <= 6)
            {
                battle.title = "Mountain Lvl " + round.ToString();
                battle.SetAllBackground(Battle.SNOW);
            }
            else if (round > 6 && round <= 9)
            {
                battle.title = "Desert Lvl " + round.ToString();
                battle.SetAllBackground(Battle.SAND);
            }
            else
            {
                battle.title = "Lava Cave Lvl " + round.ToString();
                battle.SetAllBackground(Battle.LAVA);
            }
            //take care of dataset
            InitMonsterQueue();
            InitCharacterQueue(charViewModel);
            RenderCharactersMonsters();
        }

        //it checks to see whether we have enough characters in database 
        public Boolean HaveEnoughCharacters()
        {
            if (CharacterDataset.Count < 6)
            {
                return false;
            }

            return true;
        }

        //it checks to see there is at least one monster in database 
        public Boolean HaveEnoughMonsters()
        {
            if (MonsterDataset.Count < 1)
            {
                return false;
            }

            return true;
        }

        //it checks to see there is at least one monster in database 
        public Boolean HaveEnoughItems()
        {
            if (ItemDataset.Count < 1)
            {
                return false;
            }

            return true;
        }

        // AUTO BATTLE!!!
        public int AutoBattle()
        {
            gameEngine.currentRound = 1;
            InitAutoBattle();
            CharacterDataset = charViewModel.Dataset;
            //CharacterDataset = new ObservableCollection<Character>();
            MonsterDataset = monViewModel.Dataset;
            ItemDataset = itemViewModel.Dataset;
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
            LoadDataCommand.Execute(null);


            //check characters numbers 
            var EnoughCharacter = HaveEnoughCharacters();

            if (!EnoughCharacter)
            {
                return -1;
            }

            //check Monster numbers 
            var EnoughMonster = HaveEnoughMonsters();

            if (!EnoughMonster)
            {
                return -2;
            }

            //check item numbers 
            var EnoughItem = HaveEnoughItems();

            if (!EnoughItem)
            {
                return -3;
            }




            //change generate new characters 
            GenerateNewCharacters();

            //charViewModel.Party.Clear();
            //for (int i = 0; i < 6; i++)
            //{
            //    Character c = new Character();
            //    c = CharacterDataset[i];
            //    charViewModel.Party.Add(c);
            //}
            GenerateNewMonsters();
            InitializeBattle();
            // read sqldatabase or mockdatabase
            //LoadDataCommand.Execute(null);
            // start with level 1 by default

            //create speedqueue and render map

            potionNum = 6;
            allowRoundHealing = true;
            availablePotion = true;
            availableFocusAtk = true;

            if (gameEngine.characterQueue.Peek().Spd >= gameEngine.monsterQueue.Peek().Spd)
            {
                // character turn dequeue and hold dont enqueue.
                curCharacter = gameEngine.characterQueue.Dequeue();
            }
            else
            {
                // monster turn
                while (gameEngine.monsterQueue.Count > 0 && (gameEngine.characterQueue.Peek().Spd < gameEngine.monsterQueue.Peek().Spd))
                {
                    curMonster = gameEngine.monsterQueue.Dequeue();
                    Character target = GetCharacter();
                    if (target != null)
                    {
                        //attack
                        ApplyDamageMTC(curMonster, target);
                        if (target.CurrentHP <= 0)
                        {
                            RemoveTargetFromQueues(target);
                        }
                        else
                        {
                            UpdateTargetInQueues(target);
                        }
                    }
                    else
                    {
                        gameOver = true;
                    }
                    gameEngine.movedMonsters.Enqueue(curMonster);
                    curMonster = null;
                }
                // character turn dequeue and hold dont enqueue.
                curCharacter = gameEngine.characterQueue.Dequeue();

            }
            int counter = 0;

            while (!gameOver)
            {
                Monster m = GetMonster();
                // decrease target HP by = level attack + weapon attack  MIKE PLESASE READ HERE
                ApplyDamageCTM(curCharacter, m);



                // gameEngine.ConsoleDialog1 = m.CurrentHP.ToString();
                if (m.CurrentHP <= 0)
                {
                    if (m.UniqueDropID != null)
                    {
                        Item drop = ItemDataset.Where(item => item.Guid.Equals(m.UniqueDropID)).First();
                        //curCharacter.EquipItem(drop, drop.Location);
                        AddItemToScore(drop);
                    }

                    RemoveTargetFromQueues(m);

                    if (gameEngine.movedMonsters.Count() == 0 && gameEngine.monsterQueue.Count() == 0)
                    {
                        endRound = true;
                    }
                }
                else
                {
                    UpdateTargetInQueues(m);

                }

                gameEngine.movedCharacters.Enqueue(curCharacter);
                curCharacter = null;

                // gameEngine.characterQueue.Enqueue( curCharacter);
                // curCharacter = null;
                if (gameEngine.characterQueue.Count > 0 && gameEngine.monsterQueue.Count > 0)
                {
                    // monster turn
                    while ((gameEngine.characterQueue.Count > 0 && gameEngine.monsterQueue.Count > 0) &&
                           (gameEngine.characterQueue.Peek().Spd < gameEngine.monsterQueue.Peek().Spd))
                    {
                        curMonster = gameEngine.monsterQueue.Dequeue();


                        Character target = GetCharacter();
                        if (target != null)
                        {
                            //attack
                            ApplyDamageMTC(curMonster, target);
                            // update target to queue
                            if (target.CurrentHP <= 0)
                            {//remove dead monster 
                                RemoveTargetFromQueues(target);

                                if (gameEngine.movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
                                {
                                    gameOver = true;
                                }
                            }
                            else
                            {
                                //update character queue with target
                                UpdateTargetInQueues(target);
                            }
                        }
                        gameEngine.movedMonsters.Enqueue(curMonster);
                        curMonster = null;
                    }

                }
                else if (gameEngine.characterQueue.Count() == 0)
                {
                    if (gameEngine.monsterQueue.Count() > 0)
                    {
                        // move the rest of the monsters...
                        while (gameEngine.monsterQueue.Count() > 0)
                        {
                            curMonster = gameEngine.monsterQueue.Dequeue();


                            Character target = GetCharacter();
                            if (target != null)
                            {
                                //attack
                                ApplyDamageMTC(curMonster, target);
                                // update target to queue
                                if (target.CurrentHP <= 0)
                                {//remove dead monster 
                                    RemoveTargetFromQueues(target);

                                    if (gameEngine.movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
                                    {
                                        gameOver = true;
                                    }
                                }
                                else
                                {
                                    //update character queue with target
                                    UpdateTargetInQueues(target);
                                }
                            }
                            gameEngine.movedMonsters.Enqueue(curMonster);
                            curMonster = null;
                        }
                    }

                    if (gameEngine.movedCharacters.Count() == 0)
                    {
                        gameOver = true;
                    }
                    else
                    {
                        //transfer moved queue to speed queue
                        BattleScore.TurnCount++;
                        while (gameEngine.movedMonsters.Count() > 0)
                        {
                            gameEngine.monsterQueue.Enqueue(gameEngine.movedMonsters.Dequeue());
                        }
                        while (gameEngine.movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(gameEngine.movedCharacters.Dequeue());
                        }
                    }



                }
                else if (gameEngine.monsterQueue.Count() == 0)
                {
                    if (gameEngine.characterQueue.Count() > 0)
                    {
                        curCharacter = gameEngine.characterQueue.Dequeue();

                        //hackathon changes
                        if (curCharacter.CurrentHP < curCharacter.MaxHP && potionNum > 0)
                        {
                            availablePotion = true;
                        }
                        else
                        {
                            availablePotion = false;
                        }

                        endTurn = false;

                    }

                    if (gameEngine.movedMonsters.Count() == 0)
                    {
                        endRound = true;
                    }
                    else
                    {
                        //transfer moved queue to speed queue
                        BattleScore.TurnCount++;
                        while (gameEngine.movedMonsters.Count() > 0)
                        {
                            gameEngine.monsterQueue.Enqueue(gameEngine.movedMonsters.Dequeue());
                        }
                        while (gameEngine.movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(gameEngine.movedCharacters.Dequeue());
                        }
                    }


                }



                // characterQueue is empty
                if (endRound)
                {
                    AssignItems(battle.itemPool, characterViewModel.Party);
                    BattleScore.RoundCount++;
                    //reconstruct queue and build new battle...
                    battle = new Battle();
                    //take care of CharacterDataset
                    //take care of MonsterDataset
                    //load data

                    GenerateNewMonsters();
                    //LoadDataCommandCharacterOnly.Execute(null);
                    // start with level 1 by default
                    gameEngine.currentRound += 1;

                    InitializeBattle();

                    gameEngine.movedMonsters.Clear();
                    gameEngine.movedCharacters.Clear();


                    //create speedqueue and render map
                    potionNum = 6;
                    allowRoundHealing = true;
                    availablePotion = true;
                    availableFocusAtk = true;

                    // determine weather character or monster
                    if (gameEngine.characterQueue.Peek().Spd >= gameEngine.monsterQueue.Peek().Spd)
                    {
                        // character turn dequeue and hold dont enqueue.
                        curCharacter = gameEngine.characterQueue.Dequeue();
                    }
                    else
                    {
                        // monster turn
                        while (gameEngine.monsterQueue.Count > 0 && (gameEngine.characterQueue.Peek().Spd < gameEngine.monsterQueue.Peek().Spd))
                        {
                            curMonster = gameEngine.monsterQueue.Dequeue();
                            Character target = GetCharacter();
                            if (target != null)
                            {
                                //attack
                                ApplyDamageMTC(curMonster, target);
                                if (target.CurrentHP <= 0)
                                {
                                    RemoveTargetFromQueues(target);
                                }
                                else
                                {
                                    UpdateTargetInQueues(target);
                                }

                            }
                            else
                            {
                                gameOver = true;
                            }
                            gameEngine.movedMonsters.Enqueue(curMonster);
                            curMonster = null;
                        }
                        // character turn dequeue and hold dont enqueue.
                        curCharacter = gameEngine.characterQueue.Dequeue();

                    }


                    endRound = false;
                }
                else if (gameOver)
                {
                    //PrintDialog("Game Over!");
                    BattleScore.ScoreTotal = BattleScore.ExperienceGainedTotal;

                    // Save the Score to the DataStore
                    ScoresViewModel.Instance.AddAsync(BattleScore).GetAwaiter().GetResult();

                    //To keep track of how many games have been played
                    GameGlobals.GameCount++;

                    return 0;
                }
                else
                {

                    if (gameEngine.characterQueue.Count() == 0 && gameEngine.movedCharacters.Count() > 0)
                    {
                        while (gameEngine.movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(gameEngine.movedCharacters.Dequeue());
                        }
                    }
                    // bug queue is empty
                    curCharacter = gameEngine.characterQueue.Dequeue();

                    //hackathon changes
                    if (curCharacter.CurrentHP < curCharacter.MaxHP && potionNum > 0)
                    {
                        availablePotion = true;
                    }
                    else
                    {
                        availablePotion = false;
                    }

                    endTurn = false;

                }

                // new turn! or game over! or new round
                // check empty if character 
                // character turn dequeue and hold dont enqueue.



                // "itempool" is gloabl var
                // drop item => put in the pool
                // 

                // while monster is true
                // auto move, auto attack, no highlight

                //until character
                // highlight character, highlight move grid, highlight attack grit
                // wait for click

            }

            //Navigation.InsertPageBefore(new GameOver(), Navigation.NavigationStack[1]);
            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            return 0;

        }

        private void InitAutoBattle()
        {
            gameEngine.currentRound = 1;
            battle.itemPool = new List<Item>();
            gameEngine.movedMonsters.Clear();
            gameEngine.movedCharacters.Clear();
            gameEngine.characterQueue.Clear();
            gameEngine.monsterQueue.Clear();
            gameEngine.ClearDialogCache();

            gameOver = false;
            magicRevive = true;
            BattleScore = new Score();
            BattleScore.AutoBattle = true;
            charViewModel = CharactersViewModel.Instance;
            monViewModel = MonstersViewModel.Instance;
            itemViewModel = ItemsViewModel.Instance;
        }

        // Mike please read here Apply default item for new characters
        private void AssignItems(List<Item> itemPool, List<Character> party)
        {
            foreach (Item i in itemPool)
            {
                foreach (Character c in party)
                {
                    if (c.EquippedItem.ContainsKey(i.Location) && c.EquippedItem[i.Location].Value < i.Value)
                    {
                        c.EquippedItem[i.Location] = i;
                        break;
                    }
                    else if (!c.EquippedItem.ContainsKey(i.Location))
                    {
                        c.EquippedItem[i.Location] = i;
                        break;
                    }
                }
            }

        }

    }
}

