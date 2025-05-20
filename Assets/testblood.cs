using UnityEngine;

public class testblood : MonoBehaviour
{
    [SerializeField] Material bloodMaterial;
    [SerializeField] Animator sinkAnimator;

    //string propertyRotation = "_rotationPower";
    string propertyTwirl = "_twirlpower";

    float startTwirlValue=0;
    public float targetTwirlValue = 13f;
    //public float targetRotationValue = .2f;
    float lerpDuration = 5f;
    float lerpTimer = 0f;
    bool isLerpingTwirl = false;

    float startRotationValue = 0;
    float starttwirlValue = 0f;
    void Awake()
    {
        //bloodMaterial.SetFloat(propertyRotation, startRotationValue);
        bloodMaterial.SetFloat(propertyTwirl, starttwirlValue);

        sinkAnimator.enabled = false; // Desactiva Animator al inicio
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //bloodMaterial.SetFloat(propertyRotation, targetRotationValue);
            sinkAnimator.enabled = true;

            // Inicializar lerp para twirlPower
            startTwirlValue = bloodMaterial.GetFloat(propertyTwirl);
            lerpTimer = 0f;
            isLerpingTwirl = true;
        }

        if (isLerpingTwirl)
        {
            lerpTimer += Time.deltaTime;
            float t = Mathf.Clamp01(lerpTimer / lerpDuration);
            float newTwirlValue = Mathf.Lerp(startTwirlValue, targetTwirlValue, t);
            bloodMaterial.SetFloat(propertyTwirl, newTwirlValue);

            if (t >= 1f)
            {
                isLerpingTwirl = false;
            }
        }
    }
}
