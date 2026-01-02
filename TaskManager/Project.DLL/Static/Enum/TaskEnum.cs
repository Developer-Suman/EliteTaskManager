using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Static.Enum
{
    public static class TaskEnum
    {
        public enum Priority
        {
            Optional = 1,
            Urgent = 2,
            Test = 3
        }

        public enum Status
        {
            Review = 1,
            Confused = 2,
            Done = 3,
            Doing = 4,
            ToDo = 5
        }

        public enum Reviewed
        {
            Yes = 1,
            No = 2
        }


    }
}
