using UnityEngine;

/// <summary>
/// Vector3 helpers.
/// </summary>
public static class Vector3Extensions
{
    /// <param name="decimals">How many digits you want to keep after the dot.</param>
    public static Vector3 Round(this Vector3 v, int decimals = 2)
    {
        float m = Mathf.Pow(10f, decimals);
        return new Vector3(
            Mathf.Round(v.x * m) / m,
            Mathf.Round(v.y * m) / m,
            Mathf.Round(v.z * m) / m
        );
    }
}
