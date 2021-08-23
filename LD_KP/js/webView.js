window.webView = {
    callbacks: {},
    invokeCallback: function (cbID, data) {
        if (data == undefined) {
            var cb = webView.callbacks[cbID];
            webView.callbacks[cbID] = undefined;
            return cb.apply(null);
        } else {
            var args = [decodeURIComponent(data)];
            var cb = webView.callbacks[cbID];
            webView.callbacks[cbID] = undefined;
            return cb.apply(null, args);
        }
    },
    callNative: function () {
        if (arguments.length > 0) {
            var methodSignature = arguments[0];
            var formattedArgs = [];
            if (arguments.length > 1) {
                for (var i = 1, l = arguments.length; i < l; i++) {
                    if (typeof arguments[i] == "function") {
                        var cbID = "cb" + webView.uuid();
                        webView.callbacks[cbID] = arguments[i];
                        formattedArgs.push(cbID);
                    } else {
                        formattedArgs.push(encodeURIComponent(arguments[i]));
                    }
                }
            }

            var argStr = (formattedArgs.length > 0 ? ":" + encodeURIComponent(formattedArgs.join(":")) : "");
            var iframe = document.createElement("IFRAME");
            iframe.setAttribute("src", "callnative:" + encodeURIComponent(methodSignature) + argStr);
            document.documentElement.appendChild(iframe);
            iframe.parentNode.removeChild(iframe);
            iframe = null;

            var ret = webView.retValue;
            webView.retValue = undefined;

            if (ret) {
                return decodeURIComponent(ret);
            }
        }
    },
    uuid: function () {
        // http://www.ietf.org/rfc/rfc4122.txt
        var s = [];
        var hexDigits = "0123456789abcdef";
        for (var i = 0; i < 36; i++) {
            s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
        }
        s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
        s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01
        s[8] = s[13] = s[18] = s[23] = "-";

        var uuid = s.join("");
        return uuid;
    },


    alert: function (title, msg, yes, no) {
        if (title == undefined) {
            title = '';
        }
        if (msg == undefined) {
            msg = '';
        }
        if (yes == undefined) {
            yes = '';
        }
        if (no == undefined) {
            no = '';
        }
        webView.callNative('alertWithTitle:message:ok:cancel:', title, msg, yes, no);
    },

    log: function (msg) {
        if (msg == undefined) {
            msg = '';
        }
        webView.callNative('printLog:', msg);
    }
}
