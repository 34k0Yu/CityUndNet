using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NetControl
{
    public static class WindowsMediaExtensions
    {
        public static System.Drawing.SolidBrush AsGdiBrush(this System.Windows.Media.Color color)
        {
            var gdiBrush = new System.Drawing.SolidBrush(color.AsGdiColor());
            return gdiBrush;
        }

        public static System.Drawing.Color AsGdiColor(this System.Windows.Media.Color color)
        {
            System.Drawing.Color gdiColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            return gdiColor;
        }

        public static System.Drawing.Pen AsGdiPen(this System.Windows.Media.Pen pen)
        {
            var brush = pen.Brush as SolidColorBrush;
            brush = brush ?? Brushes.Transparent;
            System.Drawing.Color color = brush.Color.AsGdiColor();
            var gdiPen = new System.Drawing.Pen(color, (float)pen.Thickness);

            return gdiPen;
        }

        public static System.Drawing.PointF AsGdiPointF(this System.Windows.Point point)
        {
            var gdiPoint = new System.Drawing.PointF((float)point.X, (float)point.Y);
            return gdiPoint;
        }

        public static System.Drawing.Point AsGdiPoint(this System.Windows.Point point)
        {
            var gdiPoint = new System.Drawing.Point(Convert.ToInt32(Math.Round(point.X)), Convert.ToInt32(Math.Round(point.Y)));
            return gdiPoint;
        }
    }
}
