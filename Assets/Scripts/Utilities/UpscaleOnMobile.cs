using UnityEngine;

public class UpscaleOnMobile : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 1.5f;
    private void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            transform.localScale *= scaleMultiplier;
        }
    }
}
