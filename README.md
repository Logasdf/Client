# Client
This is a simple third-person shooting game project.  
We are doing this with Unity 3D and C#.  

## Game Lobby
- ~~Connect to the server~~
    - Self-study on how to deal with IO with C#
    - **define PacketManager::Serialize function**
- ~~Draw UI for the lobby~~
- **Get the list of gamerooms from the server**
    - **Send the request to get the list of gamerooms right after the connection is established.**
    - ~~Display it in a scrollview or something~~
- **Create a new room when the user clicks "Create" button.**
    - ~~Create and show a modal window when clicked.~~
    - ~~Close the window when the cancel button is clicked~~
    - ~~The users will decide the name of the room and the number of people they want to play with in their room.~~
    - With the information, create a request message.
    1. **Request from the client.**
        + **Data to post: Room Name, Limits.**
    2. Processing on the server.
        + Room id 생성 -> <Room Name, Rood id>형태로 저장. (만약, Room Name이 존재할 경우, reject)
        + Room Object 생성 -> (왼쪽팀 1st, 방장, 인원수=1, limit={request.limit}, roomName={request.roomName}, 준비인원=1)
    3. **Send response to the client, then change UI(Lobby to Room UI) at the client.**
- **Enter a room.""
    1. **Request from the client.**
            + **Data to post: Room ID**
    2. Processing on the server.
        + 방인원에 따라 승인/거절
            -> 승인일 경우, Room ID에 해당하는 Room객체 상태 업데이트(Team Array에 추가, 방인원++, Team info를 Broadcast)
        + 이미 사라진 방일 경우 거절
    3. 서버로부터 받은 응답에 따라
        + **승인: Room UI로 변경**
        + **거절: 거절 응답 메시지 출력**
- ~~Create refresh button~~
    - Define a function for it
    
## Waiting Room (Before the game starts)

- A room can accommodate up to 16 people.
- "Game Start" button should be displayed only on the host's screen.
    - For others, display "Ready" button.
- Create a team-switch button, and define a function for it.
- **Push ready button.(After Entering the room)**
    1. **Client 준비표시 on/off -> 준비완료메시지(Room ID/Team info) 송신**
    2. Server는 Room ID에 해당하는 Room객체 상태 업데이트(준비인원++) -> Broadcast
    3. **Client UI 업데이트**
- Leave the room.
    1. Client와 Server는 준비이벤트와 유사
    2. 나간 사람이 방장일 경우.
        + 혼자일 경우 -> 방이 깨짐.
        + otherwise -> 방장 변경 -> Server에 요청, Room객체 상태 업데이트 및 다른 Client들에게 Broadcast
- Chatting....

## In-Game Design
