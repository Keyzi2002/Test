using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NPC : MonoBehaviour
{
    public Transform myTranform;
    private Transform cameraMainTransForm;
    [SerializeField] private UI_Notion uI_Notion;

    private void Awake()
    {
        if (myTranform == null) { myTranform = this.transform; }
        if (cameraMainTransForm == null) { cameraMainTransForm = Camera.main.transform; }

    }
    private void Update()
    {
        myTranform.LookAt(myTranform.position + cameraMainTransForm.rotation * Vector3.forward /*+ Vector3.forward*/, cameraMainTransForm.rotation * Vector3.up);
    }
    public void SetNoti(string str_noti)
    {
        uI_Notion.LoadTextNoti(str_noti);
    }
}
