using Project.BLL.DTOs.SubModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Modules
{
    public record ModulesGetDTOsWithUsers
    (
      string Id,
      string Name,
      string? Role,
      string? TargetUrl,
      bool isActive,
      List<SubModulesGetDTOsWithUsers> SubModulesGetDTOsWithUsers
      //List<MenuGetDTOs> MenuGetDTOs
        );
}
