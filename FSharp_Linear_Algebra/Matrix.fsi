namespace FSharp_Linear_Algebra.Matrix

/// <summary>Exception that indicates gauss elimination of given matrix is not possible.</summary>
exception NoLDUDecompositionPossible

[<Class>]
/// <summary>Class for generic matrices.</summary>
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

    /// <summary>Format the matrix.</summary>
    member Format : unit -> string

///<summary>Class for random-generated matrices.</summary>
[<Class>]
type RandomMatrix =
    
    /// <summary>Generates Byte random matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member RandomMatrixByte : row : int -> col : int -> byte matrix
    
    /// <summary>Generates int32 random matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member RandomMatrixInt32 : row : int -> col : int -> int32 matrix

    /// <summary>Generates int64 random matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member RandomMatrixInt64 : row : int -> col : int -> int64 matrix

    /// <summary>Generates single(float32) matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member RandomMatrixSingle : row : int -> col : int -> single matrix

    /// <summary>Generates double matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member RandomMatrixDouble : row : int -> col : int -> double matrix

    /// <summary>Generates decimal matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member RandomMatrixDecimal : row : int -> col : int -> decimal matrix


/// <summary>Module for operations on matrices.</summary>
module Matrix =

    /// <summary>Write matrix to file.</summary>
    /// <param name="location">File location to write matrix.</param>
    val WriteToFile : mat : 'T matrix -> location : string -> unit

    /// <summary>Reads int32 matrix from file. Input file should contain one row at one line.
    /// Whitespace or tab seperators are both legal.</summary>
    /// <param name="location">File location to read matrix from.</param>
    val ReadFromFileInt32 : location : string -> int32 matrix

    /// <summary>Reads double matrix from file. Input file should contain one row at one line.
    /// Whitespace or tab seperators are both legal.</summary>
    /// <param name="location">File location to read matrix from.</param>
    val ReadFromFileDouble : location : string -> double matrix

    /// <summary>Multiplies matrix1 with matrix2 and returns its result.</summary>
    /// <param name="matrix1">Matrix to be multiplied. Left side.</param>
    /// <param name="matrix2">Matrix to be multiplied. Right side.</param>
    /// <returns>Returns matrix multiplied. Size is (# of rows in matrix1) * (# of columns in matrix2)</returns>
    val inline Multiply : matrix1 : 'T matrix -> matrix2 : 'T matrix -> 'T matrix
        when 'T : (static member (*) : 'T * 'T -> 'T) and 
             'T : (static member (+) : 'T * 'T -> 'T) and 
             'T : (static member Zero : 'T)

    /// <summary>Add two matrices.</summary>
    /// <param name="matrix1">Matrix to be added. Left side.</param>
    /// <param name="matrix2">Matrix to be added. Right side.</param>
    /// <returns>Returns the addition result of two matrices.</returns>
    val inline Add : matrix1 : 'T matrix -> matrix2 : 'T matrix -> 'T matrix
        when 'T : (static member (+) : 'T * 'T -> 'T)

    /// <summary>Subtract one matrix from another.</summary>
    /// <param name="matrix1">Matrix to be subtracted from. Left side.</param>
    /// <param name="matrix2">Matrix to subtract. Right side. </param>
    /// <returns>Returns the subtraction result of two matrices.</returns>
    val inline Subtract : matrix1 : 'T matrix -> matrix2 : 'T matrix -> 'T matrix
        when 'T : (static member (-) : 'T * 'T -> 'T)

    /// <summary>Transpose given matrix.</summary>
    /// <param name="matrix1">Matrix to be transposed.</param>
    /// <returns>Returns the transposed matrix.</returns>
    val Transpose : matrix1 : 'T matrix -> 'T matrix
    
    /// <summary>Multiply by constant.</summary>
    /// <param name="c">Constant factor to be multiplied to given matrix.</param>
    /// <param name="matrix1">Matrix to be multiplied by "c".</param>
    /// <returns>Returns the matrix multiplied by constant.</returns>
    val inline ScalarMultiply : c : 'T -> matrix1 : 'T matrix -> 'T matrix
        when 'T : (static member (*) : 'T * 'T -> 'T)

    /// <summary>Creates identity matrix with given size.</summary>
    /// <param name="size">Size for row and column.</param>
    /// <param name="one">One of type 'T.</param>
    /// <returns>Returns identity matrix whose size of "size" * "size".</returns>
    val inline Identity : size : int -> one : 'T -> 'T matrix
        when 'T : (static member Zero : 'T) and
             'T : (static member One : 'T) and
             'T : comparison

    /// <summary>Gauss eliminates given decimal matrix.</summary>
    /// <param name="mat">Matrix to be eliminated.</param>
    /// <returns>Returns eliminated result.</returns>
    /// <exception cref="FSharp_Linear_Algebra.NoGaussEliminationPossible">Thrown when gauss elimination cannot be performed.</exception>
    val inline LDUdecomposition : mat:matrix<'T> -> matrix<'T> * matrix<'T> * matrix<'T> * matrix<'T>
        when 'T : (static member Zero : 'T) and 
             'T : (static member ( / ) : 'T * 'T -> 'T) and 
             ('T or 'a) : (static member ( - ) : 'T * 'a -> 'T) and 
             'T : (static member ( * ) : 'T * 'T -> 'a) and 
             'T : (static member One : 'T) and 
             'T : comparison