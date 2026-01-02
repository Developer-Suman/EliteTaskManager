using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Menu : Entity
    {
        public Menu() : base(null) { }

        public Menu(
            string id,
            string name,
            string targetUrl,
            string iconUrl,
            string role,
            string subModuleId,
            int? rank,
            bool isActive
            ) : base(id)
        {
            Name = name;
            TargetUrl = targetUrl;
            IconUrl = iconUrl;
            Role = role;
            Rank = rank;
            SubModuleId = subModuleId;
            IsActive = isActive;

        }

        public string Name { get; set; }
        public string TargetUrl { get; set; }
        public string IconUrl { get; set; }
        public string Role { get; set; }
        public string SubModuleId { get; set; }
        public bool IsActive { get; set; }

        public int? Rank { get; set; }  // Determines the order of the menu

        //NavigationProperty
        public SubModules SubModules { get; set; }
    }
}
