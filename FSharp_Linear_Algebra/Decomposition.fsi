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

/// <summary>Class for representing RREF(Row-Reduced Echelon Form)-decomposition result.</summary>
[<Class>]
type RREFResult<'T> =
    
    /// <summary>Default constructor.
    /// P is permutation matrix, L is lower triangular matrix, D is diagonal matrix, U is upper triangular matrix and
    /// R is RREF matrix.</summary>
    new : P : 'T matrix * L : 'T matrix * D : 'T matrix * U : 'T matrix * R : 'T matrix -> 'T RREFResult

    /// <summary>Gets permutation matrix of RREF-decomposition.</summary>
    member Permutation : 'T matrix with get

    /// <summary>Gets lower triangular matrix of RREF-decomposition.</summary>
    member Lower : 'T matrix with get

    /// <summary>Gets diagonal matrix of RREF-decomposition.</summary>
    member Diagonal : 'T matrix with get

    /// <summary>Gets upper triangular matrix of RREF-decomposition.</summary>
    member Upper : 'T matrix with get

    /// <summary>Gets Row-Reduced Echelon Form matrix of RREF-decomposition.</summary>
    member RREF : 'T matrix with get

/// <summary>Module containing various decompositions.</summary>
module Decomposition =

    /// <summary>LDU-decompose given matrix.</summary>
    /// <param name="mat">Matrix to be decomposed.</param>
    /// <returns>Returns decomposed result.</returns>
    /// <exception cref="FSharp_Linear_Algebra.Matrix.Decomposition.NoLDUDecompositionPossible">Thrown when LDU decomposition cannot be performed.</exception>
    val inline LDUdecomposition : mat:matrix<'T> -> LDUResult<'T>
        when 'T : comparison and 
             'T : (static member Zero : 'T) and 
             ('a or 'T) : (static member ( - ) : 'a * 'T -> 'T) and 
             ('T or 'b) : (static member ( - ) : 'T * 'b -> 'T) and 
             'T : (static member ( * ) : 'T * 'T -> 'b) and 
             'T : (static member ( / ) : 'T * 'T -> 'T) and 
             'T : (static member One : 'T) and 
             'a : (static member Zero : 'a)

    /// <summary>Gets Row-reduced Echelon Form of given matrix and decompose.</summary>
    /// <param name="mat">Matrix to be eliminated and decomposed.</param>
    /// <returns>Returns decomposition result.</returns>
    val inline RREFdecomposition : mat:matrix<'T> -> RREFResult<'T>
        when 'T : comparison and 
             'T : (static member Zero : 'T) and 
             ('a or 'T) : (static member ( - ) : 'a * 'T -> 'T) and 
             'T : (static member ( / ) : 'T * 'T -> 'T) and 
             ('T or 'b) : (static member ( - ) : 'T * 'b -> 'T) and 
             'T : (static member ( * ) : 'T * 'T -> 'b) and 
             'T : (static member One : 'T) and 
             'a : (static member Zero : 'a)