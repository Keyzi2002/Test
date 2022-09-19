using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CurrentPlayer : UI_Canvas
{
    public Transform myTranform;
    [SerializeField] private GameObject objFull;
    private Transform cameraMainTransForm;

    private void Awake()
    {
        if (myTranform == null) { myTranform = this.transform; }
        if (cameraMainTransForm == null) { cameraMainTransForm = Camera.main.transform; }
    }
    private void Update()
    {
        myTranform.LookAt(myTranform.position + cameraMainTransForm.rotation * Vector3.forward /*+ Vector3.forward*/, cameraMainTransForm.rotation * Vector3.up);
    }
    //public override void Open()
    //{
    //    objFull.SetActive(true);
    //}
    //public override void Close()
    //{
    //    objFull.SetActive(false);
    //}
}
