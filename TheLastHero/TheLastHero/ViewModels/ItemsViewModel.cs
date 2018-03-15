using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TheLastHero.Models;
using TheLastHero.Views;
using System.Linq;

namespace TheLastHero.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static ItemsViewModel _instance;

        public static ItemsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemsViewModel();
                }
                return _instance;
            }
        }

        public ObservableCollection<Item> Dataset { get; set; }
        public Command LoadDataCommand { get; set; }


        private bool _needsRefresh;

        public ItemsViewModel()
        {
            Title = "Item";
            Dataset = new ObservableCollection<Item>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            #region Messages
            MessagingCenter.Subscribe<DeleteItemPage, Item>(this, "DeleteData", async (obj, data) =>
            {
                Dataset.Remove(data);
                await DataStore.DeleteAsync_Item(data);
            });

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddData", async (obj, data) =>
            {
                Dataset.Add(data);
                await DataStore.AddAsync_Item(data);
            });

            MessagingCenter.Subscribe<EditItemPage, Item>(this, "EditData", async (obj, data) =>
            {
                // Find the Item, then update it
                var myData = Dataset.FirstOrDefault(arg => arg.Guid == data.Guid);
                if (myData == null)
                {
                    return;
                }

                myData.Update(data);
                await DataStore.UpdateAsync_Item(myData);

                _needsRefresh = true;

            });

            #endregion Messages
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
                var dataset = await DataStore.GetAllAsync_Item(true);
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

        #region DataOperations

        public async Task<bool> AddAsync(Item data)
        {
            Dataset.Add(data);
            var myReturn = await DataStore.AddAsync_Item(data);
            return myReturn;
        }

        public async Task<bool> DeleteAsync(Item data)
        {
            Dataset.Remove(data);
            var myReturn = await DataStore.DeleteAsync_Item(data);
            return myReturn;
        }

        public async Task<bool> UpdateAsync(Item data)
        {
            // Find the Item, then update it
            var myData = Dataset.FirstOrDefault(arg => arg.Guid == data.Guid);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);
            await DataStore.UpdateAsync_Item(myData);

            _needsRefresh = true;

            return true;
        }

        // Call to database to ensure most recent
        public async Task<Item> GetAsync(string id)
        {
            var myData = await DataStore.GetAsync_Item(id);
            return myData;
        }

        // Having this at the ViewModel, because it has the DataStore
        // That allows the feature to work for both SQL and the MOCk datastores...
        public async Task<bool> InsertUpdateAsync(Item data)
        {
            var myReturn = await DataStore.InsertUpdateAsync_Item(data);
            return myReturn;
        }

        #endregion DataOperations

        #region ItemConversion

        // Takes an item string ID and looks it up and returns the item
        // This is because the Items on a character are stores as strings of the GUID.  That way it can be saved to the DB.
        public Item GetItem(string ItemID)
        {
            if (string.IsNullOrEmpty(ItemID))
            {
                return null;
            }

            Item myData = DataStore.GetAsync_Item(ItemID).GetAwaiter().GetResult();
            if (myData == null)
            {
                return null;
            }

            return myData;
        }

        #endregion ItemConversion

        //public string ChooseRandomItemString(ItemLocationEnum location, AttributeEnum attribute)
        //{
        //    return null;
        //}
    }
}

