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


/// <summary>Module containing various decompositions.</summary>
module Decomposition =
    
    /// <summary>LDU-decompose given matrix.</summary>
    /// <param name="mat">Matrix to be decomposed.</param>
    /// <returns>Returns decomposed result. For decomposition of PA=LDU, return value is (P * L * D * U), four-tuple.</returns>
    /// <exception cref="FSharp_Linear_Algebra.Matrix.Decomposition.NoLDUDecompositionPossible">Thrown whenLDU decomposition cannot be performed.</exception>
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