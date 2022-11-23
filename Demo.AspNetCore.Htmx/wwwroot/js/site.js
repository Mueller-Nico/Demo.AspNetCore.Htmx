(function () {

    function htmlEncode(msg) {
        if (!msg) return msg;
        var mydiv = document.createElement("div");
        if ('textContent' in mydiv) {
            mydiv.textContent = msg;
        } else {
            mydiv.innerText = msg;
        }
        var output = mydiv.innerHTML;
        mydiv = null;
        return output;
    }

    // Error Message  start

    function overlay(display) {
        var el = document.getElementById("idOverlay");
        el.style.display = display;
        if (display === "block") {
            el.scrollIntoView();
        }
    }

    function errorMessageHide() {
        var el = document.getElementById("idErrorMessage");
        el.style.visibility = "hidden";
        el.innerHTML = "";
        overlay("none");
    }

    function showError(text, alertType, title) {
        var errdiv = document.getElementById("idErrorMessage");
        errdiv.innerHTML =
            `<div class="card fadeIn-800" role="alert">
                <div class="card-header h5 p-1 mb-0 text-center alert alert-` + htmlEncode(alertType) + `">` + htmlEncode(title) + `
                  <button type="button" class="btn-close" style="float:right;"></button>
                </div>
              <div class="card-body">` + htmlEncode(text) + `</div>
              <hr class="mt-0 mb-2"/> 
              <div class="text-center pb-2">
                <button type="button" class="btn btn-sm btn-secondary">Close</button>
              </div>
            </div>`;

        overlay("block");

        errdiv.style.visibility = "visible";

        setTimeout(() => {
            var buttons = errdiv.getElementsByTagName("button");
            buttons[1].focus({ focusVisible: true });
        }, 100);
    }

    (function initErrorMessage() {
        var errdiv = document.getElementById("idErrorMessage");
        errdiv.onclick = function (evt) {
            if (evt.target.nodeName === 'BUTTON')
                errorMessageHide();
        };
        errdiv.onkeyup = function (event) {
            if (event.keyCode == 27) {
                errorMessageHide();
            }
        };
    })();

    // Error Message  end

    document.addEventListener("htmx:configRequest", (evt) => {

        if (evt.detail.verb === 'get') { return; }

        // Antiforgery
        // Adjust the following constants if default values have been changed in Program.cs or Startup.cs

        const headerName = 'X-XSRF-TOKEN';
        const formFieldName = '__RequestVerificationToken';


        // already specified on form ?
        if (evt.detail.parameters[formFieldName]) { return; }

        let field = document.querySelector("input[name='" + formFieldName + "']");
        if (!field) {
            console.error('RequestVerificationToken not found');
            return;
        }

        evt.detail.headers[headerName] = field.value;
    });

    document.body.addEventListener('htmx:responseError', function (evt) {
        /*
         return Problem("Problem Description");
         * 
         application/problem+json; charset=utf-8
        detail:"Problem Description"
        status: 500 
        title: "An error occurred while processing your request."
        traceId: "00-f340ab37ebbacb20bec25b62e3726936-a8991112954f86ae-00"
        type: "https://tools.ietf.org/html/rfc7231#section-6.6.1"
         */
        const contType = evt.detail.xhr.getResponseHeader("Content-Type");
        //console.info(contType);
        if (contType && contType.includes("json")) {
            const json = JSON.parse(evt.detail.xhr.response);
            switch (json.type) {
                case "warning":
                case "info":
                    break;
                default:
                    json.type = "danger";
                    break;
            }
            showError(json.detail, json.type, json.title);
            return;
        }

        if (evt.detail.xhr.status === 404) {
            // alert the user when a 404 occurs 
            showError("Could Not Find Resource", "danger", "Status " + evt.detail.xhr.status);
        } else {
            showError(evt.detail.xhr.statusText, "danger", "Status " + evt.detail.xhr.status);
        }
    });

    function resetLoadingStates(elt) {
        // workaround for issue 1118 https://github.com/bigskysoftware/htmx/issues/1118
        try {
            htmx.trigger(elt, "htmx:afterOnLoad");
        } catch (e) {
            console.error(e);
        }
    }

    document.body.addEventListener('htmx:sendError', function (evt) {
        showError("Network error occured when attempting to fetch resource. Please check your network connection status.", "danger", "Error");
        resetLoadingStates(evt.detail.elt);
    });


    document.body.addEventListener('htmx:timeout', function (evt) {
        showError("It seems the server is taking too long to respond. Please try again later.", "danger", "Timeout")
        resetLoadingStates(evt.detail.elt);
    });

    document.body.addEventListener('htmx:sendAbort', function (evt) {
        resetLoadingStates(evt.detail.elt);
    });

    document.body.addEventListener('htmx:onLoadError', function (evt) {
        showError(evt.detail.exception.message, "danger", "Error")
    });

    document.body.addEventListener('htmx:afterOnLoad', function (evt) {
        //console.info(evt.detail);
    });

    document.body.addEventListener('htmx:beforeSwap', function (evt) {

        var tmp = evt.detail.serverResponse.trimStart();
        var tagStart = '<title>';
        if (!tmp.startsWith(tagStart)) {
            return;
        }

        var tagEnd = '</title>';
        var indexEnd = tmp.indexOf(tagEnd);
        if (indexEnd === -1) {
            return;
        }
        evt.detail.serverResponse = tmp.substring(indexEnd + tagEnd.length);

        document.title = tmp.substring(tagStart.length, indexEnd);
    });

    document.body.addEventListener("evtHtmxHeaderTrigger", function (evt) {
        if (evt.detail.mode === "initValid") {
            if (evt.detail.delay) {
                setTimeout(function () {
                    $.validator.unobtrusive.parse('#' + evt.detail.id);
                }, evt.detail.delay);
            }
            else {
                $.validator.unobtrusive.parse('#' + evt.detail.id);
            }
        }
        else {
            console.error("unkown mode");
            return;
        }
    });

    $(document)
        .on('click', 'form button[type=submit]', function (e) {
            var $form = $(e.target).parents('form');

            e.preventDefault(); //always prevent the default action

            $form.validate();
            if ($form.valid()) {
                htmx.trigger($form[0], "confirmed");
            }
        });
})();