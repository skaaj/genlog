﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class StatisticsActivity : Activity
    {
        public StatisticsActivity(MainWindow context)
            : base(context)
        {
            AddView("home", new StatisticsView());

            SetView("home");
        }
    }
}