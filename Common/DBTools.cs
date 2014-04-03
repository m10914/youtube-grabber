namespace Common
{
	#region

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml.Linq;

	#endregion

	public class ChannelInfo
	{
		#region Fields

		public string Name;

		public string Subscribers;

		public string Url; //primary key

		public int bBad;

		public bool bSent;

		public bool bViewed;

		#endregion

		#region Constructors and Destructors

		public ChannelInfo()
		{
			this.bBad = 0;
			this.bSent = false;
			this.bViewed = false;
		}

		public ChannelInfo(XElement elt)
		{
			this.Name = elt.Element("name").Value;
			this.Url = elt.Element("url").Value;
			this.Subscribers = elt.Element("subscribers").Value;
			this.bBad = int.Parse(elt.Element("bad").Value);
			this.bSent = elt.Element("sent").Value == "0";
			this.bViewed = elt.Element("viewed").Value == "0";
		}

		#endregion

		#region Public Methods and Operators

		public XElement GetXElement()
		{
			XElement res = new XElement("channel");
			res.Add(new XElement("name", this.Name));
			res.Add(new XElement("url", this.Url));
			res.Add(new XElement("subscribers", this.Subscribers));
			res.Add(new XElement("bad", this.bBad.ToString()));
			res.Add(new XElement("sent", this.bSent.ToString()));
			res.Add(new XElement("viewed", this.bViewed.ToString()));
			return res;
		}

		#endregion
	}

	public class ChannelInfoSmall
	{
		#region Fields

		public string Name;

		public string Subscribers;

		public string Url;

		#endregion
	}

	public class DBTools
	{
		#region Constants

		private const string databasePath = "database.txt";

		#endregion

		#region Public Methods and Operators

		public static Dictionary<string, ChannelInfo> LoadCurrentDB()
		{
			Dictionary<string, ChannelInfo> db = new Dictionary<string, ChannelInfo>();

			if (File.Exists(databasePath))
			{
				Console.WriteLine("Database exists. Loading...");
				XElement elt = XElement.Parse(File.ReadAllText(databasePath));
				foreach (XElement nd in elt.Element("nodes").Elements())
				{
					ChannelInfo ci = new ChannelInfo(nd);
					db.Add(ci.Url, ci);
				}

				Console.WriteLine("Loaded {0} entities", db.Count);
			}
			else
			{
				Console.WriteLine("No existing database found.");
			}

			return db;
		}

		public static void SaveCurrentDB(List<ChannelInfo> toSave)
		{
			XElement elt = new XElement("root");
			XElement nodes = new XElement("nodes");
			elt.Add(nodes);

			foreach (ChannelInfo nd in toSave)
			{
				nodes.Add(nd.GetXElement());
			}

			File.WriteAllText(databasePath, elt.ToString());
		}

		#endregion
	}
}