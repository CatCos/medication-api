# Medication API
WebAPI to handle medication information.  You can:
- Create a new medication with name and date of creation;
- Delete a medication
- List all the medications available

# Technologies
The WebAPI is developed using .NET Core 3.1. The medication information is stored using a MongoDb database.
The application also contains Unit Tests that were developed using MsTesting.

# How to execute
To run the application you should:
- Change the file  `conf/appsettings.json` with the link of your MongoDB Server
- Run the application on Visual Studio
- Access `localhost/swagger` to use the API 
