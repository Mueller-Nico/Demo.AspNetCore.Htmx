(function () {


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
                    json.type = "error";
                    break;
            }
            alert(json.detail); //, json.title, json.type);
            return;
        }

        if (evt.detail.xhr.status === 404) {
            // alert the user when a 404 occurs (maybe use a nicer mechanism than alert())
            alert("Could Not Find Resource");
        } else {
            alert(evt.detail.xhr.statusText);
        }
    });
})();