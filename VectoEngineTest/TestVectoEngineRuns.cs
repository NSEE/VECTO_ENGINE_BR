using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectoEngineTest.Util;
using VECTO_Engine;

namespace VectoEngineTest
{
	[TestClass]
	public class TestVectoEngineRuns
	{
		private const string basepath =
			@"TestData";

		[TestInitialize]
		public void SetCulture()
		{
			try {
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			}
			catch (Exception ex) {
				Assert.Fail();
			}
		}

		[TestMethod]
		public void TestValidInput()
		{
			var Job = new cJob();

			var jobDir = "Valid_1.4";

			Job.Manufacturer = "TUG";
			Job.Model = "Testengine";
			Job.CertNumber = "Engine0815";
			Job.Idle_Parent = 600;
			Job.Idle = 600;
			Job.Displacement = 7700;
			Job.RatedPower = 130;
			Job.RatedSpeed = 2200;
			Job.FuelType = "Ethanol / CI";
			Job.NCVfuel = 42.3;

			Job.MapFile = Path.Combine(basepath, jobDir, "Map.csv");
			Job.FlcFile = Path.Combine(basepath, jobDir, "FullLoad_Child.csv");
			Job.FlcParentFile = Path.Combine(basepath, jobDir, "FullLoad_Parent.csv");
			Job.DragFile = Path.Combine(basepath, jobDir, "Motoring.csv");

			Job.FCspecMeas_ColdTot = 200;
			Job.FCspecMeas_HotTot = 200;
			Job.FCspecMeas_HotUrb = 200;
			Job.FCspecMeas_HotRur = 200;
			Job.FCspecMeas_HotMw = 200;
			Job.CF_RegPer = 1;

			Job.OutPath = CreateOutputDirectory(jobDir);


			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.ProgressChanged += ProgressChangedEventHandler;
			GlobalDefinitions.Worker = worker;

			var success = Job.Run();

			Assert.IsTrue(success);

			var componentFile = Path.Combine(Job.OutPath, string.Format("{0}_{1}.xml", Job.Manufacturer, Job.Model));
			var xml = XDocument.Load(new XmlTextReader(componentFile));

			Assert.AreEqual(1.0, GetDoubleValue(xml, "WHTCUrban"), 1e-4);
			Assert.AreEqual(1.0741, GetDoubleValue(xml, "WHTCRural"), 1e-4);
			Assert.AreEqual(1.1237, GetDoubleValue(xml, "WHTCMotorway"), 1e-4);
			Assert.AreEqual(1.0, GetDoubleValue(xml, "BFColdHot"), 1e-4);
			Assert.AreEqual(1.0, GetDoubleValue(xml, "CFRegPer"), 1e-4);
			Assert.AreEqual(1.6459, GetDoubleValue(xml, "CFNCV"), 1e-4);
		}

		[TestMethod]
		public void TestNegativeTorqueFullLoad()
		{
			var Job = new cJob();

			var jobDir = "NegativeTorqueFullLoad_1.4";

			Job.Manufacturer = "TUG";
			Job.Model = "Testengine";
			Job.CertNumber = "Engine0815";
			Job.Idle_Parent = 600;
			Job.Idle = 600;
			Job.Displacement = 7700;
			Job.RatedPower = 130;
			Job.RatedSpeed = 2200;
			Job.FuelType = "Ethanol / CI";
			Job.NCVfuel = 42.3;

			Job.MapFile = Path.Combine(basepath, jobDir, "Map.csv");
			Job.FlcFile = Path.Combine(basepath, jobDir, "FullLoad_Child.csv");
			Job.FlcParentFile = Path.Combine(basepath, jobDir, "FullLoad_Parent.csv");
			Job.DragFile = Path.Combine(basepath, jobDir, "Motoring.csv");

			Job.FCspecMeas_ColdTot = 200;
			Job.FCspecMeas_HotTot = 200;
			Job.FCspecMeas_HotUrb = 200;
			Job.FCspecMeas_HotRur = 200;
			Job.FCspecMeas_HotMw = 200;
			Job.CF_RegPer = 1;

			Job.OutPath = CreateOutputDirectory(jobDir);


			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.ProgressChanged += ProgressChangedEventHandler;
			GlobalDefinitions.Worker = worker;

			var success = Job.Run();

			Assert.IsFalse(success);

			var componentFile = Path.Combine(Job.OutPath, string.Format("{0}_{1}.xml", Job.Manufacturer, Job.Model));

			Assert.IsFalse(File.Exists(componentFile));
		}

		[TestMethod]
		public void TestSingleFuel()
		{
			var Job = new cJob();

			var jobDir = "DualFuelAndWHR";

			Job.Manufacturer = "TUG";
			Job.Model = "Testengine";
			Job.CertNumber = "Engine0815";
			Job.Idle_Parent = 600;
			Job.Idle = 600;
			Job.Displacement = 7700;
			Job.RatedPower = 130;
			Job.RatedSpeed = 2200;

			Job.MapFile = Path.Combine(basepath, jobDir, "Demo_Map_v1.5.csv");
			Job.FlcFile = Path.Combine(basepath, jobDir, "Demo_FullLoad_Child_v1.5.csv");
			Job.FlcParentFile = Path.Combine(basepath, jobDir, "Demo_FullLoad_Parent_v1.5.csv");
			Job.DragFile = Path.Combine(basepath, jobDir, "Demo_Motoring_v1.5.csv");

			Job.FCspecMeas_HotUrb = 211.1;
			Job.FCspecMeas_HotRur = 201.7;
			Job.FCspecMeas_HotMw = 196.3;
			Job.FCspecMeas_ColdTot = 205.6;
			Job.FCspecMeas_HotTot = 198.2;
			Job.CF_RegPer = 1.0;
			Job.FuelType = "Diesel / CI";
			Job.NCVfuel = 42.5;

			Job.OutPath = CreateOutputDirectory(jobDir);


			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.ProgressChanged += ProgressChangedEventHandler;
			GlobalDefinitions.Worker = worker;

			var success = Job.Run();

			Assert.IsTrue(success);

			var componentFile = Path.Combine(Job.OutPath, string.Format("{0}_{1}.xml", Job.Manufacturer, Job.Model));
			var xml = XDocument.Load(new XmlTextReader(componentFile));

			var expectedXml =
				XDocument.Load(new XmlTextReader(Path.Combine(basepath, @"ExpectedResults\Single fuel.xml")));

			AssertHelper.FullLoadCurvesAreEqual(
				xml.XPathSelectElement(XMLHelper.QueryLocalName("FullLoadAndDragCurve")),
				expectedXml.XPathSelectElement(XMLHelper.QueryLocalName("FullLoadAndDragCurve")));
			AssertHelper.FCMapsAreEqual(
				xml.XPathSelectElement(XMLHelper.QueryLocalName("FuelConsumptionMap")),
				expectedXml.XPathSelectElement(XMLHelper.QueryLocalName("FuelConsumptionMap")));
			//AssertHelper.FCCorrectionFactorsAreEqual(
			//	xml.XPathSelectElement(XMLHelper.QueryLocalName("Mode", "Fuel")),
			//	expectedXml.XPathSelectElement(XMLHelper.QueryLocalName("Mode", "Fuel")));
		}


		private double GetDoubleValue(XDocument xml, string elementName)
		{
			var xpath = string.Format("//*[local-name()='{0}']", elementName);
			var node = xml.XPathSelectElement(xpath);
			return XmlConvert.ToDouble(node.Value);
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