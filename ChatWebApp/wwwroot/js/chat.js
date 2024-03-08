"use strict";
var GroupName = '';
var RId = '';
$(document).ready(function () {
    $("#chatBox").css("display", "none");
    LoadContacts();
});

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, datetime) {

    // Create a new li element
    var li = document.createElement("li");

    //if (user == userName) {
    //    li.className = "clearfix odd";
    //}
    //else {
    //    li.className = "clearfix";
    //}
    li.className = "clearfix";
    //<i>${new Date(datetime).toLocaleTimeString([], { hour: 'numeric', minute: '2-digit' })}</i>
    // Set innerHTML with the provided content
    li.innerHTML = `
        <div class="chat-avatar">
            <img src="/assets/images/Avatar-1.png" alt="male">
            <i>${new Date(datetime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: true }).toUpperCase()}</i>
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
    LoadContacts();
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var groupName = GroupName
    var message = document.getElementById("messageInput").value;
    var currentdate = new Date();
    if (message != "") {


        InsertMessage(Id, RId, message, groupName);

        connection.invoke("JoinGroup", groupName).catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("SendGroupMessage", groupName, userName, message, currentdate).catch(function (err) {
            return console.error(err.toString());
        });
    }

    event.preventDefault();

    //connection.invoke("SendMessage", userName, message, currentdate).catch(function (err) {
    //    return console.error(err.toString());
    //});
    //event.preventDefault();
});

function LoadContacts() {
    $.ajax({
        type: 'get',
        url: '/Home/LoadContacts',
        success: function (data) {
            $('#UserListPartial').html(data);
        },
        error: function (ex) {
            console.log(ex);
        },
    })
};

function GetUserMessages(UserId, UserName, clickedElement) {
    //display chat page
    
    $(".display-4").css("display", "none");

    $("#chatBox").css("display", "");

    //click houver effect
    $(".inbox-item").css("background", "");
    var inboxItemElement = $(clickedElement).children('.inbox-item');
    inboxItemElement.css("background", "aliceblue");

    // Change the text content
    var profileNameElement = document.getElementById("profileName");
    profileNameElement.textContent = UserName;

    //set receiver id 
    RId = UserId;

    //clear all message
    document.getElementById("messagesList").innerHTML = '';

    //leave old group 
    connection.invoke("LeaveGroup", GroupName).catch(function (err) {
        return console.error(err.toString());
    });

    $.ajax({
        type: 'get',
        url: '/Home/GetUserMessages',
        data: { rId: RId },
        success: function (data) {
            if (data != "\r\n\r\n") {
                $('#messagesList').html(data);
                //for (var i = 0; i < data.length; i++) {
                //    connection.invoke("SendMessage", data[i].userName, data[i].messageText, data[i].createDate).catch(function (err) {
                //        return console.error(err.toString());
                //    });
                //    GroupName = data[i].groupName;
                //}

                $('#messageInput').focus();
                $('.conversation-list').scrollTo('100%', '100%', {
                    easing: 'swing'
                });
                $("#messagesList li").each(function () {
                    GroupName = $(this).data("groupname");
                    // Use groupName as needed
                });
                //console.log(GroupName);
            }
            else {

                GroupName = "_Group-" + Id + "-" + UserId;
                console.log(GroupName);
            }

        },
        error: function (ex) {
            console.log(ex);
        }
    });
}

function InsertMessage(senderId, receiverId, messageText, groupName) {
    var messageModel = {
        SenderId: senderId,
        ReceiverId: receiverId,
        MessageText: messageText,
        GroupName: groupName
    };

    $.ajax({
        type: 'post',
        url: '/Home/InsertMessage',
        data: messageModel,
        success: function (data) {
            //console.log(data);
        },
        error: function (ex) {
            console.log(ex);
        }
    });

}
