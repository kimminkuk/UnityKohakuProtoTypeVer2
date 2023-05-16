using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Level, Kill, Time, Score, Health }
    public InfoType type;
    Text myText;
    Slider mySlider;
    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Level:
                // 현재 게임 스테이지 표시 (거의 무한으로 갈거임... 10정도만 설계하고 나중에 더 추가할 예정)
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.gameLevel);
                break;
            case InfoType.Kill:
                // 잡은 적 표시 
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                // 스테이지 시작할 때마다 시간이 흐르는것을 표시 (스테이지 시작할 때마다, 시간 초기화)
                
                break;
            case InfoType.Health:
                // 성채 체력 (보석 같은거로 표시할 예정)

                break;
        }
    }
}
