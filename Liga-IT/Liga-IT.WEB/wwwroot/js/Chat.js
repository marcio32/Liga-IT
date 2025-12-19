let isConnected = false;

document.addEventListener('DOMContentLoaded', function() {
    const userInput = document.getElementById('userInput');
    const messageInput = document.getElementById('messageInput');
    const connectButton = document.getElementById('connectButton');
    const sendButton = document.getElementById('sendButton');
    const chatBox = document.getElementById('chatBox');

    connectButton.addEventListener('click', function() {
        const userName = userInput.value.trim();
        if (userName) {
            isConnected = true;
            userInput.disabled = true;
            messageInput.disabled = false;
            sendButton.disabled = false;
            connectButton.disabled = true;
            connectButton.innerHTML = '<i class="fas fa-check"></i> Conectado';
            addMessage(`${userName} se ha conectado al chat`, 'system');
        }
    });

    sendButton.addEventListener('click', sendMessage);
    messageInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            sendMessage();
        }
    });

    async function sendMessage() {
        const message = messageInput.value.trim();
        const userName = userInput.value.trim();
        
        if (message && isConnected) {
            addMessage(`${userName}: ${message}`, 'user');
            messageInput.value = '';
            
            try {
                const response = await fetch('/api/AgenteFIFA/ask', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ question: message })
                });

                if (response.ok) {
                    const data = await response.json();
                    addMessage(`Agente FIFA: ${data.answer}`, 'bot');
                } else {
                    addMessage('Error: No se pudo obtener respuesta del Agente FIFA', 'error');
                }
            } catch (error) {
                addMessage('Error: Problema de conexión con el servidor', 'error');
            }
        }
    }

    function addMessage(message, type) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${type}`;
        messageDiv.textContent = message;
        chatBox.appendChild(messageDiv);
        chatBox.scrollTop = chatBox.scrollHeight;
    }
});