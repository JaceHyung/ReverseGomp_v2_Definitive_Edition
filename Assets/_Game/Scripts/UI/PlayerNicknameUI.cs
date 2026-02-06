using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNicknameUI : MonoBehaviour
{
    public const string PREFS_PLAYER_NAME = "PlayerNickName";
    [SerializeField] private TMP_InputField _nameInput;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(PREFS_PLAYER_NAME))
        {
            _nameInput.text = PlayerPrefs.GetString(PREFS_PLAYER_NAME);
        }
        else
        {
            string playerName = $"Player{UnityEngine.Random.Range(1111,9999)}";
            _nameInput.text = playerName;
            NameInput_OnValueChanged(playerName);
        }
        _nameInput.onValueChanged.AddListener(NameInput_OnValueChanged);
    }

    private void OnDestroy()
    {
        _nameInput.onValueChanged.RemoveListener(NameInput_OnValueChanged);
    }

    private void NameInput_OnValueChanged(string arg0)
    {
        PlayerPrefs.SetString(PREFS_PLAYER_NAME,arg0);
        PlayerPrefs.Save();
    }
}
