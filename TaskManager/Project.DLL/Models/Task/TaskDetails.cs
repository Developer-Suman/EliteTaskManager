using Project.DLL.Models.Task.SetUp;
using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DLL.Static.Enum.TaskEnum;

namespace Project.DLL.Models.Task
{
    public class TaskDetails : Entity
    {
        public TaskDetails(): base(null)
        {
        }
        public TaskDetails(
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
            string? finalLink,
            string description,
            bool isDeleted,
            DateTime createdAt,
            DateTime modifiedAt
            ) : base(id)
        {
            NickNameId = nickNameId;
            Title = title;
            TaskStatus = taskStatus;
            ProjectDetailsId = projectDetailsId;
            TaskPriority = taskPriority;
            TaskReviewed = taskReviewed;
            Descriptions = descriptions;
            StartDate = startDate;
            DueDate = dueDate;
            FinalLink = finalLink;
            DoingLink = doingLink;
            IsDeleted = isDeleted;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;

        }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string? NickNameId { get; set; }

        public NickName? NickName { get; set; }
        public string? Title { get; set; }
        public Status? TaskStatus { get; set; }
        public string? ProjectDetailsId { get; set; }
        public ProjectDetails? ProjectDetails { get; set; }
        public Priority? TaskPriority { get; set; }
        public Reviewed? TaskReviewed { get; set; }
        public string? Descriptions { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? DoingLink { get; set; }
        public string? FinalLink { get; set; }

    }
}
