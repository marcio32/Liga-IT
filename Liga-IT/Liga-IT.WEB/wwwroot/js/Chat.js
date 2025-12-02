let connection;
let currentRoomId = "1";
let currentUser;
let isConnected = false;

document.addEventListener("DOMContentLoaded", function () {
    const sendBtn = document.getElementById("sendButton");
    const connectBtn = document.getElementById("connectButton");
    const messageInput = document.getElementById("messageInput");
    
    sendBtn.addEventListener("click", sendMessage);
    connectBtn.addEventListener("click", toggleConnection);
    messageInput.addEventListener("input", handleTyping);
});

let isTyping = false;
function handleTyping() {
    const messageInput = document.getElementById("messageInput");
    if (connection && isConnected) {
        if (messageInput.value.trim() && !isTyping) {
            isTyping = true;
            connection.invoke("NotifyTyping", currentRoomId, currentUser);
        } else if (!messageInput.value.trim() && isTyping) {
            isTyping = false;
            connection.invoke("CanceledNotifyTyping", currentRoomId, currentUser);
        }
    }
}

async function toggleConnection() {
    const connectBtn = document.getElementById("connectButton");
    if (isConnected) {
        await disconnect();
        connectBtn.innerHTML = '<i class="fas fa-plug"></i> Conectar';
        connectBtn.classList.remove("btn-disconnect");
        connectBtn.classList.add("btn-connect");
    } else {
        await connect();
        connectBtn.innerHTML = '<i class="fas fa-plug-circle-xmark"></i> Desconectar';
        connectBtn.classList.remove("btn-connect");
        connectBtn.classList.add("btn-disconnect");
    }
}

async function disconnect() {
    if (connection) {
        await connection.invoke("LeaveRoom", currentRoomId, currentUser);
        await connection.stop();
        isConnected = false;
        document.getElementById("messageInput").setAttribute("disabled", "true");
        document.getElementById("sendButton").setAttribute("disabled", "true");
    }
}

async function connect() {
    const userInput = document.getElementById("userInput");
    currentUser = userInput.value.trim() || "Anonimo";
    connection = new signalR.HubConnectionBuilder().withUrl(`chatHub?user=${currentUser}`).withAutomaticReconnect().build();
    
    connection.on("ReceiveMessage", (userName, message, timestamp) => {
        addMessage(userName, message, timestamp);
    });
    
    connection.on("UserTyping", (userName) => {
        showTypingIndicator(userName);
    });
    
    connection.on("UserStoppedTyping", (userName) => {
        hideTypingIndicator(userName);
    });
    
    connection.on("UserCanceledTyping", (userName) => {
        hideTypingIndicator(userName);
    });
    
    await connection.start();
    isConnected = true;
    await connection.invoke("JoinRoom", currentRoomId, currentUser);
    
    document.getElementById("messageInput").removeAttribute("disabled");
    document.getElementById("sendButton").removeAttribute("disabled");
}

function showTypingIndicator(userName) {
    const chatBox = document.getElementById("chatBox");
    let indicator = document.getElementById(`typing-${userName}`);
    if (!indicator) {
        indicator = document.createElement("div");
        indicator.id = `typing-${userName}`;
        indicator.className = "typing-indicator";
        indicator.innerHTML = `<em>${userName} está escribiendo...</em>`;
        chatBox.appendChild(indicator);
        chatBox.scrollTop = chatBox.scrollHeight;
    }
}

function hideTypingIndicator(userName) {
    const indicator = document.getElementById(`typing-${userName}`);
    if (indicator) {
        indicator.remove();
    }
}

async function sendMessage() {
    const messageInput = document.getElementById("messageInput");
    const message = messageInput.value.trim();
    if (message && connection && isConnected) {
        if (isTyping) {
            isTyping = false;
            await connection.invoke("CanceledNotifyTyping", currentRoomId, currentUser);
        }
        await connection.invoke("SendMessageToRoom", currentRoomId, currentUser, message);
        addMessage(currentUser, message, new Date());
        messageInput.value = "";
    }
}

async function addMessage(user, message, timestamp) {
    const chatBox = document.getElementById("chatBox");
    const messageDiv = document.createElement("div");
    messageDiv.className = "message";
    const time = new Date(timestamp).toLocaleTimeString('es-AR', { hour: '2-digit', minute: '2-digit' });
    messageDiv.innerHTML = `<div class="message-user">${user} <span class="message-time">${time}</span></div><div class="message-text">${message}</div>`;
    chatBox.appendChild(messageDiv);
    chatBox.scrollTop = chatBox.scrollHeight;
}