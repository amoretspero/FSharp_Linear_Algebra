namespace FSharp_Linear_Algebra

open System
open System.Collections
open System.Collections.Generic
open System.Numerics


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

    member mat.printMatrix() =
        if _rowCnt >= 10 || _columnCnt >= 10 then
            printf "Matrix is too large to print. Size : %d * %d\n" _rowCnt _columnCnt
        else
            for i=1 to _rowCnt do
                for j=1 to _columnCnt do
                    printf "%A\t" _element.[i-1, j-1]
                printf "\n"
    
    member mat.Transpose() =
        let columns = [| for i in 1 .. mat.columnCnt -> mat.element.[*, i-1] |]
        matrix<'T>(mat.columnCnt, mat.rowCnt, (Array2D.init mat.columnCnt mat.rowCnt (fun idx1 idx2 -> columns.[idx1].[idx2])))


    // Static properties --------------------------------------------

    

    // Static methods -----------------------------------------------
                    

    // Explicit Constructors ----------------------------------------

    new (rowCnt : int, columnCnt : int, zero : 'T) = 
        let rnd = System.Random()
        let array2d = Array2D.create rowCnt columnCnt zero
        matrix<'T>(rowCnt, columnCnt, array2d)


module Matrix =
    let inline Multiply (matrix1 : 'T matrix) (matrix2 : 'T matrix) =
        do if matrix1.columnCnt <> matrix2.rowCnt then failwith "Matrix sizes do not match."
        let res = Array2D.create matrix1.rowCnt matrix2.columnCnt LanguagePrimitives.GenericZero
        for i=1 to matrix1.rowCnt do
            let row = matrix1.element.[i-1, *]
            for j=1 to matrix2.columnCnt do
                let column = matrix2.element.[*, j-1]
                let sum = (Array.map2 (fun elem1 elem2 -> elem1 * elem2) row column) |> Array.sum
                res.[i-1, j-1] <- sum
        matrix<'T>(matrix1.rowCnt, matrix2.columnCnt, res)