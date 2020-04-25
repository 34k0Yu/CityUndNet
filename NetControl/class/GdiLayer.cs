using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using GDI = System.Drawing; //添加应用->框架->System.Drawing
using System.IO;
using System.Drawing.Imaging;

namespace NetControl
{
    class GdiLayer : FrameworkElement
    {
        public int width = 0;
        public int height = 0;

        private WriteableBitmap bitmap;


        private GDI.SolidBrush FILL_COLOR;
        private GDI.Color LINE_COLOR;

        public GdiLayer()
        {
        }

        #region <!--格式转换-->

        public BitmapImage getImage()
        {
            Bitmap _bitmap = ConvertWriteableBitmapToBitmap(bitmap);
            SaveXXX();
            return BitmapToBitmapImage(_bitmap);
        }

        //WriteableBitmap -> Bitmap
        public Bitmap ConvertWriteableBitmapToBitmap(WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(wbm));
                enc.Save(outStream);
                Bitmap _bitmap = new Bitmap(outStream);
                _bitmap.MakeTransparent(System.Drawing.Color.Black);    //背景透明化处理
                return new System.Drawing.Bitmap(_bitmap);
            }            
        }

        // BitmapImage --> Bitmap
        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap _bitmap = new Bitmap(outStream);

                return new System.Drawing.Bitmap(_bitmap);
            }
        }

        // Bitmap --> BitmapImage
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png); // 坑点：格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        //WriteableBitmap -> BitmapImage
        public BitmapImage ConvertWriteableBitmapToBitmapImage(WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }
        #endregion

        /// <summary>
        /// C# WPF 保存WriteableBitmap图像
        /// </summary>
        /// <param name="wtbBmp"></param>
        /// 
        /*
         * 函数功能：SaveXXX
         * 函数功能介绍：将bitmap中的图形保存至文件
         * 形参列表：无
         * 返回值：void
         */
        public void SaveXXX()
        {
            WriteableBitmap wtbBmp = bitmap;
            if (wtbBmp == null)
            {
                return;
            }
            try
            {
                RenderTargetBitmap rtbitmap = new RenderTargetBitmap(wtbBmp.PixelWidth, wtbBmp.PixelHeight, wtbBmp.DpiX, wtbBmp.DpiY, PixelFormats.Default);
                DrawingVisual drawingVisual = new DrawingVisual();
                using (var dc = drawingVisual.RenderOpen())
                {
                    dc.DrawImage(wtbBmp, new Rect(0, 0, wtbBmp.Width, wtbBmp.Height));
                }
                rtbitmap.Render(drawingVisual);

                JpegBitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(rtbitmap));
                string strDir = @"C:\XXX\";
                string strpath = strDir + DateTime.Now.ToString("yyyyMMddfff") + ".jpg";
                if (!Directory.Exists(strDir))
                {
                    Directory.CreateDirectory(strDir);
                }
                if (!File.Exists(strpath))
                {
                    bitmapEncoder.Save(File.OpenWrite(strpath));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /*
         * 函数功能：ClearBitmap
         * 函数功能介绍：清除屏幕显示
         * 形参列表：无
         * 返回值：void
         */
        public void ClearBitmap()
        {
            using (GDI.Bitmap backBufferBitmap = new GDI.Bitmap(width, height,
                this.bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format24bppRgb,
                this.bitmap.BackBuffer))
            {
                using (GDI.Graphics backBufferGraphics = GDI.Graphics.FromImage(backBufferBitmap))
                {
                    backBufferGraphics.Clear(GDI.Color.Transparent);
                    backBufferGraphics.Flush();
                }
            }
        }

        /*
         * 函数功能：DrawPolygon
         * 函数功能介绍：绘制多边形
         * 形参列表：points（PointF[]，多边形各点的屏幕坐标）
         * 返回值：void
         */
        public void DrawPolygon(PointF[] points)
        {
            DrawPolygon(points, System.Drawing.Color.Black);
        }
        public void DrawPolygon(PointF[] points, System.Drawing.Color color)
        {
            this.bitmap.Lock();

            using (GDI.Bitmap backBufferBitmap = new GDI.Bitmap(width, height,
                this.bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format24bppRgb,
                this.bitmap.BackBuffer))
            {
                using (GDI.Graphics backBufferGraphics = GDI.Graphics.FromImage(backBufferBitmap))
                {
                    backBufferGraphics.SmoothingMode = GDI.Drawing2D.SmoothingMode.HighSpeed;
                    backBufferGraphics.CompositingQuality = GDI.Drawing2D.CompositingQuality.HighSpeed;

                    //backBufferGraphics.DrawLines(new System.Drawing.Pen(LINE_COLOR, (float)0.2), points);  //add
                    if (points[0] == points[points.Count() - 1])
                    {
                        backBufferGraphics.FillClosedCurve(new GDI.SolidBrush(color), points, GDI.Drawing2D.FillMode.Winding);
                        //backBufferGraphics.FillPolygon(FILL_COLOR, points, GDI.Drawing2D.FillMode.Alternate);
                    }
                    backBufferGraphics.Flush();
                }
            }
            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            this.bitmap.Unlock();
        }

        /*
         * 函数功能：DrawLine
         * 函数功能介绍：绘制线条
         * 形参列表：points（PointF[]，线条的屏幕坐标）
         * 返回值：void
         */
        public BitmapImage DrawLine(PointF[] points, System.Drawing.Color color, int _width, int _heigh)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(new GDI.SolidBrush(color), (float)2);

            width = _width;
            height = _heigh;

            this.bitmap.Lock();

            using (GDI.Bitmap backBufferBitmap = new GDI.Bitmap(width, height,
                this.bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format24bppRgb,
                this.bitmap.BackBuffer))
            {
                using (GDI.Graphics backBufferGraphics = GDI.Graphics.FromImage(backBufferBitmap))
                {
                    backBufferGraphics.SmoothingMode = GDI.Drawing2D.SmoothingMode.HighSpeed;
                    backBufferGraphics.CompositingQuality = GDI.Drawing2D.CompositingQuality.HighSpeed;

                    pen = new System.Drawing.Pen(new GDI.SolidBrush(System.Drawing.Color.Red), (float)2);
                    backBufferGraphics.DrawLines(pen, points);
                    backBufferGraphics.Flush();
                }
            }
            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            this.bitmap.Unlock();

            Bitmap _bitmap =  ConvertWriteableBitmapToBitmap(bitmap);
            //SaveXXX();
            return BitmapToBitmapImage(_bitmap);
        }

        public void DrawLine(PointF[] points, System.Drawing.Color color)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(new GDI.SolidBrush(color), (float)0.5);

            this.bitmap.Lock();

            using (GDI.Bitmap backBufferBitmap = new GDI.Bitmap(width, height,
                this.bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format24bppRgb,
                this.bitmap.BackBuffer))
            {
                using (GDI.Graphics backBufferGraphics = GDI.Graphics.FromImage(backBufferBitmap))
                {
                    backBufferGraphics.SmoothingMode = GDI.Drawing2D.SmoothingMode.HighSpeed;
                    backBufferGraphics.CompositingQuality = GDI.Drawing2D.CompositingQuality.HighSpeed;
                    backBufferGraphics.DrawLines(pen, points);
                    backBufferGraphics.Flush();
                }
            }
            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            this.bitmap.Unlock();
        }

        /*
         * 函数功能：DrawLine1
         * 函数功能介绍：保留的绘制线条功能
         * 形参列表：points（PointF[]，线条的屏幕坐标）
         * 返回值：void
         */
        public void DrawLine1(PointF[] points, System.Drawing.Color color)
        {
            System.Drawing.Pen pen = new System.Drawing.Pen(new GDI.SolidBrush(color), 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            pen.DashPattern = new float[] { 5, 5 };

            this.bitmap.Lock();

            using (GDI.Bitmap backBufferBitmap = new GDI.Bitmap(width, height,
                this.bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format24bppRgb,
                this.bitmap.BackBuffer))
            {
                using (GDI.Graphics backBufferGraphics = GDI.Graphics.FromImage(backBufferBitmap))
                {
                    backBufferGraphics.SmoothingMode = GDI.Drawing2D.SmoothingMode.HighSpeed;
                    backBufferGraphics.CompositingQuality = GDI.Drawing2D.CompositingQuality.HighSpeed;

                    backBufferGraphics.DrawLines(pen, points);
                    backBufferGraphics.Flush();
                }
            }
            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            this.bitmap.Unlock();
        }



        /*
         * 函数功能：OnRender
         * 函数功能介绍：重写后台绘图事件
         * 形参列表：dc（DrawingContext，图形纹理）
         * 返回值：void
         */


        protected override void OnRender(DrawingContext dc)
        {
            if (bitmap == null)
            {
                this.width = (int)RenderSize.Width;
                this.height = (int)RenderSize.Height;
                this.bitmap = new WriteableBitmap(width, height, 8192, 8192, PixelFormats.Bgr24, null);

                this.bitmap.Lock();
                using (GDI.Bitmap backBufferBitmap = new GDI.Bitmap(width, height,
                this.bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format24bppRgb,
                this.bitmap.BackBuffer))
                {
                    using (GDI.Graphics backBufferGraphics = GDI.Graphics.FromImage(backBufferBitmap))
                    {
                        backBufferGraphics.SmoothingMode = GDI.Drawing2D.SmoothingMode.HighSpeed;
                        backBufferGraphics.CompositingQuality = GDI.Drawing2D.CompositingQuality.HighSpeed;

                        backBufferGraphics.Clear(GDI.Color.Transparent);

                        backBufferGraphics.Flush();
                    }
                }
                this.bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
                this.bitmap.Unlock();

            }
            dc.DrawImage(bitmap, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            base.OnRender(dc);
        }
    }

}
