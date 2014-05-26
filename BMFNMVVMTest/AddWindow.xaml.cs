using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BMFNMVVMTest.Model;
using BMFNMVVMTest.Parser;
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
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBoxSelectTypeReport_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ((AddViewModel)this.DataContext).CreateFieldsForSelectedReportType(ComboBoxSelectTypeReport.SelectedItem, TreeViewNewObject);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (((AddViewModel)this.DataContext).CreateNewReport(ComboBoxSelectTypeReport.SelectedItem, TreeViewNewObject))
            {
                this.Close(); 
            }
            
        }
    }
}