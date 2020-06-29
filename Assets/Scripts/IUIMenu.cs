using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIMenu {

    IEnumerator OnMenuActivated();
    IEnumerator OnMenuDeactivated();
    IEnumerator OnMenuFoward();
    IEnumerator OnMenuBackwards();
}
