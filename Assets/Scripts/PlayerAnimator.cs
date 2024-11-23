using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    const string IS_WALKING = "IsWalking";

    [SerializeField] Player player;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
