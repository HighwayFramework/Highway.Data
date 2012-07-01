# Highway.Data

This project focuses on bringing our recommendations for data access, specifically the Repository and Specification patterns.  For this intial release we've focused on delivering these for Entity Framework, but in the future you can look foward implementations for NHibernate and other data access structures.  If we're going to work with Entity Framework in a code first fashion, we will need some POCOs.  Our guidance with Highway.Data is not to use the Mapping attributes, which instantly make your POCOs aware of your database, but rather to use a mapping class.  We provide an interface you can implement to configure your Entity Framework mappings, it is called IMappingConfiguration.  So let's say we're working with the following POCO:

{% codeblock lang:csharp %}
    public class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
{% endcodeblock %}

We can create a very simply mapping class to store that in the database in the People table:

{% codeblock lang:csharp %}
public class MyMappings : IMappingConfiguration
{
    public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Person>()
            .ToTable("People");
    }
}
{% endcodeblock %}

Now all that is left is to register with our container, and we're off and running.  For this demo, we'll show how to do that with Castle.Windsor:

{% codeblock lang:csharp %}
var container = new WindsorContainer();
container.Register(
    Component.For<IMappingConfiguration>()
        .ImplementedBy<MyMappings>(),
    Component.For<IRepository>()
        .ImplementedBy<EntityFrameworkRepository>(),
    Component.For<IDataContext>()
        .ImplementedBy<EntityFrameworkContext>()
        .DependsOn(new { connectionString = "Connection String Here" }),
    Component.For<IEventManager>()
        .ImplementedBy<EventManager>()
    );
{% endcodeblock %}

We now have a completely wired up Highway.Data implementation, and can resolved IRepository into any of our classes which need to access data.  But that's only the first half of Highway.Data.

## Specification Pattern

In addition to a repository pattern implementation, we also provide an implementation of Specification pattern in Highway.Data.  We use the pattern to ensure our queries are all testable, without access to a database, and also to be able to quickly enumerate, and if necessary generate SQL for, all the queries are project uses.  There are few things that will make a DBA smile more than to learn that a project using an ORM can quickly produce for him or her a list of all queries used by that application.  That said, let me show you how you create a simple query against the LastName property of our Person.

{% codeblock lang:csharp %}
public class LastNameQuery : Query<Person>
{
    public LastNameQuery(string lastName)
    {
        ContextQuery = m => m.AsQueryable<Person>()
            .Where(x => x.LastName == lastName);
    }
}
{% endcodeblock %}

As you can see, we create a class to represent our query, and provide the query implementation to the ContextQuery property.  Once we've created this query, using it is as simple as:

{% codeblock lang:csharp %}
public class DataConsumingClass
{
    private IRepository repo;

    public DataConsumingClass(IRepository repo)
    {
        this.repo = repo;
    }

    public void DoSomeWorkHere()
    {
        var people = repo.Find(new LastNameQuery("Rayburn"));
        // Work with the people returned however you want.
    }
}
{% endcodeblock %}

Obviously, this could be any class in your application, we depend on an IRepository, and hand the Find method our query, and our query it's parameter.  And that's it!

## NuGet Packages

We provide three NuGet Packages:

* [Highway.Data.EntityFramework][hwde-nuget] is the package most people will use, and contains our full Entity Framework implementation.
* [Highway.Data.EntityFramework.Castle][hwdec-nuget] contains a Windsor Installer already setup for everything Highway.Data needs other than your Repository, Context and Mappings.
* [Highway.Data][hwd-nuget] is our core abstractions, without an ORM dependency.


## License

<a rel="license" href="http://creativecommons.org/licenses/by/3.0/"><img alt="Creative Commons License" style="border-width:0" src="http://i.creativecommons.org/l/by/3.0/88x31.png" /></a><br />This work is licensed under a <a rel="license" href="http://creativecommons.org/licenses/by/3.0/">Creative Commons Attribution 3.0 Unported License</a>.


[book]: http://www.packtpub.com/entity-framework-4-1-experts-test-driven-development-architecture-cookbook/book
[ie]: http://improvingenterprises.com
[hw]: http://github.com/HighwayFramework/Highway.Data
[hwweb]: http://highwayframework.github.com/Highway.Data
[hwdoc]: http://highwayframework.github.com/Highway.Data/docs/index.html
[dl]: http://devlinliles.com/
[tr]: http://TimRayburn.net/about/
[hwd-nuget]: https://nuget.org/packages/Highway.Data
[hwde-nuget]: https://nuget.org/packages/Highway.Data.EntityFramework
[hwdec-nuget]: https://nuget.org/packages/Highway.Data.EntityFramework.Castle