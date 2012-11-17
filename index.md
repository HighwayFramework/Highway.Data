---
layout: default
---
### Getting Started with Highway.Data.EntityFramework


<p>So you want to get started with Highway.Data, but dont know where to start? That is what I am here for. I am going to be your guide to this awesome framework. We are going to start off with something simple and then add to it. You ready? Awesome!</p>  

<p>First thing we want to do is create a new project. I am going to create a console application because it is easier to demo that way, but feel free to use this approach in what ever project you are working on.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_46.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_46.png" width="1352" height="880" /></a></p>  
<p>There is nothing special about this, but we want to open up the Package Manager Console to install Highway.Data.EntityFramework into this application.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_47.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_47.png" width="1351" height="880" /></a></p>  
<p>We do this by typing the command Install-Package Highway.Data.EntityFramework</p>  <blockquote>   
<p>We are not going to bring in an Inversion of Control Container yet, but we will. That walk thru is here. <strong>( link to come )</strong></p> </blockquote>  
<p>I get this when installing, your might be different version numbers.That is simply that I am stuck in time, and you are not.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_48.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_48.png" width="1345" height="312" /></a></p>  
<p>Once we have this installed we are now ready to map our database so that we can jump on the Highway and cruise.</p>  
<p>We need a class called GettingStartedMappings that implements the Highway.Data.IMappingConfiguration. This allows us to use the normal Entity Framework mappings to talk with our database. This is no different now than what we would put in the OnModelBinding event on DbContext. Documentation here (<a title="http://msdn.microsoft.com/en-us/library/gg696169(v=VS.103).aspx" href="http://msdn.microsoft.com/en-us/library/gg696169(v=VS.103).aspx">http://msdn.microsoft.com/en-us/library/gg696169(v=VS.103).aspx</a>).</p>  
<p>We are going to map a couple of tables. I am going to map one Person in line, like so:</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_49.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_49.png" width="678" height="375" /></a></p>  
<p>This maps using the fluent configuration to specify the key and the table name. Anything that adheres to the standard convention doesnt have to be explicitly set. </p>  
<p>I am going to extract the person class from the mappings simply to make our lives easier. So it should look like this.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_50.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_50.png" width="1369" height="271" /></a></p>  
<p>This would be the second way to approach this. I feel this is the better approach because it is more segregated for each entity having its own file. We are going to create a new class file for AccountMap.cs and a new class for Account.</p>  
<p>The AccountMap is going to inherit from System.Data.Entity.ModelConfiguration.EntityTypeConfiguration&lt;T&gt;. This is the EntityFramework built in mapping configuration that allows you to segregate your mapping by entity type. Documentation (<a title="http://msdn.microsoft.com/en-us/library/gg696117(v=VS.103).aspx" href="http://msdn.microsoft.com/en-us/library/gg696117(v=VS.103).aspx">http://msdn.microsoft.com/en-us/library/gg696117(v=VS.103).aspx</a>)</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_51.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_51.png" width="686" height="368" /></a></p>  
<p>Notice how we are able to do each entities mapping in a separate file. All we then have to do is register this into the mapping configuration. Like so:</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_52.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_52.png" width="701" height="428" /></a></p>  
<p>That gets us configured but we still want to be able to use the framework right? Right!</p>  
<p>We need two things to talk to the database in the Highway.Data way. A context (this keeps the information about the database and current object state), and a repository (this gives us a mock-able and testable separation between our DB and our logical code.)</p>  
<p>You are lucky enough to be using a framework developed by lazy guys, we hate having to right classes just to inherit other classes so we prebuilt a context and a repository that have their dependencies injected. You know, like it is supposed to be.</p>  
<p>We will create our context first, for this we need a connection string (just a normal connection string, no meta data here), and our mappings.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_53.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_53.png" width="714" height="193" /></a></p>  
<p>We can use this as is, but we would lose the gloriousness that is repository pattern, and that is bad mmkay! (<a title="http://www.martinfowler.com/eaaCatalog/repository.html" href="http://www.martinfowler.com/eaaCatalog/repository.html">http://www.martinfowler.com/eaaCatalog/repository.html</a>)</p>  
<p>With our repository it should look like this.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_54.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_54.png" width="728" height="206" /></a></p>  
<p>This is the basic start to using Highway.Data.EntityFramework. It is beautiful, simple, elegant, and USELESS!!! 
<img class="wlEmoticon wlEmoticon-smilewithtongueout" style="border-top-style: none; border-left-style: none; border-bottom-style: none; border-right-style: none" alt="Smile with tongue out" src="http://www.devlinliles.com/image.axd?picture=wlEmoticon-smilewithtongueout_1.png" /></p>  
<p>We need to be able to add data and save data right? So lets to do that. Remember that the context is what knows about the database so who do you think is going to have the control of add and save? RIGHT, the context!!</p>  
<p>Lets add the people that made Highway.Data what it is today,and commit that to the database, shall we?</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_55.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_55.png" width="742" height="299" /></a></p>  
<p>This is pretty slick right, but Devlin you say, How can I query objects back from the database?. That is easy, but we have to explain something, it is important I promise. Do you like embedding SQL into your web pages? No, why not? Oh, it is a separation of concerns issue? I agree. So is that any different that putting LINQ everywhere? No, I dont think so either. So we need to isolate our Queries so that we can be more encapsulated. We are going to write a query for finding a person by last name.</p>  
<p>We need a new class FindPersonByLastName that inherits from Highway.Data.QueryObjects.Query&lt;Person&gt;. This gives us the base implementation to make the Command Query Separation that is built into the Highway work. </p>  
<p>Like so:</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_56.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_56.png" width="757" height="302" /></a></p>  
<p>Notice that we set ContextQuery and store a delegate ( lambda ) statement here. This allows us to keep defered execution and still parameterize the constructors of these query objects. I like this approach because paging and sorting are built into the object. We can re-use this without concern for breaking existing code. Open-Close principle is AWESOME!</p>  
<p>When we go to use this, we now just have to pass our query ( or if you like the technical term, our specification ) into the repository and it will execute it on the data needed, without being aware of the details. ( Specification pattern - <a title="Specifications - Martin Fowler" href="http://www.martinfowler.com/apsupp/spec.pdf">Specifications - Martin Fowler</a>)</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_57.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_57.png" width="771" height="333" /></a></p>  
<p>I also want to call your attention to a subtle but important detail. Notice that the results is an IEnumerable&lt;Person&gt;. This means that while we have not broken the deferred execution of this chain of LINQ statements, we have sealed the SQL statement at the point of return. Anything you add outside the query object will not change the dataset returned. You can tack on paging like the example below.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_58.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_58.png" width="785" height="24" /></a></p>  
<p>Now that we have data back, we want to modify and commit those modifications. Really, too easy!!</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_59.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_59.png" width="790" height="102" /></a></p>  
<p>But, Devlin my DBA wants me to fix this slow query and make it a stored proc!  We have that covered too! All you have to do is change the query, not other changes needed.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_60.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_60.png" width="805" height="181" /></a></p>  
<p>The one restriction to this is that the stored procedure needs to return the columns with the right name, type and order to map to the object. This is because we are doing some mapping magic. Injectable mapping is something we have concidered but havent seen enough requests for.</p>  
<p>But, Devlin I want to call a query that only returns one person. I dont want to deal with IEnumerable&lt;Person&gt;  Thanks for the easy one, just use Scalar&lt;T&gt; and while we are talking about it, how about something without a return, like a stored procedure call.</p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_61.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_61.png" width="828" height="220" /></a></p>  
<p><a href="http://www.devlinliles.com/image.axd?picture=image_62.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; padding-top: 0px; padding-left: 0px; display: inline; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_62.png" width="839" height="56" /></a></p>  
<p>We use these in the same way we use a query. Pretty slick huh?</p>  
<p>Well that is a crash course on Highway.Data.EntityFramework. There are WAY more features. Those walk-thrus are coming, but until then.</p>  
<p>&#160;</p>  
<p><font size="4">&#160;<strong>Hop on the Highway and see where it takes you</strong></font></p>

#### [Continue learning about Highway and IoC][ioc]


### Authors and Contributors

This framework was created and is maintained by Tim Rayburn (@trayburn) and Devlin Liles (@devlinliles)

### Support

Feel free to open issues on GitHub if you encounter any issues.  If you need a commercial support license, then feel free to contact us at highway@timrayburn.net and we provide you pricing for these options.

[ioc]: /Highway.Data/ioc.html