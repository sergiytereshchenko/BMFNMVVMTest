using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using BMFNMVVMTest.Model;
using GalaSoft.MvvmLight.Command;

namespace BMFNMVVMTest.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        //Commans declaration
        private readonly ICommand addWindowOpen;

        private readonly ICommand startSearch;

        public ICommand AddWindowOpen
        {
            get { return addWindowOpen; }
        }

        public ICommand StartSearch
        {
            get { return startSearch; }
        }

        //Properties declaration
        private ObservableCollection<Object> foundedItems = new ObservableCollection<Object>();

        public ObservableCollection<object> FoundedItems
        {
            get { return foundedItems; }
        }

        private string searchString;

        public string SearchString
        {
            get { return searchString; }
            set { searchString = value; }
        }

        private bool isSearchInProgress;

        public bool IsSearchInProgress
        {
            get { return isSearchInProgress; }
            set { isSearchInProgress = value; }
        }

        ///// <summary>
        ///// The <see cref="WelcomeTitle" /> property's name.
        ///// </summary>
        //public const string WelcomeTitlePropertyName = "WelcomeTitle";

        //private string _welcomeTitle = string.Empty;

        ///// <summary>
        ///// Gets the WelcomeTitle property.
        ///// Changes to that property's value raise the PropertyChanged event. 
        ///// </summary>
        //public string WelcomeTitle
        //{
        //    get
        //    {
        //        return _welcomeTitle;
        //    }

        //    set
        //    {
        //        if (_welcomeTitle == value)
        //        {
        //            return;
        //        }

        //        _welcomeTitle = value;
        //        RaisePropertyChanged(WelcomeTitlePropertyName);
        //    }
        //}


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    //WelcomeTitle = item.Title;
                });

            //Initialization for the ReportsContext class which implemets singleton pattern
            ReportsContext intitalRC = ReportsContext.Instance;

            //Commands initialization
            addWindowOpen = new AddWindowOpen();
            startSearch = new Search(this);

            //Initial filing ListBox in the MainWindow
            findItems(null);
        }
        
        public void Search(string searchString)
        {
            Thread.Sleep(5000);
            findItems(searchString);
        }

        //Methods
        private void findItems(string searchString)
        {
            try
            {
                FoundedItems.Clear();

                if (String.IsNullOrEmpty(searchString))
                {
                    foreach (var curReport in ReportsContext.TestData)
                    {
                        foundedItems.Add(curReport);
                    }
                }
                else
                {
                    Boolean isFounded;
                    foreach (var curReport in ReportsContext.TestData)
                    {
                        isFounded = false;
                        ReportsContext.ReportsParser.FindStringInReport(curReport, searchString, ref isFounded);

                        if (isFounded)
                        {
                            foundedItems.Add(curReport);
                        }
                    }
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }


        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }

    //Commands

    /// <summary>
    /// Opens the dialog window for creating a new report.
    /// </summary>
    internal class AddWindowOpen : ICommand
    {
        public AddWindowOpen()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }

    /// <summary>
    /// Makes search in the data source by search string
    /// </summary>
    internal class Search : ICommand
    {
        private MainViewModel thisViewModel;

        public Search(MainViewModel thisViewModel)
        {
            this.thisViewModel = thisViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            thisViewModel.Search(thisViewModel.SearchString);
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }


}