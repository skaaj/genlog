using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class HomeActivity : Activity
    {
        public HomeActivity(MainWindow context) : base(context)
        {
            Reload();

            Show("home");
        }

        public override void Reload()
        {
            base.Reload();

            AddView("home", new HomeView());
        }
    }
}
