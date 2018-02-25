using System.Collections.ObjectModel;
using Xamarin.Forms;
using TheLastHero.GameEngines;
using TheLastHero.Models;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace TheLastHero.ViewModels
{
    public class GameEngineViewModel : BaseViewModel
    {

        public string cell;


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

        public GameEngine gameEngine { get; set; }
        //public Command LoadDataCommand { get; set; }


        public ObservableCollection<Character> CharacterDataset { get; set; }
        public ObservableCollection<Monster> MonsterDataset { get; set; }
        public ObservableCollection<Creature> CreatureDataset { get; set; }
        // public ObservableCollection<Character> ItemDataset { get; set; }

        public Command LoadDataCommand { get; set; }
        private bool _needsRefresh;


        public GameEngineViewModel()
        {
            CharacterDataset = new ObservableCollection<Character>();
            MonsterDataset = new ObservableCollection<Monster>();
            CreatureDataset = new ObservableCollection<Creature>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            gameEngine = new GameEngine();

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
                CreatureDataset.Clear();
                var cdataset = await DataStore.GetAllAsync_Character(true);
                var mdataset = await DataStore.GetAllAsync_Monster(true);
                //var idataset = await DataStore.GetAllAsync_Character(true);
                foreach (var data in cdataset)
                {
                    CharacterDataset.Add(data);
                    CreatureDataset.Add(data);
                }
                foreach (var data in mdataset)
                {
                    MonsterDataset.Add(data);
                    CreatureDataset.Add(data);
                }
                CreatureDataset = new ObservableCollection<Creature>(CreatureDataset.OrderByDescending(i => i.Spd));
                foreach (Creature c in CreatureDataset)
                {
                    gameEngine.nextOneQueue.Enqueue(c);
                }

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
    }
}

