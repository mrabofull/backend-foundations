HTTP - Hypertext Transfer Protocol
It defines the communication rules for exachanges on the web

Stateless - it doesn't retain information between requests, this means the server doesn't remember the history of interactions with a client.
Cookies - record and track the state and history of your web navigation (allow the use of stateful sessions).
Sessions/JSON Web Tokens - are used to address the lack of the state in the HTTP protocol.
Hypertext - web pages link users to other pages through a clickable links

http://example.com/category/webpage.html?id=1

http - protocole
example.com/category/webpage.html?id=1 - URL (Uniform Resource Locator) a string of characters used to specify the address of a specific resource in the internet.
example.com - domain name, server address (where the resource is hosted)
category - path (the path to the resource in the server)
webpage.html - file name
id=1 - parameters

HTTP (Hypertext Transfer Protocol) operates at the Application Layer of the network protocol stack.
Its job is to define how clients and server communicate.
HTTP defines the structure and meaning of requests and responses.
Its sent over TCP, or over a TLS-encrypted TCP conncetion

Client: the user-agent - its a tool that acts on behalf of the users, as an example a Web browser. The browser is always the entity initiating the request. Its never the server.
proxy - is a middleman between a client and a server, instead of your device communicating directly with the destination server, it sends a request through a proxy server.

What can be controlled by HTTP:

1. Caching
2. Relaxing the origin constraint
3. Authentication
4. Proxy and tunneling
5. Sessions

CORS (Cross-Origin Resource Sharing) - is a browser security mechanism that controls whether a website running on one origin can access resources from another origin.

HTTP flow:

1. Open a TCP connection
2. Send an HTTP message
3. Read the response sent by the server
4. Close or reuse the connection for further requests.

HTTP request example:
GET /api/users/15 HTTP/1.1
Host: example.com
Accept: application/json
Authorization: Bearer your_access_token

HTTP response example:
HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 72

{
"id": 15,
"name": "example",
"email": "example@example.com"
}

URI - Uniform Resource Identifier
is and identifier used to identify a resource, it tells you where it is located or what is the name/identity is.
Every URL and URN is a URI, but not every URI is necessarily a URL or URN

URL - Uniform Resource Locator
A URL identifies a resource by specifying where it is located and how to access it.
https://example.com/users/15

URN (Uniform Resource Name)
Identifies a resource by its name or identity, rather than its location.
urn:isbn:9780132350884

HTTP requests (methods) - GET POST PUT DELETE PATCH
CRUD operations - Create, Read, Update, Delete
PUT - Replace/update the entire resource
PATCH - Update only specific fields

Status Code:

1. 1XX: Informational
2. 2XX: success
3. 3XX: redirection
4. 4XX: client error
5. 5XX: server error

HTTP response - what we get back from the server

HTTPS - its the secure version of HTTP. HTTPS employs SSL/TLS encryption to secure communications between the client and the server ensuring that exchanges are confidential.

100 - Continue - The server received the initial part of the request and is telling the client to continue sending the rest.
101 - Switching Protocols - The server agrees to switch to a different communication protocol, as requested by the client.
200 - OK - everything went well on the server side (The request succeeded, and the server returned the requested data).
201 - Created - something new was created (The request was successful, and a new resource was created).
204 - No Content - it worked but there is no content to send back (The request was successful, but the server has no response body to return).
301 - Moved Permanently - this resource has permanently moved. the browsers is automatically redirected to the new URL.
302 - temporary redirect.
304 - not modified - it asks as if a file have changed since the last time the file got downloaded and if its not modified 304 will be returned.
400 - Bad Request - the requested itself was invalid.
401 - Unauthorized - the resource that got requested requires authentication and the user isn't logged in.
403 - Forbidden - I know you but you aren't allowed to access this resource.
404 - Not Found - what you are looking for doesn't exist.
405 - Method Not Allowed - the resource you requested exists but the used HTTP method used on it is invalid.
408 - Request Timeout - your request took too long, this usually happens when you are on slow connection (Client takes too long to send a request).
409 - Conflict - Trying to register with an email that already exists.
429 - Too Many Requests
500 - Internal Server Error - Something unexpected broke. as example unhandled exceptions, database failure, misconfigured server.
502 - Bad Gateway - the server got a bad response from another server it relies on.
503 - Service Unavailable - Server is overloaded or down for maintenance.
504 - Gateway Timeout - it means one server is waiting too long for a response from another server.

Examples from features implemented in Sportify, showing HTTP status codes that would be appropriate for these scenarios. The examples don't necessarily reflect the exact status codes returned by the original implementation.
200 - The Get All Sports API successfully returns the list of available sports.
201 - A new user successfully registers in the app.
204 - A group admin successfully removes a user from a sports group, and the API doesn't return any response body.
400 - A city organizer tries to create an event but forgets to provide its name.
401 - A user tries to access their profile without being authenticated.
403 - A regular group member tries to access the Get Pending Join Requests API, which is restricted to the group's admin.
404 - A group admin tries to retrieve a user's details within a group, but that user isn't a member of the specified group.
409 - A user tries to register with an email that already belongs to another account.
500 - An unexpected database error occurs while a user is trying to retrieve a specific event's data.
