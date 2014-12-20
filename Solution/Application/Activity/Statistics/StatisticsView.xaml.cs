using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class StatisticsView : UserControl, IStartable
    {
        StatisticsActivity _parent;

        XDocument _usersDoc;
        XElement _userRoot;

        public StatisticsView(StatisticsActivity parent)
        {
            InitializeComponent();

            _parent = parent;

            _usersDoc = XDocument.Load("../../Data/SampleData.xml");
            _userRoot = _usersDoc.Element("Users");

            grid.DataContext = _usersDoc.Element("Users").Elements();
        }

        private void OnSelectUser(object sender, SelectionChangedEventArgs e)
        {
            XElement user = e.AddedItems[0] as XElement;
            IEnumerable<XElement> memTests = user.Element("MemTests").Elements("Result");
            
            DataTable table = new DataTable();
            table.Columns.Add("Temps", System.Type.GetType("System.String"));
            table.Columns.Add("Niveau", System.Type.GetType("System.String"));
            table.Columns.Add("Score", System.Type.GetType("System.String")); 
            foreach (XElement test in memTests)
            {
                DataRow newRow = table.NewRow();
                newRow[0] = test.Attribute("time").Value;
                newRow[1] = test.Attribute("level").Value;
                newRow[2] = test.Attribute("score").Value;
                table.Rows.Add(newRow);
            }

            gridMemTests.ItemsSource = table.DefaultView;

            IEnumerable<XElement> focusTests = user.Element("FocusTests").Elements("Result");

            DataTable table2 = new DataTable();
            table2.Columns.Add("Temps", System.Type.GetType("System.String"));
            table2.Columns.Add("Niveau", System.Type.GetType("System.String"));
            table2.Columns.Add("Score", System.Type.GetType("System.String"));
            foreach (XElement test in focusTests)
            {
                DataRow newRow = table2.NewRow();
                newRow[0] = test.Attribute("time").Value;
                newRow[1] = test.Attribute("level").Value;
                newRow[2] = test.Attribute("score").Value;
                table2.Rows.Add(newRow);
            }

            gridFocusTests.ItemsSource = table2.DefaultView;
        }

        private void OnTestSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (paramPanel == null) return;
            
            paramPanel.Visibility = Visibility.Visible;
            ComboBoxItem selectedItem = e.AddedItems[0] as ComboBoxItem;
            string selectedTest = (string)selectedItem.Content;

            if (selectedTest.Equals("Concentration"))
            {
                labelParam1.Content = "Vitesse";
                labelParam2.Content = "Niveau";
                FillComboxBoxItems(comboxBoxParam1, 2, 8);
                FillComboxBoxItems(comboxBoxParam2, 1, 4);
            }
            else
            {
                labelParam1.Content = "Temps";
                labelParam2.Content = "Nombre de figures";
                FillComboxBoxItems(comboxBoxParam1, 1, 60);
                FillComboxBoxItems(comboxBoxParam2, 3, 10);
            }
            comboxBoxParam1.SelectedIndex = 0;
            comboxBoxParam2.SelectedIndex = 0;
        }

        private void FillComboxBoxItems(ComboBox comboBox, int min, int max)
        {
            comboBox.Items.Clear();
            for (int i = min; i <= max; i++)
                comboBox.Items.Add(i.ToString());
        }

        private void OnSubmitParams(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = comboBoxTest.SelectedItem as ComboBoxItem;
            string selectedTest = item.Content.ToString().Equals("Concentration") ? "FocusTests" : "MemTests";
            string param1 = comboxBoxParam1.SelectedValue.ToString();
            string param2 = comboxBoxParam2.SelectedValue.ToString();

            bool userFound = false;

            int max = int.MinValue;
            int min = int.MaxValue;
            string maxUser = "";
            string minUser = "";

            List<int> scores = new List<int>();

            IEnumerable<XElement> users = _usersDoc.Element("Users").Elements("User");
            foreach (XElement user in users)
            {
                IEnumerable<XElement> results = user.Element(selectedTest).Elements("Result");
                foreach (XElement result in results)
                {
                    string xmlParam1 = result.Attribute("time").Value; 
                    string xmlParam2 = result.Attribute("level").Value; 
                    if (xmlParam1 == param1 && xmlParam2 == param2)
                    {
                        userFound = true;
                        int score = int.Parse(result.Attribute("score").Value); 
                        scores.Add(score);

                        if(score > max){
                            max = score;
                            maxUser = user.Attribute("firstname").Value + " " + user.Attribute("lastname").Value;
                        }
                        if(score < min){
                            min = score;
                            minUser = user.Attribute("firstname").Value + " " + user.Attribute("lastname").Value;
                        }
                    }
                }
            }

            if (userFound)
            {
                labelScoreMax.Content = "Score maximum : " + scores.Max();
                labelMaxUser.Content = "Effectué par : " + maxUser;
                labelScoreMin.Content = "Score minimum : " + scores.Min();
                labelMinUser.Content = "Effectué par : " + minUser;
                labelAvg.Content = "Moyenne : " + scores.Average();
                labelVar.Content = "Variance : " + scores.Variance();

                panelNoResults.Visibility = Visibility.Collapsed;
                panelResults.Visibility = Visibility.Visible;
            }
            else
            {
                panelNoResults.Visibility = Visibility.Visible;
                panelResults.Visibility = Visibility.Collapsed;
            }
        }


        void IStartable.Start()
        {
            _usersDoc = XDocument.Load("../../Data/SampleData.xml");
            grid.DataContext = _usersDoc.Element("Users").Elements();
        }
    }
    
    public static class Extension
    {
        public static double Variance(this List<int> values)
        {
            return values.Variance(values.Average(), 0, values.Count);
        }

        public static double Variance(this IEnumerable<int> values, double mean, int start, int end)
        {
            double variance = 0;

            for (int i = start; i < end; i++)
            {
                variance += Math.Pow((values.ElementAt(i) - mean), 2);
            }

            int n = end - start;
            if (start > 0) n -= 1;

            return variance / (n);
        }
    }
}
