using UnityEngine;
using System.Collections;

public class UISetup : MonoBehaviour
{
    public static UISetup Instance;

    public int m_background_width;
    public int m_background_heght;

    void Awake()
    {
        Instance = this;
    }
}
