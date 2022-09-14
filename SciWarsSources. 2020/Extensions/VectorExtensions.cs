using UnityEngine;

public static class VectorExtensions{
    public static Vector3 ToVector3(this float[] array)
    {
        return new Vector3(array[0],array[1],array[2]);
    }

    public static Quaternion ToQuaternion(this float[] array) {
        return new Quaternion(array[0], array[1], array[2],array[3]);
    }

    public static float[] ToArray(this Vector3 v)
    {
        return new[] {v.x, v.y, v.z};
    }

    public static float[] ToArray(this Quaternion q)
    {
        return new[] {q.x, q.y, q.z, q.w};
    }
}
