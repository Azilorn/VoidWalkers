using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static Animator anim;
    public static Transform characterTransform;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        characterTransform = transform;
    }

    private void FixedUpdate()
    {
       
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            MoveRight();
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            MoveLeft();
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            MoveUp();
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            MoveDown();
        }
        else
        {
            anim.SetBool("idle", true);
        }

    }

    public static void MoveDown()
    {
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy || BattleUI.Instance.BattleTransitionManager.gameObject.activeInHierarchy)
            return;
        characterTransform.position += new Vector3(0, -4.5f * Time.deltaTime);
        anim.SetInteger("vertical", -1);
        anim.SetInteger("horizontal", 0);
        anim.SetBool("idle", false);
    }

    public static void MoveUp()
    {
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy || BattleUI.Instance.BattleTransitionManager.gameObject.activeInHierarchy)
            return;
        characterTransform.position += new Vector3(0, 4.5f * Time.deltaTime);
        anim.SetInteger("vertical", 1);
        anim.SetInteger("horizontal", 0);
        anim.SetBool("idle", false);
    }

    public static void MoveLeft()
    {
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy || BattleUI.Instance.BattleTransitionManager.gameObject.activeInHierarchy)
            return;
        characterTransform.position += new Vector3(-4.5f * Time.deltaTime, 0);
        anim.SetInteger("horizontal", -1);
        anim.SetInteger("vertical", 0);
        anim.SetBool("idle", false);
    }

    public static void MoveRight()
    {
        if (BattleUI.Instance.BattleCanvasTransform.gameObject.activeInHierarchy || BattleUI.Instance.BattleTransitionManager.gameObject.activeInHierarchy)
            return;
        characterTransform.position += new Vector3(4.5f * Time.deltaTime, 0);
        anim.SetInteger("horizontal", 1);
        anim.SetInteger("vertical", 0);
        anim.SetBool("idle", false);
    }
}
