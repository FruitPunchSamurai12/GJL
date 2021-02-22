using UnityEngine;

public static class Vector3Extensions
{
    public static float FlatDistance(this Vector3 from, Vector3 to)
    {
        return Vector3.Distance(new Vector3(from.x, 0, from.z), new Vector3(to.x, 0, to.z));
    }
}