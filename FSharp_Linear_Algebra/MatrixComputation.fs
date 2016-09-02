namespace FSharp_Linear_Algebra.Matrix.Computation

open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Matrix.Decomposition

module Matrix =
    
    /// <summary>Computes inverse of given matrix by LDU-decomposition.</summary>
    /// <param name="mat">Matrix to compute inverse of.</param>
    /// <returns>Returns the inverse when invertible.</returns>
    /// <exception cref="FSharp_Linear_Algebra.Matrix.NotInvertible">Thrown when given matrix is not invertible.</exception>
    let inline Inverse (mat : 'T matrix) =
        try
            // LDU-decompose given matrix.
            let LDUResult = Decomposition.LDUdecomposition mat

            // Gets the permutation matrix.
            let permutation = LDUResult.Permutation

            // Gets the lower matrix.
            let lower = LDUResult.Lower

            // Initializes inverse matrix of lower matrix L.
            let lowerInverse = Matrix.Identity LDUResult.Lower.rowCnt LanguagePrimitives.GenericOne<'T>

            // Compute inverse of L.
            for i=0 to lower.columnCnt-1 do
                let rowTemp = lower.element.[i, *] // Temporarily get row for locality.
                for j=i downto 0 do
                    let columnTemp = lowerInverse.element.[*, j] // Temporarily get column for locality.
                    let mutable accum = LanguagePrimitives.GenericZero<'T> 
                    for k=0 to i-1 do accum <- accum + rowTemp.[k] * columnTemp.[k] // Accumulate known values of row-column multiplication.
                    lowerInverse.element.[i, j] <- (lowerInverse.element.[i, j] - accum) / rowTemp.[i] // Compute [i, j] of lowerInverse.

            (*printfn "L: \n%A" (lower.Format())
            printfn "Inverse of L: \n%A" (lowerInverse.Format())
            printfn "L * L^(-1) is: \n%A" ((Matrix.Multiply lower lowerInverse).Format())*)

            // Gets the upper matrix.
            let upper = LDUResult.Upper

            // Initializes inverse matrix of upper matrix U.
            let upperInverse = Matrix.Identity LDUResult.Upper.rowCnt LanguagePrimitives.GenericOne<'T>

            // Compute inverse of U.
            for i=upper.columnCnt-1 downto 0 do
                let rowTemp = upper.element.[i, *] // Temporarily get row for locality.
                for j=i to upper.columnCnt-1 do
                    let columnTemp = upperInverse.element.[*, j] // Temporarily get column for locality.
                    let mutable accum = LanguagePrimitives.GenericZero<'T>
                    for k=i+1 to upper.columnCnt-1 do accum <- accum + rowTemp.[k] * columnTemp.[k] // Accumulate known values of row-column multiplication.
                    upperInverse.element.[i, j] <- (upperInverse.element.[i, j] - accum) / rowTemp.[i] // Compute [i, j] of upperInverse.

            (*printfn "U: \n%A" (upper.Format())
            printfn "Inverse of U: \n%A" (upperInverse.Format())
            printfn "U * U^(-1) is: \n%A" ((Matrix.Multiply upper upperInverse).Format())*)

            // Gets the diagonal matrix.
            let diagonal = LDUResult.Diagonal

            // Gets the inverse of diagonal matrix.
            let diagonalInverse = Matrix.Identity LDUResult.Diagonal.rowCnt LanguagePrimitives.GenericOne<'T>
            for i=0 to diagonalInverse.rowCnt-1 do diagonalInverse.element.[i, i] <- LanguagePrimitives.GenericOne<'T> / diagonal.element.[i, i]

            (*printfn "D: \n%A" (diagonal.Format())
            printfn "Inverse of D: \n%A" (diagonalInverse.Format())
            printfn "D * D^(-1) is: \n%A" ((Matrix.Multiply diagonal diagonalInverse).Format())*)
            
            Matrix.Multiply (Matrix.Multiply (Matrix.Multiply upperInverse diagonalInverse) lowerInverse) permutation
        with
            | :? FSharp_Linear_Algebra.Matrix.Decomposition.NoLDUDecompositionPossible -> raise NotInvertible