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
    /// Logique d'interaction pour StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        StatisticsActivity _parent;

        XDocument _usersDoc;
        XElement _userRoot;

        //XElement _users;

        public StatisticsView(StatisticsActivity parent)
        {
            InitializeComponent();

            _parent = parent;

            _usersDoc = XDocument.Load("../../Data/SampleData.xml");
            _userRoot = _usersDoc.Element("Users");

            listBox.DataContext = _usersDoc.Element("Users").Elements();
        }

        private void go(object sender, RoutedEventArgs e)
        {
            _userRoot.Add(new XElement("User",
                new XAttribute("firstname", "test"),
                new XAttribute("lastname", "eur")));

            _usersDoc.Save("..//..//Data//SampleData.xml");
            listBox.DataContext = _usersDoc.Element("Users").Elements();
        }

        private void refresh(object sender, MouseButtonEventArgs e)
        {
            listBox.DataContext = _usersDoc.Element("Users").Elements();
        }

        private void TargetTest(object sender, DataTransferEventArgs e)
        {

        }
    }
}
