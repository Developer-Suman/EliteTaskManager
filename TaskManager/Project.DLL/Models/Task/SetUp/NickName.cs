using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models.Task.SetUp
{
    public class NickName : Entity
    {
        public NickName(): base(null)
        {
            
        }

        public NickName(
            string id,
            string name,
            DateTime createdAt
            ): base(id)
        {
            Name = name;
            CreatedAt = createdAt;
            TaskDetails = new List<TaskDetails>();

        }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<TaskDetails> TaskDetails { get; set; }

    }
}
