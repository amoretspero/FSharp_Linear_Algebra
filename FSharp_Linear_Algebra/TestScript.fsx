#load "Matrix.fs"
#load "RandomMatrix.fs"
#load "Decomposition.fs"
#load "Vector.fs"
#r ".\\bin\\Debug\\MathNet.Numerics.dll"

open FSharp_Linear_Algebra.Matrix
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
                    printfn "Diff >> Element[%d, %d]: Expected %A, Got %A" i j res.element.[i, j] expected.element.[i, j]
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

let test = RandomMatrix().RandomMatrixDouble 5 5
let testRef = MathNet.Numerics.LinearAlgebra.Matrix.Build.DenseOfArray(test.element)

let testLU = Decomposition.LDUdecomposition test
let testRefLU = testRef.LU()

printfn "Original matrix: \n%A" (test.Format())

printPrologue("LDU decomposition - Self test") // Tests if PA=LDU.
compareDoubleTrue (Matrix.Multiply (Matrix.Multiply testLU.Lower testLU.Diagonal) testLU.Upper) (Matrix.Multiply testLU.Permutation test) doublePrecision

printPrologue("LDU decomposition - Lower matrix") // Tests if Lower matrix L is same with MathNet reference.
compareDoubleTrue testLU.Lower (matrix<double>(test.rowCnt, test.columnCnt, (testRefLU.L.Storage.ToArray()))) doublePrecision

printPrologue("LDU decomposition - Upper matrix") // Tests if Upper matrix U is same with MathNet reference.
compareDoubleTrue (Matrix.Multiply testLU.Diagonal testLU.Upper) (matrix<double>(test.rowCnt, test.columnCnt, (testRefLU.U.Storage.ToArray()))) doublePrecision

// End test.
endTest()