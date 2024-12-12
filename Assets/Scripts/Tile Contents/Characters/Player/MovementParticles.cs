using System.Collections;
using UnityEngine;

public class MovementParticles : MonoBehaviour
{
    [SerializeField] private GameObject highlight;

    private void Awake()
    {
        var progress = Metronome.instance.getBeatProgress();
        
        // was the player movement pretty off beat?
        if (progress > 0.25f && progress < 0.75f)
        {
            // disable highlight if so
            Destroy(highlight);
        }
        StartCoroutine(deleteTimer());
    }

    private IEnumerator deleteTimer()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
