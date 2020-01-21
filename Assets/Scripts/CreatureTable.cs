using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureTable : MonoBehaviour
{
    public static CreatureTable Instance;
    [SerializeField] private List<CreatureSO> creatureSOs = new List<CreatureSO>();
    [SerializeField] private List<bool> unlockedCreature = new List<bool>();

    public List<CreatureSO> Creatures { get => creatureSOs; set => creatureSOs = value; }
    public List<bool> UnlockedCreature { get => unlockedCreature; set => unlockedCreature = value; }

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
    public int ReturnCreatureID(CreatureSO creature)
    {
        for (int i = 0; i < creatureSOs.Count; i++)
        {
            if (creature == creatureSOs[i]) {
                return i;
            }
        }
        return 0;
    }
}
