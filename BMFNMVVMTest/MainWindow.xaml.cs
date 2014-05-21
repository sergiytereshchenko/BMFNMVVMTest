using System.Windows;
using BMFNMVVMTest.ViewModel;

namespace BMFNMVVMTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}