using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITown : MonoBehaviour
{
    public enum ButtonType { back = 0 }
    public ButtonType type;
    public void onClick() {
        switch (type) {
            case ButtonType.back:
            SceneManager.LoadScene("SampleScene");
            break;
        }
    }

    // 1~3F 초상화 눌렀을 때,
    // CharacterScene의 Canvas의 상호관계를 작성한다.

    // CallTower했을 때... 1F에 얼굴나오게 해야합니다. (이건 더 작게?)
    // 여기서도 작은 초상화는 필요해보인다?

    // 아무래도, 자리를 지정해두고, 박스형태로 만들어 두자
    // 시작 전에, 배치를 설정할 수 있도록 만들자.
}
