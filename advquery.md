---
layout: default
---
### Highway.Data and Advanced Query Scanarios
    
<p>
<strong>
<em>
<u>
<font size="5">*This is written for version 3.5 and later*</font>
</u>
</em>
</strong>
</p>  <p>Alright, you are feeling great about Highway.Data and your application. You are running right along with Entity Framework, Nhibernate, or RavenDB etc… and you are top of the world.</p>  <p>Then you need to call a stored procedure using Entity Framework and *SCREEEECH* you find yourself looking around and not knowing how to get home? The answer is of course, “Hop on the highway” We cover this scenario, along with all other scenarios where you need to move just a little bit off the beaten path but don’t want to throw the baby out with the bath water. We can keep our query objects, and all the special amazing things they bring to the table and still work down on the “Iron” of the implementation we have chosen.</p>  <p>We have chosen Entity Framework for this example, but it holds true for NHibernate, RavenDB and any other implementation of Highway.Data.</p>  <p>We start off with out GettingStarted Application ( a console app this time ). We are going to reuse the person/account domain from the first tutorial. Here is your 30 second review.</p>  <pre class="csharpcode">
<span class="kwrd">namespace</span> Highway.Data.GettingStarted
{
    <span class="kwrd">public</span> <span class="kwrd">class</span> Person
    {
        <span class="kwrd">public</span> <span class="kwrd">int</span> Id { get; set; }
        <span class="kwrd">public</span> <span class="kwrd">string</span> FirstName { get; set; }
        <span class="kwrd">public</span> <span class="kwrd">string</span> LastName { get; set; }
    }
}</pre>
<style type="text/css">
.csharpcode, .csharpcode pre
{
	font-size: small;
	color: black;
	font-family: consolas, "Courier New", courier, monospace;
	background-color: #ffffff;
	/*white-space: pre;*/
}
.csharpcode pre { margin: 0em; }
.csharpcode .rem { color: #008000; }
.csharpcode .kwrd { color: #0000ff; }
.csharpcode .str { color: #006080; }
.csharpcode .op { color: #0000c0; }
.csharpcode .preproc { color: #cc6633; }
.csharpcode .asp { background-color: #ffff00; }
.csharpcode .html { color: #800000; }
.csharpcode .attr { color: #ff0000; }
.csharpcode .alt 
{
	background-color: #f4f4f4;
	width: 100%;
	margin: 0em;
}
.csharpcode .lnum { color: #606060; }</style>



<p>
  <pre class="csharpcode">
<span class="kwrd">namespace</span> Highway.Data.GettingStarted.Domain.Entities
{
    <span class="kwrd">public</span> <span class="kwrd">class</span> Account
    {
        <span class="kwrd">public</span> <span class="kwrd">int</span> AccountId { get; set; }

        <span class="kwrd">public</span> <span class="kwrd">string</span> AccountName { get; set; }
    }
}</pre>
  <style type="text/css">
.csharpcode, .csharpcode pre
{
	font-size: small;
	color: black;
	font-family: consolas, "Courier New", courier, monospace;
	background-color: #ffffff;
	/*white-space: pre;*/
}
.csharpcode pre { margin: 0em; }
.csharpcode .rem { color: #008000; }
.csharpcode .kwrd { color: #0000ff; }
.csharpcode .str { color: #006080; }
.csharpcode .op { color: #0000c0; }
.csharpcode .preproc { color: #cc6633; }
.csharpcode .asp { background-color: #ffff00; }
.csharpcode .html { color: #800000; }
.csharpcode .attr { color: #ff0000; }
.csharpcode .alt 
{
	background-color: #f4f4f4;
	width: 100%;
	margin: 0em;
}
.csharpcode .lnum { color: #606060; }</style>
</p>



<pre class="csharpcode">
<span class="kwrd">using</span> System.Data.Entity;
<span class="kwrd">using</span> Highway.Data.GettingStarted.Domain.Entities;

<span class="kwrd">namespace</span> Highway.Data.GettingStarted.DataAccess.Mappings
{
    <span class="kwrd">public</span> <span class="kwrd">class</span> GettingStartedMappings : IMappingConfiguration
    {
        <span class="kwrd">public</span> <span class="kwrd">void</span> ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            <span class="rem">//Approach 1</span>
            modelBuilder.Entity&lt;Person&gt;().HasKey(x=&gt;x.Id).ToTable(<span class="str">&quot;People&quot;</span>);

            <span class="rem">//Approach 2</span>
            modelBuilder.Configurations.Add(<span class="kwrd">new</span> AccountMap());
        }
    }
}</pre>
<style type="text/css">
.csharpcode, .csharpcode pre
{
	font-size: small;
	color: black;
	font-family: consolas, "Courier New", courier, monospace;
	background-color: #ffffff;
	/*white-space: pre;*/
}
.csharpcode pre { margin: 0em; }
.csharpcode .rem { color: #008000; }
.csharpcode .kwrd { color: #0000ff; }
.csharpcode .str { color: #006080; }
.csharpcode .op { color: #0000c0; }
.csharpcode .preproc { color: #cc6633; }
.csharpcode .asp { background-color: #ffff00; }
.csharpcode .html { color: #800000; }
.csharpcode .attr { color: #ff0000; }
.csharpcode .alt 
{
	background-color: #f4f4f4;
	width: 100%;
	margin: 0em;
}
.csharpcode .lnum { color: #606060; }</style>

<p>
<u>
</u>
</p>

<pre class="csharpcode">
<span class="kwrd">using</span> System;
<span class="kwrd">using</span> System.Collections.Generic;
<span class="kwrd">using</span> System.Linq;
<span class="kwrd">using</span> System.Text;
<span class="kwrd">using</span> System.Threading.Tasks;
<span class="kwrd">using</span> Highway.Data.GettingStarted.DataAccess.Mappings;

<span class="kwrd">namespace</span> Highway.Data.GettingStarted.AdvancedQueries
{
    <span class="kwrd">class</span> Program
    {
        <span class="kwrd">static</span> <span class="kwrd">void</span> Main(<span class="kwrd">string</span>[] args)
        {
            var context = <span class="kwrd">new</span> DataContext(<span class="str">&quot;Data Source=(local);Initial Catalog=GettingStarted;Integrated Security=true;&quot;</span>,
                           <span class="kwrd">new</span> GettingStartedMappings());
            var repository = <span class="kwrd">new</span> Repository(context);
        }
    }
}</pre>
<style type="text/css">
.csharpcode, .csharpcode pre
{
	font-size: small;
	color: black;
	font-family: consolas, "Courier New", courier, monospace;
	background-color: #ffffff;
	/*white-space: pre;*/
}
.csharpcode pre { margin: 0em; }
.csharpcode .rem { color: #008000; }
.csharpcode .kwrd { color: #0000ff; }
.csharpcode .str { color: #006080; }
.csharpcode .op { color: #0000c0; }
.csharpcode .preproc { color: #cc6633; }
.csharpcode .asp { background-color: #ffff00; }
.csharpcode .html { color: #800000; }
.csharpcode .attr { color: #ff0000; }
.csharpcode .alt 
{
	background-color: #f4f4f4;
	width: 100%;
	margin: 0em;
}
.csharpcode .lnum { color: #606060; }</style>

<p>&#160;</p>

<pre class="csharpcode">
<span class="kwrd">using</span> System.Data.Entity.ModelConfiguration;
<span class="kwrd">using</span> Highway.Data.GettingStarted.Domain.Entities;

<span class="kwrd">namespace</span> Highway.Data.GettingStarted.DataAccess.Mappings
{
    <span class="kwrd">public</span> <span class="kwrd">class</span> AccountMap : EntityTypeConfiguration&lt;Account&gt;
    {
        <span class="kwrd">public</span> AccountMap()
        {
            <span class="kwrd">this</span>.ToTable(<span class="str">&quot;Accounts&quot;</span>);
            <span class="kwrd">this</span>.HasKey(x =&gt; x.AccountId);
            <span class="kwrd">this</span>.Property(x =&gt; x.AccountName).HasColumnType(<span class="str">&quot;text&quot;</span>);
        }
    }
}</pre>
<style type="text/css">
.csharpcode, .csharpcode pre
{
	font-size: small;
	color: black;
	font-family: consolas, "Courier New", courier, monospace;
	background-color: #ffffff;
	/*white-space: pre;*/
}
.csharpcode pre { margin: 0em; }
.csharpcode .rem { color: #008000; }
.csharpcode .kwrd { color: #0000ff; }
.csharpcode .str { color: #006080; }
.csharpcode .op { color: #0000c0; }
.csharpcode .preproc { color: #cc6633; }
.csharpcode .asp { background-color: #ffff00; }
.csharpcode .html { color: #800000; }
.csharpcode .attr { color: #ff0000; }
.csharpcode .alt 
{
	background-color: #f4f4f4;
	width: 100%;
	margin: 0em;
}
.csharpcode .lnum { color: #606060; }</style>

<p>&#160;</p>



<p>Ok, now that we have our domain objects, their mappings, and the repository and context built we need to execute some embedded SQL *I Know….* against Entity Framework but within a query object for consistency. We need to write the Query first.</p>

<pre class="csharpcode">
<span class="kwrd">using</span> System.Linq;
<span class="kwrd">using</span> Highway.Data.GettingStarted.Domain.Entities;
<span class="kwrd">using</span> Highway.Data.QueryObjects;

<span class="kwrd">namespace</span> Highway.Data.GettingStarted.AdvancedQueries
{
    <span class="kwrd">public</span> <span class="kwrd">class</span> FindPersonByLastNameEmbeddedSQLQuery : AdvancedQuery&lt;Person&gt;
    {
        <span class="kwrd">public</span> FindPersonByLastNameEmbeddedSQLQuery(<span class="kwrd">string</span> lastName)
        {
            ContextQuery = <span class="kwrd">delegate</span>(DataContext context)
                {
                    var sql = <span class="str">&quot;Select * from People p where p.LastName = @last&quot;</span>;
                    var results = context.Database.SqlQuery&lt;Person&gt;(sql, lastName);
                    <span class="kwrd">return</span> results.AsQueryable();
                };
        }
    }
}</pre>
<style type="text/css">
.csharpcode, .csharpcode pre
{
	font-size: small;
	color: black;
	font-family: consolas, "Courier New", courier, monospace;
	background-color: #ffffff;
	/*white-space: pre;*/
}
.csharpcode pre { margin: 0em; }
.csharpcode .rem { color: #008000; }
.csharpcode .kwrd { color: #0000ff; }
.csharpcode .str { color: #006080; }
.csharpcode .op { color: #0000c0; }
.csharpcode .preproc { color: #cc6633; }
.csharpcode .asp { background-color: #ffff00; }
.csharpcode .html { color: #800000; }
.csharpcode .attr { color: #ff0000; }
.csharpcode .alt 
{
	background-color: #f4f4f4;
	width: 100%;
	margin: 0em;
}
.csharpcode .lnum { color: #606060; }</style>

<p>Notice it inherits from AdvancedQuery instead of Query. This is to make sure you know you are using coupling behavior that will not be automatically portable to another data access implementation.</p>

<p>Secondly I use a delegate definition there to make sure you can see that you get the DataContext which is a <a href="http://msdn.microsoft.com/en-us/library/system.data.entity.dbcontext(v=VS.103).aspx">DbContext</a>. This gives you everything Entity Framework has to offer there. You just get to use it, the same way you use other queries.</p>

<pre class="csharpcode">IEnumerable&lt;Person&gt; results = 
     repository.Find(<span class="kwrd">new</span> FindPersonByLastNameEmbeddedSQLQuery(<span class="str">&quot;Liles&quot;</span>));</pre>
<style type="text/css">
.csharpcode, .csharpcode pre
{
	font-size: small;
	color: black;
	font-family: consolas, "Courier New", courier, monospace;
	background-color: #ffffff;
	/*white-space: pre;*/
}
.csharpcode pre { margin: 0em; }
.csharpcode .rem { color: #008000; }
.csharpcode .kwrd { color: #0000ff; }
.csharpcode .str { color: #006080; }
.csharpcode .op { color: #0000c0; }
.csharpcode .preproc { color: #cc6633; }
.csharpcode .asp { background-color: #ffff00; }
.csharpcode .html { color: #800000; }
.csharpcode .attr { color: #ff0000; }
.csharpcode .alt 
{
	background-color: #f4f4f4;
	width: 100%;
	margin: 0em;
}
.csharpcode .lnum { color: #606060; }</style>

<p>&#160;</p>

<p>It is that simple. *For NHibernate you get an ISession, and for Raven you get an IDocumentSession*</p>

<p>“Hop on the highway and see where it takes you”</p>


#### [Go Back to Getting Started with IoC][ioc]

[ioc]: /Highway.Data/ioc.html