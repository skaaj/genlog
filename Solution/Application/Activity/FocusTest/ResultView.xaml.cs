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
    /// Logique d'interaction pour ResultView.xaml
    /// </summary>
    public partial class ResultView : UserControl
    {

        private const string databasePath = "../../Data/database.xml";

        private FocusActivity _parent;

        public ResultView(FocusActivity parent)
        {
            InitializeComponent();

            _parent = parent;
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            XElement root = XElement.Load(databasePath);

            XElement user = UserExists(root);
            if (user != null)
            {
                user.Element("FocusTests").Add(new XElement("Result",
                    new XAttribute("time", _parent.Speed),
                    new XAttribute("level", _parent.Level),
                    new XAttribute("score", _parent.Score)
                    ));
            }
            else
            {
                XElement fTests = new XElement("FocusTests");
                fTests.Add(new XElement("Result",
                    new XAttribute("time", _parent.Speed),
                    new XAttribute("level", _parent.Level),
                    new XAttribute("score", _parent.Score)
                    ));

                root.Add(new XElement("User",
                    new XAttribute("firstname", textBoxFirstname.Text),
                    new XAttribute("lastname", textBoxLastname.Text),
                    fTests,
                    new XElement("MemTests")
                    ));
            }

            root.Save(databasePath);
            _parent.GoToHome();
        }

        private XElement UserExists(XElement root)
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
    }
}
