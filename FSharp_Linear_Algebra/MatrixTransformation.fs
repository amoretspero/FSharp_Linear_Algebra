namespace FSharp_Linear_Algebra.Matrix.Transformation

open FSharp_Linear_Algebra.Vector
open FSharp_Linear_Algebra.Matrix

module Matrix = 

    /// <summary>Generates 2D stretch matrix, which will stretch 2D vector by given ratio.</summary>
    /// <param name="ratio">Ratio to stretch.</param>
    /// <returns>2D stretch matrix that stretches by given ratio.</returns>
    let inline Stretch2D (ratio : 'T) =
        let mat = Matrix.Identity 2 LanguagePrimitives.GenericOne
        mat.element.[0, 0] <- ratio 
        mat.element.[1, 1] <- ratio
        mat