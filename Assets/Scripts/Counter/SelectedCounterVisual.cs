using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] BaseCounter baseCounter;
    [SerializeField] GameObject[] visualGameObjectArray;

    void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    void Player_OnSelectedCounterChanged(object sender, Player.SelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
            Hide();
    }

    void Show()
    {
        foreach (GameObject gameObject in visualGameObjectArray)
            gameObject.SetActive(true);
    }

    void Hide()
    {
        foreach (GameObject gameObject in visualGameObjectArray)
            gameObject.SetActive(false);
    }
}
