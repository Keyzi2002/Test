using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayer : MonoBehaviour
{
    private const string Name_Anim_Run_FW = "Run FW";
    private const string Name_Anim_Idle = "Idle";
    private const string Name_Anim_Fall = "Fall";
    private const string Name_Anim_StandingUp = "Standing Up";
    #region Dance
    private const string Name_Anim_Dance_Chicken = "Dance Chicken";
    private const string Name_Anim_Dance_WaveHipHop = "Dance WaveHipHop";
    private const string Name_Anim_Dance_Dancing = "Dancing";
    private const string Name_Anim_Dance_Maraschino = "Dancing Maraschino";
    private const string Name_Anim_Dance_Ymca = "Dance Ymca";
    private const string Name_Anim_Dance_Snake = "Dance Snake";
    private const string Name_Anim_Dance_Macarena = "Dance Macarena";
    #endregion

    [SerializeField] private Animator animatorPlayer;
    public string nameAnimCurrent = "";
    private TypeAnimVictory typeAnimVictoryCurrent = TypeAnimVictory.None;

    private void Awake()
    {
        OnInIt();
    }
    private void OnInIt()
    {
        if (animatorPlayer == null) { animatorPlayer = GetComponentInChildren<Animator>(); }
    }
    public void ResetAnim() {
        animatorPlayer = GetComponentInChildren<Animator>();
    }
    public void PlayAnim(ActionAnim actionAnim)
    {
        switch (actionAnim)
        {
            case ActionAnim.Run:
                OnChangeAnim(Name_Anim_Run_FW);
                break;
            case ActionAnim.Idle:
                OnChangeAnim(Name_Anim_Idle);
                break;
            case ActionAnim.Victory:
                if(typeAnimVictoryCurrent == TypeAnimVictory.None)
                {
                    var typeAnimVictories = System.Enum.GetValues(typeof(TypeAnimVictory));
                    int indexRanom = Random.Range(0, typeAnimVictories.Length);
                    typeAnimVictoryCurrent = (TypeAnimVictory)typeAnimVictories.GetValue(indexRanom);
                }
                switch (typeAnimVictoryCurrent)
                {
                    case TypeAnimVictory.Chicken:
                        OnChangeAnim(Name_Anim_Dance_Chicken);
                        break;
                    case TypeAnimVictory.WaveHipHop:
                        OnChangeAnim(Name_Anim_Dance_WaveHipHop);
                        break;
                    case TypeAnimVictory.Dancing:
                        OnChangeAnim(Name_Anim_Dance_Dancing);
                        break;
                    case TypeAnimVictory.Maraschino:
                        OnChangeAnim(Name_Anim_Dance_Maraschino);
                        break;
                    case TypeAnimVictory.Ymca:
                        OnChangeAnim(Name_Anim_Dance_Ymca);
                        break;
                    case TypeAnimVictory.Snake:
                        OnChangeAnim(Name_Anim_Dance_Snake);
                        break;
                    case TypeAnimVictory.Macarena:
                        OnChangeAnim(Name_Anim_Dance_Macarena);
                        break;
                }
                break;
            case ActionAnim.Die:
                OnChangeAnim(Name_Anim_Idle);
                break;
            case ActionAnim.Fall:
                OnChangeAnim(Name_Anim_Fall);
                break;
            case ActionAnim.StandingUp:
                OnChangeAnim(Name_Anim_StandingUp);
                break;
        }
    }
    private void OnChangeAnim(string nameAnim)
    {
        if(nameAnimCurrent == nameAnim)
        {
            return;
        }
        nameAnimCurrent = nameAnim;
        animatorPlayer.Play(nameAnimCurrent);
    }
    public enum TypeAnimVictory
    {
        None,
        Chicken,
        WaveHipHop,
        Dancing,
        Maraschino,
        Ymca,
        Snake,
        Macarena,
    }
    public enum ActionAnim
    {
        Run,
        Idle,
        Victory,
        Die,
        Fall,
        StandingUp
    }
}
