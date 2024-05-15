using AdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSalesPortal.Models
{
    public class SearchModel
    {
        public string AgentName { get; set; }

        public List<Appointments> Appointments { get; set; }
    }
    public class Appointments
    {
        public int AppointmentId { get; set; }

        public List<Fields> Fields { get; set; }
    }


    public class Fields
    {
        public string FieldLabel { get; set; }

        public string FieldAnswer { get; set; }
    }
    public class SearchRequestModel
    {
        public string TeamName { get; set; }
        public string AgentName { get; set; }
        public string CreditCheck { get; set; }
        public string Internet { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}