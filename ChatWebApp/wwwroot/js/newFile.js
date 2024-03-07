"use strict";
//connection.on("ReceiveMessage", function (user, message, datetime) {
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    li.textContent = `${user} says ${message} ${datetime}ago`;
//});
connection.on("ReceiveMessage", function(user, message, datetime) {


    // Create a new li element
    var li = document.createElement("li");
    li.className = "clearfix";


    // Set innerHTML with the provided content
    li.innerHTML = `
        <div class="chat-avatar">
            <img src="assets/images/avatar-1.jpg" alt="male">
            <i>${formatTime(new Date(datetime))}</i>
        </div>
        <div class="conversation-text">
            <div class="ctext-wrap">
                <i>${user}</i>
                <p>${message}</p>
            </div>
        </div>
    `;
    // Append the li to the messagesList
    document.getElementById("messagesList").appendChild(li);

    $('#messageInput').val('');
    $('#messageInput').focus();
    $('.conversation-list').scrollTo('100%', '100%', {
        easing: 'swing'
    });
});
