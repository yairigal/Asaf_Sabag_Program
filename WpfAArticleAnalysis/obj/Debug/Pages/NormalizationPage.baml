   M S B A M L   `   `   `  ����N  JWpfAArticleAnalysis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:WpfAArticleAnalysis.PagesWpfAArticleAnalysis.Pages  5      1    +WpfAArticleAnalysis.Pages.NormalizationPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:WpfAArticleAnalysis.Pages     5        N� Title$  NormalizationPage��5	      $	��482��6!   .��5      �� .+�5   
   � .+�5      �� -   5         ��Name$ textBox��5      $
��Left=�6*   $	��119��6E   $��	56,10,0,0q�6Q   $
��Wrap=�6d   $
��Bold"�6x   $��Text Capitalization:��6�   $	��Top=�6�   �� 5      -   5         ��Name$ 	LettersCB��5      $
��Left=�6*   $��275,10,50,10q�6E   $	��119��6[    �� SelectedIndex$ 2��6g   5      � 5   
   .+�5      �� -   5         ��Name$ PunRB��5      $��RadioButton��6&   $
��Left=�6<   $��275,10,0,10q�6W   $	��176��6l   �� 5      -   5        $ 	textBlock��5      $
��Left=�6+   $��	56,10,0,0q�6F   $
��Wrap=�6Y   $
��Bold"�6m   $��Remove Punctuation?��6   $	��Top=�6�   $	��129��6�   5      � 5   
   .+�5      �� -   5        $ HTMLRB��5      $��RadioButton��6'   $
��Left=�6=   $��275,10,0,10q�6X   $	��182��6m   �� 5      -   5        $ 
textBlock2��5      $
��Left=�6,   $��	56,10,0,0q�6G   $
��Wrap=�6Z   $
��Bold"�6n   $��Remove HTML Tags?��6�   $	��Top=�6�   $	��129��6�   5      � 5   
   .+�5      �� -   5        $ TakeOutStopWords��5      $��No Stop Words��61   $
��Left=�6I   $��275,10,0,10q�6d   $	��207��6y   �� 5      -   5        $ 
textBlock3��5      $
��Left=�6,   $��	56,10,0,0q�6G   $
��Wrap=�6Z   $
��Bold"�6n   $��Remove Stop Words?��6�   $	��Top=�6�   $	��129��6�   5      � 5   
   .+�5      �� -	   5        $ SaveNormalizations��5      $
��Left=�63   $��275,10,0,10q�6N   $	��207��6c    d� 	IsChecked$
 TrueY�6o   �� 5      -
   5        $ 
textBlock4��5      $
��Left=�6,   $��	56,10,0,0q�6G   $
��Wrap=�6Z   $
��Bold"�6n   $)��#Save Normalized File For Next Runs?��6�   $	��Top=�6�   $	��129��6�   5      5"      5#      