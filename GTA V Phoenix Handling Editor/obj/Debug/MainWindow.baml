   M S B A M L   `   `   `  ����X  TGTA V Phoenix Handling Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:GTA_V_Phoenix_Handling_EditorGTA_V_Phoenix_Handling_Editor  5   	   .    (GTA_V_Phoenix_Handling_Editor.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:GTA_V_Phoenix_Handling_Editor     5   	   -   5          � Title$  Phoenix Handling Editor��5   	   $��650.383��6)   $��1216.595��6:    � WindowStartupLocation$ CenterScreen=�6K   ���	#FF68708B.��5	      � ���	#FF5B7A8F.+�5
   
   �� -   5         ��Name$ openFile_btn��5
      $��	Open File��6'   $
��Left=�6;   $��	10,10,0,0q�6V   $	��Top=�6i   $��75��6�   6�   �� 5   
   -   5         ��Name$ openFile_label��5      $�� ��6(   $
��Left=�63   $	��Top=�6N   $��90,7,0,0q�6f   ���	#FF85D6DE6x   �� 5   
   -   5         ��Name$ xmlModelName_input��5      $
��Left=�6.   $��22��6I   $��	10,42,0,0q�6U    �� TextWrapping$
 Wrap=�6h   $	��Top=�6|   $	��120��6�   ���	#FFA6B4DC
�������$��Center=�6  �� 5   
   -   5         ��Name$ gridScrollViewer��5      $
��Auto=�61   $
��Auto=�6U   $��0,69,0,0q�6z   .��5      � -   5        $��	nodesGrid��5      ���	#FF202C3C � ShowGridLines . $
��auto��6P   $��Visible=�6]   ��.  J� RenderTransformOrigin$ 0.5,0.5&�6�   	 �� Left$	 10��6�   }�. 
 � ColumnDefinitions
 5      �� 5      �� 5      �� 5       � RowDefinitions 5      �� 5      $��45 �6$   �� 5      $��45 �6$   .+�5      �� $��20(�6   $��0��6&   $��0��63   $
��Left=�6C   $��Center=�6^   ���	#00000000
�������.��6�   Key6�   �� 5      $��20(�6   $��0��6&   $��1��63   $
��Left=�6C   $��Center=�6^   
�������.��6�   Value6�   �� 5      $��20(�6   $��0��6&   $��2��63   $
��Left=�6C   $��Center=�6^   $	��0,4q�6y   
�������.��6�   
Attributes6�   5      5      5      5      