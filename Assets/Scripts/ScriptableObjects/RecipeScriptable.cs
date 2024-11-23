using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeScriptable : ScriptableObject
{
    public List<KitchenObjectScriptable> kitchenObjectScriptableList;
    public string recipeName;
}
