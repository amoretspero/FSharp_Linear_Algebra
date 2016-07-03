#FSharp Linear Algebra  
Library for linear algebra made with F#.  
  
##Description  
This project is to provide F#-made linear algebra library.  
Support will mainly include matrix and vector computation.  
Also, most of objects will be generic, meaning you can use this library with types you want.
Currently, no optimization is provided.
  
##Supported Features  
  
###Matrix  
**Construction** : You can create matrix with three options.  
1) <code>(rowCnt : int, columnCnt : int, element : 'T [,])</code> - Basic constructor.  
2) <code>(rowCnt : int, columnCnt : int, zero : 'T)</code> - Zero matrix constructor.  
3) <code>(elem : 'T [] [])</code> - Constructor with Array2D.  
**Stringify** : <code>Format()</code> method.  
**Computation** : You can perform basic matrix computations.  
1) Addition  
2) Subtraction  
3) Multiplication  
4) Transpose  
5) Scalar multiplication  
6) Identity matrix  
7) Gauss Elimination (<code>decimal</code> type only)
  
###Vector  
**Construction** : You can create vector with one option.  
1) <code>(element : 'T [])</code>  
**Stringify** : Not implemented.  
**Computation** : You can perform basic vector computations.  
1) Addition  
2) Subtracion  
3) Inner Production  
