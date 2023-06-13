using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static gameUIManager gameUiInstance;

    public GameObject ShopCanvas;
    public GameObject gameUiCanvas;

    public GameObject ShopTowerCallPopup;
    public GameObject ShopWeaponCallPopup;

    [Header("# Game UI Control")]
    public bool isStart = false; //true -> gameStop , false -> game Doing..

    [Header("# Shop Control")]
    public bool isTowerCall = false;
    public bool isWeaponCall = false;

    [Header("# Game UI Status")]
    public GameObject levelTextObject;
    //public Text levelText;

    // 1F Range GameObject 3개 배열 선언
    public GameObject[] range1F = new GameObject[3];
    public GameObject[] melee1F = new GameObject[3];
    public GameObject[] range2F = new GameObject[3];
    public GameObject[] melee2F = new GameObject[3];
    public GameObject[] range3F = new GameObject[3];
    public GameObject[] melee3F = new GameObject[3];

    // 2F Range GameObject 3개
    // 3F Range GameObject 3개
    

    private void Awake() {
        //levelText = GetComponent<Text>();
        gameUiInstance = this;
    }

    void Start() {
        
        //시작할 때, 게임을 멈춘 상태로 시작합니다.
        Time.timeScale = 0f;
        isStart = true;
        GameManager.instance.FloorPosOnOff(isStart);
    }

    public void StageUpdate(int curStage) {
        
        //levelTextObject의 Text를 가져옵니다.
        Text levelText = levelTextObject.GetComponent<Text>();
        levelText.text = string.Format("STAGE {0:000}", curStage);
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
    
    public void OpenShopTowerCallPopup() {
        ShopTowerCallPopup.SetActive(true);
    }

    public void CloseShopTowerCallPopup() {
        ShopTowerCallPopup.SetActive(false);
    }

    public void OpenShopWeaponCallPopup() {
        ShopWeaponCallPopup.SetActive(true);
    }

    public void CloseShopWeaponCallPopup() {
        ShopWeaponCallPopup.SetActive(false);
    }

    public void OpenPortrait(int floor, int weapon, int row) {
        if (floor == 1) {
            if (weapon == 1) {
                range1F[row].SetActive(true);
            } else if (weapon == 2) {
                melee1F[row].SetActive(true);
            }
        } else if (floor == 2) {
            if (weapon == 1) {
                range2F[row].SetActive(true);
            } else if (weapon == 2) {
                melee2F[row].SetActive(true);
            }
        } else if (floor == 3) {
            if (weapon == 1) {
                range3F[row].SetActive(true);
            } else if (weapon == 2) {
                melee3F[row].SetActive(true);
            }
        }
    }

    public void ClosePortrait(int floor, int weapon, int row) {
        if (floor == 1) {
            if (weapon == 1) {
                range1F[row].SetActive(false);
            } else if (weapon == 2) {
                melee1F[row].SetActive(false);
            }
        } else if (floor == 2) {
            if (weapon == 1) {
                range2F[row].SetActive(false);
            } else if (weapon == 2) {
                melee2F[row].SetActive(false);
            }
        } else if (floor == 3) {
            if (weapon == 1) {
                range3F[row].SetActive(false);
            } else if (weapon == 2) {
                melee3F[row].SetActive(false);
            }
        }
    }
}
