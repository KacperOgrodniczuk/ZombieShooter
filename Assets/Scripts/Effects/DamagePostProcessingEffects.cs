using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamagePostProcessingEffects : MonoBehaviour
{
    public float flashIntensity = 0.3f;
    public float fadeSpeed = 2f;

    private Volume volume;
    private Vignette vignette;

    void Start()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
        }
    }

    void Update()
    {
        if (vignette != null && vignette.intensity.value > 0f)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, Time.deltaTime * fadeSpeed);
        }
    }

    public void Flash()
    {
        if (vignette != null)
        {
            vignette.intensity.value += flashIntensity;
        }
    }
}
