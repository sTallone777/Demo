# Asynchronous_Service
An example of implementing asynchronous functions on servers that do not support asynchronous functions

1.This is not a complete project. Only the key part of the code.

2.The basic process is：
  Start the websocket server　⇒　Establish a websocket connection when opening the page, and record the sessionId to identify different pages　⇒　The page executes tasks by opening new threads　⇒　Notify the websocket server through the callback function, and then the websocket server notifies the page display completion
