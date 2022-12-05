# Demo ASP.NET Core with Htmx

The project is an example of using ASP.NET Core MVC with [HTMX](https://htmx.org).
You can look at each commit from the beginning to see how HTMX was integrated.  
The focus in this project is on HTMX, not CSS.
I used the original ASP.NET Core MVC template as a starting point and the *New Scaffolded Item* dialog in Visual Studio to create controllers and views.
During the htmx integration, I only made minor design changes that I needed.

---
The HTMX approach differs from spa frameworks such as Vue.js, where the client-side application uses JavaScript to request information from the server and receive it usually in a JSON format.
With HTMX the Server returns HTML.

**A typical process looks like this.**  
The user clicks on an element on an HTML page, which triggers a request to the server. After receiving the request, the server performs its operations and responds with an HTML fragment, typically a 'Partial View'. When HTMX gets the response, it inserts the html into the page. This [replaces or expands](https://htmx.org/attributes/hx-swap/) an element. However, the received Html can also be [split and inserted](https://htmx.org/extensions/multi-swap/) in different places of the page.

**JSON**  
It is also possible to insert json into the page by using html [templates](https://htmx.org/extensions/client-side-templates/).  

---
HTMX is easy to learn.
If something doesn't work as expected, it's easy to debug.  
The library has just about 3300 lines, including comments and blank lines.

---

## When you should not use htmx

In my opinion it doesn't make sense to use htmx if  

- You need offline functionality.
- your UI state is updated extremely frequently
- your UI has many, dynamic interdependencies
  
---

## Getting Started

To get started, you'll need a minimum of .NET 6 SDK installed on your development machine. You can get the latest SDK from [dot.net](https://dotnet.microsoft.com/download).

### Front-end

- ASP.NET Core MVC
- htmx
- Bootstrap 5
- jquery
  
### Back-end

- ASP.NET Core 6.0
- Entity Framework Core

### Database

- Sqlite

---

### Disclaimer

Motorbike manufacturers and models are used as example data.
No guarantee of any kind is given for the completeness or correctness of the data.

---

### Further reading

- <https://htmx.org/>  
- <https://github.com/search?q=htmx+asp+net+core>
- **<https://htmx.org/essays/a-real-world-react-to-htmx-port/>**
