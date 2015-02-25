(function () {
    var newLogFunction = function () {
        var message = Array.prototype.join.call(arguments, ' ');
        window.external.Log(message);
    };

    console.log = newLogFunction;
    console.info = newLogFunction;
    console.warn = newLogFunction;
    console.error = newLogFunction;

})();