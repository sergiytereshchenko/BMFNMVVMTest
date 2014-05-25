using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using BMFNMVVMTest.Model;

namespace BMFNMVVMTest.Parser
{
    public class ReportsParser
    {
        private Dictionary<Type, DataTemplate> reportsTemplates;

        private Dictionary<Type, Dictionary<string, Type>> reportsMaps;

        private Dictionary<Type, DataTemplate> ReportsTemplates
        {
            get { return reportsTemplates; }
        }

        private Dictionary<Type, Dictionary<string, Type>> ReportsMaps
        {
            get { return reportsMaps; }
        }

        public ReportsParser()
        {
            reportsTemplates = new Dictionary<Type, DataTemplate>();
            reportsMaps = new Dictionary<Type, Dictionary<string, Type>>();
        }

        //Get DataTemplate for show object of specified type
        public DataTemplate GetDataTemplate(Type inType)
        {
            if (!reportsTemplates.ContainsKey(inType))
            {
                DataTemplate newDataTemplate = new DataTemplate();
                newDataTemplate.DataType = inType;

                FrameworkElementFactory borderElement = new FrameworkElementFactory(typeof(Border));
                borderElement.SetValue(Border.BorderBrushProperty, Brushes.Blue);
                borderElement.SetValue(Border.BorderThicknessProperty, new Thickness(3));
                borderElement.SetValue(Border.MarginProperty, new Thickness(0, 3, 0, 3));

                FrameworkElementFactory gridElement = new FrameworkElementFactory(typeof(Grid));
                gridElement.AppendChild(new FrameworkElementFactory(typeof(ColumnDefinition)));
                gridElement.AppendChild(new FrameworkElementFactory(typeof(ColumnDefinition)));

                int curGridRow = 0;

                parseReportDataTemplate(inType, gridElement, ref curGridRow, "");

                borderElement.AppendChild(gridElement);
                newDataTemplate.VisualTree = borderElement;

                reportsTemplates.Add(inType, newDataTemplate);

                return newDataTemplate;
            }

            DataTemplate curDataTemplate;
            reportsTemplates.TryGetValue(inType, out curDataTemplate);

            return curDataTemplate;
        }

        //Parse type for specified type
        private void parseReportDataTemplate(Type inType, FrameworkElementFactory inGrid, ref int curGridRow, string Parents)
        {
            //DataTemplate curTemplate;
            foreach (PropertyInfo curPropertyInfo in inType.GetProperties())
            {

                string curPath;
                if (String.IsNullOrEmpty(Parents))
                {
                    curPath = String.Format("{0}", curPropertyInfo.Name);
                }
                else
                {
                    curPath = String.Format("{0}.{1}", Parents, curPropertyInfo.Name);
                }

                
                //struct
                if (curPropertyInfo.PropertyType.IsValueType && !curPropertyInfo.PropertyType.IsEnum && !curPropertyInfo.PropertyType.IsPrimitive && curPropertyInfo.PropertyType != typeof(decimal))
                {

                    parseReportDataTemplate(curPropertyInfo.PropertyType, inGrid, ref curGridRow, curPath);

                    continue;
                }

                ////enum
                //if (curPropertyInfo.PropertyType.IsEnum)
                //{
                //    parseReportDataTemplate(curPropertyInfo.PropertyType);

                //    continue;
                //}

                // simple types
                if (curPropertyInfo.PropertyType.IsValueType)
                {
                    inGrid.AppendChild(new FrameworkElementFactory(typeof(RowDefinition)));

                    FrameworkElementFactory curTxtxBlock;

                    curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
                    curTxtxBlock.SetValue(Grid.RowProperty, curGridRow);
                    curTxtxBlock.SetValue(Grid.ColumnProperty, 0);
                    curTxtxBlock.SetValue(TextBlock.TextProperty, curPath);
                    inGrid.AppendChild(curTxtxBlock);

                    curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
                    curTxtxBlock.SetValue(Grid.RowProperty, curGridRow);
                    curTxtxBlock.SetValue(Grid.ColumnProperty, 1);
                    curTxtxBlock.SetBinding(TextBlock.TextProperty, new Binding(curPath));
                    inGrid.AppendChild(curTxtxBlock);

                    curGridRow++;

                    continue;
                }

                //string
                if (curPropertyInfo.PropertyType == typeof(System.String))
                {
                    inGrid.AppendChild(new FrameworkElementFactory(typeof(RowDefinition)));

                    FrameworkElementFactory curTxtxBlock;

                    curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
                    curTxtxBlock.SetValue(Grid.RowProperty, curGridRow);
                    curTxtxBlock.SetValue(Grid.ColumnProperty, 0);
                    curTxtxBlock.SetValue(TextBlock.TextProperty, curPath);
                    inGrid.AppendChild(curTxtxBlock);

                    curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
                    curTxtxBlock.SetValue(Grid.RowProperty, curGridRow);
                    curTxtxBlock.SetValue(Grid.ColumnProperty, 1);
                    curTxtxBlock.SetBinding(TextBlock.TextProperty, new Binding(curPath));
                    inGrid.AppendChild(curTxtxBlock);

                    curGridRow++;

                    continue;
                }

                //array
                if (curPropertyInfo.PropertyType.IsArray)
                {
                    inGrid.AppendChild(new FrameworkElementFactory(typeof(RowDefinition)));

                    FrameworkElementFactory curTxtxBlock;

                    curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
                    curTxtxBlock.SetValue(Grid.RowProperty, curGridRow);
                    curTxtxBlock.SetValue(Grid.ColumnProperty, 0);
                    curTxtxBlock.SetValue(TextBlock.TextProperty, curPath);
                    inGrid.AppendChild(curTxtxBlock);

                    curTxtxBlock = new FrameworkElementFactory(typeof(ListBox));
                    curTxtxBlock.SetValue(Grid.RowProperty, curGridRow);
                    curTxtxBlock.SetValue(Grid.ColumnProperty, 1);
                    curTxtxBlock.SetBinding(ListBox.ItemsSourceProperty, new Binding(curPath));
                    inGrid.AppendChild(curTxtxBlock);

                    curGridRow++;

                    continue;
                }

                //collection
                if (curPropertyInfo.PropertyType.GetInterface("ICollection") != null)
                {
                    inGrid.AppendChild(new FrameworkElementFactory(typeof(RowDefinition)));

                    FrameworkElementFactory curTxtxBlock;

                    curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
                    curTxtxBlock.SetValue(Grid.RowProperty, curGridRow);
                    curTxtxBlock.SetValue(Grid.ColumnProperty, 0);
                    curTxtxBlock.SetValue(TextBlock.TextProperty, curPath);
                    inGrid.AppendChild(curTxtxBlock);

                    curTxtxBlock = new FrameworkElementFactory(typeof(ListBox));
                    curTxtxBlock.SetValue(Grid.RowProperty, curGridRow);
                    curTxtxBlock.SetValue(Grid.ColumnProperty, 1);
                    curTxtxBlock.SetBinding(ListBox.ItemsSourceProperty, new Binding(curPath));
                    inGrid.AppendChild(curTxtxBlock);

                    curGridRow++;

                    continue;
                }


                //class
                if (curPropertyInfo.PropertyType.IsClass)
                {
                    parseReportDataTemplate(curPropertyInfo.PropertyType, inGrid, ref curGridRow, curPath);

                    continue;
                }
            }
        }

        //Get a list of specified report's fields names and types for specified type for creation a new report
        public Dictionary<string, Type> GetReportMap(Type inType)
        {
            Dictionary<string, Type> curFieldsDictionary;

            if (!reportsMaps.ContainsKey(inType))
            {

                curFieldsDictionary = new Dictionary<string, Type>();

                parseReportFields(inType, curFieldsDictionary, "");

                reportsMaps.Add(inType, curFieldsDictionary);

                return curFieldsDictionary;

            }

            reportsMaps.TryGetValue(inType, out curFieldsDictionary);

            return curFieldsDictionary;
        }

        private void parseReportFields(Type inType, Dictionary<string, Type> fieldsDictionary, string inPath)
        {

            foreach (PropertyInfo curPropertyInfo in inType.GetProperties())
            {
                string curName;

                if (String.IsNullOrEmpty(inPath))
                {
                    curName = curPropertyInfo.Name;
                }
                else
                {
                    curName = String.Format("{0}{1}", inPath, curPropertyInfo.Name);
                }
                
                //struct
                if (curPropertyInfo.PropertyType.IsValueType && !curPropertyInfo.PropertyType.IsEnum && !curPropertyInfo.PropertyType.IsPrimitive && curPropertyInfo.PropertyType != typeof(decimal))
                {
                    fieldsDictionary.Add(curName, curPropertyInfo.PropertyType);
                    parseReportFields(curPropertyInfo.PropertyType, fieldsDictionary, curName);

                    continue;
                }

                //////enum
                ////if (curPropertyInfo.PropertyType.IsEnum)
                ////{
                ////    parseReportDataTemplate(curPropertyInfo.PropertyType);

                ////    continue;
                ////}

                // simple types
                if (curPropertyInfo.PropertyType.IsValueType)
                {

                    fieldsDictionary.Add(curName, curPropertyInfo.PropertyType);

                    continue;
                }

                //string
                if (curPropertyInfo.PropertyType == typeof(System.String))
                {

                    fieldsDictionary.Add(curName, curPropertyInfo.PropertyType);

                    continue;
                }

                //array
                if (curPropertyInfo.PropertyType.IsArray)
                {
                    fieldsDictionary.Add(curName, curPropertyInfo.PropertyType);

                    continue;
                }

                //collection
                if (curPropertyInfo.PropertyType.GetInterface("ICollection") != null)
                {
                    fieldsDictionary.Add(curName, curPropertyInfo.PropertyType);

                    continue;
                }


                //class
                if (curPropertyInfo.PropertyType.IsClass)
                {
                    fieldsDictionary.Add(curName, curPropertyInfo.PropertyType);
                    parseReportFields(curPropertyInfo.PropertyType, fieldsDictionary, curName);

                    continue;
                }

            }

        }

        public object CreateObject(Type inType, Dictionary<string, object> fieldsData, string inPath = "")
        {
            object newReport = null; 

            try
            {
                newReport = Activator.CreateInstance(inType);

                //PropertyInfo propertyInfo = newReport.GetType().GetProperty(((TextBox)curTextBox).Name);

                //if (propertyInfo != null)
                //{
                //    Type curFieldType = propertyInfo.PropertyType;

                //    TypeConverter converter = TypeDescriptor.GetConverter(curFieldType);
                //    var result = converter.ConvertFrom(((TextBox)curTextBox).Text);

                //    propertyInfo.SetValue(newReport, result);
                //}


                foreach (PropertyInfo curPropertyInfo in inType.GetProperties())
                {

                    string curName;

                    if (String.IsNullOrEmpty(inPath))
                    {
                        curName = curPropertyInfo.Name;
                    }
                    else
                    {
                        curName = String.Format("{0}{1}", inPath, curPropertyInfo.Name);
                    }

                    object curValue = "";
                    Type curFieldType = curPropertyInfo.PropertyType;
                    
                    fieldsData.TryGetValue(curName, out curValue);

                    //struct
                    if (curPropertyInfo.PropertyType.IsValueType && !curPropertyInfo.PropertyType.IsEnum && !curPropertyInfo.PropertyType.IsPrimitive && curPropertyInfo.PropertyType != typeof(decimal))
                    {
                        object newObject = CreateObject(curFieldType, fieldsData, curName);
                        curPropertyInfo.SetValue(newReport, newObject);

                        continue;
                    }

                    // simple types
                    if (curPropertyInfo.PropertyType.IsValueType)
                    {
                        if (String.IsNullOrEmpty((String)curValue))
                        {
                            continue;
                        }

                        TypeConverter converter = TypeDescriptor.GetConverter(curFieldType);
                        var result = converter.ConvertFrom(curValue);

                        curPropertyInfo.SetValue(newReport, result);

                        continue;
                    }

                    //string
                    if (curPropertyInfo.PropertyType == typeof(System.String))
                    {
                        if (String.IsNullOrEmpty((String)curValue))
                        {
                            continue;
                        }

                        curPropertyInfo.SetValue(newReport, curValue);

                        continue;
                    }

                    //array
                    if (curPropertyInfo.PropertyType.IsArray)
                    {
                        object newArray = Activator.CreateInstance(curFieldType);
                        Type arrayType = newArray.GetType().GetElementType();

                        MessageBox.Show(arrayType.ToString());


                        //foreach (var curElement in (Array)curValue)
                        //{
                        //    Result = curElement.ToString().Equals(SearchString);
                        //    if (Result)
                        //    {
                        //        break;
                        //    }
                        //}

                        continue;
                    }

                    //collection
                    if (curPropertyInfo.PropertyType.GetInterface("ICollection") != null)
                    {
                        //foreach (var curElement in (ICollection)curValue)
                        //{
                        //    Result = curElement.ToString().Equals(SearchString);
                        //    if (Result)
                        //    {
                        //        break;
                        //    }
                        //}

                        continue;
                    }

                    //class
                    if (curPropertyInfo.PropertyType.IsClass)
                    {
                        object newObject = CreateObject(curFieldType, fieldsData, curName);
                        curPropertyInfo.SetValue(newReport, newObject);

                        continue;
                    }

                }


            }
            catch (Exception)
            {
                
                throw;
            }

            return newReport;
        }

        //Try to find a specified string in a report's field
        public void FindStringInReport(object inReport, string SearchString, ref Boolean Result)
        {
            Type inType = inReport.GetType();
            object curValue;

            foreach (PropertyInfo curPropertyInfo in inType.GetProperties())
            {
                if (Result)
                {
                    break;
                }

                try
                {
                    curValue = curPropertyInfo.GetValue(inReport);
                    if (curValue == null)
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                //struct
                if (curPropertyInfo.PropertyType.IsValueType && !curPropertyInfo.PropertyType.IsEnum && !curPropertyInfo.PropertyType.IsPrimitive && curPropertyInfo.PropertyType != typeof(decimal))
                {
                    FindStringInReport(curValue, SearchString, ref Result);

                    continue;
                }

                // simple types
                if (curPropertyInfo.PropertyType.IsValueType)
                {
                    Result = curValue.ToString().Equals(SearchString);

                    continue;
                }

                //string
                if (curPropertyInfo.PropertyType == typeof(System.String))
                {
                    Result = curValue.ToString().Equals(SearchString);

                    continue;
                }

                //array
                if (curPropertyInfo.PropertyType.IsArray)
                {
                    foreach (var curElement in (Array)curValue)
                    {
                        Result = curElement.ToString().Equals(SearchString);
                        if (Result)
                        {
                            break;
                        }
                    }

                    continue;
                }

                //collection
                if (curPropertyInfo.PropertyType.GetInterface("ICollection") != null)
                {
                    foreach (var curElement in (ICollection)curValue)
                    {
                        Result = curElement.ToString().Equals(SearchString);
                        if (Result)
                        {
                            break;
                        }
                    }

                    continue;
                }

                //class
                if (curPropertyInfo.PropertyType.IsClass)
                {
                    FindStringInReport(curValue, SearchString, ref Result);

                    continue;
                }

            }

        }



    }
}
