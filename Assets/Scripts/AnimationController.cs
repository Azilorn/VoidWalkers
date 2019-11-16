using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ImageAnimation
{
    None, MoveHorizontal, MoveHorizontalAndRotationToRight, MoveVertical, MoveVerticalBounce, SpawnAnimSprite, Circle, RandomPositions, TakeDamage,
    TakeDamageShakeNoFade, Glimmer, Burn, Poison, Frozen, Sleep, Confused, Shocked, Rampage, Swirl, Ethereal, RotationSideToSide, SpawnAnimSpriteAsBackground, MoveHorizontalBackwards,
    MultipleShake
}

public class AnimationController : MonoBehaviour
{

    //0 = Poison | 1 = Burnt | 2 = Shocked  | 3 = Frozen | 4 = Confused | 5 = Ethereal | 6 = Sleep | 7 = Attack Up

    [SerializeField] private List<Gradient> gradients = new List<Gradient>();
    [SerializeField] private List<Material> materials = new List<Material>();
    [SerializeField] private List<GameObject> animationPrefabs = new List<GameObject>();

    private float ReturnDirection(Image img)
    {
        if (img == BattleController.Instance.Player1CreatureImage)
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
        if (img == BattleController.Instance.Player1CreatureImage.transform)
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
        if (img == BattleController.Instance.Player1CreatureImage)
        {
            statsUI = BattleUI.Instance.PlayerStats[0];
        }
        else
        {
            statsUI = BattleUI.Instance.PlayerStats[1];
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
        t.DOMoveX(t.position.x + 5f, duration / 2, true);
        yield return new WaitForSeconds(duration / 2);
        t.DOMoveX(tOld.x, duration / 2, true);
        yield return new WaitForSeconds(duration / 2);
    }
    public IEnumerator Glimmer(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator Burn(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Material mat = new Material(materials[1]);
        img.material = mat;
        t.DOShakeScale(duration, 0.1f, 3, 30, true);
        yield return new WaitForSeconds(duration);
        img.material = null;
    }
    public IEnumerator Frozen(Transform t1, Transform t2, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject go = Instantiate(animationPrefabs[0], ReturnSpawnedAnimationPosition(AnimationLocation.Self, t1, t2), Quaternion.identity, BattleUI.Instance.BattleCanvasTransform);
        Material mat = new Material(materials[3]);
        img.material = mat;
        yield return new WaitForSeconds(duration);
        img.material = null;
    }
    public IEnumerator Poison(Transform t, Image img, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        Material mat = new Material(materials[0]);
        img.material = mat;
        t.DOShakeScale(duration, 0.2f, 5, 45, true);
        yield return new WaitForSeconds(duration);
        img.material = null;
    }
    public IEnumerator Sleep(Transform t, Image img, float duration, float delay)
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
            Debug.Log("AnimSpawned");
            if (ad.animSprite != null)
            {
                GameObject go = Instantiate(ad.animSprite, ReturnSpawnedAnimationPosition(ad, t1, t2), Quaternion.identity, BattleUI.Instance.BattleCanvasTransform);
                Vector3 scaleOffset = go.transform.localScale;

                if (img == BattleController.Instance.Player1CreatureImage)
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
                GameObject go = Instantiate(ad.animSprite, ReturnSpawnedAnimationPosition(ad, t1, t2), Quaternion.identity, BattleUI.Instance.BattleCanvasTransform);
                Vector3 scaleOffset = Vector3.one;

                if (img == BattleController.Instance.Player1CreatureImage)
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
    public IEnumerator MultipleShake(Transform t, Image img, float duration, float delay) {

        yield return new WaitForSeconds(delay);
        t.DOShakeScale(duration, 0.3f, 15, 30, true);
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
