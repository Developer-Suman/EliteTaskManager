using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Menu
{
    public record MenuCreatesDTOs(
        string submoduleId,
        string name,
        string icon,
        string targetUrl,
        string role,
        int? rank,
        bool isActive
        );
    
}
