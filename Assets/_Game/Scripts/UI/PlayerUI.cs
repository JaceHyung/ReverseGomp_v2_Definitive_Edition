using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private TextMeshProUGUI _playerNameTMP;
    [SerializeField] private TextMeshProUGUI _playerLifesTMP;
    [SerializeField] private TextMeshProUGUI _playerReadyTMP;
    [SerializeField] private string _playerReadyText = "Ready";
    [SerializeField] private string _playerNotReadyText = "Not Ready";

    void Awake()
    {
        Player.OnPlayerDataChange += OnPlayerDataChange;
    }

    void OnDestroy()
    {
        Player.OnPlayerDataChange -= OnPlayerDataChange;
    }

    private void OnPlayerDataChange(Player player)
    {
        if(player.playerId != _id) return;
        _playerNameTMP.text = player.playerName;
        _playerLifesTMP.text = $"{player.health} HP";
        _playerReadyTMP.text = player.isReady ? _playerReadyText : _playerNotReadyText;
    }
}
