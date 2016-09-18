#load "Matrix.fs"
#load "RandomMatrix.fs"
#load "Vector.fs"
#load "Decomposition.fs"
#load "MatrixComputation.fs"

open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Matrix.Computation
open FSharp_Linear_Algebra.Vector

// Matrix - construction

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

// Matrix - stringify

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

// Matrix - File I/O

// Float matrix.
let matrix5 = matrix<float>([| [| 1.334; 8.461; 9.361; 2.904; 5.837 |]; [| 0.948; 8.847; 6.372; 4.981; 7.829 |]; [| 1.938; 3.284; 8.944; 5.748; 2.987 |] |])

// Write matrix to file.
Matrix.WriteToFile matrix5 ".\\float.txt"

// Read integer matrix from file.
let integerMatrixFromFile = Matrix.ReadFromFileInt32 ".\\Int32Matrix.txt"

// Read double matrix from file.
let doubleMatrixFromFile = Matrix.ReadFromFileDouble ".\\DoubleMatrix.txt"

// Matrix - Computation

// Matrix initialization.
let matrix6 = matrix<decimal>([| [| 2M; 1M; 1M; |]; [| 4M; -6M; 0M; |]; [| -2M; 7M; 2M; |] |])
let matrix7 = matrix<decimal>([| [| 1M; 1M; 1M; |]; [| 2M; 2M; 5M; |]; [| 4M; 6M; 8M; |] |])
let matrix8 = matrix<double>([| [| 1.0; 3.0; 3.0; 2.0 |]; [| 2.0; 6.0; 9.0; 7.0 |]; [| -1.0; -3.0; 3.0; 4.0 |] |])

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
matrixInverse.Format() |> printfn "matrixInverse: \n%s"

// Column space
let columnSpaceResult = Matrix.ColumnSpace matrix8
for vec in columnSpaceResult do vec.Format() |> printfn "Basis of column space: \n%s"
printfn ""

// Null space
let nullSpaceResult = Matrix.NullSpace matrix8
for vec in nullSpaceResult do vec.Format() |> printfn "Basis of null space: \n%s"
printfn ""

// Solve
let matrixSolverRHS = vector<double>([| 1.0; 5.0; 5.0 |])
let matrixSolverResult = Matrix.Solve matrix8 matrixSolverRHS 10E-8
matrixSolverResult.Format() |> printfn "Solver result: \n%s"
printfn ""


// Matrix - decomposition

// Random double matrix.
let matrix9 = RandomMatrix().RandomMatrixDouble 5 5
let matrix10 = RandomMatrix().RandomMatrixDouble 4 6

// LDU-decompose matrix.
let matrix9LU = Decomposition.LDUdecomposition matrix9

// Check P, L, D, U.
printfn "LDU-decomposition result - permutation matrix P : \n%A" (matrix9LU.Permutation.Format())
printfn "LDU-decomposition result - lower triangular matrix L : \n%A" (matrix9LU.Lower.Format())
printfn "LDU-decomposition result - diagonal matrix D : \n%A" (matrix9LU.Diagonal.Format())
printfn "LDU-decomposition result - upper triangular matrix U : \n%A" (matrix9LU.Upper.Format())

// RREF-decompose matrix.
let matrix10RREF = Decomposition.RREFdecomposition matrix10

// Check P, L, D, U, R
printfn "RREF-decomposition result - permutation matrix P : \n%A" (matrix10RREF.Permutation.Format())
printfn "RREF-decomposition result - lower matrix L : \n%A" (matrix10RREF.Lower.Format())
printfn "RREF-decomposition result - diagonal matrix D : \n%A" (matrix10RREF.Diagonal.Format())
printfn "RREF-decomposition result - upper matrix U : \n%A" (matrix10RREF.Upper.Format())
printfn "RREF-decomposition result - row-reduced echelon form matrix R : \n%A" (matrix10RREF.RREF.Format())

// ------------------------------------------------------------------

// Vector - construction

// Basic constructor. Should provide type.
let vector1 = vector<int>([| 1; 2; 3; 4; 5 |])

// Vector - stringify

// Decimal vector whose dimension is 3.
let vector2 = vector<decimal>([| 1.0M; 1.5M; -2.0M |])
vector2.Format() |> printfn "vector2: %s\n"

// Vector - computation

// Vector initialization
let vector3 = vector<decimal>([| 1.0M; 2.0M; 3.0M; |])
let vector4 = vector<decimal>([| 2.0M; -1.5M; -2.0M; |])

// Addition
let vectorAdd = Vector.Add vector3 vector4
vectorAdd.Format() |> printfn "vectorAdd: \n%s"

// Subtraction
let vectorSubtract = Vector.Subtract vector3 vector4
vectorSubtract.Format() |> printfn "vectorSub: \n%s"

// Inner production
let vectorInnerProduct = Vector.InnerProduct vector3 vector4
vectorInnerProduct |> printfn "vectorInnerProduct: \n%A"

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