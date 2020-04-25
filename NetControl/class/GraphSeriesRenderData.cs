using System;
using System.Collections.Generic;
using System.Linq;
using Point = System.Drawing.Point;
using System.Drawing;

namespace NetControl
{
    public class SeriesFigure
    {
        public List<PointF> Points { get; set; }
        public SeriesFigure()
        {
            Points = new List<PointF>();
        }
    }

    public class GraphSeriesRenderData
    {
        public List<SeriesFigure> Figures { get; set; }
        public GraphSeries Series { get; set; }

        public System.Drawing.Pen GdiPen { get; set; }
        public System.Drawing.SolidBrush GdiFill { get; set; }

        public GraphSeriesRenderData(GraphSeries series)
        {
            Series = series;
            Figures = new List<SeriesFigure>();
        }

        public void Clear()
        {
            Figures.Clear();
        }


        public void LineTo(Point nextPoint)
        {
            Figures.Last().Points.Add(nextPoint);
        }

        public void StartFigure(Point firstPoint)
        {
            var newfig = new SeriesFigure();
            newfig.Points.Add(firstPoint);
            Figures.Add(newfig);
        }


        protected virtual Point BeginLine(Point nextPoint)
        {
            if (!Double.IsInfinity(nextPoint.Y) && !Double.IsNaN(nextPoint.Y)
                && !Double.IsInfinity(nextPoint.X) && !Double.IsNaN(nextPoint.X))
            {
                StartFigure(nextPoint);
                Series.IsLineBroken = false;
            }
            else
            {
                Series.IsLineBroken = true;
            }

            return nextPoint;
        }


        protected virtual void CloseFilledSeries(Double maxX, Double maxY)
        {
            if (Series.IsLineBroken)
            {
                return;
            }

            var nextPoint = new Point();
            //Take the line one pixel off screen to the left.
            nextPoint.X = Convert.ToInt32(Math.Round(Series.MinX - 1));
            nextPoint.Y = Convert.ToInt32(Math.Round(Series.LastY));
            LineTo(nextPoint);

            // take the line to the bottom.
            nextPoint.X = Convert.ToInt32(Math.Round(Series.MinX - 1));
            nextPoint.Y = Convert.ToInt32(Math.Round(maxY + 1)); // just offscreen
            LineTo(nextPoint);

            // take it to the lower right to close.
            nextPoint = new Point();
            nextPoint.X = Convert.ToInt32(Math.Round(maxX + 1));
            nextPoint.Y = Convert.ToInt32(Math.Round(maxY + 1));
            LineTo(nextPoint);
        }

    }
}
