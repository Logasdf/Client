using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// 이 클래스는 게임로비(대기실아님) UI 처리를 담당하는 클래스

public class LobbyUIMgr : MonoBehaviour {

    //아래 세 변수는 게임 방 목록 관련 로직 처리할 때 필요한 객체들을 
    //유니티에서 직접 연결하기 위해서 public으로 미리 선언

    public GameObject modalWindowPrefab; // 방만들기 할 때 등장할 창
    public GameObject roomItem; // 방 목록에서 방 하나에 해당하는 객체 ( 방제목 , 방인원 )
    public GameObject scrollContent; // 방 목록이 표시 될 스크롤 패널
    
    public void RequestCreateModalWindow() // 방만들기 버튼 이벤트 리스너가 이 함수를 호출함
    {
        CreateModalWindow();
    }
    
    private Color selectedRoomColor = Color.gray; // 방 목록에 커서 올리거나 할 때 색 변경되는 거 처리할 때 쓰일 색상들
    private Color unselectedRoomColor = Color.black;
    private Color nameFieldBlankColor = Color.red;

    private void Start()
    {
        // Start()함수는 이 스크립트를 포함한 오브젝트가 만들어질 때 실행이 되는데 
        // 현재는 방목록을 아직 서버에서 받아오지 않기 때문에 방목록을 뿌리는 함수를 여기서 실행해서 확인중임.
        ShowRoomList(); 
    }

    private void InitEventTriggerForRoomItem(EventTrigger trigger, GameObject obj)
    {
        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter //마우스 커서가 이 이벤트트리거가 붙은 객체 영역으로 들어올 때
        };                                          //아래의 리스너함수를 호출해주세요..
        entry_PointerEnter.callback.AddListener((data) => { ChangeObjectBgColor(obj, selectedRoomColor); });

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerExit //객체 영역을 벗어날 때
        };                                         //아래의 리스너함수를 호출해주세요..
        entry_PointerExit.callback.AddListener((data) => { ChangeObjectBgColor(obj, unselectedRoomColor); });

        /* EventTrigger는 해당 객체에 발생할 이벤트들 중에서 처리하고 싶은 이벤트만 골라서
         * 담는 통 정도로 보면 편할 것 같구, 필요한 이벤트들은 Entry로 만들어서 추가하는 방식
         */
        trigger.triggers.Add(entry_PointerEnter);
        trigger.triggers.Add(entry_PointerExit); 
    }

    private void ShowRoomList() // 방 목록을 뿌려주는 함수
    {
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject); // 함수가 실행될 때마다 이 전에 존재했던 방 오브젝트들을 지우는 작업
        }
            
        for (int i = 0; i < 10; i++)
        {
            GameObject room = Instantiate(roomItem); // unity로 외부에서 연결해놓은 prefab을 실제 오브젝트로 동적생성
            var textFields = room.GetComponentsInChildren<Text>(); // room은 방이름과 방인원수를 표시하는 textfield 2개로 구성되어 있는데 그거 가져오는 거
            textFields[0].text = "Room #" + (i + 1); // 방 이름 수정하는 코드 (textFields[0] == 방 이름 텍스트필드)
            room.GetComponent<Button>().onClick.AddListener(delegate {
                OnClickRoomItem(textFields[0].text); // 방 하나를 나타내는 prefab에는 버튼 속성도 부여했는데 방 입장처리를 위함임, 누르면 함수 실행
            });
            EventTrigger eTrigger = room.AddComponent<EventTrigger>(); // 처리하고 싶은 이벤트들 담을 통을 준비해서
            InitEventTriggerForRoomItem(eTrigger, room); // 필요한 것만 추가할게요...
            room.transform.SetParent(scrollContent.transform, false); // 해당 방 오브젝트를 스크롤 패널에 자식으로 등록해서 올리는 과정
        }
    }

    private void CreateModalWindow() // 방 만들기 창
    {
        GameObject modalWindow = Instantiate(modalWindowPrefab); // 외부에서 연결한 prefab을 실제 오브젝트로 동적 생성
        GameObject dialogWindow = modalWindow.transform.GetChild(0).gameObject; 
        // modalWindow prefab을 보면 검은 배경(부모)에 방 만들기 대화상자가 자식의 형태로 들어가있걸랑
        // dialogWindow 변수는 방 만들기 대화상자만을 저장하고 있음. 아래 코드에서 확인할 수 있듯이 버튼을 찾기 위함..

        Button submitBtn = dialogWindow.transform.Find("SubmitBtn").gameObject.GetComponent<Button>();
        Button cancelBtn = dialogWindow.transform.Find("CancelBtn").gameObject.GetComponent<Button>();

        submitBtn.onClick.AddListener(delegate { SubmitCreateRoomRequest(dialogWindow); }); // 확인 눌렀을 때
        cancelBtn.onClick.AddListener(delegate { Destroy(modalWindow); }); // 취소버튼 눌렀을 때

        GameObject canvas = GameObject.Find("Canvas"); // 현재 보이는 화면에서 가장 최상위 오브젝트
        modalWindow.transform.SetParent(canvas.transform, false); // 방만들기 화면을 표시하게 하고
        modalWindow.transform.SetAsLastSibling(); // z-index 관련해서 가장 위로 올라오게 마지막 자식으로 설정.. 근데 이거 위치가 맞는지 모르겠음
    }

    private void SubmitCreateRoomRequest(GameObject window) // 방 만들기 대화상자에서 확인버튼 누르면 실행
    {
        //TODO : Send CREATE ROOM REQUEST to the server and Get the response.
        InputField nameField = window.transform.Find("NameInputField").gameObject.GetComponent<InputField>(); //방이름인풋필드
        string roomName = nameField.text; // 방이름 추출

        if(roomName.Trim() == "") // 방이름 아무것도 입력안했거나, 빈칸만 줄창 입력했으면
        {
            nameField.GetComponent<Image>().color = nameFieldBlankColor; // 해당 인풋필드 색깔 빨간색으로 바꿔서 거절됨을 표시
            return;
        }

        Dropdown limitDropdown = window.transform.Find("LimitDropdown").gameObject.GetComponent<Dropdown>(); // 인원 드롭다운
        string selectedVal = limitDropdown.options[limitDropdown.value].text; // 선택 값 추출

        Debug.Log("Room name : " + roomName + ", and the selected value is " + selectedVal);
        SceneManager.LoadScene("WaitingRoom");
    }

    private void OnClickRoomItem(string roomName) // 방 입장
    {
        /*
         * TODO : Send a message to the server and get the response.
         * With that response, the appropriate code block will be executed.
        */
        Debug.Log("element clicked : " + roomName);
        //SceneManager.LoadScene("WaitingRoom");
    }

    private void ChangeObjectBgColor(GameObject obj, Color color) // 해당 오브젝트의 배경색상 변경
    {
        obj.GetComponent<Image>().color = color;
    }
}
