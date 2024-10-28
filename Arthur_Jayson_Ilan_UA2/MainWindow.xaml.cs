using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arthur_Jayson_Ilan_UA2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage? _backgroundImage;
        public MainWindow()
        {
            InitializeComponent();
            PreloadBackgroundImage();
        }

        private void PreloadBackgroundImage()
        {
            _backgroundImage = new BitmapImage();
            _backgroundImage.BeginInit();
            _backgroundImage.UriSource = new Uri("pack://application:,,,/Assets/Images/img_background.jpg", UriKind.Absolute);
            _backgroundImage.CacheOption = BitmapCacheOption.OnLoad;
            _backgroundImage.EndInit();
            _backgroundImage.Freeze();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadBackgroundImageAsync();
        }

        private async Task LoadBackgroundImageAsync()
        {
            await Task.Run(() =>
            {
                var image = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/img_background.jpg", UriKind.Absolute));
                image.Freeze();
                Dispatcher.Invoke(() =>
                {
                    this.Background = new ImageBrush(image)
                    {
                        Stretch = Stretch.UniformToFill
                    };
                });
            });
        }
    }
}