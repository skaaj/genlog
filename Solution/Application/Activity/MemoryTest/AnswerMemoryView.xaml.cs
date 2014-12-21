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
    public partial class AnswerMemoryView : UserControl, IStartable ///Permet la gestion de la vue concernant la phase de réponse de l'utilisateur au test de mémoire
    {
        private MemoryTestActivity _parent;

        public int[] equivalent;
        List<TextBox> listedebox;
        int cpt = 0;


        public AnswerMemoryView(MemoryTestActivity parent)
        {
            InitializeComponent();
            _parent = parent;          
        }



        public void AffichageRandom()   ///Permet d'afficher une association que l'utilisateur doit compléter, de façon aléatoire
        {
            List<int> listecompte = new List<int> { };
            listedebox = new List<TextBox> { };


            for (int p = 1; listecompte.Count < _parent.listeMemorisation.Count; p++)
                        { listecompte.Add(p); }


            equivalent = new int[listecompte.Count];
            int nombre; 
            Random rnd = new Random();
            
            

            for (int c = 0; listecompte.Count > 0; c++) // Equivalence entre le nouveau random et l'affichage initial
            {
                nombre = rnd.Next(1, listecompte.Count);
                equivalent[c] = listecompte.ElementAt(nombre-1);
                listecompte.RemoveAt(nombre-1);

            }

            
                //Affichage des images
                Label lbl = new Label();
                lbl.Width = 40;
                lbl.Content = _parent.listeMemorisation.ElementAt(equivalent[cpt]-1)._nombre;
                imagebox.Source = new BitmapImage(new Uri(_parent.listeMemorisation.ElementAt(equivalent[cpt] - 1)._image));
        }



        
        public void VerificationReponse()/// Verification des réponses apportées par l'utilisateur
        {
            
            for (int z = 0; z < _parent.listeMemorisation.Count; z++)
            {

                if (_parent.ListeReponse.ElementAt(z)._nombre != _parent.listeMemorisation.ElementAt(equivalent[z] - 1)._nombre)
                {
                    _parent.ListeReponse.ElementAt(z)._result = false;
                }
            }
        }


        private void Suivant_Click(object sender, RoutedEventArgs e)///Vérifie qu'il reste des associations auquelles répondre sinon envoie à la page de résultat
        {
            try
            {
                _parent.ListeReponse.Add(new ImageNombre(_parent.listeMemorisation.ElementAt(cpt)._image, nombrebox.Text));
            }
            catch
            { }

            if (cpt < _parent.listeMemorisation.Count - 1)
            {
                nombrebox.Text = null;
                cpt = cpt + 1;
                imagebox.Source = new BitmapImage(new Uri(_parent.listeMemorisation.ElementAt(equivalent[cpt] - 1)._image));
            }
            else
            {
                imagebox.Source = null;
                nombrebox.Text = null;
                VerificationReponse();
                _parent.Show("result");
            }
        }

        public void Start()///Start de la view
        {
            _parent.ListeReponse = new List<ImageNombre> { };
            _parent.ListeReponse.Clear();
            cpt = 0;
            AffichageRandom();
        }

    }
}
