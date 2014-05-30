using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BMFNMVVMTest.Model;
using BMFNMVVMTest.Parser;
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

        //Commans declaration
        private readonly ICommand selectTypeCommand ;

        public ICommand SelectTypeCommand
        {
            get { return selectTypeCommand; }
        }

        //Properties declaration
        private Type selectedReportType;

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

        private bool isReportTypeSelected = false;

        public bool IsReportTypeSelected
        {
            set
            {
                isReportTypeSelected = value;
                NotifyPropertyChanged("IsReportTypeSelected");
            }
            get { return isReportTypeSelected; }
        }

        /// <summary>
        /// Returtns a list of report types registered in the application
        /// </summary>
        public List<Type> GetTypes
        {
            get { return ReportsContext.ListTypes; }
        }

        /// <summary>
        /// Create TreeView with fields that user can use to create a new object with selected type
        /// </summary>
        public void CreateFieldsForSelectedReportType(object inReportType, TreeView viewElement)
        {
            if (inReportType == null)
            {
                return;
            }

            if (!(inReportType is Type))
            {
                return;
            }

            if (viewElement == null)
            {
                return;
            }

            Type inType = (Type) inReportType;

            viewElement.Items.Clear();

            TreeViewItem newTreeViewItem = new TreeViewItem();
            newTreeViewItem.Header = ((Type)inReportType).Name;
            newTreeViewItem.IsExpanded = true;

            viewElement.Items.Add(newTreeViewItem);

            ConstructTreeWithFields(inType, newTreeViewItem);

        }

        /// <summary>
        /// parses selected Type and creates controls for a new object
        /// </summary>
        private void ConstructTreeWithFields(Type inType, TreeViewItem inTreeViewItem)
        {
            const int TEXT_BOX_WIDTH = 200;

            try
            {
                foreach (PropertyInfo curPropertyInfo in inType.GetProperties())
                {

                    //struct
                    if (curPropertyInfo.PropertyType.IsValueType && !curPropertyInfo.PropertyType.IsEnum && !curPropertyInfo.PropertyType.IsPrimitive && curPropertyInfo.PropertyType != typeof(decimal))
                    {
                        TreeViewItem newTreeViewItem = new TreeViewItem();
                        newTreeViewItem.Header = String.Format("{0} (type - {1})",curPropertyInfo.Name, curPropertyInfo.PropertyType.Name);
                        newTreeViewItem.Name = curPropertyInfo.Name;
                        newTreeViewItem.Margin = new Thickness(0, 0, 0, 10);
                        newTreeViewItem.IsExpanded = true;

                        inTreeViewItem.Items.Add(newTreeViewItem);

                        ConstructTreeWithFields(curPropertyInfo.PropertyType, newTreeViewItem);

                        continue;
                    }

                    // simple types & strings
                    if ((curPropertyInfo.PropertyType.IsValueType) || (curPropertyInfo.PropertyType == typeof(System.String)))
                    {

                        TreeViewItem newTreeViewItem = new TreeViewItem();
                        newTreeViewItem.Header = String.Format("{0} (type - {1})", curPropertyInfo.Name, curPropertyInfo.PropertyType.Name); 
                        newTreeViewItem.Name = curPropertyInfo.Name;
                        newTreeViewItem.Margin = new Thickness(0,0,0,10);
                        newTreeViewItem.IsExpanded = true;

                        TextBox newTextBox = new TextBox();
                        newTextBox.Width = TEXT_BOX_WIDTH;

                        newTreeViewItem.Items.Add(newTextBox);

                        inTreeViewItem.Items.Add(newTreeViewItem);

                        continue;
                    }

                    //arrays & collections
                    if ((curPropertyInfo.PropertyType.IsArray) || (curPropertyInfo.PropertyType.GetInterface("ICollection") != null))
                    {
                        Type elementType;
                        if (curPropertyInfo.PropertyType.IsGenericType)
                        {
                            elementType = curPropertyInfo.PropertyType.GetGenericArguments().Single();
                        }
                        else
                        {
                            elementType = curPropertyInfo.PropertyType.GetElementType();
                        }

                        if (elementType == null)
                        {
                            elementType = typeof(Object);
                        }

                        TreeViewItem newTreeViewItem = new TreeViewItem();
                        newTreeViewItem.Header = String.Format("{0} (type - {1})", curPropertyInfo.Name, elementType.Name);
                        newTreeViewItem.Name = curPropertyInfo.Name;
                        newTreeViewItem.Margin = new Thickness(0, 0, 0, 10);
                        newTreeViewItem.IsExpanded = true;

                        Button addButton = new Button();
                        addButton.Content = "+";
                        addButton.Command = new AddCollectionElement(newTreeViewItem);

                        newTreeViewItem.Items.Add(addButton);

                        inTreeViewItem.Items.Add(newTreeViewItem);

                        continue;
                    }

                    //class
                    if (curPropertyInfo.PropertyType.IsClass)
                    {

                        TreeViewItem newTreeViewItem = new TreeViewItem();
                        newTreeViewItem.Header = String.Format("{0} (type - {1})", curPropertyInfo.Name, curPropertyInfo.PropertyType.Name); 
                        newTreeViewItem.Name = curPropertyInfo.Name;
                        newTreeViewItem.Margin = new Thickness(0, 0, 0, 10);
                        newTreeViewItem.IsExpanded = true;

                        inTreeViewItem.Items.Add(newTreeViewItem);

                        ConstructTreeWithFields(curPropertyInfo.PropertyType, newTreeViewItem);

                        continue;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Create New report with data from user's fields
        /// </summary>
        public bool CreateNewReport(object inReportType, TreeView viewElement)
        {
            if (inReportType == null)
            {
                return false;
            }

            if (!(inReportType is Type))
            {
                return false;
            }

            if (viewElement == null)
            {
                return false;
            }

            if (viewElement.Items.Count == 0)
            {
                return false;
            }

            Type inType = (Type)inReportType;

            try
            {
                var newReport = Activator.CreateInstance(inType);

                TreeViewItem firstTreeViewItem = (TreeViewItem)viewElement.Items.GetItemAt(0);

                Dictionary<string, Type> reportMap = ReportsContext.ReportsParser.GetReportMap(inType);

                Dictionary<string, object> parsedData = new Dictionary<string, object>();

                bool isAllFieldsFilledRight = true;
                ReportsContext.ReportsParser.parseFilledFields(firstTreeViewItem, reportMap, parsedData, ref isAllFieldsFilledRight);

                if (isAllFieldsFilledRight)
                {
                    newReport = ReportsContext.ReportsParser.CreateObject(inType, parsedData);
                    ReportsContext.TestData.Add(newReport);
                    return true;
                }
                else
                {
                    MessageBox.Show("Some fields are filled wrong!");
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
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
        public new event PropertyChangedEventHandler PropertyChanged = delegate { };

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

    internal class AddCollectionElement : ICommand
    {
        private const int TEXT_BOX_WIDTH = 200;

        private TreeViewItem collectionTreeViewItem;

        public AddCollectionElement(TreeViewItem collectionTreeViewItem)
        {
            this.collectionTreeViewItem = collectionTreeViewItem;
        }

        public bool CanExecute(object parameter)
        {
            //return IsReportTypeSelected;
            return true;
        }

        public void Execute(object parameter)
        {
            TextBox newTextBox = new TextBox();
            newTextBox.Width = TEXT_BOX_WIDTH;

            collectionTreeViewItem.Items.Add(newTextBox);

        }

        public event EventHandler CanExecuteChanged = delegate { };
    }

}