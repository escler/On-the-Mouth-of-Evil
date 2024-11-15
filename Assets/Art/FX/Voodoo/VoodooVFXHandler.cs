using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VoodooVFXHandler : MonoBehaviour
{
    public VisualEffect vfx;

    public string sizePropertyName = "Size";
    public string sizeSealPropertyName = "SizeSeal";
    public string topHeightPropertyName = "TopHeight";

    public Transform object1;
    public Transform object2;
    public Transform object3;

    public float initialSize;
    public float targetSize;

    public float initialSizeSeal;
    public float targetSizeSeal;

    public float initialTopHeight;
    public float targetTopHeight;

    private Vector3 initialScale = new Vector3(0.3f, 0, 0.3f);
    private Vector3 targetScale = new Vector3(0.5f, 0.4f, 0.5f);

    public float lerpDuration;

    private bool isLerpingSizeSeal = false;
    private bool isLerpingSizeAndHeight = false;
    private bool isOpening = true;
    private float lerpTime = 0f;

    private bool closed;

    void Start()
    {
        ResetToInitialValues();
    }

    public void ClosePrison()
    {
        isLerpingSizeSeal = false;
        isLerpingSizeAndHeight = true;
        isOpening = false;
        lerpTime = 0f;
    }

    public void OpenPrison()
    {
        ResetToInitialValues();
        closed = false;
        isLerpingSizeSeal = true;
        isLerpingSizeAndHeight = false;
        isOpening = true;
        lerpTime = 0f;
    }

    void Update()
    {
        if (closed) return;

        if (isLerpingSizeSeal && isOpening)
        {
            PerformLerp(sizeSealPropertyName, initialSizeSeal, targetSizeSeal, ref isLerpingSizeSeal, ref isLerpingSizeAndHeight, ref lerpTime, true);
        }

        if (isLerpingSizeAndHeight)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / lerpDuration;

            float lerpedSize = Mathf.Lerp(isOpening ? initialSize : targetSize, isOpening ? targetSize : initialSize, t);
            float lerpedTopHeight = Mathf.Lerp(isOpening ? initialTopHeight : targetTopHeight, isOpening ? targetTopHeight : initialTopHeight, t);

            SetFloatProperty(sizePropertyName, lerpedSize);
            SetFloatProperty(topHeightPropertyName, lerpedTopHeight);

            Vector3 lerpedScale = Vector3.Lerp(isOpening ? initialScale : targetScale, isOpening ? targetScale : initialScale, t);
            object1.localScale = lerpedScale;
            object2.localScale = lerpedScale;
            object3.localScale = lerpedScale;

            if (lerpTime > lerpDuration)
            {
                isLerpingSizeAndHeight = false;

                if (!isOpening)
                {
                    isLerpingSizeSeal = true;
                    lerpTime = 0f;
                }
            }
        }

        if (isLerpingSizeSeal && !isOpening)
        {
            PerformLerp(sizeSealPropertyName, targetSizeSeal, initialSizeSeal, ref isLerpingSizeSeal, ref closed, ref lerpTime, false);
        }
    }

    private void PerformLerp(string propertyName, float startValue, float endValue, ref bool lerpingFlag, ref bool nextFlag, ref float lerpTimer, bool resetLerpTimer)
    {
        lerpTimer += Time.deltaTime;
        float t = lerpTimer / lerpDuration;

        float lerpedValue = Mathf.Lerp(startValue, endValue, t);
        SetFloatProperty(propertyName, lerpedValue);

        if (lerpTimer > lerpDuration)
        {
            lerpingFlag = false;
            nextFlag = true;

            if (resetLerpTimer)
                lerpTimer = 0f;
        }
    }

    private void SetFloatProperty(string propertyName, float value)
    {
        if (vfx.HasFloat(propertyName))
        {
            vfx.SetFloat(propertyName, value);
        }
        else
        {
            Debug.LogWarning($"La propiedad {propertyName} no se encontró en el VFX.");
        }
    }

    private void ResetToInitialValues()
    {
        SetFloatProperty(sizePropertyName, initialSize);
        SetFloatProperty(sizeSealPropertyName, initialSizeSeal);
        SetFloatProperty(topHeightPropertyName, initialTopHeight);

        object1.localScale = initialScale;
        object2.localScale = initialScale;
        object3.localScale = initialScale;
    }
}
