using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeScriptable : ScriptableObject
{    
    public KitchenObjectScriptable input;
    public KitchenObjectScriptable output;
    public float burningTimerMax;
}
