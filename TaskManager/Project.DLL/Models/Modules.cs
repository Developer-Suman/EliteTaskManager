using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{

    public class Modules : Entity
    {
        public Modules() : base(null) { }

        public Modules(
            string id,
            string name,
            string? role,
            string? targetUrl,
            bool isActive

            ) : base(id)
        {
            Name = name;
            Role = role;
            TargetUrl = targetUrl;
            IsActive = isActive;
            SubModules = new List<SubModules>();
            RoleModules = new List<RoleModule>();


        }

        public string Name { get; set; }
        public string? Role { get; set; }
        public string? TargetUrl { get; set; }
        public bool IsActive { get; set; }

        public ICollection<SubModules> SubModules { get; set; }
        public ICollection<RoleModule> RoleModules { get; set; }
    }
}
