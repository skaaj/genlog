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
using System.Xml.Linq;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour ResulatatMemorizationView.xaml
    /// </summary>
    public partial class ResultatMemorizationView : UserControl, IStartable ///Permet la gestion de la vue concernant l'affichage des résultats de l'utilisateur au test de mémoire
    {
        private MemoryTestActivity _parent;

        private const string databasePath = "../../Data/database.xml";
        private int _score;

        public ResultatMemorizationView(MemoryTestActivity parent)
        {
            InitializeComponent();

            _parent = parent;
            _score = 0;
        }
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            XElement root = XElement.Load(databasePath);

            XElement user = UserExists(root);
            if (user != null)
            {
                user.Element("MemTests").Add(new XElement("Result",
                    new XAttribute("time", _parent.tempsMemorisation),
                    new XAttribute("level", _parent.difficulte),
                    new XAttribute("score", _score)
                    ));
            }
            else
            {
                XElement mTests = new XElement("MemTests");
                mTests.Add(new XElement("Result",
                    new XAttribute("time", _parent.tempsMemorisation),
                    new XAttribute("level", _parent.difficulte),
                    new XAttribute("score", _score)
                    ));

                root.Add(new XElement("User",
                    new XAttribute("firstname", textBoxFirstname.Text),
                    new XAttribute("lastname", textBoxLastname.Text),
                    mTests,
                    new XElement("FocusTests")
                    ));
            }

            root.Save(databasePath);
            _parent.GoToHome();
        }


        private XElement UserExists(XElement root)///Récupération des informations renseignées sur l'utilisateur
        {
            IEnumerable<XElement> users = root.Elements("User");
            foreach (var user in users)
            {
                if (user.Attribute("firstname").Value == textBoxFirstname.Text
                    && user.Attribute("lastname").Value == textBoxLastname.Text)
                    return user;
            }

            return null;
        }

        public void AffichageResultat()///Affiche les images avec les réponses données, indique si la réponse est juste ou fausse
        {
            Affichage_resultat.Children.Clear();

            // Affichage des images
            foreach (ImageNombre imgnb in _parent.ListeReponse)
            {

                Image image = new Image();
                image.Height = 60;
                image.Width = 150;

                Label lbl = new Label();
                lbl.Width = 100;
                lbl.FontSize = 16;
                lbl.Content = imgnb._nombre;

                image.Source = new BitmapImage(new Uri(imgnb._image));

                Affichage_resultat.Children.Add(image);


                BrushConverter bc = new BrushConverter();       // Changement de couleur selon la véracité de la réponse
                if (imgnb._result != true)
                {
                    lbl.Background = (Brush)bc.ConvertFrom("#FFFF9090");
                }
                else 
                {
                    _score++;
                    lbl.Background = (Brush)bc.ConvertFrom("#8ad628");
                }


                Affichage_resultat.Children.Add(lbl); 
            }

            Nombre_reponses_justes.Content = (_score).ToString() + " / " + (_parent.ListeReponse.Count).ToString();

        }

        public void Start()///Start de la view
        {
            AffichageResultat();
        }

    }
}
