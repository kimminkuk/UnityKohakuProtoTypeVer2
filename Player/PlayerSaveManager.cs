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

    public async Task<PlayerData?> LoadPlayer() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        if (!dataSnapshot.Exists) {
            return null;
        }

        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<bool> SaveExists() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
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
