using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMFNMVVMTest.Parser
{
    public class ReportField
    {
        private Type fieldType;

        private string fieldName;

        public Type FieldType
        {
            get { return fieldType; }
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public ReportField(Type fieldType, string fieldName)
        {
            this.fieldType = fieldType;
            this.fieldName = fieldName;
        }

    }
}
