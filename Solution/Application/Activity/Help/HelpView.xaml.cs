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

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour HelpView.xaml
    /// </summary>
    public partial class HelpView : UserControl
    {
        private DispatcherTimer timer;
        private Storyboard myStoryboard;
        private int nb = 0;
        private int velocity = 2;
        private DoubleAnimation myDoubleAnimation;

        public HelpView()
        {
            InitializeComponent();

            myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = canvas.Width*2;
            myDoubleAnimation.EasingFunction = new ElasticEase();
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);
            
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, velocity);
            timer.Start();
        }

        private void SpawnShape()
        {
            Shape s;
            s = BuildLine("myRectangle" + nb++, 0, 0, 40, 60, 10, Brushes.DarkCyan);
            //s = BuildRectangle("myRectangle" + nb++, 40, 40, 0, 0, new SolidColorBrush(Colors.Beige));
            this.RegisterName(s.Name, s);

            s.MouseDown += OnRectangleClick;

            Storyboard.SetTargetName(myDoubleAnimation, s.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Canvas.LeftProperty));

            // Use the Loaded event to start the Storyboard.
            s.Loaded += new RoutedEventHandler(ShapeLoaded);
            canvas.Children.Add(s);
        }

        private Rectangle BuildRectangle(string name, int width, int height, int x, int y, Brush color)
        {
            Rectangle tmp = new Rectangle();
            tmp.Name = name;
            tmp.Width = width;
            tmp.Height = height;
            Canvas.SetTop(tmp, x);
            Canvas.SetTop(tmp, y);
            tmp.Fill = color;

            return tmp;
        }

        private Line BuildLine(string name, int x1, int y1, int x2, int y2, int thickness, Brush color)
        {
            // Add a Line Element
            Line myLine = new Line();
            myLine.Name = name;
            myLine.Stroke = color;
            myLine.X1 = x1;
            myLine.X2 = x2;
            myLine.Y1 = y1;
            myLine.Y2 = y2;
            myLine.StrokeThickness = thickness;

            myLine.RenderTransform = new RotateTransform(45);

            return myLine;
        }

        void OnRectangleClick(object sender, MouseButtonEventArgs e)
        {
            Shape s = sender as Shape;
            if (s != null)
            {
                s.Stroke = new SolidColorBrush(Colors.Red);
            }
        }

        void OnRectangleClickOK(object sender, MouseButtonEventArgs e)
        {
            Shape r = sender as Shape;
            if (r != null)
            {
                r.Fill = new SolidColorBrush(Colors.Green);
            }
        }

        private void ShapeLoaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            SpawnShape();
        }
    }
}
