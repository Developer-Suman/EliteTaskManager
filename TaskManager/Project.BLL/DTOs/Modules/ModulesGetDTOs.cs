using Project.BLL.DTOs.Menu;
using Project.BLL.DTOs.SubModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Modules
{
    public record ModulesGetDTOs(
      string Id,
      string Name,
      string? Role,
      string? TargetUrl,
      bool isActive
      //List<SubModulesGetDTOs> SubModulesGetDTOs
      //List<MenuGetDTOs> MenuGetDTOs
        );
   
}
