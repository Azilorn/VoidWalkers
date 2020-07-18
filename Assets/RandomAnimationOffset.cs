using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationOffset : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
            animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("offset", Random.Range(0, 1f));
    }
   
}
