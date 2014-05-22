using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using BMFNMVVMTest.Model;
using GalaSoft.MvvmLight;



namespace BMFNMVVMTest.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private bool isReportTypeSelected = false;
        private Type selectedReportType;
        private readonly ICommand selectTypeCommand ;

        public bool IsReportTypeSelected
        {
            set
            {
                isReportTypeSelected = value;
                NotifyPropertyChanged("IsReportTypeSelected");
            }
            get { return isReportTypeSelected; }
        }

        public Type SelectedReportType
        {
            get { return selectedReportType; }
            set
            {
                if (value == null)
                {
                    isReportTypeSelected = false;
                    return;
                }

                selectedReportType = value;
                IsReportTypeSelected = true;
                NotifyPropertyChanged("SelectedReportType");

            }
        }

        public ICommand SelectTypeCommand
        {
            get { return selectTypeCommand; }
        }


        /// <summary>
        /// Returtns a list of report types registered in the application
        /// </summary>
        public List<Type> GetTypes
        {
            get { return ReportsContext.ListTypes; }
        }

        private void createFieldsForNewReport()
        {



        }


        /// <summary>
        /// Initializes a new instance of the AddViewModel class.
        /// </summary>
        public AddViewModel()
        {
            IsReportTypeSelected = false;
            selectTypeCommand = new SelectTypeCommand(this);
        }

        /// <summary>
        /// ////////////////////////////////////////////////
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void NotifyPropertyChanged(
            string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        //////////////////////////////////////////////

    }

    internal class SelectTypeCommand : ICommand
    {
        private AddViewModel viewModelOwner;

        public SelectTypeCommand(AddViewModel viewModelOwner)
        {
            this.viewModelOwner = viewModelOwner;
        }

        public bool CanExecute(object parameter)
        {
            //return viewModelOwner.IsReportTypeSelected;
            return true;
        }

        public void Execute(object parameter)
        {
            MessageBox.Show("asda");
        }

        public event EventHandler CanExecuteChanged  = delegate { };
    }
}