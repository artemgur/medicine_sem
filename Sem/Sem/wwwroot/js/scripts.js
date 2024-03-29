function AddToFavorite(url, index, empty, filled, post) {
    $.ajax({
        type: "POST",
        url: url + "/" + index + "?handler=" + post,
        beforeSend: XHRCheck,
        data: { index: index },
        success: function () {
            $("#empty_star").css("display", empty);
            $("#filled_star").css("display", filled);
        },
        failure: function (response) {
            alert(response);
        }
    });
}

function Redirect(url) {
    // $.ajax({ //Led to many issues, removed it
    //     type: "GET",
    //     url: url,
    //     beforeSend: XHRCheck,
    //     dataType: 'html',
    //     success: function RedirectPage(data) {
    //         $('html').html(data);
    //         //window.location.href = url;
    //         window.history.pushState("object or string", "Title", url);
    //     }
    // })
    document.location = url
}

function RedirectToIndex(url, index) {
    window.location.href = url + "/" + index;
    /*$.ajax({
        type: "GET",
        url: url + "/" + index,
        data: { index: index },
        beforeSend: XHRCheck,
        dataType: 'html',
        success: function RedirectPage(data) {
            $('html').html(data);
            window.location.href = url + "/" + index;
            //window.history.pushState("object or string", "Title", url + "/" + index);
        }
    });*/
}

function PostOnClick(url, post) {
    $.ajax({
        type: "POST",
        url: url + "?handler=" + post,
        data: { login: $('#login').val(), pas: $('#password').val(), repeat_pas: $('#repeat_password').val() },
        beforeSend: XHRCheck,
        success: successAction
    });
}

function PostSignIn(url, post) {
    $.ajax({
        type: "POST",
        url: url + "?handler=" + post,
        data: { login: $('#login').val(), pas: $('#password').val() },
        beforeSend: XHRCheck,
        success: successAction
    });
}

function PostAcccountChange(url, post) {
    $.ajax({
        type: "POST",
        url: url + "?handler=" + post,
        data: { login: $('#login').val(), old_pas: $('#old_password').val(), new_pas: $('#new_password').val() },
        beforeSend: XHRCheck,
        success: successAction
    });
}

function successAction(result) {
    if (result != "") {
        $('#message').html(result);
        $('#message').show('slow');
    }
    else {
        Redirect('/');
    }
}

function XHRCheck(xhr) {
    xhr.setRequestHeader("XSRF-TOKEN",
        $('input:hidden[name="__RequestVerificationToken"]').val());
}

function PostCreateForum(url, post) {
    $.ajax({
        type: "POST",
        url: url + "?handler=" + post,
        data: { name: $('#name').val(), description: $('#description').val() },
        beforeSend: XHRCheck,
        success: function(result) {
            var els = result.split('$');
            if (els.length != 2) {
                $('#message').html(els[0]);
                $('#message').show('slow');
            }
            else {
                Redirect('/Forum/' + els[0]);
            }
        }
    });
}

let ws = null;
function sendMessage(url, post, chatId, userId, userName, userImg) {
    if ("WebSocket" in window) {
        if (ws === null) {
            ws = new WebSocket("wss://localhost:44374/wss");
            ws.onopen = function () {
                SetMessage(url, post, userId, chatId, userName, userImg);
            };
            ws.onmessage = function (evt) {
                GetMessage(evt);
            };
            ws.onclose = function () {
                ws = null;
            };
        }
        else {
            SetMessage(url, post, userId, chatId, userName, userImg);
        }
    }
}

function SetMessage(url, post, userId, chatId, userName, userImg) {
    let value = $("#chatInput").val();
    if (value !== "" && userId !== "") {
        ws.send(chatId + "&" + userId + "&" + userName + "&" + userImg + "&" + value);
        postMessage(url, post, chatId, value);
        $("#chatInput").val('');
    }
}

function postMessage(url, post, chatId, value) {
    $.ajax({
        type: "POST",
        url: url + "?handler=" + post,
        data: { forumId: chatId, text: value },
        beforeSend: XHRCheck
    });
}

function GetMessage(evt) {
    let params = decodeURIComponent(evt.data).split('&');
    let lastUserMs = $('.messages_from_user').last();
    let userIdAns = params[0];

    let divBoxMes = "<div class='message-box'><div class='message'>" +
        ConcatArray(3, params).replace('+', ' ') +
        "</div></div>";
    if (lastUserMs.attr('id') === userIdAns) {
        lastUserMs.children('.message-place').last().children('.message-box').last().after(divBoxMes);
    }
    else {
        let userName = params[1];
        let userImg = params[2];
        if (userImg == "")
            userImg = 'https://img.icons8.com/color/36/000000/administrator-male.png';
        divBoxMes = '<div class="messages_from_user" id="' + userIdAns + '">' +
            '<img class="message-img" src="' + userImg + '" alt="" title="'+ userName + '">' +
            '<div class="message-place" >' +
            divBoxMes +
            '</div></div>';
        if (lastUserMs !== null)
            lastUserMs.after(divBoxMes);
        else
            $('#forum_send').append(divBoxMes);
    }
    $('#forum_send').animate({ scrollTop: $('#forum_send').prop('scrollHeight') }, 300);
}

function ConcatArray(startIndex, array) {
    let result = "";

    for (let i = startIndex; i < array.length; i++) {
        result += array[i];
    }

    return result;
}

function Search(url, handler) {
    let array = $('.filter-option-inner-inner').first().html();
    $.ajax({
        type: "GET",
        url: url + "?handler=" + handler,
        data: { array: array, name: $('#search_input').val() },
        beforeSend: XHRCheck,
        success: function (data) {
            $('html').html(data);
        }
    });
}

$(document).ready(function () {
    $('.custom-file-input').on('change', function () {
        let fileName = $(this).val().split('\\').pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});