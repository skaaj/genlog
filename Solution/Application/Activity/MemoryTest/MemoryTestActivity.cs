using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class MemoryTestActivity : Activity
    {
        public MemoryTestActivity(MainWindow context) : base(context)
        {
            AddView("home", new MemoryTestView());

            SetView("home");
        }
    }
}
