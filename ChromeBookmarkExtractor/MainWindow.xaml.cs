using System.IO;
using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ChromeBookmarkExtractor
{
    public partial class MainWindow
    {
        private readonly string _initialDirectory;

        private readonly string _initialDestinationDirectory;

        public MainWindow()
        {
            InitializeComponent();

            var p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;

            _initialDirectory = $"C:\\Users\\{p?.Identity.Name}\\AppData\\Local\\Google\\Chrome\\User Data\\Default";

            _initialDestinationDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            if (!Directory.Exists(_initialDirectory))
                _initialDirectory = Path.GetDirectoryName(_initialDestinationDirectory);

            TB_FilePath.Text = _initialDirectory + @"\Bookmarks";

            TB_DestinationPath.Text = _initialDirectory;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
            var file = new OpenFileDialog { FileName = "Bookmarks", InitialDirectory = _initialDirectory };
            if (file.ShowDialog() == true)
            {
                TB_FilePath.Text = file.FileName;

                BT_Extract.IsEnabled = !string.IsNullOrEmpty(TB_FilePath.Text);
            }
        }


        private void DestinationSelection_OnClick(object sender, RoutedEventArgs e)
        {
            var file = new FolderBrowserDialog() {SelectedPath = _initialDirectory};

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TB_DestinationPath.Text = file.SelectedPath;
            }
        }

        private void Extract_OnClick(object sender, RoutedEventArgs e)
        {
            Info.Text = ChromeBookmarkExtractorService.Extract(TB_FilePath.Text, TB_DestinationPath.Text);
        }
    }
}
