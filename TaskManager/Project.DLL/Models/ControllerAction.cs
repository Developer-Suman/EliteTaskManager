using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class ControllerAction : Entity
    {
        public ControllerAction() : base(null) { }

        public ControllerAction(
            string id,
            string controlerName,
            string actionName

            ): base(id) 
        {
            ActionName = actionName;
            ControlerName = controlerName;
            userControllerActions = new List<UserControllerAction>();
        }
        public string ControlerName { get; set; }
        public string ActionName { get; set; }
        public ICollection<UserControllerAction> userControllerActions { get; set; }

    }
}
