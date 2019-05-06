using ArgentPonyWarcraftClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPlayground
{
    /// <summary>
    /// Interaction logic for CharacterItemView.xaml
    /// </summary>
    public partial class CharacterItemView : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion

        public static readonly DependencyProperty ItemModelProperty 
            = DependencyProperty.Register("ItemModel", typeof(CharacterItem), typeof(CharacterItemView), new PropertyMetadata(null));

        [Category("Common"),Description("Main model for this item view.")]
        public CharacterItem ItemModel
        {
            get { return (CharacterItem)GetValue(ItemModelProperty); }
            set {
                SetValue(ItemModelProperty, value);
                OnPropertyChanged();
            }
        }

        private ImageSource _itemImageSource;
        public ImageSource ItemImageSource
        {
            get => _itemImageSource;
            set
            {
                _itemImageSource = value;
                OnPropertyChanged();
            }
        }

        public CharacterItemView() : this(null, null) {}

        public CharacterItemView(CharacterItem modelIn, ImageSource imageSourceIn)
        {
            InitializeComponent();
            if (modelIn != null) ItemModel = modelIn;
            if (imageSourceIn != null) ItemImageSource = imageSourceIn;
        }
    }
}
