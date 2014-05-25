using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            try
            {
                foreach (PropertyInfo curPropertyInfo in inType.GetProperties())
                {

                    //struct
                    if (curPropertyInfo.PropertyType.IsValueType && !curPropertyInfo.PropertyType.IsEnum && !curPropertyInfo.PropertyType.IsPrimitive && curPropertyInfo.PropertyType != typeof(decimal))
                    {
                        TreeViewItem newTreeViewItem = new TreeViewItem();
                        newTreeViewItem.Header = curPropertyInfo.Name;
                        newTreeViewItem.Name = curPropertyInfo.Name;
                        newTreeViewItem.IsExpanded = true;

                        inTreeViewItem.Items.Add(newTreeViewItem);

                        ConstructTreeWithFields(curPropertyInfo.PropertyType, newTreeViewItem);


                        continue;
                    }

                    // simple types & strings
                    if ((curPropertyInfo.PropertyType.IsValueType) || (curPropertyInfo.PropertyType == typeof(System.String)))
                    {

                        TreeViewItem newTreeViewItem = new TreeViewItem();
                        newTreeViewItem.Header = curPropertyInfo.Name;
                        newTreeViewItem.Name = curPropertyInfo.Name;
                        newTreeViewItem.IsExpanded = true;

                        TextBox newTextBox = new TextBox();
                        //newTextBox.Name = curPropertyInfo.Name;

                        newTreeViewItem.Items.Add(newTextBox);

                        inTreeViewItem.Items.Add(newTreeViewItem);

                        continue;
                    }

                    //arrays & collections
                    if ((curPropertyInfo.PropertyType.IsArray) || (curPropertyInfo.PropertyType.GetInterface("ICollection") != null))
                    {

                        TreeViewItem newTreeViewItem = new TreeViewItem();
                        newTreeViewItem.Header = curPropertyInfo.Name;
                        newTreeViewItem.Name = curPropertyInfo.Name;
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
                        newTreeViewItem.Header = curPropertyInfo.Name;
                        newTreeViewItem.Name = curPropertyInfo.Name;
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
        public void CreateNewReport(object inReportType, TreeView viewElement, ListBox TestList)
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

            if (viewElement.Items.Count == 0)
            {
                return;
            }

            Type inType = (Type)inReportType;

            

            try
            {
                var newReport = Activator.CreateInstance(inType);

                TreeViewItem firstTreeViewItem = (TreeViewItem)viewElement.Items.GetItemAt(0);

                Dictionary<string, Type> reportMap = ReportsContext.ReportsParser.GetReportMap(inType);



                foreach (KeyValuePair<string, Type> keyValuePair in reportMap)
                {
                    Label lb = new Label();
                    lb.Content = String.Format("{0} {1}", keyValuePair.Key, keyValuePair.Value);
                    TestList.Items.Add(lb);
                }

                Label lb1 = new Label();
                lb1.Content = String.Format("---------------------");
                TestList.Items.Add(lb1);

                Dictionary<string, object> parsedData = new Dictionary<string, object>();
                parseFilledFields(firstTreeViewItem, reportMap, parsedData);

                foreach (KeyValuePair<string, object> keyValuePair in parsedData)
                {
                    Label lb = new Label();
                    lb.Content = String.Format("{0} {1}", keyValuePair.Key, keyValuePair.Value);
                    TestList.Items.Add(lb);

                }

                newReport = ReportsContext.ReportsParser.CreateObject(inType, parsedData);

                ReportsContext.TestData.Add(newReport);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void parseFilledFields(TreeViewItem inTreeViewItem, Dictionary<string, Type> inReportMap,
            Dictionary<string, object> inDictionary, string inPath="")
        {

            foreach (Control curTreeControl in inTreeViewItem.Items)
            {
                if (!(curTreeControl is TreeViewItem))
                {
                    continue;
                }

                TreeViewItem curTreeViewItem = (TreeViewItem)curTreeControl;

                string curFieldName;

                if (String.IsNullOrEmpty(inPath))
                {
                    curFieldName = curTreeViewItem.Name;
                }
                else
                {
                    curFieldName = String.Format("{0}{1}", inPath, curTreeViewItem.Name);
                }

                Type curType;
                object curObject = null;

                inReportMap.TryGetValue(curFieldName, out curType);

                if (curType == null)
                {
                    continue;
                }

                //struct
                if ((curType.IsValueType && !curType.IsEnum && !curType.IsPrimitive && curType != typeof(decimal))||(curType.IsClass))
                {
                    parseFilledFields(curTreeViewItem, inReportMap, inDictionary, curFieldName);
                }

                // simple types & strings
                if ((curType.IsValueType) || (curType == typeof(System.String)))
                {
                    foreach (Control curControl in curTreeViewItem.Items)
                    {
                        if (curControl is TextBox)
                        {
                            curObject = ((TextBox)curControl).Text;
                            break;
                        }
                    }
                }
                
                //arrays & collections
                if ((curType.IsArray) || (curType.GetInterface("ICollection") != null))
                {
                    List<string> curList = new List<string>();

                    foreach (Control curControl in curTreeViewItem.Items)
                    {
                        if (curControl is TextBox)
                        {
                            curList.Add(((TextBox)curControl).Text);
                        }
                    }

                    curObject = curList;
                }

                inDictionary.Add(curFieldName, curObject);
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

            collectionTreeViewItem.Items.Add(newTextBox);
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }

}