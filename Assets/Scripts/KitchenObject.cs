using System.Collections;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectScriptable kitchenObjectScriptable;

    IKitchenObjectParent kitchenObjectParent;

    float elapsedTime;

    public KitchenObjectScriptable GetKitchenObjectScriptable()
    {
        return kitchenObjectScriptable;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        this.kitchenObjectParent?.ClearKitchenObject();

        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
            Debug.LogError("The IkitchenObjectParent has an object already");

        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public void DestroySelfWhileShrinking()
    {
        float destroyDelay = .75f;
        float shrinkDelayMultiplier = .5f;
        Vector3 initialScale = transform.localScale;

        elapsedTime += Time.deltaTime;
        float t = elapsedTime / destroyDelay * shrinkDelayMultiplier;
        transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
        transform.position += Vector3.down * Time.deltaTime;

        if (elapsedTime >= destroyDelay)
        {
            elapsedTime = 0f;
            kitchenObjectParent.ClearKitchenObject();
            Destroy(gameObject);
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectScriptable kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}
