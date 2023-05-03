using UnityEngine;
using System.Collections;

/* 
 * Usage: attach this script to a camera or any other object, call Shake() method to start shaking it
* To turn off, change influence to zero
* Attach the camera to an empty game object to keep its local position and rotation
*/
public class CameraShake : MonoBehaviour
{
    #region members
    [Range(0f, 1f), Tooltip("How much this camera will be influenced by shaking, 0 turns it off")]
    public float shakeInfluence = 0.5f;
    [Range(0f, 10f), Tooltip("How much rotation will be influenced by shaking, 0 turns it off")]
    public float rotationInfluence = 0f;
    [Range(1f, 2f), Tooltip("Will clamp any shaking to this value")]
    public float maxShakeMagnitude = 1f;

    private Vector3 OriginalPos;
    private Quaternion OriginalRot;
    private bool isShakeRunning = false;
    #endregion

    /// <summary>
    /// Will shake the camera with intensity for duration
    /// </summary>
    /// <param name="minIntensity"></param>
    /// <param name="maxIntensity"></param>
    /// <param name="duration"></param>
    public void Shake(float intensity, float duration)
    {
        if (isShakeRunning)
            return;

        SaveOriginalValues();

        float shake = intensity * shakeInfluence;
        duration *= shakeInfluence;

        StartCoroutine(ProcessShake(shake, duration));
    }

    /// <summary>
    /// Will shake the camera with a random value between minIntensity and maxIntensity for duration
    /// </summary>
    /// <param name="minIntensity"></param>
    /// <param name="maxIntensity"></param>
    /// <param name="duration"></param>
    public void Shake(float minIntensity, float maxIntensity, float duration)
    {
        if (isShakeRunning)
            return;

        SaveOriginalValues();

        float shake = Random.Range(minIntensity, maxIntensity) * shakeInfluence;
        duration *= shakeInfluence;

        StartCoroutine(ProcessShake(shake, duration));
    }

    void SaveOriginalValues()
    {
        OriginalPos = transform.position;
        OriginalRot = transform.rotation;
    }

    IEnumerator ProcessShake(float shake, float duration)
    {
        isShakeRunning = true;
        float countdown = duration;
        float initialShake = Mathf.Clamp(shake, 0f, maxShakeMagnitude);

        while (countdown > 0)
        {
            countdown -= Time.deltaTime;

            float lerpIntensity = countdown / duration;
            shake = Mathf.Lerp(0f, initialShake, lerpIntensity);

            transform.position = OriginalPos + Random.insideUnitSphere * shake;
            transform.rotation = Quaternion.Euler(OriginalRot.eulerAngles + Random.insideUnitSphere * shake * rotationInfluence);

            yield return null;
        }

        FinalizeShake();
    }

    void FinalizeShake()
    {
        transform.position = OriginalPos;
        transform.rotation = OriginalRot;
        isShakeRunning = false;
    }
}
