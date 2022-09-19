using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VcamController : MonoBehaviour
{
    public Transform myTransform;
    [SerializeField] private ModeTaget modeTaget = ModeTaget.LootAt_Follow_Player_InStartLine;
    [SerializeField] private CinemachineVirtualCamera VCam;
    private CinemachineFramingTransposer transposer;
    private void Awake()
    {
        OnInIt();
    }
    private void OnInIt()
    {
        if (myTransform == null) { myTransform = this.transform; }
        if (VCam == null) { VCam = GetComponent<CinemachineVirtualCamera>(); }
        if (transposer == null) { transposer = VCam.GetCinemachineComponent<CinemachineFramingTransposer>(); }
    }
    private void Start()
    {
        EnventManager.AddListener(EventName.StartGame.ToString(), () => { modeTaget = ModeTaget.LootAt_Follow_Player_InGame; });
    }
    private void Update()
    {
        ActionTaget();
    }
    public void ActionTaget()
    {
        switch (modeTaget)
        {
            case ModeTaget.Ending_Win_Lose:
                Mode_Ending_Win_Lose();
                break;
            case ModeTaget.Ending_Player_Die:
                Mode_Ending_Player_Die();
                break;
            case ModeTaget.LootAt_Follow_Player_InGame:
                Mode_Follow_Player_InGame();
                break;
            case ModeTaget.LootAt_Follow_Player_InStartLine:
                Mode_LootAt_Follow_Player_InStartLine();
                break;
            case ModeTaget.LookAt_Follow_Player_Underwater:
                Mode_Follow_Player_Underwater();
                break;
        }
    }
    private void Mode_Ending_Win_Lose()
    {
        SetFollow_LookAt(RankingAnimManager.Instance.GetTransformRanking(), RankingAnimManager.Instance.GetTransformRanking());
        LerpDistance(15, 0.5f);
        SetYOffset(Vector3.up * 14);
        myTransform.localEulerAngles = Vector3.right * 15;
    }
    private void Mode_Ending_Player_Die()
    {
        SetFollow_LookAt();
    }
    private void Mode_LootAt_Follow_Player_InStartLine()
    {
        SetFollow_LookAt(GameManager.Instance.GetCharacter_Player().myTransform, GameManager.Instance.GetCharacter_Player().myTransform);
        LerpDistance(10, 0.5f);
        SetYOffset(Vector3.up);
        myTransform.localEulerAngles = Vector3.zero;
    }
    private void SetYOffset(Vector3 tracked)
    {
        transposer.m_TrackedObjectOffset = tracked;
    }
    private void Mode_Follow_Player_InGame()
    {
        SetFollow_LookAt(GameManager.Instance.GetCharacter_Player().myTransform, GameManager.Instance.GetCharacter_Player().myTransform);
        LerpDistance(30, 0.5f);
        myTransform.localEulerAngles = Vector3.right * 45;
    }
    private void Mode_Follow_Player_Underwater()
    {
        SetFollow_LookAt(GameManager.Instance.GetCharacter_Player().myTransform, GameManager.Instance.GetCharacter_Player().myTransform);
        LerpDistance(20, 0.5f);
        myTransform.localEulerAngles = Vector3.right * 40;
    }
    private void SetFollow_LookAt(Transform transformFollow = null, Transform transformLookAt = null)
    {
        VCam.Follow = transformFollow;
        VCam.LookAt = transformLookAt;
    }
    private void LerpDistance(float Distance, float speed)
    {
        transposer.m_CameraDistance = Mathf.Lerp(transposer.m_CameraDistance, Distance, speed);
    }
    private void SetDistance(float Distance)
    {
        transposer.m_CameraDistance = Distance;
    }
    public enum ModeTaget
    {
        Ending_Win_Lose,
        Ending_Player_Die,
        LootAt_Follow_Player_InGame,
        LootAt_Follow_Player_InStartLine,
        LookAt_Follow_Player_Underwater
    }
    public void SetModeTaget(ModeTaget modeTaget)
    {
        this.modeTaget = modeTaget;
    }
    public ModeTaget GetModeTaget()
    {
        return this.modeTaget;
    }

    public bool IsStarted()
    {
        return (modeTaget != ModeTaget.LootAt_Follow_Player_InStartLine);
    }
}
