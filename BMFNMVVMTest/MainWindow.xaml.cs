using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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

            Canvas.SetZIndex(SearchCurtain, 100);
        }

        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("asdadasd");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(SearchCurtain, 0);
        }

    }
}