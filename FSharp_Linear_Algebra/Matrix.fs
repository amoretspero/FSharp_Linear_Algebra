﻿namespace FSharp_Linear_Algebra

open System
open System.Collections
open System.Collections.Generic
open System.Numerics

exception NoGaussEliminationPossible 
exception SizeUnmatch of int * int * int * int
exception NotSquare of int * int

/// <summary>Class for generic matrix and its basic operations.</summary>
type matrix<'T>(rowCnt : int, columnCnt : int, element : 'T [,]) =

    // Private data -------------------------------------------------

    /// <summary>Random generator.</summary>
    let rnd = System.Random()

    /// <summary>Elements of matrix. Stored with type array of array of int.</summary>
    let _element = element

    /// <summary>Number of rows in matrix.</summary>
    let _rowCnt = rowCnt

    /// <summary>Number of columns in matrix.</summary>
    let _columnCnt = columnCnt

    // Instance properties ------------------------------------------

    member mat.rowCnt = _rowCnt

    member mat.columnCnt = _columnCnt

    member mat.element = _element

    // Instance methods ---------------------------------------------

    member mat.Format() =
            let sb = System.Text.StringBuilder()
            for i = 1 to _rowCnt do
                for j = 1 to _columnCnt do
                    sb.AppendFormat("{0}\t", _element.[i - 1, j - 1]) |> ignore
                sb.AppendLine() |> ignore
            sb.ToString()

    // Static properties --------------------------------------------

    

    // Static methods -----------------------------------------------
                    

    // Explicit Constructors ----------------------------------------

    new (rowCnt : int, columnCnt : int, zero : 'T) = 
        let rnd = System.Random()
        let array2d = Array2D.create rowCnt columnCnt zero
        matrix<'T>(rowCnt, columnCnt, array2d)

    new (elem : 'T [] []) =
        do if elem.Length <= 0 then failwith "Parameter for element is empty!"
        do if elem.[0].Length <= 0 then failwith "Parameter for element has length zero row!"
        let rowCnt = elem.Length
        let columnCnt = elem.[0].Length
        let array2d = elem |> array2D
        matrix<'T>(rowCnt, columnCnt, array2d)
        


module Matrix =
    let inline Multiply (matrix1 : 'T matrix) (matrix2 : 'T matrix) =
        do if matrix1.columnCnt <> matrix2.rowCnt then failwith "Matrix sizes does not match." // Check if matrix size matches. 
        let res = Array2D.create matrix1.rowCnt matrix2.columnCnt LanguagePrimitives.GenericZero // Create 2d-array for matrix element with generic zero.
        for i=1 to matrix1.rowCnt do
            let row = matrix1.element.[i-1, *]
            for j=1 to matrix2.columnCnt do
                let column = matrix2.element.[*, j-1]
                let sum = (Array.map2 (fun elem1 elem2 -> elem1 * elem2) row column) |> Array.sum
                res.[i-1, j-1] <- sum 
        matrix<'T>(matrix1.rowCnt, matrix2.columnCnt, res)

    let inline Add (matrix1 : 'T matrix) (matrix2 : 'T matrix) =
        do if matrix1.rowCnt <> matrix2.rowCnt || matrix1.columnCnt <> matrix2.columnCnt then raise (SizeUnmatch(matrix1.rowCnt, matrix1.columnCnt, matrix2.rowCnt, matrix2.columnCnt)) // Check if matrix size matches.
        let rowCnt = matrix1.rowCnt
        let columnCnt = matrix1.columnCnt
        let resArray = Array2D.init rowCnt columnCnt (fun idx1 idx2 -> matrix1.element.[idx1, idx2] + matrix2.element.[idx1, idx2]) // Generate result.
        matrix<'T>(rowCnt, columnCnt, resArray)

    let inline Subtract (matrix1 : 'T matrix) (matrix2 : 'T matrix) =
        do if matrix1.rowCnt <> matrix2.rowCnt || matrix1.columnCnt <> matrix2.columnCnt then raise (SizeUnmatch(matrix1.rowCnt, matrix1.columnCnt, matrix2.rowCnt, matrix2.columnCnt)) // Check if matrix size matches.
        let rowCnt = matrix1.rowCnt
        let columnCnt = matrix1.columnCnt
        let resArray = Array2D.init rowCnt columnCnt (fun idx1 idx2 -> matrix1.element.[idx1, idx2] - matrix2.element.[idx1, idx2]) // Generate result.
        matrix<'T>(rowCnt, columnCnt, resArray)

    let Transpose (matrix1 : 'T matrix) =
        let rowCnt = matrix1.columnCnt
        let columnCnt = matrix1.rowCnt
        let resArray = Array2D.init rowCnt columnCnt (fun idx1 idx2 -> matrix1.element.[idx2, idx1]) // Generate result, transposed.
        matrix<'T>(rowCnt, columnCnt, resArray)
        
    let inline ScalarMultiply (c : 'T) (matrix1 : 'T matrix) =
        matrix<'T>(matrix1.rowCnt, matrix1.columnCnt, (Array2D.init matrix1.rowCnt matrix1.columnCnt (fun idx1 idx2 -> matrix1.element.[idx1, idx2] * c))) // Generate result, multiplied by scalar value.

    let inline Identity (size : int) (one : 'T) =
        do if LanguagePrimitives.GenericComparison one LanguagePrimitives.GenericOne <> 0 then failwith "one is not one!" // Check if provided one is really one.
        let res = Array2D.create size size LanguagePrimitives.GenericZero // Initialize result matrix with generic zero.
        for i=1 to size do
            res.[i-1, i-1] <- one // Only diagonal elements should be one.
        matrix<'T>(size, size, res)

    let GaussEliminate (mat : decimal matrix) =
        do if mat.columnCnt <> mat.rowCnt then raise (NotSquare(mat.columnCnt, mat.rowCnt)) // Check if matrix is square.
        let mutable cnt = 0
        while (cnt < mat.rowCnt) do
            let checkPivot = mat.element.[cnt, cnt] <> 0M // Check the pivot of row.
            if checkPivot then // If pivot exists, i.e., not zero, eliminate one step.
                for idx1 = cnt+1 to mat.rowCnt-1 do
                    let ratio = mat.element.[idx1, cnt] / mat.element.[cnt, cnt] // Ratio of pivot and below-pivot element of row that is to be modified.
                    for idx2 = cnt to mat.rowCnt-1 do
                        mat.element.[idx1, idx2] <- mat.element.[idx1, idx2] - mat.element.[cnt, idx2] * ratio
                cnt <- cnt + 1
            else // When pivot does not exists, try to cure.
                let mutable findPivot = cnt + 1
                let mutable findBreak = false
                while (findPivot < mat.rowCnt && not findBreak) do // With below-pivot rows, find row that has appropriate pivot.
                    if mat.element.[findPivot, cnt] <> 0M then findBreak <- true
                    findPivot <- findPivot + 1
                if not findBreak then // When no row is available for cure, raise exception.
                    raise NoGaussEliminationPossible
                else // When row is available for cure.
                    findPivot <- findPivot - 1
                    let mutable changeRowCnt = 0
                    while (changeRowCnt < mat.rowCnt) do // Change current row with found one.
                        let changeRowTemp = mat.element.[cnt, changeRowCnt]
                        mat.element.[cnt, changeRowCnt] <- mat.element.[findPivot, changeRowCnt]
                        mat.element.[findPivot, changeRowCnt] <- changeRowTemp
                        changeRowCnt <- changeRowCnt + 1
        mat

                    