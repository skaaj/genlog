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
        private List<ImageNombre> _images;
        private Random _randomizer;
        private DispatcherTimer _timer;

        public MemorizationView(MemoryTestActivity parent)
        {
            InitializeComponent();
            _parent = parent;

            _parent.listeMemorisation = new List<ImageNombre>();
            _images = new List<ImageNombre>();
            _randomizer = new Random();

            // Timer pour le test de mémorisation 
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = new TimeSpan(0, 0, 1);
        }

        // A chaque tick, rafraichir le compteur
        public void TimerTick(object sender, EventArgs e)
        {
            int timeLeft = int.Parse(TBCountDown.Text);
            TBCountDown.Text = (timeLeft - 1).ToString();
            
            if (timeLeft <= 0)
            {
                _timer.Stop();
                _parent.Show("answer");
            }
        }

        // Méthode création des listes d'images avec nombres 
        public void LoadImagesFromDirectory(string directoryPath)
        {
            //Ajout des images du dossier dans une liste
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            string filePath;
            int randomInt;
            
            _images.Clear();

            // Récupération de toutes les images
            foreach (FileInfo fichier in directory.GetFiles())
            {
                randomInt = _randomizer.Next(1, 999);
                filePath = System.IO.Path.GetFullPath("../../Img/" + fichier.Name);

                if (filePath[filePath.Length - 1] == 'b') continue; // bad patch fixme please!

                _images.Add(new ImageNombre(filePath, (randomInt).ToString()));
            }
        }

        public void TrieAffichage()
        {
            int nombre;
            _parent.listeMemorisation.Clear();

            List<ImageNombre> x = _images;

            for (int z = 0; z < _parent.difficulte; z++)
            {
                nombre = _randomizer.Next(0, x.Count);
                _parent.listeMemorisation.Add(_images[nombre]);
                x.RemoveAt(nombre);            
            }

            Affichage_image.Children.Clear();

            // Affichage des images
            foreach (ImageNombre imgnb in _parent.listeMemorisation)
            {
                Image image = new Image();
                image.Height = 97 ;
                image.Width = 108;
                image.Margin = new Thickness(20);


                Label lbl = new Label();
                lbl.Margin = new Thickness(20);
                lbl.Width = 80;
                lbl.FontSize = 16;
                lbl.Content = imgnb._nombre;

                image.Source = new BitmapImage(new Uri(imgnb._image));

                Affichage_image.Children.Add(image);
                Affichage_image.Children.Add(lbl);
            }
        }

        // Start du timer
        public void Start()
        {
            LoadImagesFromDirectory("../../Img");
            TrieAffichage();
            TBCountDown.Text = (_parent.tempsMemorisation).ToString();
            _timer.Start();
        }
    }
}