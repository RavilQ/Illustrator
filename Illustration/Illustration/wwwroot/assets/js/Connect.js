

let connection = new signalR.HubConnectionBuilder().withUrl("/illustratorhub").build();

connection.start().then(function () {

})

document.getElementById("userMessageSend").addEventListener("click", function (e) {
    e.preventDefault();
    var message = document.getElementById("usermessageinput").value;
    var name = "Elariz"
    connection.invoke("SendMessageUser", name, message);

    var link = `/Account/InboxMessageSend?MyMessage=${message}`

    fetch(link)
        .then(response => {
            return response.text();
        }).then(html => {

        })
})

connection.on("RecieveMessage", function (name, message) {
    var destination = document.getElementById("usermessageinboxconfirm");
    var image = document.getElementById("usermessageimagesend").value;
    destination.innerHTML += ` <div class="chat-message-right pb-4">
                                <div class="ms-2">
                                    <img src="/Uploads/Users/${image}" class="rounded-circle mr-1" alt="Chris Wood" width="40" height="40">
                                </div>
                                <div style="display: flex;align-items: center;" class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">
                                   ${message}
                                </div>
                            </div>
`
    document.getElementById('usermessageinput').value = "";
})


connection.on("RecievMessage", function (name, message) {
    var destination = document.getElementById("usermessageinboxconfirm");
    destination.innerHTML += `  <div class="chat-message-left pb-4">
                                <div class="me-2">
                                    <img src="/assets/images/4f64c9f81bb0d4ee969aaf7b4a5a6f40.png" class="rounded-circle mr-1" alt="Sharon Lessman" width="40" height="40">
                                </div>
                                <div class="flex-shrink-1 bg-light rounded py-2 px-3 ml-3">
                                    <div class="font-weight-bold mb-1">Admin</div>
                                    ${message}
                                </div>
                            </div>
`
})

