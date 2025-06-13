// Подключение к SignalR Hub'у
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// Функция для извлечения параметра из URL
function getURLParameter(name) {
    return new URLSearchParams(window.location.search).get(name);
}

// Подписываемся на событие получения новых сообщений
console.log("Подписка на событие ReceiveMessage...");
connection.on("ReceiveMessage", function (from, message, timestamp) {
    console.log("Получено событие ReceiveMessage:", from, message, timestamp);
    console.log(from.senderId.toString());
    const recipientId = document.getElementById('chatInput')?.dataset.recipient;
    if (!recipientId) return;
    
    currentUserId = recipientId;
    
    const isMyMessage = from.senderId !== recipientId;
    const urlParams = new URLSearchParams(window.location.search);
    const userId = urlParams.get("userId");

    addMessageToChat(message, {
        ...from,
        isMine: isMyMessage
    }, timestamp);
});

// Загружаем историю чата через SignalR
connection.on("LoadMessages", function (messages, recipientId) {
    const msgArea = document.getElementById('messageArea');
    if (!msgArea) return;

    msgArea.innerHTML = '';

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

// Отправка нового сообщения
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

// Добавление сообщения в интерфейс
function addMessageToChat(message, from, timestamp) {
    const msgArea = document.getElementById('messageArea');
    if (!msgArea) return;

    const wrapperDiv = document.createElement('div');

    const urlParams = new URLSearchParams(window.location.search);
    const userId = urlParams.get("userId");

    if (userId !== from.senderId.toString()) {
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
                <span${new Date(timestamp).toLocaleTimeString()}</span>
            </div>`;
    }

    msgArea.appendChild(wrapperDiv);
    msgArea.scrollTop = msgArea.scrollHeight;
}

async function openChat(userId, userName) {
    const chatInput = document.getElementById('chatInput');
    const chatUserName = document.getElementById('chatUserName');
    const chatUserAvatar = document.getElementById('chatUserAvatar');

    console.log("chatInput элемент:", chatInput ? "Найден" : "Не найден");
    
    if (!chatInput || !chatUserName || !chatUserAvatar) {
        window.location.href = `/Chats?userId=${userId}`;
        return;
    }

    chatInput.setAttribute('data-recipient', userId);
    console.log('chatInput:', chatInput);
    chatInput.style.display = 'flex';
    chatUserAvatar.src = `/assets/images/left-imgs/img-${userId}.jpg`;
    chatUserName.innerText = `Чат с ${userName}`;

    document.getElementById('chatHeader').style.display = 'block';

    try {
        await connection.invoke("JoinAndLoad", userId);
    } catch (e) {
        console.error("Ошибка загрузки чата:", e);
    }

    history.pushState({ userId }, '', `/Chats?userId=${userId}`);
}

(async () => {
    try {
        await connection.start();
        console.log("SignalR подключён");

        const urlParams = new URLSearchParams(window.location.search);
        const userId = urlParams.get("userId");

        if (userId) {
            await connection.invoke("JoinAndLoad", parseInt(userId));
        }
    } catch (err) {
        console.error("SignalR Error:", err.toString());
    }
})();

connection.onreconnected(() => {
    const userId = getURLParameter("userId");
    if (userId) {
        connection.invoke("JoinAndLoad", parseInt(userId));
    }
});