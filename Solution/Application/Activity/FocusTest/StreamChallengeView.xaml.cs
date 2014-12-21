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

        private int _shapeSize;
        private double _yCenter;
        private double _xCenter;

        private double equilateralFactor = 2.0 / Math.Sqrt(3.0);

        private int _good;
        private int _bad;
        private int _gone;

        private FocusModel _model;
        private Instruction instruction;

        private bool _example;

        public StreamChallengeView(FocusActivity parent, bool example)
        {
            InitializeComponent();

            _parent = parent;

            _example = example;

            _randomizer = new Random();
            _model = new FocusModel();
        }

        public void Start()
        {
            _interval = _parent.Speed * 1000;

            instruction = new Instruction(_parent.Level, _model);
            ruleLabel.Content = instruction;

            InitializeAnimation();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        #region shapes

        private void InitializeAnimation()
        {
            _animation = new DoubleAnimation();
            _animation.Duration = new Duration(TimeSpan.FromMilliseconds(_interval));

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = TimeSpan.FromMilliseconds(_interval / 2);
            _timer.Stop();
        }

        private FocusModel.Shapes PickOtherShape(FocusModel.Shapes s)
        {
            FocusModel.Shapes[] availableShapes = new FocusModel.Shapes[FocusModel.ShapesCount - 1];

            int i = 0;
            Type enumType = typeof(FocusModel.Shapes);
            
            foreach (FocusModel.Shapes value in Enum.GetValues(enumType))
                if (value != s)
                    availableShapes[i++] = value;

            i = _randomizer.Next(availableShapes.Length);

            return availableShapes[i];
        }
        
        private Brush PickOtherColor(Brush b)
        {
            return _model.Colors.Where(brush => (brush != b)).ToArray()[_randomizer.Next(_model.Colors.Count - 1)];
        }

        private void SpawnShape()
        {
            CustomShape shape;
            ShapeProperty property = new ShapeProperty();

            bool isValid = _randomizer.NextDouble() < 0.5 ? true : false;

            if (isValid)
            {
                if (!instruction.Negations[0])
                    property.Shape = instruction.Property.Shape;
                else
                    property.Shape = PickOtherShape(instruction.Property.Shape);

                if (!instruction.Negations[1])
                    property.Color = instruction.Property.Color;
                else
                    property.Color = PickOtherColor(instruction.Property.Color);

                property.HasDots = instruction.Property.HasDots;

                shape = new CustomShape(property, _shapeSize);
                shape.MouseDown += OnCorrectAnswer;
                shape.Unloaded += OnShapeGoneUnclicked;
                shape.Unloaded += OnShapeUnloaded;
            }
            else
            {
                property.Shape = (FocusModel.Shapes) _randomizer.Next(FocusModel.ShapesCount);
                property.Color = _model.Colors[_randomizer.Next(_model.Colors.Count)];
                property.HasDots = _randomizer.NextDouble() < 0.5 ? true : false;


                if (property.Respect(instruction))
                {
                    property.HasDots = !property.HasDots;
                }

                shape = new CustomShape(property, _shapeSize);
                shape.MouseDown += OnWrongAnswer;
            }
            
            canvas.Children.Add(shape);
            Canvas.SetTop(shape, _yCenter - (_shapeSize / 2));

            _animation.From = -shape.RuntimeWidth;
            shape.BeginAnimation(Canvas.LeftProperty, _animation);

            if (canvas.Children.Count > 9)
                canvas.Children.RemoveRange(7, 1);
        }

        void OnShapeUnloaded(object sender, RoutedEventArgs e)
        {
            _gone++;

            if (_gone == 3 && _good != 3)
            {
                if (!_example)
                    _parent.Show("register");
                else
                    _parent.Show("stream");
            }
        }

        void OnShapeGoneUnclicked(object sender, RoutedEventArgs e)
        {
            _bad++; // la forme est supprimée sans avoir été cliquée, on ajoute une erreur
            labelBad.Content = "Mauvaises réponses : " + _bad.ToString();
        }

        void OnCorrectAnswer(object sender, MouseButtonEventArgs e)
        {
            CustomShape canvas = sender as CustomShape;

            if (_example)
            {
                canvas.InnerShape.Stroke = Brushes.Green;
                canvas.InnerShape.StrokeThickness = 10;
            }

            canvas.MouseDown -= OnCorrectAnswer;
            canvas.Unloaded -= OnShapeGoneUnclicked;

            _good++;

            labelGood.Content = "Bonnes réponses : " + _good.ToString();

            if(_good == 3)
            {
                if (!_example)
                    _parent.Show("register");
                else
                    _parent.Show("stream");
            }
        }


        void OnWrongAnswer(object sender, MouseButtonEventArgs e)
        {
            CustomShape canvas = sender as CustomShape;

            if (_example)
            {
                canvas.InnerShape.Stroke = Brushes.Red;
                canvas.InnerShape.StrokeThickness = 10;
            }
            canvas.MouseDown -= OnWrongAnswer;

            _bad++;

            labelBad.Content = "Mauvaises réponses : " + _bad.ToString();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            SpawnShape();
        }

        #endregion

        private void CanvasLoaded(object sender, RoutedEventArgs e)
        {
            //ruleLabel.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Roboto Light");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_example)
            {
                title.Content = "Passons aux choses sérieuses...";

                labelBad.Visibility = Visibility.Collapsed;
                labelGood.Visibility = Visibility.Collapsed;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _animation.To = canvas.ActualWidth;

            _shapeSize = (int)(canvas.ActualHeight / 3);
            _yCenter = canvas.ActualHeight / 2;
            _xCenter = canvas.ActualWidth / 2;

            // Titre
            Canvas.SetTop(title, canvas.ActualHeight / 8 - (title.ActualHeight / 2));
            Canvas.SetLeft(title, canvas.ActualWidth / 2 - (title.ActualWidth / 2));

            // Rectangle vert
            Canvas.SetTop(rectBackground, canvas.ActualHeight / 4);
            rectBackground.Width = canvas.ActualWidth;
            rectBackground.Height = (int)(canvas.ActualHeight / 2);

            // Consigne
            Canvas.SetTop(ruleLabel, canvas.ActualHeight / 2 - (ruleLabel.ActualHeight / 2));
            Canvas.SetLeft(ruleLabel, canvas.ActualWidth / 2 - (ruleLabel.ActualWidth / 2));

            // Bouton suivant
            Canvas.SetTop(buttonOK, canvas.ActualHeight / 2 - (buttonOK.ActualHeight / 2));
            Canvas.SetLeft(buttonOK, canvas.ActualWidth - (canvas.ActualWidth / 8));

            // Scores
            Canvas.SetTop(labelGood, canvas.ActualHeight - (canvas.ActualHeight / 8) - (labelGood.ActualHeight / 2));
            Canvas.SetLeft(labelGood, canvas.ActualWidth / 4);

            Canvas.SetTop(labelBad, canvas.ActualHeight - (canvas.ActualHeight / 8) - (labelGood.ActualHeight / 2));
            Canvas.SetLeft(labelBad, canvas.ActualWidth - canvas.ActualWidth / 4);
        }

        private void OnClickNext(object sender, RoutedEventArgs e)
        {
            double buttonDuration = canvas.ActualWidth - Canvas.GetLeft(buttonOK);
            double labelDuration = canvas.ActualWidth - Canvas.GetLeft(ruleLabel);

            DoubleAnimation leaveScreenAnim = new DoubleAnimation();
            leaveScreenAnim.Duration = new Duration(TimeSpan.FromMilliseconds(buttonDuration));
            leaveScreenAnim.To = canvas.ActualWidth;

            buttonOK.BeginAnimation(Canvas.LeftProperty, leaveScreenAnim);

            leaveScreenAnim.Duration = new Duration(TimeSpan.FromMilliseconds(labelDuration));
            leaveScreenAnim.Completed += (s, ev) =>
            {
                SpawnShape();
                if (_example)
                    _timer.Start();
                else
                    testc();
            };
            ruleLabel.BeginAnimation(Canvas.LeftProperty, leaveScreenAnim);
        }

        private void OnClickQuit(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            if (MessageBox.Show("Voulez-vous vraiment quitter ? Vous perdrez votre progression.", "Abandonner", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                _parent.GoToHome();
            else
                _timer.Start();
        }

        private void testc()
        {
            Console.WriteLine("wtf");
            _timer.Start();
        }
    }
}
