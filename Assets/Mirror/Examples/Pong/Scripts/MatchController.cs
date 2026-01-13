using UnityEngine;
using Mirror;
using TMPro;

public class MatchController : NetworkBehaviour
{
    public static MatchController instance;

    [Header("End Game UI")]
    public GameObject endGameCanvas;
    public TextMeshProUGUI winnerText;

    private int rematchVotes = 0;
    private int playersAlive = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // UI ma byæ ukryte na starcie gry (na ka¿dym kliencie)
        if (endGameCanvas != null)
            endGameCanvas.SetActive(false);
    }

    // =========================
    // KONIEC GRY (SERWER)
    // =========================
    [Server]
    public void PlayerDied(GameObject deadPlayer)
    {
        playersAlive--;

        if (playersAlive <= 1)
        {
            string winner = GetWinnerText(deadPlayer);
            RpcShowEndGame(winner);
            Time.timeScale = 0f;
        }
    }

    // =========================
    // UI KONIEC GRY (KLIENT)
    // =========================
    [ClientRpc]
    void RpcShowEndGame(string winner)
    {
        endGameCanvas.SetActive(true);
        winnerText.text = winner;
    }

    // =========================
    // USTALENIE ZWYCIÊZCY
    // =========================
    string GetWinnerText(GameObject deadPlayer)
    {
        if (deadPlayer.transform.position.x < 0)
            return "PLAYER 2 WINS!";
        else
            return "PLAYER 1 WINS!";
    }

    // =========================
    // WYJŒCIE Z GRY
    // =========================
    public void OnExitGameClicked()
    {
        Debug.Log("Wyjœcie z gry klikniête!");
        Application.Quit();
    }

    // =========================
    // REMATCH – REJESTRACJA G£OSU (SERWER)
    // =========================
    [Server]
    public void RegisterRematchVote()
    {
        rematchVotes++;

        if (rematchVotes >= NetworkServer.connections.Count)
        {
            rematchVotes = 0;
            Time.timeScale = 1f;
            NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
        }
    }

    // =========================
    // RESET MECZU
    // =========================
    [Server]
    public void ResetMatch(int players)
    {
        playersAlive = players;
        rematchVotes = 0;
    }

    [ClientRpc]
    void RpcHideEndGame()
    {
        endGameCanvas.SetActive(false);
    }
}
