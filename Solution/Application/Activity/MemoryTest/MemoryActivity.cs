using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;

namespace Genlog
{
    public class MemoryTestActivity : Activity
    {
        public DispatcherTimer timer;
        public int tempsMemorisation;
        public int difficulte;

        public List<ImageNombre> listeMemorisation;

        // Views
        public MemoryTestView homeView;
        public MemorizationView challengeView;
        public ResultatMemorizationView resultView;
        public AnswerMemoryView answerView;

        public MemoryTestActivity(MainWindow context)
            : base(context)
        {
            homeView = new MemoryTestView(this);
            challengeView = new MemorizationView(this);
            resultView = new ResultatMemorizationView(this);
            answerView = new AnswerMemoryView(this);


            AddView("home", homeView);
            AddView("result", resultView);
            AddView("challenge", challengeView);
            AddView("answer", answerView);

            // Timer pour le test de mémorisation 
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(challengeView.TimerTick);
            timer.Interval = new TimeSpan(0, 0, 1);


            Show("home");
        }

    }
}
