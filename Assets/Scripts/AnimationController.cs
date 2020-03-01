using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ImageAnimation
{
    None, MoveHorizontal, MoveHorizontalAndRotationToRight, MoveVertical, MoveVerticalBounce, SpawnAnimSprite, Circle, RandomPositions, TakeDamage,
    TakeDamageShakeNoFade, Glimmer, Burn, Poison, Frozen, Sleep, Confused, Shocked, Rampage, Swirl, Ethereal, RotationSideToSide, SpawnAnimSpriteAsBackground, MoveHorizontalBackwards,
    MultipleShake, SideSteps, SetPos, RETURNTOPOS, BuffUp, FadeInOut
}

public class AnimationController : MonoBehaviour
{
    Vector3 originalPos;
    [SerializeField] private List<Gradient> gradients = new List<Gradient>();
    [SerializeField] private List<Material> materials = new List<Material>();
    [SerializeField] private List<GameObject> animationPrefabs = new List<GameObject>();

    public Vector3 OriginalPos { get => originalPos; set => originalPos = value; }

    private float ReturnDirection(Image img)
    {
        if (img == BattleController.Instance.PlayerCreatureImage)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    private float ReturnDirection(Transform img)
    {
        if (img == BattleController.Instance.PlayerCreatureImage.transform)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    public IEnumerator MoveHorizontal(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 tOld = t.position;
        Vector3 newPos = t.position + ((Vector3.right * 50) * ReturnDirection(img));
        t.DOMove(newPos, duration, false);
        yield return new WaitForSeconds(duration);
        t.DOMove(tOld, duration / 2, false);
        yield return new WaitForSeconds(duration / 2);
    }
    public IEnumerator MoveHorizontalAndRotationToRight(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 tOld = t.position;
        Vector3 newPos = t.position + ((Vector3.right * 20) * ReturnDirection(img)) + (Vector3.up * 10);
        t.DOMove(newPos, duration, false);
        t.DORotate(new Vector3(0, 0, 5), 0.25f, RotateMode.Fast);
        yield return new WaitForSeconds(duration);
        t.DORotate(new Vector3(0, 0, 0), 0.1f, RotateMode.Fast);
        t.DOMove(tOld, duration / 2, false);
        yield return new WaitForSeconds(duration / 2);
    }
    public IEnumerator MoveHorizontalBackwards(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 tOld = t.position;
        Vector3 newPos = t.position - ((Vector3.right * 50) * ReturnDirection(img));
        t.DOMove(newPos, duration * 0.66f, false);
        yield return new WaitForSeconds(duration * 0.66f);
        t.DOMove(tOld + (Vector3.right * 25), duration * 0.33f, false);
        yield return new WaitForSeconds(duration * 0.33f);
        t.DOMove(tOld, duration * 0.33f, false);
    }
    public IEnumerator MoveVertical(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 tOld = t.position;
        Vector3 newPos = t.position + ((Vector3.up * 15) * ReturnDirection(img));
        t.DOMove(newPos, duration, false);
        yield return new WaitForSeconds(duration);
        t.DOMove(tOld, duration / 2, false);
        yield return new WaitForSeconds(duration / 2);
    }
    public IEnumerator MoveVerticalBounce(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator Swirl(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator Circle(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator RandomPositions(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator TakeDamage(Transform t, Image img, float duration, float delay)
    {
        PlayerStatsUI statsUI = new PlayerStatsUI();
        if (img == BattleController.Instance.PlayerCreatureImage)
        {
            statsUI = CoreUI.Instance.PlayerStats[0];
        }
        else
        {
            statsUI = CoreUI.Instance.PlayerStats[1];
        }

        CanvasGroup canvasGroup = statsUI.GetComponent<CanvasGroup>();
        yield return new WaitForSeconds(delay);
        t.DOShakeScale(duration, 0.2f, 5, 45, true);
        img.DOFade(0.25f, 0.15f);
        canvasGroup.DOFade(0.25f, 0.15f);
        statsUI.GetComponent<RectTransform>().DOShakePosition(0.45f, 10, 50, 45, true, true);
        yield return new WaitForSeconds(0.1f);
        img.DOFade(1, 0.15f);
        canvasGroup.DOFade(1, 0.15f);
        yield return new WaitForSeconds(0.1f);
        img.DOFade(0.25f, 0.15f);
        canvasGroup.DOFade(0.25f, 0.15f);
        yield return new WaitForSeconds(0.1f);
        img.DOFade(1, 0.15f);
        canvasGroup.DOFade(1, 0.15f);
        yield return new WaitForSeconds(duration);

    }
    public IEnumerator TakeDamageShakeNoFade(Transform t, Image img, float duration, float delay)
    {
        Vector3 tOld = t.position;
        yield return new WaitForSeconds(delay);
        t.DOShakeScale(duration, 0.2f, 5, 45, true);
        yield return new WaitForSeconds(duration / 2);
       
    }
    public IEnumerator Glimmer(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator Burn(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlaySFX(BattleAudio.Instance.Burnt);
        Material mat = new Material(materials[1]);
        img.material = mat;
        t.DOShakeScale(duration, 0.1f, 3, 30, true);
        yield return new WaitForSeconds(duration);
        img.material = null;
    }
    public IEnumerator Frozen(Transform t1, Transform t2, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject go = Instantiate(animationPrefabs[0], ReturnSpawnedAnimationPosition(AnimationLocation.Self, t1, t2), Quaternion.identity, CoreUI.Instance.BattleCanvasTransform);
        AudioManager.Instance.PlaySFX(BattleAudio.Instance.Frozen);
        Material mat = new Material(materials[3]);
        img.material = mat;
        yield return new WaitForSeconds(duration);
        img.material = null;
    }
    public IEnumerator Poison(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlaySFX(BattleAudio.Instance.Poison, 1, false);
        Material mat = new Material(materials[0]);
        img.material = mat;
        t.DOShakeScale(duration, 0.2f, 5, 45, true);
        yield return new WaitForSeconds(duration);
        img.material = null;
    }
    public IEnumerator Sleep(Transform t1, Transform t2, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject go = Instantiate(animationPrefabs[1], ReturnSpawnedAnimationPosition(AnimationLocation.Self, t1, t2), Quaternion.identity, CoreUI.Instance.BattleCanvasTransform);
        //AudioManager.Instance.PlayUISFX()
        yield return new WaitForSeconds(duration);
        Destroy(go);
    }
    public IEnumerator Bleeding(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator Confused(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator Shocked(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Material mat = new Material(materials[2]);
        img.material = mat;
        t.DOShakeScale(duration, 0.1f, 3, 30, true);
        yield return new WaitForSeconds(duration);
        img.material = null;
    }
    public IEnumerator Rampage(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator Ethereal(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator SpawnAnimSprite(Transform t1, Transform t2, Image img, float duration, float delay, AnimationDetail ad, bool SkipCoroutineWait)
    {
        yield return new WaitForSeconds(delay);
        Camera cam = Camera.main;
        {

            if (ad.animSprite != null)
            {
                GameObject go = Instantiate(ad.animSprite, ReturnSpawnedAnimationPosition(ad, t1, t2), Quaternion.identity, CoreUI.Instance.BattleCanvasTransform);
                go.GetComponent<RectTransform>().localPosition += new Vector3(0,go.GetComponent<RectTransform>().sizeDelta.y / 4, 0);
                Vector3 scaleOffset = go.transform.localScale;

                if (img == BattleController.Instance.PlayerCreatureImage)
                {
                    scaleOffset = new Vector3(1.2f * go.transform.localScale.x, 1.2f * go.transform.localScale.y, 1.2f);
                }

                go.transform.localScale = new Vector3(ReturnDirection(t2) * scaleOffset.x, scaleOffset.y, 1);

                if (!SkipCoroutineWait)
                {
                    yield return new WaitForSeconds(go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                }
            }
        }
    }
    public IEnumerator SpawnAnimSpriteAsBackground(Transform t1, Transform t2, Image img, float duration, float delay, AnimationDetail ad, bool SkipCoroutineWait)
    {
        yield return new WaitForSeconds(delay);
        Camera cam = Camera.main;
        {
            Debug.Log("AnimSpawned");
            if (ad.animSprite != null)
            {
                GameObject go = Instantiate(ad.animSprite, ReturnSpawnedAnimationPosition(ad, t1, t2), Quaternion.identity, CoreUI.Instance.BattleCanvasTransform);
                Vector3 scaleOffset = Vector3.one;

                if (img == BattleController.Instance.PlayerCreatureImage)
                {
                    scaleOffset = new Vector3(1.2f, 1.2f, 1.2f);
                }

                go.transform.localScale = new Vector3(ReturnDirection(t2) * scaleOffset.x, scaleOffset.y, 1);

                if (!SkipCoroutineWait)
                {

                    yield return new WaitForSeconds(go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                }
            }
        }
    }
    public IEnumerator RotationSideToSide(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 tOld = t.position;
        t.DORotate(new Vector3(0, 0, 10), duration / 8, RotateMode.Fast);
        t.DOMoveY(t.position.y - 10, duration / 8);
        t.DOMoveX(t.position.x + 10, duration / 8);
        yield return new WaitForSeconds(duration / 8);
        t.DORotate(new Vector3(0, 0, 0), duration / 8, RotateMode.Fast);
        t.DOMoveY(t.position.y + 10, duration / 8);
        t.DOMoveX(t.position.x - 10, duration / 8);
        yield return new WaitForSeconds(duration / 8);
        t.DORotate(new Vector3(0, 0, -10), duration / 8, RotateMode.Fast);
        t.DOMoveY(t.position.y - 10, duration / 8);
        t.DOMoveX(t.position.x - 10, duration / 8);
        yield return new WaitForSeconds(duration / 8);
        t.DORotate(new Vector3(0, 0, 0), duration / 8, RotateMode.Fast);
        t.DOMoveY(t.position.y + 10, duration / 8);
        t.DOMoveX(t.position.x + 10, duration / 8);
        yield return new WaitForSeconds(duration / 8);
        t.DORotate(new Vector3(0, 0, 10), duration / 4, RotateMode.Fast);
        t.DOMoveY(t.position.y - 10, duration / 8);
        t.DOMoveX(t.position.x + 10, duration / 8);
        yield return new WaitForSeconds(duration / 8);
        t.DORotate(new Vector3(0, 0, 0), duration / 8, RotateMode.Fast);
        t.DOMoveY(t.position.y + 10, duration / 8);
        t.DOMoveX(t.position.x - 10, duration / 8);
        yield return new WaitForSeconds(duration / 8);
        t.DORotate(new Vector3(0, 0, -10), duration / 8, RotateMode.Fast);
        t.DOMoveY(t.position.y - 10, duration / 8);
        t.DOMoveX(t.position.x - 10, duration / 8);
        yield return new WaitForSeconds(duration / 8);
        t.DORotate(new Vector3(0, 0, 0), duration / 4, RotateMode.Fast);
        t.DOMoveY(t.position.y + 10, duration / 8);
        t.DOMoveX(t.position.x + 10, duration / 8);
        yield return new WaitForSeconds(duration / 8);
    }
    public IEnumerator MultipleShake(Transform t, Image img, float duration, float delay)
    {

        yield return new WaitForSeconds(delay);
        t.DOShakeScale(duration, 0.3f, 15, 30, true);
        yield return new WaitForSeconds(duration);
    }
    public IEnumerator SideSteps(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 originalPos = t.position;
        t.position += new Vector3(ReturnDirection(img) * 25f, 0);
        yield return new WaitForSeconds(duration / 4);
        t.position -= new Vector3(ReturnDirection(img) * 50f, 0);
        yield return new WaitForSeconds(duration / 4);
        t.position += new Vector3(ReturnDirection(img) * 50f, 0);
        yield return new WaitForSeconds(duration / 4);
        t.position -= new Vector3(ReturnDirection(img) * 50f, 0);
        yield return new WaitForSeconds(duration / 4);
        t.position = originalPos;
    }
    public IEnumerator CaptureOriginalPos(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        originalPos = t.position;
        yield return null;
    }

    public IEnumerator ResetToDefaultPos(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        t.DOMove(originalPos, duration);
        yield return new WaitForSeconds(duration);
    }

    public IEnumerator BuffUp(Transform t, Image img, float duration, float delay) {

        yield return new WaitForSeconds(delay);
        t.DOScale(0.90f, duration / 6);
        yield return new WaitForSeconds(duration / 6);
        t.DOScale(1.33f, duration / 3);
        yield return new WaitForSeconds(duration / 3);
        t.DOScale(1f, duration / 6);
        yield return new WaitForSeconds(duration / 6);
    }
    public IEnumerator FadeInOut(Transform t, Image img, float duration, float delay) {
       
        yield return new WaitForSeconds(delay);
        img.DOFade(0.25f, 0.15f);
        yield return new WaitForSeconds(0.1f);
        img.DOFade(1, 0.15f);
        yield return new WaitForSeconds(0.1f);
        img.DOFade(0.25f, 0.15f);
        yield return new WaitForSeconds(0.1f);
        img.DOFade(1, 0.15f);
        yield return new WaitForSeconds(duration);
    }
    private Vector3 ReturnSpawnedAnimationPosition(AnimationDetail ad, Transform t1, Transform t2)
    {
        switch (ad.animationLocation)
        {
            case AnimationLocation.Center:
                return new Vector3(Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 1.75f, 0);
            case AnimationLocation.Target:
                return t2.transform.position;
            case AnimationLocation.Self:
                return t1.transform.position;
        }
        return new Vector3(Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 1.75f, 0);
    }
    private Vector3 ReturnSpawnedAnimationPosition(AnimationLocation animationLocation, Transform t1, Transform t2)
    {
        switch (animationLocation)
        {
            case AnimationLocation.Center:
                return new Vector3(Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 1.75f, 0);
            case AnimationLocation.Target:
                return t2.transform.position;
            case AnimationLocation.Self:
                return t1.transform.position;
        }
        return new Vector3(Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 1.75f, 0);
    }
}
