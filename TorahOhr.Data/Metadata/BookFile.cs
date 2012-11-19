using System;
using System.Xml.Linq;

namespace TorahOhr.Data.Metadata {
	///<summary>Describes a single file that is part of a book.</summary>
	public class BookFile {
		public BookFile(string type, Uri url, byte[] hash, long size) {
			Type = type;
			Url = url;
			Hash = hash;
			Size = size;
		}

		///<summary>Serializes this instance to XML.</summary>
		public XElement ToXml() {
			return new XElement("BookFile",
				new XAttribute("type", Type),
				new XAttribute("size", Size),

				new XElement("Url", Url),
				new XElement("Hash", Convert.ToBase64String(Hash))
			);
		}

		///<summary>Reads a serialized XML element into a BookFile.</summary>
		public static BookFile FromXml(XElement elem) {
			if (elem == null) throw new ArgumentNullException("elem");
			if (elem.Name != "BookFile") throw new ArgumentOutOfRangeException("elem");

			return new BookFile(
				(string)elem.Attribute("type"),
				new Uri(elem.Element("Url").Value),
				Convert.FromBase64String(elem.Element("Hash").Value),
				(long)elem.Attribute("size")
			);
		}

		///<summary>Gets the kind of data contained in this file.</summary>
		public string Type { get; private set; }

		///<summary>Gets the URL to the file contents.</summary>
		///<remarks>
		/// For a BookInfo describing a book on the server, this will point to a public URL for download.
		/// For a BookInfo describing a downloaded book on the client, this will be the downloaded file.
		///</remarks>
		public Uri Url { get; private set; }

		///<summary>Gets the SHA512 hash of this file.</summary>
		public byte[] Hash { get; private set; }

		///<summary>Gets the size of the file in bytes.</summary>
		public long Size { get; private set; }
	}
}
