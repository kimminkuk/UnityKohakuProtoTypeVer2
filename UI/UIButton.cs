using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    //Image icon array
    public Image[] icons;
    //Image icon;
    Text textLevel;
    private enum ImageType { StartStop = 0, Shop, Town }
    public enum ButtonType { StartStop, Shop, Town }
 

    public ButtonType type;

    //private bool isStart = false; //false state is Start, true state is Stop
    private string folderPath = "Sprites/UI";
    private string spriteName;
    
    /*
    *    Shop Canvas Add
    */
    // public GameObject storeCanvas;
    // public GameObject gameUiCanvas;
    private void Start() {
        switch (type) {
            case ButtonType.StartStop:
                spriteName = "Stop";
                Sprite loadedSprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
                icons[(int)ImageType.StartStop] = GetComponent<Image>(); // Get the Image component
                icons[(int)ImageType.StartStop].sprite = loadedSprite;
                break;
            
        }
    }

    public void OnClick() {
        //ButtonType Switch-Case
        switch (type) {
            case ButtonType.StartStop:
                //isStart: true -> Game Stop!
                //isStart: false -> Game Doing!
                gameUIManager.gameUiInstance.isStart = !gameUIManager.gameUiInstance.isStart; 
                GameManager.instance.FloorPosOnOff(gameUIManager.gameUiInstance.isStart);
                gameStartStopControl(gameUIManager.gameUiInstance.isStart);
                break;
            case ButtonType.Shop:
                //상점 열기
                gameUIManager.gameUiInstance.isStart = true;
                Time.timeScale = 0f; // Pause the game
                gameStartStopControl(true);
                GameManager.instance.FloorPosOnOff(gameUIManager.gameUiInstance.isStart);
                gameUIManager.gameUiInstance.CloseGameUi();
                gameUIManager.gameUiInstance.OpenShop();
                break;
            case ButtonType.Town:
                // CharacterScene을 호출합니다.
                gameUIManager.gameUiInstance.isStart = true;
                Time.timeScale = 0f; // Pause the game
                gameStartStopControl(true);
                GameManager.instance.FloorPosOnOff(gameUIManager.gameUiInstance.isStart);
                SceneManager.LoadScene("CharacterScene");
                break;
        }
    }

    public void gameStartStopControl(bool isStart) {
        if (isStart) {
            spriteName = "Stop";
            Time.timeScale = 0f; // Pause the game
        }
        else {
            spriteName = "Start";
            Time.timeScale = 1f; // Pause the game
        }
        Sprite loadedSprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
        icons[(int)ImageType.StartStop].sprite = loadedSprite;      
    }
}
