#FSharp Linear Algebra  
Library for linear algebra made with F#.  
  
##Version  
Current : **0.2.1.1**  
Status : **Stable**  
  
##Update History  
  
##Description  
This project is to provide F#-made linear algebra library.  
Support will mainly include matrix and vector computation.  
Also, most of objects will be generic, meaning you can use this library with types you want.  
Currently, no optimization is provided.
  
##Supported Features  
  
###Matrix  
**Construction** : You can create matrix with three options.  
1) Basic constructor - <code>(rowCnt : int, columnCnt : int, element : 'T [,])</code>  
2) Zero matrix constructor - <code>(rowCnt : int, columnCnt : int, zero : 'T)</code>  
3) Constructor with Array2D. - <code>(elem : 'T [] [])</code>  
```
// First constructor. Should provide type.
let matrix1 = matrix<int>(3, 7, (Array2D.init 3 7 (fun idx1 idx2 -> idx1 * idx2 - 1)))

// Second constructor. Should provide zero for type of matrix.
let matrix2 = matrix(3, 3, 0)

// Third constructor. Should provide type.
let matrix3 = matrix<float>([| [| 1.334; 8.461; 9.361; 2.904; 5.837 |]; [| 0.948; 8.847; 6.372; 4.981; 7.829 |]; [| 1.938; 3.284; 8.944; 5.748; 2.987 |] |])
```  

**Stringify** : <code>Format()</code> method.  
```
let mat = matrix<float>([| [| 1.334; 8.461; 9.361; 2.904; 5.837 |]; [| 0.948; 8.847; 6.372; 4.981; 7.829 |]; [| 1.938; 3.284; 8.944; 5.748; 2.987 |] |])
mat.Format() |> printfn "Matrix formatted: \n%s"
(*
Output will be:

Matrix formatted: 
1.334 8.461 9.361 2.904 5.837
0.948 8.847 6.372 4.981 7.829
1.938 3.284 8.944 5.748 2.987

*)
```  

**Computation** : You can perform basic matrix computations.  
1) Addition - <code>Matrix.Add()</code>  
2) Subtraction - <code>Matrix.Subtract()</code>  
3) Multiplication - <code>Matrix.Multiply()</code>  
4) Transpose - <code>Matrix.Transpose()</code>  
5) Scalar multiplication - <code>Matrix.ScalarMultiply()</code>  
6) Identity matrix - <code>Matrix.Identity()</code>  
7) Gauss Elimination - <code>Matrix.GaussEliminate()</code>  
```
// Matrix initialization.
let matrix1 = matrix<decimal>([| [| 2M; 1M; 1M; |]; [| 4M; -6M; 0M; |]; [| -2M; 7M; 2M; |] |])
let matrix2 = matrix<decimal>([| [| 1M; 1M; 1M; |]; [| 2M; 2M; 5M; |]; [| 4M; 6M; 8M; |] |])

// Addition
let matrixAdd = Matrix.Add matrix1  matrix2
matrixAdd.Format() |> printfn "matrixAdd: \n%s"

// Subtraction
let matrixSubtract = Matrix.Subtract matrix1 matrix2
matrixSubtract.Format() |> printfn "matrixSubt: \n%s"

// Multiplication
let matrixMultiply = Matrix.Multiply matrix1 matrix2
matrixMultiply.Format() |> printfn "matrixMultiply: \n%s"

// Transpose
let matrixTranspose = Matrix.Transpose matrix1
matrixTranspose.Format() |> printfn "matrixTranspose: \n%s"

// Scalar multiplication
let matrixScalarMultiply = Matrix.ScalarMultiply 1.5M matrix1
matrixScalarMultiply.Format() |> printfn "matrixScalarMultiply: \n%s"

// Identity matrix
let matrixIdentity = Matrix.Identity 3 1.0
matrixIdentity.Format() |> printfn "matrixIdentity: \n%s"

// Gauss elimination
let matrixGaussEliminate = Matrix.GaussEliminate matrix1
matrixGaussEliminate.Format() |> printfn "matrixGaussEliminate: \n%s"

(*
Output: 

matrixAdd: 
3	2	2	
6	-4	5	
2	13	10	

matrixSubt: 
1	0	0	
2	-8	-5	
-6	1	-6	

matrixMultiply: 
8	10	15	
-8	-8	-26	
20	24	49	

matrixTranspose: 
2	4	-2	
1	-6	7	
1	0	2	

matrixScalarMultiply: 
3.0	1.5	1.5	
6.0	-9.0	0.0	
-3.0	10.5	3.0	

matrixIdentity: 
1	0	0	
0	1	0	
0	0	1	

matrixGaussEliminate: 
2	1	1	
0	-8	-2	
0	0	1

*)
```
  
###Vector  
**Construction** : You can create vector with one option.  
1) Basic constructor - <code>(element : 'T [])</code>
```
// Basic constructor. Should provide type.
let vector1 = vector<int>([| 1; 2; 3; 4; 5 |])
```  

**Stringify** : <code>Format()</code> method.  
```
let vector2 = vector<decimal>([| 1.0M; 1.5M; -2.0M |])
vector2.Format() |> printfn "vector2: %s\n"

(*
Output:

1   1.5 -2

*)
```  

**Computation** : You can perform basic vector computations.  
1) Addition - <code>Vector.Add()</code>  
2) Subtracion - <code>Vector.Subtract()</code>  
3) Inner Production - <code>Vector.InnerProduct</code>  
```
// Vector initialization
let vec1 = vector<decimal>([| 1.0M; 2.0M; 3.0M; |])
let vec2 = vector<decimal>([| 2.0M; -1.5M; -2.0M; |])

// Addition
let vectorAdd = Vector.Add vec1 vec2
vectorAdd.Format() |> printfn "vectorAdd: \n%s"

// Subtraction
let vectorSubtract = Vector.Subtract vec1 vec2
vectorSubtract.Format() |> printfn "vectorSub: \n%s"

// Inner production
let vectorInnerProduct = Vector.InnerProduct vec1 vec2
vectorInnerProduct.Format() |> printfn "vectorInnerProduct: \n%s"

(*
Output: 

vectorAdd: 
[3.0 , 0.5 , 1.0 ]
vectorSub: 
[-1.0 , 3.5 , 5.0 ]
vectorInnerProduct: 
[2.00 , -3.00 , -6.00 ]

*)
```  
  
## License  
This program is open source under MIT license.  
  
Copyright (c) 2016 Jiung Hahm
  
Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:
  
The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.
  
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.