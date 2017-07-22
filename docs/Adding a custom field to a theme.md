# Adding a Custom Field to a Theme
Having created a custom field and published a post, you can add the custom field to your theme.

You should remember that custom fields are not available to the user until they are edited in the theme.

You can edit the theme by doing either of the following:

* Opening the .view file in your computer and adding the custom field directly to the file
* Navigating to the .view files in Graffiti. Refer to [themes](Themes) for additional information.
The pattern for accessing custom fields is $post.Custom("Custom Field Name"). For example, to add the data from the "Sample" custom field to your .view file, you enter the following: $post.Custom("Sample").

Custom fields are not validated, so they can be empty. By default, [Chalk](Chalk-Overview) will render $post.Custom("Sample") if the value does not exist. To make sure the Chalk is not visible when it does not exist, you could add a "!" after the $ like this: $!post.Custom("Sample"). This tells Chalk to not show itself when empty.
