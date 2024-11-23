using System;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Header("Keyboard Keys")]
    [SerializeField] TextMeshProUGUI keyboardMoveUpText;
    [SerializeField] TextMeshProUGUI keyboardMoveDownText;
    [SerializeField] TextMeshProUGUI keyboardMoveLeftText;
    [SerializeField] TextMeshProUGUI keyboardMoveRightText;
    [SerializeField] TextMeshProUGUI keyboardInteractText;
    [SerializeField] TextMeshProUGUI keyboardInteractAlternateText;
    [SerializeField] TextMeshProUGUI keyboardPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        UpdateVisual();

        Show();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
            Hide();
    }

    private void GameInput_OnBindingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        keyboardMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyboardMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyboardMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyboardMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyboardInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyboardInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        keyboardPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
