   M S B A M L   `   `   `  ����`  \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35tNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary!Microsoft.Windows.Controls.Ribbon  5   	   S OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:WPFSecurityControlSystem.Commands!WPFSecurityControlSystem.Commands 5   	   C&clr-namespace:WPFSecurityControlSystemWPFSecurityControlSystem 5   	   Z VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4dEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.ExtendedMicrosoft.Windows.Controls 5   	   �Fclr-namespace:WPFSecurityControlSystem.MODULE.HWConfiguration.Controls8WPFSecurityControlSystem.MODULE.HWConfiguration.Controls 5	   	   }Cclr-namespace:WPFSecurityControlSystem.MODULE.HWConfiguration.Views5WPFSecurityControlSystem.MODULE.HWConfiguration.Views 5
   	   3   -WPFSecurityControlSystem.HWConfigurationShell   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   aribbonNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary     5   	   Dcommands/clr-namespace:WPFSecurityControlSystem.Commands     5   	   8local&clr-namespace:WPFSecurityControlSystem     5   	   YctrlExtEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.Extended     5   	   WctrlFclr-namespace:WPFSecurityControlSystem.MODULE.HWConfiguration.Controls     5	   	   TviewCclr-namespace:WPFSecurityControlSystem.MODULE.HWConfiguration.Views     5
   	     � Title$+  %PremiSys 3.0 - Hardware Configuration��5   	   $	��400��67   $
��1210��6G    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$	 New��5      6>     5   
   -   5        $
 Open��5      6=     5   
   -   5        $ Delete��5      6A     5   
   -   5        $- 'commands:HWConfigCommands.GenerateDoors��5      6L   .��5      � $��2q�6    � DataContext 6   ��  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5       -   5         �Name$ 	colCenter��5    ,   $
��350* �6   6,   �� 5!      $
��Auto �6    � RowDefinitions 5#   
   �� 5$      $
��Auto �6   �� 5%      $��* �6   �� 5&      $��23 �6   .+�5*   
   .   (Microsoft.Windows.Controls.Ribbon.Ribbon  -   5        $��MainRibbonMenu��5*      $
��Auto��6.   $
��Auto��6;   $��4��6I      ApplicationMenu 5+      =   7Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenu  5,      .@�5-      A   ;Microsoft.Windows.Controls.Ribbon.RibbonApplicationMenuItem  $
��Help��6;     5.      -   5        $��Close��5.   ;   6J   5/      .@�51      1   +Microsoft.Windows.Controls.Ribbon.RibbonTab  $��	Configure��6#   .@�52      3   -Microsoft.Windows.Controls.Ribbon.RibbonGroup  $��Hardware��6)   .@�53      4   .Microsoft.Windows.Controls.Ribbon.RibbonButton     Label$	 Add��6.   $	��New��6:   	   LargeImageSource$	 /Images/config_add.png��6H     54      $
 Edit��6.   $
��Open��6;   $	 /Images/config_edit.png��6J     55      $ Remove��6.   $��Delete��6=   $	 /Images/config_delete.png��6N     56      -   5        
  Name$
 rbtnDownload��56   .   $ Download��6D   $!	 /Images/config_download.png��6U   ��57   *   ��  �� Path$&  HWConfigCommands.DownloadCommand�  58      -	   5        $
 rbtnSetDefaults��58   .   $ Set defaults��6H   $	��100��6]   $��20��6j   ��59   *   �� $) #HWConfigCommands.SetDefaultsCommand���5:   *   �� $&  CurrentDataContext.CurrentObject�5?        5@      $��Globals��6%   .@�5A        $ 	Add Group��6*   $	��100��6<   $!	 /Images/config_addgroup.png��6�   ��6I   �� $&  HWConfigCommands.AddGroupCommand�6�     5B      $ 
Edit Group��6*   $	��100��6=   $"	 /Images/config_editgroup.png��6�   ��6J   �� $' !HWConfigCommands.EditGroupCommand�6�     5C      $ Delete Group��6*   $	��100��6?   $$	 /Images/config_deletegroup.png��6�   ��6L   �� $) #HWConfigCommands.DeleteGroupCommand�6�     5D      $ Remove Doors��6*   $	��100��6?   $$	 /Images/config_removedoors.png��6�   ��6L   �� $) #HWConfigCommands.RemoveDoorsCommand�6�   5E        5F      $��Access Settings��6%   .@�5G        $ 
Add Access��6*   $"	 /Images/config_addaccess.png��6x   ��6=   �� $' !HWConfigCommands.AddAccessCommand�6x     5H      $ Edit Access��6*   $#	 /Images/config_editaccess.png��6z   ��6>   �� $( "HWConfigCommands.EditAccessCommand�6z     5I      $ Delete Access��6*   $%	 /Images/config_deleteaccess.png��6~   ��6@   �� $* $HWConfigCommands.DeleteAccessCommand�6~   5J        5K      $��Plug-ins��6%   .@�5L        $ Add Plug-in��6*   $"	 /Images/config_addplugin.png��6y   ��6>   �� $' !HWConfigCommands.AddPluginCommand�6y     5M      $ Edit Plug-in��6*   $#	 /Images/config_editplugin.png��6{   ��6?   �� $( "HWConfigCommands.EditPlguinCommand�6{     5N      $ Delete Plug-in��6*   $%	 /Images/config_deleteplugin.png��6   ��6A   �� $* $HWConfigCommands.DeletePluginCommand�6   5O      5P        5Q      $��Control��6#   5R        5S      $
��View��6#   5T      5U      ]� 5W   
   -
   5        $��pnlLeftTools��5W      
�������$��1��6@   $��0��6M   $��20��6]   $��Stretch=�6k   $��Stretch=�6�   .+�5X      d� -   5        $��btnHideLeftTools��5X       d� 	IsChecked$ FalseY�63   $��>��6E   $	��Top=�6Q   $
��Left=�6f   $��5q�6�   $��20��6�   $��20��6�    � ToolTip$ 	Hide\Show��6�   6�   � 5Y      -   5        $��pnlLeftTabs��5Y      $��2q�6&   $	��300��61   $��Stretch=�6=   $��Stretch=�6Y   .+�5Z      �� -   5         ��Name$ NavigationRegion��5Z   !   $��1��6;   $��0��6H   $��0q�6X   $��Stretch=�6c   $��Stretch=�6�   5\      5]      �� 5`   
   $��1��6   $��1��6   $��0q�6.   
�������$��1q�6P   $��Stretch=�6d   $��Stretch=�6�   ._�5a      �� -   5        $ 
ClientArea��5a      $��0q�61   $��Stretch=�6<   $��Stretch=�6a   5b      �� 5d   
   -   5        $��pnlRightTools��5d      $��1��6*   $��2��67   
������� �� Orientation$ Vertical=�6^   $��Stretch=�6u   $��Stretch=�6�   $��30��6�   .+�5e      d� -   5        $��btnHidRightTools��5e      $ FalseY�63   $	��Top=�6E   $��Right=�6Z   $�� ��6v   $ 	Hide\Show��6�   $��5q�6�   $��20��6�   $��20��6�   6�   �� 5f      -   5        $��pnlRightAccordionTools��5f      $
��Auto=�69   $��Stretch=�6\   .��5g      �� -   5        $ ToolsRegion��5g       $��Stretch=�65   $��Stretch=�6Z   5i      5j      �� 5l   
   -   5         ��Name$ MainRegionBorder��5l      $��0q�6+   
�������$��2��6O   $��4��6\   ._�5m      �� $��17��6   $��17��6'   
�������$��Bottom=�6M   5n      5o      5q      