using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private PlayerDataVer2 _playerDataVer2;
    public PlayerDataVer2 playerDataVer2 => _playerDataVer2;

    //public PlayerData PlayerData => FindObjectOfType<FirebaseAuthManager>().PlayerData;
    public UnityEvent OnPlayerUpdated = new UnityEvent();

    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public bool isGameStart = false;
    public int gameMode = 0; // 0 : Single, 1 : Multi

    [Header("# GameObject")]
    public PoolManager pool;
    public PoolCharcterManager poolCharcter;
    public PoolEnemyManager poolEnemy;
    public PoolRangeTower poolRangeTower;
    public ItemManager itemPool;

    public float makeIntervalEnemyTime = 0.5f;
    private float makeEnemyTimer = 0f;

    public float makeIntervalTime = 0.1f;

    // Respawn Pos List (GameObject, transform.position)
    /*
    *    Respawn List
    */
    public List<GameObject> respawnPosObjectList = new List<GameObject>();
    public List<GameObject> floortPosObjectList = new List<GameObject>();
    public GameObject defaultRespawnPos;
    
    /*
    *    PlayerData
    */
    public int coin;
    public int tempCoinValue;
    public int gameLevel;
    public string playerName;

    /*
    *    Firebase Realtime Database
    */
    public GameObject firebaseRTDM;
    public PlayerSaveManager playerSaveManager;


    /*
    *    UI
    */
    public int kill;
    public GameObject coinText;
    public GameObject userIdText;
    public Image[] icons;
    private string folderPath = "Sprites/UI";
    private string spriteName;    
    
    //다이아 추가 예정..
    //public GameObject diamondText;

    /*
    *    Game Stop/Start
    */
    // 1F Range GameObject 3개 배열 선언
    public GameObject[] rangePos1F = new GameObject[3];
    public GameObject[] meleePos1F = new GameObject[3];
    public GameObject[] rangePos2F = new GameObject[3];
    public GameObject[] meleePos2F = new GameObject[3];
    public GameObject[] rangePos3F = new GameObject[3];
    public GameObject[] meleePos3F = new GameObject[3];

    /*
    *    Level System
    */
    [Header("# Game Status")]
    const int MAX_STATG = 11;
    public int gameStage = 0;
    private int[] enemyCount = new int[MAX_STATG] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    private int clearEnemyCount = 0;
    public int curLife = 0;
    private const int TOTAL_LIFE = 10;
    public GameObject[] lifeObject = new GameObject[TOTAL_LIFE];

    public UIButton uiButton; 
    private void Awake() 
    {
        instance = this;        
    }

    async void Start()
    {
        // Bug
        // 2. Load하고 나서, Tower들을 클릭할 수 가 없다..?
        //    스타트/스톱 버튼을 한번 눌러서 초기화 해줘야함 왜지??

        // 1. test301, test302, test303 만들어서 firebaseId 조금만 더 Debug 해보기
        //    Sync이런걸 안해서.. 정확히 모르겠음
        //    Sync 관련해서 내용이 조금 더 있긴하네.. 적용해볼까? ㄴㄴ Fail 나오는 코드있음 그냥 제끼자 
        // 2. 이번엔, Melee, Range Tower 까지 PlayerDataVer2에 적용하자
        //    2-1. Melee, Range 소환한 케릭터들 일치 시키기
        //    2-2. 위치까지 기억해두기.
        _playerDataVer2 = FindObjectOfType<FirebaseAuthManager>().PlayerDataVer2;
        playerSaveManager = firebaseRTDM.GetComponent<PlayerSaveManager>();
        
        userIdText.GetComponent<UnityEngine.UI.Text>().text = _playerDataVer2.FirebaseId;
        PlayerDataVer2? LoadPlayerDataVer2 = await playerSaveManager.GetPlayerDataNullCheck(_playerDataVer2.FirebaseId);
        if (LoadPlayerDataVer2 != null) {
            Debug.Log("LoadPlayerDataVer2: " + LoadPlayerDataVer2);
            Debug.Log("LoadPlayerDataVer2.Value: " + LoadPlayerDataVer2.Value);
            _playerDataVer2 = LoadPlayerDataVer2.Value;
            coinText.GetComponent<UnityEngine.UI.Text>().text = _playerDataVer2.Coin.ToString();
            gameStage = _playerDataVer2.GameLevel;
        } else {
            Debug.Log("LoadPlayerDataVer2 is null");
        }
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        //UI에서 버튼 클릭 시, Magician 케릭터를 FloorPos-1 위치에 생성
        //만약에 케릭터가 3개 이상이면, FloorPos-2에 생성
        //FloorPos-2에 케릭터가 3개 이상이면, FloorPos-3에 생성
        // a버튼을 누르면 나오게 만듭니다.
        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Get 0,1 Random Code
            // PoolCharacterManager 클래스의 pools list의 count를 가져옵니다.
            
            //int getClassChangeIndex = Random.Range(0, poolCharcter.pools.Length);
            int getClassChangeIndex = 1;
            int meleeCount = 2;
            int floorSelect = 0;
            if (getClassChangeIndex >= meleeCount) {
                floorSelect = 1;
            }
            GameObject obj = poolCharcter.GetObject(getClassChangeIndex);
            obj.transform.position = floortPosObjectList[floorSelect].transform.position;
        }  
    }

    public void UpdatePlayer(PlayerDataVer2 playerData)
    {
        Debug.Log("UpdatePlayer() Call");
        if (!playerData.Equals(_playerDataVer2)) {
            _playerDataVer2 = playerData;
            Debug.Log("_playerDataVer2.Name: " + _playerDataVer2.Name);
            OnPlayerUpdated.Invoke();
        }
    }

    private void UpdateInfo() {
        Debug.Log("UpdateInfo() Call");
        gameStage++;
        _playerDataVer2.GameLevel = gameStage;
        Debug.Log("_playerDataVer2.Coin: " + _playerDataVer2.Coin + ", gameStage: " + gameStage);        
        playerSaveManager.SavePlayerVer2(_playerDataVer2);
        playerSaveManager.SavePlayerChild(_playerDataVer2);
    }

    // Level System 와꾸좀 잡아보자
    // 1. Lv UI 추가
    // 2. 돈 UI 추가
    // 3. 상점 버튼 및 UI 추가

    public void CallTower() {
        makeTowerObject();
    }

    public void CallWeapon() {

    }

    // Call Tower
    // melee, range
    // 0: Hammer
    // 1: Sword
    // 2: Staff
    private void makeTowerObject() {
        GameObject obj = null;
        int getMeleeTowerIndex = poolCharcter.pools.Length; //2
        int getRangeTowerIndex = poolRangeTower.pools.Length; //1

        int getClassChangeIndex = UnityEngine.Random.Range(0, getMeleeTowerIndex + getRangeTowerIndex); //3 -> 0,1,2
        int meleeCount = getMeleeTowerIndex; //2
        int floorSelect = 0;

        //meleeCount가 getMeleeTowerIndex보다 크거나 같으면, Range Tower를 생성합니다.
        if (getClassChangeIndex >= meleeCount) {
            obj = poolRangeTower.GetRangeTowerObject(getClassChangeIndex - meleeCount);
        }
        else {
            obj = poolCharcter.GetObject(getClassChangeIndex);
        }

        obj.transform.position = defaultRespawnPos.transform.position;
    }

    public void coinUp() {
        coinText.GetComponent<UnityEngine.UI.Text>().text = _playerDataVer2.Coin.ToString();
    }
    
    public void FloorPosOnOff(bool isOn) {
        int posMax = 3;
        if (isOn) {
            for (int pos = 0; pos < posMax; pos++) {
                meleePos1F[pos].SetActive(true);
                rangePos1F[pos].SetActive(true);
                meleePos2F[pos].SetActive(true);
                rangePos2F[pos].SetActive(true);
                meleePos3F[pos].SetActive(true);
                rangePos3F[pos].SetActive(true);
            }
        } else {
            for (int pos = 0; pos < posMax; pos++) {
                meleePos1F[pos].SetActive(false);
                rangePos1F[pos].SetActive(false);
                meleePos2F[pos].SetActive(false);
                rangePos2F[pos].SetActive(false);
                meleePos3F[pos].SetActive(false);
                rangePos3F[pos].SetActive(false);
            }
        }
    }

    // lifeObject Index 범위 벗어나는거 방지 -> GameOver Call 할 때, UIButton의 Stop처리
    // curLife가 0이면, GameOver -> 시간 정지 혹은 GameOver 띄우기
    // 레밸 별로 몬스터 나오게 하기, 몬스터 종류 변경하기
    // 음.. 뭔가 업데이트로 하면 아까움..
    // 1. GameStart
    public void StartGame() {
        //gameStage = 0;
        StartStage(gameStage);
    }
    public void StartStage(int stage) {

        Debug.Log("StartStage stage: " + (stage + 1));
        gameUIManager.gameUiInstance.StageUpdate(stage + 1);
        clearEnemyCount = 0;
        StageStop();
        StageSpawnEnemy(stage, enemyCount[stage]);
    }
    // 2. Stage의 적 Object 생성 -> Stage의 모든 Object Kill or Object Exit ->
    private void StageSpawnEnemy(int stage, int enemyCount) {
        float xOffset = 0.3f;
        for (int count = 0; count < enemyCount; count++) {
            GameObject obj = poolEnemy.GetEnemyObject(stage);

            //GameObject obj = poolEnemy.GetEnemyObject(10);

            int getRespawnPosIndex = UnityEngine.Random.Range(0, respawnPosObjectList.Count);
            //obj.transform.position = respawnPosObjectList[getRespawnPosIndex].transform.position;
            
            //obj.transform.position의 x좌표에 xOffset을 추가하고 싶습니다.
            Vector3 pos = respawnPosObjectList[getRespawnPosIndex].transform.position;
            pos.x += xOffset * count;
            obj.transform.position = pos;
        }
    }
    // 3. Stage의 모든 Object Kill or Object Exit Trigger Call
    //    EnemyMovement에서 호출,
    //    gameOver아니면, StartStage 호출
    public void DefeatObject(bool isDie) {
        if (isDie) {
            _playerDataVer2.Coin += 1;
            coinUp();
        }
        clearEnemyCount++;
        if (clearEnemyCount >= enemyCount[gameStage]) {
            StageClear();
        }
    }
    
    // 4. StageClear
    public void StageClear() {
        Debug.Log("StageClear");
        if (gameStage > MAX_STATG) {
            // Game Clear
            Debug.Log("Game Clear");
        } else {
            // Update Info
            UpdateInfo();

            // SAVE DB-DATA
            //UpdatePlayer(_playerDataVer2);

            //1. Stage Clear UI and Game Stop
            StartCoroutine(WinMotion());
            
        }
    }    
    // 5. 1~4 반복
    IEnumerator WinMotion() {
        Debug.Log("WinMotion Call");
        poolRangeTower.PoolRangeTowerWinPose();
        poolCharcter.MeleePoolsTowerWinPoseCall();
        pool.ClearPools();
        float waitTime = 3f;
        float elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("2. Game Stop");
        //2. Game Stop
        StartStage(gameStage);        
    }

    public void LifeDelete() {
        lifeObject[curLife - 1].SetActive(false);
        curLife--;
        if (curLife <= 0) {
            GameOver();
        } else {
            DefeatObject(false);
        }
    }

    public void KillEnemy() {
        clearEnemyCount++;
        kill++;
        if (clearEnemyCount >= enemyCount[gameStage]) {
            StageClear();
        }
    }

    public void StageStop() {
        // Game Stop
        gameUIManager.gameUiInstance.isStart = true;
        GameManagerStartStop(gameUIManager.gameUiInstance.isStart);
    }

    public void GameOver() {
        gameStage = 0;

        StageStop();
        Debug.Log("Game Over");
        
    }

    public void GameManagerStartStop(bool isStart) {
        if (isStart) {
            spriteName = "Stop";
            Time.timeScale = 0f; // Pause the game
        }
        else {
            spriteName = "Start";
            Time.timeScale = 1f; // Pause the game
        }
        Sprite loadedSprite = Resources.Load<Sprite>(folderPath + "/" + spriteName);
        icons[0].sprite = loadedSprite;      
    }


    //1. 타격감 조금 더 신경써서 만들기 (이펙트가 너무 없다.)
    //1-1. 그릴려다가 존나 현타오네.. 그냥 사자 10달라 이하로 ㅅㅂ
    //     근거리, 원거리 이펙트 있는애들 사자

    // 돈 주고 사기 전에,
    // 슬라임 무료 에셋으로 테스트 해보기
    // 걸어가다가 -> 케릭터 발견 -> 앞에서 점프로 회피하기
    // 한번만 동작합니다.
    // 이거 하면, 에셋 다운받은거 쓰면서, 애니메이션 변경까지 해보는거다. 

    //2. stage, 난이도 설정
    //3. 공격시, 상호작용 조금 더 조절 (맞으면 확실하게 더 멈추게??)
}