using UnityEngine;
using System.Collections;

public enum PlayState
{
    NONE = -1,
    READY = 0,
    PLAY,
    PAUSE,
    ENDGAME
}

public class GameState : MonoBehaviour
{
    public static PlayState m_state = PlayState.NONE;
    public static int m_playCount = 0;

    void Start()
    {
        Ready();
    }

    public void Pause()
    {
        m_state = PlayState.PAUSE;
    }

    public void Play()
    {
        m_state = PlayState.PLAY;
        m_playCount++;
    }

    public void EndGame()
    {
        m_state = PlayState.ENDGAME;
    }

    public void Ready()
    {
        m_state = PlayState.READY;
    }

    public void SetPause()
    {
        if (m_state == PlayState.PAUSE)
        {
            Play();
        }
        else if (m_state == PlayState.PLAY)
        {
            Pause();
        }
    }
}
