   M S B A M L   `   `   `  ����S  OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullc6clr-namespace:WPFSecurityControlSystem.Controls.Search(WPFSecurityControlSystem.Controls.Search  5      L    FWPFSecurityControlSystem.MODULE.HWConfiguration.Views.MainContentsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Isearch6clr-namespace:WPFSecurityControlSystem.Controls.Search     5      $��Stretch=�5      $	��Top=�6,   
�������  � 	Resources  5      B   <WPFSecurityControlSystem.Controls.Search.SearchTermConverter   SearchTermConverter  CellStyleBase �� 
TargetType*  $System.Windows.Controls.DataGridCell �� Property �� Value  DataGridCellStyleForSearch �� BasedOn?   9WPFSecurityControlSystem.Controls.Search.SearchOperations `� 	Converter �� Path �� RelativeSource �� Triggers	 P� Property
 P� Value �� EndPoint �� 
StartPoint � Color � Offset��%�  &	        &	      &	 Z     7  7     5   
   6%   �� 5
   
   "  6&   .�5      ��  � ��$ 1q�6'   5      �� 5   
   "  63   8   .�5      ��  �@ IsMatch 5      `� 5      8  .1�5      �� $* $(search:SearchOperations.SearchTerm)�6I    6"   �� *Self+6I   �� 5      $% Column.ContentBinding.Path.Path�6J    6#   �� *Self+6J   �� 5      $ DataContext�6J    6#   �� *Self+6J   5      5       5      P� 5      	 �@ IsMatch
 . . �5      ��  � �� 5      �� 5      $ 0.5,1&�62   $ 0.5,0&�6C   .��5   "   � $ White��6/   $ 0Q�6=   � 5   "   $ 	#FF78C4FF��6/   $ 1Q�6A   5      5!      5"      5$      5%      .��5'      � -   5        $��
LayoutRoot��5'      
������ $	��200��66   $��2q�6E   $��Stretch=�6P    � ColumnDefinitions 5(   
   �� 5)      $��10 �6   �� 5*      $��60 �6   �� 5+      $	��175 �6   �� 5,      $��50 �6   �� 5-      $	��160 �6   �� 5.      $	��161 �6   �� 5/      $	��20* �6    � RowDefinitions 51   
   �� 52      $��30 �6   �� 53      $��* �6   .+�56   
   �� -   5        $��
lblViewing��56      $��1��6"   $��Viewing:��62   $��0q�6E   $
��Left=�6Q   $��Center=�6m   �� 57   
   -   5        $��
cmbViewing��57      $��2��59      -  'System.Windows.Input.KeyboardNavigation   TabIndex$ 0��6#   $��0q�6C   $��Stretch=�5:      $��Center=�61   ��58      �� *Sites+5;      �� 5<   
   -   5        $��	lblSearch��5<      $��3��6#   $��Search:��63   $��0q�6F   $��Center=�6Q   $
��Left=�6l   �� 5=   
   -   5        $��ucSearch��5=      $��4��6"   $ 1��62   {�. $��25��6c   $	��140��6o   $��Stretch=�6   $��Center=�6�   �� 5>   
   -   5        $��lblViewColumns��5>      $��5��6(   $��View columns:��68   $
��Left=�6Q   $��Center=�6l   $��24,4,0,0q�6�   �� 5?   
   -   5        $��btnAddColumn��5?      $��+��6&   $ 3��62   $��20��6R   $��0,0,10,0q�6^   $��Right=�6p   $��Center=�6�   $��20��6�   $��5��6�   6�   &   System.Windows.Controls.DataGrid  5@   
   -   5        $��
gridPoints��5@         
IsReadOnly . $��1��5C      $��8��6    $ 4��64   $��
10,10,10,0q�6T    �� SelectedValuePath$ PointID��5D         SelectionMode$ Single=�6/   $��Stretch=�5E      $��Stretch=�6/   $
��Auto��6M      AutoGenerateColumns .     
SearchTerm 5A      ��  �� ElementName$ ucSearch��$
 Text� � DataContext 5B      ��    Columns 5F      5  /System.Windows.Controls.DataGridHyperlinkColumn  5G      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Door/Point��6*      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 5I   J      	CellStyle# [ 5J   *      ContentBinding 5H   *   �� $ DoorName�1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 5I   *   �� $ DoorID� 	  ElementStyle 5K      �� 5L      .�5M      -	   5        5N      5P        5Q      $ 	I/O Board��6*   $	 150 5S   L   # [ 5T   )    5R   )   �� $ IOBoardName� 5S   )   �� $ 	IOBoardID� 5U      �� 5V      .�5W   "   -
   5        5X      5Z        5[      $ 
Controller��6*   $	 200 5]   O   # [ 5^   )    5\   )   �� $ ControllerName� 5]   )   �� $ ControllerID� 5_      �� 5`      .�5a   "   -   5        5b      5d        5e      $
 Site��6*   $	 100 68   # [ 5h   )    5f   )   �� $ SiteName� 5g   )   �� $ SiteID� 5i      �� 5j      .�5k   "   -   5        5l      5n      5p      5w      5x      