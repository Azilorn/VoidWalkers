using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransitionAnim : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        if (anim == null) {
            anim = GetComponent<Animator>();
        }
    }
    private void OnDisable()
    {
        anim.SetBool("async", false);
    }
}
