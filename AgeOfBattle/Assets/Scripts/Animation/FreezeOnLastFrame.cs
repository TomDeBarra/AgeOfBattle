using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeOnLastFrame : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FreezeAnimation()
    {
        animator.enabled = false; // Freeze at the current frame
    }
}

