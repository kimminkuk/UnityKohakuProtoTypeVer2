using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player : MonoBehaviour
{
#if false
    [Header("# Player State")]
    public bool isPickingUp = false;
    //RididBody2D, SpriteRenderer, Anim Codes
    public Vector2 playerInputVec;
    private bool attackInput = false;

    //임시로, 여기다 두고 나중에 Weapon쪽으로 옮길 예정
    public GameObject missilePrefab;
    private bool missileLaunched = false;
    public float missileSpeed = 10f;
    public Transform missileSpawnPoint;

    //Player 2D 8-direction
    // Enum for 8 possible directions
    private int PlayerLastDirection = 0;

    private bool canPickup = false;
    private GameObject activeItem;
    
    private float pickupTime = 1.5f; // Adjust this to change the pickup time
    private float pickupTimer = 0f;

    //Hands Animation
    private Animator weaponAnimation;
    public GameObject playerObject;

    private void Awake() {
        // rb = GetComponent<Rigidbody2D>();
        // sr = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        base.ChangeClassType(WeaponManager.WeaponType.Hammer);
        hands = GetComponentsInChildren<Hand>(true);
        weaponAnimation = hands[0].GetComponent<Animator>();
        hands[0].weaponType = WeaponManager.WeaponType.Hammer;
    }

    public void reAwake() {
        // rb = GetComponent<Rigidbody2D>();
        // sr = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        base.ChangeClassType(WeaponManager.WeaponType.Sword);
        hands = GetComponentsInChildren<Hand>(true);
        weaponAnimation = hands[0].GetComponent<Animator>();
        //Temp
        hands[0].weaponType = WeaponManager.WeaponType.Sword;
    }

    void Start()
    {
        //if Player's Sprite FlipX is true, then
        if (sr.flipX) {
            PlayerLastDirection = 2;
        } else {
            PlayerLastDirection = 6;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        //if, playerObject is null
        //playerObject find Player Tag and input 
        if (playerObject == null) {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        playerInputVec.x = Input.GetAxisRaw("Horizontal");
        playerInputVec.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            attackInput = true; // Set the attack flag
        }
        if (playerInputVec != Vector2.zero) {
            PlayerLastDirection = Get2D8DirectionToInt(playerInputVec);
        }

        //Input.GetKeyDown E
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            isPickingUp = true;
        }
        if (isPickingUp) {
            pickupTimer += Time.deltaTime;
            if (pickupTimer >= pickupTime) {
                weaponType = activeItem.GetComponent<ItemDropClass>().PickupItemDropClassVer2();
                if (activeItem != null) {
                    activeItem.SetActive(false);
                }
                activeItem = null;
                Debug.Log("Item picked up and added to inventory");
                Debug.Log("Player weaponType: " + weaponType);
                
                //Change the Class,Weapon Type
                // base.weaponType = weaponType;
                // base.ChangeClassType(weaponType);
                // hands[0].ChangeWeaponType(weaponType);

                isPickingUp = false;
                pickupTimer = 0f;
                base.ChangeClassObject(weaponType);
                //base.ChangeClassType(weaponType);

#if false
                // Change to the Class
                string prefabClassName = GetPrefabClassName(weaponType);
                Debug.Log("prefabClassName: " + prefabClassName);
                GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabClassName);

                //한번 더, 바꾸면 Gameobject 없다함
                //MissingReferenceException: The object of type 'GameObject' has been destroyed but you are still trying to access it.
                //Your script should either check if it is null or you should not destroy the object.

                //아무래도, 이거 할 때 깨져서 그런듯? 한번 클래스 바꾸면 Gameobject가 Missing임
                //Update  맨 위에 생성해뒀는데.. 문제는 이러니깐 2번째 부터는 MoveManager까지 가져오네 
                /*
                MissingReferenceException: The object of type 'Rigidbody2D' has been destroyed but you are still trying to access it.
                Your script should either check if it is null or you should not destroy the object.
                */
                /*
                MissingReferenceException: The object of type 'SpriteRenderer' has been destroyed but you are still trying to access it.
                Your script should either check if it is null or you should not destroy the object.
                */
                ChangeClassAppearance(prefab);
                // GameObject newClassObject = Instantiate(prefab);
                //ChangeClassAppearance(newClassObject);
                //ChangeClassAppearanceVer2(newClassObject);
                //ChangeClassAppearanceVer3(prefab);
#endif                
            }
        } else {
            pickupTimer = 0f;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isPickingUp = false;
        }        
    }

    void FixedUpdate() 
    {
        Vector2 nextVec = playerInputVec.normalized * speed * Time.fixedDeltaTime;
        if (nextVec != Vector2.zero) {
            isMoving = true;
            isIdle = false;
        } else {
            
            isMoving = false;
            isIdle = true;
        }
        rb.MovePosition(rb.position + nextVec);


        // Normal Attack-Action
        // Space or Mouse Left Button 1Click
        if (attackInput && !isAttacking) {
            isAttacking = true;
            base.hands[0].NormalAttackTriggerOn();
            //StartCoroutine(NormalAttackCasting());
            LaunchMissileVer3(PlayerLastDirection);
            StartCoroutine(NormalAttack());
            Debug.Log("Attack?");
        }
    }

    //프레임이 종료 되기 전 실행되는 함수
    void LateUpdate() 
    {
        if (playerInputVec.x != 0) {
            sr.flipX = playerInputVec.x < 0; //좌측이면, true flipX가 켜진거임 그리고 우측이면 false
        }
        base.hands[0].isLeft = sr.flipX;
    }

    IEnumerator NormalAttackCasting() {
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator NormalAttack() {
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
        attackInput = false;
    }
    //missile을 플레이어가 이동하는 방향으로 날리기
    //1. 플레이어가 이동하는 방향을 구한다
    //2. 이동하는 값은 가속도가 아니라, 방향 자체를 알려준다.
    //3. Vector3로 변환해서, 2D 플레이어가 이동하는 방향으로 미사일을 날린다.
    void LaunchMissileVer3(int playerDirection)
    {
        if (!missileLaunched) {
            Transform bullet = GameManager.instance.pool.GetObject(0).transform;
            Vector3 dir = Vector3.zero;
            switch (playerDirection) {
                case 0:
                    dir = Vector3.up;
                    break;
                case 1:
                    dir = new Vector3(-1, 1).normalized; //left, up
                    break;
                case 2:
                    dir = Vector3.left;
                    break;
                case 3: 
                    dir = new Vector3(-1, -1).normalized; //left, down
                    break;
                case 4:
                    dir = Vector3.down;
                    break;
                case 5: 
                    dir = new Vector3(1, -1).normalized; //right, down
                    break;
                case 6:
                    dir = Vector3.right;
                    break;
                case 7:
                    dir = new Vector3(1, 1).normalized; //right, up
                    break;
            }
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(10, 1, dir); // -1 is Infinity Per.
            // 3sec After, missile is SetActive False
            StartCoroutine(MissileActiveFalse(bullet.gameObject));
        }
    }

    IEnumerator MissileActiveFalse(GameObject missile) {
        yield return new WaitForSeconds(3.0f);
        missile.SetActive(false);
    }

    // Function to get 2D 8-direction based on input direction
    public Direction2D Get2D8Direction(Vector2 inputDir) {
        // Get angle between forward direction and input direction
        float angle = Vector2.SignedAngle(Vector2.up, inputDir);

        // Calculate index of 2D 8-direction based on angle
        int index = Mathf.RoundToInt(angle / 45.0f) % 8;
        if (index < 0) {
            index += 8;
        }

        // Return corresponding direction
        return (Direction2D)index;
    }

    public int Get2D8DirectionToInt(Vector2 inputDir) {
        // Get angle between forward direction and input direction
        float angle = Vector2.SignedAngle(Vector2.up, inputDir);

        // Calculate index of 2D 8-direction based on angle
        int index = Mathf.RoundToInt(angle / 45.0f) % 8;
        if (index < 0) {
            index += 8;
        }

        // Return corresponding direction
        return index;
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Item")) {
            canPickup = true;
            activeItem = collision.gameObject;
            Debug.Log("OnTriggerEnter2D Item");
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Item")) {
            Debug.Log("OnTriggerExit2D Item");
            canPickup = false;
        }
    }
    private void PickUpItem()
    {
        Debug.Log("How to Destroy?");
    }
    // GetPrefabClassName Func
    // 1. return string of prefabClassName
    // 2. Parameter is WeaponManager.WeaponType
    public string GetPrefabClassName(WeaponManager.WeaponType weaponType) {
        string prefabClassName = "";
        switch (weaponType) {
            case WeaponManager.WeaponType.Sword:
                prefabClassName = "Level_1_Sword";
                break;
            case WeaponManager.WeaponType.Staff:
                prefabClassName = "Level_1_Staff";
                break;
            case WeaponManager.WeaponType.Hammer:
                prefabClassName = "Level_1_Hammer";
                break;
        }
        return prefabClassName;
    }

    /*
    *    Change Class And Destroy Old Class
    */
    public void ChangeClassAppearance(GameObject newPrefab) {
        // Create a new instance of the newPrefab
        GameObject newObj = Instantiate(newPrefab, transform.position, transform.rotation);
        
        // Get the newObjs's components
        Component[] newObjComponents = newObj.GetComponents<Component>();
        
        foreach (Component c in newObjComponents) {
            Debug.Log("test: " + c.GetType());
            //
        }

        // Copy the player's components to the new object
        Component[] components = playerObject.GetComponents<Component>();
        List<Component> newComponents = new List<Component>();
        foreach (Component c in components) {
            if (!(c is Transform)) { 
                if (UnityEditorInternal.ComponentUtility.CopyComponent(c)) {
                    UnityEditorInternal.ComponentUtility.PasteComponentAsNew(newObj);
                }

            }
        }
        // Destroy the current player object
        if (playerObject != null) {
            Destroy(playerObject);
        }
        newObj.GetComponent<Player>().reAwake();
        newObj.tag = "Player";
        newObj.layer = LayerMask.NameToLayer("Player");
        newObj.name = "Player_ReAwake";
        
        // Set the playerObject to the new object
        playerObject = newObj;
        playerObject = GameObject.Find("Player_ReAwake");
        //playerObject How to Active?
        
        
    }
    public void ChangeClassAppearanceVer2(GameObject newPrefab) {
        // Create a new instance of the newPrefab
        GameObject newObj = Instantiate(newPrefab, transform.position, transform.rotation);

        // Get the components on the new object
        Component[] newComponents = newObj.GetComponents<Component>();

        // Copy the player's components to the new object
        Component[] playerComponents = playerObject.GetComponents<Component>();
        for (int i = 0; i < playerComponents.Length; i++) {
            Component playerComponent = playerComponents[i];
            if (!(playerComponent is Transform)) {
                // Find the corresponding component on the new object
                Type componentType = playerComponent.GetType();
                Component newComponent = System.Array.Find(newComponents, (c) => c.GetType() == componentType);

                // Copy the values from the old component to the new component
                UnityEditorInternal.ComponentUtility.CopyComponent(playerComponent);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(newComponent);
            }
        }

        // Destroy the current player object
        if (playerObject != null) {
            Destroy(playerObject);
        }

        // newObj Add PlayerScript
        
        // Set the playerObject to the new object
        playerObject = newObj;
    }

    public void ResetPlayerComponents(Player oldPlayer) {
        // Reset the values of all the components
        transform.position = oldPlayer.transform.position;
        transform.rotation = oldPlayer.transform.rotation;
        GetComponent<Rigidbody2D>().velocity = oldPlayer.GetComponent<Rigidbody2D>().velocity;
        // Set any other component values as needed
    }
#endif    
}