using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EdibleItem : Item, IDestroyableItem, IItemAction
{
    [SerializeField]
    private List<ModifierData> modifierData = new List<ModifierData>();

    public string ActionName => "Use";

    [field: SerializeField] 
    public AudioClip actionSFX { get; private set; }

    //changes player state
    public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
    {
        foreach (ModifierData data in modifierData)
        {
            data.statModifier.AffectCharacter(character, data.value);
        }
        return true;
    }

}

public interface IDestroyableItem
{

}

public interface IItemAction
{
    public string ActionName { get; }
    public AudioClip actionSFX { get; }

    //verb, bascially what the item will do
    bool PerformAction(GameObject character, List<ItemParameter>itemState);
}

[Serializable]
public class ModifierData
{
    public CharacterStatModifier statModifier;
    public float value;
}
