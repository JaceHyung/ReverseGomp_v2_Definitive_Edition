using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUI : MonoBehaviour
{
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _readyButton.onClick.AddListener(OnReady);
        _exitButton.onClick.AddListener(OnExit);

        MatchController.OnGameStarted += OnGameStarted;
        MatchController.OnGameEnded += OnGameEnded;
    }

    private void OnDestroy()
    {
        _readyButton.onClick.RemoveListener(OnReady);
        _exitButton.onClick.RemoveListener(OnExit);

        MatchController.OnGameStarted -= OnGameStarted;
        MatchController.OnGameEnded -= OnGameEnded;
    }

    private void OnReady()
    {
        Player.local.ToggleReady();
    }

    private void OnExit()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }
    }

    private void OnGameStarted()
    {
        gameObject.SetActive(false);
    }
    
    private void OnGameEnded()
    {
        gameObject.SetActive(true);
    }

}
