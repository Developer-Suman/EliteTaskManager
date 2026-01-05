using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Task
{
    public record ProjectDetailsGetByIdDTOs
    (
       string id,
            string name,
            string description,
            DateTime createdAt
        );
}
