using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminSalesPortal.Models
{
    public class TeamModel
    {
        public int iTeamMasterID { get; set; }
        [Required(ErrorMessage = "Team Name is required")]
        public string sTeamName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string sDescription { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtUpdatedDAte { get; set; }
    }
}