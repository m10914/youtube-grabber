namespace DatabaseCreator
{
	#region

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Text.RegularExpressions;

	using Common;

	#endregion

	internal class Program
	{
		#region Static Fields

		public static Dictionary<string, ChannelInfo> ChannelsDB = new Dictionary<string, ChannelInfo>();

		private static string requestUrl;

		#endregion

		#region Methods

		private static void ExecuteNewCommand(string[] arguments)
		{
			ChannelsDB.Clear();
			LoadPages(int.Parse(arguments[1]));
			DBTools.SaveCurrentDB(ChannelsDB.Values.ToList());
			Console.WriteLine("New database of " + ChannelsDB.Count + " entities was successfully created!");
		}

		private static void ExecuteRefreshCommand(string[] arguments)
		{
			int original = ChannelsDB.Count;
			LoadPages(int.Parse(arguments[1]));
			DBTools.SaveCurrentDB(ChannelsDB.Values.ToList());
			Console.WriteLine(
				"Database was sucessfully updated! " + original + " entities were updated, " + (ChannelsDB.Count - original)
				+ " were created.");
		}

		private static bool LoadPage(int i)
		{
			Console.Write("[{0}]", i);
			int tries = 0;

			while (true)
			{
				string urlAddress = requestUrl.Replace("$1", i.ToString());
					//"http://www.youtube.com/results?search_query=let%27s+play&filters=channel&page=" + i;
				string data = ReadAllTextUrl(urlAddress);
				if (data == null)
				{
					tries++;
					if (tries > 15)
					{
						Console.WriteLine("Critical Error: cannot load page " + urlAddress);
						return false;
					}

					continue;
				}

				List<ChannelInfoSmall> reslist = new List<ChannelInfoSmall>();
				int startIndex = ChannelsDB.Count;

				//get basic info on channels
				MatchCollection rescoll;

				//get name and link
				rescoll = Regex.Matches(
					data,
					"<a[^>]*class=\"[^\"]*yt-uix-tile-link[^\"]*\"[^>]*title=\"([^\n]*)\"[^>]*href=\"([^\"]*)\"[^>]*>([^<]*)</a>",
					RegexOptions.Multiline);

				foreach (Match res in rescoll)
				{
					if (res.Success)
					{
						reslist.Add(new ChannelInfoSmall() { Name = res.Groups[1].Value, Url = res.Groups[2].Value });
					}
				}

				//get number of subscribers
				rescoll = Regex.Matches(
					data,
					"<span[^>]*class=\"[^\"]*yt-subscription-button-subscriber-count-unbranded-horizontal[^\"]*\"[^>]*>([^<]*)</span>",
					RegexOptions.Multiline);

				int iter = 0;
				foreach (Match res in rescoll)
				{
					if (res.Success)
					{
						reslist[iter].Subscribers = res.Groups[1].Value;
						iter++;
					}
				}

				//now try to merge and actualize data
				foreach (ChannelInfoSmall res in reslist)
				{
					if (ChannelsDB.ContainsKey(res.Url))
					{
						ChannelInfo current = ChannelsDB[res.Url];
						current.Name = res.Name;
						current.Subscribers = res.Subscribers;
					}
					else
					{
						ChannelsDB.Add(res.Url, new ChannelInfo() { Name = res.Name, Url = res.Url, Subscribers = res.Subscribers });
					}
				}

				return true;
			}
		}

		private static void LoadPages(int numPages)
		{
			for (int i = 1; i <= numPages; i++)
			{
				try
				{
					if (!LoadPage(i))
					{
						break;
					}
				}
				catch (Exception)
				{
					Console.WriteLine("Exception occured while parsing page number " + i);
				}
			}
			Console.WriteLine("");
		}

		/// <summary>
		///     Entry point
		/// </summary>
		/// <param name="args"></param>
		private static void Main(string[] args)
		{
			if (File.Exists("graburl.txt"))
			{
				var lines = File.ReadAllLines("graburl.txt");
				if (lines.Count() < 1)
				{
					Console.WriteLine("File graburl.txt is corrupted.");
					Console.ReadKey();
					return;
				}

				requestUrl = lines[0];
			}
			else
			{
				Console.WriteLine("Cannot find graburl.txt, please create one.");
				Console.ReadKey();
				return;
			}


			// look for existing database
			ChannelsDB = DBTools.LoadCurrentDB();

			//endless cycle
			bool bRunning = true;
			while (bRunning)
			{
				Console.WriteLine("Enter command: ");
				string cmd = Console.ReadLine();
				string[] arguments = cmd.Split(' ');

				switch (arguments[0])
				{
					case "quit":
						bRunning = false;
						break;

					case "new":
						try
						{
							ExecuteNewCommand(arguments);
						}
						catch (Exception)
						{
							Console.WriteLine("usage: new [num_of_pages]");
						}
						break;

					case "refresh":
						try
						{
							ExecuteRefreshCommand(arguments);
						}
						catch (Exception x)
						{
							Console.WriteLine("usage: refresh [num_of_pages]");
						}

						break;

					case "help":
						Console.WriteLine("new [num_pages] - to create new database");
						Console.WriteLine("refresh [num_pages] - to refresh current database");
						Console.WriteLine("quit [num_pages] - to quit");
						break;
				}
			}
		}

		private static string ReadAllTextUrl(string urlAddress)
		{
			string res = null;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				Stream receiveStream = response.GetResponseStream();
				StreamReader readStream = null;
				if (response.CharacterSet == null)
				{
					readStream = new StreamReader(receiveStream);
				}
				else
				{
					readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
				}
				res = readStream.ReadToEnd();
				response.Close();
				readStream.Close();
			}
			return res;
		}

		#endregion
	}
}