using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;

public class AwakeCircleClipper : MonoBehaviour, IClip
{
    public DestructibleTerrain terrain;

    public float diameterX = 1.2f;
    public float diameterZ = 1.2f;
    public float EulerRoate;
    private float radiusX = 1.2f, radiusZ = 1.2f;

    public int segmentCount = 10;

    private Vector2 clipPosition;

    public bool CheckBlockOverlapping(Vector2 p, float size)
    {       
        float dx = Mathf.Abs(clipPosition.x - p.x) - radiusX - size / 2;
        float dy = Mathf.Abs(clipPosition.y - p.y) - radiusZ - size / 2;

        return dx < 0f && dy < 0f;      
    }

    public ClipBounds GetBounds()
    {      
        return new ClipBounds
        {
            lowerPoint = new Vector2(clipPosition.x - radiusX, clipPosition.y - radiusX),
            upperPoint = new Vector2(clipPosition.x + radiusZ, clipPosition.y + radiusZ)
        };             
    }

    public List<Vector2i> GetVertices()
    {
        List<Vector2i> vertices = new List<Vector2i>();
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (-EulerRoate-45f - 360f / segmentCount * i);

            Vector2 point = new Vector2(clipPosition.x + radiusX * Mathf.Cos(angle), clipPosition.y + radiusZ * Mathf.Sin(angle));
            Vector2i point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }
        return vertices;
    }

    void Awake()
    {
        radiusX = diameterX / 2f;
        radiusZ = diameterZ / 2f;
    }

    void Start()
    {
        Vector2 positionWorldSpace = Vector2.zero; ;
        positionWorldSpace.x = transform.position.x;
        positionWorldSpace.y = transform.position.z;
        clipPosition = positionWorldSpace - terrain.GetPositionOffset().x * Vector2.right;

        terrain.ExecuteClip(this);
    }
}
