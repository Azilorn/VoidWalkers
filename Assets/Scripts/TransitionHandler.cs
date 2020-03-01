using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour
{
   bool playing = false;
   [SerializeField] bool delaying = true;
   [SerializeField] bool loadBattle = true;
   [SerializeField] Sprite[] sprites;
   [SerializeField] Image image;
   [SerializeField] Image childImage;
   [SerializeField] int fps;
   int currentIndex = 0;
   [SerializeField] int LoadCanvasAtFrame = 0;
   float timer = 0;
   [SerializeField] float delay = 0;
   [SerializeField] TransitionHandler nextTransition;
    
    public bool Playing { get => playing; set => playing = value; }
    public int Fps { get => fps; set => fps = value; }
    public Sprite[] Sprites { get => sprites; set => sprites = value; }

    private void OnEnable()
    {
        PlayTransition();
    }
    private void Update()
    {
        if (playing) {

            if (currentIndex == LoadCanvasAtFrame)
            {
                if (loadBattle)
                {
                    CoreUI.Instance.SetBattleUIAtStart();
                    CoreUI.Instance.BattleCanvasTransform.gameObject.SetActive(true);
                    
                }
                else if(nextTransition != null) {
                    nextTransition.gameObject.SetActive(true);
                    nextTransition.PlayTransition();
                    gameObject.SetActive(false);
                    return;
                }
            }
            if (currentIndex >= Sprites.Length) {
                playing = false;
                image.enabled = false;
                if (childImage != null)
                {
                    childImage.enabled = false;
                }
              
                gameObject.SetActive(false);
                CoreUI.Instance.Backgrounds[0].SetActive(true);
                return;
            }
            if (delay > 0 && delaying == true) {
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    delaying = false;
                }
                else return;
            }
             timer += Time.deltaTime;
            if (timer > (1 % 60) / fps) {
                image.overrideSprite = Sprites[currentIndex++];
                timer = 0;            
            }
        }
    }

    public void PlayTransition() {

        playing = true;
        currentIndex = 0;
        image.overrideSprite = Sprites[currentIndex];
        image.enabled = true;
        if (childImage != null)
        {
            childImage.enabled = true;
        }
    }
}
