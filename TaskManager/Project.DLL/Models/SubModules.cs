using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{

    public class SubModules : Entity
    {
        public SubModules() : base(null) { }

        public SubModules(
            string id,
            string name,
            string? iconUrl,
            string? targetUrl,
            string? role,
            string moduleId,
            string rank,
            bool isActive
            ) : base(id)
        {
            Name = name;
            ModuleId = moduleId;
            Role = role;
            TargetUrl = targetUrl;
            Rank = rank;
            IsActive = isActive;
            Menu = new List<Menu>();

        }
        public string Name { get; set; }
        public string? iconUrl { get; set; }
        public string? TargetUrl { get; set; }
        public string? Role { get; set; }
        public string Rank { get; set; }
        public string ModuleId { get; set; }
        public bool IsActive { get; set; }

        //Navigation Property
        public ICollection<Menu> Menu { get; set; }

        public Modules Modules { get; set; }
    }
}
