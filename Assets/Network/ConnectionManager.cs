using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{

    public void SetupIp(string ip)
    {
        if(NetworkManager.Singleton.gameObject.TryGetComponent(out UnityTransport tr))
        {
            tr.SetConnectionData(ip, tr.ConnectionData.Port);
        }
    }
    public void SetupPort(string port)
    {
        if (NetworkManager.Singleton.gameObject.TryGetComponent(out UnityTransport tr))
        {
            tr.SetConnectionData(tr.ConnectionData.Address, ushort.Parse(port));
        }
    }

    public void StartServer()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.StartServer();
            LoadMainLevel();
        }
    }
    public void StartHost()
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.StartHost();
            LoadMainLevel();
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.StartClient();
            LoadMainLevel();
        }
    }

    void LoadMainLevel()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
        //SceneManager.LoadScene("PlayScene");
    }
}
