   M S B A M L   `   `   `  ����S  OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:WPFSecurityControlSystem.BaseWPFSecurityControlSystem.Base  5   	   }Cclr-namespace:WPFSecurityControlSystem.MODULE.HWConfiguration.Views5WPFSecurityControlSystem.MODULE.HWConfiguration.Views  5   	   S OIDenticard.Access.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulleHclr-namespace:IDenticard.Access.Common;assembly=IDenticard.Access.CommonIDenticard.Access.Common 5   	   U/clr-namespace:WPFSecurityControlSystem.Services!WPFSecurityControlSystem.Services  5   	   O,clr-namespace:WPFSecurityControlSystem.UtilsWPFSecurityControlSystem.Utils  5   	   Z VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4dEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.ExtendedMicrosoft.Windows.Controls 5	   	   �Fclr-namespace:WPFSecurityControlSystem.MODULE.HWConfiguration.Controls8WPFSecurityControlSystem.MODULE.HWConfiguration.Controls  5
   	   J    DWPFSecurityControlSystem.MODULE.HWConfiguration.Views.NavigationView   5      4base+clr-namespace:WPFSecurityControlSystem.Base  5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   XmainViewCclr-namespace:WPFSecurityControlSystem.MODULE.HWConfiguration.Views     5   	   ^	treemodelHclr-namespace:IDenticard.Access.Common;assembly=IDenticard.Access.Common     5   	   ?dal/clr-namespace:WPFSecurityControlSystem.Services     5   	   >utils,clr-namespace:WPFSecurityControlSystem.Utils     5   	   YctrlExtEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.Extended     5	   	   WctrlFclr-namespace:WPFSecurityControlSystem.MODULE.HWConfiguration.Controls     5
   	   $��Stretch=�5   l   $��Stretch=�6�     � 	Resources  5      3   -WPFSecurityControlSystem.Utils.ImageConverter   imageConverter��%   &	          5   
   6   5      .��5      �� -   5        $��tabCtrlLeftTools��5      $��Stretch=�6(   $��Stretch=�6D   .@�5      �� -   5        $��tabHW��5   "   $��Hardware��6/   $��Stretch=�6A   $��Stretch=�6]   $��0q�6{     5        
hwTreeData �� ItemsSource �� Orientation �� Path �� 	Converter  hwTreeStyle �� 
TargetType �� Property �� Value��%j  &	       7   &	 
    7  �� 5   "   
hwTreeData�� 6b   �� *
Children+.R�5   &   �� $ 
Horizontal=�61   .+�5   *   �� $��0 0 16 0q�60   $��16��6�   $��16��6�   ��6B   �� $
 UiId�8   6�   �� 5   *   $
��Bold"�64   ��6F   �� *Name+5   '   5:   #   �� 5;   "   " Q�6<   .�5@   &   ��  � ��8   6G   5A   #   5B      .��5C      R   LWPFSecurityControlSystem.MODULE.HWConfiguration.Controls.HWConfigurationTree  -   5          Name$ treeHWConfiguration��5C   7   R   LWPFSecurityControlSystem.MODULE.HWConfiguration.Controls.HWConfigurationTree	   Loaded$ 	 treeHWConfiguration_Loaded��5G   5   $��2q�5C   T   M IControls.WpfUI, Version=1.0.6.33364, Culture=neutral, PublicKeyToken=null�  �Controls.WpfUI.GenericTreeView.TreeViewBase`1[[IDenticard.Access.Common.LinkNode, IDenticard.Access.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]
   TreeNodeStyle#
 [ 5D   5      ObserveChildItems .    Items 5F   5   �� $ Children� � ContextMenu 5H   "   �� 5I   &   -   5        $��cmHWTree��5I   2    �� 	StaysOpen . .@�5J   *   m� -   5        $��menuAddSite��5J   E   $��Add Site��63    m� Command$	 New��6X    m� CommandParameter$
 Site��6f   m� 5K   *   -   5        $��menuEditSite��5K   F   $��	Edit Site��63   $
 Open��6Z   $
 Site��6j   m� 5L   *   -   5        $��menuDeleteSite��5L   H   $��Delete Site��63   $ Delete��6^   $
 Site��6o   m� 5M   *   -   5        $��menuDownloadSite��5M   K   $��Download Sites��63   $ Refresh��6c   $
 Site��6u   �� 5N   *   m� 5O   *   -	   5        $��menuAddController��5O   K   $��Add Controller��63   $	 New��6d   $ 
Controller��6r   m� 5P   *   -
   5        $��menuEditController��5P   L   $��Edit Controller��63   $
 Open��6f   $ 
Controller��6u   m� 5Q   *   -   5        $��menuDeleteController��5Q   N   $��Delete Controller��63   $ Delete��6j   $ 
Controller��6{   �� 5R   *   m� 5S   *   -   5        $��menuAddIOBoard��5S   H   $��Add IOBoard��63   $	 New��6^   $ IOBoard��6l   m� 5T   *   -   5        $��menuEditIOBoard��5T   I   $��Edit IOBoard��63   $
 Open��6`   $ IOBoard��6o   m� 5U   *   -   5        $��menuDeleteIOBoard��5U   K   $��Delete IOBoard��63   $ Delete��6d   $ IOBoard��6u   5V   '   5i      5j      �� 5k      $��Globals��6"   .��5l      R� -   5        $��treeGlobals��5l   '   $��2q�6:   $��Stretch=�5m   '   $��Stretch=�6E    5n   "   �� 5o   &   -   5        $��	cmGlobals��5o   2    . .@�5p   *   m� $��	Add Group��63   m� 5q   *   $
��Edit��63   m� 5r   *   $��Delete��63   5s   '   ��5u   "   �� 5v   &    6?   �� *
Children+.R�5w   *   �� $ 
Horizontal=�65   .+�5x   .   �� $��0 0 4 0q�64   $��..\..\Images\folder.png��6E   $��16��6f   $��16��6q   �� 5y   .   $
��Bold"�68   ��6J   �� *Name+5z   +   5{   '   ��5}   "   �� 5~   &   " Q�6,   .�5   *   ��  �@Q�
IsExpanded 6G   �� *
IsExpanded+ �� Mode$ TwoWay=��� 5�   *    �@Q�
IsSelected 6G   �� *
IsSelected+$ TwoWay=��� 5�   *    � ��$ Normal"�6G   5�   '   5�      5�      �� 5�      $��Access Settings��6"   .��5�      R� -   5        $��treeAccessSettings��5�   '   $��2q�6A   $��Stretch=�5�   '   $��Stretch=�6E    5�   "   �� 5�   &    . .@�5�   *   m� -   5        $��menuAddAccessSettings��5�   @   $	��Add��63   6@   m� 5�   *   -   5        $��menuEditAccessSettings��5�   A   $
��Edit��63   6A   m� 5�   *   -   5        $��menuDeleteAccessSettings��5�   C   $��Delete��63   6C   5�   '   ��5�   "   �� 5�   &    6?   �� *
Children+.R�5�   *   �� $ 
Horizontal=�65   .+�5�   .   �� $��0 0 4 0q�64   $��..\..\Images\folder.png��6E   $��16��6f   $��16��6q   �� 5�   .   $
��Bold"�68   ��6J   �� *Name+5�   +   5�   '   ��5�   "   �� 5�   &   " Q�6,   .�5�   *   ��  �@Q�
IsExpanded 6G   �� *
IsExpanded+$ TwoWay=��� 5�   *    �@Q�
IsSelected 6G   �� *
IsSelected+$ TwoWay=��� 5�   *    � ��$ Normal"�6G   5�   '   5�      5�      �� 5�      $��Devices��6"   .��5�      R� -   5        $��treeDevices��5�   #   $��2q�66   $��Stretch=�5�   '   $��Stretch=�6E    5�      �� 5�   "    . .@�5�   &   m� -   5        $��menuAddDeviceSettings��5�   C   $��
Add Device��6/   6C   m� 5�   &   -   5        $��menuEditDeviceSettings��5�   D   $��Edit Device��6/   6D   m� 5�   &   -   5        $��menuDeleteDeviceSettings��5�   F   $��Delete Device��6/   6F   5�   #   ��5�      �� 5�   "    6;   �� *
Children+.R�5�   &   �� $ 
Horizontal=�61   .+�5�   *   �� $��0 0 4 0q�60   $��..\..\Images\folder.png��6A   $��16��6b   $��16��6m   �� 5�   *   $
��Bold"�64   ��6F   �� *Name+5�   '   5�   #   ��5�      �� 5�   "   " Q�6(   .�5�   &   ��  �@Q�
IsExpanded 6C   �� *
IsExpanded+$ TwoWay=��� 5�   &    �@Q�
IsSelected 6C   �� *
IsSelected+$ TwoWay=��� 5�   &    � ��$ Normal"�6C   5�   #   5�      5�      5�      5�      