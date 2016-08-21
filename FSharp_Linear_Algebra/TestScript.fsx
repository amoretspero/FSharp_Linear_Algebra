#load "Matrix.fs"
#load "Vector.fs"

open FSharp_Linear_Algebra

// Test settings. - Helper functions, global variables, etc.
let mutable total = 0
let mutable passed = 0
let mutable failed = 0

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

// Matrix Addition tests.
let matAddParam1 = matrix<int>([| [| 1; 4; 3 |]; [| -3; 5; 1 |]; [| 2; 6; -2 |]; [| -3; -7; 1 |] |])
let matAddParam2 = matrix<int>([| [| 15; -24; 98 |]; [| 47; 82; -27 |]; [| 2; -41; 78 |]; [| 63; 51; -9 |] |])

let matAddRef1 = matrix<int>([| [| 16; -20; 101 |]; [| 44; 87; -26 |]; [| 4; -35; 76 |]; [| 60; 44; -8 |] |])