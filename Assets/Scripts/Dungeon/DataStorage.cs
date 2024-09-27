using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static DataStorage instance = null;
    public int floor = 0;
    private void Awake()
    {
        // first awakening
        if (instance == null)
        {
            floor = 1;
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
