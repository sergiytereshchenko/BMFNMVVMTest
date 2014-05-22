using System.Windows;
using BMFNMVVMTest.Model;
using BMFNMVVMTest.ViewModel;

namespace BMFNMVVMTest
{
    /// <summary>
    /// Description for AddWindow.
    /// </summary>
    public partial class AddWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the AddWindow class.
        /// </summary>
        public AddWindow()
        {
            InitializeComponent();
            this.DataContext = new AddViewModel();

           // ComboBoxSelectTypeReport.ItemsSource = ((AddViewModel)this.DataContext).GetTypes;
            //ComboBoxSelectTypeReport.ItemsSource = ReportsContext.ListTypes;

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}