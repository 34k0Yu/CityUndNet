   M S B A M L   `   `   `  ����E  ANetControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:NetControl
NetControl  5   	       NetControl.MainWindow   5   	   a ]DevExpress.Xpf.Core.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ;dx2http://schemas.devexpress.com/winfx/2008/xaml/core  5   	   <dxb2http://schemas.devexpress.com/winfx/2008/xaml/bars  5   	   c _DevExpress.Xpf.Ribbon.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxr4http://schemas.devexpress.com/winfx/2008/xaml/ribbon  5   	   ?dxe5http://schemas.devexpress.com/winfx/2008/xaml/editors  5   	   d `DevExpress.Xpf.Docking.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a@dxdo5http://schemas.devexpress.com/winfx/2008/xaml/docking  5   	   c _DevExpress.Xpf.Charts.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxc4http://schemas.devexpress.com/winfx/2008/xaml/charts  5	   	   >dxn4http://schemas.devexpress.com/winfx/2008/xaml/navbar  5
   	   e	 aDevExpress.Xpf.Controls.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aCdxwui7http://schemas.devexpress.com/winfx/2008/xaml/windowsui 	 5   	   a
 ]DevExpress.Xpf.Grid.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a<dxg2http://schemas.devexpress.com/winfx/2008/xaml/grid 
 5   	   ] YDevExpress.Mvvm.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49aAdxmvvm2http://schemas.devexpress.com/winfx/2008/xaml/mvvm   5   	   &localclr-namespace:NetControl   5   	   -   5          �Name$  
mainwindow��5   0    � Title$ NetControl V1.1��5   	   $	��860��6!   $
��1280��6.   $	��550��6;   $	��800��6K    � WindowStartupLocation$ CenterScreen=�5   	    � UseLayoutRounding .  � 
ResizeMode$ 	CanResize=�6G    � Icon$/ )pack://application:,,,/image/demoicon.ico��5   	    � 	Resources 5         NetControl.layeridArr 	  layer   NetControl.StringtoInt  stringtoint'   !NetControl.PointToStringConverter  statelatlng   NetControl.indicator  	indicator%   NetControl.PointToZoomConverter  showzoom��%�   &	        &	      &	 &     &	 9     &	 L       5   
   6     5   
   6     5   
   6'     5   
   6     5   
   6%   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   )  #DevExpress.Xpf.Ribbon.RibbonControl  $��0��6      RibbonStyle$ 
Office2010=�6)   	   ApplicationMenu	 5!      0  *DevExpress.Xpf.Ribbon.BackstageViewControl  5"      
   Items
 5#      /  )DevExpress.Xpf.Ribbon.BackstageButtonItem  5$      -   5        $
��Onew��5$   2   )	  #DevExpress.Xpf.Ribbon.BackstageItem 	  Content)
  #DevExpress.Xpf.Core.ObjectConverter$ 新建
 6>   6K     5%      -   5        $��Osava��5%   2   $ 保存
 6?   6L     5&      -   5        $	��Osa��5&   2   $ 	另存为
 6=   6K     5'      -   5        $��Oopen��5'   2   $ 打开
 6?   6L     5(      -   5        $��	closemain��5(   2   $ 关闭
 6C   6P   5*         Items. 5.      5  /DevExpress.Xpf.Ribbon.RibbonDefaultPageCategory  2  ,DevExpress.Xpf.Ribbon.RibbonPageCategoryBase   Pages. 5/      &   DevExpress.Xpf.Ribbon.RibbonPage     Caption$ 主页
 6!      Groups. 51      +  %DevExpress.Xpf.Ribbon.RibbonPageGroup     Caption$ 文件��6*      Items. 52      '  !DevExpress.Xpf.Bars.BarButtonItem  -   5          Name$ newmap��52   ,   !  DevExpress.Xpf.Bars.BarItem   Content$ 新建地图
 6<      Glyph 53   ,   *  $DevExpress.Xpf.Core.DXImageExtension     Image1  +DevExpress.Xpf.Core.Native.DXImageConverter$ DefaultMap_16x16.png    
LargeGlyph 6\     $ DefaultMap_32x32.png 54   ,     55      -   5        $ newItem��55   ,   $ 加载 工程底图
 6=    6O     $ New_16x16.png  6x     $ New_32x32.png 56   ,     58      -	   5        $ 	ExportCSV��58   ,   $ 	导出CSV
 59   ,    5:   ,     $ ExportToCSV_32x32.png  5;   ,     $ ExportToCSV_16x16.png 5<   ,   $  DevExpress.Xpf.Bars.BarSubItem  5=      -
   5          Name$ 	savestyle��5=   )   $ 保存
 6<      CloseSubMenuOnClick$
 TrueX�6I      BarItemDisplayMode$ ContentAndGlyph=�5>   )    5?   )     $ SaveAndNew_16x16.png  5@   )     $ SaveAndNew_32x32.png    Items. 5A        -   5        $ savethis��5A   0   $ 保存
 6B    5B   0     $ SaveAndNew_16x16.png  6`     $ SaveAndNew_32x32.png 5C   Q     5D      -   5        $ 	Savelayer��5D   0   $ 	另存为
 5E   ,    6:     $ SaveAndNew_16x16.png  5F   ,     $ SaveAndNew_32x32.png 5G   ,   5H        5I      $ 加载
 6*    67     $ Open2_16x16.png  5J   +     $ Open2_32x32.png . 5K        -   5        $ 	Loadlayer��5K   0   $ 加载
 5L   ,    69   4  .DevExpress.Xpf.Core.DXImageOffice2013Extension     Image;  5DevExpress.Xpf.Core.Native.DXImageOffice2013Converter$ LoadFrom_16x16.png  5M   ,     $ LoadFrom_32x32.png 5N   ,     5O      -   5        $ import��5O   0   $ 导入
 6@    5P   0     $ Inbox_16x16.png  6[     $ Inbox_32x32.png 5Q   0   5R        5T      -   5        $ print��5T   =   $ 打印 管道图
 6,    5U   ,     $ ImportImage_16x16.png  5V   ,     $ ImportImage_32x32.png 5W   ,   *  $DevExpress.Xpf.Bars.BarItemSeparator  5X      5Z        5[      $ 地图��6*   . 5\      %  DevExpress.Xpf.Bars.BarEditItem  -   5          Name$ changelayer��5\   *   $ 选择图层:
 6@      	EditWidth$	 100X�6P      	EditValue 5]   *   ��  �� ElementName$ layerid��  �� Path$  DisplayMember�! �� Mode$! OneWay=�"   EditSettings" 5_      :  4DevExpress.Xpf.Editors.Settings.ComboBoxEditSettings  5`   "   -   5        #  Name$# layerid��5`   ;   <  6DevExpress.Xpf.Editors.Settings.LookUpEditSettingsBase$   ItemsSource#$ [  6L   8  2DevExpress.Xpf.Editors.Settings.ButtonEditSettings%   IsTextEditable$% FalseX�6q   &   DisplayMember$
& name��5a   <   '   ValueMember$' id��6Q   3  -DevExpress.Xpf.Core.DXFrameworkContentElement(   	IsEnabled( . 6b   5d        5e      $ 缩放地图:
 6*   $	 100X�6:    6J   �� $ 
mapcontrol��$
  Zoom�$! TwoWay=�" 5f      :  4DevExpress.Xpf.Editors.Settings.TrackBarEditSettings  5g   "   ;  5DevExpress.Xpf.Editors.Settings.RangeBaseEditSettings)   Maximum$) 24Q�6;   *   Minimum$* 10Q�6H   +   	LargeStep$+ 1Q�6U   ,   	SmallStep$	, 0.1Q�6c   -   TickFrequency$- 2Q�6s   5i        5j        5l      -   5        $
 move��5l   ,   .   RibbonStyle$. SmallWithText=�5m   ,   $ 移动地图
 6H   / � Cursor$/  ��5n   ,    5l   :     $ HandTool_16x16.png  6h     $ HandTool_32x32.png 5n   ,     5o      -   5        $ zoomin��5o   ,   $. SmallWithText=�5p   ,   $ 放大地图
 6H    5o   <     $ ZoomIn_16x16.png  6h     $ ZoomIn_32x32.png 5p   W     5q      -   5        $ zoomout��5q   ,   $. SmallWithText=�5r   ,   $ 缩小地图
 6H    5q   =     $ ZoomOut_16x16.png  6j     $ ZoomOut_32x32.png 5r   W   5t        5u      $ 选择��6*   . 5v        -   5        $ single��5v   ,   $ 单选
 5w   ,    5v   <     $ SelectTool_16x16.png  6l     $ SelectTool_32x32.png 5w   9     5x      -   5        $ rectsel��5x   ,   $ 框选
 6=    5y   ,     $ SelectAll2_16x16.png  5z   ,     $ SelectAll2_32x32.png 5{   ,     5|      -   5        $ idselect��5|   ,   $ 查询
 6>    5}   ,     $ Find_16x16.png  5~   ,     $ Find_32x32.png 5   ,     5�      -   5        $ cancel��5�   ,   $ 取消选择
 6<    5�   ,     $$ CloseHeaderAndFooter_32x32.png  5�   ,     $$ CloseHeaderAndFooter_32x32.png 5�   ,     5�      -   5        $ 
showdevice��5�   ,   $ 设备显示
 6A    5�   M     $ Show_16x16.png  5�   ,     $ Show_32x32.png 5�        5�      $ 检查工具��6*   . 5�        -   5        $ lackchk��5�   ,   $ 通道 缺失检查
 6=    5�   ,     $ None_16x16.png  5�   ,     $ None_32x32.png 5�   ,     5�      -   5        $ 
connectchk��5�   ,   $ 通道 连接检查
 6@    5�   ,     $ Hyperlink1_16x16.png  5�   ,     $ Hyperlink1_32x32.png 5�   ,   5�        5�      $ 计算工具��6*   . 5�        -   5        $ 
KapDisplay��5�   ,   $ 模拟计算
 6@    5�   ,     $" AddCalculatedField_16x16.png  5�   ,     $" AddCalculatedField_32x32.png 5�   ,   5�        5�      $ 	中心点��6*   . 5�        -   5        $ sethome��5�   ,   $ 设置 画图中心点
 6=    5�   ,     $ GeoPointMap_16x16.png  5�   ,     $ GeoPointMap_32x32.png   5�      -   5        $
 home��5�   ,   $ 回到 画图中心点
 6:    5�   ,     $ Home_16x16.png  5�   ,     $ Home_32x32.png 5�   ,   5�      5�        5�      $ 设备
 6!   . 5�        $ 添加管线��6*   . 5�        -   5        0 �Name$
0 line��5�   ,   $ 管线
 5�   ,   $@ :pack://application:,,,/NetControl;component/image/line.png��69   $@ :pack://application:,,,/NetControl;component/image/line.png��6|   5�   8   5�        5�      $ 通道节点��6*   . 5�        -    5        $	0 two��5�   ,   $ 两通
 5�   ,   $= 7pack://application:,,,/NetControl;component/image/2.png��69   $= 7pack://application:,,,/NetControl;component/image/2.png��6~   5�   7     5�      -!   5        $0 valve��5�   ,   $ 阀门
 5�   ,   $> 8pack://application:,,,/NetControl;component/image/fm.png��69   $> 8pack://application:,,,/NetControl;component/image/fm.png��6z   5�   9     5�      -"   5        $0 three��5�   ,   $ 三通
 5�   ,   $= 7pack://application:,,,/NetControl;component/image/3.png��69   $= 7pack://application:,,,/NetControl;component/image/3.png��6y   5�   9     5�      -#   5        $
0 four��5�   ,   $ 四通
 5�   ,   $= 7pack://application:,,,/NetControl;component/image/4.png��69   $= 7pack://application:,,,/NetControl;component/image/4.png��6y   5�   8     5�      -$   5        $
0 five��5�   ,   $ 五通
 5�   ,   $= 7pack://application:,,,/NetControl;component/image/5.png��69   $= 7pack://application:,,,/NetControl;component/image/5.png��6y   5�   8   5�        5�      $ 终端节点��6*   . 5�        -%   5        $0 
gasstation��5�   ,   $ 	供气站
 5�   ,   $@ :pack://application:,,,/NetControl;component/image/gsta.png��6:   $@ :pack://application:,,,/NetControl;component/image/gsta.png��6}   5�   >     5�      -&   5        $0 regulate��5�   ,   $ 	调压站
 5�   ,   $? 9pack://application:,,,/NetControl;component/image/res.png��6:   $? 9pack://application:,,,/NetControl;component/image/res.png��6|   5�   <     5�      -'   5        $
0 user��5�   ,   $ 终端用户
 5�   ,   $@ :pack://application:,,,/NetControl;component/image/user.png��6;   $@ :pack://application:,,,/NetControl;component/image/user.png��6~   5�   8     5�      -(   5        $0 scada��5�   ,   $ scada测点
 5�   ,   $A ;pack://application:,,,/NetControl;component/image/scada.png��6>   $A ;pack://application:,,,/NetControl;component/image/scada.png��6�   5�   9     5�      -)   5        $	0 pel��5�   ,   $ 燃气
 5�   ,   $? 9pack://application:,,,/NetControl;component/image/pel.png��69   $? 9pack://application:,,,/NetControl;component/image/pel.png��6{   5�   7   5�      5�      5�      5�      � 5�   
   -*   5        1 �Name$1 showgrid��5�      $��1��6!   .+�5�      .  (DevExpress.Xpf.Docking.DockLayoutManager  -+   5        2  Name$2 dockLayoutManager��5�   %   4  .DevExpress.Xpf.Core.Serialization.DXSerializer3   SerializationID$3 dockManagerID��6@   *   $DevExpress.Xpf.Core.WorkspaceManager4    	IsEnabled4 . ��5�   %   �� $ 
mainwindow��$  ActualWidth�$! OneWay=���5�   %   �� $ showgrid��$  ActualHeight�$! OneWay=�5   
LayoutRoot.5 5�      (!  "DevExpress.Xpf.Docking.LayoutGroup!  6 !  Orientation$6 Vertical=�6#   +"  %DevExpress.Xpf.Docking.BaseLayoutItem7 "  Margin$7 0q�6:   $��Stretch=�6E   $��Stretch=�6c   8 !  Items.8 5�      !  -,   5        $
��root��5�   '   $6 
Horizontal=�63   $7 0q�6L   9 "  	ItemWidth9 6W   �� $ 
mainwindow��$  ActualWidth�$! OneWay=�.8 5�      !  $	��600��6+   $��50��6:   $6 Vertical=�6H   $	9 200 �6_   $7 0q�6o   $
��Left=�6z   .8 5�      (#  "DevExpress.Xpf.Docking.LayoutPanel#  --   5        : # Name$: deprop��5�   /   ; "  Caption$; 设备属性编辑��6?   $y�	Collapsed=�6P   < "  ShowCloseButton< .  $7 0q�6   ($  "DevExpress.Xpf.Docking.ContentItem= $  Content.= 5�   "   *%  $DevExpress.Xpf.Core.ScrollViewerBase%  $
��Auto=�66   .��5�   &    � -.   5        >  �Name$> deframe��5�   ,   ?  � NavigationUIVisibility$? Hidden=�6=   $
��Left=�6]   $	��Top=�6   $
��Left=�5�   ,   $	��Top=�6G   6a   5�   #   5�      #  5�      -/   5        $: lineprop��5�   /   $; 管线属性编辑��6A   $y�	Collapsed=�6R   < .  $7 0q�6�   .= 5�   "   %  $
��Auto=�66   .��5�   &    � -0   5        $> 	lineframe��5�   ,   $? Hidden=�6?   $
��Left=�6_   $	��Top=�6�   $
��Left=�5�   ,   $	��Top=�6G   6a   5�   #   5�      #  5�      -1   5        $: 
checkpanel��5�   /   $; 拓扑错误列表��6C   $y�	Collapsed=�6T   < .  $7 0q�6�   .= 5�   "   %  $
��Auto=�66   .��5�   &    � -2   5        $> warmingframe��5�   ,   $
��Left=�6B   $	��Top=�6]   6w   5�   #   5�      #  5�      -3   5        $: linepiplistprop��5�   /   $; 选中设备管线��6H   $y�	Collapsed=�6Y   $	9 200 �6q   < .  $7 0q�6�   .= 5�   "   %  $
��Auto=�66   .��5�   &    � -4   5        $> linepipframe��5�   ,   $
��Left=�6B   $	��Top=�6]   6w   5�   #   5�      5�      #  5�      -5   5        $
: Gmap��5�   +   $; 地图��69   $9 7* �6F   < .  $7 0q�6m   .= 5�      � -6   5        $��Gmapgrid��5�   #   }�. .+�5�   "   &   NetControl.Gmap&  -7   5        @ & Name$@ 
mapcontrol��5�   -   '   NetControl.GmapA '  	MouseMove$A OnCanvasMouseMove��5�   )   B '  	MouseDown$B Mapcontrol_MouseDown��6G   $��0,0,0,0q�5�   A   c _GMap.NET.WindowsPresentation, Version=1.7.5.0, Culture=neutral, PublicKeyToken=b85b9027b614afef.(  (GMap.NET.WindowsPresentation.GMapControlC (  MouseWheelZoomType$C MousePositionAndCenter=�5�   h   ��5�   R   �� $ 
mainwindow��$  ActualWidth�$! OneWay=���5�   -   �� $ 
mainwindow��$  ActualHeight�$! OneWay=�5�   h   �� 5�   "   -8   5        D ��Name$D imageContainer��5�   (   E J� IsHitTestVisibleE .  z�5�   &   V� 5�   *   .~�5�   .   �� 5�   +   5�   #   )  NetControl.Draw)  5�   "   -9   5        F ) Name$F layer��5�   -   $��0,0,0,0q�6<   $
��Left=�5   s   ��5�   M   �� $ 
mainwindow��$  ActualHeight�$! OneWay=���5   -   �� $ 
mainwindow��$  ActualWidth�$! OneWay=�z�5  &   V� 5  *   .~�5  .   �� 5  +   5  #   �� 5  "   -:   5        G ��Name$G scanvas��5  )   $��0q�6:   H K� ZIndex$H 4��6]   ��6n   �� $ 
mainwindow��$  ActualWidth�$! OneWay=���5  )   �� $ 
mainwindow��$  ActualHeight�$! OneWay=�z�5	  &   V� 5
  *   .~�5  .   �� 5  +   .+�5  &   � -;   5        $1 scale��5  +   $
��Left=�6:   $	��Top=�6U   $��15��6m   $	��100��6z   
I �� Top$I 50��6�   J �� Left$J 50��6�   $y�Visible=�6�   $H 2��6�   $��1��6�   $��1��6�   .+�5  *   �� -<   5        K ��Name$K 
scale_text��5  4   $��Center=�6H   $	��Top=�6e   $��10公里��6}   $��10(�6�   6�   F� 5  *   -=   5        L F�Name$L 
scale_line��5  /   $��Bottom=�6C   $
��Left=�6^   +����        �    @B �@B @B �@B     	
���   �M � Cursor$
M None��6�   N �� Stretch$
N Fill=�6�   $��1��6�   $��0,5,0,0q�6�   5  '   5  #   �� 5  "   ->   5        $G drawrectangle��5  )   $��0q�6@   $��200000000000��6K   $��200000000000��5  )   z�5  &   V� 5  *   .~�5  .   �� 5  +   5  #   �� 5   "   -?   5        $G 	linelayer��5   )   $��0,0,0,0q�6<   ��6M   �� $ 
mainwindow��$  ActualHeight�$! OneWay=���5!  )   �� $ 
mainwindow��$  ActualWidth�$! OneWay=�z�5"  &   V� 5#  *   .~�5$  .   �� 5%  +   5'  #   �� 5)  "   -@   5        $G 	marklayer��5)  )   $��0,0,0,0q�6<   ��6M   �� $ 
mainwindow��$  ActualHeight�$! OneWay=���5*  )   �� $ 
mainwindow��$  ActualWidth�$! OneWay=�z�5+  &   V� 5,  *   .~�5-  .   �� 5.  +   50  #   �� 52  "   -A   5        $G 	selectcav��52  )   $��0q�53  o   ��52  <   �� $ 
mainwindow��$  ActualHeight�$! OneWay=���53  )   �� $ 
mainwindow��$  ActualWidth�$! OneWay=�.+�54  &   �� -B   5        O ��Name$O Seletedrect��54  0   $��5��6E   $��24��6Y   $��24��6d   
����+��$y�	Collapsed=�6�   z�55  *   V� 56  .   .~�57  2   �� 58  /   5:  '   z�5;  &   V� 5<  *   .~�5=  .   �� 5>  +   5@  #   �� 5B  "   -C   5        $G 
newselectr��5B  4   $��0q�6)   $
��Left=�5C  o   ��5B  H   �� $ 
mainwindow��$  ActualHeight�$! OneWay=���5C  )   �� $ 
mainwindow��$  ActualWidth�$! OneWay=�.+�5D  &   �� -D   5        $O alldrect��5D  0   $��5��6B   $��24��6V   $��24��6a   
����� �$y�	Collapsed=�6�   z�5E  *   V� 5F  .   .~�5G  2   �� 5H  /   5J  '   z�5K  &   V� 5L  *   .~�5M  .   �� 5N  +   5P  #   � 5R  "   .+�5S  &   *  NetControl.GdiLayer*  -E   5        P * Name$P bitmap��5S  5   $��0,0,0,0q�6E   $
��1200��6V   $
��1200��6d   $
��Left=�6q   6�   5T  #   (� 5W  "   $��Right=�6*   $	��Top=�6F   $	��130��6^   $	��200��6j   .��5X  &   +   NetControl.LegendControl+  -F   5        Q + Name$Q sympton��5X  :     UserControlControlTemplate1#���  6K   R +  Legend_mode$R 2��6�   S +  Legend_Maximum$	S 300��6�   T +  Legend_Minimum$T 0��5Y  :   $
��Left=�6M   $	��Top=�6h   $
��auto��6�   $
��auto��6�   $y�	Collapsed=�5Z  :   6S   5[  #   5]     5_     !  5a     $6 Vertical=�6+   $	��600��6B   $��50��6Q   $	9 200 �6_   $7 0q�6o   $��Right=�6z   .8 5b     #  -G   5        $��	listtable��5b  /   $; 场景列表��6@   < .  $	��600��6g   $7 0q�6v   .= 5c  "   &,   System.Windows.Controls.DataGrid,  -H   5        U , Name$U sence��5c  ^   $
��Left=�6+   $	��Top=�6F   V ,  AutoGenerateColumnsV .  W ,  CanUserAddRowsW .  X ,  RowHeaderWidth$X 0Q�6�   $	��280��6�   Y ,  RowStyleY 5d  &   �� 5e  *   Z �� 
TargetType)-  #System.Windows.Controls.DataGridRow"Z - 60   .�5f  .   �� [ �� Property[ � ��\ �� Value
\ �����6K   ] �� Triggers] 5g  .   P� 5h  2   ^ P� Property^ �@J�IsMouseOver_ P� Value_ . . �5i  6   �� [ � ��
\ �����6S   5j  3   P� 5k  2   ^ �@- 
IsSelected_ . . �5l  6   �� [ � ��
\ �����6S   �� 5m  6   [ � ��
\ �  ��6S   5n  3   5p  +   ` ,  Columns` 5r  &   4.  .System.Windows.Controls.DataGridTemplateColumn.  5s  *   ,/  &System.Windows.Controls.DataGridColumna /  Header$a 场景��6A   b /  Width50  /System.Windows.Controls.DataGridLengthConverter$b 700 6M   c /  MaxWidth$	c 100Q�6X   d .  CellTemplated 5t  .   �� 5u  2   .R�5v  6   �� $��WordEllipsis=�6V   ��6@   �� *Name+6V   5w  3   e /  	CellStylee 5y  .   �� 5z  2   .�5{  6   �� [ �@^�ToolTip\ 6_   �� *Name+5|  3   5~  +   .  5  *   $a 结果文件��6A   $	b 1100 6O   d 5�  .   �� 5�  2   .R�5�  6   �� $��WordEllipsis=�6X   ��6@   �� *Result+6X   5�  3   e 5�  .   �� 5�  2   .�5�  6   �� [ �@^�ToolTip\ 6_   �� *Result+5�  3   5�  +   .  5�  *   $a 文件状态��6A   $	b 1000 6O   d 5�  .   �� 5�  2   .R�5�  6   �� $��WordEllipsis=�6X   ��6@   �� *Modify+6X   5�  3   e 5�  .   �� 5�  2   .�5�  6   �� [ �@^�ToolTip\ 6_   �� *Modify+5�  3   5�  +   5�  #   5�     #  5�     -I   5        $��options��5�  /   $; 模拟展示��6>   < .  $7 0q�6e   .= 5�  "   � .+�5�  &   � -J   5        $��modepage��5�  +    5�  *   �� 5�  .   $	��82* �6<   �� 5�  .   $
��133* �6<   .+�5�  *   � .+�5�  .   	� -K   5        f 	�Name$f pressure��5�  :   $��	23,13,0,0q�6L   $��压强��6_   6�   	� 5�  .   -L   5        $f temperature��5�  :   $��	23,50,0,0q�6O   $��温度��6b   6�   5�  +   � 5�  *   $��1��6/   .+�5�  .   ,  -M   5        $U 	calculate��5�  j   $
��Left=�67   $	��Top=�6R   V .  W .  $X 0Q�6�   $	��400��6�   ` 5�  2   01  *System.Windows.Controls.DataGridTextColumn1  5�  6   $a 编号��6I   $	b 1000 6l   12  +System.Windows.Controls.DataGridBoundColumng 2  Bindingg 6U   �� *ID+5�  7   1  5�  6   $a 名称��6I   $	b 1000 6n   g 6U   �� *Name+5�  7   1  5�  6   $a 温度��6I   $	b 1000 6n   g 6U   �� *Temp+5�  7   1  5�  6   $a 压强��6I   $	b 1000 6l   g 6U   �� *Pa+5�  7   5�  /   5�  +   5�  '   � 5�  &   -N   5        $��	chartpage��5�  +   $y�	Collapsed=�6<   .+�5�  *   ,  -O   5        $U 	simuchart��5�  f   $
��Left=�63   $	��Top=�6N   V .  W .  $X 0Q�6�   $	��400��6�   ` 5�  .   1  5�  2   $a 时间��6E   $	b 1000 6j   g 6Q   �� *Time+5�  3   1  5�  2   $a 输入数值��6E   $	b 1000 6S   g 6_   �� *Count+$! TwoWay=�6�   1  5�  2   $a 模拟数值��6E   $	b 1000 6w   g 6S   �� *Num+$! TwoWay=�5�  3   5�  +   5�  '   5�  #   5�     5�     5�     (3  "DevExpress.Xpf.Docking.TabbedGroup3  5�     -P   5        $��
chartGroup��5�  O   h "  
ItemHeight$	h 300 �6'   $y�	Collapsed=�68   .8 5�     #  -Q   5        $��LayoutPa��5�  l   $; 压强图(Pa)��6+   i #  ShowHideButtoni .  < .  $7 0q�6|   .= 5�     � j � ColumnDefinitionsj 5�  "   �� 5�  &   �� 5�  &   .+�5�  "   (4  "DevExpress.Xpf.Charts.ChartControl4  ,5  &DevExpress.Xpf.Charts.ChartControlBasek 5  Titlesk 5�  &   !6  DevExpress.Xpf.Charts.Title6  5�  *   %7  DevExpress.Xpf.Charts.TitleBasel 7  Content$l 压强柱状图��64   6F   m 4  Diagramm 5�  &   '8  !DevExpress.Xpf.Charts.XYDiagram2D8  5�  *   -R   5        n 8 Name$n diagram1��5�  a   (o 8  !LabelsResolveOverlappingMinIndent$o 2��6;   p 8  EnableAxisXNavigationp . q 8  DefaultPaneq 5�  .    9  DevExpress.Xpf.Charts.Pane9  5�  2   r 8  AxisYr 5�  .   #:  DevExpress.Xpf.Charts.AxisY2D:  5�  2   $;  DevExpress.Xpf.Charts.AxisBases ;  GridLinesMinorVisibles .   <  DevExpress.Xpf.Charts.Axist <  
WholeRanget 5�  6   !=  DevExpress.Xpf.Charts.Range=  5�  :   u :  AlwaysShowZeroLevelu .  6D   v <  Titlev 5�  6   %>  DevExpress.Xpf.Charts.AxisTitle>  5�  :   $l 
压强(Pa)��6H   5�  3   w 8  SecondaryAxesYw 5�  .   x 8  AxisXx 5�  .   #?  DevExpress.Xpf.Charts.AxisX2D?  5�  2   y ;  GridLinesVisibley .  6>   #@  DevExpress.Xpf.Charts.Diagramz @  Series.z 5�  .   (A  "DevExpress.Xpf.Charts.LineSeries2DA  { A  MarkerVisible{ . "B  DevExpress.Xpf.Charts.Series| B  LabelsVisibility| . } B  ArgumentDataMember$	} Key��6l   ~ B  ValueDataMember$~ Value��6�    A  	LineStyle 5�  2   %C  DevExpress.Xpf.Charts.LineStyleC  5�  6   � C  	Thickness$� 1��6D   5�  /   5�  +   5�  #   4  5�  "   $
��Left=�63   $��0,0,0,0q�6N   $	��Top=�6_   $��1��6w   k 5�  &   6  5�  *   $l 压强折线图��64   6F   .m 5�  &   8  -S   5        $n diagram3��5�  ]   $o 2��67   p . q 5�  *   9  5�  .   r 5�  *   :  5�  .   s .  t 5   2   =  5  6   u .  6@   v 5  2   >  5  6   $l 
压强(Pa)��6D   5  /   w 5  *   x 5
  *   ?  5  .   y .  6:   .z 5  *   A  { . | . $	} Key��6h   $~ Value��6�    5  .   C  5  2   $� 1��6@   5  +   5  '   � 4  Legend� 5  &   "D  DevExpress.Xpf.Charts.LegendD  5  *   � D  HorizontalPosition$� Right=�65   � 4  ToolTipController� 5  &   2E  ,DevExpress.Xpf.Charts.ChartToolTipControllerE  5  *   � E  AutoPopDelay$� 0h�6E   � E  InitialDelay$� 0h�6V   5  #   5     5     #  5     -T   5        $��
LayoutTemp��5  k   $; 温度图(℃)��6+   i .  < .  $7 0q�6}   .= 5     � j 5  "   �� 5  &   �� 5   &   .+�5"  "   4  $��0��63   k 5#  &   6  5$  *   $l 温度柱状图��64   6F   m 5&  &   8  5'  *   -U   5        $n diagram2��5'  a   $o 2��6;   p . q 5(  .   9  5)  2   -V   5        $
��pane��5)  ;   r 5+  .   :  5,  2   s .  t 5-  6   =  5.  :   u .  6D   v 50  6   >  51  :   $l 温度(℃)��6H   53  3   w 55  .   x 57  .   ?  58  2   y .  6>   .z 5:  .   A  { . | . $	} Key��6l   $~ Value��6�    5;  2   C  5<  6   $� 1��6D   5>  /   5?  +   5A  #   4  5E  "   $
��Left=�63   $��0,0,0,0q�6N   $	��Top=�6_   $��1��6w   k 5F  &   6  5G  *   $l 温度折线图��64   6F   .m 5I  &   8  -W   5        $n diagram4��5I  ]   $o 2��67   p . q 5J  *   9  5K  .   r 5M  *   :  5N  .   s .  t 5O  2   =  5P  6   u .  6@   v 5R  2   >  5S  6   $l 温度(℃)��6D   5U  /   w 5W  *   x 5Y  *   ?  5Z  .   y .  6:   .z 5\  *   A  { . | . $	} Key��6h   $~ Value��6�    5]  .   C  5^  2   $� 1��6@   5`  +   5a  '   � 5b  &   D  5c  *   $� Right=�65   � 5e  &   E  5f  *   $� 0h�6E   $� 0h�6V   5h  #   5i     5j     5k     5n     5p     5r     2F  ,DevExpress.Xpf.Ribbon.RibbonStatusBarControlF  5t  
   $��2��6%   � F  	LeftItems� 5u     'G  !DevExpress.Xpf.Bars.BarStaticItemG  5v     � G  
ShowBorder� .   5w  $   �� $ 
mapcontrol��$  Mousemovepoint�� �� 	Converter#� [ G  5x     � .   6$   �� $ 
mainwindow��$  	directive�#� [ �   	IsVisible� 5y  $   �� $ 
mainwindow��$  showcomd�$! TwoWay=�5z  $   G  5{     � .   5|  $   �� $ 
mapcontrol��$
  Zoom�#� [ 5~     �� 5�  
   -X   5        $
G load��5�     $��0��6   $��3��6,   $y�	Collapsed=�6=   
������ .+�5�     H   NetControl.LoadingControlH  $��Center=�6$   $��Center=�6A   $��430,300q�6\   5�     5�     5�     