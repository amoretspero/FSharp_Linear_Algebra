namespace FSharp_Linear_Algebra.Matrix

open System
open System.Collections
open System.Collections.Generic
open System.Linq
open System.Numerics

/// <summary>Exception that indicates given two matrices' size does not match.</summary>
exception SizeUnmatch of int * int * int * int

/// <summary>Exception that indicates given matrix is not square.</summary>
exception NotSquare of int * int

/// <summary>Exception that file is not found.</summary>
exception FileNotFound of string

/// <summary>Exception that given matrix does not have inverse.</summary>
exception NotInvertible

/// <summary>Exception that given matrix does not have right inverse.</summary>
exception NoRightInverse

/// <summary>Exception that given matrix does not have left inverse.</summary>
exception NoLeftInverse

/// <summary>Class for generic matrices.</summary>
[<Class>]
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

    member mat.rowCnt 
        with get() = _rowCnt

    member mat.columnCnt 
        with get() = _columnCnt

    member mat.element 
        with get() = _element

    // Instance methods ---------------------------------------------

    /// <summary>Formats matrix to string, with tab seperator.</summary>
    /// <returns>Formatted matrix, one row in one line.</returns>
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

    /// <summary>Generates matrix with given row count and column count. All elements will be filled with given zero value.</summary>
    /// <param name="rowCnt">Number of rows</param>
    /// <param name="columnCnt">Number of columns</param>
    /// <param name="zero">Represents zero for type to use.</param>
    new (rowCnt : int, columnCnt : int, zero : 'T) = 
        let rnd = System.Random()
        let array2d = Array2D.create rowCnt columnCnt zero
        matrix<'T>(rowCnt, columnCnt, array2d)

    /// <summary>Generates matrix with given array of array of elements.</summary>
    /// <param name="elem">Array of array of 'T elements.</param>
    new (elem : 'T [] []) =
        do if elem.Length <= 0 then failwith "Parameter for element is empty!"
        do if elem.[0].Length <= 0 then failwith "Parameter for element has length zero row!"
        let rowCnt = elem.Length
        let columnCnt = elem.[0].Length
        let array2d = elem |> array2D
        matrix<'T>(rowCnt, columnCnt, array2d)
        

/// <summary>Module for operations on matrices.</summary>
module Matrix =
    
    /// <summary>Write matrix to file.</summary>
    /// <param name="location">File location to write matrix.</param>
    let WriteToFile (mat : 'T matrix) (location : string) =
        let formatted = mat.Format()
        System.IO.File.WriteAllText(location, formatted)

    /// <summary>Reads int32 matrix from file. Input file should contain one row at one line.
    /// Whitespace or tab seperators are both legal.</summary>
    /// <param name="location">File location to read matrix from.</param>
    let ReadFromFileInt32 (location : string) : int32 matrix =
        if not (System.IO.File.Exists(location)) then raise (FileNotFound("File not found : " + location))
        let raw = System.IO.File.ReadAllLines(location)
        let midArray = Array.map (fun (x : string) -> x.Split([| " "; "\t"; System.Environment.NewLine |], StringSplitOptions.RemoveEmptyEntries)) raw
        if (midArray.Length <= 0) then failwith "Matrix file is empty."
        let firstLen = midArray.[0].Length
        for elem in midArray do
            if elem.Length <> firstLen then failwith "All rows should have same length."
        matrix<int32>(Array.map (fun (x : string []) -> Array.map (fun (y : string) -> System.Convert.ToInt32(y)) x) midArray)

    /// <summary>Reads double matrix from file. Input file should contain one row at one line.
    /// Whitespace or tab seperators are both legal.</summary>
    /// <param name="location">File location to read matrix from.</param>
    let ReadFromFileDouble (location : string) : double matrix =
        if not (System.IO.File.Exists(location)) then raise (FileNotFound("File not found : " + location))
        let raw = System.IO.File.ReadAllLines(location)
        let midArray = Array.map (fun (x : string) -> x.Split([| " "; "\t"; System.Environment.NewLine |], StringSplitOptions.RemoveEmptyEntries)) raw
        if (midArray.Length <= 0) then failwith "Matrix file is empty."
        let firstLen = midArray.[0].Length
        for elem in midArray do
            if elem.Length <> firstLen then failwith "All rows should have same length."
        matrix<double>(Array.map (fun (x : string []) -> Array.map (fun (y : string) -> System.Convert.ToDouble(y)) x) midArray)

    /// <summary>Multiplies matrix1 with matrix2 and returns its result.</summary>
    /// <param name="matrix1">Matrix to be multiplied. Left side.</param>
    /// <param name="matrix2">Matrix to be multiplied. Right side.</param>
    /// <returns>Returns matrix multiplied. Size is (# of rows in matrix1) * (# of columns in matrix2)</returns>
    let inline Multiply (matrix1 : 'T matrix) (matrix2 : 'T matrix) =
        do if matrix1.columnCnt <> matrix2.rowCnt then failwith ("Matrix size does not match. Attempted ["+(matrix1.rowCnt.ToString())+" by "+(matrix1.columnCnt.ToString())+"] * ["+(matrix2.rowCnt.ToString())+" by "+(matrix2.columnCnt.ToString())+"].") // Check if matrix size matches. 
        let res = Array2D.create matrix1.rowCnt matrix2.columnCnt LanguagePrimitives.GenericZero // Create 2d-array for matrix element with generic zero.
        for i=1 to matrix1.rowCnt do
            let row = matrix1.element.[i-1, *]
            for j=1 to matrix2.columnCnt do
                let column = matrix2.element.[*, j-1]
                let sum = (Array.map2 (fun elem1 elem2 -> elem1 * elem2) row column) |> Array.sum
                res.[i-1, j-1] <- sum 
        matrix<'T>(matrix1.rowCnt, matrix2.columnCnt, res)

    /// <summary>Add two matrices.</summary>
    /// <param name="matrix1">Matrix to be added. Left side.</param>
    /// <param name="matrix2">Matrix to be added. Right side.</param>
    /// <returns>Returns the addition result of two matrices.</returns>
    let inline Add (matrix1 : 'T matrix) (matrix2 : 'T matrix) =
        do if matrix1.rowCnt <> matrix2.rowCnt || matrix1.columnCnt <> matrix2.columnCnt then raise (SizeUnmatch(matrix1.rowCnt, matrix1.columnCnt, matrix2.rowCnt, matrix2.columnCnt)) // Check if matrix size matches.
        let rowCnt = matrix1.rowCnt
        let columnCnt = matrix1.columnCnt
        let resArray = Array2D.init rowCnt columnCnt (fun idx1 idx2 -> matrix1.element.[idx1, idx2] + matrix2.element.[idx1, idx2]) // Generate result.
        matrix<'T>(rowCnt, columnCnt, resArray)

    /// <summary>Subtract one matrix from another.</summary>B
    /// <param name="matrix1">Matrix to be subtracted from. Left side.</param>
    /// <param name="matrix2">Matrix to subtract. Right side. </param>
    /// <returns>Returns the subtraction result of two matrices.</returns>
    let inline Subtract (matrix1 : 'T matrix) (matrix2 : 'T matrix) =
        do if matrix1.rowCnt <> matrix2.rowCnt || matrix1.columnCnt <> matrix2.columnCnt then raise (SizeUnmatch(matrix1.rowCnt, matrix1.columnCnt, matrix2.rowCnt, matrix2.columnCnt)) // Check if matrix size matches.
        let rowCnt = matrix1.rowCnt
        let columnCnt = matrix1.columnCnt
        let resArray = Array2D.init rowCnt columnCnt (fun idx1 idx2 -> matrix1.element.[idx1, idx2] - matrix2.element.[idx1, idx2]) // Generate result.
        matrix<'T>(rowCnt, columnCnt, resArray)

    /// <summary>Transpose given matrix.</summary>
    /// <param name="matrix1">Matrix to be transposed.</param>
    /// <returns>Returns the transposed matrix.</returns>
    let Transpose (matrix1 : 'T matrix) =
        let rowCnt = matrix1.columnCnt
        let columnCnt = matrix1.rowCnt
        let resArray = Array2D.init rowCnt columnCnt (fun idx1 idx2 -> matrix1.element.[idx2, idx1]) // Generate result, transposed.
        matrix<'T>(rowCnt, columnCnt, resArray)
        
    /// <summary>Multiply by constant.</summary>
    /// <param name="c">Constant factor to be multiplied to given matrix.</param>
    /// <param name="matrix1">Matrix to be multiplied by "c".</param>
    /// <returns>Returns the matrix multiplied by constant.</returns>
    let inline ScalarMultiply (c : 'T) (matrix1 : 'T matrix) =
        matrix<'T>(matrix1.rowCnt, matrix1.columnCnt, (Array2D.init matrix1.rowCnt matrix1.columnCnt (fun idx1 idx2 -> matrix1.element.[idx1, idx2] * c))) // Generate result, multiplied by scalar value.

    /// <summary>Creates identity matrix with given size.</summary>
    /// <param name="size">Size for row and column.</param>
    /// <param name="one">One of type 'T.</param>
    /// <returns>Returns identity matrix whose size of "size" * "size".</returns>
    let inline Identity (size : int) (one : 'T) =
        do if LanguagePrimitives.GenericComparison one LanguagePrimitives.GenericOne <> 0 then failwith "one is not one!" // Check if provided one is really one.
        let res = Array2D.create size size LanguagePrimitives.GenericZero // Initialize result matrix with generic zero.
        for i=1 to size do
            res.[i-1, i-1] <- one // Only diagonal elements should be one.
        matrix<'T>(size, size, res)