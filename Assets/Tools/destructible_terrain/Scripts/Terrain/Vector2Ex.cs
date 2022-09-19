using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using int64 = System.Int64;
using uint32 = System.UInt32;
using Vector2i = ClipperLib.IntPoint;
using Vector2f = UnityEngine.Vector2;

public static class VectorEx
{
    public static float float2int64 = 100000.0f;

    private static Vector2i vector2I = new Vector2i();
    public static Vector2i ToVector2i(this Vector2f p)
    {
        vector2I.x = (int64)(p.x * float2int64);
        vector2I.y = (int64)(p.y * float2int64);
        return vector2I;
    }
    private static Vector3 vector3f = new Vector3();
    public static Vector3 ToVector3f(this Vector2f p)
    {
        vector3f.x = p.x;
        vector3f.y = p.y;
        vector3f.z = 0f;
        return vector3f;
    }
    private static Vector2f vector2F = new Vector2f();
    public static Vector2f ToVector2f(this Vector2i p)
    {
        vector2F.x = (float)(p.x / float2int64);
        vector2F.y = (float)(p.y / float2int64);
        return vector2F;
    }

    public static Vector3 ToVector3f(this Vector2i p)
    {
        vector3f.x = (float)(p.x / float2int64);
        vector3f.y = (float)(p.y / float2int64);
        vector3f.z = 0f;
        return vector3f;
    }
    private static float m;
    public static float Cross(Vector2f A, Vector2f B)
    {
        m = A.x * B.y - A.y * B.x;
        return m;
    }

    public static Vector2f Cross(Vector2 A, float s)
    {
        vector2F.x = -A.y * s;
        vector2F.y = A.x * s;
        return vector2F;
    }

    public struct Line // ax + by + c = 0
    {
        public float a;
        public float b;
        public float c;
        public float angle;

        public Line(Vector2i p1, Vector2i p2)
        {
            vector2I.x = p2.x - p1.x;
            vector2I.y = p2.y - p1.y;
            m = Mathf.Sqrt(vector2I.x * vector2I.x + vector2I.y * vector2I.y);
            a = -vector2I.y / m;
            b = vector2I.x / m;
            c = -(a * p1.x + b * p1.y);
            angle = 0f;
        }

        public Line(Vector2 p1, Vector2 p2)
        {
            Vector2 d = p2 - p1;
            m = d.magnitude;
            a = -d.y / m;
            b = d.x / m;
            c = -(a * p1.x + b * p1.y);
            angle = Mathf.Rad2Deg * Mathf.Atan2(-a, b);
        }

        public float GetDistance(Vector2i p)
        {
            return Mathf.Abs(a * p.x + b * p.y + c);
        }

        public float GetDistance(Vector2 p)
        {
            return Mathf.Abs(a * p.x + b * p.y + c);
        }
    }
}
