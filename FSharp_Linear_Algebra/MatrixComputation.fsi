namespace FSharp_Linear_Algebra.Matrix.Computation

open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Vector

module Matrix =
    
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