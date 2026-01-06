using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models.Task.SetUp
{
    public class ProjectDetails : Entity
    {
        public ProjectDetails(): base(null)
        {
            
        }
        public ProjectDetails(
            string id,
            string name,
            string description,
            bool isDeleted,
            DateTime createdAt,
            DateTime modifiedAt
            ) : base(id)
        {
            Name = name;
            Description = description;
            IsDeleted = isDeleted;
            CreatedAt = createdAt;
            CreatedAt = createdAt;
            TaskDetails = new List<TaskDetails>();
        }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Description { get; set; }
        public ICollection<TaskDetails> TaskDetails { get; set; }
    }
   
}
