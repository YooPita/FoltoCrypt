   M S B A M L   `   `   `  ���� E  AFoltoCrypt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:FoltoCrypt.ClassesFoltoCrypt.Classes      FoltoCrypt.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:FoltoCrypt.Classes       � Title$  
FoltoCrypt��$	��350��$	��525�� � 	Resources "  FoltoCrypt.Classes.CostColor   	CostColor  IdStyle �� 
TargetType*  $System.Windows.Controls.DataGridCell �� Property �� Value �� 	Converter��%N   &	        &	      7     �� "  .���  � �� �� *Cost+8   .��� .+�o� $��25��$	��Top=�.@�m� $
��File�� � Cursor$
 Hand��.@�m� -   $	��New��$	��New��$
 Hand��m� -   $
��Open��$
 Hand���� m� -   $
��Save��$
��Save��$
 Hand��m� -   $��
Save As...��$
 Hand���� m� -   $
��Exit��$
 Hand���� $��30��$	��Top=�$��0,25,0,0q�.@��� -   $	��Add��$	��Add��$
��Left=�$��75���� -   $
��Edit��$
��Edit��$
��Left=�$��75���� -   $��Remove��$��Remove��$
��Left=�$��75���� -	    ��Name$ Refre��$��Refresh��$
��Left=�$��75��&   System.Windows.Controls.DataGrid  -
     Name$ dataGrid��$��	0,55,0,33q�	   SelectionMode$	 Single=�
   CanUserAddRows
 .     CanUserDeleteRows .     
IsReadOnly .    HorizontalGridLinesBrush
 �����   RowBackground
 �����   AlternatingRowBackground
 �����   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Id��   
Visibility$ Hidden=�1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Id+  $
 Name��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 100  �� *Name+  $ 
Investment��$ *  �� *
Investment+  $ Balance��$ *  �� *	Balance+  $
 Cost��$ *    	CellStyle# [  �� *Cost+  $ TrueBalance��$ *  �� *TrueBalance+�� $��33��$��Bottom=�.@��� -   $��
LabelTotIn��$�� ���� -   $��
LabelTotCo��$�� ���� -   $��
LabelTotBa��$�� ��