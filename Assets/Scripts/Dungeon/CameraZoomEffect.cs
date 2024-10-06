using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomEffect : MonoBehaviour
{
    private Camera cam;
    private bool isZooming = false;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        Metronome.instance.onBeat += zooooom;
    }

    private void zooooom(object sender, System.EventArgs e)
    {
        TriggerZoom(3.9f, 0.05f);
    }

    public void TriggerZoom(float targetFOV, float duration)
    {
        if (!isZooming)
            StartCoroutine(ZoomCoroutine(targetFOV, duration));
    }

    private IEnumerator ZoomCoroutine(float targetFOV, float duration)
    {
        isZooming = true;
        float startFOV = cam.orthographicSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            cam.orthographicSize = Mathf.Lerp(startFOV, targetFOV, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Return to original FOV
        elapsed = 0f;
        while (elapsed < duration)
        {
            cam.orthographicSize = Mathf.Lerp(targetFOV, startFOV, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.fieldOfView = startFOV;
        isZooming = false;
    }

    void Update()
    {
        
    }
}
