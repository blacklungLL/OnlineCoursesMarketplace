// Подключение к SignalR Hub'у
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

(async () => {
    try {
        await connection.start();
        console.log("Connected to SignalR Hub");
    } catch (err) {
        console.error("SignalR Error:", err);
    }
})();

// Обработка получения нового сообщения
connection.on("ReceiveMessage", function (from, message, timestamp) {
    const recipientId = document.getElementById('chatInput')?.getAttribute('data-recipient');

    // Проверяем, совпадает ли получатель → если да, добавляем сообщение
    if (parseInt(recipientId) === parseInt(fromId)) { // ❗ fromId не определён → см. ниже
        addMessageToChat(message, from, timestamp);
    }
});

// Загрузка истории при нажатии на пользователя
connection.on("LoadMessages", function (messages, recipientId) {
    const msgArea = document.getElementById('messageArea');
    msgArea.innerHTML = ''; // Очищаем старые сообщения

    messages.forEach(msg => {
        const div = document.createElement('div');
        div.className = msg.isMine ? 'main-message-box ta-right' : 'main-message-box st3';
        div.innerHTML = `
            <div class="message-dt">
                <div class="message-inner-dt">
                    <p>${msg.content}</p>
                </div>
                <span>${msg.senderName} — ${new Date(msg.sentAt).toLocaleTimeString()}</span>
            </div>
        `;
        msgArea.appendChild(div);
    });

    msgArea.scrollTop = msgArea.scrollHeight;
});

// Выбираем пользователя и загружаем историю его чата
async function selectUser(userId, userName) {
    const chatInput = document.getElementById('chatInput');
    const chatUserAvatar = document.getElementById('chatUserAvatar');
    chatInput.setAttribute('data-recipient', userId);
    document.getElementById('chatUserName').innerText = `Чат с ${userName}`;
    document.getElementById('chatHeader').style.display = 'block';
    document.getElementById('chatInput').style.display = 'flex';

    chatUserAvatar.src = `/assets/images/left-imgs/img-${userId}.jpg`;

    try {
        await connection.invoke('LoadChat', userId);
    } catch (e) {
        console.error("Ошибка загрузки чата:", e);
    }
}

// Отправка нового сообщения
function sendMessage(e) {
    e.preventDefault();
    const input = document.getElementById('chat-widget-message-text-2');
    const recipientId = document.getElementById('chatInput').dataset.recipient;
    const message = input.value.trim();

    if (!recipientId || !message) return;

    connection.invoke('SendPrivateMessage', recipientId, message)
        .then(() => {
            input.value = '';
        })
        .catch(err => {
            console.error("Ошибка отправки сообщения:", err);
        });
}

// Добавление сообщения в окно чата
function addMessageToChat(message, from, timestamp) {
    const msgArea = document.getElementById('messageArea');
    const recipientId = document.getElementById('chatInput').dataset.recipient;

    if (!msgArea || !recipientId) return;

    const isMine = parseInt(recipientId) !== from.Id; // если я отправитель → ta-right

    // Создаём контейнер для нового сообщения
    const messageDiv = document.createElement('div');

    if (isMine) {
        messageDiv.className = 'main-message-box ta-right';
        messageDiv.innerHTML = `
            <div class="message-dt">
                <div class="message-inner-dt"><p>${message}</p></div>
                <span>${new Date(timestamp).toLocaleTimeString()}</span>
            </div>`;
    } else {
        messageDiv.className = 'main-message-box st3';
        messageDiv.innerHTML = `
            <div class="message-dt st3">
                <div class="message-inner-dt"><p>${message}</p></div>
                <span>${from} — ${new Date(timestamp).toLocaleTimeString()}</span>
            </div>`;
    }

    // Добавляем новое сообщение в область чата
    msgArea.appendChild(messageDiv);
    msgArea.scrollTop = msgArea.scrollHeight;
}