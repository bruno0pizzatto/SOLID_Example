// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using SOLID_Example.Databases;
using SOLID_Example.Interfaces;
using SOLID_Example.Models;
using SOLID_Example.Services;

Console.WriteLine("BANCO SQLITE 1 - BANCO LITEDB 2");
var selectDatabaseType = int.Parse(Console.ReadLine());

var collectionService = new ServiceCollection();

SetupServices(collectionService, (EDatabaseType)selectDatabaseType);
var buildedService = collectionService.BuildServiceProvider();


//User

Console.ForegroundColor = ConsoleColor.DarkGreen;

var userOne = new User("Bruno Pizzatto", "bruno0pizzatto@gmail.com");
var userTwo = new User("Pedro Pizzatto", "pedro0pizzatto@gmail.com");

UserService userService = new(buildedService.GetService<IDatabase<User>>());
userService.Add(userOne);
userService.Add(userTwo);

foreach(var user in userService.GetAll())
{
    Console.WriteLine(user.ToString());
}

static void SetupServices(ServiceCollection services, EDatabaseType databaseType)
{
    if (databaseType == EDatabaseType.SQLite)
    {
        services.AddSingleton(typeof(IDatabase<>), typeof(SQLiteManager<>));

        var repository = new SQLiteManager<Repository>();
        repository.CleanAll();
        repository.Dispose();

        var user = new SQLiteManager<User>();
        user.CleanAll();
        user.Dispose();
    }
    else
    {
        services.AddSingleton(typeof(IDatabase<>), typeof(SQLiteManager<>));

        var repository = new SQLiteManager<Repository>();
        repository.CleanAll();
        repository.Dispose();

        var user = new SQLiteManager<User>(); 
        user.CleanAll();
        user.Dispose();
    }
}