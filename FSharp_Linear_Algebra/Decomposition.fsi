namespace FSharp_Linear_Algebra.Matrix.Decomposition

open FSharp_Linear_Algebra.Matrix

/// <summary>Exception that indicates gauss elimination of given matrix is not possible.</summary>
exception NoLDUDecompositionPossible

/// <summary>Class for representing LDU-decomposition result.</summary>
[<Class>]
type LDUResult<'T> =
    
    /// <summary>Default constructor.
    /// P is permutation matrix, L is lower triangular matrix, D is diagonal matrix and U is upper triangular matrix.</summary>
    new :  P : 'T matrix * L : 'T matrix * D : 'T matrix * U : 'T matrix -> 'T LDUResult
    
    /// <summary>Gets permutation matrix of LDU-decomposition.</summary>
    member Permutation : 'T matrix with get

    /// <summary>Gets lower matrix of LDU-decomposition.</summary>
    member Lower : 'T matrix with get

    /// <summary>Gets diagonal matrix of LDU-decomposition.</summary>
    member Diagonal : 'T matrix with get

    /// <summary>Gets upper matrix of LDU-decomposition.</summary>
    member Upper : 'T matrix with get

/// <summary>Module containing various decompositions.</summary>
module Decomposition =

    /// <summary>Gauss eliminates given decimal matrix.</summary>
    /// <param name="mat">Matrix to be eliminated.</param>
    /// <returns>Returns eliminated result.</returns>
    /// <exception cref="FSharp_Linear_Algebra.Matrix.NoLDUDecompositionPossible">Thrown when gauss elimination cannot be performed.</exception>
    val inline LDUdecomposition : mat:matrix<'T> -> LDUResult<'T>
        when 'T : comparison and 
             'T : (static member Zero : 'T) and 
             ('a or 'T) : (static member ( - ) : 'a * 'T -> 'T) and 
             ('T or 'b) : (static member ( - ) : 'T * 'b -> 'T) and 
             'T : (static member ( * ) : 'T * 'T -> 'b) and 
             'T : (static member ( / ) : 'T * 'T -> 'T) and 
             'T : (static member One : 'T) and 
             'a : (static member Zero : 'a)
             