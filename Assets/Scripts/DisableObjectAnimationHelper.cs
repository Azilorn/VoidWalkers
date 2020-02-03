using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectAnimationHelper : MonoBehaviour
{
    bool finished;
    public float timer;
    public float disableAfterTime;
    // Start is called before the first frame update

    private void Update()
    {
        if (disableAfterTime > 0)
        {
            if (timer < disableAfterTime)
            {
                timer += Time.deltaTime;

            }
            else finished = true;
            if (finished)
            {
                timer = 0;
                finished = false;
                gameObject.SetActive(false);
            }
        }

    }
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
