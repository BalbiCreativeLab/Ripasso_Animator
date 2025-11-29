using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ExperienceSettings : NetworkBehaviour
{
    public TMP_Text m_Text;
    public int number = 0;
    NetworkVariable<int> netNumber;

    private void Awake()
    {
        netNumber = new NetworkVariable<int>(number);
        netNumber.OnValueChanged += Upd;
    }

    private void Upd(int previousValue, int newValue)
    {
        m_Text.text = netNumber.Value.ToString();
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("NetworkBehaviour Spawned");
        if (IsServer)
        {
            UpdateNumberRpc();
        }
    }

    [Rpc(SendTo.Server)]
    void UpdateNumberRpc()
    {
        Debug.Log("Updating Number on Server");
        netNumber.Value = number;
        DebugNumberRpc();
    }   

    [Rpc(SendTo.ClientsAndHost)]
    void DebugNumberRpc()
    {
        Debug.Log($"Value Updated: #{netNumber.Value} on Client #{OwnerClientId}");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            if(netNumber.Value != number)
            {
                UpdateNumberRpc();
            }
        }
    }
}
