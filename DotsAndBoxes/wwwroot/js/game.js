//!!!!!This a js file with the sole purpose of testing the functionality of the hub
//for no reason should it be considered a dots and boxes game implementation.!!!!!!!
//-----Summary-------
//It sets up the connection with the hub, triggers some of its methods, and has some dummy implementations.
//Every time something that has an effect on the game happens, a new li element is created and added in 
//the messages list


"use strict";
//connection setup
var connection = new signalR.HubConnectionBuilder().withUrl("/gamehub").build();
//Help functions
//"Draws" line
var lineDrawn = function (line) {
    var startdot = `(${line.startdot.x},${line.startdot.y})`;
    var enddot = `(${line.enddot.x},${line.enddot.y})`;
    var li = document.createElement("li");
    li.textContent = `line with start point:${startdot} and end point:${enddot} has been drawn`;
    document.getElementById("messagesList").appendChild(li);
};
//Add li element
var addListElement = function (message) {
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);

};
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
//Only if a session is actually availiable, it is enabled
document.getElementById("joinButton").disabled = true;

//establishes the connection
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
//connection.on methods 
//When both players are active this method is called to start the game
connection.on("StartGame", function (dimension) {
 
    var message = `Board with dimension:${dimension} has been created`;
    addListElement(message);
    var startingControls = document.getElementById("startControls");
    var gameControls = document.getElementById("gameControls");
    startingControls.remove();
    gameControls.setAttribute("style", "visibility:visible");
});
//If a player is already active and waiting, the join button is enabled
connection.on("EnableJoin", function () {
    var btn = document.getElementById("joinButton");
    btn.disabled = false;
});
//When a player makes a move is made and is this player's turn
connection.on("DrawAndPlay", function (line) {
    lineDrawn(line);
    document.getElementById("playButton").disabled=false;
});
//When a move is made but it isnt this player's turn
connection.on("DrawAndWait", function (line) {
    lineDrawn(line);
    document.getElementById("playButton").disabled = true;
});
//Meant for the beggining of the game so that player2 cant play until player1 has made his/her move
connection.on("DisablePlayButton", function () {
    document.getElementById("playButton").disabled = true;
});
//When a new game is created
connection.on("WaitingPlayer2", function () {

    var message = "Waiting for a player to join....";
    addListElement(message);
});
//When a player disconnects form an active game
connection.on("GameAborted", function () {
    confirm("Your rival has fled, you stand victorious");
    document.location.reload(true);

});
//When there are no active games
connection.on("DisableJoin", function () {
    document.getElementById("joinButton").disabled = true;

});
//When a move is invalid
connection.on("InvalidMove", function () {
    addListElement("invalid move");
});
//When a winner is decided
connection.on("WinnerDecided", function (message) {

    alert(`${message}`);
    
    document.location.reload();
});
//Events
//When a player clicks create game
document.getElementById("sendButton").addEventListener("click", function (event) {
    var dimension = document.getElementById("messageInput").value;
    connection.invoke("NewGame", dimension).catch(function (err) {
        return console.error(err.toString());
    });
    
    event.preventDefault();
});
//When a player presses the join button
document.getElementById("joinButton").addEventListener("click", function (event){
    connection.invoke("JoinGame").catch(function (err) {
        return console.error(err.toString());
    });
});
//When the play button is pressed
document.getElementById("playButton").addEventListener("click", function (event) {
    var startX = document.getElementById("startPointX").value;
    var startY = document.getElementById("startPointY").value;
    var endX = document.getElementById("endPointX").value;
    var endY = document.getElementById("endPointY").value;
    var startDot = { x: startX, y: startY };
    var endDot = { x: endX, y: endY };
    var line = { startdot: startDot, enddot: endDot };
    connection.invoke("DrawLine", line);
});