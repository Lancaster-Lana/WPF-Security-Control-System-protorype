   M S B A M L   `   `   `  ����U  QIDenticard.AccessUI, Version=1.0.4346.30944, Culture=neutral, PublicKeyToken=nullV>clr-namespace:IDenticard.AccessUI;assembly=IDenticard.AccessUIIDenticard.AccessUI  5   +   S OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:WPFSecurityControlSystem.BaseWPFSecurityControlSystem.Base 5      U/clr-namespace:WPFSecurityControlSystem.Controls!WPFSecurityControlSystem.Controls 5      <   6WPFSecurityControlSystem.Controls.SIOPropertiesControl   5      Hmodel>clr-namespace:IDenticard.AccessUI;assembly=IDenticard.AccessUI  5   +   4base+clr-namespace:WPFSecurityControlSystem.Base  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @ctrl/clr-namespace:WPFSecurityControlSystem.Controls     5      .��5      � $	��350��6   .+�5   
   �� -   5        $��tabCtrlIOBoard��5      $��Stretch=�6,   $��0q�6J   $��Stretch=�6U   .@�5	      �� -   5        $��tabIOProperties��5	   )   $��	I/O Board��6   .��5
      � $��2q�6     � ColumnDefinitions  5      �� 5       $
��Auto �6+   �� 5!      $
��Auto �6+    � RowDefinitions 5#      �� 5$      $��35 �6(   �� 5%      $��35 �6(   �� 5&      $	��118 �6(   �� 5'      $��34 �6(   �� 5(      $
��120* �6(   .+�5+      �� -   5        $��lblName��5+      $��Name:��6+   $��28��6;   $
��Left=�6G   $��10,6,0,0q�6b   $	��Top=�6t   �� 5,      -   5        $��txtName��5,      -  'System.Windows.Input.KeyboardNavigation   TabIndex$ 0��6-   $
��Left=�6N   $��	115,8,0,0q�6i   $	��Top=�6|   $	��197��6�   �� 5-      -   5        $��
ckbEnabled��5-      $��
115,13,0,7q�62   $��1��6F   $��Enable Board��6S   $ 1��6j   $
��Left=�6�   $��Center=�6�    d� 	IsChecked 6�   �� *IsEnableIOBoard+6�   �� 5.      -   5        $��lblDescription��5.      $��Description:��62   $��28��6I   $
��Left=�6U   $��10,0,0,0q�6p   $	��Top=�6�   $��2��6�   �� 5/      -   5        $��txtDescription��5/      $ 2��64   $	��100��6T   $
��Left=�6a   $��	115,5,5,0q�6|   $	��Top=�6�   $	��230��6�   $��2��6�   �� 50      -   5        $��	lblIOType��50      $��	I/O Type:��6-   $��28��6A   $
��Left=�6M   $��10,6,0,0q�6h   $	��Top=�6z   $��3��6�   �� 51      -	   5        $��	cmbIOType��51      $ 3��60   $��	115,6,0,0q�6P   $	��Top=�6c   $	��197��6{   $��3��6�   $
��Left=�6�   52      &   System.Windows.Controls.DataGrid  53      -
   5        $��
gridCounts��53      $ 4��61   $��4��6Q   $��	115,6,0,0q�6^   $
��Auto��6q   $
��Auto��6~      GridLinesVisibility$
 None=�6�      CanUserAddRows .     CanUserDeleteRows .  {�.  
������    AutoGenerateColumns .  $
��Left=�55   ;   $	��Top=�6V      Columns 56      0  *System.Windows.Controls.DataGridTextColumn  57      -   5        	colInputs��,  &System.Windows.Controls.DataGridColumn	   Header$	 Inputs��57   d   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 70 6t   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6D   �� *NumOfInputs+6t     58      -   5        
colOutputs��$	 Outputs��58   f   $
 70 6w    6E   �� *NumOfOutputs+6w     59      -   5        
colReaders��$	 Readers��59   g   $
 70 6x    6E   �� *NumOfReaders+6x   5?      �� 5A      -   5        $��grbIOProperties��5A      $��3��66   $��1��6G   $	��180��6W   $��Right=�6d   $��0q�6�   $	��Top=�6�   $	��350��6�   .��5B      � $	��180��6     5C      �� 5D   "   $	��100 �63   �� 5E   "   $	��100 �63   �� 5F   "   $
��Auto �63   �� 5G   "   $
��120* �63   .+�5I      �� -   5        $��
lblMSPport��5I   $   $��	MSP port:��66   $��28��6J   $
��Left=�6V   $��10,5,0,0q�6q   $	��Top=�6�   �� 5J      -   5        $��
cmbMSPPort��5J   '   $��0,6,0,0q�69   $ 5��6J   $
��Left=�6j   $	��Top=�6�   $��90��6�   $��1��6�   .@�5L   "   �� $��1��6/    �� 
IsSelected . 6;   �� 5M   "   $��2��6/   5N      �� 5O      -   5        $��lblChannelIn��5O   $   $��Channel in:��68   $��28��6N   $��	10,35,0,0q�6Z   $
��Left=�6m   $	��Top=�6�   �� 5P      -   5        $��cmbChannelIn��5P   '   $ 6��6;   $��1��6[   $��0,35,0,0q�6k   $
��Left=�6}   $	��Top=�6�   $��90��6�   �� 5Q      -   5        $��lblChannelOut��5Q   %   $��Channel out:��6:   $��28��6Q   $
��Left=�6]   $��	10,62,0,0q�6x   $	��Top=�6�   �� 5R      -   5        $��cmbChannelOut��5R   '   $ 7��6<   $��0,62,0,0q�6\   $	��Top=�6n   $��1��6�   $
��Left=�6�   $��90��6�   �� 5S      -   5        $��
lblAddress��5S   $   $��Address:��66   $��28��6I   $��	10,90,0,0q�6U   $
��Left=�6h   $	��Top=�6�   �� 5T      -   5        $��cmbIPhysicalAddress��5T   '   $ 8��6B   $��1��6b   $��0,90,0,0q�6t   $
��Left=�6�   $	��Top=�6�   $��90��6�   �� 5U      -   5        $��lblRetryCount��5U   $   $��Retry Count:��69   $��28��6P   $
��Left=�6\   $��
10,119,0,0q�6w   $	��Top=�6�   �� 5V      -   5        $��txtRetryCount��5V   &   $��1��6;   $ 9��6K   $
��Left=�6k   $��	0,119,0,0q�6�   $	��Top=�6�   $��70��6�   �� 5W      -   5        $��ckbReverseIOOrder��5W   '   $ 10��6@   $��Reverse I/O order��6a   $��2��6}   $��15,152,0,15q�6�   $
��Left=�6�   $��Center=�6�    6�   �� *IsReverseIOOrder+6�   5X      5Y      �� 5[      -   5        $��grpNextIOBoard��5[       $��3��66   $��1��6C   $��Next I/O��6S   $	��125��6e   $
��Left=�6r   $	��Top=�6�   $	��350��6�   $��2��6�   .��5\      � $��2q�6   $��97��6*     5]      �� 5^   "   $	��100 �63   �� 5_   "   $	��100 �63   �� 5`   "   $
��Auto �63   �� 5a   "   $��4* �63   .+�5c      �� -   5        $��lblChannelNextIn��5c   $   $��Input:��6<   $
��Left=�6N   $��10,8,0,0q�6i   $	��Top=�6{   �� 5d      -   5        $��cmbChannelNextIn��5d   '   $��1��6@   $ 11��6P   $
��Left=�6q   $��0,6,0,0q�6�   $	��Top=�6�   $��90��6�   �� 5e      -   5        $��lblChannelNextOut��5e   %   $��Output:��6>   $
��Left=�6Q   $��	10,36,0,0q�6l   $	��Top=�6   �� 5f      -   5        $��cmbChannelNextOut��5f   '   $��1��6A   $ 12��6Q   $
��Left=�6s   $��0,35,0,0q�6�   $	��Top=�6�   $��90��6�   �� 5g      -   5        $��lblNextReader��5g   $   $��Reader:��6;   $��28��6M   $
��Left=�6Y   $��10,0,0,6q�6t   $��Bottom=�6�   �� 5h      -    5        $��cmbNextReader��5h   '   $��1��6<   $ 13��6L   $
��Left=�6m   $��0,64,0,0q�6�   $	��Top=�6�   $��90��6�   �� 5i      -!   5        $��lblWarningNextIO��5i   $   $	��Top=�6<   $��2��6T   
���  �� � ForceCursor . .��5j   "   ��  �� TextWrapping$
 Wrap=�6.   $	��120��6B   $��65��6N   .��6Z   NLThis board cannot be selected in the Next 'I/O' settings of any other board!6�   5k      5l      5m      5n      5o      �� 5p      -"   5        $��tabPermissions��5p   +   $��Permissions��6   $��Stretch=�6A   $��Stretch=�6]   .��5q      :  4WPFSecurityControlSystem.Controls.PermissionsControl  -#   5          Name$ ctrlPermissions��5q   *   $��0q�6C   5r      5s      5t      5u      