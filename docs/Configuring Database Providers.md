# Configuring Database Providers

Graffiti supports Microsoft Access, SQL Server 2000/2005/2008, and MySQL.

## VistaDB 
As of version 1.3, VistaDB is no longer supported. This may change in the future.
VistaDB users wanted to upgrade to 1.3 will need to [migrate to a new database provider](Upgrading-a-Graffiti-Database).

## Access 
It really depends on what you are trying to accomplish with your site. If you are running a personal blog or a simple site with just a couple of pages, Access should work very well for you. However, if your goal is to build a large site with lots of content, you should probably use SQL Server or MySQL. 
Access provides a very lightweight option which requires almost no setup and maintenance is as easy as periodically downloading a backup copy of your and storing it locally. 
_**Note:** When using an Access database on a server with a 64 bit OS, you MUST run all of IIS6 or the associated app pool in IIS7 in 32 bit mode because there is not a 64-bit version of Jet._

## SQL Server 
SQL Server is an enterprise grade database option which is ideal for heavily trafficed sites.

## MySQL
MySQL is another enterpise grade, yet cost effective database option for moderately to heavily trafficed sites.

## How do I setup my database?
### VistaDB

For users of Graffiti v1.2 or earlier, VistaDB is ready and configured out of the box. Assuming you followed the initial install steps and granted Graffiti (via ASP.Net) write access to your Web site's directory, there really shouldn't be anything for you to do. 

### Access

Here is how you can switch to Access: 

1. Copy Graffiti.mdb from /Data to /App_Data. 

2. Update the web.config file found in the root of your Web site:   
* Connection string
{code:c#}
<add name="Graffiti" connectionString="Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DATADIRECTORY|\Graffiti.mdb;Persist Security Info=True" /> 
{code:c#}
* Data provider 
{code:c#}
<add key="DataBuddy::Provider" value="DataBuddy.MSAccessProvider, DataBuddy"/> 
{code:c#}
3. Save the web.config file. 

### SQL Server

The steps to setting up SQL Server are very simple:

1. Create a new database (you can use an existing database).

2. Execute the Graffiti_SQL_Schema.sql file found in the Data folder.

3. Execute the Graffiti_SQL_Data.sql file found in the Data folder.

4. Add the connection string of your database to the web.config file. It should have the name "Graffiti":
{code:c#}
<add name="Graffiti" connectionString="server=SERVER;uid=;pwd=;Trusted_Connection=yes;database=DATABASE_NAME"/>
{code:c#}
5. Change the AppSetting key DataBuddy::Provider value to DataBuddy.SQLDataProvider, DataBuddy in the web.config file.

_**Note:** If you are using SQL Server 2005, for Step 5 you can use a provider that is specific to SQL Server 2005/2008: DataBuddy.SQL2K5DataProvider, DataBuddy. This should further improve performance._

**Optional 1:**

If you want to use integrated security, you can run either SQL_IntegratedSecurity_xp.sql (for Windows XP) or SQL_IntegratedSecurity.sql (for Windows Server or Vista) to automatically setup the permissions.

**Optional 2:**

If you want to use the ASP.Net Membershp provider:
# Run the Graffiti_ASPNet_Membership_Provider_Schema.sql script
# Run the Graffiti_ASPNet_Membership_Provider_Data.sql script
# Update & uncomment the Graffiti_ASPNetMembership conn string in the web.config file. Note: you can use the same database for Graffiti and Membership.


### MySQL

1. Create a new database (you can use an existing database).

2. Execute the Graffiti_mySQL_Schema.sql file found in the Data folder.

3. Execute the Graffiti_mySQL_Data.sql file found in the Data folder.

4. Add the connection string of your database to the web.config file. It should have the name "Graffiti":
{code:c#}
<add name="Graffiti" connectionString="server=127.0.0.1;uid=graffiti;pwd=yourStrongPasswordHere;database=Graffiti"/>
{code:c#}
5. Change the AppSetting key DataBuddy::Provider value to DataBuddy.MySQLDataProvider, DataBuddy in the web.config file.