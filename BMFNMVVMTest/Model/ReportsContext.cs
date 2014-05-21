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


            curReport = new Report1();
            ((Report1)curReport).Field1 = 22.33;
            curSubReport = new SubReport();
            curSubReport.Field7 = 12.0001;
            curSubReport.Field8 = 1200;
            curSubReport.Field9 = 121;
            curSubReport.Field10 = "Subrep1";
            curArrayList = new ArrayList();
            curArrayList.Add(10);
            curArrayList.Add(20.20);
            curArrayList.Add("str30");
            curArrayList.Add(40);
            curSubReport.Field11 = curArrayList;
            ((Report1)curReport).Field2 = curSubReport;
            ((Report1)curReport).Field3 = "Rep1-obj1";
            ((Report1)curReport).Field4 = 8;
            ((Report1)curReport).Field5 = 99999.001;
            curList = new List<int>();
            curList.Add(11);
            curList.Add(22);
            curList.Add(33);
            ((Report1)curReport).Field6 = curList;

            testData.Add(curReport);

            curReport = new Report1();
            ((Report1)curReport).Field1 = 42.663;
            curSubReport = new SubReport();
            curSubReport.Field7 = 24.0001;
            curSubReport.Field8 = 2400;
            curSubReport.Field9 = 241;
            curSubReport.Field10 = "Subrep2";
            curArrayList = new ArrayList();
            curArrayList.Add(1010);
            curArrayList.Add(2020.2020);
            curArrayList.Add("str3030");
            curArrayList.Add(4040);
            curSubReport.Field11 = curArrayList;
            ((Report1)curReport).Field2 = curSubReport;
            ((Report1)curReport).Field3 = "Rep1-obj2";
            ((Report1)curReport).Field4 = 7;
            ((Report1)curReport).Field5 = 3423423;
            curList = new List<int>();
            curList.Add(111);
            curList.Add(222);
            curList.Add(333);
            ((Report1)curReport).Field6 = curList;


            testData.Add(curReport);


            curReport = new Report2();
            ((Report2)curReport).Field1 = 42.663;
            ((Report2)curReport).Field2 = new Point(33, 55);
            ((Report2)curReport).Field3 = "Rep2-obj1";
            ((Report2)curReport).Field4 = 7;
            ((Report2)curReport).Field5 = 3423.423;
            ((Report2)curReport).Field6 = new string[] { "str1", "str2", "str3" };

            testData.Add(curReport);



        }
    }
}
