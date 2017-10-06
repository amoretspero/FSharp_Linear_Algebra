namespace FSharp_Linear_Algebra.Matrix.Computation

open FSharp_Linear_Algebra.Matrix
open FSharp_Linear_Algebra.Vector

module Matrix =

    /// <summary>Generates 2D stretch matrix, which will stretch 2D vector by given ratio.</summary>
    /// <param name="ratio">Ratio to stretch.</param>
    /// <returns>2D stretch matrix that stretches by given ratio.</returns>
    val inline Stretch2D : ratio : 'T -> 'T matrix
        when 'T : comparison and
             'T : (static member Zero : 'T) and
             'T : (static member One : 'T) 