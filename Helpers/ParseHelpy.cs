using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Helpers
{
	/// <summary>
	/// Help in parsing objects betwixt Json, XML and cs objects
	/// Remember to nuget the newtonsoft json package.
	/// </summary>
	public static class ParseHelpy
	{

		public static MemoryStream StringToMemoryStream(string body)
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(body));
		}

		/// <summary>
		/// Uses the XmlSerializer to serialize to xml - 
		/// DataContractSerializer is buggy as
		/// </summary>
		public static string ParseToXml<T>(T[] coll)
		{
			var xmlSerializer = new XmlSerializer(coll.GetType());
			var memStream = new MemoryStream();
			xmlSerializer.Serialize(memStream, coll);
			memStream.Position = 0;
			var reader = new StreamReader(memStream);
			var content = reader.ReadToEnd();
			return content;
		}

		public static string ParseToXml<T>(T coll)
		{
			var xmlSerializer = new XmlSerializer(coll.GetType());
			var memStream = new MemoryStream();
			xmlSerializer.Serialize(memStream, coll);
			memStream.Position = 0;
			var reader = new StreamReader(memStream);
			var content = reader.ReadToEnd();
			return content;
		}

		public static T ParseXmlToClass<T>(Stream body)
		{
			var xmlSerializer = new XmlSerializer(typeof(T));
			var result = (T)xmlSerializer.Deserialize(body);
			return result;
		}

		public static T ParseXmlToClass<T>(string body)
		{
			return ParseXmlToClass<T>(StringToMemoryStream(body));
		}

		public static T LoadFromXmlFile<T>(string fileNameAndPath)
		{
			using (var f = File.OpenText(fileNameAndPath))
			{
				var content = f.ReadToEnd();
				var inflated = ParseXmlToClass<T>(content);
				return inflated;
			}
		}

		public static void SaveToXmlFile<T>(T obj, string fileNameAndPath)
		{
			var deflated = ParseToXml(obj);
			if (File.Exists(fileNameAndPath))
			{
				File.Delete(fileNameAndPath);
			}
			using (var s = File.CreateText(fileNameAndPath))
			{
				s.Write(deflated);
			}
		}


		public static string ObjToJson<T>(T obj)
		{
			var js = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
			return js;
		}

		public static T ObjFromJson<T>(string js)
		{
			var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(js);
			return ret;
		}

		public static T LoadFromJsonFile<T>(string fileNameAndPath)
		{
			using (var f = File.OpenText(fileNameAndPath))
			{
				var content = f.ReadToEnd();
				var inflated = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
				return inflated;
			}
		}

		public static void SaveToJsonFile<T>(T obj, string fileNameAndPath)
		{
			var deflated = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
			if (File.Exists(fileNameAndPath))
			{
				File.Delete(fileNameAndPath);
			}
			using (var s = File.CreateText(fileNameAndPath))
			{
				s.Write(deflated);
			}
		}

	}
}
