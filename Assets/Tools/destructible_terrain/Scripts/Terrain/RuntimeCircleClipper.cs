using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;
using Vector2f = UnityEngine.Vector2;

public class RuntimeCircleClipper : MonoBehaviour, IClip
{
    [SerializeField] private Transform Shovel;
    public void ChangeShovel(Transform newShovelTransform) { Shovel = newShovelTransform; }
    private struct TouchLineOverlapCheck 
    {
        public float a;
        public float b;
        public float c;
        public float angle;
        public float dividend;
        private Vector2 d;
        private float m;
        private float da;
        public TouchLineOverlapCheck(Vector2 p1, Vector2 p2)
        {
            d = p2 - p1;
            m = d.magnitude;
            a = -d.y / m;
            b = d.x / m;
            c = -(a * p1.x + b * p1.y);
            angle = Mathf.Rad2Deg * Mathf.Atan2(-a, b);

            
            if (d.x / d.y < 0f)
                da = 45 + angle;
            else
                da = 45 - angle;

            dividend = Mathf.Abs(1.0f / 1.4f * Mathf.Cos(Mathf.Deg2Rad * da));
        }

        public float GetDistance(Vector2 p)
        {
            return Mathf.Abs(a * p.x + b * p.y + c);
        }
    }

    public DestructibleTerrain terrain;

    public float diameter = 1.2f;

    private float radius = 1.2f;

    public int segmentCount = 10;

    public float touchMoveDistance = 0.1f;

    private Vector2f currentTouchPoint;

    private Vector2f previousTouchPoint;

    private TouchPhase touchPhase;

    private TouchLineOverlapCheck touchLine;

    private List<Vector2i> vertices = new List<Vector2i>();

    //  private Camera mainCamera;

    //  private float cameraZPos;

    private float dx;
    private float dy;
    private float distance;
    public bool CheckBlockOverlapping(Vector2f p, float size)
    {
        if (touchPhase == TouchPhase.Began)
        {
            dx = Mathf.Abs(currentTouchPoint.x - p.x) - radius - size / 2;
            dy = Mathf.Abs(currentTouchPoint.y - p.y) - radius - size / 2;
            return dx < 0f && dy < 0f;
        }
        else if (touchPhase == TouchPhase.Moved)
        {          
            distance = touchLine.GetDistance(p) - radius - size / touchLine.dividend;
            return distance < 0f;
        }
        else
        {
            return false;
        }
           
    }
    private Vector2f upperPoint;
    private Vector2f lowerPoint;
    private ClipBounds clipBounds = new ClipBounds();
    public ClipBounds GetBounds()
    {
        if (touchPhase == TouchPhase.Began)
        {
            clipBounds.lowerPoint.x = currentTouchPoint.x - radius;
            clipBounds.lowerPoint.y = currentTouchPoint.y - radius;

            clipBounds.upperPoint.x = currentTouchPoint.x + radius;
            clipBounds.upperPoint.y = currentTouchPoint.y + radius;
        }
        else if (touchPhase == TouchPhase.Moved)
        {
            upperPoint = currentTouchPoint;
            lowerPoint = previousTouchPoint;
            if (previousTouchPoint.x > currentTouchPoint.x)
            {
                upperPoint.x = previousTouchPoint.x;
                lowerPoint.x = currentTouchPoint.x;
            }
            if (previousTouchPoint.y > currentTouchPoint.y)
            {
                upperPoint.y = previousTouchPoint.y;
                lowerPoint.y = currentTouchPoint.y;
            }

            clipBounds.lowerPoint.x = lowerPoint.x - radius;
            clipBounds.lowerPoint.y = lowerPoint.y - radius;

            clipBounds.upperPoint.x = upperPoint.x - radius;
            clipBounds.upperPoint.y = upperPoint.y - radius;
        }
        else
        {
            clipBounds.lowerPoint = Vector2f.zero;
            clipBounds.upperPoint = Vector2f.zero;
        }
        return clipBounds;
    }

    public List<Vector2i> GetVertices()
    {
        return vertices;
    }

    void Awake()
    {
        //mainCamera = Camera.main;
        //cameraZPos = mainCamera.transform.position.z;
        radius = diameter / 2f;
    }
    void Update()
    {
        //UpdateTouch();
       
        UpdateShovel();
    }
    private Vector2 ShovelPosition;
    private Vector2 positionTerrain;
    private void UpdateShovel()
    {
        if (terrain == null)
        {
            return;
        }
        ShovelPosition  = Vector2.zero;
        if (Shovel.eulerAngles.y > 100)
        {
            ShovelPosition.x = Shovel.position.x + 0.3f;
        }
        else
        {
            ShovelPosition.x = Shovel.position.x + 0.8f;
        }
      
        ShovelPosition.y = Shovel.position.z - terrain.myTransform.position.z;
        positionTerrain = Vector2.zero;
        positionTerrain.x = terrain.GetPositionOffset().x;
        positionTerrain.y = terrain.GetPositionOffset().y;
        currentTouchPoint = ShovelPosition - positionTerrain;

        if ((currentTouchPoint - previousTouchPoint).sqrMagnitude <= touchMoveDistance * touchMoveDistance)
        {
            return;
        }
      
        BuildVertices(previousTouchPoint, currentTouchPoint);
        terrain.ExecuteClip(this);

        previousTouchPoint = currentTouchPoint;

    }
    public DestructibleTerrain GetDestructibleTerrain()
    {
        return terrain;
    }
    //void UpdateTouch()
    //{
    //    if (TouchUtility.TouchCount > 0)
    //    {
    //        Touch touch = TouchUtility.GetTouch(0);
    //        Vector2 touchPosition = touch.position;

    //        touchPhase = touch.phase;
    //        if (touch.phase == TouchPhase.Began)
    //        {
    //            Vector2 XOYPlaneLocation = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, -cameraZPos));
    //            currentTouchPoint = XOYPlaneLocation - terrain.GetPositionOffset();

    //            BuildVertices(currentTouchPoint);

    //            terrain.ExecuteClip(this);               

    //            previousTouchPoint = currentTouchPoint;            
    //        }
    //        else if (touch.phase == TouchPhase.Moved)
    //        {
    //            Vector2 XOYPlaneLocation = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, -cameraZPos));
    //            currentTouchPoint = XOYPlaneLocation - terrain.GetPositionOffset();

    //            if ((currentTouchPoint - previousTouchPoint).sqrMagnitude <= touchMoveDistance * touchMoveDistance)
    //                return;

    //            BuildVertices(previousTouchPoint, currentTouchPoint);

    //            terrain.ExecuteClip(this);               

    //            previousTouchPoint = currentTouchPoint;
    //        }
    //    }
    //}

    //void BuildVertices(Vector2 center)
    //{
    //    vertices.Clear();
    //    for (int i = 0; i < segmentCount; i++)
    //    {
    //        float angle = Mathf.Deg2Rad * (-90f - 360f / segmentCount * i);

    //        Vector2 point = new Vector2(center.x + radius * Mathf.Cos(angle), center.y + radius * Mathf.Sin(angle));
    //        Vector2i point_i64 = point.ToVector2i();
    //        vertices.Add(point_i64);
    //    }
    //}
    private Vector2 point;
    private Vector2i point_i64;
    private float angle;
    private int halfSegmentCount;
    void BuildVertices(Vector2 begin, Vector2 end)
    {
        vertices.Clear();
        halfSegmentCount = segmentCount / 2;
        touchLine = new TouchLineOverlapCheck(begin, end);
        
        for (int i = 0; i <= halfSegmentCount; i++)
        {
            angle = Mathf.Deg2Rad * (touchLine.angle + 270f - (float)360f / segmentCount * i);
            point.x = begin.x + radius * Mathf.Cos(angle);
            point.y = begin.y + radius * Mathf.Sin(angle);
            point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }

        for (int i = halfSegmentCount; i <= segmentCount; i++)
        {
            angle = Mathf.Deg2Rad * (touchLine.angle + 270f - (float)360f / segmentCount * i);
            point.x = end.x + radius * Mathf.Cos(angle);
            point.y = end.y + radius * Mathf.Sin(angle);
            point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }
    }
    public void SetTerrain(DestructibleTerrain newTerrain)
    {
        terrain = newTerrain;
    }
    public void IsActive(bool isActive)
    {
        this.enabled = isActive;
    }
}
