using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DLL.Static.Enum.TaskEnum;

namespace Project.BLL.DTOs.Task
{
    public record AllTaskDetailsDTOs
    (
       string id,
            string? nickNameId,
            string? title,
            Status? taskStatus,
            string? projectDetailsId,
            Priority? taskPriority,
            Reviewed? taskReviewed,
            string? descriptions,
            DateTime? startDate,
            DateTime? dueDate,
            string? doingLink,
            string? finalLink
        );
}
