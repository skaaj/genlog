﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;

namespace Genlog
{
    public class MemoryTestActivity : Activity///Détermine l'Activity utilisée pour les Views concernant le test de mémoire
    {
        public int tempsMemorisation;       // Temps donné pour la mémorisation des figure et des nombres
        public int difficulte;              // Difficulté de la mémorisation
        public List<ImageNombre> listeMemorisation;
        public List<ImageNombre> ListeReponse;

        // Views
        public MemoryTestView homeView;
        public MemorizationView challengeView;
        public ResultatMemorizationView resultView;
        public AnswerMemoryView answerView;

        public MemoryTestActivity(MainWindow context)
            : base(context)
        {
            Reload();

            Show("home");
        }

        public override void Reload() ///Rechargement des différentes Views
        {
            base.Reload();

            homeView = new MemoryTestView(this);
            challengeView = new MemorizationView(this);
            resultView = new ResultatMemorizationView(this);
            answerView = new AnswerMemoryView(this);

            AddView("home", homeView);
            AddView("result", resultView);
            AddView("challenge", challengeView);
            AddView("answer", answerView);

            Show("home");
        }
    }
}
