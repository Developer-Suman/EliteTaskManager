using Microsoft.AspNetCore.Identity;
using Project.DLL.Premetives;
using Project.DLL.Static.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{

    public class RoleModule : Entity
    {
        public RoleModule(
            ) : base(null)
        {

        }

        public RoleModule(
            string id,
            string roleId,
            string moduleId
            ) : base(id)
        {
            RoleId = roleId;
            ModuleId = moduleId;

        }
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        public string ModuleId { get; set; }
        public Modules Modules { get; set; }
    }
}
