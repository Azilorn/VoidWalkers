using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAnimationEndDestroy : MonoBehaviour
{
    public void DestroyAnimation() {
        Destroy(gameObject);
    }
}
