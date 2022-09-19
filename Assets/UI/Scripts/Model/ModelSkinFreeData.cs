using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SkinFreeData {
    public ModelAbleObj modelSkin;
    public int levelGet;
}
[CreateAssetMenu(fileName = "ModelData", menuName = "ScriptAbleObjects/NewFreeSkinData")]
public class ModelSkinFreeData : ScriptableObject
{
    public List<SkinFreeData> freeSkins;
}

