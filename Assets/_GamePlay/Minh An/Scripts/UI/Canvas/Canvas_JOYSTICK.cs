using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_JOYSTICK : UI_Canvas
{
    private static Canvas_JOYSTICK share_CANVASJOYSTICK;
    public static Canvas_JOYSTICK Share_CANVASJOYSTICK
    {
        get
        {
            if(share_CANVASJOYSTICK == null)
            {
                share_CANVASJOYSTICK = FindObjectOfType<Canvas_JOYSTICK>();
            }
            return share_CANVASJOYSTICK;
        }
    }
    private static float LIMIT_DISTANCE_TOUTCH = 120;
    [SerializeField] private Transform Trans_Touch;
    [SerializeField] private Transform Trans_BG;
    [SerializeField] private GameObject JoyStick;

    private Vector3 Diraction = Vector3.zero;
    private Vector3 Position_Mouse;
    private void Awake()
    {
        GameManager.Instance.SetActionStartGame(Stop);
    }
    public override void OnInIt()
    {
        base.OnInIt();
    }
    protected void Start()
    {
        EnventManager.AddListener(EventName.WatchRevivalDone.ToString(), Open);
    }
    private void Update()
    {
        if (IsStop())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Position_Mouse = Input.mousePosition;
            Trans_BG.position = Position_Mouse;
            Trans_Touch.localPosition = Vector3.zero;
            JoyStick.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
            Position_Mouse = Input.mousePosition;
            Trans_Touch.transform.position = Position_Mouse;

            Diraction = Trans_Touch.transform.localPosition.normalized;

            if (Vector3.Distance(Vector3.zero, Trans_Touch.transform.localPosition) >= LIMIT_DISTANCE_TOUTCH)
            {
                Diraction.Normalize();
                Trans_Touch.transform.localPosition = Diraction * LIMIT_DISTANCE_TOUTCH;
            }
            else
            {
                Trans_Touch.transform.position = Position_Mouse;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Trans_Touch.transform.localPosition = Vector3.zero;
            JoyStick.SetActive(false);
        }
    }
    public Vector3 Get_Diraction()
    {
        Vector3 Diraction_ = new Vector3(Diraction.x, 0, Diraction.y);
        return Diraction_.normalized;
    }
    public bool isOpenJoystick()
    {
        return JoyStick.activeSelf;
    }
}
