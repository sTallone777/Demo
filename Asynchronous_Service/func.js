var ws = null;
var clientID = null;
var wsUrl = "ws://localhost:8113/ws";

function createWsConn(){
    clientID = document.getElementById("it1::content").value;
    if(ws == null) ws = new WebSocket(wsUrl);

    ws.onopen = function() {
        console.log("Open websocket connection: " + wsUrl);
        ws.send("OPEN_" + clientID);
    }
    ws.onclose = function() {
        console.log("Close websocket connection: " + wsUrl);
        ws = null;
    }
    ws.onmessage = function(e) {
        if(e.data != "ping"){
            console.log("Receive message: " + e.data);
            try{
                var dlButton = AdfPage.PAGE.findComponentByAbsoluteId(e.data);
                var dlEvent = new AdfActionEvent(dlButton);
                dlButton.queueEvent(dlEvent,true);
            }catch(err){
                console.log(err);
            }
        }
    }
    ws.onerror = function(e) {
        console.log("Websocket connection error: " + e.data);
    }
}

window.addEventListener('beforeunload', function(){
    ws.send("CLOSE_" + clientID);
});