using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIShop : MonoBehaviour
{
    private enum ImageType { back = 0, callTower = 1, callWeapon = 2, popup }
    public enum ButtonType { back = 0, callTower, callWeapon, popup }
    public ButtonType type;
    public void OnClickInShop() {
        switch (type) {
            case ButtonType.back:
                gameUIManager.gameUiInstance.CloseShop();
                gameUIManager.gameUiInstance.OpenGameUi();
                break;
            case ButtonType.callTower:
                //타워 소환

                // 소환하시겠습니까? 팝업창 띄우기
                gameUIManager.gameUiInstance.OpenShopTowerCallPopup();
                // 팝업창에서 확인을 누르면 타워 소환
                // 팝업창에서 취소를 누르면 타워 소환 취소
                break;
            case ButtonType.callWeapon:
                //타워 무기 소환
                // 소환하시겠습니까? 팝업창 띄우기
                gameUIManager.gameUiInstance.OpenShopWeaponCallPopup();
                break;
        }
    }

    public void OkCallTowerPopup() {
        // 소환 모션
        Debug.Log("OkCallTowerPopup");
        GameManager.instance.CallTower();
        CloseCallTowerPopup();
    }

    public void OkCallWeaponPopup() {
        // 소환 모션
        Debug.Log("OkCallWeaponPopup");
        CloseCallWeaponPopup();
    }

    public void CloseCallTowerPopup() {
        gameUIManager.gameUiInstance.CloseShopTowerCallPopup();
    }

    public void CloseCallWeaponPopup() {
        gameUIManager.gameUiInstance.CloseShopWeaponCallPopup();
    }


}
