using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace TorahOhr.Data.Metadata {
	///<summary>Holds basic metadata about a ספר that may or may not have been downloaded.</summary>
	public class BookInfo {
		public BookInfo(string name, string author, string icon, IEnumerable<BookFile> files) {
			Name = name;
			Author = author;
			IconName = icon;

			Files = new BookFileDictionary(files);
		}

		///<summary>Serializes this instance to XML.</summary>
		public XElement ToXml() {
			return new XElement("Book",
				new XAttribute("name", Name),
				new XAttribute("author", Author),

				new XElement("Icon", IconName),
				new XElement("Files",
					Files.Values.Select(f => f.ToXml())
				)
			);
		}

		///<summary>Reads a serialized XML element into a BookInfo instance.</summary>
		public static BookInfo FromXml(XElement elem) {
			if (elem == null) throw new ArgumentNullException("elem");
			if (elem.Name != "Book") throw new ArgumentOutOfRangeException("elem");

			return new BookInfo(
				(string)elem.Attribute("type"),
				(string)elem.Attribute("author"),
				(string)elem.Element("Icon"),
				elem.Element("Files").Elements().Select(BookFile.FromXml)
			);
		}

		///<summary>Gets the name of the ספר.</summary>
		public string Name { get; private set; }

		///<summary>Gets the name of the person who wrote the ספר.</summary>
		public string Author { get; private set; }

		///<summary>Gets the size of the ספר's encoded file in bytes.</summary>
		public long Size { get { return Files.Values.Sum(f => f.Size); } }


		///<summary>Gets the files containing the book's text and full metadata.</summary>
		public BookFileDictionary Files { get; private set; }

		///<summary>Gets the name of the icon to use for this ספר.</summary>
		public string IconName { get; private set; }
	}

	///<summary>A collection of files in a book, organized by type.</summary>
	public class BookFileDictionary : ReadOnlyDictionary<string, BookFile> {
		public BookFileDictionary(IEnumerable<BookFile> files) : base(files.ToDictionary(f => f.Type)) { }

		///<summary>Gets the file of the specified strongly-typed type.</summary>
		///<remarks>
		/// This overload is preferred, since it avoids hard-coded magic strings.
		/// However, some books may need custom or indexed file types that do not
		/// exist in the enum.
		///</remarks>
		public BookFile this[FileType type] {
			get { return base[type.ToString()]; }
		}
	}

	///<summary>Describes the different kinds of files in a book.</summary>
	public enum FileType {
		Index,
		Text
	}
}
