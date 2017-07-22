# Building a Widget
Refer to Widgets to learn more about widgets.Refer to [Adding A Widget](Adding-A-Widget) for information about adding a widget to Graffiti.

To build your own widget, you need to complete the following steps:

# Create a class which derives from Graffiti.Core.Widget and is marked as being Serializable.
# Add the WidgetInfo attribute and specify a unique ID (Guid), a name, and a description.
# Implement the abstract methods of Widget (RenderData and Name) described below.
# Compile and add to the bin directory.