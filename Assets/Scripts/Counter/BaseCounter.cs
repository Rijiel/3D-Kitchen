using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedDown;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedDown = null;
    }

    [SerializeField] Transform counterTopPoint;

    KitchenObject kitchenObject;

    public virtual void Interact(Player player) 
    {
        Debug.LogError("BaseCounter.Interact() was not implemented");
    }

    public virtual void InteractAlternate()
    {
        //Debug.LogError("BaseCounter.InteractAlternate() was not implemented");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (HasKitchenObject())
            OnAnyObjectPlacedDown?.Invoke(this, EventArgs.Empty);        
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
