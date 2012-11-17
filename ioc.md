---
layout: default
---
### Highway.Data.EntityFramework -- Getting Started with IoC

<p>So you have now gotten comfortable with Highway.Data.EntityFramework and decided that you want another sharp poke in the eye with a stick of Awesome!? Great! We have just the awesome stick, IoC!
</p>  

<p>If you are not familiar with the concept… allow me to explain in true internet fashion.
</p>  

<p>
<a title="http://en.wikipedia.org/wiki/Inversion_of_control" href="http://en.wikipedia.org/wiki/Inversion_of_control">http://en.wikipedia.org/wiki/Inversion_of_control
</a>
</p>  

<p>
<a title="http://msdn.microsoft.com/en-us/library/aa973811.aspx" href="http://msdn.microsoft.com/en-us/library/aa973811.aspx">http://msdn.microsoft.com/en-us/library/aa973811.aspx
</a> – The funny one to me is the MSDN article uses Castle.Windsor not Microsoft’s Unity…… irony.
</p>  

<p>&#160;
</p>  

<p>So lets get going on IoC. To make this example slightly harder to follow and more realistic we are going to jump into our first application, but with some architectural refactoring done.
</p>  

<p>It looks something like this…. well exactly like this.
</p>  

<p>
<a href="http://www.devlinliles.com/image.axd?picture=image_63.png">
<img title="image" style="float: none; margin: 0px auto; display: block" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_63.png" width="265" height="539" />
</a>
</p>  

<p>&#160;
</p>  

<p>The first thing I did was pull the domain objects into their own project, then I moved all data access specific code into it’s own project. This took all of 3 minutes with 
<a href="http://www.jetbrains.com/resharper/">Resharper
</a>… have I mentioned how awesome 
<a href="http://www.jetbrains.com/resharper/">Resharper
</a> is? No, well it is one step shy of my wife’s home made Cherry pie!!! 
<a href="http://www.ncrunch.net/">NCrunch
</a> is better than her home made Cherry pie!! But I digress…
</p>  

<p>We want to start in the MVC project down at the bottom there. We are going to wire IoC into MVC so that we get the full effect of this. Then we will leverage our new found powers to make Home –&gt; Index even better!!
</p>  

<p>First we need to bring in our IoC of choice. Highway.Data supports any IoC that adheres to Common Service Locator. We have also gone through and written several adapters to make your life easier.
</p>  

<p>So pick… ( if you pick something other than Castle.Windsor your mapping syntax will be different than the examples – You have been warned ).
</p>  

<p>&#160;
</p>  

<p>
<a href="http://www.devlinliles.com/image.axd?picture=image_64.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; float: none; padding-top: 0px; padding-left: 0px; margin: 0px auto; display: block; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_64.png" width="453" height="127" />
</a>
</p>  

<p>&#160;
</p>  

<p>I am picking Castle, and I get this output from Package Manager.
</p>  

<p>&#160;
</p>  

<p>
<a href="http://www.devlinliles.com/image.axd?picture=image_65.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; float: none; padding-top: 0px; padding-left: 0px; margin: 0px auto; display: block; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_65.png" width="585" height="244" />
</a>
</p>  

<p>&#160;
</p>  

<p>This get my IoC and the Highway.Data.EntityFramework bootstrap in place. Now all we have to do is wire this into the global.asax.
</p>  

<p>&#160;
</p>  

<p>
<a href="http://www.devlinliles.com/image.axd?picture=image_66.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; float: none; padding-top: 0px; padding-left: 0px; margin: 0px auto; display: block; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_66.png" width="600" height="277" />
</a>
</p>  

<p>Notice the highlighted part? It is highlighted because I 
<strong>
<em>
<u>HATE
</u>
</em>
</strong> when there is a ton of stuff plugged right into the App_Start method. Please, extract it to a method. 
<strong>
<em>
<u>I BEG YOU!
</u>
</em>
</strong>
</p>  

<p>Now back to our regularly scheduled tutorial….
</p>  

<p>In the WireUpIoC method we will……wire up the IoC. Like so:
</p>  

<p>
<a href="http://www.devlinliles.com/image.axd?picture=image_67.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; float: none; padding-top: 0px; padding-left: 0px; margin: 0px auto; display: block; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_67.png" width="600" height="337" />
</a>
</p>  

<p>Notice here we wire up Highway.Data’s Repository, Context, some default logging, and context configuration. We do however load up the mappings specific to our implementation. *That is important*
</p>  

<p>We then just need a controller factory for MVC, and luckily I have one.
</p>  

<p>
<a href="http://www.devlinliles.com/image.axd?picture=image_68.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; float: none; padding-top: 0px; padding-left: 0px; margin: 0px auto; display: block; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_68.png" width="600" height="282" />
</a>
</p>  

<p>And just like that we are wired up. We just have to use it now, like so:
</p>  

<p>
<a href="http://www.devlinliles.com/image.axd?picture=image_69.png">
<img title="image" style="border-left-width: 0px; border-right-width: 0px; background-image: none; border-bottom-width: 0px; float: none; padding-top: 0px; padding-left: 0px; margin: 0px auto; display: block; padding-right: 0px; border-top-width: 0px" border="0" alt="image" src="http://www.devlinliles.com/image.axd?picture=image_thumb_69.png" width="600" height="210" />
</a>
</p>  

<p>We simply inject the repository via constructor and then use it. The IoC supplies a new version each time, and we are off to the races!!!
</p>  

<p>
<strong>“Hop on the Highway and see where it takes you”
</strong>
</p>

#### [Go Back to Getting Started with Highway.Data.EntityFramework][index]
#### [Continue with Highway.Data and Advanced Query Scanarios][advquery]

[index]: /Highway.Data/index.html
[advquery]: /Highway.Data/advquery.html
