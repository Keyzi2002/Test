using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NamePlayer : UI_Canvas
{
    public static List<NameRandom> nameRandoms_Trigger = new List<NameRandom>();
    public Transform myTranform;
    [SerializeField] private ModeLoad modeLoad;
    [SerializeField] private Text textName;
    [SerializeField] private GameObject objFull;
    private NameRandom nameRandomCurrent = NameRandom.None;
    private Transform cameraMainTransForm;
    private void Awake()
    {
        if (myTranform == null) { myTranform = this.transform; }
        if (cameraMainTransForm == null) { cameraMainTransForm = Camera.main.transform; }
    }
    //public override void OnInIt()
    //{
      
    //}
    //private void Start()
    //{
    //    OnInIt();
    //   // EnventManager.AddListener(EventName.LoadNamePlayer.ToString(), LoadName);
    //   // Close();
    //}
    private void Update()
    {
        myTranform.LookAt(myTranform.position + cameraMainTransForm.rotation * Vector3.forward /*+ Vector3.forward*/, cameraMainTransForm.rotation * Vector3.up);
    }
    public void LoadName(ModeLoad modeLoad)
    {
        switch (modeLoad)
        {
            case ModeLoad.Load_Random:
                if (nameRandomCurrent == NameRandom.None)
                {
                    var nameRandoms = System.Enum.GetValues(typeof(NameRandom));
                    List<NameRandom> nameRandomsTrigger = new List<NameRandom>();
                    for (int i = 0; i < nameRandoms.Length; i++)
                    {
                        nameRandomsTrigger.Add((NameRandom)nameRandoms.GetValue(i));
                    }
                    foreach (NameRandom nameRandom in nameRandoms_Trigger)
                    {
                        nameRandomsTrigger.Remove(nameRandom);
                    }
                    nameRandomsTrigger.Remove(NameRandom.None);
                    int indexName = Random.Range(0, nameRandomsTrigger.Count);
                    if(nameRandomsTrigger[indexName].ToString() == UIManager.Instance.GetUIPanel(UIType.StartGamePanel).GetComponent<PanelStartGame>().GetNamePlayer())
                    {
                        nameRandomsTrigger.RemoveAt(indexName);
                    }
                    nameRandomCurrent = nameRandomsTrigger[indexName];
                    nameRandoms_Trigger.Add(nameRandomCurrent);
                }
                SetTextName(nameRandomCurrent.ToString());
                break;
            case ModeLoad.Load_NamePlayerMain:
                //SetTextName(UIManager.Instance.GetUIPanel(UIType.StartGamePanel).GetComponent<PanelStartGame>().GetNamePlayer());
                SetTextName("You");
                break;
            default:
                Close();
                break;
        }
    }
    private void ResetName()
    {
        nameRandoms_Trigger.Clear();
    }
    private void SetTextName(string value)
    {
        textName.text = value;
    }
    //public override void Open()
    //{
    //    objFull.SetActive(true);
    //}
    //public override void Close()
    //{
    //    objFull.SetActive(false);
    //}
    public enum NameRandom
    {
        None,
        Ronando,
        Messi,
        Ronan,
        Keyzi,
        Tunzke,
        Vietnam,
        ThaiLan,
        Tom,
        Hiddleston,
        Acacia,
        Adela,
        Adelaide,
        Agatha,
        Fiona,
        Genevieve,
        Isolde,
        Kelsey,
        Laelia,
        Lani,
        Martha,
        Milcah,
        Mildred,
        Neala,
        Rowena,
        Sophronia,
        Phoebe,
        Randolph,
        Mervyn,
        Carwyn,
        Orborne,
        Patrick,
        Egan,
        Ambrose,
        Jesse,
        Samson,
        Matthew,
        Darling,
        Poppet,
        Tesoro
    }
    public enum ModeLoad
    {
        None,
        Load_Random,
        Load_NamePlayerMain
    }
}
