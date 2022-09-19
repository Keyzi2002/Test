using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Collider colliderThis;
    private Character characterTaget;
    private Character characterThis;
    private void Awake()
    {
        if(characterThis == null) { characterThis = Cache.GetComponetCharacterInParent(colliderThis); }
    }
    private void OnTriggerEnter(Collider other)
    {
        characterTaget = null;
        characterTaget = Cache.GetComponetCharacterInParent(other);
        if(characterTaget != null)
        {
            if(characterThis == GameManager.Instance.GetCharacter_Player())
            {
                if(characterThis.GetBagTerrainRoll().GetScoreTerrainRollInBag() >= characterTaget.GetBagTerrainRoll().GetScoreTerrainRollInBag())
                {
                    characterTaget.statePlayer = StatePlayer.Fall;
                }
            }
            else
            {
                if (characterThis.GetBagTerrainRoll().GetScoreTerrainRollInBag() > characterTaget.GetBagTerrainRoll().GetScoreTerrainRollInBag())
                {
                    characterTaget.statePlayer = StatePlayer.Fall;
                }
            }
            if(characterTaget.statePlayer == StatePlayer.Fall)
            {
                ThrillingManager.Instance.OpenThrilling(characterThis, characterTaget);
            }
        }
    }
}
