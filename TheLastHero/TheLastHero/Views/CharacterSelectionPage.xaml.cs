using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TheLastHero.Models;
using TheLastHero.ViewModels;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterSelectionPage : ContentPage
    {
        // ReSharper disable once NotAccessedField.Local
        private CharactersViewModel _viewModel;
        public String Slot1 = "Bow.png";
        public int locationPtr = 0;

        public CharacterSelectionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = CharactersViewModel.Instance;
        }

        private void OnCharacterSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Character;
            if (data == null)
            {
                return;
            }

            if (locationPtr < 6)
            {
                data.PartySlotNum = locationPtr;
                _viewModel.Party[locationPtr] = data;
                locationPtr++;
                //InitializeComponent();
                BindingContext = null;
                BindingContext = _viewModel;
            }
            else
            {
                DisplayAlert("Alert", "Your Party is full! Deselect first to select more characters!", "OK");
            }
            //await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(data)));


            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }


        private void OnCharacterDeselected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Character;
            if (data == null)
            {
                return;
            }

            if (locationPtr != 0)
            {

                if (data.PartySlotNum != 5)
                {
                    int tempNum = data.PartySlotNum;
                    while (tempNum != 5)
                    {
                        _viewModel.Party[tempNum] = _viewModel.Party[tempNum + 1];
                        _viewModel.Party[tempNum].PartySlotNum = tempNum;
                        tempNum++;
                    }

                    _viewModel.Party[tempNum] = new Character();
                    _viewModel.Party[tempNum].PartySlotNum = 5;


                }
                else
                {
                    int tempNum = data.PartySlotNum;
                    _viewModel.Party[tempNum] = new Character();
                    _viewModel.Party[tempNum].PartySlotNum = 5;
                }


                locationPtr--;
                //InitializeComponent();
                BindingContext = null;
                BindingContext = _viewModel;
            }
            //await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(data)));


            // Manually deselect item.
            ItemsListView2.SelectedItem = null;
        }


        private void RemoveCharacter_Clicked(object sender, EventArgs e)
        {
            if (locationPtr != 0)
            {
                locationPtr--;
                _viewModel.Party[locationPtr] = new Character();
                BindingContext = null;
                BindingContext = _viewModel;
            }
            else
            {
                DisplayAlert("Alert", "You cannot remove more character!", "OK");
            }
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewCharacterPage());
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            //Navigation.InsertPageBefore(new GameOver(), Navigation.NavigationStack[1]);
            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
            await Navigation.PushAsync(new BattlePage());
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

            if (_viewModel.Dataset.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            _viewModel.RefreshParty();
            BindingContext = _viewModel;
        }
    }
}