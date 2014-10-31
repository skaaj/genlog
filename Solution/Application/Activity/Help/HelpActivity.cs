using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class HelpActivity : Activity
    {
        public HelpActivity(MainWindow context) : base(context)
        {
            AddView("home", new HelpView());

            SetView("home");
        }
    }
}
