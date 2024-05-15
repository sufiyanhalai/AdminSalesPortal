using System.Collections.Generic;

namespace AdminPortal.Models
{
    public class ViewDataModel
    {
        public int AppointmentId { get; set; }
        public List<ViewDataFields> Fields { get; set; }
    }

    public class ViewDataFields
    {
        public string FieldLabel { get; set; }
        public string FieldAnswer { get; set; }
    }
}
