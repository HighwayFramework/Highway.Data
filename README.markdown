# Highway.Data
## The fastest and smoothest way to great architecture 

# Documentation

We encourage you to check out our documentation site for the Highway Framework at [http://hwyfwk.com](http://hwyfwk.com), specifically the [Highway.Data project site](http://hwyfwk.com/projects/data/index.html), and [the Features documentation](http://hwyfwk.com/projects/data/features.html) that documents how every part of Highway.Data works.  Below we've reproduced our getting started section to make things easy for you, but full documentation is on the [Highway.Data project site](http://hwyfwk.com/projects/data/index.html).

# [Getting Started](http://hwyfwk.com/projects/data/start.html)
So you want to get started with Highway.Data, but don’t know where to start? Lets start the same place we always start with something new. "Tests!!!"

``` csharp
[TestClass]
public class DriversEducationServiceTests
{
   [TestMethod]
   public void ShouldAddDriverByName()
   {
		//arrange
		var context = new InMemoryDataContext();
		var service = new DriversEducationService(new Repository(context));
		 
		//act
		service.AddDriver("Devlin");
    
		//assert
		context.AsQueryable<Driver>().Count(x => x.Name == "Devlin").Should().Be(1);
	}
}
```

Now that we have a test, lets work on an implementation. In this implementation we are going to focus on the patterns in Highway.Data. We will dive into Entity Framework, nHibernate, and RavenDb later. In order to facilitate this, we are going to use the InMemoryDataContext that ships with Highway.Data as our test stub.

We just need to add a driver to the database. 

``` csharp
    public class DriversEducationService
    {
        private readonly IRepository _repository;

        public DriversEducationService(IRepository repository)
        {
            _repository = repository;
        }

        public void AddDriver(string name)
        {
            _repository.Context.Add(new Driver(name));
            _repository.Context.Commit();
        }
    }

    public class Driver
    {
        public string Name { get; set; }

        public Driver(string name)
        {
            Name = name;
        }
    }
```
    
Once we have the repository, we can use the Context on Repository as a unit of work. Check [here](http://www.martinfowler.com/eaaCatalog/repository.html "Repository Pattern") and [here](http://www.martinfowler.com/eaaCatalog/unitOfWork.html "Unit of Work Pattern") read up on Repository or Unit of Work. 

This gives us the ability to separate the persistence from the business logic of our services.

Once we have added an object we will obviously need to query the object back out. For this we will need to use a mixture of [Specification Pattern](http://en.wikipedia.org/wiki/Specification_pattern "Specification Pattern") and [Query Object Pattern](http://martinfowler.com/eaaCatalog/queryObject.html "Query Object Pattern"). Below is the test for querying back an object. 

``` csharp
    [TestMethod]
    public void ShouldQueryDriversByName()
    {
        //arrange 
        var context = new InMemoryDataContext();
        context.Add(new Driver("Devlin"));
        context.Add(new Driver("Tim"));
        context.Commit();

        var service = new DriversEducationService(new Repository(context));

        //act
        Driver driver = service.GetDriver("Devlin");

        //assert
        driver.Should().NotBeNull();
    }
```

And our service code that passes the test.

``` csharp
    public Driver GetDriver(string name)
    {
        return _repository.Find(new DriverByName(name));
    }
```

With the following query.

``` csharp
    public class DriverByName : Scalar<Driver>
    {
        public DriverByName(string name)
        {
            ContextQuery = c => c.AsQueryable<Driver>().Single(x => x.Name == name);
        }
    }
```

If we want to query a collection of entities, that is as simple as changing the base class our query object inherits from.

``` csharp
    public class DriversByName : Query<Driver>
    {
        public DriversByName(string name)
        {
            ContextQuery = c => c.AsQueryable<Driver>().Where(x => x.Name == name);
        }
    }
```

Built into the framework is the ability to fire off commands that affect the database but don't return a value. The syntax is very similar.

``` csharp
    public void RemoveDriversByName(string name)
    {
        _repository.Execute(new RemoveDrivers(name));
    }

    public class RemoveDrivers : Command
    {
        public RemoveDrivers(string name)
        {
            ContextQuery = c =>
            {
                foreach (var driver in c.AsQueryable<Driver>().Where(x=>x.Name == name))
                {
                    c.Remove(driver);
                }
                c.Commit();
            };
        }
    }
```

