using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    public static DeliveryManager Instance { get; private set; }

    [SerializeField] RecipeListScriptable recipeListScriptable;

    List<RecipeScriptable> waitingRecipeScriptableList;
    float spawnRecipeTimer;
    float SpawnRecipeTimerMax = 4f;
    int waitingRecipeMax = 4;
    int successfulRecipesAmount;

    void Awake()
    {
        Instance = this;
        waitingRecipeScriptableList = new List<RecipeScriptable>();
    }

    void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = SpawnRecipeTimerMax;

            if (GameManager.Instance.IsGamePlaying() && waitingRecipeScriptableList.Count < waitingRecipeMax)
            {
                RecipeScriptable waitingRecipeScriptable = recipeListScriptable.recipeScriptableList[UnityEngine.Random.Range(0, recipeListScriptable.recipeScriptableList.Count)];
                waitingRecipeScriptableList.Add(waitingRecipeScriptable);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeScriptableList.Count; i++)
        {
            RecipeScriptable waitingRecipeScriptable = waitingRecipeScriptableList[i];

            // check if has the same number of ingredients
            if (waitingRecipeScriptable.kitchenObjectScriptableList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;

                // cycling through all ingredients in the recipe
                foreach (KitchenObjectScriptable recipeKitchenObjectScriptable in waitingRecipeScriptable.kitchenObjectScriptableList)
                {
                    bool ingredientFound = false;
                    // cycling through all ingredients in the plate
                    foreach (KitchenObjectScriptable plateKitchenObjectScriptable in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectScriptable == recipeKitchenObjectScriptable)
                        {
                            // ingredients match
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        // this ingredient was not found in the plate
                        break;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    // all ingredients have been found
                    successfulRecipesAmount++;

                    waitingRecipeScriptableList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        // no matches found
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeScriptable> GetWaitingRecipeList()
    {
        return waitingRecipeScriptableList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
