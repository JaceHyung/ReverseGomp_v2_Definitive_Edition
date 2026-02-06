using UnityEngine;
using Mirror;
using TMPro;
using Mirror.Examples.Pong;
using System;

public class MatchController : NetworkBehaviour
{
    public static event Action OnGameStarted;
    public static event Action OnGameEnded;
    public static MatchController Instance {get; private set;} 

    [Header("Ball")]
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Transform _ballSpawnPosition;

    [Header("Rackets")]
    [SerializeField] private GameObject _racketPrefab;
    [SerializeField] private Transform _racket1Position;
    [SerializeField] private Transform _racket2Position;

    [Header("Finish screen")]
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private TextMeshProUGUI _endScreenTMP;
    [SerializeField] private string _winnerPrefix = "";
    [SerializeField] private string _winnerSuffix = "WON!";

    private bool _isPlaying = false;
    private Ball _instantiatedBall;
    private GameObject _racket1;
    private GameObject _racket2;
    private Player _player1;
    private Player _player2;

    private void Awake()
    {
        Instance = this;
    }

    [Server]
    public void StartGame(Player player1, Player player2)
    {
        if(_isPlaying) return;

        _endScreen.SetActive(false);

        _player1 = player1;
        _player2 = player2;

        player1.SetReady(false);
        player2.SetReady(false);

        _isPlaying = true;

        _instantiatedBall = Instantiate(_ballPrefab, _ballSpawnPosition.position, Quaternion.identity);
        NetworkServer.Spawn(_instantiatedBall.gameObject);

        _racket1 = Instantiate(_racketPrefab, _racket1Position.position, Quaternion.identity);
        NetworkServer.Spawn(_racket1, player1.gameObject);

        _racket2 = Instantiate(_racketPrefab, _racket2Position.position, Quaternion.identity);
        NetworkServer.Spawn(_racket2, player2.gameObject);

        RpcAssignRacket(player1, _racket1);
        RpcAssignRacket(player2, _racket2);

        OnGameStarted?.Invoke();
    }

    [Server]
    public void EndGame(Player looser = null)
    {
        if(!_isPlaying) return;
        _isPlaying = false;
        
        NetworkServer.Destroy(_racket1);
        NetworkServer.Destroy(_racket2);
        NetworkServer.Destroy(_instantiatedBall.gameObject);
        
        if(looser != null)
        {
            var winner = looser == _player1 ? _player2 : _player1;
            RpcShowEndScreen(winner);
        }
        else
        {
            Debug.Log("Test if player 2 name is available");
        }

        OnGameEnded?.Invoke();
    }

    [ClientRpc]
    private void RpcAssignRacket(Player player, GameObject racket)
    {
        player.AssignRacket(racket);
    }

    [ClientRpc]
    private void RpcShowEndScreen(Player winner)
    {
        _endScreen.SetActive(true);
        _endScreenTMP.text = $"{_winnerPrefix} {winner.playerName} {_winnerSuffix}";
    }
}
