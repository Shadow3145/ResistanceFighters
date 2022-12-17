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

public class Effect : MonoBehaviour
{
    public EffectType effectType { get; private set; }
    public string effectName { get; private set; }
    public string effectDescription { get; private set; }
}
