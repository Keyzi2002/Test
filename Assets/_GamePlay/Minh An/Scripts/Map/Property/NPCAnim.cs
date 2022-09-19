using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnim : MonoBehaviour
{
    private static string NameAnim_Dancing = "Dancing";
    private static string NameAnim_Maraschino = "Maraschino";
    private static string NameAnim_Ymca = "Ymca";
    private static string NameAnim_Snake = "Snake";
    private static string NameAnim_Macarena = "Macarena";
    private static string NameAnim_Chicken = "Chicken";
    private static string NameAnim_Hiphop = "Hiphop";
    private static string NameAnim_Shooting = "Shooting";

    [SerializeField] Animator animator;
    [SerializeField] PNCAnimate currentAnim;


    void Start()
    {
        if(animator == null) { animator = GetComponent<Animator>(); }
       
        SetAnimation();
    }

    void SetAnimation()
    {
        switch (currentAnim)
        {
            case PNCAnimate.Dancing:
                animator.SetBool(NameAnim_Dancing, true);
                break;
            case PNCAnimate.Maraschino:
                animator.SetBool(NameAnim_Maraschino, true);
                break;
            case PNCAnimate.Ymca:
                animator.SetBool(NameAnim_Ymca, true);
                break;
            case PNCAnimate.Snake:
                animator.SetBool(NameAnim_Snake, true);
                break;
            case PNCAnimate.Macarena:
                animator.SetBool(NameAnim_Macarena, true);
                break;
            case PNCAnimate.Chicken:
                animator.SetBool(NameAnim_Chicken, true);
                break;
            case PNCAnimate.Hiphop:
                animator.SetBool(NameAnim_Hiphop, true);
                break;
            case PNCAnimate.Shooting:
                animator.SetBool(NameAnim_Shooting, true);
                break;
            default:
                break;
        }
    }
    public enum PNCAnimate
    {
        Dancing,
        Maraschino,
        Ymca,
        Snake,
        Macarena,
        Chicken,
        Hiphop,
        Shooting,

    }
}
