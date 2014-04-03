namespace YoutubeGrabber
{
	#region

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;

	using Common;

	#endregion

	/// <summary>
	///     Interaction logic for YoutubeGrabberMainControl.xaml
	/// </summary>
	public partial class YoutubeGrabberMainControl : UserControl
	{
		#region Fields

		public Dictionary<string, ChannelInfo> ChannelsDB = new Dictionary<string, ChannelInfo>();

		private ChannelControl currentSelectedControl;

		#endregion

		#region Constructors and Destructors

		public YoutubeGrabberMainControl()
		{
			this.InitializeComponent();

			this.Loaded += this.OnLoaded;
			this.StatusBox.SelectionChanged += this.OnStatusSelected;
			this.SentBox.SelectionChanged += this.OnSentSelected;
			this.FilterBox.SelectionChanged += this.OnFilterChanged;
			this.BtnSave.Click += this.OnSaveBtn;
		}

		private void OnSaveBtn(object sender, RoutedEventArgs e)
		{
			DBTools.SaveCurrentDB(this.ChannelsDB.Values.ToList());
		}

		private void OnFilterChanged(object sender, SelectionChangedEventArgs e)
		{
			var index = this.FilterBox.SelectedIndex;
			this.LoadWithFilter(index);
		}


		private void LoadWithFilter(int index)
		{
			List<ChannelInfo> toLoad = new List<ChannelInfo>();

			switch (index)
			{
				case 0:
					//no filtering
					toLoad = ChannelsDB.Select(el => el.Value).ToList();
					break;
				case 1:
					//Bad only
					toLoad = ChannelsDB.Select(el => el.Value).Where(el => el.bBad < 0).ToList();
					break;
				case 2:
					//Not rated
					toLoad = ChannelsDB.Select(el => el.Value).Where(el => el.bBad == 0).ToList();
					break;
				case 3:
					//Good only
					toLoad = ChannelsDB.Select(el => el.Value).Where(el => el.bBad > 0).ToList();
					break;
				case 4:
					//Good, not sent
					toLoad = ChannelsDB.Select(el => el.Value).Where(el => el.bBad > 0 && !el.bSent).ToList();
					break;
				case 5:
					//Good, sent
					toLoad = ChannelsDB.Select(el => el.Value).Where(el => el.bBad > 0 && el.bSent).ToList();
					break;
			}


			this.Channels.Children.Clear();
			foreach (var channel in toLoad.OrderByDescending(
				el =>
				{
					if (el.Subscribers.Length == 0) return 0;
					return int.Parse(el.Subscribers.Replace(",", ""));
				}))
			{
				this.Channels.Children.Add(new ChannelControl(channel, this));
			}
		}

		private void OnSentSelected(object sender, SelectionChangedEventArgs e)
		{
			if (this.currentSelectedControl != null)
			{
				this.currentSelectedControl.DataObject.bSent = this.SentBox.SelectedIndex == 1;
				this.currentSelectedControl.Update();
			}		
		}

		private void OnStatusSelected(object sender, SelectionChangedEventArgs e)
		{
			if (this.currentSelectedControl != null)
			{
				int index = this.StatusBox.SelectedIndex;

				this.currentSelectedControl.DataObject.bBad = index - 1;
				this.currentSelectedControl.Update();
			}
		}

		#endregion

		#region Public Methods and Operators

		public void ElementClicked(ChannelControl channelControl)
		{
			if (this.currentSelectedControl != null)
			{
				this.currentSelectedControl.Picked = false;
			}

			this.currentSelectedControl = channelControl;
			this.currentSelectedControl.Picked = true;

			var dataObject = channelControl.DataObject;
			this.Browser.Navigate("http://youtube.com/"+dataObject.Url);

			//init combos
			
			this.StatusBox.SelectedIndex = dataObject.bBad + 1;
			this.SentBox.SelectedIndex = dataObject.bSent ? 1 : 0;
		}

		#endregion

		#region Methods

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.ChannelsDB = DBTools.LoadCurrentDB();

			//load all controls
			this.LoadWithFilter(0);

			//init combos
			this.StatusBox.Items.Clear();
			this.StatusBox.Items.Add("Bad");
			this.StatusBox.Items.Add("None");
			this.StatusBox.Items.Add("Good");
			this.StatusBox.SelectedIndex = 1;

			this.SentBox.Items.Clear();
			this.SentBox.Items.Add("NotSent");
			this.SentBox.Items.Add("Sent");
			this.SentBox.SelectedIndex = 0;

			this.FilterBox.Items.Clear();
			this.FilterBox.Items.Add("No filtering");
			this.FilterBox.Items.Add("Bad only");
			this.FilterBox.Items.Add("Not rated");
			this.FilterBox.Items.Add("Good only");
			this.FilterBox.Items.Add("Good, not sent");
			this.FilterBox.Items.Add("Good, sent");
			this.FilterBox.SelectedIndex = 0;
		}

		#endregion
	}
}