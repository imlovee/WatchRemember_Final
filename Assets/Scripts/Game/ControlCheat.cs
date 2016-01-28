using UnityEngine;
using System.Collections;

public class ControlCheat : MonoBehaviour
{
    public static ControlCheat Instance;

    public bool isUse = false;
    public static bool OnCheat = false;
    public int m_clickCount1 = 0;
    public int m_clickCount2 = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OnCheat = isUse;
    }

    public void Click1()
    {
        m_clickCount1++;
        CheckCheat();
    }

    public void Click2()
    {
        m_clickCount2++;
        CheckCheat();
    }

    private void CheckCheat()
    {
        if (!Debug.isDebugBuild) return;

        if ((m_clickCount1 == 5 && m_clickCount2 == 5) || Input.GetKeyDown(KeyCode.F10))
        {
            isUse = true;
            OnCheat = true;
            Debug.Log("Cheat On");

			GameCash.CashPoint = 99999;
            SoundFX.Instance.PlaySound(SoundType.START_DELAY);
        }
    }
}
