using System;

[Flags]
public enum IngredientState
{
    None = 0,
    Raw = 1,
    Cooked = 2,
    Cut = 4,
    Burnt = 8
}