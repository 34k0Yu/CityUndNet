using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NetControl
{
    [Serializable]
    public class devicedata
    {
        public List<twowayvalve_data> twdata = new List<twowayvalve_data>();
        public List<threewayvalve_data> thdata = new List<threewayvalve_data>();
        public List<line_attribute_t> linedata = new List<line_attribute_t>();
        public List<fourwayvalve_data> fodata = new List<fourwayvalve_data>();
        public List<fivewayvalve_data> fidata = new List<fivewayvalve_data>();
        public List<scadatest_data> scdata = new List<scadatest_data>();
        public List<user_data> usdata = new List<user_data>();
        public List<pel_data> pedata = new List<pel_data>();
        public List<valve_data> vadata = new List<valve_data>();
        public List<regulatestation_data> redata = new List<regulatestation_data>();
        public List<gasstation_data> gadata = new List<gasstation_data>();
    }
    [Serializable]
    public class line_attribute_t//管线
    {
        public int id = 0;//编号
        public string name = null;//名称
        public double roughness = 0;//粗糙度
        public string material = null;//材质
        public double thickness = 0;//壁厚
        public string damage = null;//设备状态
        public double length = 0;//长度
        public int equip_1_id = 0;//口1连接元件编号,即端口1连接的设备id号
        public int eqinter_1_id = 0;//口1连接元件接口编号
        public int equip_1_h = 0;//口1高度
        public int equip_2_id = 0;//口2连接元件编号,即端口2连接的设备id号
        public int eqinter_2_id = 0;//口2连接元件接口编号
        public int equip_2_h = 0;//口2高度
        public double pressure_1 = 0;//口1压力
        public double speed_1 = 0;//口1速度    
        public double tempera_1 = 0;//口1温度
        public double pressure_2 = 0;//口2压力
        public double speed_2 = 0;//口2速度
        public double tempera_2 = 0;//口2温度
        public double pa = 0;//压降pa
        public double frictional = 0;//比摩阻
        public double flow = 0;//质量流量
        public double coefficient = 0;//平均摩擦系数
        public double compress = 0;//压缩因子
        public double diameter = 0;//内径
        public double lat = 0;//中心点纬度
        public double lng = 0;//中心点经度
        public double startX = 0;//起点X
        public double startY = 0;//起点Y
        public double endX = 0;//终点X
        public double endY = 0;//终点Y
    }
    [Serializable]
    public class twowayvalve_data//两通
    {
        public int id = 0;//编号
        public int count = 2;//端口数
        public string name = null;//名称
        public double lat = 0;//纬度
        public double lng = 0;//经度
        public string damage = null;//设备状态
        public int equip_1_id = 0;//口1连接元件编号,即端口1连接的管线id号
        public int eqinter_1_id = 0;//口1连接元件接口编号
        public double diameter_1 = 0;//口1管径
        public int equip_2_id = 0;//口2连接元件编号,即端口2连接的管线id号
        public int eqinter_2_id = 0;//口2连接元件接口编号
        public double diameter_2 = 0;//口2管径
    }
    [Serializable]
    public class threewayvalve_data: twowayvalve_data//三通
    {
        public new int count = 3;//端口数
        public int equip_3_id = 0;//口3连接元件编号
        public int eqinter_3_id = 0;//口3连接元件接口编号
        public double diameter_3 = 0;//口3管径
    }
    [Serializable]
    public class fourwayvalve_data: threewayvalve_data//四通
    {
        public new int count = 4;//端口数
        public int equip_4_id = 0;//口4连接元件编号
        public int eqinter_4_id = 0;//口4连接元件接口编号
        public double diameter_4 = 0;//口4管径
    }
    [Serializable]
    public class fivewayvalve_data: fourwayvalve_data//五通
    {
        public new int count = 5;//端口数
        public int equip_5_id = 0;//口5连接元件编号
        public int eqinter_5_id = 0;//口5连接元件接口编号
        public double diameter_5 = 0;//口5管径
    }
    [Serializable]
    public class gasstation_data//供气站
    {
        public int id = 0;//编号
        public int count = 2;//端口数
        public string name = null;//名称
        public double lat = 0;//纬度
        public double lng = 0;//经度
        public int sign = 0;//类型标识
        public int equip_1_id = 0;//口1连接元件编号
        public int equip_2_id = 0;//口2连接元件编号
        public int eqinter_1_id = 0;//口1连接元件接口编号
        public int eqinter_2_id = 0;//口2连接元件接口编号
        public double pressure = 0;//压力
        public double speed = 0;//速度
        public double tempera = 0;//温度
    }
    [Serializable]
    public class regulatestation_data//调压站
    {
        public int id = 0;//编号
        public int count = 2;//端口数
        public string name = null;//名称
        public double lat = 0;//纬度
        public double lng = 0;//经度
        public int sign = 0;//类型标识
        public int equip_1_id = 0;//口1连接元件编号
        public int equip_2_id = 0;//口2连接元件编号
        public int eqinter_1_id = 0;//口1连接元件接口编号
        public int eqinter_2_id = 0;//口2连接元件接口编号
        public double inpressure = 0;//进口压强
        public double influ = 0;//进口流量
        public double intempera = 0;//进口温度
        public double outpressure = 0;//出口压强
        public double outflu = 0;//出口流量
        public double outtempera = 0;//出口温度
    }
    [Serializable]
    public class scadatest_data : gasstation_data { }//scada测点
    [Serializable]
    public class user_data : gasstation_data { }//终端用户
    [Serializable]
    public class valve_data : twowayvalve_data { }//阀门
    [Serializable]
    public class pel_data//燃气
    {
        public int id = 0;//编号
        public int count = 1;//端口数
        public string name = null;//名称
        public double lat = 0;//纬度
        public double lng = 0;//经度
        public int equip_1_id = 0;//口1连接元件编号
        public int eqinter_1_id = 0;//口1连接元件接口编号
        public int equip_2_id = 0;//口2连接元件编号
        public int eqinter_2_id = 0;//口2连接元件接口编号
        public double methane = 0;//甲烷
        public double ethane = 0;//乙烷
        public double propane = 0;//丙烷
        public double nitrogen = 0;//氮气
        public double hydrogen = 0;//氢气
        public double carbon = 0;//二氧化碳
    }

    [Serializable]
    public class elemt_data
    {
        public string filepath;
        public string resultname;
        public List<Mapnode> mapnode = new List<Mapnode>();
        public List<myline> myline = new List<myline>();
        public List<myMap> myGmap = new List<myMap>();
        public List<myCanvas> myCanvas = new List<myCanvas>();
        public List<line_attribute_t> Lines = new List<line_attribute_t>();
        public List<twowayvalve_data> two = new List<twowayvalve_data>();
        public List<threewayvalve_data> three = new List<threewayvalve_data>();
        public List<fourwayvalve_data> four = new List<fourwayvalve_data>();
        public List<fivewayvalve_data> five = new List<fivewayvalve_data>();
        public List<gasstation_data> gas = new List<gasstation_data>();
        public List<regulatestation_data> regul = new List<regulatestation_data>();
        public List<scadatest_data> scada = new List<scadatest_data>();
        public List<user_data> user = new List<user_data>();
        public List<valve_data> valve = new List<valve_data>();
        public List<pel_data> pel = new List<pel_data>();
    }
    [Serializable]
    public class Mapnode//设备信息
    {
        public double imgwidth;
        public double imgheight;
        public double width;
        public double height;
        public Point move;
        public int ultisign;
        public string sign;
        public double Scale;
        public string name;
        public string path;
        public string damage;
        public List<string> chilname;
    }
    [Serializable]
    public class myline//线段信息
    {
        public double X1;
        public double Y1;
        public double X2;
        public double Y2;
        public double Thickness;
        public string name;
        public double rotate;
    }
    [Serializable]
    public class myMap
    {
        public PointLatLng center;
        public int mzoom;
    }
    [Serializable]
    public class myCanvas
    {
        public string name;
        public double ScaleX;
        public double ScaleY;
        public double with;
        public double height;
        public Thickness thickness;
        public Point render;
    }

    class Equipment
    {   
    }
}
