using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStreet : MonoBehaviour
{
    [SerializeField] private LayerMask layerMaskTaget;
    [SerializeField] private float ZOffset;
    [SerializeField] private float YOffset;
    [SerializeField] private Character player;
    private RaycastHit[] m_Hits;
    private Vector3 vt_RayStart, vt_RayDiraction = Vector3.forward;
 //   private Step_ stepController;
    private Distance distanceCurrent;
    private Color colorTaget;
    private List<Distance> distances_Move = new List<Distance>(); //tat ca cac distance da di qua
    //private void Start() 
    //{
    //    OnInIt();
    //}
    //private void OnInIt()
    //{
    //   // SetLayerMaskTaget(LayerMask.GetMask("Limit " + player.nameColorThis.ToString()));
    //}
    public void SetLayerMaskTaget(LayerMask layerMask)
    {
        layerMaskTaget = layerMask;
    }
    private void Update()
    {
        //vt_RayDiraction = player.myTransform.forward.normalized * ZOffset;
        //vt_RayStart = player.myTransform.position + Vector3.up;// + Vector3.up * player.myTransform.localScale.y/2 + player.myTransform.forward * ZOffset;
        //Debug.DrawRay(vt_RayStart, vt_RayDiraction, Color.red);
        vt_RayStart = player.myTransform.position + player.myTransform.forward.normalized * 0.5f;
        vt_RayDiraction = -player.myTransform.up.normalized * YOffset;
        Debug.DrawRay(vt_RayStart, vt_RayDiraction, Color.red);
        m_Hits = Physics.RaycastAll(vt_RayStart, vt_RayDiraction, YOffset, layerMaskTaget);
        if (m_Hits.Length > 0)
        {
            foreach(RaycastHit m_Hit in m_Hits)
            {
                if (m_Hit.collider != null)
                {
                    //  stepController = Cache.GetComponetStepInParent(m_Hit.collider);
                    distanceCurrent = Cache.GetComponetDistanceInParent(m_Hit.collider);
                    if (distanceCurrent.GetNameColor() != player.nameColorThis && player.GetBagTerrainRoll().GetScoreTerrainRollInBag() >= 10)
                    {
                        switch (player.nameColorThis)
                        {
                            case NameColor.Pink:
                                colorTaget = ColorSettings.Instance.color_Pink;
                                break;
                            case NameColor.Green:
                                colorTaget = ColorSettings.Instance.color_Green;
                                break;
                            case NameColor.Blue:
                                colorTaget = ColorSettings.Instance.color_Blue;
                                break;
                            case NameColor.Yeallow:
                                colorTaget = ColorSettings.Instance.color_Yeallow;
                                break;
                            case NameColor.Violet:
                                colorTaget = ColorSettings.Instance.color_Violet;
                                break;
                            case NameColor.Orange:
                                colorTaget = ColorSettings.Instance.color_Orange;
                                break;
                        }

                        if (distanceCurrent.GetNameColor() == NameColor.None)
                        {
                            distanceCurrent.GetColorLerp().SetColor(colorTaget);
                        }
                        else
                        {
                            distanceCurrent.GetColorLerp().SetColorStartAndEnd(distanceCurrent.GetColorLerp().GetColorStart(), colorTaget);
                            distanceCurrent.GetColorLerp().StartLerp(0);
                        }
                        distanceCurrent.SetNameColor(player.nameColorThis);
                        player.GetBagTerrainRoll().SetScoreTerrainRollInBag_UP(-distanceCurrent.GetScoreBuild());
                        distances_Move.Add(distanceCurrent);
                    }
                    else if (!distances_Move.Contains(distanceCurrent) && distanceCurrent.GetNameColor() != player.nameColorThis)
                    {
                        player.SetSpeed(0);
                        GameManager.Instance.PlayNoti(player, GameManager.TypeCall_NPCNoti.NotEnoughPoints, 3);
                    }
                    else
                    {
                        player.SetSpeed(player.SpeedMax);
                    }

                }
            }
          
        }
        else
        {
            player.SetSpeed(player.SpeedMax);
            distances_Move.Clear();
        }
      
    }
}
