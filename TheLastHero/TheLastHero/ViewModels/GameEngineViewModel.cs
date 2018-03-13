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

            // assign items
            foreach (Character c in CharacterDataset)
            {
                foreach (Item i in ItemDataset)
                {
                    if (i.EquippableLocation.Equals("Head"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, Character.Locations.Head);
                    }
                    else if (i.EquippableLocation.Equals("Body"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, Character.Locations.Body);
                    }
                    else if (i.EquippableLocation.Equals("Feet"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, Character.Locations.Feet);
                    }
                    else if (i.EquippableLocation.Equals("Primary"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, Character.Locations.Primary);
                    }
                    else if (i.EquippableLocation.Equals("Offhand"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, Character.Locations.Offhand);
                    }
                    else if (i.EquippableLocation.Equals("LeftFinger"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, Character.Locations.LeftFinger);
                    }
                    else if (i.EquippableLocation.Equals("RightFinger"))
                    {
                        i.EquippedBy = c.Id;
                        c.EquipItem(i, Character.Locations.RightFinger);
                    }
                }
            }

            gameEngine = GameEngine.Instance;
            battle = new Battle();
            curCharacter = new Character();

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


    }
}

