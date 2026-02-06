using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Logic components")]
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private GompNetworkDiscovery _networkDiscovery;
    [SerializeField] private LobbySearchUI _lobbySearchUI;

    [Header("UI screens")]
    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private GameObject _creditsScreen;
    [SerializeField] private GameObject _playScreen;
    [SerializeField] private GameObject _lobbySearchScreen;

    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _quitButton;
    [Space(10)]
    [SerializeField] private Button _returnFromCreditsScreenButton;
    [SerializeField] private Button _returnFromPlayScreenButton;
    [SerializeField] private Button _returnFromLobbySearchScreenButton;
    [Space(10)]
    [SerializeField] private Button _hostGameButton;
    [SerializeField] private Button _joinGameButton;

    private void Awake()
    {
        _startButton.onClick.AddListener(StartButton_OnClick);
        _creditsButton.onClick.AddListener(CreditsButton_OnClick);
        _quitButton.onClick.AddListener(QuitButton_OnClick);

        _returnFromCreditsScreenButton.onClick.AddListener(BackToMenu);
        _returnFromPlayScreenButton.onClick.AddListener(BackToMenu);
        _returnFromLobbySearchScreenButton.onClick.AddListener(BackToMenu);

        _hostGameButton.onClick.AddListener(HostGameButton_OnClick);
        _joinGameButton.onClick.AddListener(JoinGameButton_OnClick);

        _mainMenuScreen.SetActive(true);
        _playScreen.SetActive(false);
        _creditsScreen.SetActive(false);
        _lobbySearchScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveListener(StartButton_OnClick);
        _creditsButton.onClick.RemoveListener(CreditsButton_OnClick);
        _quitButton.onClick.RemoveListener(QuitButton_OnClick);

        _returnFromCreditsScreenButton.onClick.RemoveListener(BackToMenu);
        _returnFromPlayScreenButton.onClick.RemoveListener(BackToMenu);
        _returnFromLobbySearchScreenButton.onClick.RemoveListener(BackToMenu);

        _hostGameButton.onClick.RemoveListener(HostGameButton_OnClick);
        _joinGameButton.onClick.RemoveListener(JoinGameButton_OnClick);
    }

    private void StartButton_OnClick()
    {
        _mainMenuScreen.SetActive(false);
        _playScreen.SetActive(true);
        _creditsScreen.SetActive(false);
        _lobbySearchScreen.SetActive(false);
    }

    private void CreditsButton_OnClick()
    {
        _mainMenuScreen.SetActive(false);
        _playScreen.SetActive(false);
        _creditsScreen.SetActive(true);
        _lobbySearchScreen.SetActive(false);
    }

    private void BackToMenu()
    {
        _mainMenuScreen.SetActive(true);
        _playScreen.SetActive(false);
        _creditsScreen.SetActive(false);
        _lobbySearchScreen.SetActive(false);

        _lobbySearchUI.StopDiscovery();
    }

    private void QuitButton_OnClick()
    {
        Application.Quit();
    }

    private void HostGameButton_OnClick()
    {
        _networkManager.StartHost();
        _networkDiscovery.AdvertiseServer();
    }

    private void JoinGameButton_OnClick()
    {
        _mainMenuScreen.SetActive(false);
        _playScreen.SetActive(false);
        _creditsScreen.SetActive(false);
        _lobbySearchScreen.SetActive(true);

        _lobbySearchUI.StartDiscovery();
    }
}
