#load "Matrix.fs"
#load "Vector.fs"
#load "RandomMatrix.fs"
#load "Decomposition.fs"
#load "MatrixComputation.fs"
#load "Vector.fs"
#r ".\\bin\\Debug\\MathNet.Numerics.dll"

open FSharp_Linear_Algebra.Vector
open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Matrix.Computation
open FSharp_Linear_Algebra.Matrix.Decomposition
open MathNet.Numerics

// Test settings. - Helper functions, global variables, etc.
let mutable total = 0
let mutable passed = 0
let mutable failed = 0

let first3 (a, _, _) = a
let second3 (_, b, _) = b
let third3 (_, _, c) = c

let first4 (a, _, _, _) = a
let second4 (_, b, _, _) = b
let third4 (_, _, c, _) = c
let fourth4 (_, _, _, d) = d

let doublePrecision = 1.0E-8

type TestType =
    | Add
    | Subtract
    | Multiply
    | Transpose
    | ScalarMultiply
    | Identity
    | GaussEliminate

let compareTrue (res : 'T matrix) (expected : 'T matrix) = 
    if res.element = expected.element then 
        printfn "Test #%d success." (total+1)
        total <- total + 1
        passed <- passed + 1
        true 
    else 
        printfn "Test #%d failed." (total+1)
        printfn "Expected: \n%s" (expected.Format())
        printfn "Got: \n%s" (res.Format())
        for i=0 to res.rowCnt - 1 do
            for j=0 to res.columnCnt - 1 do
                if res.element.[i, j] <> expected.element.[i, j] then printfn "Diff >> Element[%d, %d]: Expected %A, Got %A" i j res.element.[i, j] expected.element.[i, j]
        total <- total + 1
        failed <- failed + 1
        false

let compareSpaceTrue (res : vector<'T> []) (expected : vector<'T> []) =
    if res = expected then
        printfn "Test #%d success." (total+1)
        total <- total + 1
        passed <- passed + 1
        true
    else
        printfn "Test #%d failed." (total+1)
        printfn "Expected: \n%s" (String.concat System.Environment.NewLine (Array.map (fun (x : 'T vector) -> x.Format()) res))
        printfn "Got: \n%s" (String.concat System.Environment.NewLine (Array.map (fun (x : 'T vector) -> x.Format()) expected))
        for i=0 to res.Length-1 do
            for j=0 to res.[i].dim-1 do
                if expected.[i].element.[j] <> res.[i].element.[j] then printfn "Diff >> vector #%d [%d]: Expected %A, got %A" i j expected.[i].element.[j] res.[i].element.[j]
        total <- total + 1
        failed <- failed + 1
        false

let compareDoubleTrue (res : double matrix) (expected : double matrix) (threshold : double) = 
    let mutable diff = false
    for i=0 to res.rowCnt-1 do
        for j=0 to res.columnCnt-1 do
            if (System.Math.Abs(res.element.[i, j] - expected.element.[i, j]) > threshold) then
                diff <- true
    if not diff then 
        printfn "Test #%d success." (total+1)
        total <- total + 1
        passed <- passed + 1
        true 
    else 
        printfn "Test #%d failed." (total+1)
        printfn "Expected: \n%s" (expected.Format())
        printfn "Got: \n%s" (res.Format())
        for i=0 to res.rowCnt - 1 do
            for j=0 to res.columnCnt - 1 do
                if (System.Math.Abs(res.element.[i, j] - expected.element.[i, j]) > threshold) then 
                    printfn "Diff >> Element[%d, %d]: Expected %A, Got %A" i j expected.element.[i, j] res.element.[i, j]
        total <- total + 1
        failed <- failed + 1
        false

let compareFalse (res : 'T matrix) (expected : 'T matrix) =
    if res.element = expected.element then
        printfn "Test #%d failed." (total+1)
        printfn "Expected: \n%s" (expected.Format())
        printfn "Got: \n%s" (res.Format())
        total <- total + 1
        failed <- failed + 1
        false
    else
        printfn "Test #%d success." (total+1)
        total <- total + 1
        passed <- passed + 1
        true

let printPrologue (testType : string) =
    printfn "Now testing: <<Matrix %s>>..." testType
    printfn "Test number: %d" (total + 1)

let endTest () =
    printfn "\n==========End of Matrix Test==========\n"
    printfn "Total # of tests: %d" total
    printfn "Passed # of tests: %d" passed
    printfn "Failed # of tests: %d" failed
    printfn ""

let sw = new System.Diagnostics.Stopwatch()
let swRef = new System.Diagnostics.Stopwatch()


// Matrix Addition tests.
let matAddParam1 = matrix<int>([| [| 1; 4; 3 |]; [| -3; 5; 1 |]; [| 2; 6; -2 |]; [| -3; -7; 1 |] |])
let matAddParam2 = matrix<int>([| [| 15; -24; 98 |]; [| 47; 82; -27 |]; [| 2; -41; 78 |]; [| 63; 51; -9 |] |])

let matAddRef1 = matrix<int>([| [| 16; -20; 101 |]; [| 44; 87; -26 |]; [| 4; -35; 76 |]; [| 60; 44; -8 |] |])

printPrologue("Addition")
compareTrue (Matrix.Add matAddParam1 matAddParam2) matAddRef1

// Matrix Subtraction tests.
let matSubtractParam1 = matrix<decimal>([| [| 1.9M; -2.5M; 3.2M; 0.84M |]; [| 10.3M; 8.7M; -11.4M; 0.3M |]; [| 3.4M; 1.43M; 0.2M; -1.42M |] |])
let matSubtractParam2 = matrix<decimal>([| [| 8.4M; 2.1M; -5.3M; 1.2M |]; [| 6.5M; -14.2M; 8.3M; 7.21M |]; [| 5.2M; 2.31M; 9.4M; 1.0M |] |])

let matSubtractRef1 = matrix<decimal>([| [| -6.5M; -4.6M; 8.5M; -0.36M |]; [| 3.8M; 22.9M; -19.7M; -6.91M; |]; [| -1.8M; -0.88M; -9.2M; -2.42M |] |])

printPrologue("Subtraction")
compareTrue (Matrix.Subtract matSubtractParam1 matSubtractParam2) matSubtractRef1

// Matrix Addition/Subtraction tests.
let matAddSubParam1 = RandomMatrix().RandomMatrixDouble 128 128
let matAddSubParam2 = RandomMatrix().RandomMatrixDouble 128 128

printPrologue("Addition/Subtraction")
compareDoubleTrue (Matrix.Add (Matrix.Subtract matAddSubParam1 matAddSubParam2) matAddSubParam2) matAddSubParam1 doublePrecision

printPrologue("Addition/Subtraction")
compareDoubleTrue (Matrix.Subtract (Matrix.Add matAddSubParam1 matAddSubParam2) matAddSubParam2) matAddSubParam1 doublePrecision

// Matrix Multiplication tests.
let matMultParam1 = RandomMatrix().RandomMatrixDouble 128 128
let matMultParam2 = RandomMatrix().RandomMatrixDouble 128 128

let matMultRef1 = matrix<double>(matMultParam1.rowCnt, matMultParam2.columnCnt, (MathNet.Numerics.LinearAlgebra.Matrix.Build.DenseOfArray(matMultParam1.element).Multiply(MathNet.Numerics.LinearAlgebra.Matrix.Build.DenseOfArray(matMultParam2.element)).Storage.ToArray()))

printPrologue("Multiplication")
compareDoubleTrue (Matrix.Multiply matMultParam1 matMultParam2) matMultRef1 doublePrecision

// LDU decomposition test.

let test = RandomMatrix().RandomMatrixDouble 16 16
let testRef = MathNet.Numerics.LinearAlgebra.Matrix.Build.DenseOfArray(test.element)

let testLU = Decomposition.LDUdecomposition test
let testRefLU = testRef.LU()

printPrologue("LDU decomposition - Self test") // Tests if PA=LDU.
compareDoubleTrue (Matrix.Multiply (Matrix.Multiply testLU.Lower testLU.Diagonal) testLU.Upper) (Matrix.Multiply testLU.Permutation test) doublePrecision

printPrologue("LDU decomposition - Lower matrix") // Tests if Lower matrix L is same with MathNet reference.
compareDoubleTrue testLU.Lower (matrix<double>(test.rowCnt, test.columnCnt, (testRefLU.L.Storage.ToArray()))) doublePrecision

printPrologue("LDU decomposition - Upper matrix") // Tests if Upper matrix U is same with MathNet reference.
compareDoubleTrue (Matrix.Multiply testLU.Diagonal testLU.Upper) (matrix<double>(test.rowCnt, test.columnCnt, (testRefLU.U.Storage.ToArray()))) doublePrecision

// Inverse matrix test.

let inverseTest = RandomMatrix().RandomMatrixDouble 16 16
let inverseTestRef = MathNet.Numerics.LinearAlgebra.Matrix.Build.DenseOfArray(inverseTest.element)

let inverseTestRes = Matrix.Inverse inverseTest
let inverseTestRefRes = inverseTestRef.Inverse()

printPrologue("Inverse matrix - Self test") // Tests if A * A^(-1) = I
compareDoubleTrue (Matrix.Multiply inverseTest inverseTestRes) (Matrix.Identity inverseTest.rowCnt 1.0) doublePrecision

printPrologue("Inverse matrix - Referenct test") // Tests if Inverse matrix is same with MathNet reference.
compareDoubleTrue (matrix<double>(inverseTest.rowCnt, inverseTest.columnCnt, inverseTestRefRes.Storage.ToArray())) inverseTestRes doublePrecision

// Row-Reduced Echelon Form test.

let rrefTest1 = matrix<double>([| [| 1.0; 3.0; 3.0; 2.0 |]; [| 2.0; 6.0; 9.0; 7.0 |]; [| -1.0; -3.0; 3.0; 4.0 |] |])
let rrefTest2 = matrix<double>([| [| 1.0; 2.0; 3.0; 5.0 |]; [| 2.0; 4.0; 8.0; 12.0 |]; [| 3.0; 6.0; 7.0; 13.0 |] |])
let rrefTest3 = RandomMatrix().RandomMatrixDouble 16 12

let rrefTestRes1 = Decomposition.RREFdecomposition rrefTest1
let rrefTestRes2 = Decomposition.RREFdecomposition rrefTest2
let rrefTestRes3 = Decomposition.RREFdecomposition rrefTest3

printPrologue("Row-Reduced Echelon Form - Test from book 1")
compareDoubleTrue rrefTestRes1.RREF (matrix<double>([| [| 1.0; 3.0; 0.0; -1.0; |]; [| 0.0; 0.0; 1.0; 1.0; |]; [| 0.0; 0.0; 0.0; 0.0 |] |])) doublePrecision

printPrologue("Row-Reduced Echelon Form - Test from book 2")
compareDoubleTrue rrefTestRes2.RREF (matrix<double>([| [| 1.0; 2.0; 0.0; 2.0 |]; [| 0.0; 0.0; 1.0; 1.0 |]; [| 0.0; 0.0; 0.0; 0.0 |] |])) doublePrecision

printPrologue("Row-Reduced Echelon Form - Self test")
(*rrefTestRes3.Lower.Format() |> printfn "Lower matrix - \n%A"
rrefTestRes3.Diagonal.Format() |> printfn "Diagonal matrix - \n%A"
rrefTestRes3.Upper.Format() |> printfn "Upper matrix - \n%A"
rrefTestRes3.RREF.Format() |> printfn "RREF matrix - \n%A"
(Matrix.Multiply rrefTestRes3.Upper rrefTestRes3.RREF).Format() |> printfn "UR - \n%A"
(Matrix.Multiply (Matrix.Multiply rrefTestRes3.Diagonal rrefTestRes3.Upper) rrefTestRes3.RREF).Format() |> printfn "DUR - \n%A"
(Matrix.Multiply (Matrix.Multiply (Matrix.Multiply rrefTestRes3.Lower rrefTestRes3.Diagonal) rrefTestRes3.Upper) rrefTestRes3.RREF).Format() |> printfn "LDUR - \n%A"*)
compareDoubleTrue (Matrix.Multiply (Matrix.Multiply (Matrix.Multiply rrefTestRes3.Lower rrefTestRes3.Diagonal) rrefTestRes3.Upper) rrefTestRes3.RREF) (Matrix.Multiply rrefTestRes3.Permutation rrefTest3) doublePrecision

// Column space test.

let columnSpaceTest1 = matrix<double>([| [| 1.0; 3.0; 3.0; 2.0 |]; [| 2.0; 6.0; 9.0; 7.0 |]; [| -1.0; -3.0; 3.0; 4.0 |] |])
let columnSpaceTest2 = matrix<double>([| [| 1.0; 2.0; 3.0; 5.0 |]; [| 2.0; 4.0; 8.0; 12.0 |]; [| 3.0; 6.0; 7.0; 13.0 |] |])

let columnSpaceTestRes1 = Matrix.ColumnSpace columnSpaceTest1
let columnSpaceTestRes2 = Matrix.ColumnSpace columnSpaceTest2

let columnSpaceTestResMat1 = matrix<double>(columnSpaceTestRes1.[0].dim, columnSpaceTestRes1.Length, (Array2D.init columnSpaceTestRes1.[0].dim columnSpaceTestRes1.Length (fun idx0 idx1 -> columnSpaceTestRes1.[idx1].element.[idx0])))
let columnSpaceTestResMat2 = matrix<double>(columnSpaceTestRes2.[0].dim, columnSpaceTestRes2.Length, (Array2D.init columnSpaceTestRes2.[0].dim columnSpaceTestRes2.Length (fun idx0 idx1 -> columnSpaceTestRes2.[idx1].element.[idx0])))

let columnSpaceTestRef1 = matrix<double>([| [| 1.0; 3.0 |]; [| 2.0; 9.0 |]; [| -1.0; 3.0 |] |])
let columnSpaceTestRef2 = matrix<double>([| [| 1.0; 3.0 |]; [| 2.0; 8.0 |]; [| 3.0; 7.0 |] |])

printPrologue("Column-Space - Test from book 1")
compareTrue columnSpaceTestResMat1 columnSpaceTestRef1

printPrologue("Column-Space - Test from book 2")
compareTrue columnSpaceTestResMat2 columnSpaceTestRef2

// Null space test.

let nullSpaceTest1 = matrix<double>([| [| 1.0; 3.0; 3.0; 2.0 |]; [| 2.0; 6.0; 9.0; 7.0 |]; [| -1.0; -3.0; 3.0; 4.0 |] |])
let nullSpaceTest2 = matrix<double>([| [| 1.0; 2.0; 3.0; 5.0 |]; [| 2.0; 4.0; 8.0; 12.0 |]; [| 3.0; 6.0; 7.0; 13.0 |] |])

let nullSpaceTestRes1 = Matrix.NullSpace nullSpaceTest1
let nullSpaceTestRes2 = Matrix.NullSpace nullSpaceTest2

let nullSpaceTestResMat1 = matrix<double>(nullSpaceTestRes1.[0].dim, nullSpaceTestRes1.Length, (Array2D.init nullSpaceTestRes1.[0].dim nullSpaceTestRes1.Length (fun idx0 idx1 -> nullSpaceTestRes1.[idx1].element.[idx0])))
let nullSpaceTestResMat2 = matrix<double>(nullSpaceTestRes2.[0].dim, nullSpaceTestRes2.Length, (Array2D.init nullSpaceTestRes2.[0].dim nullSpaceTestRes2.Length (fun idx0 idx1 -> nullSpaceTestRes2.[idx1].element.[idx0])))

let nullSpaceTestRef1 = matrix<double>([| [| -3.0; 1.0 |]; [| 1.0; 0.0 |]; [| 0.0; -1.0 |]; [| 0.0; 1.0 |] |])
let nullSpaceTestRef2 = matrix<double>([| [| -2.0; -2.0 |]; [| 1.0; 0.0 |]; [| 0.0; -1.0 |]; [| 0.0; 1.0 |] |])

printPrologue("Null-Space - Test from book 1")
compareDoubleTrue nullSpaceTestResMat1 nullSpaceTestRef1 doublePrecision

printPrologue("Null-Space - Test from book 2")
compareDoubleTrue nullSpaceTestResMat2 nullSpaceTestRef2 doublePrecision

// End test.
endTest()