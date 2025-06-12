// Подключаемся к SignalR Hub'у
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// Функция для извлечения параметра из URL
function getURLParameter(name) {
    return new URLSearchParams(window.location.search).get(name);
}

// Подписываемся на событие получения новых сообщений
connection.on("ReceiveMessage", function (from, message, timestamp) {
    const recipientId = document.getElementById('chatInput')?.dataset.recipient;

    // Показываем только если это наш собеседник
    if (!recipientId || from.Id !== parseInt(recipientId)) return;

    addMessageToChat(message, from, timestamp);
});

// Загружаем историю чата
connection.on("LoadMessages", function (messages, recipientId) {
    const msgArea = document.getElementById('messageArea');
    if (!msgArea) return;

    msgArea.innerHTML = ''; // Очищаем предыдущие сообщения

    messages.forEach(msg => {
        const div = document.createElement('div');

        if (msg.isMine) {
            div.className = 'main-message-box ta-right';
            div.innerHTML = `
                <div class="message-dt">
                    <div class="message-inner-dt">
                        <p>${msg.content}</p>
                    </div>
                    <span>${new Date(msg.sentAt).toLocaleTimeString()}</span>
                </div>`;
        } else {
            div.className = 'main-message-box st3';
            div.innerHTML = `
                <div class="message-dt st3">
                    <div class="message-inner-dt">
                        <p>${msg.content}</p>
                    </div>
                    <span>${new Date(msg.sentAt).toLocaleTimeString()}</span>
                </div>`;
        }

        msgArea.appendChild(div);
    });

    msgArea.scrollTop = msgArea.scrollHeight;
});

// Подключаемся к SignalR и загружаем чат
(async () => {
    try {
        await connection.start();
        console.log("Connected to SignalR Hub");

        const urlParams = new URLSearchParams(window.location.search);
        const userId = urlParams.get("userId");

        if (userId) {
            await connection.invoke("LoadChat", parseInt(userId));
        }
    } catch (err) {
        console.error("SignalR Error:", err.toString());
    }
})();

// Отправляем новое сообщение
function sendMessage(e) {
    e.preventDefault();
    const input = document.getElementById('messageInput');
    const recipientId = document.getElementById('chatInput').dataset.recipient;
    const message = input.value.trim();

    if (!recipientId || !message) return;

    connection.invoke('SendPrivateMessage', recipientId, message)
        .then(() => input.value = '')
        .catch(err => console.error("Ошибка отправки:", err));
}

// Добавляем сообщение в интерфейс
function addMessageToChat(message, from, timestamp) {
    const msgArea = document.getElementById('messageArea');
    if (!msgArea) return;

    const isMine = from.isMine; // ← получаем напрямую из SignalR

    const wrapperDiv = document.createElement('div');

    if (isMine) {
        wrapperDiv.className = 'main-message-box ta-right';
        wrapperDiv.innerHTML = `
            <div class="message-dt">
                <div class="message-inner-dt">
                    <p>${message}</p>
                </div>
                <span>${new Date(timestamp).toLocaleTimeString()}</span>
            </div>`;
    } else {
        wrapperDiv.className = 'main-message-box st3';
        wrapperDiv.innerHTML = `
            <div class="message-dt st3">
                <div class="message-inner-dt">
                    <p>${message}</p>
                </div>
                <span>${new Date(timestamp).toLocaleTimeString()}</span>
            </div>`;
    }

    msgArea.appendChild(wrapperDiv);
    msgArea.scrollTop = msgArea.scrollHeight;
}

// Открываем чат с пользователем
async function openChat(userId, userName) {
    const chatInput = document.getElementById('chatInput');
    const chatUserName = document.getElementById('chatUserName');
    const chatUserAvatar = document.getElementById('chatUserAvatar');

    if (!chatInput || !chatUserName || !chatUserAvatar) {
        window.location.href = `/Chats?userId=${userId}`;
        return;
    }

    chatInput.setAttribute('data-recipient', userId);
    chatInput.style.display = 'flex';
    chatUserAvatar.src = `/assets/images/left-imgs/img-${userId}.jpg`;
    chatUserName.innerText = `Чат с ${userName}`;

    try {
        await connection.invoke('LoadChat', userId);
    } catch (e) {
        console.error("Ошибка загрузки чата:", e);
    }

    history.pushState({ userId }, '', `/Chats?userId=${userId}`);
}