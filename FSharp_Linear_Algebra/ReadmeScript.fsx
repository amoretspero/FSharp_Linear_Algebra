#load "Matrix.fs"
#load "Vector.fs"
open FSharp_Linear_Algebra

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

//---------------------------------------------------------------------------

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