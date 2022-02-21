# Database Management

## Creating a local database
It is assumed you already have Microsoft's SQL Server Management Studio.
If you haven't yet installed MSSMS, check the README.md file for instructions.

To create a new database:
1. Begin by opening MSSMS.
2. Once it is open, assuming it has been setup correctly, you can simply click the connect button to connect to your local database server. While you're here, you should also copy the "Server name" property for later use.
3. Once you are connected, right click on the Databases folder and click "New Database".
4. Enter a name for the database and be sure to also copy this down for later.For our purposes, no settings need to be changed.
5. Click OK and the database will be created.

## Connecting to a database

To setup the project to connect with your newly created database, you will need the "Server name" and "Database name" values from the previous instructions.
1. Begin by creating a file called appsettings.json in the same folder this readme is located in. That is, the root folder for the backend project. If this file already exists, open it.
2. Once you have appsettings.json open:
* If you did not previous have this file, copy the following contents into it:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
}
```
* Now, add the following entry to the json:
```json
"ConnectionStrings": {
    "DbConnectionString": "Server=SERVER_NAME;Database=DATABASE_NAME;integrated security=true;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
```
* Replace SERVER_NAME with the "Server name" value from earlier. Also replace DATABASE_NAME with the "Database name" value from earlier.
4. Save the file. The project is now ready to connect to the database.

NOTE: The reason this file does not sync across git is because we are not using a centralized database during development. Everyone would be constantly pushing their database connection string and overwritting others, creating headaches, so it is easier to just put this file in the gitignore.

## Creating a database entity (in C#)

Here you will learn how to create a new database entity in C#:
1. Begin by creating a new class file within the "Models/DatabaseObjects" folder.
2. Give the class a name. Please stick to the names we outline in the <a href="https://docs.google.com/spreadsheets/d/1eOTJwPj-w2ThoDjbEGlKIn1ampjKZX1F9NJ3_RKH1Ck/edit?usp=sharing">Data Dictionary</a>
3. To set up database fields, simply create a public C# auto property with the name of the field.
4. Next, you should mark the new property with the "Column" attribute. You should also provide a type to the column. See the example below:
```cs
[Column(TypeName = "NVARCHAR(25)")]
public string MyProp { get; set; }
```
5. Replace NVARCHAR(25) with any database type. For a full list of data types, check out <a href="https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver15">HERE</a>

6. If you wish to make a property a primary key, add the "Key" attribute.
```cs
[Key]
[Column(TypeName = "NVARCHAR(25)")]
public string MyProp { get; set; }
```

7. If you wish to establish foreign keys, this can be accomplished by creating a property in the "parent" object that references the "child" object. Then in the "child" object, create a reference to the "parent" object and give it the "Required" attribute.<br/>

Parent Object:
```cs
//Don't specify a column attribute on this
public ChildObject Child { get; set; }
```
Child Object:
```cs
//Don't specify a column attribute on this
[Required]
public ParentObject Parent { get; set; }
```

<ul>
<li style="list-style-type:none;">With the way these objects are currently set up, you will not be able to access them easily. To make access easier, we can setup Lazy Loading. To do this, simply add the virtual keyword to both property declarations.</li>
</ul>

Parent Object:
```cs
//Don't specify a column attribute on this
public virtual ChildObject Child { get; set; }
```
Child Object:
```cs
//Don't specify a column attribute on this
[Required]
public virtual ParentObject Parent { get; set; }
```

7. Once you have created all the properties you need for a given entity, you must create a table to represent the entity.

8. Navigate to the "AppDbContext.cs" class in the "Models/Services" folder. Within this class, add a new public Auto Property of type DbSet< EntityType > and give it the name of the table. It is recommended to make this a plural version of the entity's name.

```cs
public DbSet<MyProp> MyProps { get; set; }
```

8. Once the table is set up, proceed to the database migration section to update the database.

## Database Migrations

Database Migrations are a feature of Entity Framework Core that makes it simple to keep the database's tables and fields up to date with the Application. You will need access to visual studio to perform migrations.

### What is a migration
A migration in this context is a set of instructions that are automatically or manually generated that inform the database on how to maintain specific tables. This could be creating a new table, dropping a old table, adding or removing fields from a table, changing the primary key, etc...

Entity Framework Core allows you to create multiple migrations that apply different changes to the database, allowing for the database to be ever-evolving.

### Creating a Mirgration
To create a migration, begin by opening the Nuget Package Manager Console.
In the Console, Enter the following command:
```
Add-Migration NameOfMigration
```
Replace NameOfMigration with a descriptive name for the migration.
Ex. AddMyPropTable<br/>
Thats it!

### Deleting the most recent migration
If you wish to undo the latest migration you have created (Say in the instance you realize more fields are needed for an entity), the process to undo a migration (assuming the database hasn't already been updated) is rather simple. In the Nuget Package Manager Console, Enter the following command:
```
Remove-Migration
```
This will remove the latest migration, at which point, you can create another when you are ready.<br/>
NOTE: It is highly recommended to undo a migration and make a new one rather than making two seperate migrations during development.

### Updating the database
Now that you have created your migration(s), the next step is actually updating the database. To do this, simpley enter the following command into the Nuget Package Manager Console:
```
Update-Database
```
This will create, update and delete any tables depending on the migrations you created.

### Rolling back the database
Sometimes bad migrations make it to the live database. This can be a big issue. To rollback the database, we can simply run the following command in the Nuget Package Manager Console:
```
Update-Database NameOfMigration
```
Replace NameOfMigration with the name of the migration to roll the database back to.<br/>
NOTE: This process is very dangerous and can lead to data loss. Ensure you only roll back the database if you absolutely have to!