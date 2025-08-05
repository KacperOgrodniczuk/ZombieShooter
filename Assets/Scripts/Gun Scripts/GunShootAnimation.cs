using System.Collections;
using UnityEngine;

public class GunShootAnimation : MonoBehaviour
{
    public Transform slidePart;
    public Vector3 slideOffset = new Vector3(0, 0, -0.05f);
    public float slideDuration = 0.1f;

    public AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 originalLocalPos;
    private Coroutine currentCoroutine;

    void Awake()
    {
        if (slidePart != null)
            originalLocalPos = slidePart.localPosition;
    }

    public void PlaySlide()
    {
        if (slidePart == null) return;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(SlideCoroutine());
    }

    private IEnumerator SlideCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            float t = elapsed / slideDuration;
            float curveValue = slideCurve.Evaluate(t);
            slidePart.localPosition = Vector3.LerpUnclamped(originalLocalPos, originalLocalPos + slideOffset, curveValue);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset to original to avoid small drift
        slidePart.localPosition = originalLocalPos;
        currentCoroutine = null;
    }
}
