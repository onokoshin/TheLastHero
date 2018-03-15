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


        public GameEngine gameEngine { get; set; }
        public Battle battle { get; set; }
        public Character curCharacter { get; set; }

        public ObservableCollection<Character> CharacterDataset { get; set; }
        public ObservableCollection<Monster> MonsterDataset { get; set; }
        //public ObservableCollection<Creature> CreatureDataset { get; set; }
        public ObservableCollection<Item> ItemDataset { get; set; }
        public Command LoadDataCommand { get; set; }
        public Command LoadDataCommandCharacterOnly { get; set; }

        // for hackathon
        public int potionNum { get; set; }
        public bool allowRoundHealing { get; set; }
        public bool availablePotion { get; set; }
        public bool availableFocusAtk { get; set; }
        public bool magicRevive { get; set; }

        private bool _needsRefresh;

        public bool autoPlay = false;


        // Batstle map is a grid layout  
        //Grid battleGrid = new Grid();

        Monster curMonster = new Monster();
        bool atkTurn = false;
        bool endTurn = false;
        bool endRound = false;
        public bool gameOver = false;

        Queue<Character> movedCharacters = new Queue<Character>();
        Queue<Monster> movedMonsters = new Queue<Monster>();

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



        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static GameEngineViewModel _instance;

        public static GameEngineViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameEngineViewModel();
                }
                return _instance;
            }
        }

        private GameEngineViewModel()
        {
            CharacterDataset = new ObservableCollection<Character>();
            MonsterDataset = new ObservableCollection<Monster>();
            //CreatureDataset = new ObservableCollection<Creature>();
            ItemDataset = new ObservableCollection<Item>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());
            LoadDataCommandCharacterOnly = new Command(async () => await ExecuteLoadDataCommandCharacterOnly());

            // assign items
            foreach (Character c in CharacterDataset)
            {
                foreach (Item i in ItemDataset)
                {
                    if (i.EquippableLocation.Equals("Head"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, ItemLocationEnum.Head);
                    }
                    else if (i.EquippableLocation.Equals("Necklass"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, ItemLocationEnum.Necklass);
                    }
                    else if (i.EquippableLocation.Equals("Feet"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, ItemLocationEnum.Feet);
                    }
                    else if (i.EquippableLocation.Equals("PrimaryHand"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, ItemLocationEnum.PrimaryHand);
                    }
                    else if (i.EquippableLocation.Equals("OffHand"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, ItemLocationEnum.OffHand);
                    }
                    else if (i.EquippableLocation.Equals("LeftFinger"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, ItemLocationEnum.LeftFinger);
                    }
                    else if (i.EquippableLocation.Equals("RightFinger"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, ItemLocationEnum.RightFinger);
                    }
                }
            }

            gameEngine = GameEngine.Instance;
            battle = new Battle();
            curCharacter = new Character();

        }

        public void MoveFirstCreature()
        {
            // read sqldatabase or mockdatabase
            LoadDataCommand.Execute(null);
            // start with level 1 by default
            gameEngine.currentRound = 1;
            //create speedqueue and render map
            InitializeBattle();
            potionNum = 6;
            allowRoundHealing = true;
            availablePotion = true;
            availableFocusAtk = true;
            //????should we use 2 queues characterqueue and monsterqueue,
            // creature queue doesn make sense because we dont need another sets
            // of creature, we already have character and monsters

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
                                }
                                else
                                {
                                    battle.battleMapHP[target.xPosition, target.yPosition] = target.CurrentHP.ToString();
                                    UpdateTargetInQueues(target);
                                }

                            }

                        }
                    }
                    movedMonsters.Enqueue(curMonster);
                    curMonster = null;
                }
            }

            //_script.scriptCounter = 1;
            //RunScript(_script, 0);
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
        public void InitCharacterQueue()
        {
            if (CharacterDataset.Count > 0)
            {
                gameEngine.characterQueue.Clear();
                int y = 0;
                foreach (Character c in CharacterDataset)
                {
                    c.xPosition = 0;
                    c.yPosition = y;
                    gameEngine.characterQueue.Enqueue(c);
                    y++;
                }
            }
        }
        public void InitMonsterQueue()
        {
            if (MonsterDataset.Count > 0)
            {
                gameEngine.monsterQueue.Clear();

                int y = 0;
                foreach (Monster m in MonsterDataset)
                {
                    m.xPosition = 4;
                    m.yPosition = y;
                    gameEngine.monsterQueue.Enqueue(m);
                    y++;
                }
            }
        }

        public Item GenerateNewItem()
        {
            //return guid of the item
            Random r = new Random();
            int index = r.Next(1, ItemDataset.Count());
            Item i = new Item();
            CopyItem(i, ItemDataset[index]);
            return i;
        }

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
        }

        public void GenerateNewMonsters()
        {
            MonsterDataset.Clear();
            LevelTable levelTable = LevelTable.Instance;
            LevelDetails levelDetails = levelTable.LevelDetailsList[gameEngine.currentRound];

            for (int i = 0; i < 6; i++)
            {
                Monster m = new Monster();
                m.Id = Guid.NewGuid().ToString();
                m.Friendly = false;
                m.AtkRange = 1;
                m.LiveStatus = true;
                m.CurrentMP = 100;
                m.MaxMP = 100;
                m.MoveRange = 1;
                m.Drop = false;
                m.Luk = 10;
                m.xPosition = 4;
                m.yPosition = i;
                Random r = new Random();


                m.UniqueDropID = GenerateNewItem().Guid;

                int rnum = r.Next(1, 9);
                switch (rnum)
                {
                    case 1:
                        m.Name = "Wolf";
                        m.Type = "Beast";
                        m.ImgSource = "WolfLeft.png";
                        break;
                    case 2:
                        m.Name = "Skeleton";
                        m.Type = "Beast";
                        m.ImgSource = "SkeletonLeft.png";
                        break;
                    case 3:
                        m.Name = "Skeleton";
                        m.Type = "Ghost";
                        m.ImgSource = "SkeletonLeft2.png";
                        break;
                    case 4:
                        m.Name = "Skeleton";
                        m.Type = "Ghost";
                        m.ImgSource = "SkeletonLeft3.png";
                        break;
                    case 5:
                        m.Name = "Tiger";
                        m.Type = "Beast";
                        m.ImgSource = "TigerLeft.png";
                        break;
                    case 6:
                        m.Name = "Raven";
                        m.Type = "Beast";
                        m.ImgSource = "RavenLeft.png";
                        break;
                    case 7:
                        m.Name = "Heron";
                        m.Type = "Beast";
                        m.ImgSource = "HeronLeft.png";
                        break;
                    case 8:
                        m.Name = "Hawk";
                        m.Type = "Beast";
                        m.ImgSource = "HawkLeft.png";
                        break;
                    case 9:
                        m.Name = "Cat";
                        m.Type = "Beast";
                        m.ImgSource = "CatLeft.png";
                        break;
                    default:
                        break;
                }
                m.ScaleLevel(gameEngine.currentRound);

                MonsterDataset.Add(m);
            }

        }

        public void HandleButtonClicked(int x, int y)
        {

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
                     || movedMonsters.Where(z => z.Id.Equals(battle.battleMapId[x, y])).Count() > 0))
                {
                    Monster m = new Monster();
                    // grab target from monsterqueue, calculate dmg, decresase target.HP, 
                    if (gameEngine.monsterQueue.Where(z => z.Id.Equals(battle.battleMapId[x, y])).Count() > 0)
                        m = gameEngine.monsterQueue.Where(z => z.Id.Equals(battle.battleMapId[x, y])).First();
                    else if (movedMonsters.Where(z => z.Id.Equals(battle.battleMapId[x, y])).Count() > 0)
                        m = movedMonsters.Where(z => z.Id.Equals(battle.battleMapId[x, y])).First();

                    PrintDialog(curCharacter + " is attacking " + m.Name);
                    // decrease target HP by = level attack + weapon attack  MIKE PLESASE READ HERE
                    ApplyDamageCTM(curCharacter, m);



                    // gameEngine.ConsoleDialog1 = m.CurrentHP.ToString();
                    if (m.CurrentHP <= 0)
                    {
                        if (m.UniqueDropID != null)
                        {
                            Item drop = ItemDataset.Where(item => item.Guid.Equals(m.UniqueDropID)).First();
                            curCharacter.EquipItem(drop, drop.Location);
                        }

                        RemoveTargetFromQueues(m);
                        battle.battleMapId[m.xPosition, m.yPosition] = "";
                        battle.battleMapTop[m.xPosition, m.yPosition] = "";
                        battle.battleMapHP[m.xPosition, m.yPosition] = "";

                        if (movedMonsters.Count() == 0 && gameEngine.monsterQueue.Count() == 0)
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
                battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                movedCharacters.Enqueue(curCharacter);
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

                                if (movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
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
                        movedMonsters.Enqueue(curMonster);
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
                                    if (movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
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
                            movedMonsters.Enqueue(curMonster);
                            curMonster = null;
                        }
                    }

                    if (movedCharacters.Count() == 0)
                    {
                        gameOver = true;
                    }
                    else
                    {
                        //transfer moved queue to speed queue
                        while (movedMonsters.Count() > 0)
                        {
                            gameEngine.monsterQueue.Enqueue(movedMonsters.Dequeue());
                        }
                        while (movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(movedCharacters.Dequeue());
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

                    if (movedMonsters.Count() == 0)
                    {
                        endRound = true;
                    }
                    else
                    {
                        //transfer moved queue to speed queue
                        while (movedMonsters.Count() > 0)
                        {
                            gameEngine.monsterQueue.Enqueue(movedMonsters.Dequeue());
                        }
                        while (movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(movedCharacters.Dequeue());
                        }
                    }


                }



                // characterQueue is empty
                if (endRound)
                {
                    //reconstruct queue and build new battle...
                    battle = new Battle();
                    //take care of CharacterDataset
                    //take care of MonsterDataset
                    //load data

                    GenerateNewMonsters();
                    LoadDataCommandCharacterOnly.Execute(null);
                    // start with level 1 by default
                    gameEngine.currentRound += 1;

                    InitializeBattle();

                    movedMonsters.Clear();
                    movedCharacters.Clear();


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

                }
                else
                {

                    if (gameEngine.characterQueue.Count() == 0 && movedCharacters.Count() > 0)
                    {
                        while (movedCharacters.Count() > 0)
                        {
                            gameEngine.characterQueue.Enqueue(movedCharacters.Dequeue());
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
                    battle.SetAllSelection(Battle.HIGHLIGHTGREY);
                    battle.battleMapSelection[curCharacter.xPosition, curCharacter.yPosition] = Battle.HIGHLIGHTGREEN;
                    battle.battleMapId[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.Id;
                    battle.battleMapHP[curCharacter.xPosition, curCharacter.yPosition] = curCharacter.CurrentHP.ToString();

                    RenderMoveAttackRange(curCharacter.xPosition, curCharacter.yPosition, curCharacter.MoveRange + curCharacter.AtkRange, curCharacter.AtkRange);
                    PrintDialog(curCharacter.Name + "'s turn");

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



            //_script.scriptCounter = 1;
            //RunScript(_script, 0);
            battle.RefreshAllCell();
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
            gameEngine.ConsoleDialog1 = gameEngine.DialogCache[0];
            /*+ "\n"
            +  gameEngine.DialogCache[1] + "\n"
            +  gameEngine.DialogCache[2] + "\n"
            +  gameEngine.DialogCache[3] + "\n"
            +  gameEngine.DialogCache[4];*/
        }

        public void Potion_Clicked(object sender, EventArgs e)
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

        public void FocusAtk_Clicked(object sender, EventArgs e)
        {

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
                            if (movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
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

                                    if (movedCharacters.Count() == 0 && gameEngine.characterQueue.Count() == 0)
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
                    movedMonsters.Enqueue(curMonster);
                    curMonster = null;
                }
            }
        }

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
            while (movedMonsters.Count() > 0)
            {
                tmpM = movedMonsters.Dequeue();
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
                movedMonsters.Enqueue(tmpM);
            }
        }

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
                    tmpC = null;
                }
                else
                {
                    tmp1.Enqueue(tmpC);
                }
            }
            while (movedCharacters.Count() > 0)
            {
                tmpC = movedCharacters.Dequeue();
                if (tmpC.Id.Equals(target.Id))
                {
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
                movedCharacters.Enqueue(tmpC);
            }
        }

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
            while (movedCharacters.Count() > 0)
            {
                tmpC = movedCharacters.Dequeue();
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
                movedCharacters.Enqueue(tmpC);
            }
        }

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
            while (movedMonsters.Count() > 0)
            {
                tmpM = movedMonsters.Dequeue();
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
                movedMonsters.Enqueue(tmpM);
            }
        }

        private void ApplyDamageMTC(Monster m, Character c)
        {//def need to be in calculation
            int dmg = (int)Math.Ceiling(m.Atk / 4.0);
            c.TakeDamage(dmg);
            PrintDialog(m.Name + " took " + dmg + " damage!");
        }

        private void ApplyDamageCTM(Character c, Monster m)
        {
            int exp = 0;
            int dmg = 0;

            //if character equips weapon, the damage needs to be added. Else (no weapon), just use atk attribute 
            if (c.EquippedItem.ContainsKey(ItemLocationEnum.PrimaryHand))
            {
                dmg = (int)Math.Ceiling(c.Atk / 4.0) + c.EquippedItem[ItemLocationEnum.PrimaryHand].Atk;
                //using monster calculateExperienceEarned function to calculate appropriate amount of experience based on damage 
                exp = m.CalculateExperienceEarned(dmg);
            }
            else
            {
                dmg = (int)Math.Ceiling(c.Atk / 4.0);
                exp = m.CalculateExperienceEarned(dmg);
            }

            //Calculate miss/dodge based on calculation 
            TakeTurn(c, m);

            //Instead of calculating damage here directly, i am using m.TakeDamage function. It also changes LiveStatus from true to false if CurrentHP < 0 
            //m.CurrentHP -= dmg;
            m.TakeDamage(dmg);
            PrintDialog(m.Name + " took " + dmg + " damage!" + "\n"
                        + "Monster LiveStatus: " + m.LiveStatus);

            //Testing level up and experience earning functionalities
            int curLevel = c.Lvl;
            c.AddExperience(exp);




            PrintDialog(c.Name + " Earned " + exp + " experience!");
            int updatedLevel = c.Lvl;
            if (updatedLevel > curLevel)
                PrintDialog(c.Name + " has leveled up! " + c.Name + " is now Lvl:" + c.Lvl);
        }

        public bool TakeTurn(Character Attacker, Monster Defender)
        {
            // Choose Move or Attack

            // For Attack, Choose Who
            //var Target = AttackChoice(Attacker);

            // Do Attack
            var AttackScore = Attacker.Lvl + Attacker.GetAttack();
            var DefenseScore = Defender.Def + Defender.Lvl;
            TurnAsAttack(Attacker, AttackScore, Defender, DefenseScore);

            return true;
        }

        // Character attacks Monster
        public bool TurnAsAttack(Character Attacker, int AttackScore, Monster Target, int DefenseScore)
        {
            TurnMessage = string.Empty;
            TurnMessageSpecial = string.Empty;
            AttackStatus = string.Empty;

            if (Attacker == null)
            {
                return false;
            }

            if (Target == null)
            {
                return false;
            }

            //This is for score
            BattleScore.TurnCount++;

            // Choose who to attack

            TargetName = Target.Name;
            AttackerName = Attacker.Name;

            var HitSuccess = RollToHitTarget(AttackScore, DefenseScore);

            if (HitStatus == HitStatusEnum.Miss)
            {
                TurnMessage = Attacker.Name + " misses " + Target.Name;
                Debug.WriteLine(TurnMessage);

                return true;
            }

            if (HitStatus == HitStatusEnum.CriticalMiss)
            {
                TurnMessage = Attacker.Name + " swings and really misses " + Target.Name;
                Debug.WriteLine(TurnMessage);

                return true;
            }

            // It's a Hit or a Critical Hit
            //Calculate Damage
            DamageAmount = Attacker.GetDamageRollValue();

            if (HitStatus == HitStatusEnum.Hit)
            {
                Target.TakeDamage(DamageAmount);
                AttackStatus = string.Format(" hits for {0} damage on ", DamageAmount);
            }

            if (HitStatus == HitStatusEnum.CriticalHit)
            {
                //2x damage
                DamageAmount += DamageAmount;

                BattleScore.ExperienceGainedTotal += Target.CalculateExperienceEarned(DamageAmount);

                Target.TakeDamage(DamageAmount);
                AttackStatus = string.Format(" hits really hard for {0} damage on ", DamageAmount);
            }

            TurnMessageSpecial = " remaining health is " + Target.CurrentHP;

            // Check for alive
            if (Target.LiveStatus == false)
            {
                // Remover target from list...
                //MonsterList.Remove(Target);

                // Mark Status in output
                TurnMessageSpecial = " and causes death";

                // Add one to the monsters killd count...
                BattleScore.MonsterSlainNumber++;

                // Add the monster to the killed list
                //BattleScore.MonstersKilledList += Target.FormatOutput();

                // Drop Items to item Pool
                //var myItemList = Target.DropAllItems();

                // If Random drops are enabled, then add some....
                // myItemList.AddRange(GetRandomMonsterItemDrops(BattleScore.RoundCount));

                // Add to Score
                //foreach (var item in myItemList)
                //{
                //    BattleScore.ItemsDroppedList += item.FormatOutput();
                //    TurnMessageSpecial += " Item " + item.Name + " dropped";
                //}

                //ItemPool.AddRange(myItemList);
            }

            TurnMessage = Attacker.Name + AttackStatus + Target.Name + TurnMessageSpecial;
            Debug.WriteLine(TurnMessage);

            return true;
        }

        public HitStatusEnum RollToHitTarget(int AttackScore, int DefenseScore)
        {

            var d20 = HelperEngine.RollDice(1, 20);

            // Turn On UnitTestingSetRoll
            if (GameGlobals.ForceRollsToNotRandom)
            {
                // Don't let it be 0, if it was not initialized...
                if (GameGlobals.ForceToHitValue < 1)
                {
                    GameGlobals.ForceToHitValue = 1;
                }

                d20 = GameGlobals.ForceToHitValue;
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
            }
            else
            {
                // Hit
                HitStatus = HitStatusEnum.Hit;
            }

            return HitStatus;
        }

        public Item dropItem(int monsterID)
        {
            // read monster from Dataset,
            var result = MonsterDataset.Where(x => x.Id == gameEngine.monsterQueue.Peek().Id).First();
            return ItemDataset.Where(i => i.Guid.Equals(result.UniqueDropID)).First();
        }

        private Character CheckNearbyCharacter(int x, int y)
        {
            if (x > 0 && gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x - 1, y])).Count() > 0)
            {
                return gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x - 1, y])).First();
            }
            if (x > 0 && movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x - 1, y])).Count() > 0)
            {
                return movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x - 1, y])).First();
            }
            if (y > 0 && gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x, y - 1])).Count() > 0)
            {
                return gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x, y - 1])).First();
            }
            if (y > 0 && movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x, y - 1])).Count() > 0)
            {
                return movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x, y - 1])).First();
            }
            if (x < 4 && gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x + 1, y])).Count() > 0)
            {
                return gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x + 1, y])).First();
            }
            if (x < 4 && movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x + 1, y])).Count() > 0)
            {
                return movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x + 1, y])).First();
            }
            if (y < 5 && gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x, y + 1])).Count() > 0)
            {
                return gameEngine.characterQueue.Where(c => c.Id.Equals(battle.battleMapId[x, y + 1])).First();
            }
            if (y < 5 && movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x, y + 1])).Count() > 0)
            {
                return movedCharacters.Where(c => c.Id.Equals(battle.battleMapId[x, y + 1])).First();
            }
            return null;
        }


        private bool CheckNearbyMonster(int x, int y)
        {

            if (x > 0 && (gameEngine.monsterQueue.Where(m => m.Id.Equals(battle.battleMapId[x - 1, y])).Count() > 0
                          || movedMonsters.Where(m => m.Id.Equals(battle.battleMapId[x - 1, y])).Count() > 0))
            {
                return true;

            }
            if (x < 4 && (gameEngine.monsterQueue.Where(m => m.Id.Equals(battle.battleMapId[x + 1, y])).Count() > 0
                          || movedMonsters.Where(m => m.Id.Equals(battle.battleMapId[x + 1, y])).Count() > 0))
            {
                return true;
            }
            if (y > 0 && (gameEngine.monsterQueue.Where(m => m.Id.Equals(battle.battleMapId[x, y - 1])).Count() > 0
                          || movedMonsters.Where(m => m.Id.Equals(battle.battleMapId[x, y - 1])).Count() > 0))
            {
                return true;
            }
            if (y < 5 && (gameEngine.monsterQueue.Where(m => m.Id.Equals(battle.battleMapId[x, y + 1])).Count() > 0
                          || movedMonsters.Where(m => m.Id.Equals(battle.battleMapId[x, y + 1])).Count() > 0))
            {
                return true;
            }
            return false;
        }

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

        public void InitializeBattle()
        {

            int round = gameEngine.currentRound;
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
                battle.SetAllBackground(Battle.SAND);
            }
            else if (round > 6 && round <= 9)
            {
                battle.title = "Desert Lvl " + round.ToString();
                battle.SetAllBackground(Battle.GRASS);
            }
            else
            {
                battle.title = "Lava Cave Lvl " + round.ToString();
                battle.SetAllBackground(Battle.LAVA);
            }
            //take care of dataset
            InitMonsterQueue();
            InitCharacterQueue();
            RenderCharactersMonsters();
        }
    }
}

