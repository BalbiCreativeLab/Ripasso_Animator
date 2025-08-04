using System;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public Action<string> StringAction;
    public Action Action;

    public Action LoadGame, SaveGame;

    public Action StartGame, EndGame, Respawn, Team1Win, Team2Win;

    public Action<Team> Win;

    private void Start()
    {
        LoadGame?.Invoke();
        StringAction += Funzione;
        StringAction.Invoke("CIAO!");

    }

    void Funzione(string stringa)
    {
        print(stringa);
    }
}


public class Team
{
    public int id = 1;
}