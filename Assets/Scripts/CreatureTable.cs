using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureTable : MonoBehaviour
{
    public static CreatureTable Instance;
    [SerializeField] private List<CreatureSO> creatureSOs = new List<CreatureSO>();

    public List<CreatureSO> Creatures { get => creatureSOs; set => creatureSOs = value; }

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
}
