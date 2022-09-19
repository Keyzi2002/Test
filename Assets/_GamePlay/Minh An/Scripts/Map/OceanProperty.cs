using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanProperty : MapProperty
{
    public List<GameObject> aboveWaterObjects;
    public List<Transform> trapPositions;
    public GameObject navalMine;

    void Update()
    {
        CheckPlayerUnderWater();
    }

    bool IsLowPosition()
    {
        if (playerTransform.position.y < -14f)
        {
            return true;
        }
        return false;
    }
    void CheckPlayerUnderWater()
    {
        if (!CameraManager.Instance.GetVcamController().IsStarted())
        {
            return;
        }
        if (IsLowPosition())
        {
            if (CameraManager.Instance.GetVcamController().GetModeTaget() == VcamController.ModeTaget.LootAt_Follow_Player_InGame)
            {
                CameraManager.Instance.GetVcamController().SetModeTaget(VcamController.ModeTaget.LookAt_Follow_Player_Underwater);
                ActiveAbovewaterObjects(false);
            }
        }
        else
        {
            if (CameraManager.Instance.GetVcamController().GetModeTaget() == VcamController.ModeTaget.LookAt_Follow_Player_Underwater)
            {
                CameraManager.Instance.GetVcamController().SetModeTaget(VcamController.ModeTaget.LootAt_Follow_Player_InGame);
                ActiveAbovewaterObjects(true);
            }
        }
    }
    void ActiveAbovewaterObjects (bool active)
    {
        for(int i = 0; i < aboveWaterObjects.Count; i++)
        {
            aboveWaterObjects[i].SetActive(active);
        }
    }

    void SpawnTrap()
    {
        for (int i = 0; i < trapPositions.Count; i++)
        {
            if(trapPositions[i].position.y > -5f)
            {
                continue;
            }

        }
    }
}
