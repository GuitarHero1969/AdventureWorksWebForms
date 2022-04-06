# AdventureWorksWebForms
Replacing the ASP.NET postback system with just AJAX

This is a 'Work in Progress' and also a revision in ASP.NET webforms technology
using AJAX to replace the postback system employed by ASP.NET webforms

Here I use Axios fetch API to call page methods in the code behind.

Of course there will be a limit to what I can do, since I am attempting to mix old libraries and frameworks with newer tools like Axios for example.
Since ASP.NET is designed around a postback mechanism, I don't recommend using this in production code, since ajax tools already exist specifically for webforms.

The web (page) methods are in C#, LINQ and EF6 are used to connect to the Adventure Works 2014 database.

Further improvements to follow
* one get request to retrieve all product, category and sub-category data for the three dropdowns
* nested menu to replace the three separate drop-downs - less clutter in the bootstrap styling
* refactoring of JavaScript business logic
