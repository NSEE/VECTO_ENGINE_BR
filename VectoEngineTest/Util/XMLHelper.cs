using System.Linq;

namespace VectoEngineTest.Util
{
	public class XMLHelper
	{
		public static string QueryLocalName(string nodeName)
		{
			return string.Format(".//*[local-name()='{0}']", nodeName);
		}

		public static string QueryLocalName(params string[] nodePath)
		{
			return "./" + string.Join("/", nodePath.Where(x => x != null).Select(x => $"/*[local-name()='{x}']").ToArray());
		}
	}
}
