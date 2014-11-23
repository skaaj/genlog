using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private FocusActivity _parent;

        private DispatcherTimer _timer;
        private DoubleAnimation _animation;

        private Random _randomizer;

        private int _interval;
        private int _level;

        private int _shapeSize;
        private double _yCenter;
        private double _xCenter;

        private double equilateralFactor = 2.0 / Math.Sqrt(3.0);

        private List<Brush> _colors;

        private enum ShapeEnum { Square, Triangle, Circle, Unknown }; // fixme : order
        private enum ColorEnum { Red, Green, Blue };

        private ShapeEnum _correctShape;
        private int _correctColor; // index dans _colors
        private Shape _template;
        private bool _hasDots;

        private int _good;
        private int _bad;

        public StreamChallengeView(FocusActivity parent)
        {
            InitializeComponent();

            _parent = parent;

            _randomizer = new Random();

            _colors = new List<Brush>();
            _colors.Add(Brushes.DarkRed);
            _colors.Add(Brushes.DarkGreen);
            _colors.Add(Brushes.DarkBlue);
        }

        public void Start()
        {
            _interval = _parent.Speed * 1000;
            _level = _parent.Level;

            InitializeAnimation();

            GenerateRule();

            _timer.Start();
            Console.WriteLine("Timer started :)");
        }

        public void Stop()
        {
            _timer.Stop();
            Console.WriteLine("Timer stopped sir !");
        }

        #region rule

        private void GenerateRule()
        {
            bool[] negations = { false, false, false };
            int randomIndex;
            int nbPlaced = 0;

            _correctShape = (ShapeEnum)_randomizer.Next(0, 3);
            _correctColor = _randomizer.Next(0, 3);

            // ~1,3 microseconds (w/ i5-4430 @ 3.0 Ghz)
            while (nbPlaced < (_level - 1))
            {
                randomIndex = _randomizer.Next(0, 3);

                if (!negations[randomIndex])
                {
                    negations[randomIndex] = true;
                    nbPlaced++;
                }
            }

            StringBuilder strBuilder = new StringBuilder();
            if (negations[0])
                strBuilder.Append("Ne cliquez pas sur les");
            else
                strBuilder.Append("Cliquez sur les");

            switch (_correctShape)
            {
                case ShapeEnum.Square:
                    strBuilder.Append(" carrés, ");
                    _template = ShapeFactory.Rectangle(1, 1);
                    break;
                case ShapeEnum.Triangle:
                    strBuilder.Append(" triangles, ");
                    _template = ShapeFactory.EquilateralTriangle(1);
                    break;
                case ShapeEnum.Circle:
                    strBuilder.Append(" cercles, ");
                    _template = ShapeFactory.Ellipse(1, 1);
                    break;
                default:
                    strBuilder.Append(" formes, ");
                    break;
            }

            if (negations[1])
                strBuilder.Append("qui ne sont pas");
            else
                strBuilder.Append("de couleur");

            switch (_correctColor)
            {
                case 0:
                    strBuilder.Append(" rouge et ");
                    break;
                case 1:
                    strBuilder.Append(" vert et ");
                    break;
                case 2:
                    strBuilder.Append(" bleu et ");
                    break;
                default:
                    break;
            }

            _template.Fill = _colors[_correctColor];

            if (negations[0])
            {
                _hasDots = false;
                strBuilder.Append("qui n'ont pas de points noirs.");
            }
            else
            {
                _hasDots = true;
                strBuilder.Append("qui ont des points noirs.");
            }

            ruleLabel.Content = strBuilder.ToString();
        }

        #endregion

        #region shapes

        private void InitializeAnimation()
        {
            _animation = new DoubleAnimation();
            _animation.From = -200.0;
            _animation.Duration = new Duration(TimeSpan.FromMilliseconds(_interval));
            //_animation.EasingFunction = new SineEase();

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = TimeSpan.FromMilliseconds(_interval / 2);
        }

        private void SpawnShape()
        {
            Shape shape;
            bool validity = _randomizer.NextDouble() < 0.5 ? true : false;

            double shapeY = _yCenter - (_shapeSize / 2);

            int chosenColor = _randomizer.Next(0, _colors.Count);
            ShapeEnum chosenShape = (ShapeEnum)_randomizer.Next(0, 3);
            bool hasDots = _randomizer.NextDouble() < 0.5 ? true : false;

            if (validity)
            {
                hasDots = _hasDots;
                chosenColor = _correctColor;
                chosenShape = _correctShape;
            }
            else
            {
                while (chosenShape == _correctShape && chosenColor == _correctColor && _hasDots == hasDots)
                {
                    chosenShape = (ShapeEnum)_randomizer.Next(0, 3);
                    chosenColor = _randomizer.Next(0, _colors.Count);
                    hasDots     = _randomizer.NextDouble() < 0.5 ? true : false;
                }
            }

            // On créer la forme
            switch (chosenShape)
            {
                case ShapeEnum.Square:
                    shape = ShapeFactory.Rectangle(_shapeSize, _shapeSize);
                    break;
                case ShapeEnum.Triangle:
                    shape = ShapeFactory.EquilateralTriangle((int)(_shapeSize * equilateralFactor));
                    break;
                case ShapeEnum.Circle:
                    shape = ShapeFactory.Ellipse(_shapeSize, _shapeSize);
                    break;
                default:
                    shape = ShapeFactory.Rectangle(_shapeSize, _shapeSize);
                    break;
            }

            // On lui donne sa couleur
            shape.Fill = _colors[chosenColor];

            if (validity)
            {
                shape.MouseDown += OnCorrectAnswer;
                shape.Unloaded += (s, e) =>
                {
                    _bad++; // la forme est supprimée sans avoir été cliquée, on ajoute une erreur
                };
            }
            else
            {
                shape.MouseDown += OnWrongAnswer;
            }

            if (hasDots)
            {
                Canvas complexShape = ShapeFactory.AddDots(shape, 3, 20, 10);
                canvas.Children.Add(complexShape);
                Canvas.SetTop(complexShape, shapeY);
                complexShape.BeginAnimation(Canvas.LeftProperty, _animation);
            }
            else
            {
                canvas.Children.Add(shape);
                Canvas.SetTop(shape, shapeY);
                shape.BeginAnimation(Canvas.LeftProperty, _animation);
            }

            if (canvas.Children.Count > 4)
                canvas.Children.RemoveRange(2, 1);
            Console.WriteLine(canvas.Children.Count);
        }

        void shape_Unloaded(object sender, RoutedEventArgs e)
        {
            _bad++;
            Console.WriteLine("T'es con !");
        }

        void OnCorrectAnswer(object sender, MouseButtonEventArgs e)
        {
            Shape s = sender as Shape;
            s.MouseDown -= OnCorrectAnswer;

            _good++;

            Console.WriteLine("Correct answer");
        }

        void OnWrongAnswer(object sender, MouseButtonEventArgs e)
        {
            Shape s = sender as Shape;
            s.MouseDown -= OnWrongAnswer;

            _bad++;

            Console.WriteLine("Bad answer");
        }

        /*
        private void SpawnShapeOld()
        {
            Shape shape = null;

            // TIRAGES
            int randShape = _randomizer.Next(0, 3);
            int randColor = _randomizer.Next(0, _colors.Count);
            double randValidity = _randomizer.NextDouble();
            double randDots = _randomizer.NextDouble();
            double validityProbability = 0.10; // 

            Console.WriteLine("(" + randShape + ", " + randValidity + ")");

            // INITS
            double shapeY = _yCenter - (_shapeSize / 2);

            // SUCCESS or NOT
            if (randValidity < validityProbability)
            {
                randShape = (int)_correctShape;
                randColor = (int)_correctColor;
                randDots = _hasDots ? 0.0 : 1.0;
            }

            // CHOIX FORME
            switch (randShape)
            {
                case 0:
                    shape = ShapeFactory.Rectangle(_shapeSize, _shapeSize);
                    break;
                case 1:
                    shape = ShapeFactory.EquilateralTriangle((int)(_shapeSize * equilateralFactor));
                    break;
                case 2:
                    shape = ShapeFactory.Ellipse(_shapeSize, _shapeSize);
                    break;
                default:
                    shape = ShapeFactory.Rectangle(_shapeSize, _shapeSize);
                    break;
            }

            // CHOIX COULEUR
            shape.Fill = _colors[randColor];

            // HAS DOTS
            Canvas complexShape = null;
            if (randDots < 0.5)
            {
                complexShape = ShapeFactory.AddDots(shape, 3, 20, 10);
            }

            // FIXME
            if (canvas.Children.Count > 20)
                canvas.Children.RemoveRange(2, canvas.Children.Count - 10);

            // SUCCESS or NOT
            if (randValidity < validityProbability)
            {
                shape.MouseDown += OnRectangleClickOK;
            }
            else
            {
                shape.MouseDown += OnRectangleClick;
            }

            if (complexShape == null)
            {
                canvas.Children.Add(shape);
                Canvas.SetTop(shape, shapeY);
                shape.BeginAnimation(Canvas.LeftProperty, _animation);
            }
            else
            {
                canvas.Children.Add(complexShape);
                Canvas.SetTop(complexShape, shapeY);
                complexShape.BeginAnimation(Canvas.LeftProperty, _animation);
            }
        }

        void OnRectangleClick(object sender, MouseButtonEventArgs e)
        {
            Shape s = sender as Shape;
            if (s != null)
            {
                s.StrokeThickness = 10;
                s.Stroke = new SolidColorBrush(Colors.Red);
            }
        }

        void OnRectangleClickOK(object sender, MouseButtonEventArgs e)
        {
            Shape r = sender as Shape;
            if (r != null)
            {
                r.StrokeThickness = 10;
                r.Stroke = new SolidColorBrush(Colors.Green);
            }
        }
        */

        private void TimerTick(object sender, EventArgs e)
        {
            SpawnShape();
        }

        #endregion

        private void CanvasLoaded(object sender, RoutedEventArgs e)
        {
            _animation.To = canvas.ActualWidth;

            _shapeSize = (int)(canvas.ActualHeight / 3);
            _yCenter = canvas.ActualHeight / 2;
            _xCenter = canvas.ActualWidth / 2;

            Line middle = new Line();
            middle.Stroke = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            middle.X1 = canvas.ActualWidth / 2;
            middle.X2 = middle.X1;
            middle.Y1 = 0;
            middle.Y2 = canvas.ActualHeight;
            middle.StrokeThickness = 2;

            canvas.Children.Add(middle);

            SpawnShape();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // refactoring inc
        }
    }
}
