   M S B A M L   `   `   `  ����   Connect4Game    Connect4.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     �Name$  
GameWindow�� � 
ResizeMode$ NoResize=� � Title$ 4 en línea��$	��550��$	��800�� � 	Resources  �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers	 P� Property
 P� Value��%�   '��       �� " ��.���  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��� $��border��$	��0.5q�
���   �#��z��._��� $��Center=�$��Center=� P� Trigger��	 �@J�IsMouseOver
 . . ���  �@J�Opacity$ 1Q�.��� -    �Name$ MainGrid�� � ColumnDefinitions �� $��* � � RowDefinitions �� $��* �.+�� -   $ MenuGrid�� �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* � �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� -    ��Name$ Start��$��Jugar��
�������$��15(�$��3��$��3��
������� � Cursor$
 Hand��$��0q�#��[" �$��75��$��50���� -   $ 	SmallGrid��$��Pequeño��
�������$��15(�$��5��$��2��$
 Hand��$��0q�#��[" �$��75��$��50���� -   $ 
MediumGrid��$��Mediano��
�������$��15(�$��5��$��3��$
 Hand��$��0q�#��[" �$��75��$��50���� -   $ 	LargeGrid��$��Grande��
�������$��15(�$��5��$��4��$
 Hand��$��0q�#��[" �$��75��$��50���� -   $ 	OnePlayer��$��	1 Jugador��
�������$��15(�$��2��$��4��$
 Hand��$��0q�#��[" �$��75��$��50���� -	   $ 
TwoPlayers��$��2 Jugadores��
�������$��15(�$��4��$��4��$
 Hand��$��0q�#��[" �$��75��$��50��� -
   $ GameGrid��$��50q�$y�Hidden=�