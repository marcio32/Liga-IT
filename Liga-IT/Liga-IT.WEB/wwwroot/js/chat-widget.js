$(document).ready(function() {
    $('.chat-bubble').click(function() {
        $('.chat-window').toggleClass('active');
    });

    $('.chat-close').click(function() {
        $('.chat-window').removeClass('active');
    });

    $('#sendMessage').click(function() {
        const message = $('#chatInput').val().trim();
        if (message) {
            sendMessageToAgent(message);
            $('#chatInput').val('');
        }
    });

    function sendMessageToAgent(message) {
        // Agregar mensaje del usuario
        addMessage(message, 'user');
        
        // Enviar al AgenteFIFA
        $.ajax({
            url: '/AgenteFIFA/Ask',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Question: message }),
            success: function(response) {
                addMessage(response.answer, 'agent');
            },
            error: function() {
                addMessage('Error al conectar con AgenteFIFA', 'agent');
            }
        });
    }

    function addMessage(message, sender) {
        const messageClass = sender === 'user' ? 'user-message' : 'agent-message';
        const messageHtml = `<div class="${messageClass}" style="margin: 10px 0; padding: 8px; border-radius: 8px; ${sender === 'user' ? 'background: #007bff; color: white; text-align: right;' : 'background: #f1f1f1; color: #333;'}">${message}</div>`;
        $('#chatBody').append(messageHtml);
        $('#chatBody').scrollTop($('#chatBody')[0].scrollHeight);
    }

    $('#chatInput').keypress(function(e) {
        if (e.which === 13) {
            $('#sendMessage').click();
        }
    });
});
