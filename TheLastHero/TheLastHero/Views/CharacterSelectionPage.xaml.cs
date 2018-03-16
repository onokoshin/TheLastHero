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

            //create a character based on the selection
            var c = CharacterCreation(data); 
                
            //if selected characters are less than 6, it will display alert message. 
            if (locationPtr < 6)
            {
                c.PartySlotNum = locationPtr;
                _viewModel.Party[locationPtr] = c;
                locationPtr++;
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

        private Character CharacterCreation(Character data)
        {
            Character c = new Character();
            if (data.ImgSource.Equals("") || data.ImgSource.Equals(null))
            {
                c.Spd = data.Spd;
                c.Def = data.Def;
                c.Atk = data.Atk;
                c.CurrentHP = data.CurrentHP;
                c.MaxHP = data.MaxHP;
                c.MaxMP = data.MaxMP;
                c.Lvl = data.Lvl;
                c.Luk = data.Luk;
                c.Name = data.Name;
                c.ImgSource = "EmptySlot2.png";
            }
            else
            {
                c.Spd = data.Spd;
                c.Def = data.Def;
                c.Atk = data.Atk;
                c.CurrentHP = data.CurrentHP;
                c.MaxHP = data.MaxHP;
                c.MaxMP = data.MaxMP;
                c.Lvl = data.Lvl;
                c.Luk = data.Luk;
                c.Name = data.Name;
                c.ImgSource = data.ImgSource;
            }

            return c;
        }

        private void OnCharacterDeselected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Character;
            if (data == null)
            {
                return;
            }

            if (locationPtr != 0 && !data.ImgSource.Equals("EmptySlot2.png"))
            {

                if (data.PartySlotNum != 5 )
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
            if(locationPtr < 6)
            {
                await DisplayAlert("Alert", "Please select 6 characters to start the battle.", "OK");
            }
            else
            {
                 
                await Navigation.PushAsync(new BattlePage(_viewModel));
            }

            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;

            if (ToolbarItems.Count > 0)
            {
                ToolbarItems.RemoveAt(1);
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
            locationPtr = 0; 
            BindingContext = _viewModel;
        }
    }
}
