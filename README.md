#FSharp Linear Algebra  
Library for linear algebra made with F#.  
  
##Version  
Current : **0.3.1.0**  
Status : **Unstable**  
  
##Update History  
  
### 0.3.0.0 -> 0.3.1.0  
  
- Gauss elimination supports PA=LU style elimination. It will return (P * L * U), size-3 tuple containing elimination result when elimination is possible.
  
### 0.2.1.1 -> 0.3.0.0  
  
- For <code>int32</code>, <code>int64</code>, <code>float32</code>, <code>double</code> typed vectors, size can be calculated.  
- Added zero vector, unit vector creator.  
- Added method to check if given vector is unit vector.  
  
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
4) Size (int32) - <code>Vector.SizeInt32</code>  
5) Size (int64) - <code>Vector.SizeInt64</code>  
6) Size (float32) - <code>Vector.SizeFloat32</code>  
7) Size (double) - <code>Vector.SizeDouble</code>  
8) Unit vector check - <code>Vector.IsUnitVector()</code>  
9) Zero vector constructor - <code>Vector.ZeroVector()</code>  
10) Unit vector constructor - <code>Vector.UnitVector()</code>  
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

// Size - int32
let sizeInt32Vector = vector<int32>([| 3; 4 |])
Vector.SizeInt32 sizeInt32Vector |> printfn "Size of %s is: \n%d" (sizeInt32Vector.Format())

// Size - int64
let sizeInt64Vector = vector<int64>([| 3L; 4L |])
Vector.SizeInt64 sizeInt64Vector |> printfn "Size of %s is: \n%d" (sizeInt32Vector.Format())

// Size - float32
let sizeFloat32Vector = vector<float32>([| 3.2f; 4.1f |])
Vector.SizeFloat32 sizeFloat32Vector |> printfn "Size of %s is: \n%A" (sizeFloat32Vector.Format())

// Size - int32
let sizeDoubleVector = vector<double>([| 2.5; 4.7; 5.6 |])
Vector.SizeDouble sizeDoubleVector |> printfn "Size of %s is: \n%A" (sizeDoubleVector.Format())

// Zero vector creation
let zeroVector = Vector.ZeroVector<decimal> 3
zeroVector.Format() |> printfn "zeroVector: \n%s"

// Unit vector creation
let unitVector = Vector.UnitVector 5 3 1.0
unitVector.Format() |> printfn "unitVector: \n%s"

// Check unit vector
let nonUnitVector = vector<float>([| 1.2; 2.1; 0.0 |])
let unitVector2 = vector<float>([| 0.5; -0.5; 0.5; -0.5 |])
if Vector.IsUnitVector nonUnitVector then 
    printfn "ERROR! This is not unit vector: \n%s" (nonUnitVector.Format()) 
else 
    printfn "CORRECT! This is not unit vector: \n%s" (nonUnitVector.Format())
if Vector.IsUnitVector unitVector then
    printfn "CORRECT! This is unit vector: \n%s" (unitVector2.Format())
else
    printfn "ERROR! This is unit vector: \n%s" (unitVector2.Format())

(*
Output: 

vectorAdd: 
[3.0 , 0.5 , 1.0 ]
vectorSub: 
[-1.0 , 3.5 , 5.0 ]
vectorInnerProduct: 
[2.00 , -3.00 , -6.00 ]
Size of [3 , 4 ] is: 
5
Size of [3 , 4 ] is: 
5
Size of [3.2 , 4.1 ] is: 
5.20096159f
Size of [2.5 , 4.7 , 5.6 ] is: 
7.726577509
zeroVector: 
[0 , 0 , 0 ]
unitVector: 
[0 , 0 , 1 , 0 , 0 ]
CORRECT! This is not unit vector: 
[1.2 , 2.1 , 0 ]
CORRECT! This is unit vector: 
[0.5 , -0.5 , 0.5 , -0.5 ]

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