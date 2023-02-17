using Kameleon3.Model;
using Kameleon3.Persistence;
using Kameleon3.ViewModel;

namespace Kameleon3
{
    public partial class App : Application
    {

        private const string SuspendedGameSavePath = "SuspendedGame";

        private readonly AppShell _appShell;
        private readonly IKameleonDataAccess _kameleonDataAccess;
        private readonly KameleonGameModel _kameleonGameModel;
        private readonly IStore _kameleonStore;
        private readonly KameleonViewModel _kameleonViewModel;


        public App()
        {
            InitializeComponent();

            _kameleonStore = new KameleonStore();
            _kameleonDataAccess = new KameleonFileDataAccess(FileSystem.AppDataDirectory);

            _kameleonGameModel = new KameleonGameModel(_kameleonDataAccess);
            _kameleonViewModel = new KameleonViewModel(_kameleonGameModel);

            _appShell = new AppShell(_kameleonStore, _kameleonDataAccess, _kameleonGameModel, _kameleonViewModel)
            {
                BindingContext = _kameleonViewModel
            };
            MainPage = _appShell;
            
        }



        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);
            _kameleonGameModel.fNewGame();

         


            return window;
        }
    }
}