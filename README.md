# Client
This is a simple third-person shooting game project.  
We are doing this with Unity 3D and C#.  

## Game Lobby
- Connect to the server
    - Self-study on how to deal with IO with C#
        - [AsyncIO c# example](https://docs.microsoft.com/ko-kr/dotnet/framework/network-programming/asynchronous-client-socket-example)
    - define PacketManager::Serialize function
- Draw UI for the lobby
- Get the information for the initialization on the client side.
    - Get the roomlist and display it in a scrollview
    - Get the assigned username from the server and store it.
- Create a new room when the user clicks "Create" button.
    - Create and show a modal window when clicked.
    - Close the window when the cancel button is clicked
    - The users will decide the name of the room and the maximum number of people they want to play with in their room.
    - With the information, create a request message.
        - Request from the client.
            1. Data to post: Room Name, Limits, User Name.
        - If the request was accepted, Change the values in RoomContext instance using the data from server, and call LoadScene().
        - If the request was declined, Display the error message window.
- Enter a room.
    - Request from the client.
            - Data to post: Room Name, User Name.
    - If the request was accepted, Change the values in RoomContext instance using the data from server, and call LoadScene().
    - If the request was declined, Display the error message window.
- Create refresh button
    - Send the request and get the roomlist.
    
## Waiting Room (Before the game starts)

- A room can accommodate up to 16 people.
- "Start" button should be displayed only on the host's screen.
    - For others, display "Ready" button.
- Push ready button.(After Entering the room)
    - Send the request with the "READY_EVENT" header-type.
    - When the response from the server arrives, update UI.
- Team Switch
    - Send the request with the "TEAM_CHANGE" header-type.
    - When the response from the server arrives, update UI.
- Leave the room.
    - Send the request with the "LEAVE_GAMEROOM" header-type.
    - Call LoadScene() and go to the lobby scene.
- Chat
    - Send the data { type : CHAT_MESSAGE , data : username + chat msg }
    - When the response from the server arrvies, update UI.
    
## In-Game

## ScreenShots
