namespace FSharp_Linear_Algebra.Matrix.Decomposition

open System
open System.Collections
open System.Collections.Generic
open System.Linq
open System.Numerics
open FSharp_Linear_Algebra.Matrix

/// <summary>Exception that indicates gauss elimination of given matrix is not possible.</summary>
exception NoLDUDecompositionPossible

/// <summary>Class for representing LDU-decomposition result.</summary>
[<Class>]
type LDUResult<'T> (P : 'T matrix, L : 'T matrix, D : 'T matrix, U : 'T matrix) =
    
    // Private data -------------------------------------------------

    /// <summary>Permutation matrix of LDU-decomposition.</summary>
    let _permutation = P

    /// <summary>Lower matrix of LDU-decomposition.</summary>
    let _lower = L

    /// <summary>Diagonal matrix of LDU-decomposition.</summary>
    let _diagonal = D

    /// <summary>Upper matrix of LDU-decomposition.</summary>
    let _upper = U

    // Instance properties ------------------------------------------

    /// <summary>Gets permutation matrix of LDU-decomposition.</summary>
    member LDU.Permutation
        with get() = _permutation

    /// <summary>Gets lower matrix of LDU-decomposition.</summary>
    member LDU.Lower
        with get() = _lower

    /// <summary>Gets diagonal matrix of LDU-decomposition.</summary>
    member LDU.Diagonal
        with get() = _diagonal

    /// <summary>Gets upper matrix of LDU-decomposition.</summary>
    member LDU.Upper
        with get() = _upper

/// <summary>Class for representing RREF(Row-Reduced Echelon Form)-decomposition result.</summary>
[<Class>]
type RREFResult<'T> (P : 'T matrix, L : 'T matrix, D : 'T matrix, U : 'T matrix, R : 'T matrix) =
    
    // Private data -------------------------------------------------

    /// <summary>Permutation matrix of RREF-decomposition.</summary>
    let _permutation = P
    
    /// <summary>Lower triangular matrix of RREF-decomposition.</summary>
    let _lower = L

    /// <summary>Diagonal matrix of RREF-decomposition.</summary>
    let _diagonal = D

    /// <summary>Upper triangular matrix of RREF-decomposition.</summary>
    let _upper = U

    /// <summary>Row-Reduced Echelon Form matrix of RREF-decomposition.</summary>
    let _rref = R

    // Instance properties ------------------------------------------

    /// <summary>Gets permutation matrix of RREF-decomposition.</summary>
    member RREF.Permutation
        with get() = _permutation
    
    /// <summary>Gets lower triangular matrix of RREF-decomposition.</summary>
    member RREF.Lower
        with get() = _lower

    /// <summary>Gets diagonal matrix of RREF-decomposition.</summary>
    member RREF.Diagonal
        with get() = _diagonal

    /// <summary>Gets upper triangular matrix of RREF-decomposition.</summary>
    member RREF.Upper
        with get() = _upper

    /// <summary>Gets Row-Reduced Echelon Form matrix of RREF-decomposition.</summary>
    member RREF.RREF
        with get() = _rref
    


/// <summary>Module containing various decompositions.</summary>
module Decomposition =
    
    /// <summary>LDU-decompose given matrix.</summary>
    /// <param name="mat">Matrix to be decomposed.</param>
    /// <returns>Returns decomposed result.</returns>
    /// <exception cref="FSharp_Linear_Algebra.Matrix.Decomposition.NoLDUDecompositionPossible">Thrown when LDU decomposition cannot be performed.</exception>
    let inline LDUdecomposition (mat : 'T matrix) : 'T LDUResult =
        let fst3 (a, _, _) = a
        let snd3 (_, b, _) = b
        let thd3 (_, _, c) = c
        let fst4 (a, _, _, _) = a
        let snd4 (_, b, _, _) = b
        let thd4 (_, _, c, _) = c
        let fth4 (_, _, _, d) = d
        let genericAbs (a : 'T) =
            if a >= LanguagePrimitives.GenericZero then a else LanguagePrimitives.GenericZero - a
        do if mat.columnCnt <> mat.rowCnt then raise (NotSquare(mat.columnCnt, mat.rowCnt)) // Check if matrix is square.
        let upperMatrix = matrix<'T>(mat.rowCnt, mat.columnCnt, Array2D.copy(mat.element)) // Copies input matrix.
        let mutable cnt = 0
        let ratios = ref ([| |] : (int * int * int * 'T) []) // Keeps the ratios and subtraction location with permutations for row subtraction. This will be used to construct L of Elimination.
        let permutations = ref ([| |] : (int * int) []) // Keeps the permutation informations. This will be used to construct P of Elimination.
        let diagonals = ref ([| |] : (int * int * 'T) []) // Keeps the diagonals produced. This will be used to construct D of Elimination.
        while (cnt < upperMatrix.rowCnt) do
            let checkPivot = upperMatrix.element.[cnt, cnt] <> LanguagePrimitives.GenericZero // Check the pivot of row.
            if checkPivot then // If pivot exists, i.e., not zero, eliminate one step.
                //printfn "Pivot exists. cnt=%d" cnt
                let mutable pivotLocation = cnt // Keeps location(row) of pivot. Default is 'cnt'.
                let mutable pivot = upperMatrix.element.[cnt, cnt]
                for idx1 = cnt+1 to upperMatrix.rowCnt-1 do // Find largest pivot available.
                    //printfn "Now finding largest pivot... idx1=%d" idx1
                    if genericAbs(pivot) < genericAbs(upperMatrix.element.[idx1, cnt]) && upperMatrix.element.[idx1, cnt] <> LanguagePrimitives.GenericZero then 
                        pivotLocation <- idx1
                        pivot <- upperMatrix.element.[idx1, cnt]
                if pivotLocation <> cnt then // When largest pivot is not default one, change row.
                    permutations.Value <- Array.append permutations.Value [| (cnt, pivotLocation) |] // Preserve row permutation information.
                    for idx1 = cnt to upperMatrix.rowCnt-1 do // Change current pivot row with newly selected one.
                        //printfn "Now changing row..."
                        let elemTemp = upperMatrix.element.[pivotLocation, idx1]
                        upperMatrix.element.[pivotLocation, idx1] <- upperMatrix.element.[cnt, idx1]
                        upperMatrix.element.[cnt, idx1] <- elemTemp
                diagonals.Value <- Array.append diagonals.Value [| (cnt, cnt, pivot) |] // Preserve diagonal information.
                //printfn "Successfully added pivot to diagonals."
                for idx1 = cnt+1 to upperMatrix.rowCnt-1 do // Do subtractions.
                    let tmp1 = upperMatrix.element.[idx1, cnt]
                    let tmp2 = upperMatrix.element.[cnt, cnt]
                    let ratio = upperMatrix.element.[idx1, cnt] / upperMatrix.element.[cnt, cnt] // Ratio of pivot and below-pivot element of row that is to be modified.
                    let mutable ratioTupleIdx0 = cnt
                    let mutable ratioTupleIdx1 = idx1
                    ratios.Value <- Array.append ratios.Value [| (permutations.Value.Length, ratioTupleIdx0, ratioTupleIdx1, ratio) |] // Add ratio to array of them.
                    for idx2 = cnt to upperMatrix.rowCnt-1 do
                        upperMatrix.element.[idx1, idx2] <- upperMatrix.element.[idx1, idx2] - upperMatrix.element.[cnt, idx2] * ratio
                for idx1 = cnt to upperMatrix.columnCnt - 1 do upperMatrix.element.[cnt, idx1] <- upperMatrix.element.[cnt, idx1] / pivot // Set pivot to 1.
                //printfn "Current upperMatrix: \n%A" (upperMatrix.Format())
                //for permutation in permutations.Value do 
                    //printfn "permutation: \n(%d, %d)" (fst permutation) (snd permutation)
                cnt <- cnt + 1
            else // When pivot does not exists, try to cure.
                let mutable findPivot = cnt + 1
                let mutable findBreak = false
                while (findPivot < upperMatrix.rowCnt && not findBreak) do // With below-pivot rows, find row that has appropriate pivot.
                    if upperMatrix.element.[findPivot, cnt] <> LanguagePrimitives.GenericZero then findBreak <- true
                    findPivot <- findPivot + 1
                if not findBreak then // When no row is available for cure, raise exception.
                    raise NoLDUDecompositionPossible
                else // When row is available for cure.
                    findPivot <- findPivot - 1
                    permutations.Value <- Array.append permutations.Value [| (cnt, findPivot) |] // Add permutation information.
                    let mutable changeRowCnt = 0
                    while (changeRowCnt < upperMatrix.rowCnt) do // Change current row with found one.
                        let changeRowTemp = upperMatrix.element.[cnt, changeRowCnt]
                        upperMatrix.element.[cnt, changeRowCnt] <- upperMatrix.element.[findPivot, changeRowCnt]
                        upperMatrix.element.[findPivot, changeRowCnt] <- changeRowTemp
                        changeRowCnt <- changeRowCnt + 1
        //printfn "Now generating lower, diagonal, permutation matrices..."
        let lowerMatrix = Matrix.Identity upperMatrix.rowCnt LanguagePrimitives.GenericOne<'T>
        let diagonalMatrix = Matrix.Identity upperMatrix.rowCnt LanguagePrimitives.GenericOne<'T>
        let permutationMatrix = Matrix.Identity upperMatrix.rowCnt LanguagePrimitives.GenericOne<'T>
        // For ratios perserved, apply permutations done after subtraction was done.
        // If P_21 * (L_20)^(-1) * P_10 is applied to A, then L_20 should be changed to L_10, due to permutation matrix P_21.
        // This indicates that subtraction of k times 0th row from 2nd row will be subtraction of k times 0th row from 1st row
        // after P_21 permutation matrix is applied. P_10 matrix, which is changing 0th row with 1st row, does not have any effect
        // for L_20 matrix, since P_21 * (L_20)^(-1) can be re-written as (L_10)^(-1) * P_21, which will contribute to getting PA=LDU form.
        // Note: P_ij matrix changes i-th row and j-th row, 
        //       L_ij matrix has element.[i, j] as ratio for subtracting j-th row times ratio from i-th row.
        //       P_ij * L_kl = L_kl(If k or l has value of i or j, change i to j, j to i.) * P_ij.
        //       Above also applies to (L_kl)^(-1).
        ratios.Value <- Array.map (fun x ->
                                            let skippedPermutationCount = (fst4 x)
                                            let mutable target0 = (snd4 x)
                                            let mutable target1 = (thd4 x)
                                            let ratio = (fth4 x)
                                            let appliedPermutations = permutations.Value.Skip(skippedPermutationCount).ToArray()
                                            for permutation in appliedPermutations do
                                                if target0 = (fst permutation) then target0 <- (snd permutation)
                                                else if target0 = (snd permutation) then target0 <- (fst permutation)
                                                if target1 = (fst permutation) then target1 <- (snd permutation)
                                                else if target1 = (snd permutation) then target1 <- (fst permutation)
                                            (skippedPermutationCount, target0, target1, ratio)
                                            ) ratios.Value
        for ratio in ratios.Value do 
            Array2D.set lowerMatrix.element (thd4 ratio) (snd4 ratio) (fth4 ratio) // Set lower matrix.
        for pivot in diagonals.Value do
            Array2D.set diagonalMatrix.element (fst3 pivot) (snd3 pivot) (thd3 pivot) // Set diagonal matrix.
        for permutation in permutations.Value do // Set permutation matrix. Apply permutations in order.
            let mutable colCnt = 0
            let target1 = fst permutation
            let target2 = snd permutation
            while (colCnt < upperMatrix.columnCnt) do
                let targetTemp = permutationMatrix.element.GetValue(target2, colCnt)
                permutationMatrix.element.SetValue(permutationMatrix.element.GetValue(target1, colCnt), target2, colCnt)
                permutationMatrix.element.SetValue(targetTemp, target1, colCnt)
                colCnt <- colCnt + 1
        LDUResult(permutationMatrix, lowerMatrix, diagonalMatrix, upperMatrix)

    /// <summary>Gets Row-reduced Echelon Form of given matrix and decompose.</summary>
    /// <param name="mat">Matrix to be eliminated and decomposed.</param>
    /// <returns>Returns decomposition result.</returns>
    let inline RREFdecomposition (mat : 'T matrix) =
        let fst3 (a, _, _) = a
        let snd3 (_, b, _) = b
        let thd3 (_, _, c) = c
        let fst4 (a, _, _, _) = a
        let snd4 (_, b, _, _) = b
        let thd4 (_, _, c, _) = c
        let fth4 (_, _, _, d) = d
        let genericAbs (a : 'T) =
            if a >= LanguagePrimitives.GenericZero then a else LanguagePrimitives.GenericZero - a
        let rrefRes = matrix<'T>(mat.rowCnt, mat.columnCnt, mat.element) // Copies input matrix

        // Keeps information of row-subtraction ratios and its location between permutations. 
        // This will be used to construct lower triangular matrix L.
        // (permutations before this subtraction) * (row to subtract) * (row to subtract from) * (row-subtraction ratio)
        let ratios = ref ([| |] : (int * int * int * 'T) [])

        // Keeps information of row-subtractions after echelon form is achieved.
        // This will be used to construct upper triangular matrix U.
        // (row to subtract) * (row to subtract from) * (row-subtraction ratio)
        let upperRatios = ref ([| |] : (int * int * 'T) [])

        // Keeps information of permutation.
        // This will be used to construct permutation matrix P.
        // (row to permute) * (row to be permuted)
        let permutations = ref ([| |] : (int * int) [])

        // Keeps information of diagonals.
        // This will be used to construct diagonal matrix D.
        // (row-number of diagonal element) * (column-number of diagonal element) * (element)
        let pivots = ref ([| |] : (int * int * 'T) [])

        let mutable rowCnt = 0
        let mutable columnCnt = 0

        // Downward elimination. Generates lower triangular matrix.
        while (rowCnt < mat.rowCnt && columnCnt < mat.columnCnt) do
            // Check for pivot.
            if (rrefRes.element.[rowCnt, columnCnt] <> LanguagePrimitives.GenericZero<'T>) then // When default pivot exists.
                // Find largest(absolute) pivot for stability.
                let mutable pivotLocation = rowCnt
                let mutable pivot = rrefRes.element.[rowCnt, columnCnt]
                Array.iteri (fun idx elem -> 
                    if (genericAbs pivot) < (genericAbs elem) then 
                        pivotLocation <- idx
                        pivot <- elem) rrefRes.element.[*, columnCnt]
                // If row-exchange is needed, change.
                if (pivotLocation <> rowCnt) then
                    permutations.Value <- Array.append permutations.Value [| (rowCnt, pivotLocation) |]
                    for i=0 to rrefRes.columnCnt-1 do
                        let elemTemp = rrefRes.element.[pivotLocation, i]
                        rrefRes.element.[pivotLocation, i] <- rrefRes.element.[rowCnt, i]
                        rrefRes.element.[rowCnt, i] <- elemTemp
                // Subtract rows.
                for i=rowCnt+1 to rrefRes.rowCnt-1 do
                    // Calculate ratio of subtraction.
                    let ratio = rrefRes.element.[i, columnCnt] / rrefRes.element.[rowCnt, columnCnt]
                    ratios.Value <- Array.append ratios.Value [| (permutations.Value.Length, rowCnt, i, ratio) |]
                    // Subtract from current row.
                    for j=columnCnt to rrefRes.columnCnt-1 do
                        rrefRes.element.[i, j] <- rrefRes.element.[i, j] - (rrefRes.element.[rowCnt, j] * ratio)
                // Preserve pivot information.
                pivots.Value <- Array.append pivots.Value [| (rowCnt, columnCnt, pivot) |]
                // Get ready for next iteration.
                rowCnt <- rowCnt + 1
                columnCnt <- columnCnt + 1
            else // When default pivot does not exist.
                // Try to cure.
                let mutable pivotLocation = rowCnt
                let mutable pivot = rrefRes.element.[rowCnt, columnCnt]
                Array.iteri (fun idx elem ->
                    if (genericAbs pivot) < (genericAbs elem) then
                        pivotLocation <- idx
                        pivot <- elem) rrefRes.element.[*, columnCnt]
                if (pivotLocation <> rowCnt) then // When cured successfully.
                    // Exchange rows and restart iteration.
                    permutations.Value <- Array.append permutations.Value [| (rowCnt, pivotLocation) |]
                    for i=0 to rrefRes.columnCnt-1 do
                        let elemTemp = rrefRes.element.[pivotLocation, i]
                        rrefRes.element.[pivotLocation, i] <- rrefRes.element.[rowCnt, i]
                        rrefRes.element.[rowCnt, i] <- elemTemp
                else // When cured is unsuccessful.
                    // Jump to next column.
                    columnCnt <- columnCnt + 1

        // Diagonal reducing.
        for pivot in pivots.Value do
            let pivotRow = fst3 pivot
            let pivotColumn = snd3 pivot
            let pivotValue = thd3 pivot
            for i=pivotColumn to mat.columnCnt-1 do
                rrefRes.element.[pivotRow, i] <- rrefRes.element.[pivotRow, i] / pivotValue

        // Subtraction for pivot-above elements. Generate upper triangular matrix.
        for pivot in pivots.Value.Reverse() do
            let pivotRow = fst3 pivot
            let pivotColumn = snd3 pivot
            let pivotValue = thd3 pivot
            // Subtract from each row above pivot.
            for i=pivotRow-1 downto 0 do
                let upperRatio = rrefRes.element.[i, pivotColumn] / pivotValue // Gets subtraction ratio.
                upperRatios.Value <- Array.append upperRatios.Value [| (pivotRow, i, upperRatio) |]
                // Subtract from current row.
                for j=pivotColumn to mat.columnCnt-1 do
                    rrefRes.element.[i, j] <- rrefRes.element.[i, j] - (rrefRes.element.[pivotRow, j] * upperRatio)

        // Initialize result matrices.
        let permutationMatrix = Matrix.Identity mat.rowCnt LanguagePrimitives.GenericOne<'T> // Initialize permutation matrix.
        let lowerMatrix = Matrix.Identity mat.rowCnt LanguagePrimitives.GenericOne<'T> // Initialize lower triangular matrix.
        let diagonalMatrix = Matrix.Identity mat.rowCnt LanguagePrimitives.GenericOne<'T> // Initialize diagonal matrix.
        let upperMatrix = Matrix.Identity mat.rowCnt LanguagePrimitives.GenericOne<'T> // Initialize upper triangular matrix.

        // Apply permutations to lower-subtraction ratios.
        ratios.Value <- Array.map (fun x ->
            let appliedPermutationsCount = fst4 x
            let mutable ratioIdx0 = snd4 x
            let mutable ratioIdx1 = thd4 x
            let ratio = fth4 x
            let appliedPermutations = permutations.Value.Skip(appliedPermutationsCount) // Get permutations to apply.
            for permutation in appliedPermutations do // Apply permutations.
                let permutationSrc0 = fst permutation
                let permutationSrc1 = snd permutation
                if ratioIdx0 = permutationSrc0 then ratioIdx0 <- permutationSrc1
                else if ratioIdx0 = permutationSrc1 then ratioIdx0 <- permutationSrc0
                if ratioIdx1 = permutationSrc0 then ratioIdx1 <- permutationSrc1
                else if ratioIdx1 = permutationSrc1 then ratioIdx1 <- permutationSrc0
            (appliedPermutationsCount, ratioIdx0, ratioIdx1, ratio)) ratios.Value

        // Set permutation matrix.
        for permutation in permutations.Value do
            let permutationSrc0 = fst permutation
            let permutationSrc1 = snd permutation
            for i=0 to permutationMatrix.columnCnt-1 do // Apply current permutation.
                let elemTemp = permutationMatrix.element.[permutationSrc1, i]
                permutationMatrix.element.[permutationSrc1, i] <- permutationMatrix.element.[permutationSrc0, i]
                permutationMatrix.element.[permutationSrc0, i] <- elemTemp

        // Set lower matrix.
        for ratio in ratios.Value do
            lowerMatrix.element.[(thd4 ratio), (snd4 ratio)] <- (fth4 ratio)

        // Set diagonal matrix.
        for pivot in pivots.Value do
            diagonalMatrix.element.[(fst3 pivot), (fst3 pivot)] <- (thd3 pivot)

        // Set upper matrix.
        for upperRatio in upperRatios.Value do
            upperMatrix.element.[(snd3 upperRatio), (fst3 upperRatio)] <- (thd3 upperRatio)

        // Return result.
        RREFResult<'T>(permutationMatrix, lowerMatrix, diagonalMatrix, upperMatrix, rrefRes)