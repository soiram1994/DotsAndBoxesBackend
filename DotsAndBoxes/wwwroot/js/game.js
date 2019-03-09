"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gamehub").build();
var lineDrawn = function (line) {
    var startdot = line.startdot;
    var enddot = line.enddot;
    var li = document.createElement("li");
    li.textContent = `line with start point:(${startdot.x},${startdot.y}) and end point:(${enddot.x},${enddot.y}) has been drawn`;
    document.getElementById("messagesList").appendChild(li);
};
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
document.getElementById("joinButton").disabled = true;


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("StartGame", function (dimension) {
    
    var li = document.createElement("li");
    li.textContent = ` Board with dimension:${dimension} has been created`;
    document.getElementById("messagesList").appendChild(li);
});
connection.on("EnableJoin", function () {
    var btn = document.getElementById("joinButton");
    btn.disabled = false;
});
connection.on("DrawAndPlay", function (line) {
    lineDrawn(line);

});
connection.on("WaitingPlayer2", function () {
    var li = document.createElement("li");
    li.textContent = "Waiting for a player to join....";
    document.getElementById("messagesList").appendChild(li);

});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var dimension = document.getElementById("messageInput").value;
    //var message = document.getElementById("messageInput").value;
    connection.invoke("NewGame", dimension).catch(function (err) {
        return console.error(err.toString());
    });
    
    event.preventDefault();
});

document.getElementById("joinButton").addEventListener("click", function (event){
    connection.invoke("JoinGame").catch(function (err) {
        return console.error(err.toString());
    });
});