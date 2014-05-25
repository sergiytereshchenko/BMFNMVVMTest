using System;
using System.Collections.ObjectModel;
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

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }

            set
            {
                if (_welcomeTitle == value)
                {
                    return;
                }

                _welcomeTitle = value;
                RaisePropertyChanged(WelcomeTitlePropertyName);
            }
        }

        private ObservableCollection<Object> foundedItems = new ObservableCollection<Object>();

        public ObservableCollection<object> FoundedItems
        {
            get { return foundedItems; }
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

                    WelcomeTitle = item.Title;
                });

            //Initialisation for the ReportsContext class which implemets singleton pattern
            ReportsContext intitalRC = ReportsContext.Instance;

            //Initial filing ListBox in the MainWindow
            findItems(null);

        }


        //Commands
        public void Search(string searchString)
        {
            findItems(searchString);
        }



        //Methods
        private void findItems(string searchString)
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







        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}