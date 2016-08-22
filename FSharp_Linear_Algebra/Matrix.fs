namespace FSharp_Linear_Algebra

open System
open System.Collections
open System.Collections.Generic
open System.Linq
open System.Numerics

/// <summary>Exception that indicates gauss elimination of given matrix is not possible.</summary>
exception NoGaussEliminationPossible
exception SizeUnmatch of int * int * int * int
exception NotSquare of int * int
exception FileNotFound of string

/// <summary>Class for generic matrices.</summary>
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
        do if matrix1.columnCnt <> matrix2.rowCnt then failwith "Matrix sizes does not match." // Check if matrix size matches. 
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

    /// <summary>Gauss eliminates given decimal matrix.</summary>
    /// <param name="mat">Matrix to be eliminated.</param>
    /// <returns>Returns eliminated result. For elimination of PA=LU, return value is (P * L * U), three-tuple.</returns>
    /// <exception cref="FSharp_Linear_Algebra.NoGaussEliminationPossible">Thrown when gauss elimination cannot be performed.</exception>
    let inline GaussEliminate (mat : 'T matrix) : 'T matrix * 'T matrix * 'T matrix =
        let fst3 (a, _, _) = a
        let snd3 (_, b, _) = b
        let thd3 (_, _, c) = c
        do if mat.columnCnt <> mat.rowCnt then raise (NotSquare(mat.columnCnt, mat.rowCnt)) // Check if matrix is square.
        let mutable cnt = 0
        let ratios = ref ([| |] : (int * int * 'T) []) // Keeps the ratios for row subtraction. This will be used to construct L of Elimination.
        let permutations = ref ([| |] : (int * int) []) // Keeps the permutation informations. This will be used to construct P of Elimination.
        while (cnt < mat.rowCnt) do
            let checkPivot = mat.element.[cnt, cnt] <> LanguagePrimitives.GenericZero // Check the pivot of row.
            if checkPivot then // If pivot exists, i.e., not zero, eliminate one step.
                for idx1 = cnt+1 to mat.rowCnt-1 do
                    let tmp1 = mat.element.[idx1, cnt]
                    let tmp2 = mat.element.[cnt, cnt]
                    let ratio = mat.element.[idx1, cnt] / mat.element.[cnt, cnt] // Ratio of pivot and below-pivot element of row that is to be modified.
                    ratios.Value <- Array.append ratios.Value [| (cnt, idx1, ratio) |] // Add ratio to array of them.
                    for idx2 = cnt to mat.rowCnt-1 do
                        mat.element.[idx1, idx2] <- mat.element.[idx1, idx2] - mat.element.[cnt, idx2] * ratio
                cnt <- cnt + 1
            else // When pivot does not exists, try to cure.
                let mutable findPivot = cnt + 1
                let mutable findBreak = false
                while (findPivot < mat.rowCnt && not findBreak) do // With below-pivot rows, find row that has appropriate pivot.
                    if mat.element.[findPivot, cnt] <> LanguagePrimitives.GenericZero then findBreak <- true
                    findPivot <- findPivot + 1
                if not findBreak then // When no row is available for cure, raise exception.
                    raise NoGaussEliminationPossible
                else // When row is available for cure.
                    findPivot <- findPivot - 1
                    permutations.Value <- Array.append permutations.Value [| (cnt, findPivot) |] // Add permutation information.
                    let mutable changeRowCnt = 0
                    while (changeRowCnt < mat.rowCnt) do // Change current row with found one.
                        let changeRowTemp = mat.element.[cnt, changeRowCnt]
                        mat.element.[cnt, changeRowCnt] <- mat.element.[findPivot, changeRowCnt]
                        mat.element.[findPivot, changeRowCnt] <- changeRowTemp
                        changeRowCnt <- changeRowCnt + 1
                    // Change row and column information in ratios array also.
                    ratios.Value <- Array.map (fun x -> 
                                                    let mutable newRow = fst3 x
                                                    let mutable newCol = snd3 x
                                                    if (fst3 x) = cnt then newRow <- findPivot
                                                    else if (fst3 x) = findPivot then newRow <- cnt
                                                    if (snd3 x) = cnt then newCol <- findPivot
                                                    else if (snd3 x) = findPivot then newCol <- cnt
                                                    (newRow, newCol, (thd3 x))) ratios.Value
        let lowerMatrix = Identity mat.rowCnt LanguagePrimitives.GenericOne<'T>
        let permutationMatrix = Identity mat.rowCnt LanguagePrimitives.GenericOne<'T>
        for ratio in ratios.Value do 
            Array2D.set lowerMatrix.element (snd3 ratio) (fst3 ratio) (thd3 ratio) // Set lower matrix.
        for permutation in permutations.Value do // Set permutation matrix.
            let mutable colCnt = 0
            let target1 = fst permutation
            let target2 = snd permutation
            while (colCnt < mat.columnCnt) do
                let targetTemp = permutationMatrix.element.GetValue(target2, colCnt)
                permutationMatrix.element.SetValue(permutationMatrix.element.GetValue(target1, colCnt), target2, colCnt)
                permutationMatrix.element.SetValue(targetTemp, target1, colCnt)
        (permutationMatrix, lowerMatrix, mat)

    /// <summary>Generates int32 random matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    let RandomMatrixInt32 (row : int) (col : int) : int32 matrix =
        let rnd = new System.Random()
        let elem = Array2D.init row col (fun _ _ -> rnd.Next())
        matrix<int32>(row, col, elem)

    /// <summary>Generates double matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    let RandomMatrixDouble (row : int) (col : int) : double matrix =
        let rnd = new System.Random()
        let elem = Array2D.init row col (fun _ _ -> rnd.NextDouble())
        matrix<double>(row, col, elem)