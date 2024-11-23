using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    public event EventHandler OnCut;

    [SerializeField] CuttingRecipeScriptable[] CuttingRecipeSOArray;

    int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectScriptable()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeScriptable cuttingRecipeScriptable = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectScriptable());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() { progressNormalized = (float)cuttingProgress / cuttingRecipeScriptable.cuttingProgressMax });
                }
            }
            else
            {
                // No nothing
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectScriptable()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate()
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectScriptable()))
        {
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeScriptable cuttingRecipeScriptable = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectScriptable());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() { progressNormalized = (float)cuttingProgress / cuttingRecipeScriptable.cuttingProgressMax });

            if (cuttingProgress >= cuttingRecipeScriptable.cuttingProgressMax)
            {
                KitchenObjectScriptable kitchenObjectScriptable = GetOutputForInput(GetKitchenObject().GetKitchenObjectScriptable());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(kitchenObjectScriptable, this);
            }
        }
    }

    bool HasRecipeWithInput(KitchenObjectScriptable inputKitchenObjectSO)
    {
        CuttingRecipeScriptable cuttingRecipeScriptable = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeScriptable != null;
    }

    KitchenObjectScriptable GetOutputForInput(KitchenObjectScriptable inputKitchenObjectSO)
    {
        CuttingRecipeScriptable cuttingRecipeScriptable = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeScriptable != null)
            return cuttingRecipeScriptable.output;
        else
            return null;
    }

    CuttingRecipeScriptable GetCuttingRecipeSOWithInput(KitchenObjectScriptable inputKitchenObjectSO)
    {
        foreach (CuttingRecipeScriptable cuttingRecipeScriptable in CuttingRecipeSOArray)
        {
            if (cuttingRecipeScriptable.input == inputKitchenObjectSO)
            {
                return cuttingRecipeScriptable;
            }
        }
        return null;
    }
}
