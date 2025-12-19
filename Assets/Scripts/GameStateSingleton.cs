using System;
using UnityEngine;

public class GameStateSingleton : MonoBehaviour
{
    // Dentro GameStateSingleton e' salvata l'istanza di se stesso come static, cosi' da essere
    // accessibile ovunque
    public static GameStateSingleton Current;

    public int score;
    public Action<int> OnScoreAdd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Se current non e' vuoto, allora esiste gia' un GameStateSingleton in scena, quindi non puo'
        // essercene un'altro e mi autodistruggo
        // Altrimenti riempio Current con me stesso
        if (Current == null)
            Current = this;
        else
            Destroy(this);
    }

    public void AddScore(int _score)
    {
        score += _score;
        OnScoreAdd?.Invoke(score);
    }
}