using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSalesPortal.Models
{
    public class FieldModel
    {
        public int iFormFieldID { get; set; }
        public string sFieldAnswer { get; set; }
        public string FieldLabel { get; set; }
        public int FieldInputTypeId { get; set; }
        public string FieldInputTypeName { get; set; }
        public List<ReturnFieldInputValue> DefaultValues { get; set; }
        public List<string> SelectedRadioValues { get; set; }
    }
    public class ReturnFieldInputValue
    {
        public int FormFieldId { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
        public int? ParentfieldInputValueId { get; set; }

        public int? ChildfieldInputValueId { get; set; }
    }
}