   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      S OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:WPFSecurityControlSystem.UtilsWPFSecurityControlSystem.Utils 5      U/clr-namespace:WPFSecurityControlSystem.Services!WPFSecurityControlSystem.Services 5      U/clr-namespace:WPFSecurityControlSystem.Controls!WPFSecurityControlSystem.Controls 5      Z VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4dEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.ExtendedMicrosoft.Windows.Controls 5      ` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35tNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary!Microsoft.Windows.Controls.Ribbon 5	      0   *Common.WindowStyle.WindowCustomBorderStyle\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries   WindowRootMargin  ibtnAdd �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value �� 
TargetName  WindowCorderRadius  TitleCorderRadius  TitleBarRadialButton	 �� Triggers
 �� 	Resources6  0WPFSecurityControlSystem.Utils.AsteriskConverter  Key0  *WPFSecurityControlSystem.Controls.LabelExt �� Orientation  asteriskConverter �� 	Converter �� TextWrapping �� 
Foreground  ErrorTemplate8  2WPFSecurityControlSystem.Controls.ValidationButton � Cursor � ToolTip �� ElementName �� Path.  (Microsoft.Windows.Controls.MaskedTextBox/  )Microsoft.Windows.Controls.DateTimePicker  CloseRadialButton �� BasedOn �Name J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   Color   
BlurRadius ��Name :� RoutedEvent �� 
TargetName �� TargetProperty	 W� To g� Duration 	 MinimizeRadialButton 
 MaximizeRadialButton  WindowBorder �� Binding  �� RelativeSource! �� Value  TitleBarBackgroundBorder  TitleBarBackgroundBrush  
WindowRoot  RectBorderStyle  WindowTemplate" � 	Resources# ��Name$ � Icon  	TextBrush% � Title& �� SelectedIndex' �� Mode( �� TargetNullValue) �� FallbackValue
* � Tag+ ��Name  SkinWindowStyle   5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .sys&clr-namespace:System;assembly=mscorlib  5      6utils,clr-namespace:WPFSecurityControlSystem.Utils  5      7dal/clr-namespace:WPFSecurityControlSystem.Services  5      8ctrl/clr-namespace:WPFSecurityControlSystem.Controls  5      QctrlExtEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.Extended  5      YribbonNclr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary  5	        5
      %�&  &	        &	      &	 $    &	 E    &	 f    '�� m    '�� �    '�� 0    '  �    7  &	 
    '�� Q    7  '  �    7  '�� =    7  '�� �    '  1    7  '�� �    &	 �    7  &		 �    7  &	
 k    7  &	 A    7  &	 �    7  &	 <    &	 �    &	 b    7 	 7 
 7  7  7  7  7  7  7  7  7  &	 �#    7  u�5      7,7,7,7q�62   �� 5      " ��6   .�5   
   ��  �@�Cursor$
 Hand��6#   62   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      � $��14��6   $��14��6&   .+�5      �� $��Normal��6    $��..\Images\add.png��6.   $y�Visible=�6I   �� 5      $��Hover��6    $��..\Images\add_hover.png��6-   $y�Hidden=�6N   �� 5      $��Center=�6+   $��Center=�6H   5       5      P� 5       � {� .  . �5      �� $ Normal��6%    � y�$ Hidden=�6O   �� 5      $ Hover��6%    � y�$ Visible=�6N   5      P� 5       �@J�IsMouseOver .  . �5      �� $ Normal��6%    � y�$ Hidden=�6O   �� 5      $ Hover��6%    � y�$ Visible=�6N   5      5       5"      5#      ��5&      	10,10,5,5��69   ��5'      	10,10,0,0��68   �� 5*      " ��6)   .�5+   
   ��  �@J�SnapsToDevicePixels . 60   �� 5,   
    �@�OverridesDefaultStyle . 62   �� 5-   
    � ��$ 13��6&   �� 5.   
    � ��$ 13��6%   �� 5/   
    �@J�Opacity$	 0.9Q�6$   50      �� 53      
StyleGroup��" ��6    .�54   
   ��  � ��$ 12(�6%   �� 55   
    � ��$
 Bold"�6'   56      �� 57      StyleCheckBox��" ��6#   .�58   
   ��  �@�Cursor$
 Hand��6#   62   	 59   
   P� 5:       �@d�	IsChecked$ FalseY�6+   . �5;      ��  � ��$ Normal"�6/   6@   5<      P� 5=       �@d�	IsChecked$
 TrueY�6+   . �5>      ��  � ��$
 Bold"�6/   6>   5?      5A      �� 5C      
StyleLabel��" ��6    
 5D   
   ��  5E      asteriskConverter 6&   5F      .�5G   
   ��  � ��$ Normal"�6'   5_      �� 5a      StyleLabelExt��"  6#   
 5b   
   ��  5c      asteriskConverter 6&   5d      .�5e   
   ��  � ��$ Normal"�6'   �� 5f   
    � �� 5g      �� 5h      "  6"   .R�5i      �� $ 
Horizontal=�6!   
������ .+�5j      �� $��asterisk��6    $��Center=�61   $��Right=�6L   $
��Bold"�5l   "   $��0q�64   
���  ����5k   "   �� *
IsRequired+8   5l   H   �� 5n      $��theLabel��6    $	��0,0q�61   $
��Auto��6>   $��Center=�6L   $
��Left=�6g   .��5o      �� $
��Left=�6)   $	��0,0q�6D   $
 Wrap=�6Q   # z��6e   #��z��5p   )   #��z��6O   5q      5s       5t      P� 5u       � {� .  . �5v      �� $ asterisk��6%    � ��
 �����6Q   �� 5w      $ theLabel��6%    � ��
 �����6W   5x      5}      5      5�      �� 5�      .R�5�   
   ]� ��. .+�5�        $��5q�6$   $��Right=�6/   $
 Hand��6F    5�      �� $ ValidationAdorner��$8 2AdornedElement.(Validation.Errors)[0].ErrorContent��� 5�      
���  ��$��1q�6'   ._�5�      �� $��ValidationAdorner��6,   5�      5�      5�      �� 5�      " ��6   .�5�   
   ��  � ��$ 24��6#   �� 5�   
    �@=�ErrorTemplate8   65   5�      �� 5�      "  6   .�5�   
   ��  � ��$ 24��6#   �� 5�   
    �@=�ErrorTemplate8   65   5�      �� 5�      " ��6   .�5�   
   ��  � ��$ 24��6#   �� 5�   
    � ��$ Center=�65   �� 5�   
    �@=�ErrorTemplate8   65   5�      �� 5�      " ��6   .�5�   
   ��  �@�Cursor$
 Hand��6#   62   5�      �� 5�      "  6   .�5�   
   ��  � ��$ 24��6#   �� 5�   
    �@=�ErrorTemplate8   65   5�      �� 5�      " ��6   .�5�   
   ��  �@�Cursor$
 Hand��6#   62   5�      �� 5�      " ��6&   8   .�5�   
   ��  � �� 5�      �� 5�      " ��6"   .R�5�      � $ 
ButtonRoot��6    5�        5�      $ 0Q�6/   $	 Red��6?   $ 0Q�6K   .+�5�      �� $ Image��6    $��Images\normal close.png��6/   $��Center=�5�       $��Center=�6=   $��13��6X   $��13��6c   5�      5�       5�      P� 5�       �@J�IsMouseOver . . �5�      �� $ Image��6%    � ��$" Images\highlighted close.png��6J   5�      P� 5�       � �� . 5�      :� 5�      $ 
MouseEnter��6'   .[�5�      �� .��5�   "   �� .��5�   &   W� $ 
ButtonRoot��66   $6 0(UIElement.Effect).(DropShadowEffect.BlurRadius)�5�   6   $ 15X�5�   6   $ 0:0:0.3D�6>   W� 5�   &   $ Opacity�66   $ 1X�5�   6   $ 0:0:0.5D�6=   5�   #   5�      5�      :� 5�      $ 
MouseLeave��6'   .[�5�      �� .��5�   "   �� .��5�   &   W� $ 
ButtonRoot��66   $6 0(UIElement.Effect).(DropShadowEffect.BlurRadius)�5�   6   $ 0X�5�   6   $ 0:0:0.2D�6=   W� 5�   &   $ Opacity�66   $	 0.9X�5�   6   $ 0:0:0.5D�6?   5�   #   5�      5�      5�      5�      5�      �� 5�      " ��6)   8   .�5�   
   ��  � �� 5�      �� 5�      " ��6"   .R�5�      � .+�5�      �� $ Image��6    $ ��Images\normal minimize.bmp��6/   $��Center=�5�       $��Center=�6=   $��13��6X   $��13��6c   5�       5�      P� 5�       �@J�IsMouseOver . . �5�      �� $ Image��6%    � ��$% Images\highlighted minimize.bmp��6J   5�      P� 5�       � �� . 5      5     5     5     �� 5     " ��6)   8   .�5	  
   ��  � �� 5
     �� 5     " ��6"   .R�5     � .+�5     �� $ Image��6    $ ��Images\normal maximize.bmp��6/   $��Center=�5      $��Center=�6=   $��13��6X   $��13��6c   5      5     P� 5      �@J�IsMouseOver . . �5     �� $ Image��6%    � ��$% Images\highlighted maximize.bmp��6J   5     P� 5      � �� . 5     5     5     5     �� 5     " ��6!   .�5  
   ��  �@��CornerRadius8   6)   �� 5  
    � �� �#ABABAB6'   �� 5   
    � ��
 �iii�6(   �� 5!  
    � ��$ 1q�6,   �� 5"  
    � ��$ Stretch=�60   �� 5#  
    � ��$ Stretch=�6.   	 5$  
   �� 5%      6   ��   �� *TemplatedParent+$ WindowState�! 	Maximized6p   .a�5&     ��  �@��CornerRadius$ 0��61   5'     5)     �� 5+     " ��6-   .�5,  
   ��  � ��$ 30��6#   �� 5-  
    �@��CornerRadius8   6)   �� 5.  
    � ��# �  6'   	 5/  
   �� 50      6   ��   �� *TemplatedParent+$ WindowState�! 	Maximized6p   .a�51     ��  �@��CornerRadius$ 0��61   52     54     �� 56     " �6   	 57  
   �� 58      6   ��   �� *TemplatedParent+$ WindowState�! 	Maximized6p   .a�59     ��  � ��# �   6+   5:     5<     �� 5>     " ��6$   .�5?  
   ��  � |� .  6&   �� 5@  
    � ��
 ���� 6!   5A     �� 5D     " �6-   " 5E  
   ���5F     BorderWidth 7 �60   5G     .R�5I  
   � $ 
WindowRoot��6   -   5        #���  5I  #   .+�5J     �� $# WindowFrame��6   #���  6*   ._�5K     � $ WindowLayout��6   .+�5N     � $��0q�6   $	��Top=�6&   .+�5O     �� -   5        #���  5O  !   5P  B   �� 5Q     $ IconApp��6    $��10, 0, 0, 0q�61   $
��Left=�6F   #��z$ 6a   6�   �� 5R     -   5        #���  5R  $   #��z% 6M   $��Center=�5S  $   $��Center=�6A   $��Normal"�6\   5T  $   �� 5W     $	��0,8q�6%   $ 
Horizontal=�62   $��Right=�6K   $��Center=�6g   .+�5X     �� -   5        $��0,0,5,0q�5X  '   $��16��68   $��8(�6D   $��3q�6Q   & 5Z  '   �� $ Settings[SelectedTheme]�$' TwoWay=�$( 2��$) 2��.@�5]  "   �� $
* None��6/   .��6:   None6@   �� 5^  "   $* 	ShinyBlue��6/   .��6?   
Shiny Blue6K   �� 5_  "   $* 
BureauBlue��6/   .��6@   Bureau Blue6M   �� 5`  "   $* WhistlerBlue��6/   .��6B   Whistler Blue6Q   �� 5a  "   $* ExpressionDark��6/   .��6D   Expression Dark6U   �� 5b  "   $* ExpressionLight��6/   .��6E   Expression Light6W   5c     �� 5d     -   5        8��  $��Right=�5d  j   $��Center=�6�   $��0,0,5,0q�6�   �� 5e     -   5        8�� $��Right=�5e  j   $��Center=�6�   $��0,0,5,0q�6�   �� 5f     -   5        8�� $��Right=�5f  d   $��Center=�6�   $��0,0,10,0q�6�   5g     5h     �� 5k     $# WindowContent��6   $��0,30,0,0q�64   ._�5l     �� .u�5m     �� 5n     5o     �� 5s     $+ leftSizeGrip��6    8�� $��7��6_   $
��Left=�6i   �� 5t     $+ rightSizeGrip��6    8�� $��7��6`   $��Right=�6j   �� 5u     $+ topSizeGrip��6    8�� $��7��6^   $	��Top=�6i   �� 5v     $+ bottomSizeGrip��6    8�� $��7��6a   $��Bottom=�6l   �� 5x     $��topLeftSizeGrip��6    8�� $��7��6`   $��7��6j   $
��Left=�6u   $	��Top=�6�   �� 5y     $��bottomRightSizeGrip��6    8�� $��7��6e   $��7��6o   $��Right=�6z   $��Bottom=�6�   �� 5z     $��topRightSizeGrip��6    8��	 $��7��6b   $��7��6l   $��Right=�6w   $	��Top=�6�   �� 5{     $��bottomLeftSizeGrip��6    8��
 $��7��6d   $��7��6n   $
��Left=�6y   $��Bottom=�6�   5|     5}     5~     5     �� 5�     " �6$   .�5�  
   ��  �@�WindowStyle$
 None=�6(   �� 5�  
    �@�AllowsTransparency . 6/   �� 5�  
    �@�
ResizeMode$ 	CanResize=�6'   �� 5�  
    � ��
 ���� 6'   �� 5�  
    � ��$	 150��6&   �� 5�  
    � ��$	 250��6%   �� 5�  
    �@J�Opacity$	 1.0Q�6$   �� 5�  
    � ��8   6%   	 5�  
   �� 5�      6   �� *WindowState+! 	Maximized6:   5�     5�     5�     