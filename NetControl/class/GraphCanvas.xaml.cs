using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

//NOTE: default Pen class is the gdi pen class.
using Point = System.Drawing.Point;
using Pen = System.Drawing.Pen;

namespace NetControl
{
    /// <summary>
    /// GraphCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class GraphCanvas : UserControl
    {
        public BackgroundWorker RenderWorker;

        protected bool IsBitmapInitialized { get; set; }

        protected System.Drawing.Bitmap GdiBitmap;
        protected Graphics GdiGraphics;
        protected InteropBitmap InteropBitmap;

        const uint FILE_MAP_ALL_ACCESS = 0xF001F;
        const uint PAGE_READWRITE = 0x04;
        protected PixelFormat PixelFormat;

        protected System.Drawing.Imaging.PixelFormat GdiPixelFormat;
        protected int BytesPerPixel;

        private int ImageWidth = 1;
        private int ImageHeight = 1;

        public GraphCanvas()
        {
            InitializeComponent();

            GdiPixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
            PixelFormat = PixelFormats.Pbgra32;
            BytesPerPixel = PixelFormat.BitsPerPixel / 8;

            this.SizeChanged += new SizeChangedEventHandler(GraphCanvas_SizeChanged);

            RenderWorker = new BackgroundWorker();
            RenderWorker.DoWork += new DoWorkEventHandler(RenderWorker_DoWork);
            RenderWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RenderWorker_RunWorkerCompleted);
            RenderWorker.WorkerSupportsCancellation = true;
        }

        private bool _isBusy;
        public bool IsBusy { get { return RenderWorker.IsBusy || _isBusy; } }

        private bool IsBitmapInvalid = true;
        void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            IsBitmapInvalid = true;
        }

        protected DateTime RenderStarted = DateTime.Now;

        public void UpdateImageAsync(IEnumerable<GraphSeries> renderDataList)
        {
            if (this.IsBusy)
            {
                Debug.WriteLine("RenderWorker busy.");
                return;
            }

            // copy actualwidth and height to my variables here in this thread. 
            // but don't touch while renderWorker is using them.
            // the "isBusy" condition above will ensure this is safe.
            ImageWidth = Convert.ToInt32(this.ActualWidth);
            ImageHeight = Convert.ToInt32(this.ActualHeight);

            RenderStarted = DateTime.UtcNow;
            _isBusy = true;
            RenderWorker.RunWorkerAsync(renderDataList);
        }

        private void RenderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _isBusy = false;
                return;
            }

            try
            {
                var bmpsrc = (e.Result as BitmapSource);

                if (bmpsrc != null && bmpsrc.CheckAccess())
                {
                    TheImage.Source = bmpsrc;
                }
                else
                {
                    Debug.WriteLine("No access to TheImage");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occured getting image from drawing thread.");
            }
            finally
            {
                _isBusy = false;
            }

            //Debug.WriteLine("Render finished in " + (DateTime.UtcNow - RenderStarted).TotalMilliseconds + "ms");
        }

        private void RenderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var renderDataList = (List<GraphSeries>)e.Argument;

                //return if nothing to draw.
                if (renderDataList == null || renderDataList.Count == 0)
                {
                    return;
                }

                Initialize();

                RenderSeries(renderDataList);

                InteropBitmap.Invalidate();
                InteropBitmap.Freeze();

                e.Result = InteropBitmap;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred in RenderWorker_DoWork: " + ex.Message);
            }
        }

        public void RenderSeries(IEnumerable<GraphSeries> renderDataList)
        {
            foreach (var s in renderDataList)
            {
                if (s.IsFilled)
                {
                    DrawFilledPath(s.RenderData.Figures, s.RenderData.GdiPen, s.RenderData.GdiFill);
                }
                else if (s.IsPoints)
                {
                    DrawPoints(s.RenderData.Figures, s.RenderData.GdiPen, 5);
                }
                else
                {
                    DrawPath(s.RenderData.Figures, s.RenderData.GdiPen);
                }
            }
        }

        protected void DrawPoints(List<SeriesFigure> figures, System.Drawing.Pen gdiPen, float d)
        {
            foreach (SeriesFigure figure in figures)
            {
                PointF point = figure.Points[0];
                point.X -= d / 2;
                point.Y -= d / 2;
                RectangleF rect = new RectangleF(point, new SizeF(d, d));
                GdiGraphics.DrawEllipse(gdiPen, rect);
            }
        }

        protected void DrawPath(List<SeriesFigure> figures, System.Drawing.Pen gdiPen)
        {
            foreach (var figure in figures) // could do parallel For loop here...
            {
                var points = figure.Points;
                var pointCount = figure.Points.Count;
                for (int i = 0; i < pointCount - 1; i++)
                {
                    GdiGraphics.DrawLine(gdiPen, points[i], points[i + 1]);
                }
            }
        }

        protected void DrawFilledPath(List<SeriesFigure> figures, System.Drawing.Pen gdiPen, System.Drawing.SolidBrush gdiBrush)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            foreach (var figure in figures) // could do parallel For loop here...
            {
                path.StartFigure();

                var points = figure.Points;
                var pointCount = figure.Points.Count;
                for (int i = 0; i < pointCount - 1; i++)
                {
                    path.AddLine(points[i], points[i + 1]);
                }
            }
            path.CloseAllFigures();
            GdiGraphics.FillPath(gdiBrush, path);
            GdiGraphics.DrawPath(gdiPen, path);
        }

        public void Clear()
        {
            //The next draw will release the image bitmap from memory.
            TheImage.Source = null;
        }


        /// <summary>
        /// Alternate to UpdateImageAsync. NOTE: Using both calls in the same session may cause multi-threading errors.
        /// </summary>
        /// <param name="renderDataList"></param>
        //public void UpdateImage(IEnumerable<GraphSeriesRenderData> renderDataList)
        //{
        //    if (this.ActualHeight < 5 || this.ActualWidth < 5) { return; }

        //    if (renderDataList == null)
        //    {
        //        ClearGdiGraphics();
        //    }
        //    else
        //    {
        //        RenderSeries(renderDataList);
        //    }

        //    InteropBitmap.Invalidate();
        //    TheImage.Source = InteropBitmap;
        //}

        protected IntPtr MapFileHandle = (IntPtr)(-1);
        protected IntPtr MapViewPtr = (IntPtr)(-1);

        protected void Initialize()
        {
            if (IsBitmapInitialized || IsBitmapInvalid)
            {
                Deallocate();
            }

            // this additional copy may not be necessary, but i want to keep using a local variable, and i don't want to access this.ActualWidth from this thread.
            int actualWidth = this.ImageWidth;
            int actualHeight = this.ImageHeight;

            // but at least 1 pixel to avoid problems.
            actualHeight = Math.Max(1, actualHeight);
            actualWidth = Math.Max(1, actualWidth);

            uint byteCount = (uint)(actualWidth * actualHeight * BytesPerPixel);

            // Make memory map
            MapFileHandle = NativeMethods.CreateFileMapping(new IntPtr(-1), IntPtr.Zero, PAGE_READWRITE, 0, byteCount, null);
            MapViewPtr = NativeMethods.MapViewOfFile(MapFileHandle, FILE_MAP_ALL_ACCESS, 0, 0, byteCount);

            //Create the InteropBitmap  
            InteropBitmap = Imaging.CreateBitmapSourceFromMemorySection(MapFileHandle,
                                                                        (int)actualWidth,
                                                                        (int)actualHeight,
                                                                        PixelFormat,
                                                                        (int)(actualWidth * PixelFormat.BitsPerPixel / 8),
                                                                        0)
                                                                        as InteropBitmap;
            GdiGraphics = GetGdiGraphics(MapViewPtr);

            IsBitmapInitialized = true;
            IsBitmapInvalid = false;
        }

        private void Deallocate()
        {
            if (GdiGraphics != null)
            {
                GdiGraphics.Dispose();
            }
            try
            {
                if (MapViewPtr != (IntPtr)(-1))
                {
                    NativeMethods.UnmapViewOfFile(MapViewPtr);
                }
                if (MapFileHandle != (IntPtr)(-1))
                {
                    NativeMethods.CloseHandle(MapFileHandle);
                }
            }
            catch (Exception ex)
            {
                // most likely error is because we freed twice, so don't worry about it. 
                // if its in use, let them worry about it.
#if DEBUG
                MessageBox.Show("Error occurred freeing bitmap in GraphCanvas." + ex.Message);
#endif
            }
            finally
            {
                MapViewPtr = (IntPtr)(-1);
                MapFileHandle = (IntPtr)(-1);
                IsBitmapInitialized = false;
            }
        }

        private Graphics GetGdiGraphics(IntPtr mapPointer)
        {
            int actualWidth = Convert.ToInt32(this.ImageWidth);
            int actualHeight = Convert.ToInt32(this.ImageHeight);
            actualHeight = Math.Max(1, actualHeight);
            actualWidth = Math.Max(1, actualWidth);

            //create the GDI Bitmap 
            GdiBitmap = new System.Drawing.Bitmap(actualWidth,
                                                  actualHeight,
                                                  actualWidth * BytesPerPixel,
                                                  GdiPixelFormat,
                                                  mapPointer);
            // Get GDI Graphics 
            Graphics gdiGraphics = System.Drawing.Graphics.FromImage(GdiBitmap);
            gdiGraphics.CompositingMode = CompositingMode.SourceCopy;
            gdiGraphics.CompositingQuality = CompositingQuality.HighSpeed;
            gdiGraphics.SmoothingMode = SmoothingMode.HighSpeed;

            return gdiGraphics;
        }
    }
}
