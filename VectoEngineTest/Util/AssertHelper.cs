using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectoEngineTest.Util
{
	public class AssertHelper
	{
		public static void FullLoadCurvesAreEqual(XElement flc, XElement expectedFlc)
		{
			var entries = flc.XPathSelectElements(XMLHelper.QueryLocalName("Entry")).ToArray();
			var expected = expectedFlc.XPathSelectElements(XMLHelper.QueryLocalName("Entry")).ToArray();

			Assert.AreEqual(
				expected.Length, entries.Length,
				"number of entries in full-load curve does not match expected: expected: {0}, found: {1}", expected.Length,
				entries.Length);

			var cnt = 1;
			foreach (var tuple in expected.Zip(entries, Tuple.Create)) {
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, "engineSpeed", cnt);
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, "maxTorque", cnt);
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, "dragTorque", cnt);
				cnt++;
			}
		}

		
		public static void FCMapsAreEqual(XElement fcMap, XElement expectedFcMap)
		{
			var entries = fcMap.XPathSelectElements(XMLHelper.QueryLocalName("Entry")).ToArray();
			var expected = expectedFcMap.XPathSelectElements(XMLHelper.QueryLocalName("Entry")).ToArray();

			Assert.AreEqual(
				expected.Length, entries.Length,
				"number of entries in fc-map does not match expected: expected: {0}, found: {1}", expected.Length,
				entries.Length);

			var cnt = 1;
			foreach (var tuple in expected.Zip(entries, Tuple.Create)) {
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, "engineSpeed", cnt);
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, "torque", cnt);
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, "fuelConsumption", cnt);
				cnt++;
			}
		}

		public static void FCCorrectionFactorsAreEqual(XElement correctionFactors, XElement expected)
		{
			Assert.IsNotNull(expected);
			Assert.IsNotNull(correctionFactors);
			AssertNodeValuesAreEqual(expected, correctionFactors, "WHTCUrban");
			AssertNodeValuesAreEqual(expected, correctionFactors, "WHTCRural");
			AssertNodeValuesAreEqual(expected, correctionFactors, "WHTCMotorway");
			AssertNodeValuesAreEqual(expected, correctionFactors, "BFColdHot");
			AssertNodeValuesAreEqual(expected, correctionFactors, "CFRegPer");
			AssertNodeValuesAreEqual(expected, correctionFactors, "CFNCV");
		}

		

		public static void WHREntriesAreEqual(XElement whrEntries, XElement expectedWhrEntries, string whr)
		{
			var entries = whrEntries.XPathSelectElements(XMLHelper.QueryLocalName("Entry")).ToArray();
			var expected = expectedWhrEntries.XPathSelectElements(XMLHelper.QueryLocalName("Entry")).ToArray();

			Assert.AreEqual(
				expected.Length, entries.Length,
				"number of entries in fc-map does not match expected: expected: {0}, found: {1}", expected.Length,
				entries.Length);

			var cnt = 1;
			foreach (var tuple in expected.Zip(entries, Tuple.Create)) {
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, "engineSpeed", cnt);
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, "torque", cnt);
				AssertAttribuesAreEqual(tuple.Item1, tuple.Item2, whr, cnt);
				cnt++;
			}
		}

		public static void WHRCorrectionFactorsAreEqual(XElement whrCf, XElement expectedWhrCf)
		{
			Assert.IsNotNull(whrCf);
			Assert.IsNotNull(expectedWhrCf);
			AssertNodeValuesAreEqual(whrCf, expectedWhrCf, "Urban");
			AssertNodeValuesAreEqual(whrCf, expectedWhrCf, "Rural");
			AssertNodeValuesAreEqual(whrCf, expectedWhrCf, "Motorway");
			AssertNodeValuesAreEqual(whrCf, expectedWhrCf, "BFColdHot");
			AssertNodeValuesAreEqual(whrCf, expectedWhrCf, "CFRegPer");

		}

		private static void AssertNodeValuesAreEqual(XElement expected, XElement node, string nodeName)
		{
			Assert.IsNotNull(expected.XPathSelectElement(XMLHelper.QueryLocalName(nodeName)), nodeName);
			Assert.AreEqual(
				expected.XPathSelectElement(XMLHelper.QueryLocalName(nodeName))?.Value,
				node.XPathSelectElement(XMLHelper.QueryLocalName(nodeName))?.Value, nodeName);
		}

		private static void AssertAttribuesAreEqual(XElement expected, XElement element, string attribute, int idx)
		{
			Assert.IsNotNull(expected.Attribute(attribute), "attribute {0} is null", attribute);
			Assert.AreEqual(
				expected.Attribute(attribute)?.Value, element.Attribute(attribute)?.Value, "entry {1} differs in {0}", attribute,
				idx);
		}
	}
}
