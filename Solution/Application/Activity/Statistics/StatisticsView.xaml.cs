using System;
using System.Collections.Generic;
using System.Data;
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

            grid.DataContext = _usersDoc.Element("Users").Elements();
        }

        private void go(object sender, RoutedEventArgs e)
        {
            _userRoot.Add(new XElement("User",
                new XAttribute("firstname", "test"),
                new XAttribute("lastname", "eur")));

            _usersDoc.Save("..//..//Data//SampleData.xml");
            grid.DataContext = _usersDoc.Element("Users").Elements();
        }

        private void refresh(object sender, MouseButtonEventArgs e)
        {
            grid.DataContext = _usersDoc.Element("Users").Elements();
        }

        private void TargetTest(object sender, DataTransferEventArgs e)
        {

        }

        private void OnSelectUser(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(e.ToString());
            XElement user = e.AddedItems[0] as XElement;
            IEnumerable<XElement> memTests = user.Element("MemTests").Elements("Result");
            
            DataTable table = new DataTable();
            table.Columns.Add("Temps", System.Type.GetType("System.String"));
            table.Columns.Add("Niveau", System.Type.GetType("System.String"));
            table.Columns.Add("Score", System.Type.GetType("System.String")); 
            foreach (XElement xEle in memTests)
            {
                DataRow newRow = table.NewRow();
                newRow[0] = xEle.Attribute("time").Value;
                newRow[1] = xEle.Attribute("level").Value;
                newRow[2] = xEle.Attribute("score").Value;
                table.Rows.Add(newRow);
            }

            gridMemTests.ItemsSource = table.DefaultView;

            IEnumerable<XElement> focusTests = user.Element("FocusTests").Elements("Result");

            DataTable table2 = new DataTable();
            table2.Columns.Add("Temps", System.Type.GetType("System.String"));
            table2.Columns.Add("Niveau", System.Type.GetType("System.String"));
            table2.Columns.Add("Score", System.Type.GetType("System.String"));
            foreach (XElement xEle in focusTests)
            {
                DataRow newRow = table2.NewRow();
                newRow[0] = xEle.Attribute("time").Value;
                newRow[1] = xEle.Attribute("level").Value;
                newRow[2] = xEle.Attribute("score").Value;
                table2.Rows.Add(newRow);
            }

            gridFocusTests.ItemsSource = table2.DefaultView;

        }
    }
}
