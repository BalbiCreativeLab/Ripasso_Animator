using UnityEngine;

public class SavePosition : MonoBehaviour
{
    public SaveData saveData;
    public EventSystem eventSystem;

    void Awake()
    {
        eventSystem.LoadGame += OnLoadGame;
        eventSystem.SaveGame += OnSaveGame;
    }

    void OnLoadGame()
    {
        transform.position = saveData.position;
        transform.rotation = saveData.rotation;
    }

    void OnSaveGame()
    {
        saveData.position = transform.position;
        saveData.rotation = transform.rotation;
        Debug.Log("saving position of: " + gameObject.name);
    }
}
