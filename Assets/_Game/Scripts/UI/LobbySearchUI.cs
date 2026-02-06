using System;
using System.Collections.Generic;
using Mirror;
using Mirror.BouncyCastle.Bcpg.Sig;
using Mirror.Discovery;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbySearchUI : MonoBehaviour
{

    [SerializeField] private GompNetworkDiscovery _networkDiscovery;
    [SerializeField] private GameObject _serverButtonPrefab;
    [SerializeField] private Transform _serverButtonContainer;

    private readonly Dictionary<long, DiscoveryResponse> _discoveredServers = new();
    private readonly Dictionary<long, GameObject> _instantiatedButtons = new();

    private void Awake()
    {
        _networkDiscovery.OnServerFound.AddListener(NetworkDiscovery_OnServerFound);
    }

    private void OnDestroy()
    {
        _networkDiscovery.OnServerFound.RemoveListener(NetworkDiscovery_OnServerFound);
    }

    public void StartDiscovery()
    {
        _networkDiscovery.StartDiscovery();
    }

    public void StopDiscovery()
    {
        _networkDiscovery.StopDiscovery();
        DestroyAllButtons();
    }

    private void NetworkDiscovery_OnServerFound(DiscoveryResponse info)
    {
        if (_discoveredServers.ContainsKey(info.serverId))
        {
            if (info.isFull)
            {
                _instantiatedButtons[info.serverId].GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(_instantiatedButtons[info.serverId]);
                _discoveredServers.Remove(info.serverId);
                _instantiatedButtons.Remove(info.serverId);
            }
            return;
        }
        if (info.isFull) return;

        _discoveredServers[info.serverId] = info;
        SpawnButton(info);
    }

    private void SpawnButton(DiscoveryResponse info)
    {
        var go = Instantiate(_serverButtonPrefab, _serverButtonContainer);
        _instantiatedButtons.Add(info.serverId, go);
        go.GetComponentInChildren<TextMeshProUGUI>().text = info.hostName;
        go.GetComponent<Button>().onClick.AddListener(() => ServerButton_OnClick(info));
    }

    private void DestroyAllButtons()
    {
        foreach(var button in _instantiatedButtons.Values)
        {
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(button);
        }
        _discoveredServers.Clear();
    }

    private void ServerButton_OnClick(DiscoveryResponse info)
    {
        Connect(info);
    }

    private void Connect(DiscoveryResponse info)
    {
        StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }
}
