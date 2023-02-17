using Kameleon3.Model;
using Kameleon3.Persistence;
using Kameleon3.ViewModel;
using Kameleon3.View;

namespace Kameleon3
{
    public partial class AppShell : Shell
    {

        private IKameleonDataAccess _kameleonDataAccess;
        private readonly KameleonGameModel _kameleonGameModel;
        private readonly KameleonViewModel _kameleonViewModel;

        private readonly IStore _store;
        private readonly StoredGameBrowserModel _storedGameBrowserModel;
        private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;


        public AppShell(IStore kameleonStore,
        IKameleonDataAccess kameleonDataAccess,
        KameleonGameModel kameleonGameModel,
        KameleonViewModel kameleonViewModel)
        {
            
            InitializeComponent();

            _store = kameleonStore;
            _kameleonDataAccess = kameleonDataAccess;
            _kameleonGameModel = kameleonGameModel;
            _kameleonViewModel = kameleonViewModel;


            _kameleonGameModel.GameOver += KameleonGameModel_GameOver;

            _kameleonViewModel.NewGame += KameleonViewModel_NewGame;
            _kameleonViewModel.LoadGame += KameleonViewModel_LoadGame;
            _kameleonViewModel.SaveGame += KameleonViewModel_SaveGame;
            _kameleonViewModel.ExitGame += KameleonViewModel_ExitGame;

            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;

            _kameleonGameModel.fNewGame();

        }

        private async void StoredGameBrowserViewModel_GameSaving(object sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); 

            try
            {
                await _kameleonGameModel.SaveGameAsync(e.Name);
                await DisplayAlert("Kaméleon játék", "Sikeres mentés.", "OK");
            }
            catch
            {
                await DisplayAlert("Kaméleon játék", "Sikertelen mentés.", "OK");
            }
        }

        private async void StoredGameBrowserViewModel_GameLoading(object sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); 

            try
            {
                await _kameleonGameModel.LoadGameAsync(e.Name);

                await Navigation.PopAsync(); 
                await DisplayAlert("Kaméleon játék", "Sikeres betöltés.", "OK");

            }
            catch
            {
                await DisplayAlert("Kaméleon játék", "Sikertelen betöltés.", "OK");
            }
        }

        private async void KameleonViewModel_ExitGame(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = _kameleonViewModel
            });
        }

        private async void KameleonViewModel_SaveGame(object sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();

            

            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void KameleonViewModel_LoadGame(object sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();

            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private void KameleonViewModel_NewGame(object sender, EventArgs e)
        {
            _kameleonGameModel.fNewGame();
        }

        private async void KameleonGameModel_GameOver(object sender, KameleonEventArgs e)
        {
            await DisplayAlert("Kaméleon játék","A győztes: " + e.Player,"OK");
        }
    }
}