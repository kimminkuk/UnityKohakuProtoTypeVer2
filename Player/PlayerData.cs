using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerData {
    public string Name;
    public int GameLevel;
    public int Coin;
}

[Serializable]
public struct PlayerDataVer2 {
    public string FirebaseId;
    public string Name;
    public int GameLevel;
    public int Coin;
}