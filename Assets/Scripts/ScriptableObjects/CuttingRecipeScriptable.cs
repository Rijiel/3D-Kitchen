using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeScriptable : ScriptableObject
{    
    public KitchenObjectScriptable input;
    public KitchenObjectScriptable output;
    public int cuttingProgressMax;
}
