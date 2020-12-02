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

function emptyStarOnClick(artivleId) {
    alert('/Article/' + artivleId);
    $.post('https://localhost:44374/Article/' + artivleId, { handler: 'Add', id: artivleId }, function () {
        $("#empty_star").css("display", "none");
        $("#filled_star").css("display", "flex");
    });
}

function filledStarOnClick(artivleId) {
    $.post('https://localhost:44374/Article/' + artivleId, { handler: 'Remove', id: artivleId }, function () {
        $("#empty_star").css("display", "flex");
        $("#filled_star").css("display", "none")
    });
}