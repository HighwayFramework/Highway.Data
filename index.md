---
layout: default
---
### Getting Started

The easiest way to get started is to simply wire up Highway.Data to your existing IoC container.  For these examples we will use the Castle.Windsor syntax:

{% highlight csharp %}
container.Register(
	Component.For<IRepository>()
		.ImplementedBy<EntityFrameworkRepository>(),
	Component.For<IDataContext>()
		.ImplementedBy<EntityFrameworkContext>()
		.DependsOn(new { connectionString = "Connection String" }),
	Component.For<IEventHandler>()
		.ImplementedBy<EventHandler>(),
	Component.For<IMappingConfiguration>()
		.ImplementedBy<YourMappingClass>(),
);
{% endhighlight %}

### Creating Your Mappings

With Highway we do not try to reinvent the wheel, so every ORM implementation will have a slightly different mapping structure that uses the syntax from that ORM.  This is because Highway is not meant to replace your ORM, but rather provide tested pattern implementations.  For EntityFramework, we need to create a class which implements IMappingConfiguration, as we mentioned above in our registrations.  This interface will be injected into the context for you, and gives you access to Entity Framework's DbModelBuilder.

{% highlight csharp %}
public class YourMappings : IMappingConfiguration
{
    public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Person>()
            .ToTable("People");
    }
}
{% endhighlight %}

### Authors and Contributors

This framework was created and is maintained by Tim Rayburn (@trayburn) and Devlin Liles (@devlinliles)

### Support

Feel free to open issues on GitHub if you encounter any issues.  If you need a commercial support license, then feel free to contact us at highway@timrayburn.net and we provide you pricing for these options.
