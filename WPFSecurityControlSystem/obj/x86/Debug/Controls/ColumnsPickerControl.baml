   M S B A M L   `   `   `  ����S  OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:WPFSecurityControlSystem.BaseWPFSecurityControlSystem.Base  5      <    6WPFSecurityControlSystem.Controls.ColumnsPickerControl   5      4base+clr-namespace:WPFSecurityControlSystem.Base  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      $	��300��6K   .��5      � $��2q�6     � ColumnDefinitions  5   
   �� 5	      $	��250 �6   �� 5
      $��50 �6   �� 5      $	��250 �6   �� 5      $��50 �6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��* �6   .+�5   
   �� -   5        $��btnAdd��5      $��>��6   $��1��6.   $��1��6>   $��23��6K   $
��Left=�6W   $��	10,57,0,0q�6r   $	��Top=�6�   $��29��6�   6�   �� 5   
   -   5        $��	btnAddAll��5      $��>>��6"   $��1��65   $��1��6E   $��23��6R   $
��Left=�6^   $��	10,87,0,0q�6y   $	��Top=�6�   $��29��6�   6�   �� 5   
   -   5        $��btnRemoveAll��5      $��<<��6%   $��1��68   $��1��6H   $��23��6U   $
��Left=�6a   $��
10,116,0,0q�6|   $	��Top=�6�   $��28��6�   6�   �� 5   
   -   5        $��	btnRemove��5      $��<��6"   $��23��61   $
��Left=�6=   $��
10,145,0,0q�6X   $	��Top=�6l   $��29��6�   $��1��6�   $��1��6�   6�   �� 5   
   -   5        $��label1��5   Z   $��Source��6   $��28��6!   $
��Left=�6-   $��12,3,0,0q�6H   $	��Top=�6h   $
��Bold"�6�   $��2��6�   �� 5   
   -   5        $��	lstSource��5      $��3q�6$   $��0��60   $��1��6@    �� SelectionMode$ Multiple=�5       �� DisplayMemberPath$
 Name��5       �� SelectedValuePath$ ID��6,   $��Stretch=�5      $��Stretch=�6.   ��5      �� *
SourceList+ �� Mode$ OneWay=�5      �� 5   
   -   5        $��label2��5   j   $��Target��6   $��2��6!   $��28��61   $
��Left=�6=   $��11,1,0,0q�6X   $	��Top=�6x   $
��Bold"�6�   �� 5    
   -   5        $��	lstTarget��5       $��3q�6$   $��2��6/   $��1��6?   $ Single=�5!      $
 Name��5#      $ ID��6+   $��Stretch=�5$      $��Stretch=�6.   ��5"      �� *
TargetList+$ OneWay=�5$   .   �� 5&   
   -	   5        $��
btnMoveTop��5&      $��^��6#   $��23��6/   $
��Left=�6;   $��	14,18,0,0q�6V   $	��Top=�6i   $��25��6�   $��3��6�   $��1��6�   6�   �� 5'   
   -
   5        $��btnMoveDown��5'      $��v��6$   $��1��60   $��23��6=   $
��Left=�6I   $��	14,48,0,0q�6d   $	��Top=�6w   $��25��6�   $��3��6�   6�   5)      5*      