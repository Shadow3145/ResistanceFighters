using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Restoration,
    Immunity,
    Cure,
    Damage,
    Buff,
    Debuff,
    Misc
}
[CreateAssetMenu(menuName = "SO/Alchemy/Effect")]
[System.Serializable]
public class Effect : ScriptableObject
{
    [SerializeField] private EffectType effectType;
    [SerializeField] private string effectName;
    [SerializeField] private string effectDescription;

    public EffectType GetEffectType()
    {
        return effectType;
    }

    public string GetEffectName()
    {
        return effectName;
    }

    public string GetEffectDescription()
    {
        return effectDescription;
    }

}
