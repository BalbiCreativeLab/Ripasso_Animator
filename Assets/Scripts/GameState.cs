using System;
using UnityEngine;

// Questa classe la useremo per salvare e accedere allo stato del nostro gioco
// come ad esempio i collezionabili raccolti, il punteggio ecc
public static class GameState
{
    // questa variabile essendo static sara' accessibile direttamente dal tipo di classe GameState,
    // senza dover creare una sua instanza nel gioco
    public static int collectibles = 0;

    // evento usato per comunicare quando un collezionabile viene raccolto
    public static Action<int> OnCollectiblePick;

    // funzione statica utilizzabile direttamente con GameState.GetCollectible()
    //usata quando il giocatore raccoglie un collezionabile
    public static void GetCollectible()
    {
        collectibles++;
        OnCollectiblePick?.Invoke(collectibles);

        Vector3 vectorVar = Vector3.up;
    }
}