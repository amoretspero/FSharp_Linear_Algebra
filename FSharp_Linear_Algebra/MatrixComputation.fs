namespace FSharp_Linear_Algebra.Matrix.Computation

open FSharp_Linear_Algebra.Vector
open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Matrix

/// <summary>Thrown when given linear system is not solvable.</summary>
exception UnsolvableLinearSystem

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
            | :? FSharp_Linear_Algebra.Matrix.NoLDUDecompositionPossible -> raise NotInvertible

    /// <summary>Computes column space of given matrix.</summary>
    /// <param name="mat">Matrix to compute column space of.</param>
    /// <returns>Returns column space of given matrix as array of column vectors.</returns>
    let inline ColumnSpace (mat : 'T matrix) : 'T vector [] =
        // RREF-decompose to find pivot columns.
        let rrefResult = Decomposition.RREFdecomposition mat
        let rrefResultRREF = rrefResult.RREF

        // Find pivot columns.
        let mutable pivots = ref ([| |] : int [])
        for i=0 to mat.rowCnt-1 do
            let mutable checkPivotLocation = i
            // Skip while element is zero, which indicates that column is not pivot column.
            while (checkPivotLocation < mat.columnCnt && rrefResult.RREF.element.[i, checkPivotLocation] = LanguagePrimitives.GenericZero<'T>) do
                checkPivotLocation <- checkPivotLocation + 1
            // When pivot column has been found, check if pivot is not zero and add pivot to pivot array.
            if (checkPivotLocation < mat.columnCnt) then
                if (rrefResult.RREF.element.[i, checkPivotLocation] = LanguagePrimitives.GenericOne<'T>) then pivots.Value <- Array.append pivots.Value [| checkPivotLocation |]

        // Get column space.
        let columnSpace = ref ([| |] : 'T vector [])
        for pivot in pivots.Value do
            columnSpace.Value <- Array.append columnSpace.Value [| vector<'T>(mat.element.[*, pivot]) |]

        columnSpace.Value

    /// <summary>Computes null space of given matrix.</summary>
    /// <param name="mat">Matrix to compute null space of.</param>
    /// <returns>Returns null space of given matrix as array of column vectors.</returns>
    let inline NullSpace (mat : 'T matrix) : 'T vector [] =
        // RREF-decompose to find pivot/free columns.
        let rrefResult = Decomposition.RREFdecomposition mat
        let rrefResultRREF = rrefResult.RREF

        // Find pivot columns.
        let mutable pivots = ref ([| |] : int [])
        for i=0 to mat.rowCnt-1 do
            let mutable checkPivotLocation = i
            // Skip while element is zero, which hindicates that column is not pivot column.
            while (checkPivotLocation < mat.columnCnt && rrefResultRREF.element.[i, checkPivotLocation] = LanguagePrimitives.GenericZero<'T>) do
                checkPivotLocation <- checkPivotLocation + 1
            // When pivot column has been found, check if pivot is not zero and add pivot to pivot array.
            if (checkPivotLocation < mat.columnCnt) then
                if (rrefResultRREF.element.[i, checkPivotLocation] = LanguagePrimitives.GenericOne<'T>) then pivots.Value <- Array.append pivots.Value [| checkPivotLocation |]

        // Get free columns.
        let mutable freeColumns = ref [| 0 .. mat.columnCnt-1 |]
        freeColumns.Value <- Array.except pivots.Value freeColumns.Value

        // Generate result matrix. It will be converted to vector array at the end.
        let resMatrix = matrix<'T>(mat.columnCnt, freeColumns.Value.Length, (Array2D.init mat.columnCnt freeColumns.Value.Length (fun idx0 idx1 -> LanguagePrimitives.GenericZero<'T>)))
        let mutable pivotColumnCnt = 0
        let mutable freeColumnCnt = 0
        for i=0 to mat.columnCnt-1 do
            if (Array.contains i pivots.Value) then // When current column is pivot column.
                // Fill with free column elements from (current pivot count)th row of RREF result, signs reverted.
                for j=0 to freeColumns.Value.Length-1 do
                    resMatrix.element.[i, j] <- LanguagePrimitives.GenericZero<'T> - rrefResultRREF.element.[pivotColumnCnt, freeColumns.Value.[j]]
                pivotColumnCnt <- pivotColumnCnt + 1
            else // When current column is free column.
                // Fill with elementary row-vector, one at location of current column.
                resMatrix.element.[i, freeColumnCnt] <- LanguagePrimitives.GenericOne<'T>
                freeColumnCnt <- freeColumnCnt + 1

        // Convert result matrix to array of column vectors.
        let resArray = ref ([| |] : 'T vector [])
        for i=0 to resMatrix.columnCnt-1 do
            resArray.Value <- Array.append resArray.Value [| vector<'T>(resMatrix.element.[*, i]) |]

        resArray.Value

    /// <summary>Solves system of linear equations, Ax=b. Solution dose not include null-space solutions.</summary>
    /// <param name="mat">Matrix of coefficients, A.</param>
    /// <param name="rhs">Right-hand side of equation, b.</param>
    /// <param name="threshold">Threshold of deciding whether given linear system if solvable or not.</param>
    /// <returns>Particular solution to Ax=b as vector.</returns>
    /// <exception cref="FSharp_Linear_Algebra.Matrix.Computation.UnsolvableLinearSystem">Thrown when given linear system is not solvable.</exception>
    let inline Solve (mat : 'T matrix) (rhs : 'T vector) (threshold : 'T) : 'T vector =
        let genericAbs (a : 'T) = 
            if a >= LanguagePrimitives.GenericZero<'T> then a else LanguagePrimitives.GenericZero<'T> - a
        // Decompose given matrix to RREF.
        let rrefResult = Decomposition.RREFdecomposition mat
        let rrefResultRREF = rrefResult.RREF
        let rrefResultPermutation = rrefResult.Permutation

        // Convert vector RHS to matrix form.
        let rhsMatrixForm = matrix<'T>(rhs.dim, 1, (Array2D.init rhs.dim 1 (fun idx0 idx1 -> rhs.element.[idx0])))

        // Compute inverse matrix for L, D, U of RREF-decomposition.
        // This will be used to form (U^-1)(D^-1)(L^-1)PA=R 
        let lowerInverse = Inverse rrefResult.Lower
        let diagonalInverse = matrix<'T>(rrefResult.Diagonal.rowCnt, rrefResult.Diagonal.columnCnt, (Array2D.init rrefResult.Diagonal.rowCnt rrefResult.Diagonal.columnCnt (fun idx0 idx1 -> if idx0=idx1 then LanguagePrimitives.GenericOne<'T> / rrefResult.Diagonal.element.[idx0, idx1] else LanguagePrimitives.GenericZero<'T>)))
        let upperInverse = Inverse rrefResult.Upper

        // Apply matrix multiplication by U^-1, D^-1, L^-1 and P to RHS matrix.
        let rrefRHS = Matrix.Multiply upperInverse (Matrix.Multiply diagonalInverse (Matrix.Multiply lowerInverse (Matrix.Multiply rrefResultPermutation rhsMatrixForm)))

        // Find pivot location.
        let mutable pivots = ref ([| |] : int [])
        let mutable pivotCheckRow = 0
        while (pivotCheckRow < mat.rowCnt) do
            let mutable pivotCheckColumn = pivotCheckRow
            let mutable pivotFound = false
            while (pivotCheckColumn < mat.columnCnt && not pivotFound) do
                if (rrefResultRREF.element.[pivotCheckRow, pivotCheckColumn] = LanguagePrimitives.GenericOne<'T>) then 
                    pivots.Value <- Array.append pivots.Value [| pivotCheckColumn |]
                    pivotFound <- true
                else
                    pivotCheckColumn <- pivotCheckColumn + 1
            pivotCheckRow <- pivotCheckRow + 1

        // Find free columns.
        let mutable freeColumnLocations = Array.except pivots.Value [| 0 .. mat.columnCnt-1 |]
        
        // Check if given linear system is solvable. If not, throw error.
        let mutable solvable = true
        let rank = pivots.Value.Length
        for i=rank to mat.rowCnt-1 do
            if (genericAbs rrefRHS.element.[i, 0]) > threshold then 
                solvable <- false
                printfn "Error value: %A" rrefRHS.element.[i, 0]
        if not solvable then 
            raise UnsolvableLinearSystem

        // Generate solution.
        let result = vector<'T>(Array.init mat.columnCnt (fun idx -> LanguagePrimitives.GenericZero<'T>))
        Array.iteri (fun idx elem -> result.element.[elem] <- rrefRHS.element.[idx, 0]) pivots.Value

        result