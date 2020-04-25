using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NetControl
{
    [Serializable]
    public class GraphSeries : DependencyObject
    {
        public GraphSeries()
        {
            SetDefaults();
        }


        private void SetDefaults()
        {
            IsLineBroken = true;
            StrokeThickness = 1;
            PathColor = Colors.Gray;
            PathBrush = new SolidColorBrush(PathColor);
            FillColor = Colors.Transparent;
            Fill = new SolidColorBrush(FillColor);
            PathPen = new Pen(PathBrush, StrokeThickness);
        }


        public string DisplayName { get; set; }

        public string Units { get; set; }

        /// <summary>
        /// Is the Series visible in the graph
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(GraphSeries),
                new FrameworkPropertyMetadata((bool)true));

        /// <summary>
        /// Gets or sets the IsVisible property.  This dependency property 
        /// indicates ....
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }


        private SolidColorBrush _pathBrush;
        public SolidColorBrush PathBrush
        {
            get { return _pathBrush; }
            set
            {
                _pathBrush = (value as SolidColorBrush);
                _pathBrush.Freeze();

                _pathColor = _pathBrush.Color;
                PathPen = new Pen(PathBrush, StrokeThickness);
            }
        }


        private Pen _pathPen;
        public Pen PathPen
        {
            get { return _pathPen; }
            set
            {
                _pathPen = value;
                PathPen.Freeze();
            }
        }

        private double _strokeThickness;
        public double StrokeThickness
        {
            get { return _strokeThickness; }
            set
            {
                _strokeThickness = value;
                PathPen = new Pen(PathBrush, StrokeThickness);
            }
        }

        private Color _pathColor; // for serializing : serializing a brush is hard. 
        public Color PathColor
        {
            get { return _pathColor; }
            set
            {
                _pathColor = value;
                PathBrush = new SolidColorBrush(_pathColor);
            }
        }

        private Color _fillColor; // for serializing : serializing a brush is harder htan a color. 
        public Color FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                Fill = new SolidColorBrush(_fillColor);
            }
        }

        private SolidColorBrush _fillBrush;
        public SolidColorBrush Fill
        {
            get { return _fillBrush; }
            set
            {
                _fillBrush = value as SolidColorBrush;
                _fillBrush = _fillBrush ?? Brushes.Transparent;
                _fillBrush.Freeze();
                _fillColor = _fillBrush.Color;
            }
        }


        public bool IsFilled
        {
            get; set;

        }

        public bool IsPoints { get; set; }

        // These are all set at runtime and need not be persisted. 
        public double MinX { get; set; }
        public double LastY { get; set; }

        /// <summary>
        /// Last figure ended.i.e. Line stopped.
        /// Initialize to true since no figure begun yet.
        /// </summary>
        public bool IsLineBroken { get; set; }


        public GraphSeriesRenderData RenderData { get; set; }

    }
}
