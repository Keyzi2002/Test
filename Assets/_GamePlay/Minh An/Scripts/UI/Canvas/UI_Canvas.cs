using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
    public bool isIgnoreOpen = false;
    private bool isStop;
    public virtual void OnInIt()
    {

    }
    public virtual void Open()
    {
        if (isIgnoreOpen)
        {
            return;
        }
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    public bool IsStop()
    {
        return isStop;
    }

    public void Stop(bool isStop)
    {
        this.isStop = isStop;
    }
}
