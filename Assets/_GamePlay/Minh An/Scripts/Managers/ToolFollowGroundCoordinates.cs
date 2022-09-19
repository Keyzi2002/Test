using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolFollowGroundCoordinates : Singleton<ToolFollowGroundCoordinates>
{
    [SerializeField] Transform transformFollow;
    private Vector3 vt_Obj_In_GroundCoordinate;
    private RaycastHit m_Hit;
    public override void Awake()
    {
        if(transformFollow == null) { transformFollow = FindObjectOfType<Player>().myTransform; }
    }

    private void Update()
    {
        if (IsGround() && m_Hit.collider != null)
        {
            vt_Obj_In_GroundCoordinate = m_Hit.point;
            vt_Obj_In_GroundCoordinate.y = m_Hit.collider.transform.position.y + 5;
        }
    }
    public bool IsGround()
    {
#if UNITY_EDITOR
        Debug.DrawRay(transformFollow.position - transformFollow.forward * 1.2f, Vector3.down * 2, Color.red);
        Debug.DrawRay(transformFollow.position, Vector3.down * 2, Color.red);
#endif
        if (Physics.Raycast(transformFollow.position - transformFollow.forward * 1.2f, Vector3.down * 2, out m_Hit, 2) 
            || Physics.Raycast(transformFollow.position, Vector3.down * 2, 2))
        {
            return true;
        }
        return false;
    }
    public Vector3 GetPointInGround()
    {
        return vt_Obj_In_GroundCoordinate;
    }
}
