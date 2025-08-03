using UnityEngine;
using UnityEngine.UI;

public class UICrosshair : MonoBehaviour
{
    [Range(0, 1)] public float minAlpha;
    [Range(0, 1)] public float maxAlpha;
    [Range(0, 1)] public float alphaWeight;
    public float transitionSpeed = 20f;

    Image crosshairImage;
    Color currentColour;


    private void OnEnable()
    {
        crosshairImage = GetComponent<Image>();
    }

    void Update()
    {
        float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, alphaWeight);
        currentColour = crosshairImage.color;
        currentColour.a = Mathf.Lerp(currentColour.a, newAlpha, transitionSpeed);
        crosshairImage.color = currentColour;
    }
}
