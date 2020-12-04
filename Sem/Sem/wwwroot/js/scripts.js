function openArticle(){
    document.location.href = "Article";
}

function openForum(){
    document.location.href = "Forum"
}

function openConstructor(){
    document.location.href = "Constructor"
}

function sendMessage(message)
{
    if ("WebSocket" in window) {
        if (ws === null)
        {
            ws = new WebSocket("ws://localhost:44374");
            ws.onopen = function () {
                if (message == 'reg')
                {
                    log = document.getElementById("txtLog").value;
                    ws.send(document.getElementById("txtLog").value + '#' + document.getElementById("txtPas").value);
                }
                else
                    ws.send(document.getElementById("messageText").value);
            };
            ws.onmessage = function (evt) {
                var a = evt.data.split('%');
                if (a.length > 1)
                {
                    let div = document.createElement('div');
                    div.className = 'user';
                    div.append(a[1]);
                    document.getElementById("messageText").before(div);
                    document.getElementById("regitrationBlock").replaceWith('');
                }
                else
                {
                    let div = document.createElement('div');
                    div.className = 'message';
                    div.append(evt.data);
                    document.getElementById("messageText").before(div);
                }
            };
            ws.onclose = function () {
                ws = null;
            };
        }
        else 
        {
            if (message == 'reg')
            {
                log = document.getElementById("txtLog").value;
                ws.send(document.getElementById("txtLog").value + '#' + document.getElementById("txtPas").value);
            }
            else
                ws.send(document.getElementById("messageText").value);
        }
    }
}

function StarOnClick(articleId, empty, filled, post) {
    $.ajax({
        type: "POST",
        url: "/Article/" + articleId + "?handler=" + post,
        beforeSend: XHRCheck,
        data: { articleId: articleId },
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
    $.ajax({
        type: "GET",
        url: url,
        beforeSend: XHRCheck,
        dataType: 'html',
        success: function RedirectPage(data) {
            $('html').html(data);
            window.history.pushState("object or string", "Title", url);
        }
    })
}

function RedirectToIndexArticle(url, index) {
    $.ajax({
        type: "GET",
        url: url,
        data: { articleId: index },
        beforeSend: XHRCheck,
        dataType: 'html',
        success: function RedirectPage(data) {
            $('html').html(data);
            window.history.pushState("object or string", "Title", url + "/" + index);
        }
    });
}

function RedirectToIndexForum(url, index) {
    $.ajax({
        type: "GET",
        url: url,
        data: { forumId: index },
        beforeSend: XHRCheck,
        dataType: 'html',
        success: function RedirectPage(data) {
            $('html').html(data);
            window.history.pushState("object or string", "Title", url + "/" + index);
        }
    });
}

function PostOnClick(url, post) {
    $.ajax({
        type: "POST",
        url: url + "?handler=" + post,
        data: { login: $('#login').val(), pas: $('#password').val(), repeat_pas: $('#repeat_password').val() },
        beforeSend: XHRCheck,
        success: ResultRegister
    });
}

function PostSignIn(url, post) {
    $.ajax({
        type: "POST",
        url: url + "?handler=" + post,
        data: { login: $('#login').val(), pas: $('#password').val() },
        beforeSend: XHRCheck,
        success: ResultRegister
    });
}

function ResultRegister(result) {
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

function PostAcccountChange(url, post) {
    $.ajax({
        type: "POST",
        url: url + "?handler=" + post,
        data: { login: $('#login').val(), old_pas: $('#old_password').val(), new_pas: $('#new_password').val() },
        beforeSend: XHRCheck,
        success: ResultRegister
    });
}