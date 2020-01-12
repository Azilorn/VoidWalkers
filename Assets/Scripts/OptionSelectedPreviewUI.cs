using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public enum OptionSelectedType {Battle, Tavern, Reward, Shop, Boss}
public class OptionSelectedPreviewUI : MonoBehaviour
{
    [Label("Battle = 0, Tavern = 1, Reward = 2, Shop = 3, Boss = 4")]
    public List<Sprite> optionIcons;

    public List<Image> options;

    public void SetOptions(int optionImage, OptionSelectedType optionType)
    {
        switch (optionType)
        {
            case OptionSelectedType.Battle:
                options[optionImage].sprite = optionIcons[0];
                break;
            case OptionSelectedType.Tavern:
                options[optionImage].sprite = optionIcons[1];
                break;
            case OptionSelectedType.Reward:
                options[optionImage].sprite = optionIcons[2];
                break;
            case OptionSelectedType.Shop:
                options[optionImage].sprite = optionIcons[3];
                break;
            case OptionSelectedType.Boss:
                options[optionImage].sprite = optionIcons[4];
                break;
        }

    }
    public void AddOptionSelectionUI(int i ) {

        if (i >= options.Count)
        {
            GameObject go = Instantiate(options[0].gameObject, options[0].transform.parent) as GameObject;
            options.Add(go.GetComponent<Image>());
        }
    }
} 
