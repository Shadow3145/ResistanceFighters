using UnityEngine;

[System.Serializable]
public class IngredientEffect
{
    [SerializeField] private Effect effect;
    [SerializeField] private float effectStrenght;

    public Effect GetEffect()
    {
        return effect;
    }

    public float GetEffectStrength()
    {
        return effectStrenght;
    }
}
