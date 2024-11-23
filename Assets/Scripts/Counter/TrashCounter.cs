using System;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelfWhileShrinking();
        }
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (!HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);    
                
                OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
