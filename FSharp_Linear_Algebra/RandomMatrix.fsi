namespace FSharp_Linear_Algebra.Matrix

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