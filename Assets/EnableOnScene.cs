using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnableOnScene : MonoBehaviour
{
    [SerializeField] private List<int> scenesToEnable = new List<int>();
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (scenesToEnable.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            return;
        }
        else gameObject.SetActive(false);
    }
}
