using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour MemorizationView.xaml
    /// </summary>
    public partial class MemorizationView : UserControl, IStartable
    {
        private MemoryTestActivity _parent;
        List<ImageNombre> ListeComplete;


        public MemorizationView(MemoryTestActivity parent)
        {
            InitializeComponent();
            _parent = parent;

            _parent.listeMemorisation = new List<ImageNombre>();
            ListeComplete = new List<ImageNombre>();

            
        }



        // A chaque tick, rafraichir le compteur
        public void TimerTick(object sender, EventArgs e)
        {
            int timeLeft = int.Parse(TBCountDown.Text);
            TBCountDown.Text = (timeLeft - 1).ToString();
            
            if (timeLeft <= 0)
            {
                _parent.timer.Stop();
                _parent.Show("answer");
            }
        }




        // Méthode création des listes d'images avec nombres 

        public void CreateList()
        {
            //Ajout des images du dossier dans une liste
            DirectoryInfo path = new DirectoryInfo("../../Img/");
            string chemin;
            Random rnd = new Random();
            int nombre;

            
            ListeComplete.Clear();

            // Récupération de toutes les images
            foreach (FileInfo fichier in path.GetFiles())
            {
                
                nombre = rnd.Next(1, 999);

                chemin = System.IO.Path.GetFullPath("../../Img/"+fichier.Name);

                ListeComplete.Add(new ImageNombre(chemin, (nombre).ToString()));
            }

        }

        public void TrieAffichage()
        {
            Random rnd = new Random();
            int nombre;
            _parent.listeMemorisation.Clear();

            List<ImageNombre> x = ListeComplete;

            for (int z = 0; z < _parent.difficulte; z++)
            {
                nombre = rnd.Next(0, x.Count);
                _parent.listeMemorisation.Add(ListeComplete[nombre]);
                x.RemoveAt(nombre);            
            }

            Affichage_image.Children.Clear();


            // Affichage des images
            foreach (ImageNombre imgnb in _parent.listeMemorisation)
            {
                Image image = new Image();
                image.Height = 20 ;
                image.Width = 50;

                Label lbl = new Label();
                lbl.Width = 40;
                lbl.Content = imgnb._nombre;

                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(imgnb._image);
                myBitmapImage.DecodePixelWidth = 50;
                myBitmapImage.EndInit();
                image.Source = myBitmapImage;

                Affichage_image.Children.Add(image);
                Affichage_image.Children.Add(lbl);
            }
        }

        // Start du timer
        public void Start()
        {
            CreateList();
            TrieAffichage();
            TBCountDown.Text = (_parent.tempsMemorisation).ToString();
            _parent.timer.Start();
        }
    }
}