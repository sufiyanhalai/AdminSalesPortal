using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminSalesPortal.Models
{
    public class UserModel
    {
        public int iUserMasterID { get; set; }
        
        public int? ManagerId { get; set; }
        [Required(ErrorMessage = "Team is required.")]
        public int TeamMasterId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
       
        public string sFirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string sLastName { get; set; }
       
        public string sPreferredName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
       
        public string sEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string sPassword { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        [DataType(DataType.PhoneNumber)]
        public string sPhone { get; set; }
        [Required(ErrorMessage = "Primary Address is required.")]
        public string sAddressPrimary { get; set; }
        [Required(ErrorMessage = "Secondary Address is required.")]
        public string sAddressSecondary { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public int iRoleMasterID { get; set; }
        public bool bIsActive { get; set; }
        public DateTime dtCreatedDate { get; set; }
        public DateTime dtLastUpdatedDate { get; set; }
        public int SelectedRoleId { get; set; }
        public int SelectedTeamId { get; set; }
        public int SelectedAgentId { get; set; }
    }
}