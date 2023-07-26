using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginPageManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;    
    public static LoginPageManager LoginInstance;
    public bool IS_LOGIN = false;
    public GameObject LoginFailPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake() {
        LoginInstance = this;
    }

    public void GoGameScene()
    {
        if (IS_LOGIN)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void SetUserId(string userId)
    {
        _playerData.Name = userId;
        Debug.Log("SetUserId: " + _playerData.Name);
    }

    public void CloseLoginFailPanel()
    {
        if (!IS_LOGIN) {
            LoginFailPanel.SetActive(false);
        }
    }
}
