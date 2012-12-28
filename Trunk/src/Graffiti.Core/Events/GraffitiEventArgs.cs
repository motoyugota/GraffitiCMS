using System;

namespace Graffiti.Core
{
	public enum PostRenderLocation
	{
		Web = 0,
		Feed = 1,
		Other = 2
	}

	public class PostEventArgs : EventArgs
	{
		private Post _post;

		private PostRenderLocation _renderLocation;

		public PostEventArgs(Post post, PostRenderLocation renderLocation)
		{
			_post = post;
			_renderLocation = renderLocation;
		}

		public Post Post
		{
			get { return _post; }
		}

		public PostRenderLocation RenderLocation
		{
			get { return _renderLocation; }
		}
	}
}