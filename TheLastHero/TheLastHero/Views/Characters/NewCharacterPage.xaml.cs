using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TheLastHero.Models;

namespace TheLastHero.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCharacterPage : ContentPage
    {
        public Character Data { get; set; }

        public NewCharacterPage()
        {
            InitializeComponent();

            Data = new Character
            {
                Name = "Character name",
                ImgSource = "KnightRight.png",
                Type = "Human",

        };

            BindingContext = this;


        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            //make sure current and max are the same 
            Data.CurrentHP = Data.MaxHP; 

            MessagingCenter.Send(this, "AddData", Data);
            await Navigation.PopAsync();
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
