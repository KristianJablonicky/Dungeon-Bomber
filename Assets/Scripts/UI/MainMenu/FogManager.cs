using System.Buffers.Text;
using UnityEngine;
using UnityEngine.UI;

public class FogManager : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private float speedX;

    private void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(speedX, 0) * Time.deltaTime, rawImage.uvRect.size);
    }
}
