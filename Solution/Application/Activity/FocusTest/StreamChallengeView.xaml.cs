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
    /// Logique d'interaction pour StreamChallengeView.xaml
    /// </summary>
    public partial class StreamChallengeView : UserControl, IStartable, IStoppable
    {
        private Activity _parent;

        private DispatcherTimer timer;
        private Storyboard _storyboard;
        private DoubleAnimation _animation;

        private Random _randomizer;

        private int _count;
        private int _interval;

        public StreamChallengeView(Activity parent)
        {
            InitializeComponent();

            _count = 0;
            _interval = 2;
            _randomizer = new Random();

            _parent = parent;
        }

        public void Start()
        {
            InitializeAnimation();

            timer.Start();
            Console.WriteLine("Timer started :)");
        }

        public void Stop()
        {
            timer.Stop();
            Console.WriteLine("Timer stopped sir !");
        }

        #region shapes

        private void InitializeAnimation()
        {
            _animation = new DoubleAnimation();
            _animation.From = 0.0;
            _animation.To = canvas.ActualWidth + 20;
            _animation.Duration = new Duration(TimeSpan.FromSeconds(_interval));
            _animation.EasingFunction = new ElasticEase();

            _storyboard = new Storyboard();
            _storyboard.Children.Add(_animation);

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, _interval);
        }

        private void SpawnShape()
        {
            Shape s;

            int randomInt = _randomizer.Next() % 3;

            Console.WriteLine(randomInt);

            switch (randomInt)
            {
                case 0:
                    s = BuildLine("line" + _count++, 0, 0, 50, 50, 20, Brushes.IndianRed);
                    break;
                default:
                    s = BuildRectangle("rect" + _count++, 50, 50, 0, 0, Brushes.IndianRed);
                    break;
            }

            this.RegisterName(s.Name, s);

            if (randomInt == 0)
                s.MouseDown += OnRectangleClickOK;
            else
                s.MouseDown += OnRectangleClick;

            Storyboard.SetTargetName(_animation, s.Name);
            Storyboard.SetTargetProperty(_animation, new PropertyPath(Canvas.LeftProperty));

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
            Line myLine = new Line();
            myLine.Name = name;
            myLine.Stroke = color;
            myLine.X1 = x1;
            myLine.X2 = x2;
            myLine.Y1 = y1;
            myLine.Y2 = y2;
            myLine.StrokeThickness = thickness;
            myLine.RenderTransformOrigin = new Point(0.0, 0.0);

            myLine.RenderTransform = new RotateTransform(45);

            return myLine;
        }

        void OnRectangleClick(object sender, MouseButtonEventArgs e)
        {
            Shape s = sender as Shape;
            if (s != null)
            {
                s.Stroke = new SolidColorBrush(Colors.Red);
                s.Fill = new SolidColorBrush(Colors.Red);
            }
        }

        void OnRectangleClickOK(object sender, MouseButtonEventArgs e)
        {
            Shape r = sender as Shape;
            if (r != null)
            {
                r.Fill = new SolidColorBrush(Colors.Green);
                r.Stroke = new SolidColorBrush(Colors.Green);
            }
        }

        private void ShapeLoaded(object sender, RoutedEventArgs e)
        {
            _storyboard.Begin(this);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            SpawnShape();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UserControl uc = sender as UserControl;
            _animation.To = uc.ActualWidth + 20;
        }

        #endregion

        private void CanvasLoaded(object sender, RoutedEventArgs e)
        {
            _animation.To = canvas.ActualWidth;
        }

        private void CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _animation.To = canvas.ActualWidth + 20;
        }

    }
}
