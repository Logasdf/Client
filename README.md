# Client
This is a simple third-person shooting game project.  
We are doing this with Unity 3D and C#.  

## Game Lobby
- ~~Connect to the server~~
- ~~Draw UI for the lobby~~
- **Get the list of gamerooms from the server**
    - Send the request to get the list of gamerooms right after the connection is established.
    - ~~Display it in a scrollview or something~~
- **Create a new room when the user clicks "Create" button.**
    - ~~Create and show a modal window when clicked.~~
    - ~~Close the window when the cancel button is clicked~~
    - ~~The users will decide the name of the room and the number of people they want to play with in their room.~~
    - With the information, create a request message.
- **Create a refresh button, and define a function for it**
    
## Waiting Room (Before the game starts)
- A room can accommodate up to 16 people.
- "Game Start" button should be displayed only on the host's screen.
    - For others, display "Ready" button.
- Create a team-switch button, and define a function for it.
## In-Game Design
