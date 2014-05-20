SharpZendeskApi
============

C# wrapper for the Zendesk Api v2

Getting Started
----

All CRUD operations for all zendesk objects are performed through "managers".

```C#
// setup the client
ZendeskClient client = new ZendeskClient(
        "https://mydomain.zendesk.com", 
        "myemail@mydomain.com",
        "password123",
        ZendeskAuthenticationMethod.Basic);

// get a ticket manager
ITicketManager ticketManager = new TicketManager(client);

// get a ticket
ITicket myticket = ticketManager.Get(1);

// update a ticket
myticket.Subject = "new subject"
ticketManager.SubmitUpdatesFor(myticket);

// create a ticket
// parameterized constructor can be used to fill in the required fields
// or use the default constructor and fill in the fields later.
ITicket ticketToBeSubmitted = new Ticket(..);
ticketManager.SubmitNew(ticketToBeSubmitted);

// get tickets by ID
IList<ITicket> tickets = ticketManager.GetMany(1,2,3).ToList();

// get tickets from a view using the view id
IList<ITicket> tickets = ticketManager.FromView(1).ToList();
```

GetMany and similar functions return a lazy paging object. If more than 100 objects would be sent, multiple
requests are made as needed. "AtEndOfPage" property is exposed to let you know if the next call to "MoveNext()" will cause a new page request to be performed.

<br />

ZenDesk Api Documention
----
http://developer.zendesk.com/documentation/rest_api/introduction.html

Thanks & Inspiration
----
http://blog.martindoms.com/2011/01/16/writing-testable-web-api-wrappers/

http://bit.ly/1jtMGd2 ( Auto Mocking and the Testable pattern )