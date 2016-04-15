namespace FSharp_Linear_Algebra

open System
open System.Collections
open System.Collections.Generic
open System.Numerics


/// <summary>Class for float matrix and its basic operations.</summary>
type Matrix(rowCnt : int, columnCnt : int, element : float [,]) =

    // Private data -------------------------------------------------

    /// Random generator.
    let rnd = System.Random()

    /// Elements of matrix. Stored with type array of array of int.
    let _element = element

    /// Number of rows in matrix.
    let _rowCnt = rowCnt

    /// Number of columns in matrix.
    let _columnCnt = columnCnt

    // Instance properties ------------------------------------------

    /// <summary>Gets number of rows.</summary>
    member mat.rowCnt = _rowCnt

    /// <summary>Gets number of columns.</summary>
    member mat.columnCnt = _columnCnt

    /// <summary>Gets elements of matrix.</summary>
    member mat.element = _element

    // Instance methods ---------------------------------------------

    /// <summary>Prints out the matrix.</summary>
    member mat.printMatrix() =
        if _rowCnt >= 10 || _columnCnt >= 10 then
            printf "Matrix is too large to print. Size : %d * %d\n" _rowCnt _columnCnt
        else
            for i=1 to _rowCnt do
                for j=1 to _columnCnt do
                    printf "%2.14f\t" _element.[i-1, j-1]
                printf "\n"
    
    /// <summary>Transpose given matrix.</summary>
    member mat.Transpose() =
        let columns = [| for i in 1 .. mat.columnCnt -> mat.element.[*, i-1] |]
        Matrix(mat.columnCnt, mat.rowCnt, (Array2D.init mat.columnCnt mat.rowCnt (fun idx1 idx2 -> columns.[idx1].[idx2])))


    // Static properties --------------------------------------------

    

    // Static methods -----------------------------------------------

    /// <summary>Generate zero matrix.</summary>
    /// <param name="rowCnt">Number of rows in matrix.</param>
    /// <param name="columnCnt">Number of columns in matrix.</param>
    static member zeroMatrix(rowCnt : int, columnCnt : int) = Matrix(rowCnt, columnCnt, (Array2D.create rowCnt columnCnt 0.0))

    /// <summary>Multiplies matrix1 with matrix2 and returns its result.</summary>
    /// <param name="matrix1">Matrix to be multiplied. Left side.</param>
    /// <param name="matrix2">Matrix to be multiplied. Right side.</param>
    static member Multiply(matrix1 : Matrix, matrix2 : Matrix) =
        do if matrix1.columnCnt <> matrix2.rowCnt then failwith "Matrix sizes do not match."
        let res = Array2D.create matrix1.rowCnt matrix2.columnCnt 0.0
        for i=1 to matrix1.rowCnt do
            let row = matrix1.element.[i-1, *]
            for j=1 to matrix2.columnCnt do
                let column = matrix2.element.[*, j-1]
                let sum = (Array.map2 (fun elem1 elem2 -> elem1 * elem2) row column) |> Array.sum
                res.[i-1, j-1] <- sum
        Matrix(matrix1.rowCnt, matrix2.columnCnt, res)
                    


    // Explicit Constructors ----------------------------------------

    /// <summary>Generates Matrix with given row count and column count. All elements will be filled with random float from 0 (inclusive) to 1 (exclusive).</summary>
    /// <param name="rowCnt">Number of rows</param>
    /// <param name="columnCnt">Number of columns</param>
    new (rowCnt : int, columnCnt : int) = 
        let rnd = System.Random()
        let array2d = Array2D.create rowCnt columnCnt 0.0
        for i=1 to rowCnt do
            for j=1 to columnCnt do
                array2d.[i-1, j-1] <- rnd.NextDouble()
        Matrix(rowCnt, columnCnt, array2d)

