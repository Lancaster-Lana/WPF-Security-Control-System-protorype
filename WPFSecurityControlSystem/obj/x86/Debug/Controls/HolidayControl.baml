   M S B A M L   `   `   `  ����S  OWPFSecurityControlSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:WPFSecurityControlSystem.DTOWPFSecurityControlSystem.DTO  5   .   M+clr-namespace:WPFSecurityControlSystem.BaseWPFSecurityControlSystem.Base  5      6    0WPFSecurityControlSystem.Controls.HolidayControl   5      4model*clr-namespace:WPFSecurityControlSystem.DTO  5   .   4base+clr-namespace:WPFSecurityControlSystem.Base  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      .��5      � $��2q�6     � ColumnDefinitions  5   
   �� 5      $	��100 �6   �� 5	      $	��280 �6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��80 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   .+�5   
   �� -   5        $��lblName��5      $��Holiday Name:��6   $
��Left=�67   $��0q�6R   $	��Top=�6]   �� 5   
   -   5        $��txtName��5      $��0q�6!   $��1��6,   -  'System.Windows.Input.KeyboardNavigation   TabIndex$ 0��6<   $
��Left=�6]   $��Center=�6x   $	��250��6�   ��5      �� *Name+ �� Mode$ TwoWay=��� 5   
   -   5        $��lblDescription��5      $��Description:��6&   $��1��6>   $
��Left=�6L   $��0q�6g   $	��Top=�6r   �� 5   
   -   5        $��txtDescription��5      $��1��6(   $��1��68   $ 1��6E   $
��Left=�6f   $	��Top=�6�   $	��250��6�   $��71��6�   �� 5   
   -   5        $��lblDate��5      $��0q�6   $��Date:��6*   $��2��6:   $
��Left=�6H   (  "System.Windows.Controls.DatePicker  5   
   -   5        $��dtpHolidayDate��5      $��1��6,   $��2��6<   $ 2��6I   $
��Left=�6j   $��Stretch=�6�   $	��172��6�   �� 5   
   -   5        $��lblAdditionalDay��5      $��0q�6(   $��Additional Day(s):��63   $��3��6Q   $
��Left=�6_   $	��Top=�6z   �� 5   
   -   5        $��txtAdditionalDay��5      $��0q�6*   $��1��65   $��1��6>   $��3��6N   $ 4��6[   $
��Left=�6|   $��Center=�6�   $��83��6�   �� 5   
   -	   5        $��lblHolidayType��5      $��0q�6&   $��Holiday Type:��61   $��4��6J   $
��Left=�6X   $	��Top=�6s   �� 5   
   -
   5        $��cmbHolidayType��5      $��0q�6)   $��1��64   $��4��6D   $ 5��6Q   $
��Left=�6r   $��Center=�6�   $	��250��6�   5       5!      