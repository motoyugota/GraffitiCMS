# Chalk Overview
Chalk, which is a combination of NVelocity and Graffiti goodies, is a simple templating language. Graffiti uses Chalk to render themes. 

Chalk is much simpler than anything you have used to build web pages in the past. With this system, you can write your HTML/CSS as you would normally and then decorate it with Chalk. When Graffiti renders a view file, it will find all of your Chalk items and replace them with your site content. 

Here are some of the more common things you will do with Chalk.

## Properties
Format $ItemName.PropertyName properties allow you access to additional information about an item. For example, to display the title of a post, you use $post.Title.

## Methods
Methods are similar to properties except they usually require you to supply additional information. For example, to render a list of comma separated tags as links for the current post but want to avoid writing a lot of Chalk, you can use the TagList method on the macros helper: $macros.TagList($post.TagList). Here we are passing in the $post.TagList property to the macros.TagList method.

## Conditions
There are times you will want content displayed only if certain criteria is met. To do this, you can use an if/elseif/else.

Most conditional operators are supported: (==, >, >=, <, <=, !=)

For example, render the CSS class if the post and comment have the same user:
{code:html}
#if($comment.CreatedBy == $post.CreatedBy) class="author" #end
{code:html}
If the values are the same, the values after the second ) and before #end will be rendered to the browser.


You can also use an else and else if:
{code:html}
#if($where == "category")
    <h2 class="archive_head">Entries categorized '$category.Name'</h2>
#elseif($where == "tag")
    <h2 class="archive_head">Entries tagged '$tag'</h2>
#elseif($where == "search")
    <h2 class="archive_head">Search Results for '$macros.SearchQuery'</h2>
#else
    <h2 class="archive_head">Welcome to my site!</h2>
#end
{code:html}
In the example above, we want to render an H2 element depending on the value of the $where value. If we do not find a match, the #else will be used.

## Loops
Chalk allows you to quickly and easily loop through a site of items and list them. In addition, while in the loop, there is a special property called $count which lets you know what iteration you are on.

For example, the following sample code would write an unordered list of posts as a link with the title of the post as link text.
{code:html}
#foreach($post in $posts)
    <li><a href="$post.Url">$post.Title</a></li>
#end
{code:html}

This is simple enough, but it can begin to get complicated when you want apply different formatting for alternating rows, handle the first or last item differently, or display a special message when no items are present. However, there is a really simple syntax you can use to handle all of this and more.

{code:html}
#foreach($post in $posts)

#each (this is optional since it’s the default section)
       text which appears for each item
#before
       text which appears before each item
#after
       text which appears after each item
#between
       text which appears between each two items
#odd
       text which appears for every other item, including the first
#even
       text which appears for every other item, starting with the second
#nodata
       Content rendered if $items evaluated to null or empty
#beforeall
       text which appears before the loop, only if there are items
       matching condition
#afterall
       text which appears after the loop, only of there are items
       matching condition

#end
{code:html}
All inner sections are optional, and they can appear in any order multiple times (sections with same name will have their content appended)

## Commenting
Two pound signs will hide a line, preventing it from being processed or displayed in the final output. This allows for easy commenting or to temporarily remove lines of code.

{code:html}
##start a loop here
#foreach($post in $posts)
    ##return this html for every post in the posts collection
    <li><a href="$post.Url">$post.Title</a></li>
    ##this is the old markup from a previous revision
    ##<li><a href="$post.Url">$post.Name</a></li>
##end the loop here
#end
{code:html}

## NVelocity
NVelocity is a port of the java Velocity project. There is little documentation on NVelocity, but the syntax is exactly the same, so if you want to understand this on a very low level, [this is the place to start](http://velocity.apache.org/engine/releases/velocity-1.5/vtl-reference-guide.html).

Graffiti uses a version of NVelocity which has been [updated](http://www.castleproject.org/others/nvelocity/improvements.html) by the [Castle Project](http://www.castleproject.org/) team. Many thanks to this community for some very welcomed additions.  In particular, this version of NVelocity added the special for each loop processing.
