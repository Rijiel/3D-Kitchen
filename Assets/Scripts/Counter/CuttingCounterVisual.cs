using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    const string CUT = "Cut";

    [SerializeField] CuttingCounter cuttingCounter;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
