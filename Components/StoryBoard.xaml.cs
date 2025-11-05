namespace WOZStoryBuilder.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
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

    using WOZStoryBuilder.Models;

    /// <summary>
    /// Interaction logic for StoryBoard.xaml
    /// </summary>
    public partial class StoryBoard : UserControl
    {

        //private bool isDragging = false;
        //private Point mouseOffset;
        //private StoryBoard_SceneBox? draggedBox = null;

        private readonly List<BoxConnection> _connections = new List<BoxConnection>();


        public List<Scene> Scenes
        {
            get { return (List<Scene>)GetValue(ScenesProperty); }
            set { SetValue(ScenesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScenesProperty =
            DependencyProperty.Register("Scenes", typeof(List<Scene>), typeof(StoryBoard), new PropertyMetadata(null, OnScenesChanged));

        public Scene CurrentSelectedScene
        {
            get { return (Scene)GetValue(CurrentSelectedSceneProperty); }
            set { SetValue(CurrentSelectedSceneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentSelectedScene.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSelectedSceneProperty =
            DependencyProperty.Register("CurrentSelectedScene", typeof(Scene), typeof(StoryBoard), new PropertyMetadata(null));

        public StoryBoard()
        {
            InitializeComponent();
        }


        private static void OnScenesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            List<Scene> Scenes = (List<Scene>)e.NewValue;

            // Get the current instance of the StoryBoard usercontrol
            StoryBoard storyBoardInstance = (StoryBoard)d; 

            if (Scenes != null)
            {
                // We can start to draw:
                storyBoardInstance.Draw(Scenes);
            }
        }

        internal void Draw(List<Scene> Scenes)
        {
            this.canvas.Children.Clear();




            for (int i = 0; i < Scenes.Count(); i++)
            {
                Scene curScene = Scenes[i];

                StoryBoard_SceneBox sceneBox = new StoryBoard_SceneBox { Title = curScene.Name, SceneObj = curScene};
                Canvas.SetLeft(sceneBox, i * 110);
                Canvas.SetTop(sceneBox, 20 + new Random().Next(50, 500));
                Canvas.SetZIndex(sceneBox, 1);
                DraggableBehavior.SetIsEnabled(sceneBox, true);

                // Listen for position changes
                DraggableBehavior.AddPositionChangedHandler(sceneBox, (s, e) => UpdateConnections());

                this.canvas.Children.Add(sceneBox);

            }


            // Draw lines between boxes (VERY NESTED!!!) do this first to place them below

            List<StoryBoard_SceneBox> sceneBoxes = new List<StoryBoard_SceneBox>();
            foreach (UIElement UiEl in this.canvas.Children)
            {
                if (UiEl is StoryBoard_SceneBox)
                {
                    sceneBoxes.Add((StoryBoard_SceneBox)UiEl);
                }
            }



            foreach (var boxA in sceneBoxes)
            {
                foreach (var choice in boxA.SceneObj.Choices)
                {
                    var boxB = sceneBoxes.FirstOrDefault(b => b.SceneObj.ID == choice.SceneObj.ID);
                    if (boxB != null)
                    {
                        var line = new Line { Stroke = Brushes.Black, StrokeThickness = 2, StrokeEndLineCap = PenLineCap.Triangle };
                        var arrow = new Polygon { Fill = Brushes.Black };

                        canvas.Children.Add(line);
                        canvas.Children.Add(arrow);

                        _connections.Add(new BoxConnection { From = boxA, To = boxB, Line = line, Arrow = arrow });

                        // Initial line position
                        UpdateLine(boxA, boxB, line, arrow);
                    }
                }
            }

        }

        private void UpdateConnections()
        {
            foreach (var conn in _connections)
            {
                UpdateLine(conn.From, conn.To, conn.Line, conn.Arrow);
            }
        }


        private void UpdateLine(StoryBoard_SceneBox from, StoryBoard_SceneBox to, Line line, Polygon arrow)
        {
            // Safe get positions
            double x1 = Canvas.GetLeft(from) + from.Width / 2;
            double y1 = Canvas.GetTop(from) + from.Height / 2;
            double x2 = Canvas.GetLeft(to) + to.Width / 2;
            double y2 = Canvas.GetTop(to) + to.Height / 2;

            // Direction vector
            double dx = x2 - x1;
            double dy = y2 - y1;
            double length = Math.Sqrt(dx * dx + dy * dy);

            if (length < 0.001)
                return; // Avoid division by zero

            // Normalize direction
            dx /= length;
            dy /= length;

            // Compute edge offsets
            double halfW = to.Width / 2;
            double halfH = to.Height / 2;

            // Calculate scale to reach edge of target box
            double scale = 1.0 / Math.Max(
                Math.Abs(dx) / halfW,
                Math.Abs(dy) / halfH
            );

            // Edge of target box
            double endX = x2 - dx * scale;
            double endY = y2 - dy * scale;

            // Edge of source box
            halfW = from.Width / 2;
            halfH = from.Height / 2;
            scale = 1.0 / Math.Max(
                Math.Abs(dx) / halfW,
                Math.Abs(dy) / halfH
            );

            double startX = x1 + dx * scale;
            double startY = y1 + dy * scale;

            // Update line
            line.X1 = startX;
            line.Y1 = startY;
            line.X2 = endX;
            line.Y2 = endY;

            // Draw arrow
            double arrowLength = 12;
            double arrowWidth = 6;

            double angle = Math.Atan2(dy, dx) * 180 / Math.PI;

            arrow.Points.Clear();
            arrow.Points.Add(new Point(0, 0));
            arrow.Points.Add(new Point(-arrowLength, arrowWidth));
            arrow.Points.Add(new Point(-arrowLength, -arrowWidth));

            Canvas.SetLeft(arrow, endX);
            Canvas.SetTop(arrow, endY);
            arrow.RenderTransform = new RotateTransform(angle, 0, 0);
        }

        



    }
}
