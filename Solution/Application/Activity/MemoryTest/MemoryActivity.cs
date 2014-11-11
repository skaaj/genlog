﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Genlog
{
    public class MemoryTestActivity : Activity
    {
        public DispatcherTimer timer;

        // Views
        MemoryTestView homeView;
        MemorizationView challengeView;
        ResultatMemorizationView resultView;

        public MemoryTestActivity(MainWindow context) : base(context)
        {
            homeView = new MemoryTestView(this);
            challengeView = new MemorizationView(this);
            resultView = new ResultatMemorizationView(this);

            AddView("home", homeView);
            AddView("result", resultView);
            AddView("challenge", challengeView);

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(challengeView.TimerTick);
            timer.Interval = new TimeSpan(0, 0, 1);

            SetView("home");
        }
    }
}