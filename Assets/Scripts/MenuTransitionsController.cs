using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransitionsController : MonoBehaviour
{
    public static MenuTransitionsController Instance;
    public List<GameObject> transitions = new List<GameObject>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void StartTransition(int i, bool end)
    {
        transitions[i].SetActive(true);
        Animator anim = transitions[i].GetComponent<Animator>();
        anim.SetBool("async", false);
        if (end)
            anim.SetTrigger("end");
        else if (!end) {
            anim.SetTrigger("start");
        }
    }
    public IEnumerator StartTransitionWithDelay(int i, bool end, float delay)
    {
        yield return new WaitForSeconds(delay);
        transitions[i].SetActive(true);
        Animator anim = transitions[i].GetComponent<Animator>();
        anim.SetBool("async", false);
        if (end)
            anim.SetTrigger("end");
        else if (!end)
        {
            anim.SetTrigger("start");
        }
    }
    public IEnumerator StartAsyncTransition(int i) {

        transitions[i].SetActive(true);
        Animator anim = transitions[i].GetComponent<Animator>();
        anim.SetBool("async", true);
        anim.SetTrigger("start");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
    
    }
    public IEnumerator EndAsyncTransition(int i) {

        Animator anim = transitions[i].GetComponent<Animator>();
        anim.SetTrigger("end");
        yield return new WaitForEndOfFrame();
    }

}
