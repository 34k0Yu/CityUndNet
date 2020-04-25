using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Globalization;

namespace NetControl
{
    class Draw:Canvas
    {
        public List<Visual> visuals = new List<Visual>();   //显示区

        private class VisualMember
        {
            public DrawingVisual visual;
            public string name = string.Empty;
            public bool visiable = false;

            public VisualMember(DrawingVisual _visual, string _name)
            {
                visual = _visual; name = _name; visiable = false;
            }
        }
        private List<VisualMember> visualMembers = new List<VisualMember>();    //存储区

        //获取Visual的个数
        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        //获取Visual
        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        //添加Visual
        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        //删除Visual
        public void RemoveVisual(Visual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        //命中测试
        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }

        //使用DrawVisual绘图

        public void DrawLine(List<Point> _list, string _group, Brush _color, double _thinkness)
        {
            DrawingVisual visual = new DrawingVisual();
            DrawingContext dc = visual.RenderOpen();
            Pen pen = new Pen(_color, _thinkness);
            pen.Freeze();  //冻结画笔，这样能加快绘图速度

            for (int i = 0; i < _list.Count - 1; i++)
            {
                dc.DrawLine(pen, _list[i], _list[i + 1]);
            }
            dc.Close();

            visualMembers.Add(new VisualMember(visual, _group));
            setVisiable(_group, true);
        }

        public void setVisiable(string _name, bool _visiable)
        {
            foreach (VisualMember member in visualMembers)
            {
                if (member.name.Equals(_name))
                {
                    if (member.visiable != _visiable)
                    {
                        member.visiable = _visiable;
                        if (_visiable) AddVisual(member.visual);
                        else RemoveVisual(member.visual);
                    }
                    break;
                }
            }
        }
    }
}
