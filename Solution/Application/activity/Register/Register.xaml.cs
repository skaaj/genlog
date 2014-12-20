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
using System.Xml.Linq;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        private Activity _parent;
        private int _score;

        public Register(Activity parent, int score)
        {
            InitializeComponent();
            _parent = parent;
            _score = score;
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            XElement root = XElement.Load("../../Data/SampleData.xml");
            FocusActivity fa = _parent as FocusActivity;
            MemoryTestActivity ma = _parent as MemoryTestActivity;
            if (fa != null)
            {
                XElement user = UserExists(root);
                if (user != null)
                {
                    user.Element("FocusTests").Add(new XElement("Result",
                        new XAttribute("time", fa.Speed),
                        new XAttribute("level", fa.Level),
                        new XAttribute("score", _score)
                        ));
                }
                else
                {
                    XElement fTests = new XElement("FocusTests");
                    fTests.Add(new XElement("Result",
                        new XAttribute("time", fa.Speed),
                        new XAttribute("level", fa.Level),
                        new XAttribute("score", _score)
                        ));

                    root.Add(new XElement("User",
                        new XAttribute("firstname", textBoxFirstname.Text),
                        new XAttribute("lastname", textBoxLastname.Text),
                        fTests,
                        new XElement("MemTests")
                        ));
                }
            }

            if (ma != null)
            {
                XElement user = UserExists(root);
                if (user != null)
                {
                    user.Element("MemTests").Add(new XElement("Result",
                        new XAttribute("time", ma.tempsMemorisation),
                        new XAttribute("level", ma.difficulte),
                        new XAttribute("score", ma.difficulte) // FIXME: mettre le score
                        ));
                }
                else
                {
                    XElement mTests = new XElement("MemTests");
                    mTests.Add(new XElement("Result",
                        new XAttribute("time", ma.tempsMemorisation),
                        new XAttribute("level", ma.difficulte),
                        new XAttribute("score", ma.difficulte) // FIXME: mettre le score
                        ));

                    root.Add(new XElement("User",
                        new XAttribute("firstname", textBoxFirstname.Text),
                        new XAttribute("lastname", textBoxLastname.Text),
                        mTests,
                        new XElement("FocusTests")
                        ));
                }
            }

            root.Save("../../Data/SampleData.xml");

            _parent.GoToHome();
        }

        private XElement UserExists(XElement root)
        {
            IEnumerable<XElement> users = root.Elements("User");
            foreach (var user in users)
            {
                if(user.Attribute("firstname").Value == textBoxFirstname.Text
                    && user.Attribute("lastname").Value == textBoxLastname.Text)
                    return user;
            }

            return null;
        }
    }
}
