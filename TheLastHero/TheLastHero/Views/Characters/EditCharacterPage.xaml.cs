using System;

using TheLastHero.Models;
using TheLastHero.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCharacterPage : ContentPage
    {
        // ReSharper disable once NotAccessedField.Local
        private CharacterDetailViewModel _viewModel;

        public Character Data { get; set; }

        public EditCharacterPage(CharacterDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Edit " + viewModel.Title;

            InitializeComponent();


            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "EditData", Data);

            // removing the old ItemDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new items details page, with the new Item data on it
            await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(Data)));

            // Last, remove this page
            Navigation.RemovePage(this);
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // The stepper function for Range
        void Lvl_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            LevelValue.Text = String.Format("{0}", e.NewValue);
        }

        // The stepper function for HP
        void Health_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            HealthValue.Text = String.Format("{0}", e.NewValue);
        }

        // The stepper function for Attack
        void Attack_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            AttackValue.Text = String.Format("{0}", e.NewValue);
        }

        // The stepper function for Defense
        void Defense_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            DefenseValue.Text = String.Format("{0}", e.NewValue);
        }

        // The stepper function for Speed
        void Speed_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            SpeedValue.Text = String.Format("{0}", e.NewValue);
        }

        // The stepper function for Moveing Range
        //void MoveRange_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    MoveRangeValue.Text = String.Format("{0}", e.NewValue);
        //}

        //// The stepper function for Attack Range
        //void AttackRange_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        //{
        //    AttackRangeValue.Text = String.Format("{0}", e.NewValue);
        //}
    }
}
