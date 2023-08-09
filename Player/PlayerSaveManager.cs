using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;

public class PlayerSaveManager : MonoBehaviour
{
    private const string PLAYER_KEY = "PLAYER_KEY";
    private const string PLAYER_KEY_VER2 = "PLAYER_KEY_VER2";
    private const string PLAYER_KEY_VER3 = "PLAYER_KEY_VER3";
    private FirebaseDatabase _database;

    void Start()
    { 
        // Initialize Firebase
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://console.firebase.google.com/project/kohaku-4/firebaseio.com/");
        Debug.Log("PlayerSaveManager Start");
        _database = FirebaseDatabase.DefaultInstance;
    }

    public void SavePlayer(PlayerData player) {
        Debug.Log("SavePlayer Call");
        PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(player));
        _database.GetReference(PLAYER_KEY).SetRawJsonValueAsync(JsonUtility.ToJson(player));
    }

    public void SavePlayerVer2(PlayerDataVer2 player) {
        Debug.Log("SavePlayerVer2 Call");
        PlayerPrefs.SetString(PLAYER_KEY_VER2, JsonUtility.ToJson(player));
        _database.GetReference(PLAYER_KEY_VER2).SetRawJsonValueAsync(JsonUtility.ToJson(player));
    }

    public void SavePlayerVer3(PlayerDataVer2 player) {
        Debug.Log("SavePlayerVer3 Call");
        PlayerPrefs.SetString(PLAYER_KEY_VER3, JsonUtility.ToJson(player));
        _database.GetReference(PLAYER_KEY_VER3).SetRawJsonValueAsync(JsonUtility.ToJson(player));
    }

    public async void SavePushPlayer(PlayerDataVer2 player) {
        Debug.Log("SavePushPlayer Call");
        PlayerPrefs.SetString(PLAYER_KEY_VER3, JsonUtility.ToJson(player));
        await _database.GetReference(PLAYER_KEY_VER3).Push().SetRawJsonValueAsync(JsonUtility.ToJson(player));
    }

    public async void SavePlayerChild(PlayerDataVer2 player) {
        Debug.Log("SavePlayerChild Call");
        PlayerPrefs.SetString(PLAYER_KEY_VER3, JsonUtility.ToJson(player));
        var playerRef = _database.GetReference(PLAYER_KEY_VER3).Child(player.FirebaseId);
        await playerRef.SetRawJsonValueAsync(JsonUtility.ToJson(player));
    }

    public async Task<PlayerData?> LoadPlayer() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        if (!dataSnapshot.Exists) {
            return null;
        }

        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<PlayerDataVer2?> LoadPlayerVer2() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY_VER2).GetValueAsync();
        Debug.Log("LoadPlayerVer2 Call");
        Debug.Log(dataSnapshot.GetRawJsonValue());
        if (!dataSnapshot.Exists) {
            return null;
        }

        return JsonUtility.FromJson<PlayerDataVer2>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<PlayerDataVer2?> LoadPlayerVer3() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY_VER3).GetValueAsync();
        Debug.Log("LoadPlayerVer3 Call");
        Debug.Log(dataSnapshot.GetRawJsonValue());
        if (!dataSnapshot.Exists) {
            return null;
        }

        return JsonUtility.FromJson<PlayerDataVer2>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<PlayerDataVer2?> GetPlayerDataNullCheck(string firebaseId) {
        var playerRef = _database.GetReference(PLAYER_KEY_VER3).Child(firebaseId);
        var dataSnapshot = await playerRef.GetValueAsync();
        if (!dataSnapshot.Exists) {
            return null;
        }
        return JsonUtility.FromJson<PlayerDataVer2>(dataSnapshot.GetRawJsonValue());
    }
    
    public async Task<bool> SaveExists() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public async Task<bool> SaveExistsVer2() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY_VER2).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public async Task<bool> SaveExistsVer3() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY_VER3).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public async Task<bool> SaveExistsAny(string KEY) {
        var dataSnapshot = await _database.GetReference(KEY).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void EraseSave() {
        _database.GetReference(PLAYER_KEY).RemoveValueAsync();
    }

    // public PlayerData? LoadPlayer() {
    //     if (SaveExists()) {
    //         return JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(PLAYER_KEY));
    //     } else {
    //         return null;
    //     }
    // }

    // public bool SaveExists() {
    //     return PlayerPrefs.HasKey(PLAYER_KEY);
    // }
}
