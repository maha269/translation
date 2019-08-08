﻿function getPathFromUrl(url) { return url.split("?")[0]; }
function clearChildren(element) { while (element.firstChild) { element.removeChild(element.firstChild); } }
function createElement(tag) { return document.createElement(tag); }
function getElement(id) { return document.getElementById(id); }
function getValue(id) { return document.getElementById(id).value; }
function getAntiForgeryTokenFromLayout() { return document.getElementsByName('__RequestVerificationToken')[0].value; }

function getLowerCase(text) {
    return text.toLowerCase().replace(/ğ/gim, "g").replace(/ü/gim, "u").replace(/ş/gim, "s").replace(/ö/gim, "o").replace(/ç/gim, "c").replace(/ /gim, "_").replace(/-/gim, "_");
}

function createLink(text, href) {
    let a = createElement('a');
    a.innerText = text;
    a.href = href;
    return a;
}

function createButton(text, onClick) {
    let btn = createElement('button');
    btn.innerText = text;
    btn.onclick = onClick;
    return btn;
}

function hideElement(item) {
    item.style.opacity = 0;
    setTimeout(function () { item.style.display = 'none'; }, 377);
}

function showElement(item, opacity) {
    item.style.opacity = opacity;
    setTimeout(function () { item.style.display = 'block'; }, 89);
}

function doGet(url, onSuccess) {
    let req = new XMLHttpRequest();
    req.open('GET', url, true);
    req.onreadystatechange = function () {
        if (req.readyState === XMLHttpRequest.DONE) {
            if (199 < req.status && req.status < 300) {
                onSuccess(req);
            } else {
                if (req.status != 404) {
                    console.log('error', req.status.toString(), req);
                }
            }
        }
    }
    req.send(null);
}

function doPostWithJsonContent(url, data, onSuccess, onError) {
    doPostRequest(url, 'application/json', data, onSuccess, onError);
}

function doPostWithFormUrlEncodedContent(url, data, onSuccess, onError) {
    doPostRequest(url, 'application/x-www-form-urlencoded', data, onSuccess, onError);
}

function doPostRequest(url, contentType, data, onSuccess, onError) {
    let req = new XMLHttpRequest();
    req.open('POST', url, true);
    req.setRequestHeader('Content-type', contentType);
    req.setRequestHeader('RequestVerificationToken', getAntiForgeryTokenFromLayout());
    req.onreadystatechange = function () {
        if (req.readyState === XMLHttpRequest.DONE) {
            if (199 < req.status && req.status < 300) {
                onSuccess(req);
            } else {
                onError(req);
            }
        }
    };
    req.send(data);
}

function doIfConfirmed(btn, onConfirm) {
    showPopup(btn.dataset.confirmTitle, btn.dataset.confirmContent, true, onConfirm);
}

function doRedirectIfConfirmedSuccess(btn, redirectUrl) {
    showPopup(btn.dataset.confirmTitle, btn.dataset.confirmContent, true, function () {
        doPostWithFormUrlEncodedContent(btn.dataset.url, btn.dataset.prm,
            function (req) {
                let response = JSON.parse(req.response);
                if (response.isOk === true) {
                    window.location.href = redirectUrl;
                } else {
                    let messages = response.messages.join('<br/>');
                    showPopupMessage(messages);
                }
            },
            function (req) {
                let messages = JSON.parse(req.response).messages.join('<br/>');
                showPopupMessage(messages);
            });
    });
}

let labels = JSON.parse(snappyStorage().getItem('translations'));

function translateScreen() {
    translateElement(document.head);
    translateElement(document.body);
}

function translateElement(element) {
    if (element === null || element === undefined) {
        return;
    }
    
    if (labels === null || labels === undefined) {
        return;
    }
    
    let defaultLang = 'en';
    if (currentUser !== undefined && currentUser !== null) {
        defaultLang = currentUser.language;
    }
    
    let placeholders = element.querySelectorAll('[placeholder]');
    placeholders.forEach(function (item) {
        for (let i = 0; i < labels.length; i++) {
            let label = labels[i];
            
            if (label.key === item.placeholder) {
                label.translations.forEach(function (translation) {
                    if (translation.languageIsoCode2 === defaultLang) {
                        item.setAttribute('placeholder', translation.translation);
                        return;
                    }
                });
                
                break;
            }
        }
    });
    
    let items = element.querySelectorAll('[data-translation]');
    items.forEach(function (item) {
        for (let i = 0; i < labels.length; i++) {
            let label = labels[i];
            if (label.key === item.dataset.translation) {
                label.translations.forEach(function (translation) {
                    if (translation.languageIsoCode2 === defaultLang) {
                        item.innerHTML = translation.translation;
                        return;
                    }
                });
                
                break;
            }
        }
    });
}

doGet('/Data/GetCurrentUser', function (req) {

    console.log("req.responseText: ", req.responseText);
    if (199 < req.status && req.status < 300) {
        if (req.status === 200
            && req.responseText !== null) {
            localStorage.setItem('currentUser', req.responseText);
        } else if (req.status === 200
            && req.responseText === null) {
            window.redirect('/Login');
        }
    }
});

if (!labels) {
    doGet('/Data/GetMainLabels', function (req) {
        if (199 < req.status && req.status < 300) {
            if (result.isOk) {
                labels = result.item;
                localStorage.setItem('labels', req.responseText);
                translateScreen();
            }    
        }
    });
} else {
    translateScreen();
}
