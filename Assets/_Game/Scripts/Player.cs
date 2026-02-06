using System;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    public static Player local;
    public static event Action<Player> OnPlayerDataChange;
    public static event Action<Player> OnPlayerDeath;
    public event Action<bool> OnPlayerReadyChange;

    [SerializeField] private float _speed;
    
    [SyncVar(hook = nameof(OnHealthChange))] public int health = 5;
    [SyncVar(hook = nameof(OnNameChange))] public string playerName;
    [SyncVar(hook = nameof(OnIdChange))] public int playerId;
    [SyncVar(hook = nameof(OnReadyChange))] public bool isReady;

    private Racket _racket;
    private float _currentInput;

    protected void FixedUpdate()
    {
        if(_racket != null)
        {
            _racket.Rigidbody2D.linearVelocityY = _speed * _currentInput;
        }
    }

    private void OnHealthChange(int oldVal, int newVal) => OnPlayerDataChange?.Invoke(this);

    private void OnNameChange(string oldVal, string newVal) => OnPlayerDataChange?.Invoke(this);

    private void OnIdChange(int oldVal, int newVal) => OnPlayerDataChange?.Invoke(this);

    private void OnReadyChange(bool oldVal, bool newVal)
    {
        OnPlayerReadyChange?.Invoke(newVal);
        OnPlayerDataChange?.Invoke(this);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        local = this;
        
        CmdRegisterName(PlayerPrefs.GetString(PlayerNicknameUI.PREFS_PLAYER_NAME));

        InputSystem.actions.FindAction("Move").started += OnMove;
        InputSystem.actions.FindAction("Move").performed += OnMove;
        InputSystem.actions.FindAction("Move").canceled += OnMove;
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();
        
        InputSystem.actions.FindAction("Move").started += OnMove;
        InputSystem.actions.FindAction("Move").performed += OnMove;
        InputSystem.actions.FindAction("Move").canceled += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _currentInput = context.ReadValue<float>();
    }

    [Command]
    private void CmdRegisterName(string name)
    {
        playerName = name;
    }

    [Command(requiresAuthority = false)]
    public void CmdRegisterId(int id)
    {
        Debug.Log($"Setting id {id}");
        playerId = id;
    }

    [Command]
    private void CmdSetReady(bool ready)
    {
        isReady = ready;
    }

    public void SetReady(bool ready)
    {
        CmdSetReady(ready);
    }

    public void ToggleReady()
    {
        CmdSetReady(!isReady);
    }

    public void AssignRacket(GameObject racket)
    {
        _racket = racket.GetComponent<Racket>();
        _racket.OnHit += Racket_OnHit;
    }

    private void Racket_OnHit()
    {
        health--;

        if(health <= 0)
        {
            // OnPlayerDeath?.Invoke(this);
            MatchController.Instance.EndGame(this);
        }
    }
}