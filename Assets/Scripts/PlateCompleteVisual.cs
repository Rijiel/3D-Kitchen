using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectScriptable_GameObject
    {
        public KitchenObjectScriptable KitchenObjectScriptable;
        public GameObject gameObject;
    }

    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] List<KitchenObjectScriptable_GameObject> kitchenObjectScriptable_GameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (var kitchenObjectScriptable_GameObject in kitchenObjectScriptable_GameObjectList)
        {
            kitchenObjectScriptable_GameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var kitchenObjectScriptable_GameObject in kitchenObjectScriptable_GameObjectList)
        {
            if (kitchenObjectScriptable_GameObject.KitchenObjectScriptable == e.KitchenObjectScriptable)
            {
                kitchenObjectScriptable_GameObject.gameObject.SetActive(true);
            }
        }
    }
}
