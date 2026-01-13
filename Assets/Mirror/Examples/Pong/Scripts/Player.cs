using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody2D rigidbody2d;
        [SyncVar]
        public int health = 5;

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
            void Die()
            {
                Debug.Log("Player died");

                NetworkServer.Destroy(gameObject);
            }

        }
    }
}