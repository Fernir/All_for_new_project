   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   T  F BWoWRelogger, Version=1.1.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Alex.WoWRelogger.ControlsAlex.WoWRelogger.Controls 6�  I)clr-namespace:Alex.WoWRelogger.ConvertersAlex.WoWRelogger.Converters 6�  3clr-namespace:Alex.WoWReloggerAlex.WoWRelogger 6�  8   2Alex.WoWRelogger.Controls.AccountConfigUserControl   6   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    6W   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6�   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6�   @d2http://schemas.microsoft.com/expression/blend/2008     6  6sys&clr-namespace:System;assembly=mscorlib     6T  <Controls'clr-namespace:Alex.WoWRelogger.Controls     6�  @
Converters)clr-namespace:Alex.WoWRelogger.Converters     6�  4	HighVoltzclr-namespace:Alex.WoWRelogger     6�  -   5        $
��root��5   K     � 	Resources  5      4 .Alex.WoWRelogger.Converters.SpacifierConverter   SpaceConverter8 2Alex.WoWRelogger.Converters.TaskIsRunningConverter  TaskIsRunningConverter��%:   &	        &	        5      6$     5      6(   5      .��5      � .+�5      �� -   5        $��AccountTabControl��5      $��Stretch=�6*   $��Stretch=�6H   $��4q�6d   $��Center=�6p   .@�5	      �� -   5        $��	AcountTab��5	      $��Account��6!   $��Stretch=�62   $��Stretch=�6W    � DataContext 6z   ��  �� Path$ 	/Settings�.��5
   
   �� -   5        $��GameConfigBorder��5
       �� Padding$ 1q�6)   $��1q�65   ���	#FF808080 �� CornerRadius$ 1��6a   $��0q�6r   ._�5      �� $
��Auto=�6   $��	-3,3,3,10q�6I   .��5      �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5       $��15.151 �6#   �� 5!      $��53.849 �6#   �� 5"      $��19 �6#   �� 5#      $��* �6#   �� 5$      $��12 �6#   .+�5&      �� $��Profile Name��6   $
��Left=�6.   $��20��6I   $��5,0,0,1q�6U   $��3��6f   �� 5'      -   5        $��ProfileNameText��5'      $��3��6/    � ToolTip$n hThis can be anything, preferably something relevant and easy to remember. This will be used in log files��6?   ��6�   �� $ ProfileName��� 5(      $��	Character��6   $��6��6+   $
��Left=�68   $��20��6S   $��5,1,0,0q�6_   $��2��6p   �� 5)      -   5        $��CharacterText��5)       �� 	MaxLength$ 10��6-   $��3��6<   $��6��6L   $, &This is the name of your WoW Character��6Y   ��6�   �� $ WowSettings.CharacterName��� 5*      $��Server��6   $��7��6(   $
��Left=�65   $��20��6P   $��5,1,0,0q�6\   $��2��6m   �� 5+      -   5        $��
ServerText��5+      $��3��6*   $��7��6:   $@ :Name of the server/realm that your character is located on��6G   ��6�   �� $ WowSettings.ServerName��� 5,      $��
Wow Region��6   $��8��6,   $
��Left=�69   $��20��6T   $��5,1,0,0q�6`   $��3��6q   �� 5-      -   5        $��RegionCombo��5-      $��3��6,   $��8��6<   $ The Wow regional area��6I   6i   �� 5.      $��Faction��6   $��9��6)   $
��Left=�66   $��20��6Q   $��5,1,0,0q�6]   $��3��6n   �� 5/      -	   5        $��FactionCombo��5/      $��3��6-   $��9��6=   $ The faction��6J   $��0,2,0,20q�6`   $��2��6r   6�   �� 50      $��WoW Exe Path��6   $��10��6.   $
��Left=�6<   $��20��6W   $��5,1,0,0q�6c   $��3��6t   ,  &Alex.WoWRelogger.Controls.FileInputBox  51      -
   5        	  Name$	 WoWFileInput��51   &   ,  &Alex.WoWRelogger.Controls.FileInputBox
   FileNameChanged$!
 WoWFileInputFileNameChanged��6<   $��10��6j   $��3��6x   $��2��6�      Title$, &Browse to and select your WoW.exe file��6�      
DefaultExt$
 .exe��6�      Filter$ 
.exe|*.exe��6�   $� }The path to Wow.exe. click the '...' button on the right to browse to it. This can also be a batch file that launches WoW.exe��6�   �� 52      $��Params��6   $��12��6(   $
��Left=�66   $��20��6Q   $��5,1,0,0q�6]   $��3��6n   �� 53      -   5        $��Params��53      $��3��6&   $��12��66   $, &This is the name of your WoW Character��6D   ��6u   �� $ WowSettings.Params�54      55      56      57   	   �� 58      $��Window��6   $��Stretch=�6    $��Stretch=�6E    6h   �� $ 	/Settings�.��59   
   �� $ 2q�6   $��1q�6   ���	#FF808080$ 3��6I   ._�5:      �� $
��Auto=�6   .��5;      �  5<      �� 5=      $
��Auto �6    �� 5>      $
��Auto �6    �� 5?      $
��Auto �6    �� 5@      $
��Auto �6    �� 5A      $
��Auto �6    �� 5B      $
��Auto �6    �� 5C      $
��Auto �6     5E      �� 5F      $
��Auto �6#   �� 5G      $��* �6#   .+�5I      �� $��WoW Window X��6   $
��Left=�6.   $��20��6I   $��5,0,5,0q�6U   �� 5J      -   5        $��WowWindowXText��5J      $��1��6.   $K EThe X screen location where you want the Wow window placed (optional)��6>   ��6�   �� $ WowSettings.WowWindowX��� 5K      $��WoW Window Y��6   $��1��6.   $
��Left=�6;   $��20��6V   $��5,0,5,0q�6b   �� 5L      -   5        $��WowWindowYText��5L      $��1��6.   $��1��6>   $K EThe Y screen location where you want the Wow window placed (optional)��6K   ��6�   �� $ WowSettings.WowWindowY��� 5M      $��WoW Window Width��6   $��2��62   $
��Left=�6?   $��20��6Z   $��5,0,5,0q�6f   �� 5N      -   5        $��WowWindowWidthText��5N      $��1��62   $��2��6B   $% The Wow window Width (optional)��6O   ��6y   �� $  WowSettings.WowWindowWidth��� 5O      $��WoW Window Height��6   $��3��63   $
��Left=�6@   $��20��6[   $��5,0,5,0q�6g   �� 5P      -   5        $��WowWindowHeightText��5P      $��1��63   $��3��6C   $&  The Wow window Height (optional)��6P   ��6{   �� $! WowSettings.WowWindowHeight��� 5Q      $��Grab window size/location��6   $��4��6;   $
��Left=�6H   $��20��6c   $��5,0,5,0q�6o   �� 5R      -   5        $��WowWindowGrabText��5R      $
��Grab��60   $��1��6?   $��4��6O   $H BGrabs the Wow window size and location, requires wow to be running��6\   6�   �� 5S      $��5��6    �� Orientation$ 
Horizontal=�6(   $[ UAutomatically re-sizes the Wow window to the entered ratio of the monitor that its on��6A   .+�5T      �� $��Resize to 1/��6   $
��Left=�60   $��20��6K   $��5,0,5,0q�6W   �� 5U      -   5        $��WowWindowRatioText��5U      $��3��64   �� 5V      $��ratio of screen��6   $
��Left=�63   $��20��6N   $��5,0,5,0q�6Z   5W      �� 5X      -   5        $��WowWindowRatioButton��5X      $��5��63   $��1��6@   $��Resize��6P   $[ UAutomatically re-sizes the Wow window to the entered ratio of the monitor that its on��6a   6�   5Y      5Z      5[      5\   	   5]      5^      5_      