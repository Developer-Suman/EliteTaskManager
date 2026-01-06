using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Task
{
    public record DeleteNickNameDTOs
    (
        string Id,
        string NickName,
        bool IsActive
        );
}
