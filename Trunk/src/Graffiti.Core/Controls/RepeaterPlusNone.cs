//------------------------------------------------------------------------------
// <copyright company="Telligent Systems">
//     Copyright (c) Telligent Systems Corporation.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Graffiti.Core
{
	/// <summary>
	///     This class is based on the <see cref="Repeater" /> class.  It allows you to define a NoneTemplate that
	///     will be displayed when there are no items found in the data source.
	/// </summary>
	public class Repeater : System.Web.UI.WebControls.Repeater
	{
		#region Public Properties

		/// <summary>
		///     The template that is used to define what is displayed when no items are found.
		/// </summary>
		[Browsable(false), DefaultValue(null),
		 Description("Defines the ITemplate to be used when no items are defined in the datasource."),
		 PersistenceMode(PersistenceMode.InnerProperty),]
		public virtual ITemplate NoneTemplate { get; set; }

		[DefaultValue(false)]
		public virtual bool ShowHeaderFooterOnNone
		{
			get { return (bool) (ViewState["ShowHeaderFooterOnNone"] ?? false); }
			set { ViewState["ShowHeaderFooterOnNone"] = value; }
		}

		#endregion

		#region OnDataBinding

		/// <exclude />
		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			if ((Items.Count == 0) && (NoneTemplate != null))
			{
				Controls.Clear();

				if (ShowHeaderFooterOnNone && (HeaderTemplate != null))
				{
					RepeaterItem headerItem = CreateItem(-1, ListItemType.Header);
					RepeaterItemEventArgs headerArgs = new RepeaterItemEventArgs(headerItem);
					InitializeItem(headerItem);
					OnItemCreated(headerArgs);
					Controls.Add(headerItem);
					headerItem.DataBind();
					OnItemDataBound(headerArgs);
				}

				// Process the NoneTemplate
				RepeaterItem noneItem = new RepeaterItem(-1, ListItemType.Item);
				RepeaterItemEventArgs noneArgs = new RepeaterItemEventArgs(noneItem);
				NoneTemplate.InstantiateIn(noneItem);
				OnItemCreated(noneArgs);
				Controls.Add(noneItem);
				OnNoneItemsDataBound(noneArgs);

				if (ShowHeaderFooterOnNone && (FooterTemplate != null))
				{
					RepeaterItem footerItem = CreateItem(-1, ListItemType.Footer);
					RepeaterItemEventArgs footerArgs = new RepeaterItemEventArgs(footerItem);
					InitializeItem(footerItem);
					OnItemCreated(footerArgs);
					Controls.Add(footerItem);
					footerItem.DataBind();
					OnItemDataBound(footerArgs);
				}

				ChildControlsCreated = true;
			}
		}

		#endregion

		#region Event Handlers

		private static readonly object EventNoneItemsDataBound = new object();

		/// <summary>
		///     This event is called when no items were found in the data source.
		/// </summary>
		public event RepeaterItemEventHandler NoneItemsDataBound
		{
			add { base.Events.AddHandler(EventNoneItemsDataBound, value); }
			remove { base.Events.RemoveHandler(EventNoneItemsDataBound, value); }
		}

		/// <summary>
		///     This method is called to invoke the <see cref="NoneItemsDataBound" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="RepeaterItemEventArgs" /> to be passed to the event.
		/// </param>
		protected virtual void OnNoneItemsDataBound(RepeaterItemEventArgs e)
		{
			RepeaterItemEventHandler handler = (RepeaterItemEventHandler) base.Events[EventNoneItemsDataBound];
			if (handler != null)
				handler(this, e);
		}

		#endregion
	}
}