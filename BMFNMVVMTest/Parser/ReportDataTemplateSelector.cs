using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using BMFNMVVMTest.Model;


namespace BMFNMVVMTest.Parser
{
    class ReportDataTemplateSelector : DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                return ReportsContext.ReportsParser.GetDataTemplate(item.GetType());
            }

            //DataTemplate dtImpTask = new DataTemplate();
            //dtImpTask.DataType = typeof(Task);
            //FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            //border.SetValue(Border.BorderBrushProperty, Brushes.Brown);
            //border.SetValue(Border.BorderThicknessProperty, new Thickness(5));

            //FrameworkElementFactory gridElement = new FrameworkElementFactory(typeof(Grid));
            //gridElement.AppendChild(new FrameworkElementFactory(typeof(RowDefinition)));
            //gridElement.AppendChild(new FrameworkElementFactory(typeof(RowDefinition)));
            //gridElement.AppendChild(new FrameworkElementFactory(typeof(RowDefinition)));
            //gridElement.AppendChild(new FrameworkElementFactory(typeof(ColumnDefinition)));
            //gridElement.AppendChild(new FrameworkElementFactory(typeof(ColumnDefinition)));

            //FrameworkElementFactory curTxtxBlock;
            //curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //curTxtxBlock.SetValue(Grid.RowProperty, 0);
            //curTxtxBlock.SetValue(Grid.ColumnProperty, 0);
            //curTxtxBlock.SetValue(TextBlock.TextProperty, "Task Name:");
            //gridElement.AppendChild(curTxtxBlock);

            //curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //curTxtxBlock.SetValue(Grid.RowProperty, 0);
            //curTxtxBlock.SetValue(Grid.ColumnProperty, 1);
            //curTxtxBlock.SetBinding(TextBlock.TextProperty, new Binding("TaskName"));
            //gridElement.AppendChild(curTxtxBlock);

            //curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //curTxtxBlock.SetValue(Grid.RowProperty, 1);
            //curTxtxBlock.SetValue(Grid.ColumnProperty, 0);
            //curTxtxBlock.SetValue(TextBlock.TextProperty, "Description:");
            //gridElement.AppendChild(curTxtxBlock);

            //curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //curTxtxBlock.SetValue(Grid.RowProperty, 1);
            //curTxtxBlock.SetValue(Grid.ColumnProperty, 1);
            //curTxtxBlock.SetBinding(TextBlock.TextProperty, new Binding("Description"));
            //gridElement.AppendChild(curTxtxBlock);

            //curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //curTxtxBlock.SetValue(Grid.RowProperty, 2);
            //curTxtxBlock.SetValue(Grid.ColumnProperty, 0);
            //curTxtxBlock.SetValue(TextBlock.TextProperty, "Priority:");
            //gridElement.AppendChild(curTxtxBlock);

            //curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //curTxtxBlock.SetValue(Grid.RowProperty, 2);
            //curTxtxBlock.SetValue(Grid.ColumnProperty, 1);
            //curTxtxBlock.SetBinding(TextBlock.TextProperty, new Binding("Priority"));
            //gridElement.AppendChild(curTxtxBlock);


            //border.AppendChild(gridElement);

            ////FrameworkElementFactory txtBlockDescription = new FrameworkElementFactory(typeof(TextBlock));
            ////txtBlockDescription.SetBinding(TextBlock.TextProperty, new Binding("Description"));
            ////border.AppendChild(txtBlockDescription);

            //dtImpTask.VisualTree = border;

            //if (element != null && item != null && item is Report1)
            //{
            //    Report1 curItem = item as Report1;
            //    DataTemplate templateReport1 = new DataTemplate();
            //    templateReport1.DataType = typeof(Report1);

            //    FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            //    border.SetValue(Border.BorderBrushProperty, Brushes.Brown);
            //    border.SetValue(Border.BorderThicknessProperty, new Thickness(5));

            //    FrameworkElementFactory curTxtxBlock;
            //    //curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //    //curTxtxBlock.SetValue(TextBlock.TextProperty, "Field1:");
            //    //border.AppendChild(curTxtxBlock);

            //    curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //    curTxtxBlock.SetBinding(TextBlock.TextProperty, new Binding("Field1"));
            //    border.AppendChild(curTxtxBlock);

            //    templateReport1.VisualTree = border;

            //    return;

            //    if (taskitem.Priority == 1)
            //        return dtImpTask;
            //    //element.FindResource("importantTaskTemplate") as DataTemplate;
            //    else
            //        return
            //            element.FindResource("myTaskTemplate") as DataTemplate;
            //}

            //if (element != null && item != null && item is Report2)
            //{
            //    Report2 curItem = item as Report2;
            //    DataTemplate templateReport1 = new DataTemplate();
            //    templateReport1.DataType = typeof(Report1);

            //    FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            //    border.SetValue(Border.BorderBrushProperty, Brushes.Aqua);
            //    border.SetValue(Border.BorderThicknessProperty, new Thickness(5));

            //    FrameworkElementFactory curTxtxBlock;
            //    //curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //    //curTxtxBlock.SetValue(TextBlock.TextProperty, "Field1:");
            //    //border.AppendChild(curTxtxBlock);

            //    curTxtxBlock = new FrameworkElementFactory(typeof(TextBlock));
            //    curTxtxBlock.SetBinding(TextBlock.TextProperty, new Binding("Field1"));
            //    border.AppendChild(curTxtxBlock);

            //    templateReport1.VisualTree = border;

            //    return templateReport1;

            //    //if (taskitem.Priority == 1)
            //    //    return dtImpTask;
            //    //    //element.FindResource("importantTaskTemplate") as DataTemplate;
            //    //else
            //    //    return
            //    //        element.FindResource("myTaskTemplate") as DataTemplate;
            //}


            return null;
        }
    }

}



