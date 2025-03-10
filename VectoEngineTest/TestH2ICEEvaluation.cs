using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectoEngineTest.Util;
using VECTO_Engine;

namespace VectoEngineTest
{
	[TestClass]
	public class TestH2ICEEvaluation
	{
		private const string basepath = @"TestData";

		[TestMethod]
		public void TestH2DualFuel()
		{
			var Job = new cJob();

			var jobDir = "H2DualFuelAndWHR";

			Job.Manufacturer = "TUG";
			Job.Model = "Testengine";
			Job.CertNumber = "Engine0815";
			Job.Idle_Parent = 600;
			Job.Idle = 600;
			Job.Displacement = 7700;
			Job.RatedPower = 130;
			Job.RatedSpeed = 2200;


			Job.MapFile = Path.Combine(basepath, jobDir, "Demo_Map_v1.5_DF.csv");
			Job.FlcFile = Path.Combine(basepath, jobDir, "Demo_FullLoad_Child_v1.5.csv");
			Job.FlcParentFile = Path.Combine(basepath, jobDir, "Demo_FullLoad_Parent_v1.5.csv");
			Job.DragFile = Path.Combine(basepath, jobDir, "Demo_Motoring_v1.5.csv");

			Job.FCspecMeas_HotUrb = 211.1;
			Job.FCspecMeas_HotRur = 201.7;
			Job.FCspecMeas_HotMw = 196.3;
			Job.FCspecMeas_ColdTot = 205.6;
			Job.FCspecMeas_HotTot = 198.2;
			Job.CF_RegPer = 1.0;
			Job.FuelType = "H2 / CI";
			Job.NCVfuel = 120;

			Job.DualFuel = true;
			Job.FCspecMeas_HotUrb2 = 25.6;
			Job.FCspecMeas_HotRur2 = 19.2;
			Job.FCspecMeas_HotMw2 = 15.1;
			Job.FCspecMeas_ColdTot2 = 23.0;
			Job.FCspecMeas_HotTot2 = 18.0;
			Job.CF_RegPer2 = 1.0;
			Job.FuelType2 = "Diesel / CI";
			Job.NCVfuel2 = 42.5;

			Job.OutPath = CreateOutputDirectory(jobDir);


			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.ProgressChanged += ProgressChangedEventHandler;
			GlobalDefinitions.Worker = worker;

			var success = Job.Run();

			Assert.IsTrue(success);

			var componentFile = Path.Combine(Job.OutPath, $"{Job.Manufacturer}_{Job.Model}.xml");
			using (var componentXmlReader = new XmlTextReader(componentFile))
			{
				using (var expectedXmlReader = new XmlTextReader(Path.Combine(basepath, @"ExpectedResults\H2 Dual fuel.xml")))
				{
					var xml = XDocument.Load(componentXmlReader);
					var expectedXml = XDocument.Load(expectedXmlReader);

					AssertHelper.FullLoadCurvesAreEqual(
						xml.XPathSelectElement(XMLHelper.QueryLocalName("FullLoadAndDragCurve")),
						expectedXml.XPathSelectElement(XMLHelper.QueryLocalName("FullLoadAndDragCurve")));
					AssertHelper.FCMapsAreEqual(
						xml.XPathSelectElements(XMLHelper.QueryLocalName("FuelConsumptionMap")).ToArray()[0],
						expectedXml.XPathSelectElements(XMLHelper.QueryLocalName("FuelConsumptionMap")).ToArray()[0]);
					AssertHelper.FCMapsAreEqual(
						xml.XPathSelectElements(XMLHelper.QueryLocalName("FuelConsumptionMap")).ToArray()[1],
						expectedXml.XPathSelectElements(XMLHelper.QueryLocalName("FuelConsumptionMap")).ToArray()[1]);

					AssertHelper.FCCorrectionFactorsAreEqual(
						xml.XPathSelectElements(XMLHelper.QueryLocalName("Mode", "Fuel")).ToArray()[0],
						expectedXml.XPathSelectElements(XMLHelper.QueryLocalName("Mode", "Fuel")).ToArray()[0]);
					AssertHelper.FCCorrectionFactorsAreEqual(
						xml.XPathSelectElements(XMLHelper.QueryLocalName("Mode", "Fuel")).ToArray()[1],
						expectedXml.XPathSelectElements(XMLHelper.QueryLocalName("Mode", "Fuel")).ToArray()[1]);
				}
			}
		}

		private static string CreateOutputDirectory(string jobDir)
		{
			var outPath = Path.Combine(basepath, jobDir, "results");
			if (!Directory.Exists(outPath)) {
				//Directory.Delete(outPath, true);
				//}
				Directory.CreateDirectory(outPath);
			}
			return outPath + Path.DirectorySeparatorChar;
		}

		public static void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs progressChangedEventArgs)
		{
			var msg = progressChangedEventArgs.UserState as GlobalDefinitions.cWorkerMsg;
			if (msg == null) {
				return;
			}
			Console.WriteLine(msg.MsgType + " - " + msg.Msg);
		}
	}
}
