using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Destination : MonoBehaviour
{
    public Transform myTransform;
    [SerializeField] private List<TransTop> transformTops = new List<TransTop>();
    [SerializeField] private AnimationCurve animationCharacterLerpPositionCurve;
    [Header("Range Fireworks")]
    [SerializeField] private float YOffset;
    [SerializeField] private float MinX;
    [SerializeField] private float MaxX;
    [SerializeField] private float MaxY;
    [SerializeField] private float MinY;
    [SerializeField] private float TimeDelayInOneShot;
    private float ZOffset;

    private int CountCharacterGoThis;
    private void Awake()
    {
        if(myTransform == null) { myTransform = this.transform; }
        CountCharacterGoThis = 0;
        MinX += transformTops[1].transformTop.position.x;
        MaxX += transformTops[1].transformTop.position.x;
        MaxY += YOffset + myTransform.position.y;
        MinY += YOffset + myTransform.position.y;
        ZOffset = transformTops[0].transformTop.position.z;
    }
    /// <summary>
    /// AnimDone ==> change Position of charactor
    /// Need change camera position
    /// </summary>
    /// <param name="other"></param>
    /// 
    private void OnTriggerEnter(Collider other)
    {
        Character character = Cache.GetComponetCharacter(other);

        if(character != null)
        {
            if(CountCharacterGoThis >= 3)
            {
                character.isIgnoreMove = true;
                character.Active(false);
                return;
            }
            RankingManager.Instance.SetRank(character);
            character.isIgnoreMove = true;
            //if (GameManager.Instance.GetCharacter_Player() == character)
            //{
            //    Canvas_JOYSTICK.Share_CANVASJOYSTICK.Close();
            //}
            StartCoroutine(IE_CharacterFollowThis(character));
            StartCoroutine(IE_Singtelon.IE_DelayAction(() =>
            {
                foreach(TransTop transTop in transformTops)
                {
                    if(transTop.top == RankingManager.Instance.GetRank(character))
                    {
                        //character.myTransform.position = transTop.transformTop.position;
                        //Add top 3 charactor first then call animStart
                        RankingAnimManager.Instance.AddCharactor(character);
                        CountCharacterGoThis++;
                        if (GameManager.Instance.GetCharacter_Player() == character)
                        {
                            foreach (Character _character in LoadRankRandom(3 - CountCharacterGoThis))
                            {
                                RankingAnimManager.Instance.AddCharactor(_character);
                                _character.statePlayer = StatePlayer.Victory;
                                CountCharacterGoThis++;
                            }
                        }
                      
                        RankingAnimManager.Instance.AnimStart();
                        character.statePlayer = StatePlayer.Victory;
                        StartCoroutine(IE_Singtelon.IE_DelayAction(() => { StartCoroutine(IE_Fireworks(TimeDelayInOneShot)); }, 1));
                        if (CountCharacterGoThis == 3)
                        {

                            if (RankingAnimManager.Instance.GetCharacters_InRank()[0] == GameManager.Instance.GetCharacter_Player()) //Player
                            {
                                GameManager.Instance.Ending(GameManager.TypeEnding.Win);
                            }
                            else
                            {
                                GameManager.Instance.Ending(GameManager.TypeEnding.Lose);
                            }

                        }
                        return;
                    }
                }
            }, 1));
        }
    }
    IEnumerator IE_CharacterFollowThis(Character character)
    {
        Vector3 vt_Start = character.myTransform.position;
        Vector3 vt_End = myTransform.position;
        vt_End.y = vt_Start.y;
        float m_time = 0;
        while(m_time < 1)
        {
            character.myTransform.position = Vector3.Lerp(vt_Start, vt_End, animationCharacterLerpPositionCurve.Evaluate(m_time));
            character.LookAt(vt_End);
            m_time += Time.deltaTime;
            yield return null;
        }
    }
    private List<Character> LoadRankRandom(int amountCharacterLoad)
    {
        List<Character> newRank = new List<Character>();
        int indexRandomTop = 0;
        List<Character> charactersInGame = new List<Character>();
        foreach (Character character in GameManager.Instance.GetCharacters_InGame())
        {
            if (character != GameManager.Instance.GetCharacter_Player())
            {
                charactersInGame.Add(character);
            }
        }
        foreach(Character character in RankingAnimManager.Instance.GetCharacters_InRank())
        {
            if (charactersInGame.Contains(character))
            {
                charactersInGame.Remove(character);
            }
           
        }
        for (int i = 0; i < amountCharacterLoad; i++)
        {
            indexRandomTop = UnityEngine.Random.Range(0, charactersInGame.Count);
            newRank.Add(charactersInGame[indexRandomTop]);
            charactersInGame.RemoveAt(indexRandomTop);
        }
        return newRank;
    }
    public Transform GetTransformTop(int index) { return transformTops[index].transformTop; }
    IEnumerator IE_Fireworks(float TimeDelayInOneShot)
    {
        ParticlePool.Play(PoolsManager.Instance.GetParticleSystem_ConfettiExplosion_Random(), new Vector3(UnityEngine.Random.Range(MinX, MaxX), 
            UnityEngine.Random.Range(MinY, MaxY), ZOffset), Quaternion.identity, PoolsManager.Instance.myTransform);
        yield return Cache.GetWaiforSecond(TimeDelayInOneShot);
        StartCoroutine(IE_Fireworks(TimeDelayInOneShot));
    }
}
[Serializable]
public class TransTop
{
    public RankingManager.Top top;
    public Transform transformTop;
}
