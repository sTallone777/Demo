package main

import (
	"log"
	"net/http"
	"strings"

	"github.com/gorilla/websocket"
)

//cross domain
var upgrader = websocket.Upgrader{
	ReadBufferSize:  1024 * 10,
	WriteBufferSize: 1024 * 10,
	CheckOrigin: func(r *http.Request) bool {
		return true
	},
}

var clientList map[string]*websocket.Conn

func checkClose() {
	for {
		for k, v := range clientList {
			if err := v.WriteMessage(websocket.TextMessage, []byte("ping")); err != nil {
				//send failed
				v.Close()
				delete(clientList, k)
				log.Println("Connect: ", k, " is closed.")
				log.Println("Now connection count: ", len(clientList))
			}
		}
	}
}

func wsHandler(w http.ResponseWriter, r *http.Request) {
	var data []byte
	var clientID string

	conn, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Print("Upgrade connection error:", err)
		return
	}

	for {
		if _, data, err = conn.ReadMessage(); err != nil {
			goto ERR
		}
		msg := string(data[:])
		if strings.HasPrefix(msg, "OPEN_") {
			clientID = strings.Replace(msg, "OPEN_", "", -1)
			//if _, ok := clientList[clientID]; !ok {
			clientList[clientID] = conn
			log.Println("New connection join: " + clientID)
			log.Println("Now connection count: ", len(clientList))
			//}
		}
		if strings.HasSuffix(msg, "CLOSE_") {
			clientID = strings.Replace(msg, "CLOSE_", "", -1)
			delete(clientList, clientID)
			log.Println("Connect: ", clientID, " is closed.")
			log.Println("Now connection count: ", len(clientList))
		}
	}

ERR:
	conn.Close()
}

func receiveResult(w http.ResponseWriter, r *http.Request) {
	id := r.URL.Query().Get("clientID")
	ctlID := r.URL.Query().Get("ctlId")

	if v, ok := clientList[id]; ok {
		if err := v.WriteMessage(websocket.TextMessage, []byte(ctlID)); err != nil {
			log.Println("Send message error:", err)
			v.Close()
		} else {
			log.Println("Send message to Client: " + id)
		}
	}
	// select {
	// case c := <-clients:
	// 	if c.clientID == id {
	// 		err := c.clientConn.WriteMessage(websocket.TextMessage, []byte(ctlID))
	// 		if err != nil {
	// 			log.Println("write:", err)
	// 			break
	// 		}
	// 		log.Println("Send message to Client: " + strconv.Itoa(c.clientID))
	// 	}
	// }
}

func main() {
	clientList = make(map[string]*websocket.Conn)
	//go checkClose()

	http.HandleFunc("/ws", wsHandler)
	http.HandleFunc("/rule", receiveResult)
	log.Println("Listening port: 8113")
	http.ListenAndServe(":8113", nil)
}
