   M S B A M L   `   `   `  ����A  =Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ServerServer  5   	       Server.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Server     5   	     � Title$  
MainWindow��5   	   $	��550��6   $	��700��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ 	btnListen��5
      $��开始监听��6$   $	��Top=�63   $��
592,27,0,0q�6K   $
��Left=�6_   $��75��6z   6�   �� 5   
   -   5         ��Name$ 
txtContent��5      $
��Left=�6&   $	��157��6A    �� TextWrapping$
 Wrap=�6N   $	��Top=�6b   $	��643��6z   $��	24,75,0,0q�6�   �� 5   
   -   5        $ txtPort��5      $
��Left=�6#   $��23��6>   $
 Wrap=�6J   $��50000��6^   $	��Top=�6k   $��74��6�   $��
244,27,0,0q�6�   �� 5   
   -   5        $ txtIP��5      $
��Left=�6!   $��23��6<   $
 Wrap=�6H   $��	127.0.0.1��6\   $	��Top=�6m   $	��199��6�   $��	24,27,0,0q�6�   �� 5   
   -   5        $ 
txtSendMsg��5      $
��Left=�6&   $	��131��6A   $
 Wrap=�6N   $	��Top=�6b   $	��643��6z   $��
24,266,0,0q�6�   �� 5   
   -   5        $ 
btnSendMsg��5      $��发送消息��6%   $
��Left=�64   $	��Top=�6O   $��75��6g   $��515,480,0,0q�6r   6�   �� 5   
   -   5         ��Name$ cboUsers��5      $
��Left=�6%   $��
347,27,0,0q�6@   $	��Top=�6T   $	��223��6l   $��23��6x   .@�5      �� .��6   	127.0.0.16&   5      �� 5   
   -   5        $ btnShock��5      $��震动��6#   $
��Left=�60   $��610,480,0,0q�6K   $	��Top=�6`   $��57��6x   6�   �� 5   
   -	   5        $ txtFilePath��5      $
��Left=�6'   $��23��6B   $��
24,433,0,0q�6N   $
 Wrap=�6b   $	��Top=�6v   $	��470��6�   �� 5   
   -
   5        $ btnSendFile��5      $��发送文件��6&   $
��Left=�65   $��592,433,0,0q�6P   $	��Top=�6e   $��75��6}   6�   �� 5   
   -   5        $ btnChooseFile��5      $�� 选择��6(   $
��Left=�66   $��515,433,0,0q�6Q   $	��Top=�6f   $��55��6~   6�   5      5      