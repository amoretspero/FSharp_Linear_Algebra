namespace FSharp_Linear_Algebra

[<Class>]
type matrix<'T> = 
    
    /// <summary>Generates matrix with given row count, column count and element.
    /// Parameter element should be provided as type of Array2D.</summary>
    /// <param name="rowCnt">Number of rows</param>
    /// <param name="columnCnt">Number of columns</param>
    /// <param name="element">Element to use for initialize matrix</param>
    new : rowCnt:int * columnCnt:int * element:'T [, ] -> 'T matrix

    /// <summary>Generates matrix with given row count and column count. All elements will be filled with given zero value.</summary>
    /// <param name="rowCnt">Number of rows</param>
    /// <param name="columnCnt">Number of columns</param>
    /// <param name="zero">Represents zero for type to use.</param>
    //new : rowCnt:int -> columnCnt:int -> zero:'T -> 'T matrix
    
    /// <summary>Gets number of rows.</summary>
    member rowCnt : int

    /// <summary>Gets number of columns.</summary>
    member columnCnt : int

    /// <summary>Gets elements of matrix.</summary>
    member element : 'T [, ]

    /// <summary>Prints out the matrix.</summary>
    member printMatrix : unit -> unit

    /// <summary>Transpose given matrix.</summary>
    member Transpose : unit -> 'T matrix



module Matrix =

    /// <summary>Multiplies matrix1 with matrix2 and returns its result.</summary>
    /// <param name="matrix1">Matrix to be multiplied. Left side.</param>
    /// <param name="matrix2">Matrix to be multiplied. Right side.</param>
    val inline Multiply : matrix1 : 'T matrix -> matrix2 : 'T matrix -> 'T matrix
        when 'T : (static member (*) : 'T * 'T -> 'T) and 
             'T : (static member (+) : 'T * 'T -> 'T) and 
             'T : (static member Zero : 'T)

    /// <summary>Add two matrices.</summary>
    /// <param name="matrix1">Matrix to be added. Left side.</param>
    /// <param name="matrix2">Matrix to be added. Right side.</param>
    val inline Add : matrix1 : 'T matrix -> matrix2 : 'T matrix -> 'T matrix
        when 'T : (static member (+) : 'T * 'T -> 'T)