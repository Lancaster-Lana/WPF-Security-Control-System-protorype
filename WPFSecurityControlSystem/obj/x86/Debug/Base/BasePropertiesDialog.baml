   M S B A M L   `   `   `  ����S  OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:WPFSecurityControlSystem.BaseWPFSecurityControlSystem.Base  5   	   8    2WPFSecurityControlSystem.Base.BasePropertiesDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <base+clr-namespace:WPFSecurityControlSystem.Base     5   	     � SizeToContent$  WidthAndHeight=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ BasePropertiesDialog��6     J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5	   
   -   5           Command$
 Save��5	      6(     5
   
   -   5        $ Close��5
      6)   .��5   
   �  � RowDefinitions 5   
   �� 5      $
��300* �6   6,   �� 5      $��45 �6    � ColumnDefinitions 5   
   �� 5      $
��100* �6   .+�5+   
   �� -   5         ��Name$ viewPropertiesControl��5+      �� 50   
   $��3q�6   $��1��6    $��Stretch=�6-   $��Stretch=�6I   
�������.+�51      �� $��1q�6    �� Orientation$ 
Horizontal=�6$   $��Center=�6=   $��Right=�6X   .+�52      �� -   5        $��btnSave��52      $��0,5,5,0q�6(   $��OK��69   $��75��6F   $��25��6Q   $
��Save��6]   �� 53      -   5        $��	btnCancel��53      $��0,5,10,0q�6*   $��Cancel��6<   $��75��6M   $��25��6X   $��Close��6e   54      55      56      57      