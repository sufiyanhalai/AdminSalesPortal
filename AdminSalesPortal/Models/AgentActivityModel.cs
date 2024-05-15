using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSalesPortal.Models
{
    public class AgentActivityModel
    {

        public string AgentName { get; set; }
        public int AppointmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string Internet { get; set; }
        public string CreditCheck { get; set; }
        public string IsVerified { get; set; }
        public string Status { get; set; }
    }
}