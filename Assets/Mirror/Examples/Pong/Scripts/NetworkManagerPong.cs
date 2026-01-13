using UnityEngine;
using Mirror;

namespace Mirror.Examples.Pong
{
    [AddComponentMenu("")]
    public class NetworkManagerPong : NetworkManager
    {
        public Transform leftRacketSpawn;
        public Transform rightRacketSpawn;

        private GameObject ball;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);

            // Spawn pi³ki, gdy s¹ dwaj gracze
            if (numPlayers == 2)
            {
                ball = Instantiate(spawnPrefabs.Find(p => p.name == "Ball"));
                NetworkServer.Spawn(ball);

                // Reset stanu meczu
                MatchController.instance.ResetMatch(numPlayers);
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (ball != null)
                NetworkServer.Destroy(ball);

            base.OnServerDisconnect(conn);
        }

        // Wyjœcie do lobby
        public void OnExitClicked()
        {
            if (NetworkServer.active)
                StopHost();
            else
                StopClient();
        }
    }
}
