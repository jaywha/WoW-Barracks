using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ArgentPonyWarcraftClient;

namespace WpfPlayground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string DEFAULT_CHAR_IMAGE_SAVE_LOC = @"c:\temp\<CHAR>_image.jpeg";
        private const string DEFAULT_ITEM_URL = @"https://render-us.worldofwarcraft.com/icons/56/<ITEM>.jpg";

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion

        #region Properties
        private ObservableCollection<ItemModel> _gridItems = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> GridItems
        {
            get => _gridItems;
            set {
                _gridItems = value;
                OnPropertyChanged();
            }
        }

        private ItemModel _selectedItem;
        public ItemModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private string _characterName;
        public string CharacterName
        {
            get { return _characterName; }
            set {
                _characterName = value;
                OnPropertyChanged();
            }
        }

        private string _characterLevel;
        public string CharacterLevel
        {
            get { return _characterLevel; }
            set
            {
                _characterLevel = value;
                OnPropertyChanged();
            }
        }

        private string _characterItemLevel;
        public string CharacterItemLevel
        {
            get { return _characterItemLevel; }
            set
            {
                _characterItemLevel = value;
                OnPropertyChanged();
            }
        }

        private Brush _itemLevelColor;
        public Brush ItemLevelColor
        {
            get { return _itemLevelColor; }
            set {
                _itemLevelColor = value;
                OnPropertyChanged();
            }
        }


        private ImageSource _characterImageUriString;
        public ImageSource CharacterImage
        {
            get => _characterImageUriString;
            set {
                _characterImageUriString = value;
                OnPropertyChanged();
            }
        }

        private string _realmName;
        public string RealmName
        {
            get => _realmName;
            set {
                _realmName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WndTestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var imageClient = new WebClient())
            {
                imageClient.DownloadFile(@"https://us.battle.net/wow/static/images/2d/avatar/1-0.jpg", DEFAULT_CHAR_IMAGE_SAVE_LOC.Replace("<CHAR>", "Default"));

                CharacterImage = new BitmapImage(new Uri(DEFAULT_CHAR_IMAGE_SAVE_LOC.Replace("<CHAR>", "Default")));
            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string clientId = "c3ec638cb01b46c1b67b6e92276f99d7";
            string clientSecret = "XQ3ITx7TNofhVlEIYqgTlo1yehqkJw1D";
            var warcraftClient = new WarcraftClient(clientId, clientSecret, Region.US, Locale.en_US);

            RequestResult<Character> result = await warcraftClient.GetCharacterAsync(RealmName, CharacterName, CharacterFields.All);

            if (result.Success)
            {
                var ilvl = result.Value.Items.AverageItemLevel;

                CharacterLevel = result.Value.Level + "";
                CharacterItemLevel = ilvl + "";

                if(ilvl >= 355 && ilvl < 370)
                {
                    ItemLevelColor = Brushes.Blue;
                } else if(ilvl >= 370)
                {
                    ItemLevelColor = Brushes.Violet;
                } else
                {
                    ItemLevelColor = Brushes.Black;
                }

                using(var imageClient = new WebClient())
                {
                    imageClient.DownloadFile(@"http://render-us.worldofwarcraft.com/character/" + result.Value.Thumbnail, DEFAULT_CHAR_IMAGE_SAVE_LOC.Replace("<CHAR>", CharacterName));

                    CharacterImage = new BitmapImage(new Uri(DEFAULT_CHAR_IMAGE_SAVE_LOC.Replace("<CHAR>", CharacterName)));
                }

                Console.WriteLine(result.Value.Items.Back.Icon);
            }
            else
            {
                Console.WriteLine("HTTP Status Code: " + result.Error.Code);
                Console.WriteLine("HTTP Status Description: " + result.Error.Type);
                Console.WriteLine("Details: " + result.Error.Detail);
            }
        }

        private void Keyboard_Shortcuts(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                btnSearch_Click(btnSearch, null);
            }
        }
    }
}
