using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Ball : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody2D rigidbody2d;

        public override void OnStartServer()
        {
            base.OnStartServer();
            rigidbody2d.simulated = true;

            rigidbody2d.linearVelocity = new Vector2(RandomNegOrPos(),RandomNegOrPos()) * speed;

            int RandomNegOrPos()
            {
                return Random.Range(0,2) == 0 ? -1 : 1;
            }
        }

        // float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
        // {
        //     return (ballPos.y - racketPos.y) / racketHeight;
        // }

        // [ServerCallback]
        // void OnCollisionEnter2D(Collision2D col)
        // {
        //     // Note: 'col' holds the collision information. If the
        //     // Ball collided with a racket, then:
        //     //   col.gameObject is the racket
        //     //   col.transform.position is the racket's position
        //     //   col.collider is the racket's collider

        //     // did we hit a racket? then we need to calculate the hit factor
        //     if (col.transform.GetComponent<Player>())
        //     {
        //         // Calculate y direction via hit Factor
        //         float y = HitFactor(transform.position,
        //                             col.transform.position,
        //                             col.collider.bounds.size.y);

        //         // Calculate x direction via opposite collision
        //         float x = col.relativeVelocity.x > 0 ? 1 : -1;

        //         // Calculate direction, make length=1 via .normalized
        //         Vector2 dir = new Vector2(x, y).normalized;

        //         rigidbody2d.linearVelocity = dir * speed;
        //     }
        // }
        // void FixedUpdate()
        // {
        //     Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //     if (rb.linearVelocity.magnitude < 7f)
        //     {
        //         rb.linearVelocity = rb.linearVelocity.normalized * 8f;
        //     }
        // }

    }
}
