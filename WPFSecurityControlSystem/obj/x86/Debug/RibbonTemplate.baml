   M S B A M L   `   `   `  ����`  \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35tNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary!Microsoft.Windows.Controls.Ribbon  5       WPFSecurityControlSystem   RibbonTemplate\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ShellWindowTemplate  �� 
TargetType � 	Resources  Key �Name  
WindowRoot ��Name  WindowBorder � ColumnDefinitions � RowDefinitions  TitleBarBackgroundBorder ��Name � Icon  	TextBrush � Title	 �� Orientation
 �� SelectedIndex �� Path �� Mode �� TargetNullValue �� FallbackValue
 � Tag  MinimizeRadialButton  MaximizeRadialButton  CloseRadialButton ��Name � DataContext �Name d� 	IsChecked � ToolTip �� ShowsPreview ��Name 	 RectBorderStyle 
 SkinShellWindowStyle �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value   5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      YribbonNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary  5      %  &	        7  7  7  7 	 7 	 7 	 7 	 7 	 7 	 7 	 7 	 &	
 �    7   �� 5      "  �62    5   
   ���5	      BorderWidth 7 �60   5      .R�5   
   � $ 
WindowRoot��6   #���  6#   .+�5      �� $ WindowFrame��6   #���  6*   ._�5      � $ WindowLayout��6    5      �� 5      $
��Auto �6+   �� 5      $��3 �6+   �� 5      $
��300* �6+   �� 5      $
��Auto �6+    5      �� 5      $	��135 �6(   �� 5      $
��200* �6(   �� 5      $��30 �6(   .+�5!      � $��0q�6   $	��Top=�6&   .+�5"      �� #���  6!   �� 5#      $ IconApp��6    $��10, 0, 0, 0q�61   $
��Left=�6F   #��z 6a   6�   �� 5$      #���  6$   #��z 6M   $��Center=�5%   $   $��Center=�6A   $��Normal"�6\   �� 5(      $	��0,8q�6%   $	 
Horizontal=�62   $��Right=�6K   $��Center=�6g   .+�5)      �� $��0,0,5,0q�6'   $��16��68   $��8(�6D   $��3q�6Q   
 5*   '   �� $ Settings[SelectedTheme]�$ TwoWay=�$ 2��$ 2��.@�5,   "   �� $
 None��6/   .��6:   None6@   �� 5-   "   $ 	ShinyBlue��6/   .��6?   
Shiny Blue6K   �� 5.   "   $ 
BureauBlue��6/   .��6@   Bureau Blue6M   �� 5/   "   $ WhistlerBlue��6/   .��6B   Whistler Blue6Q   �� 50   "   $ ExpressionDark��6/   .��6D   Expression Dark6U   �� 51   "   $ ExpressionLight��6/   .��6E   Expression Light6W   52      �� 53      8��  $��Right=�6T   $��Center=�6p   $��0,0,5,0q�6�   �� 54      8�� $��Right=�6S   $��Center=�6o   $��0,0,5,0q�6�   �� 55      8�� $��Right=�6Q   $��Center=�6m   $��0,0,10,0q�6�   56      57      �� 5O      $ MenuArea��6%   $��0q�67   $��4��6B   $��0��6V   $��Stretch=�6c   $��Stretch=�6�   �� 5R      $ WindowContent��6   $��0q�64   $��1��6?   $��4��6L   ._�5S      �� .u�5T      � $��2q�6#    6.   ��  5U   "   �� 5V   &   $
��Auto �67   �� 5W   &   $��3 �67   �� 5X   &   $ 	colCenter��6D   $
��300* �67   6D   �� 5Y   &   $��3 �67   �� 5Z   &   $
��Auto �67   .+�5]   "   ]� $��pnlLeftTools��6,   $	��300��6@   $��Stretch=�6O   $��Stretch=�6m   
�������.+�5^   &   d� $��btnHideLeftTools��63   $ FalseY�6K   $��>��6]   $	��Top=�6i   $
��Left=�6~   $��5q�6�   $��20��6�   $��20��6�   $ 	Hide\Show��6�   � 5_   &   $��pnlLeftTabs��6+   $��2q�6>   $��Stretch=�6I   $��Stretch=�6e   .+�5`   *   �� $��navigationRegion��6;   5a   '   5b   #   �� 5d   "   $��splitterLeft��6/   $��1��6C   $��Stretch=�6S   $
��Left=�6r   $��Stretch=�6�    .  6�   �� 5g   "   $ 
ClientArea��61   $��2��6E   $	��200��6V   $��Stretch=�6e   $��Stretch=�6�   �� 5k   "   $��pnlRightTools��6-   $��3��6B   $��30��6R   $	��260��6`   $	 Vertical=�6l   $��Stretch=�6�   $��Stretch=�6�   
�������.+�5l   &   d� $��btnHidRightTools��63   $ FalseY�6K   $��Right=�6]   $�� ��6y   $ 	Hide\Show��6�   $��5q�6�   $��20��6�   $��20��6�   �� 5m   &   $
��Auto=�63   $��Stretch=�6V   .��5n   *   �� $��pnlRightAccordionTools��65   $	��260��6S   
�������.+�5p   .   �� $��actionsToolsRegion��6?   $	��200��6Y   5r   +   5s   '   5t   #   5u      5w      5x      �� 5z      $��Bottom=�6    $��4��68   $��2��6M   $��0q�6Z   $��30��6e   
�������$��Bottom=�6�   �� 5~      $ leftSizeGrip��6    8�� $��7��6_   $
��Left=�6i   �� 5      $ rightSizeGrip��6    8�� $��7��6`   $��Right=�6j   �� 5�      $ topSizeGrip��6    8�� $��7��6^   $	��Top=�6i   �� 5�      $ bottomSizeGrip��6    8�� $��7��6a   $��Bottom=�6l   �� 5�      $��topLeftSizeGrip��6    8�� $��7��6`   $��7��6j   $
��Left=�6u   $	��Top=�6�   �� 5�      $��bottomRightSizeGrip��6    8�� $��7��6e   $��7��6o   $��Right=�6z   $��Bottom=�6�   �� 5�      $��topRightSizeGrip��6    8��	 $��7��6b   $��7��6l   $��Right=�6w   $	��Top=�6�   �� 5�      $��bottomLeftSizeGrip��6    8��
 $��7��6d   $��7��6n   $
��Left=�6y   $��Bottom=�6�   5�      5�      5�      5�      �� 5�      " �6)   .�5�   
   ��  �@�WindowStyle$
 None=�6(   �� 5�   
    �@�AllowsTransparency . 6/   �� 5�   
    �@�
ResizeMode$ 	CanResize=�6'   �� 5�   
    � ��
 ���� 6'   �� 5�   
    � ��$	 150��6&   �� 5�   
    � ��$	 250��6%   �� 5�   
    �@J�Opacity$	 1.0Q�6$   �� 5�   
    � ��8   6%    5�   
   �� 5�       6   �� *WindowState+ 	Maximized6:   5�      5�      5�      