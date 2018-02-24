using System.Collections.ObjectModel;
using Xamarin.Forms;
using TheLastHero.GameEngines;

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

        public GameEngineViewModel()
        {
            gameEngine = new GameEngine();

        }




    }
}

