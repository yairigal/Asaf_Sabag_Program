   M S B A M L   `   `   `  ���� N  JWpfAArticleAnalysis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:WpfAArticleAnalysis.PagesWpfAArticleAnalysis.Pages  1    +WpfAArticleAnalysis.Pages.NormalizationPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     9local'clr-namespace:WpfAArticleAnalysis.Pages       N� Title$  NormalizationPage��$	��482��.���� .+�� .+��� -    ��Name$ textBox��$
��Left=�$	��119��$��	56,10,0,0q�$
��Wrap=�$
��Bold"�$��Text Capitalization:��$	��Top=��� -    ��Name$ 	LettersCB��$
��Left=�$��275,10,50,10q�$	��119�� �� SelectedIndex$ 2��� .+��� -    ��Name$ PunRB��$��RadioButton��$
��Left=�$��275,10,0,10q�$	��176���� -   $ 	textBlock��$
��Left=�$��	56,10,0,0q�$
��Wrap=�$
��Bold"�$��Remove Punctuation?��$	��Top=�$	��129��� .+��� -   $ HTMLRB��$��RadioButton��$
��Left=�$��275,10,0,10q�$	��182���� -   $ 
textBlock2��$
��Left=�$��	56,10,0,0q�$
��Wrap=�$
��Bold"�$��Remove HTML Tags?��$	��Top=�$	��129��� .+��� -   $ TakeOutStopWords��$��No Stop Words��$
��Left=�$��275,10,0,10q�$	��207���� -   $ 
textBlock3��$
��Left=�$��	56,10,0,0q�$
��Wrap=�$
��Bold"�$��Remove Stop Words?��$	��Top=�$	��129��� .+��� -	   $ SaveNormalizations��$
��Left=�$��275,10,0,10q�$	��207�� d� 	IsChecked$
 TrueY��� -
   $ 
textBlock4��$
��Left=�$��	56,10,0,0q�$
��Wrap=�$
��Bold"�$)��#Save Normalized File For Next Runs?��$	��Top=�$	��129��