   M S B A M L   `   `   `  ����I  EMasterFloorApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterFloorApp.PagesMasterFloorApp.Pages  5      &     MasterFloorApp.Pages.HistoryPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:MasterFloorApp.Pages     5        N� 
Background
  ����� N� Title$ HistoryPage��5	      .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      $��50 �6   6*   .+�5   
   �� -   5         ��Name$ HistoryListView��5      $��Center=�6,   ��5      �� 5      .R�5      �� 
���   �$��1q�61   $��5q�6E   ._�5      � $��10q�6   $	��600��6+   $	��100��67   .+�5      �� $��Center=�6)   .+�5   "   �� $��Medium"�6,   $
��Wrap=�6@   �� 5   &   $2�,Наименование продукции:��6*   6K    �� 5   &   �6*   �� *Products.Name+6K   5   #   �� 5   "   �� 5   &   $�Количество:��6*   6?    �� 5   &   �6*   �� *
Countity+6F    �� 5   &   $�шт.��6*   67   5   #   �� 5    "   �� 5!   &   $�Дата продажи:��6*   6A    �� 5"   &   �6*   �� *RealisationDate+ �� StringFormat$ d��6]   5#   #   5$      5%      5&      5'      5)      �� 5+   
   -   5         ��Name$ 
BackButton��5+   [   $��1��6   $
��Left=�6   $��
Назад��69   $��10 0 0 0q�6I   6�   5,      5-      