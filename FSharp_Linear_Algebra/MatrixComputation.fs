namespace FSharp_Linear_Algebra.Matrix.Computation

open FSharp_Linear_Algebra.Matrix

module Matrix =
    
    let inline Inverse (mat : 'T matrix) =
        mat