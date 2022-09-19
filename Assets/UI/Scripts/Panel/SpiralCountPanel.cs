using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralCountPanel : UIPanel
{
    public Vector3 offSet;
    public GameObject slot;
    public List<SpiralCountSlot> slotsCreated;
    public SpiralCountSlot currentSlot;
    public override void Awake()
    {
        uiType = UIType.SpiralCountPanel;
        base.Awake();
    }
    private void Start()
    {
        EnventManager.AddListener(EventName.SpiralStartCount.ToString(), AddCountUI);
    }
    public void AddCountUI() {
       
        if (currentSlot != null)
        {
            currentSlot.EndCount();
            currentSlot = null;
        }
        Vector3 playerPosition = GameManager.Instance.GetCharacter_Player().myTransform.position;
        playerPosition = Camera.main.WorldToScreenPoint(playerPosition + offSet);
        SpiralCountSlot newSlot = Instantiate(slot, transform.position, Quaternion.identity, transform).GetComponent<SpiralCountSlot>();
        newSlot.pointSpawn = playerPosition;
        slotsCreated.Add(newSlot);
        currentSlot = newSlot;
        currentSlot.ChangeSpiralManager(this);
        currentSlot.StartCount();
        StopAllCoroutines();
    }
    public void CountChange(int spiralCount) {
        if (currentSlot == null)
            return;
        currentSlot.ChangeSpiral(spiralCount);
    }
    public void EndCount() {
        if (currentSlot != null)
        {
            currentSlot.EndCount();
            currentSlot = null;
        }
    }
    public void RemoveSlot(SpiralCountSlot slotRemove) {
        slotsCreated.Remove(slotRemove);
        Destroy(slotRemove.gameObject, .5f);
        if (slotsCreated.Count <= 0)
            StartCoroutine(IE_WaitToCloseSpiralPanel());
    }
    IEnumerator IE_WaitToCloseSpiralPanel() {
        yield return new WaitForSeconds(.5f);
        UIManager.Instance.CloseSpiralPanel();
    }
}
