using System;
using TheLastHero.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheLastHero.ViewModels;
using TheLastHero.Controller;
using TheLastHero.Models;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            SettingDataSource.IsToggled = true;
            // Set debug settings
            EnableCriticalHitDamage.IsToggled = GameGlobals.EnableCriticalHitDamage;
            EnableCriticalMissProblems.IsToggled = GameGlobals.EnableCriticalMissProblems;
            EnableMiss.IsToggled = GameGlobals.EnableMiss; 
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.

            if (e.Value == true)
            {
                ItemsViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Mock);
                MonstersViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Mock);
                CharactersViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Mock);
                ScoresViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Mock);
            }
            else
            {
                ItemsViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Sql);
                MonstersViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Sql);
                CharactersViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Sql);
                ScoresViewModel.Instance.SetDataStore(BaseViewModel.DataStoreEnum.Sql);
            }

            // Load the Data
            ItemsViewModel.Instance.LoadDataCommand.CanExecute(null);
            ItemsViewModel.Instance.LoadDataCommand.Execute(null);

            MonstersViewModel.Instance.LoadDataCommand.CanExecute(null);
            MonstersViewModel.Instance.LoadDataCommand.Execute(null);

            CharactersViewModel.Instance.LoadDataCommand.CanExecute(null);
            CharactersViewModel.Instance.LoadDataCommand.Execute(null);

            ScoresViewModel.Instance.LoadDataCommand.CanExecute(null);
            ScoresViewModel.Instance.LoadDataCommand.Execute(null);

            // Have data refresh...
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            MonstersViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.SetNeedsRefresh(true);
            ScoresViewModel.Instance.SetNeedsRefresh(true);
        }

        private async void ClearDatabase_Command(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete", "Sure you want to Delete All Data, and start over?", "Yes", "No");
            if (answer)
            {
                // Call to the SQL DataStore and have it clear the tables.
                SQLDataStore.Instance.InitializeDatabaseNewTables();
            }
        }

        private async void GetItems_Command(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Get", "Sure you want to Get Items from the Server?", "Yes", "No");
            if (answer)
            {
                // Call to the Item Service and have it Get the Items
                ItemsController.Instance.GetItemsFromServer();
            }
        }

        private async void GetItemsPost_Command(object sender, EventArgs e)
        {
            //ItemsController.Instance.GetItemsFromGame(int number, int level, AttributeEnum attribute, ItemLocationEnum location, bool random, bool updateDataBase)

            var number = 10;    // 10 items
            var level = 0;  // Max Value of 6
            var attribute = AttributeEnum.Unknown;  // Any Attribute
            var location = ItemLocationEnum.Unknown;    // Any Location
            var random = true;  // Random between 1 and Level
            var updateDataBase = true;  // Add them to the DB

            var myDataList = await ItemsController.Instance.GetItemsFromGame(number, level, attribute, location, random, updateDataBase);

            var myOutput = "No Results";

            if (myDataList != null && myDataList.Count > 0)
            {
                // Reset the output
                myOutput = "";

                foreach (var item in myDataList)
                {
                    // Add them line by one, use \n to force new line for output display.
                    myOutput += item.FormatOutput() + "\n";
                }
            }

            var answer = await DisplayAlert("Returned List", myOutput, "Yes", "No");
        }

        // Turn on Critical Misses
        private void EnableMissProblems_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            GameGlobals.EnableMiss = e.Value;
            if (GameGlobals.EnableMiss)
            {
                GameGlobals.EnableCriticalHitDamage = false;
                EnableCriticalHitDamage.IsToggled = GameGlobals.EnableCriticalHitDamage;
                GameGlobals.EnableCriticalMissProblems = false;
                EnableCriticalMissProblems.IsToggled = GameGlobals.EnableCriticalMissProblems;
            }
        }

        // Turn on Critical Hit Damage
        private void EnableCriticalHitDamage_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            GameGlobals.EnableCriticalHitDamage = e.Value;
            if (GameGlobals.EnableCriticalHitDamage)
            {
                GameGlobals.EnableMiss = false;
                EnableMiss.IsToggled = GameGlobals.EnableMiss;
                GameGlobals.EnableCriticalMissProblems = false;
                EnableCriticalMissProblems.IsToggled = GameGlobals.EnableCriticalMissProblems;
            }
        }

        private void EnableCriticalMissProblems_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            GameGlobals.EnableCriticalMissProblems = e.Value;
            if (GameGlobals.EnableCriticalMissProblems)
            {
                GameGlobals.EnableMiss = false;
                EnableMiss.IsToggled = GameGlobals.EnableMiss;
                GameGlobals.EnableCriticalHitDamage = false;
                EnableCriticalHitDamage.IsToggled = GameGlobals.EnableCriticalHitDamage;
            }
        }
    }
}
