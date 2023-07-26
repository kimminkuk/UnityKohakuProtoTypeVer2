using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerToSave : MonoBehaviour
{
    [SerializeField] private PlayerSaveManager _playerSaveManager;
    
    private void Reset() {
        _playerSaveManager = FindObjectOfType<PlayerSaveManager>();
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        var playerDataTask = _playerSaveManager.LoadPlayer();
        yield return new WaitUntil(() => playerDataTask.IsCompleted);
        var playerData = playerDataTask.Result;

        if (playerData.HasValue) {
            GameManager.instance.UpdatePlayer(playerData.Value);
        }
        GameManager.instance.OnPlayerUpdated.AddListener(HandlePlayerUpdated);
    }

    private void HandlePlayerUpdated() {
        _playerSaveManager.SavePlayer(GameManager.instance.PlayerData);
    }
}
