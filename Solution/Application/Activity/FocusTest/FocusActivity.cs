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

        public static Random Randomizer = new Random();

        public FocusActivity(MainWindow context) : base(context)
        {
            AddView("home", new FocusView(this));
            AddView("stream", new StreamChallengeView(this, false));
            AddView("example", new StreamChallengeView(this, true));

            Show("home");
        }

        public void GoToRegister(int score)
        {
            AddView("register", new Register(this, score));
            Show("register");
        }
    }
}
