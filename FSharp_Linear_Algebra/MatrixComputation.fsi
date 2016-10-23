namespace FSharp_Linear_Algebra.Matrix.Computation

open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Vector

module Matrix =

    /// <summary>Computes rank of given matrix with RREF-decomposition.</summary>
    /// <param name="mat">Matrix to compute rank.</param>
    /// <returns>Returns rank of given matrix.</returns>
    val inline Rank : mat : 'T matrix -> int // TODO: inline conditions.

    /// <summary>Check if right inverse of given matrix exists.</summary>
    /// <param name="mat">Matrix to check if right inverse exists.</param>
    /// <returns>True if right inverse exists, false if it does not.</returns>
    let inline IsRightInverseExist : mat : 'T matrix -> bool // TODO: inline conditions.  

    /// <summary>Check if left inverse of given matrix exists.</summary>
    /// <param name="mat">Matrix to check if left inverse exists.</param>
    /// <returns>True if left inverse exists, false if it does not.</returns>
    let inline IsLeftInverseExist : mat : 'T matrix -> bool // TODO: inline conditions.
    
    /// <summary>Computes inverse of given matrix by LDU-decomposition.</summary>
    /// <param name="mat">Matrix to compute inverse of.</param>
    /// <returns>Returns the inverse when invertible.</returns>
    /// <exception cref="FSharp_Linear_Algebra.Matrix.NotInvertible">Thrown when given matrix is not invertible.</exception>
    val inline Inverse : mat : 'T matrix -> 'T matrix
        when 'T : comparison and 
             'T : (static member Zero : 'T) and 
             ('a or 'T) : (static member ( - ) : 'a * 'T -> 'T) and 
             'T : (static member ( - ) : 'T * 'T -> 'T) and 
             'T : (static member ( * ) : 'T * 'T -> 'T) and 
             'T : (static member One : 'T) and 
             'T : (static member ( / ) : 'T * 'T -> 'T) and 
             'T : (static member ( + ) : 'T * 'T -> 'T) and 
             'a : (static member Zero : 'a)

    /// <summary>Computes right inverse of given matrix if exists.</summary>
    /// <param name="mat">Matrix to compute right inverse of.</param>
    /// <returns>Right inverse of given matrix.</returns>
    /// <exception cref="FSharp_Linear_Algebra.Matrix.NoRightInverse">Thrown when given matrix does not have right inverse.</exception>
    let inline RightInverse : mat : 'T matrix -> 'T matrix

    /// <summary>Computes left inverse of given matrix if exists.</summary>
    /// <param name="mat">Matrix to compute left inverse of.</param>
    /// <returns>Left inverse of given matrix.</returns>
    /// exception cref="FSharp_Linear_Algebra.Matrix.NoLeftInverse">Thrown when given matrix does not have left inverse.</exception>
    let inline LeftInverse : mat : 'T matrix -> 'T matrix

    /// <summary>Computes column space of given matrix.</summary>
    /// <param name="mat">Matrix to compute column space of.</param>
    /// <returns>Returns column space of given matrix as array of column vectors.</returns>
    val inline ColumnSpace : mat : 'T matrix -> 'T vector []
        when 'T : comparison and 
        'T : (static member Zero : 'T) and 
        ('a or 'T) : (static member ( - ) : 'a * 'T -> 'T) and 
        'T : (static member ( / ) : 'T * 'T -> 'T) and 
        ('T or 'b) : (static member ( - ) : 'T * 'b -> 'T) and 
        'T : (static member ( * ) : 'T * 'T -> 'b) and 
        'T : (static member One : 'T) and 
        'a : (static member Zero : 'a)
    
    /// <summary>Computes row space of given matrix.</summary>
    /// <param name="mat">Matrix to compute row space of.</param>
    /// <returns>Returns row space of given matrix as array of row vectors.</returns>
    val inline RowSpace : mat : 'T matrix -> 'T vector [] // TODO: inline conditions.

    /// <summary>Computes null space of given matrix.</summary>
    /// <param name="mat">Matrix to compute null space of.</param>
    /// <returns>Returns null space of given matrix as array of column vectors.</returns>
    val inline NullSpace : mat : 'T matrix -> 'T vector []
        when 'T : comparison and 
             'T : (static member Zero : 'T) and 
             ('a or 'T) : (static member ( - ) : 'a * 'T -> 'T) and 
             'T : (static member ( / ) : 'T * 'T -> 'T) and 
             ('T or 'b) : (static member ( - ) : 'T * 'b -> 'T) and 
             'T : (static member ( * ) : 'T * 'T -> 'b) and 
             'T : (static member One : 'T) and 
             'T : (static member ( - ) : 'T * 'T -> 'T) and 
             'a : (static member Zero : 'a)
    
    /// <summary>Computes left null space of given matrix.</summary>
    /// <param name="mat">Matrix to compute left null space.</param>
    /// <returns>Returns left null space of given matrix as array of vectors.</returns>
    val inline LeftNullSpace : mat : 'T matrix -> 'T vector [] // TODO: inline conditions.

    /// <summary>Solves system of linear equations, Ax=b. Solution dose not include null-space solutions.</summary>
    /// <param name="mat">Matrix of coefficients, A.</param>
    /// <param name="rhs">Right-hand side of equation, b.</param>
    /// <returns>Particular solution to Ax=b as vector.</returns>
    val inline Solve : mat : 'T matrix -> rhs : 'T vector -> threshold : 'T -> 'T vector // TODO: inline conditions.