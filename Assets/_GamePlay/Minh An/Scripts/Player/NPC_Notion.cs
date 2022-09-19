using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Notion : MonoBehaviour
{
    public Transform myTransform;
    public string NameNoti_BagFull = "The bag is full\nand cannot\nhold more!";
    public string NameNoti_NotEnoughPoints = "Need 10 reels\nto continue\nbuilding!";
    [SerializeField] private Transform transformTaget;
    [SerializeField] private UI_NPC UI_NPC;
    private Vector3 pointTaget;
    private Vector3 pointReset;
    private bool isStartTaget;
    private void Awake()
    {
        if(myTransform == null) { myTransform = this.transform; }
    }
    //private void Start()
    //{
    //    PlayNoti(GameManager.Instance.GetCharacter_Player().myTransform, NameNoti_NotEnoughPoints, 5);
    //}
    public void PlayNoti(Transform transformTaget, string NoiDung = " ", float timeNoti = 0)
    {
        if (!isStartTaget)
        {
            isStartTaget = true;
            this.transformTaget = transformTaget;
            UI_NPC.SetNoti(NoiDung);
            ResetPointStart();
            StartCoroutine(IE_Singtelon.IE_DelayAction(() => { isStartTaget = false; }, timeNoti));
        }
    }
    private void ResetPointStart()
    {
        pointReset.x = transformTaget.position.x + 20;
        pointReset.y = transformTaget.position.y + 5;
        pointReset.z = transformTaget.position.z;
        myTransform.position = pointReset;
    }
    private void Update()
    {
        if (isStartTaget)
        {
            LookAt(transformTaget.position);
            pointTaget.z = transformTaget.position.z;
            pointTaget.x = transformTaget.position.x + 3;
            pointTaget.y = transformTaget.position.y + 2;
            myTransform.position = Vector3.Lerp(myTransform.position, pointTaget, 0.1f);
        }
        else
        {
            pointReset.x = transformTaget.position.x + 20;
            pointReset.y = transformTaget.position.y + 5;
            pointReset.z = transformTaget.position.z;
            myTransform.position = Vector3.Lerp(myTransform.position, pointReset, 0.1f);
            if(Vector3.Distance(myTransform.position, pointReset) <= 0.1f)
            {
                SetActive(false);
            }
        }
    }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    private void LookAt(Vector3 pointLookAt)
    {
        pointLookAt.y = myTransform.position.y;
        myTransform.LookAt(pointLookAt);
    }
}
