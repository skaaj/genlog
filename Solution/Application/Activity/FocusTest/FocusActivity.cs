using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class FocusActivity : Activity
    {
        public int Speed { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }

        public static Random Randomizer = new Random();

        public FocusActivity(MainWindow context) : base(context)
        {
            Reload();

            Show("home");
        }

        public override void Reload()
        {
            base.Reload();

            AddView("home", new FocusView(this));
            AddView("stream", new StreamChallengeView(this, false));
            AddView("example", new StreamChallengeView(this, true));
            AddView("register", new ResultView(this));

            Show("home");
        }
    }
}
