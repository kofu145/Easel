using System;

namespace Easel.Math;

/// <summary>
/// Provides helpful math functions useful in game development, such as linear interpolation and clamping.
/// </summary>
public static class EaselMath
{
    /// <summary>
    /// Convert degrees to radians.
    /// </summary>
    /// <param name="degrees">The value in degrees.</param>
    /// <returns>The converted value.</returns>
    public static float ToRadians(float degrees) => degrees * (MathF.PI / 180);

    /// <summary>
    /// Convert radians to degrees.
    /// </summary>
    /// <param name="radians">The value in radians.</param>
    /// <returns>The converted value.</returns>
    public static float ToDegrees(float radians) => radians * (180 / MathF.PI);

    /// <summary>
    /// <b>L</b>inearly int<b>erp</b>olate between two <see langword="float"/> values, from the given <b>normalized</b>
    /// multiplier value (aka 0 = min, 1 = max)
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="multiplier">The normalized multiplier</param>
    /// <returns>The interpolated value.</returns>
    /// <remarks>The <paramref name="multiplier"/> value can be outside of the 0-1 range, you will just get numbers larger
    /// or smaller than the max/min values respectively.</remarks>
    public static float Lerp(float min, float max, float multiplier) => multiplier * (max - min) + min;
    
    /// <summary>
    /// Clamp the given value between the min and max values.
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped value.</returns>
    public static float Clamp(float value, float min, float max) => value <= min ? min : value >= max ? max : value;
}