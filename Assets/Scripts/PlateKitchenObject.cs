using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectScriptable KitchenObjectScriptable;
    }

    [SerializeField] List<KitchenObjectScriptable> validKitchenObjectSOList;

    List<KitchenObjectScriptable> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectScriptable>();
    }

    public bool TryAddIngredient(KitchenObjectScriptable kitchenObjectScriptable)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectScriptable)) 
            return false;
        if (kitchenObjectSOList.Contains(kitchenObjectScriptable))
            return false;
        else
        {
            kitchenObjectSOList.Add(kitchenObjectScriptable);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { KitchenObjectScriptable = kitchenObjectScriptable });
            return true;
        }
    }

    public List<KitchenObjectScriptable> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }

}
