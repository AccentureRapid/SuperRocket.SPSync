SuperRocket.SharePointSync allows you to sync files from Sharepoint to AWS.
This project is based on abp and SuperRocket.Orchard, which is really owesome another template for web development, containing web and web api etc.
Its Architecture is as below picture:  
Its Sequencial flow is as below picture:  
How to run it?
1.	You should have visual studio 2015 update 3 or visual studio 2017 installed.
2.	Download the source code or clone to your local computer usinghttps://github.com/AccentureRapid/SuperRocket.SharePointSync.git
3.	Open the solution in visual studio.
4.	Setup your database and change the connection string in web.config, this will be used as the data store for users, tenants, and the Hangfire jobs.  
5.	Open Package Manager Console, make sure "SuperRocket.Orchard.EntityFramework" is selected as the default project in Manager Console. In solution explorer, make sure "SuperRocket.Orchard.Web" is setup as the start project. Then run below command "update-database -verbose" to do migrations for entity framework.  
6.	Setup sharepoint configuration file in App_Data folder.  
7.	If you want to Sync your sharepoint document library to AWS S3. You should enable the aws in web.config part. As below picture:
 
8.	Setup your IIS to point to your site. Open it in browser: http://localhost:8083/ ,the default User is admin, Password is 123qwe. You should see below  
9.	Open it in browser : http://localhost:8083/hangfire You should see below  
10.	User Postman to enqueue an Sync job. URL :http://localhost:8083/api/services/app/sharePointSyncService/EnqueueSharepointSyncJob That is it! You can track all the jobs on Hangfire dashboard.
### Links
You may find document or develop guide information at:

  * [Introduction to ASP.NET Boilerplate](https://www.codeproject.com/Articles/768664/Introduction-to-ASP-NET-Boilerplate)
  
  * [ASP.NET Boilerplate is a starting point for new modern web applications using best practices and popular tools.](http://aspnetboilerplate.com/)
  
  * [ASP.NET Boilerplate Documentation](http://aspnetboilerplate.com/Pages/Documents)
  
  * [Using ASP.NET Core, Entity Framework Core and ASP.NET Boilerplate to Create NLayered Web Application (Part I)](https://www.codeproject.com/Articles/1115763/Using-ASP-NET-Core-Entity-Framework-Core-and-ASP-N)
  
  * [Using ASP.NET Core, Entity Framework Core and ASP.NET Boilerplate to Create NLayered Web Application (Part II)](https://www.codeproject.com/Articles/1117216/Using-ASP-NET-Core-Entity-Framework-Core-and-ASP)

  * [A Multi-Tenant (SaaS) Application With ASP.NET MVC, Angularjs, EntityFramework and ASP.NET Boilerplate](https://www.codeproject.com/Articles/1043326/A-Multi-Tenant-SaaS-Application-With-ASP-NET-MVC-A)

### Contributors
  * [SmartFire](https://github.com/david0718)
