   M S B A M L   `   `   `  ����E  ANetControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:NetControl
NetControl  5   	       NetControl.Newcreat   5   	   a ]DevExpress.Xpf.Core.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:NetControl     5   	   -   5          � Title$  新建地图��5   	   $��182.4��6   $��333.561��6%    � WindowStartupLocation$ CenterOwner=�5	   	    � UseLayoutRounding .   � 
ResizeMode$ NoResize=�6G   .��5
      � $	��242��6   $	��Top=�6    � RowDefinitions 5   
   �� 5      $	��13* �6   �� 5      $	��13* �6   �� 5      $	��95* �6   .+�5   
   � .+�5      �� -   5         ��Name$ center��5      $��选择中心点：��6$   $��70��65   $
��Left=�6@   $��11,0,0,0q�6[   6o   �� 5      -   5         ��Name$ 
selectcent��5      $	��100��6+   $��23��67   $	��Top=�6C   $
��Left=�6[   $��90,0,0,0q�6v    �� DisplayMemberPath$
 Name��5       �� SelectedValuePath$ Id��60   6x   5      � 5   
   $��1��6   .+�5      �� -   5        $ layer��5      $��选 择 图 层：��6#   $��70��66   $
��Left=�6A   $��11,0,0,0q�6\   6p   �� 5      -   5        $ selectlayer��5      $	��100��6,   $��23��68   $	��Top=�6D   $
��Left=�6\   $��90,0,0,0q�6w   $
 Name��5      $	 Tag��60   6z   5      � 5   
   $��2��6   .+�5      �� -   5        $��50��5      $��32��6    $��确定��6,   $��Center=�69   $	��Top=�6V   6n   5      5      5      