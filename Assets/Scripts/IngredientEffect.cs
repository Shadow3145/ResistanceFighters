using UnityEngine;

[System.Serializable]
public class IngredientEffect
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
}
