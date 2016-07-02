//F#에 대해 http://fsharp.org에서 자세히 알아봅니다. F# 프로그래밍에 대한 자세한 지침은 
// 'F# 자습서' 프로젝트를 참조하세요.

#load "Matrix.fs"
open FSharp_Linear_Algebra

// 라이브러리 스크립팅 코드를 정의합니다.

let testMatrix = matrix<int>(3, 7, (Array2D.init 3 7 (fun idx1 idx2 -> idx1 * idx2 - 1)))
let zeroMatrix = matrix(7, 7, 0)

printfn "Size of matrix is : %d * %d" testMatrix.rowCnt testMatrix.columnCnt
printfn "First element is : %A\n" testMatrix.element.[0, 0]
testMatrix.Format()
|> printf "The whole matrix is :\n%s" 


let mat1 = matrix<float>(3, 4, (Array2D.init 3 4 (fun idx1 idx2 -> (float)idx1 * (float)idx2 + 7.0)))
mat1.Format()
|> printfn "mat1 is :\n%s"

let mat2 = matrix<float>(4, 3, (Array2D.init 4 3 (fun idx1 idx2 -> (float)idx1 * (float)idx2 + 2.3)))
mat2.Format()
|> printfn "mat2 is :\n%s"

(Matrix.Multiply mat1 mat2).Format()
|> printfn "mat1 multiplied by mat2 is :\n%s"

let mat3 = matrix<float>([| [| 1.334; 8.461; 9.361; 2.904; 5.837 |]; [| 0.948; 8.847; 6.372; 4.981; 7.829 |]; [| 1.938; 3.284; 8.944; 5.748; 2.987 |] |])
let mat4 = matrix<float>([| [| 5.984; 3.847; 2.938 |]; [| 3.948; 2.912; 3.333 |]; [| 5.938; 3.928; 4.081 |]; [| 4.928; 1.932; 2.241 |]; [| 9.846; 3.523; 6.324 |] |])

mat3.Format()
|> printfn "mat3 is :\n%s"

mat4.Format()
|> printfn "mat4 is :\n%s"

(Matrix.Multiply mat3 mat4).Format()
|> printfn "mat3 multiplied by mat4 is :\n%s"

Matrix.Transpose(mat3).Format()
|> printfn "Transposed mat3 is :\n%s"

let mat5 = matrix<decimal>([| [| 2M; 1M; 1M; |]; [| 4M; -6M; 0M; |]; [| -2M; 7M; 2M; |] |])
Matrix.GaussEliminate(mat5).Format() |> printfn "mat5 eliminated is: \n%s"

let mat6 = matrix<decimal>([| [| 1M; 1M; 1M; |]; [| 2M; 2M; 5M; |]; [| 4M; 6M; 8M; |] |])
Matrix.GaussEliminate(mat6).Format() |> printfn "mat6 eliminated is: \n%s"

let mat7 = matrix<decimal>([| [| 1M; 1M; 1M; |]; [| 2M; 2M; 5M; |]; [| 4M; 4M; 8M; |] |])
try
    Matrix.GaussEliminate(mat7).Format() |> printfn "mat7 eliminated is: \n%s"
with
    | :? System.Exception as e -> printfn "mat7 cannot be eliminated."

try
    Matrix.Add mat3 mat4 |> ignore
with
    | :? FSharp_Linear_Algebra.SizeUnmatch as e -> printfn "Size unmatched. Matrix of %d by %d with matrix of %d by %d." e.Data0 e.Data1 e.Data2 e.Data3