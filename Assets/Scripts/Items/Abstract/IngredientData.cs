using System;
using UnityEngine;

[Serializable]
public class IngredientData
{
    [field: SerializeField] public IngredientType Type { get; private set; }
    [field: SerializeField] public IngredientState State { get; private set; }

    public IngredientData(IngredientType Type, IngredientState State)
    {
        this.Type = Type;
        this.State = State;
    }

    public override bool Equals(object obj)
    {
        if (obj is IngredientData other)
            return other.Type == Type && other.State == State;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Type, State);

    public override string ToString()
    {
        return $"IngredientType: {Type} || IngredientState: {State}";
    }
}