using Kameleon3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;

namespace Kameleon3.ViewModel
{
    public class KameleonViewModel : ViewModelBase
    {
        private KameleonGameModel _model;
        private int _tableSize;
        private GameMapSizeViewModel _mapsize = null!;


        public DelegateCommand NewGameCommand { get; private set; }


        public DelegateCommand LoadGameCommand { get; private set; }



        public DelegateCommand SaveGameCommand { get; private set; }


        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<KameleonField> Fields { get; set; }

        public ObservableCollection<GameMapSizeViewModel> MapSizes { get; set; }

        public GameMapSizeViewModel MapSize
        {
            get => _mapsize;
            set
            {
                _mapsize = value;
                _model.fMapType = value.MapType;
                OnPropertyChanged();
            }
        }


        public string WhosRound { get { return _model.Player == Player.Green ? "Zöld" : "Piros"; } }

        public int TableSize
        {
            get => _tableSize;
            set
            {
                _tableSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GameTableRows));
                OnPropertyChanged(nameof(GameTableColumns));
            }
        }

        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), TableSize).ToArray());
        }

        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), TableSize).ToArray());
        }



        public event EventHandler? NewGame;
        public event EventHandler? LoadGame;


        public event EventHandler? SaveGame;


        public event EventHandler? ExitGame;

        private System.Drawing.Point p1;
        private System.Drawing.Point p2;

        public KameleonViewModel(KameleonGameModel model)
        {

            p1 = new System.Drawing.Point(-1, -1);
            p2 = new System.Drawing.Point(-1, -1);

            _model = model;
            _model.GameAdvanced += new EventHandler<KameleonEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<KameleonEventArgs>(Model_GameOver);
            _model.GameCreated += new EventHandler<KameleonEventArgs>(Model_GameCreated);
            _model.Change += new EventHandler<ChangeEventArgs>(Model_Change);
            _model.SuccessStep += new EventHandler<TwoPlayerArgs>(Model_SuccessStep);
            _model.FailureStep += new EventHandler<KameleonEventArgs>(Model_FailureStep);

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            

            MapSizes = new ObservableCollection<GameMapSizeViewModel>
            {
                new GameMapSizeViewModel { MapType = MapType.Little},
                new GameMapSizeViewModel { MapType = MapType.Medium },
                new GameMapSizeViewModel { MapType = MapType.Large }
            };
            MapSize = MapSizes[1];

            TableSize = _model.Map.MapSize;


            Fields = new ObservableCollection<KameleonField>();

            for (int i = 0; i < _model.Map.MapSize; i++)
            {
                for (int j = 0; j < _model.Map.MapSize; j++)
                {
                    Color a = new Color();
                    if (_model.Map.getFieldsColor(i, j) == Kameleon3.Persistence.Color.Red)
                    {
                        a = Colors.Red;
                    }
                    else if (_model.Map.getFieldsColor(i, j) == Kameleon3.Persistence.Color.Green)
                    {
                        a = Colors.Green;
                    }
                    else
                    {
                        a = Colors.White;
                    }

                    Fields.Add(new KameleonField
                    {
                        IsLocked = false,
                        Color = a,
                        X = i,
                        Y = j,
                        Position = i * _model.Map.MapSize + j,

                        StepCommand = new DelegateCommand(param => Choose(Convert.ToInt32(param)))

                    });
                }
            }

            RefreshTable();
        }

        private void Choose(int x)
        {
            if (p1.X == -1)
            {
                KameleonField f = Fields[x];
                if (_model.Map.isEmpty(new System.Drawing.Point(f.X, f.Y))) return;
                p1.X = f.X;
                p1.Y = f.Y;
            }
            else
            {
                KameleonField fa = Fields[x];
                p2.X = fa.X;
                p2.Y = fa.Y;

                if (p1 != p2)
                {
                    StepGame(p1, p2);
                }

                p1 = new System.Drawing.Point(-1, -1);
                p2 = new System.Drawing.Point(-1, -1);

            }
        }

        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshTable()
        {
            foreach (KameleonField field in Fields)
            {
                if (_model.Map.getFieldsPlayer(field.X, field.Y) == Kameleon3.Persistence.Color.Red)
                {

                    field.Player = "Piros";
                }
                else if (_model.Map.getFieldsPlayer(field.X, field.Y) == Kameleon3.Persistence.Color.Green)
                {
                    field.Player = "Zöld";
                }
                else
                {
                    field.Player = String.Empty;
                }
            }

        }

        private void StepGame(System.Drawing.Point x, System.Drawing.Point y)
        {
            _model.Step(x, y);

        }

        private void Model_FailureStep(object? sender, KameleonEventArgs e)
        {

        }

        private void Model_SuccessStep(object? sender, TwoPlayerArgs e)
        {
            _model.Advance();
        }

        private void Model_Change(object? sender, ChangeEventArgs e)
        {
            RefreshTable();
        }

        private void Model_GameCreated(object? sender, KameleonEventArgs e)
        {


            TableSize = _model.Map.MapSize;
            Fields.Clear();

            for (int i = 0; i < _model.Map.MapSize; i++)
            {
                for (int j = 0; j < _model.Map.MapSize; j++)
                {
                    Color a = new Color();
                    if (_model.Map.getFieldsColor(i, j) == Kameleon3.Persistence.Color.Red)
                    {
                        a = Colors.Red;
                    }
                    else if (_model.Map.getFieldsColor(i, j) == Kameleon3.Persistence.Color.Green)
                    {
                        a = Colors.Green;
                    }
                    else
                    {
                        a = Colors.White;
                    }

                    Fields.Add(new KameleonField
                    {
                        IsLocked=false,
                        Color = a,
                        X = i,
                        Y = j,
                        Position = i * _model.Map.MapSize + j,

                        StepCommand = new DelegateCommand(param => Choose(Convert.ToInt32(param)))

                    });
                }
            }

            OnPropertyChanged(nameof(WhosRound));
            RefreshTable();

        }

        private void Model_GameOver(object? sender, KameleonEventArgs e)
        {
            foreach (KameleonField field in Fields)
            {
                field.IsLocked = true;
            }
        }

        private void Model_GameAdvanced(object? sender, KameleonEventArgs e)
        {
            RefreshTable();
            OnPropertyChanged(nameof(WhosRound));
        }
    }
}
