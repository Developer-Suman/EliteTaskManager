using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.ModulesFlags
{
    public record UpdateModulesFlag
   (
        string modulesId,
        bool isActive
        );
}
