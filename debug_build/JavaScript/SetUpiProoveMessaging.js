(function () {
    // Create IE + others compatible event handler
    var eventMethod = window.addEventListener ? 'addEventListener' : 'attachEvent';
    var eventer = window[eventMethod];
    var messageEvent = eventMethod == 'attachEvent' ? 'onmessage' : 'message';
    var domain = 'https://secure.iproov.me';

    // Listen to message from child window
    eventer(messageEvent, function (event) {
        console.log('Parent received message: ', event.data);

        if (event.origin !== domain) return;

        if (event.data == 'getIdentity') {
            var iframe = document.getElementById('iproov_nobot').contentWindow;
            var identity = '02' + document.getElementById('login_id').value; // Username

            console.log('Identity: ', identity);

            iframe.postMessage(identity, domain);
        } else if (event.data == 'focusIdentity') {
            document.getElementById('login_id').focus();
        } else {
            var token = event.data.substring(0, 64);
            var result = event.data.substring(64);
            var username = document.getElementById('login_id').value; // same as identity but withouth 02 at the front
            var userAgent = navigator.userAgent;

            window.external.NotifyAuthenticationResult(result, token, username, userAgent);
        }
    }, false);

    if (document.getElementById('login_id').value.length == 0) {
        document.getElementById('login_id').focus();
    }

})();