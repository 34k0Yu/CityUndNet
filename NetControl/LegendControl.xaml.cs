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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace NetControl
{
    /// <summary>
    /// LegendControl.xaml 的交互逻辑
    /// </summary>
    public partial class LegendControl : UserControl
    {
        public LegendControl()
        {
            InitializeComponent();

        }


        [Description("图例模式 1：压强 2：温度"), Category("控件接口")]
        public int Legend_mode
        {
            get { return (int)base.GetValue(Legend_modeProperty); }
            set { base.SetValue(Legend_modeProperty, value); }
        }
        public static readonly DependencyProperty Legend_modeProperty =
        DependencyProperty.Register("Legend_mode", typeof(int), typeof(LegendControl), null);



        [Description("最低（向上取整）"), Category("控件接口")]
        public double Legend_Minimum
        {
            get { return (double)base.GetValue(Legend_MinimumProperty); }
            set { base.SetValue(Legend_MinimumProperty, value); }
        }
        public static readonly DependencyProperty Legend_MinimumProperty =
        DependencyProperty.Register("Legend_Minimum", typeof(double), typeof(LegendControl), null);

        [Description("最大（向下取整）"), Category("控件接口")]
        public double Legend_Maximum
        {
            get { return (double)base.GetValue(Legend_MaximumProperty); }
            set { base.SetValue(Legend_MaximumProperty, value); }
        }
        public static readonly DependencyProperty Legend_MaximumProperty =
        DependencyProperty.Register("Legend_Maximum", typeof(double), typeof(LegendControl), null);

        [Description("数据总数"), Category("控件接口")]
        public int Legend_count
        {
            get { return (int)base.GetValue(Legend_countProperty); }
            set { base.SetValue(Legend_countProperty, value); }
        }
        public static readonly DependencyProperty Legend_countProperty =
        DependencyProperty.Register("Legend_count", typeof(int), typeof(LegendControl), null);

        [Description("Step1"), Category("控件接口")]
        public double Legend_Step1
        {
            get { return (double)base.GetValue(Legend_Step1Property); }
            set { base.SetValue(Legend_Step1Property, value); }
        }
        public static readonly DependencyProperty Legend_Step1Property =
        DependencyProperty.Register("Legend_Step1", typeof(double), typeof(LegendControl), null);

        [Description("Step1"), Category("控件接口")]
        public double Legend_Step2
        {
            get { return (double)base.GetValue(Legend_Step2Property); }
            set { base.SetValue(Legend_Step2Property, value); }
        }
        public static readonly DependencyProperty Legend_Step2Property =
        DependencyProperty.Register("Legend_Step2", typeof(double), typeof(LegendControl), null);

        [Description("Step3"), Category("控件接口")]
        public double Legend_Step3
        {
            get { return (double)base.GetValue(Legend_Step3Property); }
            set { base.SetValue(Legend_Step3Property, value); }
        }
        public static readonly DependencyProperty Legend_Step3Property =
        DependencyProperty.Register("Legend_Step3", typeof(double), typeof(LegendControl), null);

        [Description("Step4"), Category("控件接口")]
        public double Legend_Step4
        {
            get { return (double)base.GetValue(Legend_Step4Property); }
            set { base.SetValue(Legend_Step4Property, value); }
        }
        public static readonly DependencyProperty Legend_Step4Property =
        DependencyProperty.Register("Legend_Step4", typeof(double), typeof(LegendControl), null);

        [Description("Step5"), Category("控件接口")]
        public double Legend_Step5
        {
            get { return (double)base.GetValue(Legend_Step5Property); }
            set { base.SetValue(Legend_Step5Property, value); }
        }
        public static readonly DependencyProperty Legend_Step5Property =
        DependencyProperty.Register("Legend_Step5", typeof(double), typeof(LegendControl), null);

        [Description("Step6"), Category("控件接口")]
        public double Legend_Step6
        {
            get { return (double)base.GetValue(Legend_Step6Property); }
            set { base.SetValue(Legend_Step6Property, value); }
        }
        public static readonly DependencyProperty Legend_Step6Property =
        DependencyProperty.Register("Legend_Step6", typeof(double), typeof(LegendControl), null);

        [Description("Step7"), Category("控件接口")]
        public double Legend_Step7
        {
            get { return (double)base.GetValue(Legend_Step7Property); }
            set { base.SetValue(Legend_Step7Property, value); }
        }
        public static readonly DependencyProperty Legend_Step7Property =
        DependencyProperty.Register("Legend_Step7", typeof(double), typeof(LegendControl), null);

        [Description("Step8"), Category("控件接口")]
        public double Legend_Step8
        {
            get { return (double)base.GetValue(Legend_Step8Property); }
            set { base.SetValue(Legend_Step8Property, value); }
        }
        public static readonly DependencyProperty Legend_Step8Property =
        DependencyProperty.Register("Legend_Step8", typeof(double), typeof(LegendControl), null);

        [Description("Step9"), Category("控件接口")]
        public double Legend_Step9
        {
            get { return (double)base.GetValue(Legend_Step9Property); }
            set { base.SetValue(Legend_Step9Property, value); }
        }
        public static readonly DependencyProperty Legend_Step9Property =
        DependencyProperty.Register("Legend_Step9", typeof(double), typeof(LegendControl), null);

        [Description("Step10"), Category("控件接口")]
        public double Legend_Step10
        {
            get { return (double)base.GetValue(Legend_Step10Property); }
            set { base.SetValue(Legend_Step10Property, value); }
        }
        public static readonly DependencyProperty Legend_Step10Property =
        DependencyProperty.Register("Legend_Step10", typeof(double), typeof(LegendControl), null);


    }


    public class inttoVisibleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int A = System.Convert.ToInt32(value);
            int B = System.Convert.ToInt32(parameter);
            if (A == B)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return null;
        }
    }


    public class LegendShow_Convert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int Min = System.Convert.ToInt32(values[0]);
            int Max = System.Convert.ToInt32(values[1]);
            int Range = System.Convert.ToInt32(values[2]);//1：4  2：7
            int part;
            int Num = System.Convert.ToInt32(parameter);

            if (Min % 10 != 0)
                Min = ((int)(Min / 10 + 1)) * 10;

            if (Max % 10 != 0)
                Max = ((int)(Max / 10)) * 10;
            string A = "";
            if (Range == 1)//压强
            {
                part = (int)(((Max - Min) / 5) + 0.5F);//四舍五入
                switch (Num)
                {
                    case 1: A = System.Convert.ToString(">" + Max); break;
                    case 2: A = System.Convert.ToString((((int)((Min + part * 4) / 10)) * 10) + "-" + ((int)((Min + part * 5) / 10)) * 10); break;
                    case 3: A = System.Convert.ToString((((int)((Min + part * 3) / 10)) * 10) + "-" + ((int)((Min + part * 4) / 10)) * 10); break;
                    case 4: A = System.Convert.ToString((((int)((Min + part * 2) / 10)) * 10) + "-" + ((int)((Min + part * 3) / 10)) * 10); break;
                    case 5: A = System.Convert.ToString((((int)((Min + part * 1) / 10)) * 10) + "-" + ((int)((Min + part * 2) / 10)) * 10); break;
                    case 6: A = System.Convert.ToString(Min + "-" + ((int)((Min + part) / 10)) * 10); break;
                    case 7: A = System.Convert.ToString("<" + Min); break;
                    default: break;
                }
                return A;

            }
            else//温度
            {
                part = (int)(((Max - Min) / 2) + 0.5F);//四舍五入
                switch (Num)
                {
                    case 1: A = System.Convert.ToString(">" + Max); break;
                    case 2: A = System.Convert.ToString((((int)((Min + part * 1) / 10)) * 10) + "-" + ((int)((Min + part * 2) / 10)) * 10); break;
                    case 3: A = System.Convert.ToString(Min + "-" + ((int)((Min + part * 1) / 10)) * 10); break;
                    case 4: A = System.Convert.ToString("<" + Min); break;
                    default: break;
                }
                return A;
            }


        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }


    public class LegendShow_Convert_1 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double Min = System.Convert.ToDouble(values[0]);
            double Max = System.Convert.ToDouble(values[1]);
            int count = System.Convert.ToInt32(values[2]);
            double step1 = System.Convert.ToDouble(values[3]);
            double step2 = System.Convert.ToDouble(values[4]);
            double step3 = System.Convert.ToDouble(values[5]);
            double step4 = System.Convert.ToDouble(values[6]);
            double step5 = System.Convert.ToDouble(values[7]);
            double step6 = System.Convert.ToDouble(values[8]);
            double step7 = System.Convert.ToDouble(values[9]);
            double step8 = System.Convert.ToDouble(values[10]);
            double step9 = System.Convert.ToDouble(values[11]);
            double step10 = System.Convert.ToDouble(values[12]);

            int mode = System.Convert.ToInt32(values[13]);

            double part;
            int Num = System.Convert.ToInt32(parameter);
            string A = "";
            if (mode == 1)
            {

                if (count < 10)//若数据量小于10
                {

                    part = (Max - Min) / 9;//四舍五入
                    switch (Num)
                    {
                        case 1: A = System.Convert.ToString(Math.Round(Max, 4)); break;
                        case 2: A = System.Convert.ToString(Math.Round(Min + part * 8, 4)); break;
                        case 3: A = System.Convert.ToString(Math.Round(Min + part * 7, 4)); break;
                        case 4: A = System.Convert.ToString(Math.Round(Min + part * 6, 4)); break;
                        case 5: A = System.Convert.ToString(Math.Round(Min + part * 5, 4)); break;
                        case 6: A = System.Convert.ToString(Math.Round(Min + part * 4, 4)); break;
                        case 7: A = System.Convert.ToString(Math.Round(Min + part * 3, 4)); break;
                        case 8: A = System.Convert.ToString(Math.Round(Min + part * 2, 4)); break;
                        case 9: A = System.Convert.ToString(Math.Round(Min + part * 1, 4)); break;
                        case 10: A = System.Convert.ToString(Math.Round(Min, 4)); break;
                        default: break;
                    }
                }
                else//若数据量为10
                {
                    switch (Num)
                    {
                        case 1: A = System.Convert.ToString(Math.Round(step10, 4)); break;
                        case 2: A = System.Convert.ToString(Math.Round(step9, 4)); break;
                        case 3: A = System.Convert.ToString(Math.Round(step8, 4)); break;
                        case 4: A = System.Convert.ToString(Math.Round(step7, 4)); break;
                        case 5: A = System.Convert.ToString(Math.Round(step6, 4)); break;
                        case 6: A = System.Convert.ToString(Math.Round(step5, 4)); break;
                        case 7: A = System.Convert.ToString(Math.Round(step4, 4)); break;
                        case 8: A = System.Convert.ToString(Math.Round(step3, 4)); break;
                        case 9: A = System.Convert.ToString(Math.Round(step2, 4)); break;
                        case 10: A = System.Convert.ToString(Math.Round(step1, 4)); break;
                        default: break;
                    }

                }
            }
            else 
            {
                part = (Max - Min) / 9;//四舍五入
                switch (Num)
                {
                    case 1: A = System.Convert.ToString(Math.Round(Max, 1)); break;
                    case 2: A = System.Convert.ToString(Math.Round(Min + part * 8, 1)); break;
                    case 3: A = System.Convert.ToString(Math.Round(Min + part * 7, 1)); break;
                    case 4: A = System.Convert.ToString(Math.Round(Min + part * 6, 1)); break;
                    case 5: A = System.Convert.ToString(Math.Round(Min + part * 5, 1)); break;
                    case 6: A = System.Convert.ToString(Math.Round(Min + part * 4, 1)); break;
                    case 7: A = System.Convert.ToString(Math.Round(Min + part * 3, 1)); break;
                    case 8: A = System.Convert.ToString(Math.Round(Min + part * 2, 1)); break;
                    case 9: A = System.Convert.ToString(Math.Round(Min + part * 1, 1)); break;
                    case 10: A = System.Convert.ToString(Math.Round(Min, 1)); break;
                    default: break;
                }
            }


            return A;


        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
