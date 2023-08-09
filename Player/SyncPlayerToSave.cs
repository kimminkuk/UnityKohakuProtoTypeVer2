using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerToSave : MonoBehaviour
{
    bool PLAYER_VER1 = false;
    bool PLAYER_VER2 = true;
    bool PLAYER_KEY3 = false;
    [SerializeField] private PlayerSaveManager _playerSaveManager;
    
    private void Reset() {
        _playerSaveManager = FindObjectOfType<PlayerSaveManager>();
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        #if PLAYER_VER1
            var playerDataTask = _playerSaveManager.LoadPlayer();
            yield return new WaitUntil(() => playerDataTask.IsCompleted);
            var playerData = playerDataTask.Result;

            if (playerData.HasValue) {

                // playerData Struct를 폐기해서 이제 안씁니다.
                GameManager.instance.UpdatePlayer(playerData.Value);
            }
            GameManager.instance.OnPlayerUpdated.AddListener(HandlePlayerUpdated);
        #endif

        #if PLAYER_VER2
            var playerVer2DataTask = _playerSaveManager.LoadPlayerVer2();
            yield return new WaitUntil(() => playerVer2DataTask.IsCompleted);
            var playerDataVer2 = playerVer2DataTask.Result;
            if (playerDataVer2.HasValue) {
                GameManager.instance.UpdatePlayerVer2(playerDataVer2.Value);
            }
            GameManager.instance.OnPlayerUpdated.AddListener(HandlePlayerVer2Updated);
        #endif
        yield return null;
    }

#if PLAYER_KEY1
    private void HandlePlayerUpdated() {
        _playerSaveManager.SavePlayer(GameManager.instance.PlayerData);
    }
#endif
    private void HandlePlayerVer2Updated() {
        _playerSaveManager.SavePlayerVer2(GameManager.instance.playerDataVer2);
    }

    private void HandlePlayerVer3Updated() {
        _playerSaveManager.SavePushPlayer(GameManager.instance.playerDataVer2);
    }
}
