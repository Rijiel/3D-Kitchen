using System;
using System.Collections;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] FryingRecipeScriptable[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeScriptable[] burningRecipeSOArray;

    float fryingTimer;
    float burningTimer;
    FryingRecipeScriptable fryingRecipeScriptable;
    BurningRecipeScriptable burningRecipeScriptable;
    State state;

    void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeScriptable.fryingTimerMax });

                    if (fryingTimer > fryingRecipeScriptable.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeScriptable.output, this);

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeScriptable = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectScriptable());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burningTimer / burningRecipeScriptable.burningTimerMax });

                    if (burningTimer > burningRecipeScriptable.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeScriptable.output, this);
                        Debug.Log(burningRecipeScriptable.output);
                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectScriptable()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeScriptable = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectScriptable());

                    state = State.Frying;
                    fryingTimer = 0;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipeScriptable.fryingTimerMax });
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

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
            }
        }
    }

    bool HasRecipeWithInput(KitchenObjectScriptable inputKitchenObjectSO)
    {
        FryingRecipeScriptable fryingRecipeScriptable = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeScriptable != null;
    }

    KitchenObjectScriptable GetOutputForInput(KitchenObjectScriptable inputKitchenObjectSO)
    {
        FryingRecipeScriptable fryingRecipeScriptable = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeScriptable != null)
            return fryingRecipeScriptable.output;
        else
            return null;
    }

    FryingRecipeScriptable GetFryingRecipeSOWithInput(KitchenObjectScriptable inputKitchenObjectSO)
    {
        foreach (FryingRecipeScriptable fryingRecipeScriptable in fryingRecipeSOArray)
        {
            if (fryingRecipeScriptable.input == inputKitchenObjectSO)
            {
                return fryingRecipeScriptable;
            }
        }
        return null;
    }

    BurningRecipeScriptable GetBurningRecipeSOWithInput(KitchenObjectScriptable inputKitchenObjectSO)
    {
        foreach (BurningRecipeScriptable burningRecipeScriptable in burningRecipeSOArray)
        {
            if (burningRecipeScriptable.input == inputKitchenObjectSO)
            {
                return burningRecipeScriptable;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
