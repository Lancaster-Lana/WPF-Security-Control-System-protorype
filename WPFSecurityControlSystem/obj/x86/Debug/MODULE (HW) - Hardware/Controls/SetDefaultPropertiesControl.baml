   M S B A M L   `   `   `  ����S  OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:WPFSecurityControlSystem.BaseWPFSecurityControlSystem.Base  5      L HIDenticard, Version=1.0.4339.20281, Culture=neutral, PublicKeyToken=nullM5clr-namespace:IDenticard.Premisys;assembly=IDenticardIDenticard.Premisys 5      C    =WPFSecurityControlSystem.Controls.SetDefaultPropertiesControl   5      4base+clr-namespace:WPFSecurityControlSystem.Base  5      ?model5clr-namespace:IDenticard.Premisys;assembly=IDenticard  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      $	��450��5   a   .��5      � $��2q�6     � ColumnDefinitions  5   
   �� 5	      $	��380 �6   �� 5
      $	��360 �6    � RowDefinitions 5   
   �� 5      $��45 �6   �� 5      $��30 �6   �� 5      $��70 �6   �� 5      $	��200 �6   �� 5      $��* �6    � 	Resources 5   
      templateCardFormats��%!   &	        �� 5      5      5      .+�5   
   �� $��2��6   $
��Wrap=�6(   $��	20,10,5,5q�6<   $��18(�6O   $
��Bold"�6^   $P��JNo sites have been configured yet. Set defaults to continue configuration.��6p   �� 5   
   $��2��6   $��1��6(   $
��Wrap=�66   $��20,5,5,5q�6J   $f��`You are still able to edit the settings of any component after it is added to the hardware tree.��6\   �� 5   
   -   5        $��
grpChannel��5      $��2��6&   $��Channel��63   $��0q�6E   $��Stretch=�6P   $��Stretch=�6n   .��5      � .+�5      �� -   5        $��lblCommType��5      $��
Comm Type:��6+   $
��Left=�6@   $��10,0,0,0q�6[   $��Center=�6m   �� 5      -   5        $��cmbCommType��5      -  'System.Windows.Input.KeyboardNavigation   TabIndex$ 0��6.   $
��Left=�6O   $��	138,0,0,0q�6j   $��Center=�6}   $	��144��6�   .@�5      �� $��Network��6#    � ToolTip$+ %Communication to host is via Ethernet��65   �� 5      $��Serial��6#   $L FCommunication to host is via RS-232 or via RS-232-to-RS-485 conversion��64   5       5!      5"      �� 5#   
   -   5        $��grpController��5#      $��3��6(   $��
Controller��65   $��Stretch=�5$      $��Stretch=�61   $��0q�6M   $1 +Local time zone where controller is located��5%      .��5&      � $��2q�6     5'      �� 5(      $
��120* �6'   �� 5)      $
��200* �6'   .+�5+      �� -   5        $��lblTimeZone��5+      $��
Time Zone:��6+   $��3��6@   $
��Left=�6U   $��	10,10,0,0q�6p   $	��Top=�6�   �� 5,      -   5        $��cmbTimeZone��5,      $ 1��6.   $��1��6N   $
��Left=�6_   $��0,6,0,0q�6z   $	��Top=�6�   $	��200��6�   �� 5-      -   5        $��lblCardDatabase��5-      $��Card Database:��60   $
��Left=�6J   $��	10,38,0,0q�6e   $	��Top=�6x   �� 5.      -   5        $��cmbCardDatabase��5.      $ 2��62   $��1��6R   $
��Left=�6b   $��0,40,0,0q�6}   $	��Top=�6�   $	��200��6�   $! System database in PremiSys��6�   �� 5/      -	   5        $��
lblScpType��5/      $��Type of Controller:��6*   $��28��6H   $
��Left=�6T   $��	10,69,0,0q�6o   $	��Top=�6�   $	��115��6�   �� 50      -
   5        $��
cmbScpType��50      $ 3��6.   $��1��6N   $��	0,74,28,0q�6^   $	��Top=�6q   $ Controller type��6�   �� 51      -   5        $��lblIOBautRate��51      $��I/O Baut Rate:��6-   $
��Left=�6G   $��
10,101,0,0q�6b   $	��Top=�6v   �� 52      -   5        $��cmbIOBautRate��52      $ 4��60   $��	0,106,0,0q�6Q   $	��Top=�6d   $��1��6|   $) #Baud rate for communication to I/Os��6�   $
��Left=�6�   $	��103��6�   �� 53      -   5        $��lblIOTimeout��53      $��I/O Timeout:��6,   $��28��6C   $
��Left=�6O   $��
10,135,0,0q�6j   $	��Top=�6~   �� 54      -   5        $��txtIOTimeout��54      $ 5��6/   $��1��6O   $
��Left=�6`   $��	0,137,0,0q�6{   $	��Top=�6�   $	��103��6�   $8 2Valid response time-out values for I/Os: 50-150 ms��6�   5;      5<      �� 5=   
   -   5        $��	grpReader��5=      $��Reader��6%   $��1��5>      $��2��6$   $��3��61   $��Stretch=�5?      $��Stretch=�62   .��5@      �   5A      �� 5B      $
��120* �6'   �� 5C      $
��200* �6'    5E      �� 5F      $��30 �6$   �� 5G      $��30 �6$   �� 5H      $��30 �6$   �� 5I      $��1 �6$   �� 5J      $	��140 �6$   �� 5K      $��* �6$   .+�5M      �� -   5        $��lblReaderDefaultMode��5M      $��Default Mode:��65   $��28��6M   $
��Left=�6Y   $��10,6,0,0q�6t   $��Center=�6�   �� 5N      -   5        $��cmbReaderMode��5N      $ 8��60   $��1��6P   $? 9Select the reader mode to be used under normal conditions��6`   $
��Left=�6�   $��Center=�6�   $	��188��6�   $	��0,3q�6�   �� 5S      -   5        $��lblDefaultProtocol��5S      $��1��62   $��Default Protocol:��6?   $
��Left=�6[   $��10,0,0,0q�6v   $��Center=�6�   �� 5T      -   5        $��cmbReaderProtocol��5T      $��1��65   $ 9��6B   $2 ,Select the kind of card format you are using��6b   $��0,0,29,0q�6�   $��Center=�6�   $��1��6�   �� 5V      -   5        $��lblConfiguration��5V      $��2��60   $��Configuration:��6=   $
��Left=�6V   $��10,0,0,0q�6q   $��Center=�6�   �� 5W      -   5        $��cmbReaderConfiguration��5W      $��0,0,29,0q�6:   $��2��6L   $ 11��6Y   $? 9Choose whether door has a single reader or paired readers��6z   $��Center=�6�   $��1��6�   �� 5Y      -   5        $��
lblControl��5Y      $��Control:��6+   $��4��6>   $
��Left=�6K   $��11,6,0,0q�6f   $	��Top=�6x   &   System.Windows.Controls.DataGrid  5[      -   5        $��gridControlFlags��5[      $��0q�6K   
������ $��4��5\      $��1��6)   $	��0,5q�69   $��Stretch=�5]      $��Stretch=�6:      HeadersVisibility$
 None=�5^         GridLinesVisibility$
 None=�65      CanUserAddRows .  $ 12��5_         AutoGenerateColumns .  ��5[   3   �� 	   Columns	 5a      4  .System.Windows.Controls.DataGridCheckBoxColumn  5b      -   5        colControlChecked��,  &System.Windows.Controls.DataGridColumn
   Header$
 Select��5b   M      Width5  /System.Windows.Controls.DataGridLengthConverter$ 30 6�      CanUserResize .  1  +System.Windows.Controls.DataGridBoundColumn   Binding 6]   ��  �� Path$ 
IsAssigned�6�   0  *System.Windows.Controls.DataGridTextColumn  5c      -   5        colControlFlagID��$
 
Mask Value��5c   H      
Visibility$ Hidden=�6�    6\   �� $ AcrControlFlag1�6�     5d      -   5        coControlFlagName��$

 Name��5d   I   $	 150 6u    6W   �� $
 Name�6u     5e      -   5        colControlFlagDescription��$
 Description��5e   Q   $ Hidden=�6�    6f   �� $ Description�6�   5h      �� 5j      -   5        $��lblCardFormat��5j      $��5��6-   $
��10,0q�6:   $��Card Format:��6H   $	��Top=�5k      $
��Left=�60     5n      -   5        $��gridCardFormats��5n      $��0q�5p      
������ $��5��5q      $��1��6)   $	��0,5q�69   $��Stretch=�5r      $��Stretch=�6:   $
 None=�5s      $
 None=�65    .  $ 13��5t       .  ��5o      �� *SCPCardFormatsView+	 5v        5w      -   5        colIsAssigned��$
  ��5w   I   $ 30 6w    6S   �� $ 
IsAssigned�6w     5x      -   5        colCardFormatID��$
 ID��5x   G   $ 50 6o    .  $ Hidden=�6�    6S   �� $ ID�6�     5y      -    5        colCardFormatName��$
 Card Formats��5y   I   $	 150 6}    6_   �� $
 Name�6}     5z      -!   5        colCardFormatDescription��$

 Name��5z   P   $ Hidden=�6�    6^   �� $ Description�6�   5|      �� 5}      -"   5        $��btnMoreFormats��5}   �   $��14��6   $��more formats...��6'   $��5��6A   $��23��6N   $
��Left=�6Z   $��	14,23,0,0q�6u   $	��Top=�6�   $��96��6�   6�   5~      5      �� 5�   
   -#   5        $��grpDoors��5�   "   $��Doors��6   $��0q�62   $��0��6=   $��4��6M   $��Stretch=�6Z   $��Stretch=�6x   $1 +Local time zone where controller is located��6�   .��5�      � $��2q�6     5�      �� 5�      $
��120* �6'   �� 5�      $
��200* �6'   .+�5�      �� -$   5        $��lblTimeZoneForUnlock��5�      $��	4,10,0,38q�64   $��Time Zone for unlock:��6H   $
��Left=�6h   $	��Top=�6�   �� 5�      -%   5        $��cmbTimeZoneForUnlock��5�      $��25��67   $ 6��6C   $��	0,10,27,0q�6c   $@ :Time Zone during which this door is automatically unlocked��6w   $	��Top=�6�   $��1��6�   �� 5�      -&   5        $��btnAddTimeZone��5�   @   $��1��6   $��20��6)   $��20��64     ibtnAdd#���  6V   $ Add a new time zone��6x   $��0,13,3,0q�6�   $	��Top=�6�   $��Right=�6�   6�   �� 5�      -'   5        $��ckbIsFirstUnlock��5�      $ 7��63   $��First Card Unlock��6S   $��8,42,0,0q�6o   $
��Left=�6�   $	��Top=�6�   $H BApplies first-card unlock action to door during time zone selected��5�      5�      5�      5�      5�      