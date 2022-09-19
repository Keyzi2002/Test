using ClipperLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    private static Dictionary<Collider, Step_> stepControlls_InParent = new Dictionary<Collider, Step_>();
    public static Step_ GetComponetStepInParent(Collider collider)
    {
        if (!stepControlls_InParent.ContainsKey(collider))
        {
            stepControlls_InParent.Add(collider, collider.GetComponentInParent<Step_>());
        }
        return stepControlls_InParent[collider];
    }
    private static Dictionary<Collider, Character> Character_InParent = new Dictionary<Collider, Character>();
    public static Character GetComponetCharacterInParent(Collider collider)
    {
        if (!Character_InParent.ContainsKey(collider))
        {
            Character_InParent.Add(collider, collider.GetComponentInParent<Character>());
        }
        return Character_InParent[collider];
    }
    private static Dictionary<Collider, Character> Character = new Dictionary<Collider, Character>();
    public static Character GetComponetCharacter(Collider collider)
    {
        if (!Character.ContainsKey(collider))
        {
            Character.Add(collider, collider.GetComponent<Character>());
        }
        return Character[collider];
    }
    private static Dictionary<float, WaitForSeconds> waiforseconds = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWaiforSecond(float value)
    {
        if (!waiforseconds.ContainsKey(value))
        {
            waiforseconds.Add(value, new WaitForSeconds(value));
        }
        return waiforseconds[value];
    }
    private static Dictionary<Collider, Distance> Distance_InParent = new Dictionary<Collider, Distance>();
    public static Distance GetComponetDistanceInParent(Collider collider)
    {
        if (!Distance_InParent.ContainsKey(collider))
        {
            Distance_InParent.Add(collider, collider.GetComponentInParent<Distance>());
        }
        return Distance_InParent[collider];
    }
    private static Dictionary<Collider, DestructibleTerrain> DestructibleTerrain_InParent = new Dictionary<Collider, DestructibleTerrain>();
    public static DestructibleTerrain GetComponetDestructibleTerrainInParent(Collider collider)
    {
        if (!DestructibleTerrain_InParent.ContainsKey(collider))
        {
            DestructibleTerrain_InParent.Add(collider, collider.GetComponentInParent<DestructibleTerrain>());
        }
        return DestructibleTerrain_InParent[collider];
    }
    private static Dictionary<Character, UI_InCharacter> ui_InCharacters = new Dictionary<Character, UI_InCharacter>();
    public static UI_InCharacter GetComponetUIInCharacter(Character character)
    {
        if (!ui_InCharacters.ContainsKey(character))
        {
            ui_InCharacters.Add(character, character.GetComponent<UI_InCharacter>());
        }
        return ui_InCharacters[character];
    }
   
}
