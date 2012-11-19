using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace TorahOhr.Data.Metadata {
	public class BookFolder {
		public BookFolder(string name, IEnumerable<BookInfo> books, IEnumerable<BookFolder> subfolders) {
			Name = name;
			Books = new ReadOnlyCollection<BookInfo>(books.ToList());
			Subfolders = new ReadOnlyCollection<BookFolder>(subfolders.ToList());
		}

		///<summary>Serializes this instance to XML.</summary>
		public XElement ToXml() {
			return new XElement("Folder",
				new XAttribute("name", Name),

				new XElement("Books",
					Books.Select(f => f.ToXml())
				),
				new XElement("Subfolders",
					Subfolders.Select(f => f.ToXml())
				)
			);
		}

		///<summary>Reads a serialized XML element into a BookInfo instance.</summary>
		public static BookFolder FromXml(XElement elem) {
			if (elem == null) throw new ArgumentNullException("elem");
			if (elem.Name != "Folder") throw new ArgumentOutOfRangeException("elem");

			return new BookFolder(
				(string)elem.Attribute("name"),
				elem.Element("Books").Elements().Select(BookInfo.FromXml),
				elem.Element("Subfolders").Elements().Select(BookFolder.FromXml)
			);
		}

		///<summary>Gets the display name of the folder.</summary>
		public string Name { get; private set; }

		///<summary>Gets the books directly within this folder, if any.</summary>
		public ReadOnlyCollection<BookInfo> Books { get; private set; }

		///<summary>Gets the subfolders directly within this folder, if any.</summary>
		public ReadOnlyCollection<BookFolder> Subfolders { get; private set; }
	}
}
