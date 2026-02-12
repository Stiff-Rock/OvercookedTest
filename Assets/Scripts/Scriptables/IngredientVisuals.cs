using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientVisuals", menuName = "Kitchen/IngredientVisuals")]
public class IngredientVisuals : ScriptableObject
{
    [SerializeField] private List<IngredientUIInfo> lookupTable;

    public Sprite GetSprite(IngredientType type)
    {
        return lookupTable.Find(x => x.type == type).sprite;
    }
}