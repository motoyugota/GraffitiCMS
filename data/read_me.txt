1. Create a database (default conn string uses a database name of "Graffiti")
2. Run the Graffiti_Schema.sql script
3. Run the the Graffiti_Data.sql script (this will create a user with the credentials admin/password)

Optional:
If you want to use integrated security, you could also run either xp.sql or server.sql.

Optional:
If you want to use the ASP.Net Membershp provider:

1. Run the Graffiti_ASPNet_Membership_Provider_Schema.sql script
2. Run the Graffiti_ASPNet_Membership_Provider_Data.sql script
3. Update the Graffiti_ASPNetMembership in the __config/connectionstrings.config file. Note: you can use the same database for Graffiti and Membership.

For both ASP.Net and Graffiti's internal membership system, the default credentials are admin and "password" (no quotes). 