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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour HelpView.xaml
    /// </summary>
    public partial class HelpView : UserControl ///Permet de gérer la View Help
    {
        public HelpView()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            XElement xelement = XElement.Load(@"../../data/DataSample.xml");
            IEnumerable<XElement> employees = xelement.Elements();

            xelement.Add(new XElement("User",
                 new XAttribute("firstname", "Benjamin"),
                 new XAttribute("lastname", "Denom")));

            xelement.Save(@"../../data/DataSample.xml");

            foreach (var employee in employees)
            {
                Console.WriteLine(employee.Attribute("firstname").Value);
            }
        }
    }
}
