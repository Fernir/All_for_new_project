   M S B A M L   `   `   `  ����F  BWoWRelogger, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Alex.WoWReloggerAlex.WoWRelogger  5	   	   E'clr-namespace:Alex.WoWRelogger.ControlsAlex.WoWRelogger.Controls  5
   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   !    Alex.WoWRelogger.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   0localclr-namespace:Alex.WoWRelogger     5	   	   <Controls'clr-namespace:Alex.WoWRelogger.Controls     5
   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9system&clr-namespace:System;assembly=mscorlib     5   	   -   5          � Title$  WoWRelogger��5   	   $	��400��5   	   $	��750��6   $	��750��6"   $	��400��5   	    � Icon$ Images/Logo.ico��6    � DataContext 5   	   �� *CharacterProfiles+%  Alex.WoWRelogger.HbRelogManager   Settings �� Source# Z  � TaskbarItemInfo 5      *  $System.Windows.Shell.TaskbarItemInfo  5         ThumbButtonInfos 5   
   *  $System.Windows.Shell.ThumbButtonInfo  -   5        StartThumbButton��   Description$ 	Start All��5   4      ImageSource$ Images/play1pressed.png��6L   6r     5   
   -   5        PauseThumbButton��$ 	Pause All��5   4   $ Images/pausehot.png��6L   6n     5   
   -   5        StopThumbButton��$ Stop All��5   3   $ Images/stop1normalred.png��6J   6r   5      .��5      � 	 � RowDefinitions	 5      �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      -   5        
 �Name$
 LogRow��5      $
��Auto �6$   .+�5      �� $��2q�6   ._�5      �  � ColumnDefinitions 5    
   �� 5!      $
��Auto �6   �� 5"      $��* �6   �� 5#      $��* �6   �� 5$      $��* �6   �� 5%      $��* �6   �� 5&      $��* �6   �� 5'      $��* �6   .+�5)   
   �� -   5        $��SelectAllButton��5)      ��5*      � 5+       5,      �� 5-      $��24 �6#   �� 5.      $��* �6#   .+�50      �� $ ��Images/edit_select_all.png��6   $��24��6:   �� 51      $��
Select All��6   $	��4,0q�6,   $��Center=�69   $
��Wrap=�6T   $
��Auto��6h   $��1��6u   52      54      �� 55   
   -   5        $��StartButton��55      $��1��6$   ��56      � 57       58      �� 59      $��24 �6#   �� 5:      $��* �6#   .+�5<      �� $��Images/play1pressed.png��6   $��24��67   �� 5=      $��Start��6   $	��4,0q�6'   $��Center=�64   $��1��6O   $
��Wrap=�6_   $
��Auto��6s   5>      5@      �� 5A   
   -   5        $��PauseButton��5A      $��2��6$   ��5B      � 5C       5D      �� 5E      $��24 �6#   �� 5F      $��* �6#   .+�5H      �� $��Images/pausehot.png��6   $��24��63   �� 5I      $��Pause��6   $	��4,0q�6'   $��Center=�64   $��1��6O   $
��Wrap=�6_   $
��Auto��6s   5J      5L      �� 5M   
   -	   5        $��
StopButton��5M      $��3��6#   ��5N      � 5O       5P      �� 5Q      $��24 �6#   �� 5R      $��* �6#   .+�5T      �� $��Images/stop1normalred.png��6   $��24��69   �� 5U      $
��Stop��6   $	��4,0q�6&   $��Center=�63   $��1��6N   $
��Wrap=�6^   $
��Auto��6r   5V      5X      �� 5Y   
   -
   5        $��	AddButton��5Y      $��4��6"   ��5Z      � 5[       5\      �� 5]      $��24 �6#   �� 5^      $��* �6#   .+�5`      �� $'��!Images/112_Plus_Blue_32x32_72.png��6   $��24��6A   �� 5a      $	��Add��6   $	��4,0q�6%   $��Center=�62   $��1��6M   $
��Wrap=�6]   $
��Auto��6q   5b      5d      �� 5e   
   -   5        $��
EditButton��5e      $��5��6#   ��5f      � 5g       5h      �� 5i      $��24 �6#   �� 5j      $��* �6#   .+�5l      �� $��Images/Settings.png��6   $��24��63   �� 5m      $
��Edit��6   $	��4,0q�6&   $��Center=�63   $��1��6N   $
��Wrap=�6^   $
��Auto��6r   5n      5p      �� 5q   
   -   5        $��	DelButton��5q      $��6��6"   ��5r      � 5s       5t      �� 5u      $��24 �6#   �� 5v      $��* �6#   .+�5x      �� $��Images/delete.png��6   $��24��61   �� 5y      $��Delete��6   $	��4,0q�6(   $��Center=�65   $��1��6P   $
��Wrap=�6`   $
��Auto��6t   5z      5|      5}   	   5~      &   System.Windows.Controls.DataGrid  5      -   5        $��AccountGrid��5      $ �� IsSynchronizedWithCurrentItem$
 TrueY�6"      GridLinesVisibility$ Vertical=�6G   $��1��6f      CanUserAddRows .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserSortColumns .     AutoGenerateColumns .     CanUserResizeRows .  $
��Left=�6  $��Disabled=�68  ��6n  ��    Columns 5�      4  .System.Windows.Controls.DataGridTemplateColumn  5�   
   ,  &System.Windows.Controls.DataGridColumn   Header$ *��6!      CellTemplate 5�      �� 5�      .R�5�      �� $��ProfileEnabledCheckBox��6    d� 	IsChecked 67   �� *Settings.IsEnabled+
 � Tag 6`   �� *
Settings+5�      5�      0  *System.Windows.Controls.DataGridTextColumn  5�   
   $ Profile��6      MinWidth$	 100Q�6.      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6O   �� *Settings.ProfileName+  5�   
   $ Server��6   $ 70Q�6-    .  6M   �� *!Settings.WowSettings.ServerName+  5�   
   $ 	Character��6   $ 70Q�60    .  6P   �� *$"Settings.WowSettings.CharacterName+  5�   
   $ Status��6      Width5	  /System.Windows.Controls.DataGridLengthConverter$ *	 6-    .  6I   �� *Status+   RowStyle 5�      �� 5�       �� 
TargetType)
  #System.Windows.Controls.DataGridRow" 
 6    �� BasedOn# [B
 6:   .�5�      ��   �� Property  �@^�ToolTip! �� Value! 65   �� *	Tooltip+�� 5�        �@^�InitialShowDelay$! 0��6>   �� 5�        �@^�ShowDuration$! 10000��6:   �� 5�        �@�ContextMenu! 5�      �� 5�      $��RowContextMenu��6   .@�5�      m� $��SkipMenu��6   $��Skip Current Task��6+    6F   �� ��5�      �� 5�   *   " m�60   # [Bm�6O   5�      m� 5�      $��MaximizeWowMenu��6   $��Maximize Wow��62    6H   �� ��5�      �� 5�   *   " m�60   # [Bm�6O   5�      m� 5�      $��BringHbToForegroundMenu��6   $��Bring HB To Foreground��6:    6Z   �� ��5�      �� 5�   *   " m�60   # [Bm�6O   5�      m� 5�      $��
KillHBMenu��6   $��Kill HB��6-    6>   �� ��5�      �� 5�   *   " m�60   # [Bm�6O   5�      5�      5�      5�      5�      � 5�      -   5        $��OptionsAndLogGrid��5�      $��2��6$   $��20��61   " � 	Resources" 5�      ��%�   '��       7���� 5�   
   " ��6)   8   .�5�      ��   � ��$! 20��66   �� 5�        � ��$! 0q�6.   �� 5�        � ��$! 0q�66   �� 5�        � ��! �	#FF00000061   5�      5�   	   .+�5�      �� -   5        $��OptionsAndLogTabCtrl��5�      $��2��6/   $	��200��6<   .@�5�   
   �� -   5        $��
OptionsTab��5�      $��Stretch=�6$   $��Stretch=�6G   $��Stretch=�6l   ��5�      �� 5�      $��Options��6   $	��-10q�6'   # �� Padding$
# 15,0q�64   .��5�      2   ,Alex.WoWRelogger.Controls.OptionsUserControl  -   5        $  Name$$ HbrelogOptions��5�   (   # Z 6@   5�      �� 5�   
   -   5        $��Stretch=�5�      ��5�      �� 5�      $	��Log��6   $	��-10q�6#   $
# 15,0q�60   .��5�      �� -   5        $��
LogTextBox��5�      $��2��6*   $��Stretch=�67   $��Stretch=�6U   % �� 
IsReadOnly% . $& �� HorizontalScrollBarVisibility$
& Auto=�6�   "' �� VerticalScrollBarVisibility$
' Auto=�6�   $��0q�6�   ( �� AcceptsReturn( .  ) J� 	AllowDrop) .  * �� AutoWordSelection* .  $��Consolas)�6  5�      5�   	   5�      � 5�      -   5        $��AccountConfigGrid��5�      $��0,4,-8,4q�6$   $��1��66   $��2��6C   $	��300��6T   $	��Top=�6d   $��0��6|   $��Right=�6�    5�      �� 5�   
   �� 5�   
   -   5        $
 AccountConfigGridColumn��5�      $	��275 �6:   $	��275��6F   	 5�      �� 5�   
   �� 5�   
   $��25 �6   .+�5�      �� $��1��6   $��2��6   $��2q�60   + �� CornerRadius$+ 4��6D   8   2Alex.WoWRelogger.Controls.AccountConfigUserControl  5�      -   5        ,  Name$, AccountConfig��5�   *   $��1��6A   $��4q�6Q   �� 5�      -   5        $��AccountConfigCloseButton��5�      $��Save and Close��6/   $��1��6H   $
��Left=�6X   $��4,0,4,4q�6s   $��1��6�   $��11(�6�   6�   5�      5�      5�      