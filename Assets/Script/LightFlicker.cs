using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public Light targetLight; // Assign the Light component in the Inspector
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f; // How often the intensity changes

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
            if (targetLight == null)
            {
                Debug.LogError("No Light component found on this GameObject or assigned to targetLight.");
                enabled = false; // Disable the script if no light is found
                return;
            }
        }
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // Randomly set the light's intensity within the defined range
            targetLight.intensity = Random.Range(minIntensity, maxIntensity);

            // Wait for a short, random duration before changing intensity again
            yield return new WaitForSeconds(Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f));
        }
    }
}