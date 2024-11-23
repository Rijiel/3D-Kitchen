using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] KitchenObjectScriptable plateKitchenObjectScriptable;

    float spawnPlateTimer;
    float spawnPlateTimerMax = 4f;
    int platesSpawnedAmount;
    int platesSpawnedAmountMax = 4;

    void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;

            if (GameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectScriptable, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            // Player has no kitchen object
        }

    }
}
