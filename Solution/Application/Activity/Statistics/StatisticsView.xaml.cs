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
        XElement _users;

        public StatisticsView(StatisticsActivity parent)
        {
            InitializeComponent();

            _parent = parent;


            _users = _parent.GetData();

            /*
            userList = (XElement)((XmlDataProvider)Resources["dataKey"]).Data;

            IEnumerable<XElement> users = userList.Elements();
            // Read the entire XML
            foreach (var employee in users)
            {
                Console.WriteLine(employee);
            }*/
        }

        private void go(object sender, RoutedEventArgs e)
        {
            _users.Add(new XElement("User",
                new XAttribute("firstname", "ben"),
                new XAttribute("lastname", "jo")));
            _parent.SaveData();
            BindingOperations.GetBindingExpressionBase(list, ListBox.ItemsSourceProperty).UpdateTarget();
        }
    }
}
