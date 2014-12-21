using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class StatisticsActivity : Activity///Détermine l'Activity utilisée pour la View Statistics
    {
        MainWindow _context;

        public StatisticsActivity(MainWindow context)
            : base(context)
        {
            Reload();

            Show("home");
        }

        public override void Reload()
        {
            base.Reload();

            AddView("home", new StatisticsView(this));
        }
    }
}
