using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assets : MonoBehaviour
{
    private static GameObject instance;
    public static GameObject i
    {
        get
        {
            if (instance == null)
            {
                instance = (Instantiate(Resources.Load("Assets")) as GameObject);
            }
            return instance;
        }
    }
}
