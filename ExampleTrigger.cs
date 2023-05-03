using UnityEngine;

/* This script will attempt to trigger a camera shake on the CameraShaker.cs class
* It finds the main camera in the scene and calls the Shake() method on it when you press the 'W' key
* It is based on proximity, the closer it is to the camera the more it will shake
* Not intended to be used as is, but requires modification from you the user, this is just an example
*/

public class ExampleTrigger : MonoBehaviour
{
    #region members
    ParticleSystem particles;
    CameraShake camera;

    [SerializeField]
    float maxdistance = 10f;
    [SerializeField]
    float minDistance = 1.0f;
    [SerializeField]
    float maxstrength = 5f;
    #endregion

    private void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        particles = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            float distance = GetDistanceToExplosion();
            float strength = CalculateMagnitude(distance);

            particles.Play();
            camera.Shake(strength, 1f);
        }
    }
    
    float CalculateMagnitude(float dist)
    {
        dist = Mathf.Clamp(dist, 0f, maxdistance);
        float lerp = Mathf.InverseLerp(maxdistance, minDistance, dist);
        float finalMagnitude = Mathf.Lerp(0, maxstrength, lerp);
        return finalMagnitude;
    }
    
    float GetDistanceToExplosion()
    {
        return Vector3.Distance(transform.position, camera.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxdistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistance);
    }
}
