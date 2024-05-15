using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Models
{
    public class AgentActivitiesResponseModel
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
}