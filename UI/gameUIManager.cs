using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static gameUIManager gameUiInstance;

    public GameObject ShopCanvas;
    public GameObject gameUiCanvas;

    [Header("# Game UI Control")]
    public bool isStart = false;

    private void Awake() {
        gameUiInstance = this;
    }

    public void CloseShop() {
        ShopCanvas.SetActive(false);
    }

    public void OpenShop() {
        ShopCanvas.SetActive(true);
    }

    public void CloseGameUi() {
        gameUiCanvas.SetActive(false);
    }

    public void OpenGameUi() {
        gameUiCanvas.SetActive(true);
    }
}
