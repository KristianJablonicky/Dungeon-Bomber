using UnityEngine;

public class HideOnSpecificPlatform : MonoBehaviour
{
    [SerializeField] private DeviceType platform;
    private void Start()
    {
        if (SystemInfo.deviceType == platform)
        {
            Destroy(gameObject);
        }
    }
}