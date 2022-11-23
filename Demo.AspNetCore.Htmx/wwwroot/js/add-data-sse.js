(function () {

    function isJsonString(str) {
        try {
            if (!str.startsWith("{")) {
                return false;
            }
            JSON.parse(str);
            return true;
        } catch (e) {
            return false;
        }
    }
    htmx.defineExtension("add-data-sse", {
        /**
         * onEvent handles all events passed to this extension.
         * 
         * @param {string} name 
         * @param {Event} evt 
         * @returns void
         */
        onEvent: function (name, evt) {
            if (name !== "htmx:sseMessage") {
                return;
            }

            var isJson = isJsonString(evt.detail.data);

            if (isJson) {
                //* If json was sent, leave it as is
                evt.detail.elt.setAttribute("hx-vals", evt.detail.data);
            }
            else {
                //* If not, create json for hx-vals
                var msg = {};
                var paramName = evt.detail.elt.getAttribute("hx-params");

                if (!paramName || paramName === "*" || paramName === "data") {
                    // default param name is 'data'
                    msg.data = evt.detail.data;
                }
                else {
                    // custom param name
                    if (!paramName.match(/^(?!\d)[\w]+$/g)) {
                        console.error('hx-params type mismatch.');
                        return;
                    }
                    else {
                        msg[paramName] = evt.detail.data;
                    }
                }
                evt.detail.elt.setAttribute("hx-vals", JSON.stringify(msg));
            }
        }
    });
})();
