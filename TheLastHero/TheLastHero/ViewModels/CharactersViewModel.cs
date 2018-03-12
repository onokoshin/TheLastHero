using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TheLastHero.Models;
using TheLastHero.Views;
using System.Linq;
using System.Collections.Generic;

namespace TheLastHero.ViewModels
{
    public class CharactersViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static CharactersViewModel _instance;

        public static CharactersViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharactersViewModel();
                }
                return _instance;
            }
        }

        public ObservableCollection<Character> Dataset { get; set; }

        public List<Character> Party { get; set; }
        public Command LoadDataCommand { get; set; }

        public String Slot1;
        public String Slot2;
        public String Slot3 = "EmptySlot.png";
        public String Slot4 = "EmptySlot.png";
        public String Slot5 = "EmptySlot.png";
        public String Slot6 = "EmptySlot.png";

        Character c1 = new Character();
        Character c2 = new Character();
        Character c3 = new Character();
        Character c4 = new Character();
        Character c5 = new Character();
        Character c6 = new Character();

        private bool _needsRefresh;

        public CharactersViewModel()
        {

            Title = "Character List";
            Dataset = new ObservableCollection<Character>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            //Testing Partlist 
            Party = new List<Character>();
            for(int i=1; i<7; i++)
            {
                Party.Add(c1); 
            }

            MessagingCenter.Subscribe<DeleteCharacterPage, Character>(this, "DeleteData", async (obj, data) =>
            {
                Dataset.Remove(data);
                await DataStore.DeleteAsync_Character(data);
            });

            MessagingCenter.Subscribe<NewCharacterPage, Character>(this, "AddData", async (obj, data) =>
            {
                Dataset.Add(data);
                await DataStore.AddAsync_Character(data);
            });

            MessagingCenter.Subscribe<EditCharacterPage, Character>(this, "EditData", async (obj, data) =>
            {

                // Find the Item, then update it
                var myData = Dataset.FirstOrDefault(arg => arg.Id == data.Id);
                if (myData == null)
                {
                    return;
                }

                myData.Update(data);
                await DataStore.UpdateAsync_Character(data);

                _needsRefresh = true;

            });

        }

        public void RefreshParty()
        {
            Party = new List<Character>();
            for (int i = 1; i < 7; i++)
            {
                Party.Add(c1);
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

        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Dataset.Clear();
                var dataset = await DataStore.GetAllAsync_Character(true);
                foreach (var data in dataset)
                {
                    Dataset.Add(data);
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
    }
}
