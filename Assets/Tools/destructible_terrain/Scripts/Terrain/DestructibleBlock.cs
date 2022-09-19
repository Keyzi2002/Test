using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using int64 = System.Int64;
using Vector2i = ClipperLib.IntPoint;
using Vector2f = UnityEngine.Vector2;

public class DestructibleBlock : MonoBehaviour
{
    private List<List<Vector2i>> polygons;

    //private List<List<Vector2f>> edgesList;

   // private List<EdgeCollider2D> colliders = new List<EdgeCollider2D>();

    public List<List<Vector2i>> Polygons { get { return polygons; } }

    private Mesh mesh;

    private MeshRenderer meshRenderer;
    private void Awake()
    {
        mesh = new Mesh();
        mesh.MarkDynamic();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    //public void UpdateGemetry(List<List<Vector2i>> inPolygons, float depth)
    //{      
    //    if (polygons != null)
    //        polygons.Clear();
    //    else
    //        polygons = new List<List<Vector2i>>();

    //    List<List<Vector2>> edgesList = new List<List<Vector2f>>();

    //    int totalVertexCount = 0;
    //    int edgeTriangleIndexCount = 0;

    //    for (int i = 0; i < inPolygons.Count; i++)
    //    {
    //        Vector2i[] simplifiedPolygon = BlockSimplification.Execute(inPolygons[i], edgesList);
    //        if (simplifiedPolygon != null)
    //        {
    //            polygons.Add(new List<Vector2i>(simplifiedPolygon));

    //            totalVertexCount += simplifiedPolygon.Length;
    //        }
    //    }

    //    for (int i = 0; i < edgesList.Count; i++)
    //    {
    //        int vertexCount = edgesList[i].Count;
    //        totalVertexCount += vertexCount * 2;
    //        edgeTriangleIndexCount += (vertexCount - 1) * 6;
    //    }

    //    Vector3[] vertices = new Vector3[totalVertexCount];
    //    List<int> triangles = new List<int>();
    //    int[] edgeTriangles = new int[edgeTriangleIndexCount];

    //    int vertexIndex = 0;
    //    int vertexOffset = 0;

    //    for (int i = 0; i < polygons.Count; i++)
    //    {
    //        List<Vector2i> polygon = polygons[i];
    //        int vertexCount = polygon.Count;

    //        for (int j = vertexCount - 1; j >= 0; j--)
    //        {
    //            vertices[vertexIndex] = polygon[j].ToVector3f();
    //            vertexIndex++;
    //        }

    //        Triangulate.Execute(vertices, vertexOffset, vertexOffset + vertexCount, triangles);
    //        vertexOffset += vertexCount;
    //    }

    //    int edgeTriangleIndex = 0;
    //    for (int i = 0; i < edgesList.Count; i++)
    //    {
    //        List<Vector2f> edgePoints = edgesList[i];
    //        int vertexCount = edgePoints.Count;
    //        Vector3 point;
    //        for (int j = 0; j < vertexCount - 1; j++)
    //        {
    //            point = edgePoints[j].ToVector3f();
    //            vertices[vertexIndex] = point;
    //            point.z += depth;
    //            vertices[vertexIndex + 1] = point;

    //            edgeTriangles[edgeTriangleIndex + 0] = vertexIndex;
    //            edgeTriangles[edgeTriangleIndex + 1] = vertexIndex + 2;
    //            edgeTriangles[edgeTriangleIndex + 2] = vertexIndex + 1;

    //            edgeTriangles[edgeTriangleIndex + 3] = vertexIndex + 2;
    //            edgeTriangles[edgeTriangleIndex + 4] = vertexIndex + 3;
    //            edgeTriangles[edgeTriangleIndex + 5] = vertexIndex + 1;

    //            vertexIndex += 2;
    //            edgeTriangleIndex += 6;
    //        }

    //        point = edgePoints[vertexCount - 1].ToVector3f();
    //        vertices[vertexIndex] = point;
    //        point.z += depth;
    //        vertices[vertexIndex + 1] = point;
    //        vertexIndex += 2;
    //    }

    //    triangles.AddRange(edgeTriangles);

    //    mesh.Clear();
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles.ToArray();
    //    mesh.RecalculateNormals();
    //    //mesh.MarkModified();

    //    UpdateColliders(edgesList);  
    //}
    private List<List<Vector2>> edgesList = new List<List<Vector2f>>();
    private Vector2i[] simplifiedPolygon;
    private List<Vector2f> edgePoints = new List<Vector2f>();
    private List<Vector2i> polygon = new List<Vector2i>();
    private List<int> triangles = new List<int>();
    private Vector3 point1;
    private Vector3 point2;
    private Vector3 point;
    private int totalVertexCount = 0;
    private int edgeTriangleIndexCount = 0;
    private int vertexCount;
    private int vertexIndex = 0;
    private int vertexOffset = 0;

    public void UpdateGeometryWithMoreVertices(List<List<Vector2i>> inPolygons, float width, float height, float depth)
    {
        if (polygons != null)
            polygons.Clear();
        else
            polygons = new List<List<Vector2i>>();

        edgesList.Clear();

         totalVertexCount = 0;
         edgeTriangleIndexCount = 0;

        for (int i = 0; i < inPolygons.Count; i++)
        {
            simplifiedPolygon = BlockSimplification.Execute(inPolygons[i], edgesList);
            if (simplifiedPolygon != null)
            {
                polygons.Add(new List<Vector2i>(simplifiedPolygon));

                totalVertexCount += simplifiedPolygon.Length;
            }
        }

        for (int i = 0; i < edgesList.Count; i++)
        {
            vertexCount = edgesList[i].Count;
            totalVertexCount += (vertexCount -1) * 4;
            edgeTriangleIndexCount += (vertexCount - 1) * 6;
        }
        Vector3[] vertices = new Vector3[totalVertexCount];
        Vector3[] normals = new Vector3[totalVertexCount];
        Vector2f[] texCoords = new Vector2f[totalVertexCount];

        triangles.Clear();
        int[] edgeTriangles = new int[edgeTriangleIndexCount];

         vertexIndex = 0;
         vertexOffset = 0;

        for (int i = 0; i < polygons.Count; i++)
        {
            polygon = polygons[i];
            vertexCount = polygon.Count;

            for (int j = vertexCount - 1; j >= 0; j--)
            {
                point = polygon[j].ToVector3f();
                vertices[vertexIndex] = point;
                normals[vertexIndex] = -Vector3.forward;
                texCoords[vertexIndex] = new Vector2f(point.x / width, point.y / height);
                vertexIndex++;
            }

            Triangulate.Execute(vertices, vertexOffset, vertexOffset + vertexCount, triangles);
            vertexOffset += vertexCount;
        }

        int edgeTriangleIndex = 0;
       // int vertexOnEdgeIndex = vertexIndex;
        for (int i = 0; i < edgesList.Count; i++)
        {
            edgePoints = edgesList[i];
            vertexCount = edgePoints.Count;
            for (int j = 0; j < vertexCount - 1; j++)
            {
                point1 = edgePoints[j].ToVector3f();
                point2 = edgePoints[j + 1].ToVector3f();
                vertices[vertexIndex + 0] = point1;
                vertices[vertexIndex + 2] = point2;

                point1.z += depth;
                point2.z += depth;

                vertices[vertexIndex + 1] = point1;
                vertices[vertexIndex + 3] = point2;

                Vector3 normal = (point2 - point1).normalized;
                normal = new Vector3(normal.y, -normal.x);
                normals[vertexIndex + 0] = normal;
                normals[vertexIndex + 2] = normal;
                normals[vertexIndex + 1] = normal;
                normals[vertexIndex + 3] = normal;


                edgeTriangles[edgeTriangleIndex + 0] = vertexIndex;
                edgeTriangles[edgeTriangleIndex + 1] = vertexIndex + 2;
                edgeTriangles[edgeTriangleIndex + 2] = vertexIndex + 1;

                edgeTriangles[edgeTriangleIndex + 3] = vertexIndex + 2;
                edgeTriangles[edgeTriangleIndex + 4] = vertexIndex + 3;
                edgeTriangles[edgeTriangleIndex + 5] = vertexIndex + 1;

                vertexIndex += 4;
                edgeTriangleIndex += 6;
                
            }          
        }

        triangles.AddRange(edgeTriangles);
        mesh.Clear();
        mesh.vertices = vertices;
       
        //mesh.normals = normals;
        mesh.uv = texCoords;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        //if(mesh.bounds.size.x < 0.1f && mesh.bounds.size.y < 0.1f)
        //{
        //    Destroy(gameObject);
        //}
        //mesh.MarkModified();

            //   UpdateColliders(edgesList);
    }

    //private void UpdateColliders(List<List<Vector2>> edgesList)
    //{
    //    int colliderCount = colliders.Count;
    //    int edgesCount = edgesList.Count;

      
    //    //if (colliderCount < edgesCount)
    //    //{
           
    //    //    for (int i = edgesCount - colliderCount; i > 0; i--)
    //    //    {
    //    //        colliders.Add(gameObject.AddComponent<EdgeCollider2D>());
               
    //    //    }
       
    //    //}
    //    //else if (edgesCount < colliderCount)
    //    //{
    //    //    for (int i = colliderCount - 1; i >= edgesCount; i--)
    //    //    {
    //    //        Destroy(colliders[i]);
    //    //        colliders.RemoveAt(i);
               
    //    //    }
        
    //    //}
    //    for (int i = 0; i < colliders.Count; i++)
    //    {
    //        colliders[i].points = edgesList[i].ToArray();
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    //Vector2 origin = transform.parent.position;

    //    //for (int i = 0; i < edgesList.Count; i++)
    //    //{
    //    //    for (int j = 1; j < edgesList[i].Count; j++)
    //    //    {
    //    //        Debug.DrawLine(edgesList[i][j] + origin, edgesList[i][j - 1] + origin, Color.red);
    //    //    }
    //    //}
    //}
}
