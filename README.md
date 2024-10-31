본 프로젝트는 유니티 엔진을 활용한 Fall Guys 모작입니다.

프로젝트 기간은 약 한달정도 소요되었으며, Photon을 이용한 멀티플레이와 WinSocket을 이용한 채팅 시스템을 구현하였습니다.

개발 인원은 1인입니다.

[플레이영상](https://www.youtube.com/watch?v=Mt5rhxtEc5g,"Youtube")
[ChatServer](https://github.com/JeonSeongWon95/FallguysChatServer.git,"GitHub")


# 구현한 기능

토글을 열어 **자세한 코드**를 확인하실 수 있습니다.


<details>
  <summary> GameManager(TitleScene, PlayScene, EndScene) </summary>

### 1. 부모(Base) GameManager 설계 및 상속
```cpp
    public enum eSkinColor
    {
        Red,
        Yellow,
        Purple,
        Blue,
        Green,
        White
    }

    public eSkinColor SkinColor;
    public Renderer CharacterRender;

    public void LoadSkin() 
    {
        ExitGames.Client.Photon.Hashtable properties = PhotonNetwork.LocalPlayer.CustomProperties;
        properties.TryGetValue("Skin", out SkinColor);
    }

    public void ChangeSkin()
    {

        switch (SkinColor)
        {

            case eSkinColor.Red:
                CharacterRender.material.color = Color.red;
                break;
            case eSkinColor.Yellow:
                CharacterRender.material.color = Color.yellow;
                break;
            case eSkinColor.Purple:
                CharacterRender.material.color = new Color(255, 0, 255);
                break;
            case eSkinColor.Blue:
                CharacterRender.material.color = Color.blue;
                break;
            case eSkinColor.Green:
                CharacterRender.material.color = Color.green;
                break;
            case eSkinColor.White:
                CharacterRender.material.color = Color.white;
                break;
            default:
                break;

        }
    }

    public void SetCharacterRender(GameObject NewCharacter)
    {
        CharacterRender = NewCharacter.GetComponent<GetCharacterRenderScript>().GetRender();
        ChangeSkin();
    }
```

### 2. Title Scene Game Manager : 포톤 네트워크 접속, 닉네임 설정, 로딩 및 PlayScene 전환
```cpp
    public enum eStep
    {
        Title,
        Lobby,
        Loading,
        InGame
    }

    private eStep gamestep = eStep.Title;
    public GameObject[] TitleHUD;
    public GameObject LobbyCharacter;
    public GameObject LoadingCharacter;
```
Title -> Lobby -> Loading -> InGame 순으로 순차적으로 진행되도록 Game Step 변수를 선언하였습니다.

```cpp
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ConnectToPhotonMasterServer();
            TitleHUD[(int)eStep.Title].SetActive(true);
        }
        else 
        {
            base.LoadSkin();
            gamestep = eStep.Lobby;
            PlayGameLogic();
        }
    }

    private IEnumerator ConnectToPhotonMasterServer()
    {

        for(int i = 0; i < 5; i++) 
        {
           PhotonNetwork.ConnectUsingSettings();

           yield return new WaitForSeconds(1f);

            if (PhotonNetwork.IsConnected)            
            {
                Connect = 1;
                yield break;
            }
        }

    }
```
시작되면 PhotonNetWork의 Master Server와 연결을 시도합니다. 5번의 연결 시도를 하며 코루틴을 사용하여 약간의 텀을 두었습니다.
연결이 성공되면 Title HUD의 배열 내 첫번째 UI(닉네임 설정)가 화면에 표시됩니다.

만약 게임이 끝나고 다시 Title로 온 경우 이미 닉네임을 설정하였으므로 Start의 else문의 코드가 실행됩니다.
부모 Class에 정의된 LoadSkin 함수를 호출하여 SkinColor 값을 가져와 저장하도록 하였습니다.

```cpp
    public void NextStep()
    {
      gamestep++;
      PlayGameLogic();
    }

    public void PlayGameLogic()
    {
        switch (gamestep)
        {
            case eStep.Lobby:
                JoinLobby();
                LobbyCharacter.SetActive(true);
                TitleHUD[(int)eStep.Lobby].SetActive(true);

                base.SetCharacterRender(LobbyCharacter);
                LobbyUIScript LobbyUIScr = TitleHUD[(int)eStep.Lobby].GetComponent<LobbyUIScript>();
                LobbyUIScr.LoadName();
                break;
        }
    }

    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }
    public void LoadName() 
    {
        PlayerName.text = PhotonNetwork.NickName;
    }

```
Photon의 Master Server와 연결이 되면 eStep 변수의 값이 증가하며 이에 맞는 Logic이 Swith/case문을 통해 실행됩니다.
JoinLobby 함수를 통해 Photon의 Default Lobby로 접속하며, LobbyCharacter와 LobbyUI의 Active가 활성화됩니다.

활성화된 캐릭터의 Skin을 바꾸어야 하므로 해당 GameObject의 Render Component를 가져오는 부모의 SetCharacterRender()함수가 실행됩니다.
Player가 기입한 닉네임을 보여주는 Lobby UI속 Name(Text)를 가져와 PhotonNetwork.NickName 변수 값을 받아 저장합니다.

```cpp
            case eStep.Loading:
                LobbyCharacter.SetActive(false);
                TitleHUD[(int)eStep.Lobby].SetActive(false);

                LoadingCharacter.SetActive(true);
                TitleHUD[(int)eStep.Loading].SetActive(true);

                base.SetCharacterRender(LoadingCharacter);
                JoinGameRoom();

                break;

    private void JoinGameRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("RoomOne", new RoomOptions { MaxPlayers = MaxClientCount }, null);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        UpdatePlayerUI();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdatePlayerUI();
    }

    private void UpdatePlayerUI()
    {
        if (TitleHUD[(int)eStep.Loading].activeSelf)
        {

            Text[] texts = TitleHUD[(int)eStep.Loading].GetComponentsInChildren<Text>();

            if (texts.Length > 0)
            {
                foreach (Text text in texts)
                {
                    if (text.name == "TotalPlayerCount_Text")
                    {
                        text.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
                        break;
                    }
                }

                if (MaxClientCount == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    NextStep();
                }
            }
        }
    }

```
Player가 Lobby UI의 Start Button을 누르면 Title GameManager의 NextStep 함수가 실행되며, eStep.Loading case가 실행됩니다.
기존의 Lobby용 캐릭터와 UI가 비활성화되며 Loading용 캐릭터와 UI가 활성화되며, CharacterRender 변수 값도 변경하였습니다.

JoinGameRoom 함수를 통해 PhotonNetwork의 Room을 생성(없는 경우) 또는 입장하도록 하였습니다.
Room Option 구조체를 통해 MaxPlayer 수를 설정된 3의 값으로 저장합니다.

방이 생성되거나 입장할때 Loading UI의 Player Count Text의 text 값을 갱신하여 현재 접속한 인원이 몇명인지 알 수 있도록 하였습니다.
이때 MaxPlayer 수에 도달하면 NextStep 함수가 실행되며, InGame Step의 코드가 실행됩니다.

```cpp
            case eStep.InGame:
                SaveUserData();
                StartCoroutine(StartGame());
                break;

    void SaveUserData()
    {
        ResetUserData();
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        if (customProperties.ContainsKey("Skin"))
        {
            customProperties["Skin"] = (int)SkinColor;
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable PlayerSkin = new ExitGames.Client.Photon.Hashtable();
            PlayerSkin["Skin"] = (int)SkinColor;
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerSkin);
        }
    }

    void ResetUserData()
    {
        ExitGames.Client.Photon.Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        if (customProperties.ContainsKey("IsWinner"))
        {
            customProperties.Remove("IsWinner");
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(8.0f);
        SceneManager.LoadScene("PlayScene");
    }
```
InGame Step에서는 User Data를 Scene이 넘어가기 전 저장합니다.
저장하기 전 기존에 데이터에서 승자 데이터를 지웁니다.

PhotonNetwork의 CustomProperties를 받아와 "IsWinner"를 인덱스로하여 데이터를 찾습니다.
만약 있는 경우 해당 데이터를 지우고 지운 HashTable을 CustomProperties로 설정합니다.

현재 선택된 SkinColor 값을 Photon.Network의 CustomProperties로 저장하였습니다.
이미 있는 경우에는 데이터를 바꾸고 저장하였으며, 없는 경우 새롭게 생성하여 할당하였습니다.

저장 및 초기화가 끝나면 StartGame 함수가 실행됩니다.

StartGame 함수는 코루틴을 사용하여 일정 시간 뒤 LoadScene을 호출합니다.

### 3. Play Scene Game Manager : 로컬 플레이어 설정, 시간 카운트 다운, 게임 종료 처리
```cpp
    void Start()
    {
        SpawnAndSetLocalPlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Goal", GoalPosition, Quaternion.identity);
        }
    }

    void SpawnAndSetLocalPlayer() 
    {
        Transform SpawnTransform = SpawnPosition[UnityEngine.Random.Range(0, SpawnPosition.Length)];
        Player = PhotonNetwork.Instantiate("Player", SpawnTransform.position, Quaternion.Euler(0, 90, 0));

        base.SetCharacterRender(Player);
        base.LoadSkin();
        base.ChangeSkin();

        ChatUI.SetActive(true);
        PlayerScript PlayerScr = Player.GetComponent<PlayerScript>();
        ChatUIScr = ChatUI.GetComponent<ChatUIScript>();
        ChatUIScr.SetPlayerScript(PlayerScr);
        PlayerScr.enabled = true;
        PlayerScr.LocalPlayerSet();
    }
```
PlayScene GameManager에서는 시작 시 Spawn Position에서 랜덤한 위치를 찾아 Player Character를 Spawn합니다.
Resource 폴더 안에 "Player" Prefab을 이용하여 생성하며, ChatUI도 활성화합니다.

닉네임 및 Player의 Input을 처리할 Player Script, 채팅을 화면에 표시할 Chat UI Script를 세팅해줍니다.
그리고 도착지는 Master Client 기준 하나만 생성하여 해당 위치를 Photon.View, Photon.Transform을 이용하여 동기화하였습니다.

```cpp
    void Update()
    {
        if (IsGameStart == false)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameStartTimer += Time.deltaTime;

                if (GameStartTimer >= 1.0f)
                {
                    photonView.RPC("RPC_CountDown", Photon.Pun.RpcTarget.All);
                    GameStartTimer = 0;
                }
            }

            if (Count == 0)
            {
                photonView.RPC("RPC_GameStart", Photon.Pun.RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void RPC_CountDown()
    {
        Count -= 1;
        CountDownUIScr.ChangeNumber(Count);

        if (Count <= 0)
        {
            IsGameStart = true;
        }
    }

    [PunRPC]
    public void RPC_GameStart() 
    {
        CountDownUIScr.gameObject.SetActive(false);
    }

```
기본 설정이 끝나면 Timer가 동작합니다.
Update문에서 각 Time을 더 해 Count Down을 시작하도록 하였습니다.
Master Client는 1초마다 Count Down UI의 Text 값을 수정하여 Count Down 및 모든 Client에게 동기화하며, Text값이 0이 되면 IsGameStart 변수가 True가 되고 게임이 시작됩니다.

```cpp
    IEnumerator EndGame() 
    {
        GameEndUI.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("EndScene");
    }

    [PunRPC]
    public void RPC_EndGame()
    {
       StartCoroutine(EndGame());
    }
```
Goal Object의 Collision에 충돌한 Player는 PlayerScene GameManager에 선언되어 있는 RPC_EndGame()함수를 호출합니다.
RPC_EndGame()함수는 코루틴 함수를 이용해 일정 시간 뒤 EndScene을 Load합니다.

### 4. End Scene Game Manager : 승자 확인 및 세팅, 게임 종료에 따른 방 떠나기 Title Scene Load
```cpp
    private Player Winner;
    public GameObject EndHUD;

    void Start()
    {
        EndHUD.SetActive(true);
        FindWinnerPlayer();

        StartCoroutine(GameEnd());
    }

    IEnumerator GameEnd() 
    {
        yield return new WaitForSeconds(8.0f);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Title");
    }

    void FindWinnerPlayer()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("IsWinner") && (bool)player.CustomProperties["IsWinner"])
            {
                Winner = player;
                break;
            }
        }

        if (Winner != null)
        {
            SetWinnerNickNameScript SWNN = EndHUD.GetComponent<SetWinnerNickNameScript>();
            SWNN.SetName(Winner.NickName);

            ExitGames.Client.Photon.Hashtable properties;
            properties = Winner.CustomProperties;
            properties.TryGetValue("Skin", out SkinColor);
            ChangeSkin();

        }
    }
```
End Scene에서는 시작과 동시에 End HUD를 활성화하고, PhotonNewwork.PlayerList를 순회하며 승리한 Player를 찾습니다.
Player CustomProperties에서 IsWinner라는 데이터가 있고, 해당 데이터가 True인 경우 모든 Client는 해당 Player를 Winner 변수에 저장합니다.

그리고 그 Player의 NickName을 End HUD의 Name Text에 세팅하고 해당 Player의 SkinColor 값도 가져와 End Character에 넣어주었습니다.

승자 설정 로직이 끝나면 코루틴이 동작하며 8초 뒤 방을 떠나고 다시 Title로 돌아갑니다.
</details>

<details>
  <summary> HUD(TitleScene, PlayScene, EndScene) </summary>

### 1. Title Scene HUD

+ Title UI : 초기 닉네임 설정
```cpp
    public InputField InputField_Name;
    public GameObject GameManager;

    public void OnClickConnectButton() 
    {
        if (InputField_Name == null)
            return;

        if(InputField_Name.text.Length > 0) 
        {
            if (GameManager == null)
                return;

            TitleGameManagerScript TitleGameManagerScr = GameManager.GetComponent<TitleGameManagerScript>();
            TitleGameManagerScr.SetUserName(InputField_Name.text);
            TitleGameManagerScr.NextStep();

            gameObject.SetActive(false);
        }

    }
```
Title UI에 Button을 Player가 누르면 Input Field에 작성된 내용을 확인하여 Title Game Manager의 User Name 변수에 저장합니다.
만약 InputField가 0보다 같거나 작은 경우는 저장하지 않도록 예외처리하였습니다.

+ Lobby UI : 옵션, 캐릭터 스킨, 종료 처리
```cpp
    public Text PlayerName;
    public GameObject Inventory;
    public GameObject OptionUI;
    public GameObject TitleGameManager;

    public void OnClickPlay() 
    {
        TitleGameManagerScript TGM = GameObject.Find("GameManager").GetComponent<TitleGameManagerScript>();
        TGM.NextStep();
    }
    public void OnClickChange()
    {
        if (!Inventory.activeSelf)
        {
            Inventory.SetActive(true);
        }
    }
    public void OnClickOption()
    {
        if (!OptionUI.activeSelf)
        {
            OptionUI.SetActive(true);
        }
    }

    public void OnClickExit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadName() 
    {
        PlayerName.text = PhotonNetwork.NickName;
    }
```
Lobby UI에는 각 Button마다 호출된 함수를 정의하였습니다.
게임 시작 버튼은 OnClickPlay, Change 버튼은 OnClickChange, Option 버튼은 OnClickOption, Exit 버튼은 OnClickExit 함수가 호출됩니다.

Option이나 Change 버튼은 별도의 UI 창이 추가로 활성화되며, Exit나 Start 버튼은 Application을 종료하거나 GameManaer의 NextStep 함수를 호출합니다.

  + Inven UI : 플레이어 Skin 변경
  ```cpp
      public TitleGameManagerScript TitleGameManagerScr;
  
      public void OnClickedRedSkin()
      {
          TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Red);
      }
      public void OnClickedGreenSkin()
      {
          TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Green);
      }
      public void OnClickedBlueSkin()
      {
          TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Blue);
      }
      public void OnClickedWhiteSkin()
      {
          TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.White);
      }
      public void OnClickedYellowSkin()
      {
          TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Yellow);
      }
      public void OnClickedPurpleSkin()
      {
          TitleGameManagerScr.SetNewSkin(TitleGameManagerScript.eSkinColor.Purple);
      }
      public void OnClickedExitButton() 
      {
          gameObject.SetActive(false);
      }
  ```
  인벤 UI는 플레이어가 선택할 수 있는 색별로 버튼이 있으며, 해당 버튼을 누르면 연결된 함수가 호출됩니다.
  Title Scene Game Manager의 Set New Skin함수에 인자로 본인의 색깔을 넘기며, 이를 바탕으로 base.SkinColor 변수의 값이 정해집니다.

  + Option UI : 해상도 변경
  ```cpp
    public Dropdown Resolution_DropDown;
    public int ResolutionData;
    private Resolution[] resolutions;
    private bool IsFullScreen = false;

      void Awake()
    {
        Resolution_DropDown.ClearOptions();
        resolutions = Screen.resolutions;

        foreach (Resolution resolution in resolutions)
        {
            float refreshRateValue = Convert.ToSingle(resolution.refreshRateRatio);

            if (refreshRateValue != 60.0f)
                continue;

            Dropdown.OptionData Data = new Dropdown.OptionData();
            Data.text = resolution.width + " X " + resolution.height;
            Resolution_DropDown.options.Add(Data);

            if (Screen.width == resolution.width && Screen.height == resolution.height)
            {
                Resolution_DropDown.value = Resolution_DropDown.options.Count - 1;
            }
        }

        RefreshDropDown();
    }

    public void RefreshDropDown() 
    {
        Resolution_DropDown.RefreshShownValue();
    }
  ```
  시작 시 Option UI의 DropDown을 모두 초기화하고 Resolution[] 배열로 선택할 수 있는 모든 해상도를 가져옵니다.
  가져온 배열을 foreach문을 통해 순회하며 주사율이 60인 경우 DropDown의 option으로 추가하도록 하였습니다.

  Option에 추가할 때 해당 해상도의 너비 + " X " + 높이 형태로 Text를 생성하여 넣었으며, 현재 스크린의 너비 높이와 같은 경우  DropDown의 Value값으로 넣었습니다.
  배열에서 삽입되는 데이터는 뒤에 추가하므로 Resolution_DropDown.options.Count - 1을 통해 가장 마지막 인덱스(가장 마지막으로 넣은 데이터)를 할당하였습니다.

  모든 데이터를 넣고 현재 해상도 값을 지정한 뒤 DropDown를 Refresh합니다.

  ```cpp
      public void ChangeResolution() 
    {
        Screen.SetResolution(resolutions[ResolutionData].width, resolutions[ResolutionData].height, IsFullScreen);
    }
  ```
  해상도를 선택하고 버튼을 누르면 현재 해상도를 선택된 해상도로 변경합니다.
  또한, IsFullScreen 변수 값을 확인하여 FullScreen도 설정해주도록 하였습니다.

### 2. Play Scene HUD : 닉네임, 플레이어 표시, 게임 종료

+ 플레이어 닉네임
```cpp
    public PhotonView PV;
    public Text PlayerName;

    void Start()
    {
        SetName();
    }
    void SetName() 
    {
        PlayerName.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
    }
```
서버로 요청하여 생성된 Player Prefab은 SetNickName Script를 갖으며 해당 Script에서 PhotonView를 확인합니다.
본인의 PV인 경우 본인의 닉네임, 아닌 경우 해당 PhotonView의 Owner의 닉네임을 불러옵니다.

```cpp
    void Update()
    {
        Vector3 Location = Camera.main.transform.position;
        Location.x *= -1;
        transform.LookAt(Location);
    }
```
닉네임은 Look At Camera Script를 갖으며, 해당 Script에서 main Camera의 위치를 가져와 해당 위치값을 바라보도록 하였습니다.

+ 플레이어 표시
```cpp
    public void LocalPlayerSet() 
    {
        CameraArm.SetActive(true);
        ArrowUI.SetActive(true);
    }
```
플레이어가 본인 캐릭터를 확인할 수 있는 화살표 UI는 초기 Play Scene Game Manager에서 Player Object 생성 후 Player Script의 Local Player Set 함수를 호출하면서 활성화합니다.

### 3. End Scene HUD : 승자 Nick Name 표시

```cpp
    public Text WinnerName;
    public void SetName(string NewName) 
    {
        WinnerName.text = NewName;
    }
```
End Scene Game Manager에서 FindWinnerPlayer함수를 통해 승자의 Name을 인자로 받아 모든 Client들이 해당 이름으로 Text 값을 할당합니다. 
</details>

<details>
  <summary> Character: 플레이어, 장애물, 목표 </summary>

### 1. Player : 움직임

+ Player 움직임
```cpp
   void FixedUpdate()
    {
        if (IsChatActive)
            return;

        if (GameManagerScr.GetIsGameStart())
        {
            LookAround();
            Move();
            SetCameraLocation();
        }

        if (IsJump && !IsJumping)
        {
            IsJumping = true;
            PlayerRigidbody.AddForce(Vector3.up * 8.0f, ForceMode.Impulse);
        }

        PlayerAnimator.SetBool("IsJump", IsJumping);
    }
```
캐릭터는 FixedUpdate 함수를 통해 회전, 이동, 카메라 이동 함수가 순차적으로 실행됩니다.

```cpp
    void Move()
    {
        Vector2 CharcterMove = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        bool IsMove = (CharcterMove.magnitude != 0);
        PlayerAnimator.SetBool("Walk", IsMove);

        IsRun = Input.GetButton("Run");
        IsJump = Input.GetButton("Jump");

        if (IsMove) 
        {
            Vector3 LookForward = new Vector3(CameraBody.forward.x, 0.0f, CameraBody.forward.z).normalized;
            Vector3 LookRight = new Vector3(CameraBody.right.x, 0.0f, CameraBody.right.z).normalized;
            Vector3 MoveDirection = (LookForward * CharcterMove.x) + (LookRight * CharcterMove.y);

            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection);
            CharacterBody.rotation = Quaternion.Slerp(CharacterBody.rotation, targetRotation, RotationSpeed * Time.deltaTime);

            if (CanMove)
            {
                PlayerRigidbody.MovePosition(Character.transform.position + MoveDirection 
                    * Time.fixedDeltaTime * (IsRun ? RunSpeed : WalkSpeed));
            }
        }

    }
```
움직임의 경우 Input.GetAxisRaw를 통해 Project에서 설정한 키값을 바탕으로 1, 0, -1 값을 받아 LookForward 변수에 저장합니다.
LookForward 벡터의 크기를 확인하여 0이 아닌 경우는 움직이는 경우이므로, 해당 비교 값을 Bool로 받아 Animator의 Walk 변수와 동기화하였습니다.

점프하거나, 달리는 경우도 미리 설정한 Run(Left Shift), Jump(Space) 값으로 받아옵니다.

카메라가 바라보는 방향을 기준으로 캐릭터가 이동하도록 구현할 것이므로 현재 카메라의 forward와 right 값을 가져옵니다.
(수평을 기준으로만 벡터를 가져와야 하므로 해당 벡터의 y값은 0.0으로 만들어 줍니다.)

이 벡터에 CharacterMove 벡터를 곱하여 카메라 정면 방향 기준 앞, 뒤 카메라 우측 방향 기준 좌, 우 값을 정해주었습니다.
이동할 방향으로 캐릭터가 회전하도록 Quaternion의 LookRotation을 통해 캐릭터가 회전해야하는 Quaternion 값을 저장하고, Slerp를 이용해 현재 Rotation에서 해당 Rotation으로 부드럽게 회전하도록 합니다.


```cpp
    void LookAround() 
    {
        Vector2 CameraMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 CamAngle = CameraBody.rotation.eulerAngles;

        float LimitX = CamAngle.x - CameraMove.y;

        if(LimitX < 180.0f) 
        {
            LimitX = Mathf.Clamp(LimitX, -1.0f, 70.0f);
        }
        else
        {
            LimitX = Mathf.Clamp(LimitX, 335.0f, 361.0f);
        }

        CameraBody.rotation = Quaternion.Euler(LimitX, CamAngle.y + CameraMove.x, CamAngle.z);
    }
```
카메라의 움직임은 마우스의 X, Y의 값과 각도를 가져옵니다.
(각도는 오일러 각도로 저장하였습니다.)

Camera의 X축의 회전 각도(위, 아래)에서 player가 입력한 Y축 값(위, 아래)을 가져와서 LimitX 값을 계산합니다.
LimitX 값이 180보다 작은 경우는 카메라가 캐릭터의 앞에 위치한 경우로 이때 카메라의 x축 각도는 -1.0 ~ 70.0f로 제한하였습니다.
반대로 큰 경우는 캐릭터의 뒤에 위치한 경우로 이때 카메라의 X축 각도를 335 ~ 361.0f로 제한하였습니다.

제한한 z축과 y축(현재 y축과 Player 입력에 따른 값을 더한 결과 값), z축(현재 z축)으로 카메라 object의 각도를 조절하였습니다.

### 2. 장애물 : 부셔지는 벽, 움직이는 장애물, 점프대, 망치 장애물

+ 부셔지는 벽
```cpp
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Transform PlayerTransform = collision.gameObject.transform;
            if (PlayerTransform == null)
                return;

            Rigidbody WallRigidbody = gameObject.GetComponent<Rigidbody>();
            if (WallRigidbody == null)
                return;

            Vector3 Direction = transform.position - PlayerTransform.position;
            Direction.Normalize();

            WallRigidbody.AddForce(Direction * 5.0f, ForceMode.Impulse);
        }
    }
```
부셔지는 벽은 충돌한 객체의 Tag를 확인하여 Player인 경우 해당 객체와의 반대 벡터를 계산하여 해당 방향으로 날아가도록 계산하였습니다.

+ 움직이는 장애물
```cpp
    enum Direction
    {
        Right,
        Left
    }

    public int Dir;
    public float Speed;
    private Direction dir = Direction.Right;

    void Start()
    {
        dir = (Direction)Dir;
    }


    void Update()
    {
        Vector3 Rot = dir == Direction.Left ? Vector3.up : Vector3.down;
        transform.Translate(Rot * Speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if (dir == Direction.Right)
            {
                dir = Direction.Left;
            }
            else 
            {
                dir = Direction.Right;
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            Transform PlayerTransform = collision.gameObject.transform;

            if (PlayerTransform == null)
                return;

            Rigidbody PlayerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (PlayerRigidbody == null)
                return;

            Vector3 Direction = PlayerTransform.position - transform.position;
            Direction.y = 0;
            Direction.Normalize();

            PlayerRigidbody.AddForce(Direction * 5.0f, ForceMode.Impulse);
        }
    }
```
움직이는 장애물은 시작 방향을 정할 enum class 변수를 하나 갖습니다.
시작 시 Start 함수에서 Editor에서 설정한 Int 값을 시작 방향 변수로 저장합니다.

FixedUpdate에서 방향 변수에 따라 정해진 방향으로 움직이도록 하였습니다.
만약 장애물 태그가 붙은 Object와 부딪히게 되면 방향을 바꾸어 이동하도록 하였습니다.

Player가 부딪힌 경우 Player의 transform과 Rigidbody를 받아와 부딪힌 방향과 정방대 벡터를 구해 해당 벡터로 5만큼 밀어내도록 하였습니다.



+ 점프대
```cpp
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody PlayerRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (PlayerRigidbody == null)
                return;

            PlayerRigidbody.AddForce(Vector3.up * 40.0f, ForceMode.Impulse);
        }
    }
```
점프대에는 점프가 되는 콜리전을 추가하여 총 2개의 콜리전으로 관리됩니다.
해당 콜리전에 겹쳐진 Object의 태그가 Player인 경우 해당 Object의 Rigidbody Component를 가져와 y축 방향으로 힘을 가하도록 하였습니다.

+ 망치 장애물
```cpp
    //Rotation Script
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.AngleAxis(-10.0f, Vector3.up);
        transform.rotation *= rotation;
    }

    //Hit Script
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody PlayerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (PlayerRigidbody == null)
                return;

            PlayerRigidbody.AddForce(transform.forward * 100.0f, ForceMode.Impulse);
        }
    }
```
망치 장애물은 FixedUpdate에서 계속해서 회전하도록 값을 설정하였으며, Collision을 갖는 Object를 자식 Object로 추가하여 해당 Object에서 충돌처리 하였습니다.
Collision의 태그를 확인하여 현재 망치 Object의 forward 방향으로 밀려나게 구현하였습니다.
  </details>

  <details>
  <summary> 채팅시스템 </summary>

### 1. Chat 서버 연결
```cpp
    void Start()
    {
        if (ConnectServer())
        {
            isConnected = true;
            ReceiveMessage();
        }
    }

    bool ConnectServer()
    {
        Int32 port = 7777;
        string host = "127.0.0.1";

        client = new TcpClient(host, port);
        stream = client.GetStream();

        if (stream == null)
        {
            return false;
        }

        return true;
    }
```
Socket Script는 시작 시 WinSocket으로 만든 Chat Server에 접속을 시도합니다.
루프백 주소와 포트번호 7777를 저장하는 새로운 TcpClient 객체를 생성 및 연결합니다.

### 2. Chat Message 발신, 수신
```cpp
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!IsActiveChat)
            {
                PlayerScr.SetIsChatActive(true);
                InputMessage();
            }
            else
            {
                PlayerScr.SetIsChatActive(false);
                SendMessage();
            }
        }
    }
   void InputMessage()
   {
       InputField_Message.ActivateInputField();
       IsActiveChat = true;
   }
```
Chat UI Script에서 Enter 입력을 확인하여 채팅창을 활성화합니다.
Player Script에서는 채팅창이 활성화 되면 움직임을 막으며, Input Field를 활성화합니다.

```cpp
    void SendMessage()
    {
        string Message = PhotonNetwork.NickName;
        Message += "950";
        Message += InputField_Message.text;
        socketScr.SendMessageToServer(Message);
        IsActiveChat = false;
        InputField_Message.text = "";
        InputField_Message.DeactivateInputField();
    }
```
다시 Enter키를 누르면 채팅창이 비활성화되며 Send Message 함수가 실행됩니다.
자기 자신의 NickName을 가져와 NickName의 끝 체크용으로 950 문자를 추가합니다.
그 다음 Input Field에 입력된 데이터를 추가하며, Input field는 다시 비우고 비활성화 하도록 하였습니다.

```cpp
    public async void SendMessageToServer(string ChatMessage)
    {
        if (ChatMessage == "")
            return;

        byte[] dataToSend = Encoding.ASCII.GetBytes(ChatMessage);

        try
        {
            await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error sending message: {ex.Message}");
        }
    }
```
앞서 닉네임 + 950 + 채팅메세지를 하나로 합친 String은 Socket Script의 Send Message To Server 함수의 인자로 전달합니다.
전달 받은 내용이 없는 경우 전송하지 않으며, 해당 String은 아스키 인코딩을 사용하여 바이트 배열로 전환 및 Win Socket Server로 전달합니다.

try/catch 문을 이용하여 정상적으로 함수가 종료되지 않는 경우 에러 메세지를 확인할 수 있도록 로그 출력을 하며, async 키워드를 이용하여 비동기 방식으로 채팅을 보내도록 하였습니다.
채팅을 주고 받는 것은 비동식으로 처리하여 메인 스레드가 블록되지 않도록 해야할 것이라 생각하였습니다.

```cpp
    async void ReceiveMessage()
    {
        byte[] Buffer = new byte[1024];

        while (true)
        {
            int RecvCount = await stream.ReadAsync(Buffer, 0, Buffer.Length);

            if (RecvCount > 0)
            {
                string Message = Encoding.ASCII.GetString(Buffer, 0, RecvCount);
                string NickName = "";
                string text = "";

                for (int i = 0; i < Message.Length; ++i) 
                {
                    if(Message[i] == '9' && Message[i+1] == '5' && Message[i+2] == '0') 
                    {
                        for(int j = 0; j < i; ++j) 
                        {
                            NickName += Message[j];
                        }
                        for(int k = i + 3; k < Message.Length; ++k) 
                        {
                            text += Message[k];
                        }

                        ChatUIScr.AddText(NickName, text);
                        break;
                    }
                }
            }
            else
            {
                break;
            }

        }
    }
```
메세지를 받는 것은 Socket Script에서 Server에 연결됨과 동시에 비동기로 호출됩니다.
해당 함수에서는 While문을 통해 계속해서 recv를 시도합니다.

다만 await 키워드를 사용하여 해당 함수가 완료될 때까지 대기하게 하여 채팅 메세지가 오는 것을 기다리도록 하였습니다.
전달 받은 Message의 크기가 0보다 작거나 같은 경우에는 반복문이 종료되며 채팅 수신이 종료됩니다.

아니라면 받은 데이터를 아스키를 사용하여 string으로 다시 디코딩해서 가져옵니다.
950을 기준으로 앞의 인덱스까지는 Name으로, 뒤의 인덱스는 Message로 나눠서 Chat UI Script의 Add Text 합수 인자로 전달합니다.

```cpp
    public void AddText(string Nickname, string Massge)
    {
        GameObject CreateTextBox = Instantiate(TextBox);
        Text FindTextBox =  CreateTextBox.GetComponent<Text>();
        FindTextBox.text = Nickname + " : " + Massge;
        CreateTextBox.transform.SetParent(ChatBox.transform);
    }
```
Add Text함수에서는 전달 받은 Name과 Message를 Text Component가 있는 GameObejct를 생성하여 할당합니다.
그리고 생성한 GameObject를 Chat UI의 Scroll View -> Content 안에 자식 오브젝트로 추가합니다.
