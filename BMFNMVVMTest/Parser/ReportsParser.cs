﻿using System;
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

        public ReportsParser()
        {
            reportsTemplates = new Dictionary<Type, DataTemplate>();
            reportsMaps = new Dictionary<Type, Dictionary<string, Type>>();
        }

        /// <summary>
        /// Get DataTemplate for show object of specified type
        /// </summary>
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

                FrameworkElementFactory newTreeeView = new FrameworkElementFactory(typeof(TreeView));

                FrameworkElementFactory newTreeViewItem = new FrameworkElementFactory(typeof(TreeViewItem));
                newTreeViewItem.SetValue(TreeViewItem.HeaderProperty, inType.Name);
                newTreeViewItem.SetValue(TreeViewItem.IsExpandedProperty, true);
                newTreeeView.AppendChild(newTreeViewItem);

                int curGridRow = 0;

                parseReportDataTemplate(inType, newTreeViewItem, ref curGridRow, "");

                borderElement.AppendChild(newTreeeView);
                newDataTemplate.VisualTree = borderElement;

                reportsTemplates.Add(inType, newDataTemplate);

                return newDataTemplate;
            }

            DataTemplate curDataTemplate;
            reportsTemplates.TryGetValue(inType, out curDataTemplate);

            return curDataTemplate;
        }

        /// <summary>
        /// Parse type for specified type
        /// </summary>
        private void parseReportDataTemplate(Type inType, FrameworkElementFactory inTreeViewItem, ref int curGridRow, string Parents)
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

                    FrameworkElementFactory curTreeViewItem = new FrameworkElementFactory(typeof(TreeViewItem));
                    curTreeViewItem.SetValue(TreeViewItem.HeaderProperty, curPropertyInfo.Name);

                    inTreeViewItem.AppendChild(curTreeViewItem);

                    parseReportDataTemplate(curPropertyInfo.PropertyType, curTreeViewItem, ref curGridRow, curPath);

                    continue;
                }

                // simple types
                if ((curPropertyInfo.PropertyType.IsValueType)||(curPropertyInfo.PropertyType == typeof(System.String)))
                {

                    FrameworkElementFactory curTreeViewItem = new FrameworkElementFactory(typeof(TreeViewItem));
                    curTreeViewItem.SetValue(TreeViewItem.HeaderProperty, curPropertyInfo.Name);
                    curTreeViewItem.SetValue(TreeViewItem.IsExpandedProperty, true);

                    FrameworkElementFactory curTextBlock = new FrameworkElementFactory(typeof(TextBlock));
                    curTextBlock.SetBinding(TextBlock.TextProperty, new Binding(curPath));

                    curTreeViewItem.AppendChild(curTextBlock);
                    inTreeViewItem.AppendChild(curTreeViewItem);

                    continue;
                }

                //array
                if ((curPropertyInfo.PropertyType.IsArray)||(curPropertyInfo.PropertyType.GetInterface("ICollection") != null))
                {

                    FrameworkElementFactory curTreeViewItem = new FrameworkElementFactory(typeof(TreeViewItem));
                    curTreeViewItem.SetValue(TreeViewItem.HeaderProperty, curPropertyInfo.Name);

                    FrameworkElementFactory curElement = new FrameworkElementFactory(typeof(ListBox));
                    curElement.SetValue(ListBox.BorderBrushProperty, Brushes.Gold);
                    curElement.SetValue(ListBox.BorderThicknessProperty, new Thickness(3));
                    curElement.SetBinding(ListBox.ItemsSourceProperty, new Binding(curPath));

                    curTreeViewItem.AppendChild(curElement);
                    inTreeViewItem.AppendChild(curTreeViewItem);

                    continue;
                }

                //class
                if (curPropertyInfo.PropertyType.IsClass)
                {

                    FrameworkElementFactory curTreeViewItem = new FrameworkElementFactory(typeof(TreeViewItem));
                    curTreeViewItem.SetValue(TreeViewItem.HeaderProperty, curPropertyInfo.Name);

                    inTreeViewItem.AppendChild(curTreeViewItem);

                    parseReportDataTemplate(curPropertyInfo.PropertyType, curTreeViewItem, ref curGridRow, curPath);

                    continue;
                }
            }
        }

        /// <summary>
        /// Get a list of specified report's fields names and types for specified type for creation a new report
        /// </summary>
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

        /// <summary>
        /// Parses type for creating its map
        /// </summary>
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

        /// <summary>
        /// Parses fields filled by user 
        /// </summary>
        public void parseFilledFields(TreeViewItem inTreeViewItem, Dictionary<string, Type> inReportMap,
            Dictionary<string, object> inDictionary, ref bool isAllFieldsFilledRight, string inPath = "")
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
                if ((curType.IsValueType && !curType.IsEnum && !curType.IsPrimitive && curType != typeof(decimal)) || (curType.IsClass))
                {
                    parseFilledFields(curTreeViewItem, inReportMap, inDictionary, ref isAllFieldsFilledRight, curFieldName);
                }

                // simple types & strings
                if ((curType.IsValueType) || (curType == typeof(System.String)))
                {
                    foreach (Control curControl in curTreeViewItem.Items)
                    {
                        if (curControl is TextBox)
                        {
                            curObject = ((TextBox)curControl).Text;
                            checkTextBoxDataType((TextBox)curControl, curType, ref isAllFieldsFilledRight);
                            break;
                        }
                    }
                }

                //arrays & collections
                if ((curType.IsArray) || (curType.GetInterface("ICollection") != null))
                {
                    List<string> curList = new List<string>();

                    Type elementType = null;
                    if (curType.IsGenericType)
                    {
                        elementType = curType.GetGenericArguments().Single();
                    }
                    else
                    {
                        elementType = curType.GetElementType();
                    }

                    foreach (Control curControl in curTreeViewItem.Items)
                    {
                        if (curControl is TextBox)
                        {
                            curList.Add(((TextBox)curControl).Text);

                            checkTextBoxDataType((TextBox)curControl, elementType, ref isAllFieldsFilledRight);
                        }
                    }

                    curObject = curList;
                }

                inDictionary.Add(curFieldName, curObject);
            }
        }

        /// <summary>
        /// Checks a value in a textbox
        /// </summary>
        private void checkTextBoxDataType(TextBox inTextBox, Type inType, ref bool isFieldFilledRight)
        {
            if (String.IsNullOrEmpty(inTextBox.Text))
            {
                return;
            }

            if (inType != null)
            {
                try
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(inType);
                    var result = converter.ConvertFrom(inTextBox.Text);
                }
                catch (Exception)
                {
                    inTextBox.BorderBrush = Brushes.Red;
                    isFieldFilledRight = false;
                    return;
                }
            }

            inTextBox.BorderBrush = Brushes.YellowGreen;
        }

        /// <summary>
        /// Creates an object of specified type
        /// </summary>
        public object CreateObject(Type inType, Dictionary<string, object> fieldsData, string inPath = "")
        {
            object newReport = null; 

            try
            {
                newReport = Activator.CreateInstance(inType);

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
                        Type elementType = curPropertyInfo.PropertyType.GetElementType();
                        ArrayList list = new ArrayList();
                        TypeConverter converter = TypeDescriptor.GetConverter(elementType);

                        foreach (string curString in (List<string>)curValue)
                        {
                            if (String.IsNullOrEmpty(curString))
                            {
                                continue;
                            }

                            list.Add(converter.ConvertFrom(curString));
                        }

                        curPropertyInfo.SetValue(newReport, list.ToArray(elementType));

                        continue;
                    }

                    //collection
                    if (curPropertyInfo.PropertyType.GetInterface("ICollection") != null)
                    {

                        object newCollection = Activator.CreateInstance(curPropertyInfo.PropertyType);

                        Type elementType;

                        if (curPropertyInfo.PropertyType.IsGenericType)
                        {
                            elementType = curPropertyInfo.PropertyType.GetGenericArguments().Single();
                        }
                        else
                        {
                            elementType = curPropertyInfo.PropertyType.GetElementType();
                        }

                        

                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(String)); 
                        if (elementType!=null)
                        {
                            converter = TypeDescriptor.GetConverter(elementType);
                        }


                        if (newCollection is IList)
                        {
                            foreach (string curString in (List<string>)curValue)
                            {
                                if (String.IsNullOrEmpty(curString))
                                {
                                    continue;
                                }
                                if (elementType != null)
                                {
                                    ((IList)newCollection).Add(converter.ConvertFrom(curString));
                                }
                                else
                                {
                                    ((IList)newCollection).Add(curString);    
                                }
                            }
                        }

                        curPropertyInfo.SetValue(newReport, newCollection);

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

        /// <summary>
        /// Searches a specified string in report's fields
        /// </summary>
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
