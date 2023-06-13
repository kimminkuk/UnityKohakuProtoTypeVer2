using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPortrait : MonoBehaviour
{
    public enum Floor { First = 1, Second, Third };
    public enum Type { Range = 1, Melee };
    public enum Row { First = 1, Second, Third };

    public Floor curFloor;
    public Type curType;
    public Row curRow;

    /*
    *    Portrait-Border
    */

    //GameManager에서 isBorderActive 숫자를 카운팅하자.
    // ex) 1Floor,Melee,FirstRow -> 111
    // ex) 1Floor,Range,SecondRow ->122
    // ex) 3Floor,Melee,ThirdRow -> 313
    // ex) 3Floor,Range,FirstRow -> 321

    void Start() {

    }

    public void callPortrait() {
        // enum Floor, Type, Row를 int로 바꿔서 onClick에 넣는다.
        onClick((int)curFloor, (int)curType, (int)curRow);
    }
    void onClick(int floor, int type, int row) {
        Debug.Log("onClick");
        Debug.Log("cur Floor: " + floor + " type: " + type + " row: " + row);
    }

    void firstClickColorEdge() {
        
    }

    // Call Portrait and Character Status
    void onPortrait() {
        
    }

    void onCharacterStatus() {
        
    }
}
