using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class UserControllerAction : Entity
    {
        public UserControllerAction(): base(null) { }

        public UserControllerAction(
            string id,
            string userId,
            string controlllerActionId

            ): base(id)
        {
            UserId = userId;
            ControllerActionId = controlllerActionId;  
        }

        public string UserId { get; set; }
        public ApplicationUsers User { get; set; }
        public string ControllerActionId { get; set; }
        public ControllerAction ControllerActions { get; set; }
    }
}
