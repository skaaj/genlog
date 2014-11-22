using System;
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
    /// Logique d'interaction pour AnswerMemoryView.xaml
    /// </summary>
    public partial class AnswerMemoryView : UserControl, IStartable
    {
        private MemoryTestActivity _parent;

        public int[] equivalent;
        List<TextBox> listedebox;


        public AnswerMemoryView(MemoryTestActivity parent)
        {
            InitializeComponent();
            _parent = parent;          
        }


        // Onsubmit
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            VerificationReponse();
            _parent.Show("result");
        }



        public void AffichageRandom()
        {


            // rendu random de l'affichage // Création des textbox
            List<int> listecompte = new List<int> { };
            listedebox = new List<TextBox> { };


            for (int cpt = 1; listecompte.Count < _parent.listeMemorisation.Count; cpt++)
                        {
                            listecompte.Add(cpt);
                            listedebox.Add(new TextBox());
                            listedebox.ElementAt(cpt-1).Width = 40;
                            listedebox.ElementAt(cpt-1).Height = 20;
                        }


            equivalent = new int[listecompte.Count];
            int nombre; 
            Random rnd = new Random();
            
            // Affichage des images

            for (int cpt = 0; listecompte.Count > 0; cpt++)
            {
                nombre = rnd.Next(1, listecompte.Count);
                equivalent[cpt] = listecompte.ElementAt(nombre-1);
                listecompte.RemoveAt(nombre-1);

            }

            Affichage_image_answer.Children.Clear();

            for (int cpt = 0; cpt < _parent.listeMemorisation.Count; cpt++)
            {

                Image image = new Image();
                image.Height = 20;
                image.Width = 50;

                Label lbl = new Label();
                lbl.Width = 40;
                lbl.Content = _parent.listeMemorisation.ElementAt(equivalent[cpt]-1)._nombre;

                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(_parent.listeMemorisation.ElementAt(equivalent[cpt]-1)._image);
                myBitmapImage.DecodePixelWidth = 50;
                myBitmapImage.EndInit();
                image.Source = myBitmapImage;

                Affichage_image_answer.Children.Add(image);
                Affichage_image_answer.Children.Add(listedebox.ElementAt(equivalent[cpt] - 1));

            }
        }



        // Verification des réponses apportées par l'utilisateur
        public void VerificationReponse()
        {
            _parent.ListeReponse = new List<ImageNombre> { };
            _parent.ListeReponse.Clear();


            for (int z = 0; z < _parent.listeMemorisation.Count; z++)
            {
                _parent.ListeReponse.Add(new ImageNombre(_parent.listeMemorisation.ElementAt(z)._image, listedebox.ElementAt(z).Text, true));

                if (_parent.ListeReponse.ElementAt(z)._nombre != _parent.listeMemorisation.ElementAt(z)._nombre)
                {
                    _parent.ListeReponse.ElementAt(z)._result = false;
                }
            }
        }

        public void Start()
        {
            AffichageRandom();
        }

    }
}
