Structure:
PPOK-Twilio is our web project.
In the App Start there holds our configs.
Auth contains the principal that fills the user data upon login and the serializer for forms authentication.
Content holds javascript file and some of our own javascript but mainly the vendor content pulled in
Controllers holds all the controllers for handling the pages.
fonts is our fonts
PageScripts holds our custom JavaScript
Scripts holds vendor scripts and some of our own custom scripts
Views holds all our views. - Landing page is our home page
Search allows you to search for any of the events - is not finished
System Admin is about 80% done, it just needs to be updated for the new data structure and beautified.

Domain holds the services and models to be used in the web project
Models holds our custom data objects to be used in pages or anywhere we need them.
Scripts hold the database creation script
Service holds the CRUD functionality and all other services
Types are the classes that correspond to database objects

Twilio Test - this is used to run our testing scripts
 1) Make a PPOk local database in SQL Server
 2) Run the Twilio Test project (a console application) 
 3) Option 1 will set up the DB with the dummy data necessary for the web project
