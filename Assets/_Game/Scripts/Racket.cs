using System;
using Mirror;
using UnityEngine;

public class Racket : NetworkBehaviour
{
    public event Action OnHit;

    [SerializeField] private Rigidbody2D _rigidbody2D;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (!isLocalPlayer) return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            OnHit?.Invoke();
        }
    }
}
