using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminSalesPortal.Models
{
    public class FieldsViewModel
    {
        public FieldsViewModel()
        {
            fieldsModel = new List<FieldModel>();
        }

        public List<FieldModel> fieldsModel { get; set; }
    }
    public class FieldModel
    {
        public string Fieldlabel { get; set; }
        public int iFieldAnswerID { get; set; }
        public int iAppointmentMasterID { get; set; }
        public int iFormFieldID { get; set; }
        public string sFieldAnswer { get; set; }
        public int FieldInputTypeId { get; set; }
        public string FieldInputTypeName { get; set; }
        public List<ReturnFieldInputValue> DefaultValues { get; set; }
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