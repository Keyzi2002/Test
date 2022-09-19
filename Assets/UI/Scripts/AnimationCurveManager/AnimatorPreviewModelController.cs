using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatorPreviewModelController : MonoBehaviour
{
    Animator anim;
    public void InitData() {
        anim = GetComponent<Animator>();
        EnventManager.AddListener(EventName.ChangeAnimModel.ToString(), ChangeNormalAnim);
    }
    public Animator GetAnimator() { return anim; }
    public void ChangeDanceAnim(int danceID) {
        anim.SetInteger("DanceID", danceID);
    }
    void ChangeNormalAnim() {
        int randomAnim = Random.Range(0, 3);
        switch (randomAnim)
        {
            case 2:
                anim.Play("Idle");
                break;
            case 1:
                anim.Play("Dancing");
                break;
            case 0:
                anim.Play("Run FW");
                break;
            default:
                break;
        }
    }
    public void Reset()
    {
        anim.SetInteger("DanceID", -1);
        anim.Play("Idle");
    }
}
