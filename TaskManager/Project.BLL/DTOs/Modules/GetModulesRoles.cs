using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Modules
{
    public record GetModulesRoles
    (
        string roleId,
        List<string> moduleId
        );
}
