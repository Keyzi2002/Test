using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Model", menuName = "ScriptAbleObjects/NewModel")]
public class ModelAbleObj : ScriptableObject
{
    public GameObject skin;
    public int modelID;
    public Rarity rarity;
    public ModelType modelType;
    public Sprite iconOn;
    public Payment payment;
    public int priceAmount;
}
