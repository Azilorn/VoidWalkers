using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTester : MonoBehaviour
{
    public Ability testAbility;

    public void TestAbility() {
        StartCoroutine(TestAbilityCoroutine());
    }
    public IEnumerator TestAbilityCoroutine() {
       yield return StartCoroutine(BattleController.Instance.AttackController.TestAbility(testAbility));
    }
}
