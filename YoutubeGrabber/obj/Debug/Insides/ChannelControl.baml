   M S B A M L   `   `   `  ����  YoutubeGrabber#    YoutubeGrabber.ChannelControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5          >�Name$  UserControl��5       � Cursor$
 Hand��5	   +   $��0,0,0,2q�69   .��5      �� -   5         ��Name$ 
LayoutRoot��5   
   $��0q�6   ���	#FFBEF0BE$��18��6@   '  !System.Windows.VisualStateManager   VisualStateGroups 5      %  System.Windows.VisualStateGroup  5      -   5        VisualStateGroup��   States. 5         System.Windows.VisualState  -   5        Selected��   
Storyboard. 5      �� .��5      T�  �� TargetProperty$ (Border.BorderBrush)�6&    �� 
TargetName$ 
LayoutRoot��6W   ..�5   	   i�  R� KeyTime$ 0��6    	 R� Value	 5   
   �� 5      
 �� Color$
 	#FF323232��6   5   
   5   	   r� 5      $ (Border.BorderThickness)�6)   $ 
LayoutRoot��6^   .	�5   	   <  6System.Windows.Media.Animation.EasingThicknessKeyFrame   p� KeyTime$ 0��6!    p� Value$ 1q�6-   5   	   r� 5      $ (FrameworkElement.Margin)�6)   $ 
LayoutRoot��6_   .	�5   	     $ 0��6!   $	 0,3q�6-   5   	   5      5        5      -   5        Default��5      5       ._�5"      ��  �� Orientation$ 
Horizontal=�6   $��3,0,0,0q�6(   .+�5#      �� -   5         ��Name$
 subs��5#      $��(5.000.000)��6   $��0q�6/   $��Center=�6:   $
��Left=�6U   $��0q�6p   $��9.333(�6|   ���	#FF5957576�   �� 5$      -   5        $
 Text��5$      $��Label��6   $��0q�6)   $	��Top=�64   $
��Left=�6L   $��3,0,0,0q�6g   5%      5&      5'      