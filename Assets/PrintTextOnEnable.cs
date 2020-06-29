using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrintTextOnEnable : MonoBehaviour
{
    Coroutine co;
    TextMeshProUGUI textBox;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (textBox == null)
        {
            textBox = GetComponentInChildren<TextMeshProUGUI>();
        }
        co = StartCoroutine(PrintTextToBox(0.5f));
        
    }
    private void OnDisable()
    {
        if(co != null)
            StopCoroutine(co);
    }
    private IEnumerator PrintTextToBox(float speed)
    {
        Debug.Log("printing text");
        textBox.maxVisibleCharacters = 0;
        yield return new WaitForSeconds(0.25f);
        bool textfinished = false;
        int totalVisibleCharacters = textBox.textInfo.characterCount;
        int counter = 0;
        while (!textfinished)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            textBox.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                textfinished = true;
                yield return null;
            }
            counter += 1;
            yield return new WaitForSeconds(speed/totalVisibleCharacters);
        }
    }
}