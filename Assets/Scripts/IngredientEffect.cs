using System;
using UnityEngine;

[System.Serializable]
public class IngredientEffect : IEquatable<IngredientEffect>, IComparable<IngredientEffect>
{
    [SerializeField] private Effect effect;
    [SerializeField] private float effectStrength;

    public IngredientEffect(Effect effect, float effectStrength)
    {
        this.effect = effect;
        this.effectStrength = effectStrength;
    }

    public Effect GetEffect()
    {
        return effect;
    }

    public float GetEffectStrength()
    {
        return effectStrength;
    }

    public string GetEffectDescriptionHeading()
    {
        return effect.GetEffectName() + " " + GetEffectStrengthDescription();
    }

    public string GetEffectStrengthDescription()
    {
        return "(" + effectStrength.ToString() + "/15)";
    }

    public void ChangeEffectStrength(float value)
    {
        effectStrength += value;
    }
    
    public bool Equals(IngredientEffect other)
    {
        return effect == other.effect;
    }

    public override bool Equals(object obj)
    {
        IngredientEffect other = obj as IngredientEffect;
        return this.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public int CompareTo(IngredientEffect other)
    {
        return effectStrength.CompareTo(other.effectStrength);
    }
}
