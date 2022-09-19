using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrillingManager : GenerticSingleton<ThrillingManager>
{

    public void OpenThrilling(Character characterFun, Character characterAngry)
    {
        Cache.GetComponetUIInCharacter(characterAngry).Open_Thrilling(NameThrilling.Angry);
        Cache.GetComponetUIInCharacter(characterFun).Open_Thrilling(NameThrilling.Fun);
    }
    public enum NameThrilling
    {
        Angry,
        Fun
    }
}
