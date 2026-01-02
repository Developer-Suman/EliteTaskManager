using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.SubModules
{
    public record SubModulesGetDTOsWithUsers
    (
      string ModulesId,
      string SubModulesId,
      string name,
      string? IconUrl,
      string? TargetUrl,
      string? Role,
      string? Rank,
      bool isActive
        );
}
