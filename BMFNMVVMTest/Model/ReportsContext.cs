using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BMFNMVVMTest.Parser;

namespace BMFNMVVMTest.Model
{
    public sealed class ReportsContext
    {
        private static volatile ReportsContext instance;
        private static object syncRoot = new Object();

        private static List<Type> listTypes;
        private static List<Object> testData;
        private static ReportsParser reportsParser;

        public static List<Type> ListTypes
        {
            get { return listTypes; }
        }

        public static List<Object> TestData
        {
            get { return testData; }
        }

        public static ReportsParser ReportsParser
        {
            get { return reportsParser; }
        }

        /// <summary>
        /// All reports types must be listed here
        /// </summary>
        private ReportsContext()
        {
            listTypes = new List<Type>() {typeof(Report1), typeof(Report2)};

            reportsParser = new ReportsParser();

            dataInitialization();

        }

        public static ReportsContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ReportsContext();
                    }
                }

                return instance;
            }
        }

        private void dataInitialization()
        {
            testData = new List<Object>();

            Object curReport;
            SubReport curSubReport;
            List<int> curList;
            ArrayList curArrayList;

            for (int i = 1; i < 11; i++)
            {
                curReport = new Report1();
                ((Report1)curReport).Field1 = i*22.33;
                curSubReport = new SubReport();
                curSubReport.Field7 = i * 12.0001;
                curSubReport.Field8 = i * 1200;
                curSubReport.Field9 = i * 121;
                curSubReport.Field10 = String.Format("Subrep{0}", i);
                curArrayList = new ArrayList();
                curArrayList.Add(i * 10);
                curArrayList.Add(i * 20.20);
                curArrayList.Add("str30");
                curArrayList.Add(i * 40);
                curSubReport.Field11 = curArrayList;
                ((Report1)curReport).Field2 = curSubReport;
                ((Report1)curReport).Field3 = String.Format("Rep1-obj{0}", i);
                ((Report1)curReport).Field4 = i * 8;
                ((Report1)curReport).Field5 = i * 99999.001;
                curList = new List<int>();
                curList.Add(11);
                curList.Add(22);
                curList.Add(33);
                ((Report1)curReport).Field6 = curList;

                testData.Add(curReport);

                curReport = new Report2();
                ((Report2)curReport).Field1 = i * 42.663;
                ((Report2)curReport).Field2 = new Point(i * 3, i * 5);
                ((Report2)curReport).Field3 = String.Format("Rep2-obj{0}", i);
                ((Report2)curReport).Field4 = i * 7;
                ((Report2)curReport).Field5 = i * 3423.423;
                ((Report2)curReport).Field6 = new string[] { String.Format("str1{0}", i), String.Format("ste2{0}", i), String.Format("str3{0}", i) };

                testData.Add(curReport);
            }
        }
    }
}
