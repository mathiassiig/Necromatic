using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Necromatic.Char.NPC.TaskHandlers
{
    public interface ITaskHandler
    {
        void Think();
        void TaskUpdate();
    }
}
