using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class FocusTestActivity : Activity
    {
        public FocusTestActivity(MainWindow context) : base(context)
        {
            AddView("home", new FocusTestView(this));
            AddView("challenge", new StreamChallengeView(this));

            SetView("home");
        }
    }
}
