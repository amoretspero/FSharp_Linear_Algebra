//F#에 대해 http://fsharp.org에서 자세히 알아봅니다. F# 프로그래밍에 대한 자세한 지침은 
// 'F# 자습서' 프로젝트를 참조하세요.

#load "MatrixBasic.fs"
open FSharp_Linear_Algebra

// 라이브러리 스크립팅 코드를 정의합니다.

let matrix = Matrix(3, 7)
let zeroMatrix = Matrix.zeroMatrix(7, 7)
printf "Size of matrix is : %d * %d\n" matrix.rowCnt matrix.columnCnt
printf "First element is : %f\n\n" matrix.element.[0, 0]
printf "The whole matrix is : \n"
matrix.printMatrix()
printf "Transposed one is : \n"
matrix.Transpose().printMatrix()

let mat1 = Matrix(3, 4)
printf "mat1 is : \n"
mat1.printMatrix()
let mat2 = Matrix(4, 3)
printf "mat2 is : \n"
mat2.printMatrix()
printf "mat1 multiplied by mat2 is : \n"
Matrix.Multiply(mat1, mat2).printMatrix()