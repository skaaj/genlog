using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class FocusActivity : Activity
    {

        private FocusView homeView;
        private StreamChallengeView streamView;

        public FocusActivity(MainWindow context) : base(context)
        {
            AddView("home", new FocusView(this));
            AddView("stream", new StreamChallengeView(this));

            Show("home");
        }
    }
}
