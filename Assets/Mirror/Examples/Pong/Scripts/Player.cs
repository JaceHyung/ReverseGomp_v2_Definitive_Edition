using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        private TextMeshProUGUI healthText;
        public float speed = 30;
        public Rigidbody2D rigidbody2d;
        [SyncVar]
        public int health = 5;

        public void OnRematchClicked()
        {
            if (!isLocalPlayer)
                return;

            CmdVoteForRematch();
        }

        [Command]
        void CmdVoteForRematch()
        {
            // Przekazujemy serwerowi, aby policzy³ g³os przez MatchController
            if (MatchController.instance != null)
            {
                MatchController.instance.RegisterRematchVote();
            }
        }

        public override void OnStartLocalPlayer()
        {
            GameObject ui = GameObject.Find("HealthText");
            if (ui != null)
            {
                healthText = ui.GetComponent<TextMeshProUGUI>();
                Debug.Log("HealthText podpiêty poprawnie");
            }
            else
            {
                Debug.LogError("Nie znaleziono obiektu HealthText w scenie!");
            }
        }

        // need to use FixedUpdate for rigidbody

        void FixedUpdate()
        {
            // only let the local player control the racket.
            // don't control other player's rackets
            if (isLocalPlayer)
#if UNITY_6000_0_OR_NEWER
                rigidbody2d.linearVelocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
#else
                rigidbody2d.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
#endif
        }
        void Update()
        {
            if (!isLocalPlayer) return;

            if (healthText != null)
                healthText.text = "Health: " + health;
        }

        void Die()
        {
            MatchController.instance.PlayerDied(gameObject);
            NetworkServer.Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!isServer) return;

            if (collision.gameObject.CompareTag("Ball"))
            {
                health -= 1;

                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }
}