using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSalesPortal.Models
{
    public class AppointmentLAbel
    {
        public string Fieldlabel { get; set; }
        public int iFieldAnswerID { get; set; }
        public int iAppointmentMasterID { get; set; }
        public int? iFormFieldID { get; set; }
        public string sFieldAnswer { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public int? iCreatedBy { get; set; }
        public DateTime? dtLastUpdatedDate { get; set; }
        public int? iLastUpdatedBy { get; set; }
        public int FieldInputTypeId { get; set; }
    }
}