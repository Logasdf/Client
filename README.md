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
    - The room will be named simply like "Room #Number". -> not settled on this matter
    - user limit ? lock for a room?

## Waiting Room (Before the game starts)
- A room can accommodate up to 16 people.
- "Game Start" button should be displayed only on the host's screen.
    - For others, display "Ready" button.

## In-Game Design
