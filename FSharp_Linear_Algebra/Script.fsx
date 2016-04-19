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

let mat1 = matrix<float>(3, 4, (Array2D.init 3 4 (fun idx1 idx2 -> (float)idx1 / (float)idx2 + 7.0)))
printf "mat1 is : \n"
mat1.printMatrix()
let mat2 = matrix<float>(4, 3, (Array2D.init 3 4 (fun idx1 idx2 -> (float)idx1 * (float)idx2 + 2.3)))
printf "mat2 is : \n"
mat2.printMatrix()
printf "mat1 multiplied by mat2 is : \n"
(Matrix.Multiply mat1 mat2).printMatrix()