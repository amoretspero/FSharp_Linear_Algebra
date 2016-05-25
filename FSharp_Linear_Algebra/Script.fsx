//F#에 대해 http://fsharp.org에서 자세히 알아봅니다. F# 프로그래밍에 대한 자세한 지침은 
// 'F# 자습서' 프로젝트를 참조하세요.

#load "Matrix.fs"
open FSharp_Linear_Algebra

// 라이브러리 스크립팅 코드를 정의합니다.

let testMatrix = matrix<int>(3, 7, (Array2D.init 3 7 (fun idx1 idx2 -> idx1 * idx2 - 1)))
let zeroMatrix = matrix(7, 7, 0)
printf "Size of matrix is : %d * %d\n" testMatrix.rowCnt testMatrix.columnCnt
printf "First element is : %A\n\n" testMatrix.element.[0, 0]
printf "The whole matrix is : \n"
testMatrix.printMatrix()
printf "Transposed one is : \n"
testMatrix.Transpose().printMatrix()

let mat1 = matrix<float>(3, 4, (Array2D.init 3 4 (fun idx1 idx2 -> (float)idx1 * (float)idx2 + 7.0)))
printf "mat1 is : \n"
mat1.printMatrix()
let mat2 = matrix<float>(4, 3, (Array2D.init 4 3 (fun idx1 idx2 -> (float)idx1 * (float)idx2 + 2.3)))
printf "mat2 is : \n"
mat2.printMatrix()
printf "mat1 multiplied by mat2 is : \n"
(Matrix.Multiply mat1 mat2).printMatrix()
let mat3 = matrix<float>([| [| 1.334; 8.461; 9.361; 2.904; 5.837 |]; [| 0.948; 8.847; 6.372; 4.981; 7.829 |]; [| 1.938; 3.284; 8.944; 5.748; 2.987 |] |])
let mat4 = matrix<float>([| [| 5.984; 3.847; 2.938 |]; [| 3.948; 2.912; 3.333 |]; [| 5.938; 3.928; 4.081 |]; [| 4.928; 1.932; 2.241 |]; [| 9.846; 3.523; 6.324 |] |])
printfn "mat3 is : "
mat3.printMatrix()
printfn "mat4 is : "
mat4.printMatrix()
printfn "mat3 multiplied by mat4 is : "
(Matrix.Multiply mat3 mat4).printMatrix()
Matrix.Transpose(mat3)