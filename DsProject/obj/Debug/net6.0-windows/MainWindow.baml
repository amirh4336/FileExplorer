   M S B A M L   `   `   `  ����D  @DsProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:FileExplorer.ViewModelsFileExplorer.ViewModels  5   	   =#clr-namespace:FileExplorer.MWM.ViewFileExplorer.MWM.View  5   	   +clr-namespace:FileExplorerFileExplorer  5   	       FileExplorer.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <
viewmodels%clr-namespace:FileExplorer.ViewModels     5   	   <userControls#clr-namespace:FileExplorer.MWM.View     5   	   ,localclr-namespace:FileExplorer     5   	   -   5        $	��600��5
   	   $	��920��6     � 
ResizeMode$  NoResize=�5   	   
������  � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers	 P� Property
 P� Value��%  '��       �� 5   
   " ��6   .�5      ��  � �� �#2222226+   �� 5       � �� �#EEEEEE6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   ._�5      �� $��Center=�6/   $��Center=�6L   #��z��6g   5      5      5       5      P� 5      	 �@J�IsMouseOver$

 True��61   . �5       ��  � �� �#0042C163   5!      P� 5"      	 � ��$

 True��6/   . �5#      ��  � ��
 � � �63   5$      5&      5'       � DataContext 5)      +   %FileExplorer.ViewModels.MainViewModel  5*   
   .��5.      �� ���#272537 �� CornerRadius$ 20��6"   ._�50   
   �  � ColumnDefinitions 51      �� 52      $	��200 �6#   �� 53       � RowDefinitions 56      �� 57      $��75 �6    �� 58      .+�5:      �� $��FileExplorer��6   $��/Fonts/#Poppins)�6,   $��Center=�6I   $
��Left=�6d   $��22, 0 , 0 ,0q�6   
�������$��22(�6�   � 5<      $��1��6    5=      �� 5>      �� 5?      .+�5A      �� -   5         ��Name$ dynamicFileSystem��5A   *   $��0��6   5B      �� 5C      -   5        $ dynamicVolumes��5C   *   $��1��6   6D   5D      � 5E      $��1��6   $��0��6#   $��2��60   $��5q�6A   .+�5F      �  5G      �� 5H      �� 5I      .+�5K      �� $��0��6   $��
0 ,50, 0,0q�6+   
������ $��Stretch=�5L      ��5M      �� *FileItemsSystem+ �� UpdateSourceTrigger$ PropertyChanged=��� 5Q      $��1��6   $��
0 ,50, 0,0q�6+   
������ $��Stretch=�5R      ��5Q   X   �� *	FileItems+$ PropertyChanged=�5R      5T      5U      �� 5V      -   5        $��txtDir��5V         ModernTextbox#��[  5Y      $
��Left=�5Z      $��118,230,0,0q�61    �� TextWrapping$
 Wrap=�6F   $	��Top=�6Z   $	��225��6r   $��1��6~   $��1��6�   $��41��6�   ��5X      �� *Path+$ PropertyChanged=�5Z   �   �� 5[      -   5        $��btnBack��5[      $
��back��6:   $
��Left=�6I   $��
37,243,0,0q�6d   $	��Top=�6x    J� RenderTransformOrigin$ -0.517,-0.107&�6�   $��1��6�   $��1��6�   �� 5\      -   5        $��btnNext��5\      $
��Next��6:   $
��Left=�6I   $��
93,243,0,0q�6d   $	��Top=�6x   $ -0.517,-0.107&�6�   $��1��6�   $��1��6�   �� 5]      -   5        $��
addPartion��5]      $��add Partion��6'   $��112,52,10,5q�6V   �� 5^      -   5         ��Name$ 	addFolder��5^      $��
add Folder��6(   $��265,20,377,37q�6U   $��1��6l   �� 5_      -	   5        $ delete��5_      $��delete��6:   $��477,20,194,37q�6K   $��1��6b   �� 5`      -
   5        $	 cut��5`      $	��cut��64   $��540,21,145,36q�6B   $��1��6Y   �� 5a      -   5        $
 copy��5a      $
��copy��66   $��588,20,97,37q�6E   $��1��6[   �� 5b      -   5        $ paste��5b      $��paste��68   $��637,20,48,37q�6I   $��1��6_   �� 5c      -   5        $ btnClose��5c   %   $��1��6   $��32��67   $��26��6B   $��X��6N   $
��Left=�6Z   $��	683,3,0,0q�6u   $	��Top=�6�   6�   �� 5d      -   5         ��Name$ txtDirFileSystem��5d      #��[  5f      $
��Left=�5g      $��35,7,0,0q�6,   $
 Wrap=�6>   $	��Top=�6R   $	��225��6j   $��1��6v   $��42��6�   ��5e      �� *	PathSys+$ PropertyChanged=�5g   �   �� 5h      -   5        $ btnBackFileSystem��5h      $
��back��6P   $
��Left=�6_   $��	10,20,0,0q�6z   $	��Top=�6�   $ -0.517,-0.107&�6�   $��1��6�   �� 5i      -   5        $ 
AddFileTxt��5i      $��add File txt��6)   $��365,32,256,25q�6Y   $��1��6p   �� 5j      -   5        $ 
ImportFile��5j      $��import file��6)   $��365,9,256,48q�6X   $��1��6n   �� 5k      -   5        $ 
ImportTree��5k      $��import Tree��6)   $��10,52,112,5q�6X   5l      5n      5p      