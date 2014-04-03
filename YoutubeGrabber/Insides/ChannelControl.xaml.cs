namespace YoutubeGrabber
{
	#region

	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;

	using Common;

	#endregion

	/// <summary>
	///     Interaction logic for ChannelControl.xaml
	/// </summary>
	public partial class ChannelControl : UserControl
	{
		#region Fields

		public ChannelInfo DataObject;

		private YoutubeGrabberMainControl parentControl;

		private bool selected;

		#endregion

		#region Constructors and Destructors

		public ChannelControl(ChannelInfo dataObject, YoutubeGrabberMainControl parentControl)
		{
			this.InitializeComponent();

			this.DataObject = dataObject;
			this.parentControl = parentControl;

			this.Text.Content = dataObject.Name
				.Replace("&#39;","");
			this.subs.Content = "(" + dataObject.Subscribers + ")";

			this.Loaded += this.OnLoaded;
			this.Unloaded += this.OnUnloaded;
		}

		#endregion

		#region Public Properties

		public bool Picked
		{
			get
			{
				return this.selected;
			}
			set
			{
				this.selected = value;

				if (value == true)
				{
					VisualStateManager.GoToElementState(this.LayoutRoot, "Selected", false);
				}
				else
				{
					VisualStateManager.GoToElementState(this.LayoutRoot, "Default", false);
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Update()
		{
			if (this.DataObject.bBad < 0)
			{
				this.LayoutRoot.Background = new SolidColorBrush(Color.FromRgb(255, 137, 137));
			}
			else if (this.DataObject.bBad > 0)
			{
				this.LayoutRoot.Background = new SolidColorBrush(Color.FromRgb(190, 240, 190));
			}
			else
			{
				this.LayoutRoot.Background = new SolidColorBrush(Color.FromRgb(184, 181, 181));
			}
		}

		#endregion

		#region Methods

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.MouseLeftButtonDown += this.OnMouseClick;

			VisualStateManager.GoToElementState(this.LayoutRoot, "Default", false);
			this.Update();
		}

		private void OnMouseClick(object sender, MouseButtonEventArgs e)
		{
			this.parentControl.ElementClicked(this);
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			this.MouseLeftButtonDown -= this.OnMouseClick;
		}

		#endregion
	}
}