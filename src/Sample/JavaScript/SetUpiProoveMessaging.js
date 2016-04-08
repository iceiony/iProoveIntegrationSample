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
		
		var iProovMessage = JSON.parse(event.data);

        if (iProovMessage.action == 'getIdentity') {
            var iframe = document.getElementById('iproov_nobot').contentWindow;
			
			var response = {};
            response.username = document.getElementById('login_id').value; // Username
			response.version = '02';
			
            console.log('Identity: ', response.username);

			iframe.postMessage(JSON.stringify(response), domain);
        } else if (iProovMessage.action == 'focusIdentity') {
            document.getElementById('login_id').focus();
        } else if (iProovMessage.action == 'setResult') {
            var token = iProovMessage.token;
            var result = iProovMessage.result;
            var username = document.getElementById('login_id').value; // same as identity but withouth 02 at the front
            var userAgent = navigator.userAgent;

            window.external.NotifyAuthenticationResult(result, token, username, userAgent);
        }
    }, false);

    if (document.getElementById('login_id').value.length == 0) {
        document.getElementById('login_id').focus();
    }

})();
