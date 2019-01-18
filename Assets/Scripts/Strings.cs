

public static class PathStrings
{
    //PREFABS PATH
    public const string CREATE_ROOM_PANEL = "Prefabs/CreateRoomPanel";
    public const string ERROR_MESSAGE_PANEL = "Prefabs/ErrorMessagePanel";
    public const string ROOMITEM = "Prefabs/RoomItem";
    public const string EACHUSER = "Prefabs/EachUser";

    //SCENES
    public const string SCENE_GAMELOBBY = "GameLobby";
    public const string SCENE_WAITINGROOM = "WaitingRoom";
    public const string SCENE_INGAME = "InGame";
}

public static class ElementStrings
{
    //GLOBAL
    public const string CANVAS = "Canvas";
    public const string PACKETMANAGER = "PacketManager";
    public const string MAINCAMERA = "Main Camera";

    //Lobby
    public const string LOBBYUIPAINTER = "LobbyUIPainter";

    //CreateRoom-related strings
    public const string ROOMNAME_INPUTFIELD = "NameInputField";
    public const string ROOMLIMIT_DROPDOWN = "LimitDropdown";
    public const string SUBMIT_BTN = "SubmitBtn";
    public const string CANCEL_BTN = "CancelBtn";

    //ErrorMessageWindow-related strings
    public const string ERRORMESSAGE_WINDOW = "ErrorWindow";
    public const string ERRORMESSAGE_TEXTFIELD = "ErrorMessage";
    public const string CLOSE_BTN = "CloseBtn";

    //WaitingRoom
    public const string WAITINGROOMUIPAINTER = "WaitingRoomUIPainter";

    public const string REDTEAMLIST = "RedTeamList";
    public const string BLUETEAMLIST = "BlueTeamList";
    public const string CHAT_INPUTFIELD = "ChatMessage";
    public const string USERNAME_PANEL = "UserName";
    public const string READY_BTN = "ReadyBtn";
    public const string START_BTN = "StartBtn";

    //InGame
    public const string KDASTATE = "KDA";
    public const string MYTEAMTAG = "MYTEAM";
    public const string ENEMYTAG = "ENEMY";
}


public static class MessageTypeStrings
{
    public const string DATA = "Data";
    public const string INT32 = "Int32";
    public const string CONTENT_TYPE = "contentType";
    public const string USERNAME = "userName";

    //LOBBY
    public const string ASSIGN_USERNAME = "ASSIGN_USERNAME";
    public const string ROOMLIST = "RoomList";
    public const string ROOMINFO = "RoomInfo";
    public const string ERRORMESSAGE = "errorMessage";

    public const string CREATE_ROOM = "CREATE_ROOM";
    public const string ENTER_ROOM = "ENTER_ROOM";
    public const string REJECT_CREATEROOM = "REJECT_CREATE_ROOM";
    public const string REJECT_ENTERROOM = "REJECT_ENTER_ROOM";

    public const string ROOMNAME = "roomName";
    public const string LIMIT = "limits";

    //WAITING ROOM
    public const string LEAVE_GAMEROOM = "LEAVE_GAMEROOM";
    public const string TEAM_CHANGE = "TEAM_CHANGE";
    public const string START_GAME = "START_GAME";
    public const string READY_EVENT = "READY_EVENT";
    public const string HOST_CHANGED = "HOST_CHANGED";
    public const string CHAT_MESSAGE = "CHAT_MESSAGE";

    public const string CLIENT = "Client";
    public const string ROOMID = "roomId";
    public const string POSITION = "position";
    public const string TOREADY = "toReady";
    public const string PREV_POSITION = "prev_position";
    public const string NEXT_POSITION = "next_position";
    public const string CHAT_STRING = "message";
    public const string NEWHOST = "newHost";

    //InGame
    public const string FIRE_BULLET = "FIRE_BULLET";
    public const string BE_SHOT = "BE_SHOT";

    public const string PLAYSTATE = "PlayState";
    public const string MESSAGEFROM = "fromClnt";
    public const string MESSAGETO = "toClnt";

}
