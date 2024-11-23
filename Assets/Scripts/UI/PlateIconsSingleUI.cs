using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour
{
    [SerializeField] Image image;

    public void SetKitchenObjectScriptable(KitchenObjectScriptable kitchenObjectScriptable)
    {
        image.sprite = kitchenObjectScriptable.sprite;
    }

}
