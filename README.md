# FSharp Linear Algebra  
Library for linear algebra made with F#.  
  
## Version  
**Stable** : **0.4.1.0**  
**Unstable** : **N/A** (stable version = newest version)     
  
## Update History  
  
### 0.4.0.0 -> 0.4.1.0  
  
- Finding inverse matrix is now available.  
- Inverse matrix computation is based on LDU decomposition and backward/forward substitution.  
  
## Description  
This project is to provide F#-made linear algebra library.  
Support will mainly include matrix and vector computation.  
Also, most of objects will be generic, meaning you can use this library with types you want.  
Currently, no optimization is provided.
  
## Supported Features  
  
### Matrix  

**Construction** : You can create matrix with three options.  
1) Basic constructor - <code>matrix<'T>(rowCnt : int, columnCnt : int, element : 'T [,])</code>  
2) Zero matrix constructor - <code>matrix<'T>(rowCnt : int, columnCnt : int, zero : 'T)</code>  
3) Constructor with Array2D. - <code>matrix<'T>(elem : 'T [] [])</code>  
```fsharp
open FSharp_Linear_Algebra.Matrix

// First constructor. Should provide type.
let matrix1 = matrix<int>(3, 7, (Array2D.init 3 7 (fun idx1 idx2 -> idx1 * idx2 - 1)))

// Second constructor. Should provide zero for type of matrix.
let matrix2 = matrix(3, 3, 0)

// Third constructor. Should provide type.
let matrix3 = matrix<float>([| [| 1.334; 8.461; 9.361; 2.904; 5.837 |]; [| 0.948; 8.847; 6.372; 4.981; 7.829 |]; [| 1.938; 3.284; 8.944; 5.748; 2.987 |] |])

// Check matrices.
printfn "matrix1 is : \n%A" (matrix1.Format())
printfn "matrix2 is : \n%A" (matrix2.Format())
printfn "matrix3 is : \n%A" (matrix3.Format())
```  

**Stringify** : <code>Format()</code> method.  
```fsharp
open FSharp_Linear_Algebra

// Float matrix.
let matrix4 = matrix<float>([| [| 1.334; 8.461; 9.361; 2.904; 5.837 |]; [| 0.948; 8.847; 6.372; 4.981; 7.829 |]; [| 1.938; 3.284; 8.944; 5.748; 2.987 |] |])
matrix4.Format() |> printfn "Matrix formatted: \n%s"
(*
Output will be:

Matrix formatted: 
1.334 8.461 9.361 2.904 5.837
0.948 8.847 6.372 4.981 7.829
1.938 3.284 8.944 5.748 2.987

*)
```  
  
**File I/O** : You can write to or read from files.  
1) Write to file - <code>Matrix.WriteToFile()</code>  
2) Read from file (<code>int32</code>) - <code>Matrix.ReadFromFileInt32()</code>
3) Read from file (<code>double</code>) - <code>Matrix.ReadFromFileDouble()</code>  
  
```fsharp
open FSharp_Linear_Algebra

// Float matrix.
let matrix5 = matrix<float>([| [| 1.334; 8.461; 9.361; 2.904; 5.837 |]; [| 0.948; 8.847; 6.372; 4.981; 7.829 |]; [| 1.938; 3.284; 8.944; 5.748; 2.987 |] |])

// Write matrix to file.
Matrix.WriteToFile matrix5 ".\float.txt"

// Read integer matrix from file.
let integerMatrixFromFile = Matrix.ReadFromFileInt32 ".\Int32Matrix.txt"

// Read double matrix from file.
let doubleMatrixFromFile = Matrix.ReadFromFileDouble ".\DoubleMatrix.txt"
```

**Computation** : You can perform basic matrix computations.  
1) Addition - <code>Matrix.Add()</code>  
2) Subtraction - <code>Matrix.Subtract()</code>  
3) Multiplication - <code>Matrix.Multiply()</code>  
4) Transpose - <code>Matrix.Transpose()</code>  
5) Scalar multiplication - <code>Matrix.ScalarMultiply()</code>  
6) Identity matrix - <code>Matrix.Identity()</code>  
7) Inverse matrix - <code>Matrix.Inverse()</code>

```fsharp
open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Matrix.Computation

// Matrix initialization.
let matrix6 = matrix<decimal>([| [| 2M; 1M; 1M; |]; [| 4M; -6M; 0M; |]; [| -2M; 7M; 2M; |] |])
let matrix7 = matrix<decimal>([| [| 1M; 1M; 1M; |]; [| 2M; 2M; 5M; |]; [| 4M; 6M; 8M; |] |])

// Addition
let matrixAdd = Matrix.Add matrix6  matrix7
matrixAdd.Format() |> printfn "matrixAdd: \n%s"

// Subtraction
let matrixSubtract = Matrix.Subtract matrix6 matrix7
matrixSubtract.Format() |> printfn "matrixSubt: \n%s"

// Multiplication
let matrixMultiply = Matrix.Multiply matrix6 matrix7
matrixMultiply.Format() |> printfn "matrixMultiply: \n%s"

// Transpose
let matrixTranspose = Matrix.Transpose matrix6
matrixTranspose.Format() |> printfn "matrixTranspose: \n%s"

// Scalar multiplication
let matrixScalarMultiply = Matrix.ScalarMultiply 1.5M matrix6
matrixScalarMultiply.Format() |> printfn "matrixScalarMultiply: \n%s"

// Identity matrix
let matrixIdentity = Matrix.Identity 3 1.0
matrixIdentity.Format() |> printfn "matrixIdentity: \n%s"

// Inverse matrix
let matrixInverse = Matrix.Inverse matrix6
matrixInverse.Format() |> printfn "matrixInverse: \n%s

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

matrixInverse: 
0.750000	-0.312500	-0.375000	
0.50000	-0.37500	-0.25000	
-1.000	1.000	1.000	

*)
```  
  
**Decomposition** : Decomposition of matrix is provided.  
1) LDU-decomposition - <code>Matrix.Decomposition.LDUdecomposition()</code>  

```fsharp
open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Matrix.Decomposition

// Random double matrix.
let matrix8 = RandomMatrix().RandomMatrixDouble 5 5

// LDU-decompose matrix.
let matrix8LU = Decomposition.LDUdecomposition matrix8

// Check P, L, D, U.
printfn "LDU-decomposition result - permutation matrix P : \n%A" (matrix8LU.Permutation.Format())
printfn "LDU-decomposition result - lower triangular matrix L : \n%A" (matrix8LU.Lower.Format())
printfn "LDU-decomposition result - diagonal matrix D : \n%A" (matrix8LU.Diagonal.Format())
printfn "LDU-decomposition result - upper triangular matrix U : \n%A" (matrix8LU.Upper.Format())

(*
Output will be (since random, results will vary) :

LDU-decomposition result - permutation matrix P : 
"1	0	0	0	0	
0	1	0	0	0	
0	0	0	0	1	
0	0	0	1	0	
0	0	1	0	0	
"
LDU-decomposition result - lower triangular matrix L : 
"1	0	0	0	0	
0.912469584879621	1	0	0	0	
0.833733308876576	0.968478037172233	1	0	0	
0.590786545174727	0.262825024811019	0.180887100037611	1	0	
0.921801935169788	0.865103479331415	-0.124928821945207	-0.36879768313967	1	
"
LDU-decomposition result - diagonal matrix D : 
"0.745390404362879	0	0	0	0	
0	0.615656479489813	0	0	0	
0	0	0.954266027830404	0	0	
0	0	0	0.0599831511524589	0	
0	0	0	0	0.19338922474478	
"
LDU-decomposition result - upper triangular matrix U : 
"1	0.358528834085624	0.901718659865987	0.294647960357563	0.274469225759811	
0	1	-0.883290706941838	0.719233246935208	-0.0118039081690822	
0	0	1	0.358018525062971	0.796112066982027	
0	0	0	1	-2.96933778536181	
0	0	0	0	1	
"

*)
```
  
### Vector  
**Construction** : You can create vector with one option.  
1) Basic constructor - <code>(element : 'T [])</code>  
```fsharp
open FSharp_Linear_Algebra.Vector

// Basic constructor. Should provide type.
let vector1 = vector<int>([| 1; 2; 3; 4; 5 |])
```  

**Stringify** : <code>Format()</code> method.  
```fsharp
open FSharp_Linear_Algebra.Vector

// Decimal vector whose dimension is 3.
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
```fsharp
open FSharp_Linear_Algebra.Vector

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