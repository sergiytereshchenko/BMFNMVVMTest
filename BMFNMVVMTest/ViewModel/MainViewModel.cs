using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
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

        private Rectangle searchCurtain;

        public Rectangle SearchCurtain
        {
            get { return searchCurtain; }
            set { searchCurtain = value; }
        }

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
            this.searchString = null;
            Search();
        }

        //Methods

        /// <summary>
        /// Search in the datasource.
        /// </summary>
        public void Search()
        {
            Thread thread = new Thread(searchItems);
            thread.Name = "Search thread";
            thread.Start();
        }

        /// <summary>
        /// Hides UI with unvisible rectangle djuring search procedure.
        /// Invokes the method which creates collection of founded items. 
        /// </summary>
        private void searchItems()
        {
            try
            {
                if (Application.Current.Dispatcher!=null)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                                (ThreadStart)delegate()
                                {
                                    Canvas.SetZIndex(searchCurtain, 99);
                                    Keyboard.Focus(searchCurtain);

                                    isSearchInProgress = true;
                                }
                                  ); 
                }


                ObservableCollection<Object> newSearchResult = findItems();

                if (Application.Current.Dispatcher != null)
                {

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                        (ThreadStart) delegate()
                        {
                            foundedItems.Clear();

                            foreach (object curReport in newSearchResult)
                            {
                                foundedItems.Add(curReport);
                            }

                            Canvas.SetZIndex(searchCurtain, 0);

                            isSearchInProgress = false;
                        }
                        );
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Returns collection of founded items. 
        /// </summary>
        private ObservableCollection<Object> findItems()
        {
            ObservableCollection<Object> newSearchResult = new ObservableCollection<object>();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            try
            {
                 if (String.IsNullOrEmpty(this.searchString))
                {
                    foreach (var curReport in ReportsContext.TestData)
                    {
                        newSearchResult.Add(curReport);
                    }
                }
                else
                {
                    Boolean isFounded;
                    foreach (var curReport in ReportsContext.TestData)
                    {
                        isFounded = false;
                        ReportsContext.ReportsParser.FindStringInReport(curReport, this.searchString, ref isFounded);

                        if (isFounded)
                        {
                            newSearchResult.Add(curReport);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }

            return newSearchResult;
        }

        /// <summary>
        /// Shows message if search procedure in progress. 
        /// </summary>
        public void SearchMessage()
        {
            if (IsSearchInProgress)
            {
                MessageBox.Show("Search is in progress now and this operation will be available after its ending");
            }
        }
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
            thisViewModel.Search();
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }

}