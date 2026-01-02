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
            DateTime createdAt
            ) : base(id)
        {
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            TaskDetails = new List<TaskDetails>();
        }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public ICollection<TaskDetails> TaskDetails { get; set; }
    }
   
}
