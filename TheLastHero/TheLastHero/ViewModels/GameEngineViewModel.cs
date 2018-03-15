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
    }
}

