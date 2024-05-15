using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSalesPortal.Models
{
    public class DropdownModel
    {
        public List<TeamDropdownItem> Teams { get; set; }
        public List<AgentDropdownItem> Agents { get; set; }
        public List<RoleDropdownItem> Roles { get; set; }
        public List<ManagerDropdownItem> Managers { get; set; }
    }

    public class TeamDropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AgentDropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleDropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ManagerDropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }
}