using System;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TUGraz.VectoCommon.Hashing;
using TUGraz.VectoCore.Utils;
using TUGraz.VectoHashing;
using VECTO_Engine;
using XmlDocumentType = TUGraz.VectoCore.Utils.XmlDocumentType;
using XMLHelper = VectoEngineTest.Util.XMLHelper;

namespace VectoEngineTest
{
	[TestClass]
	public class TestXmlWriting
	{
		private string _dirPath;
		private string _jobBaseDirPath;

		[TestInitialize]
		public void Init()
		{
			_dirPath = @"TestData/JsonInputFile";
			_jobBaseDirPath = @"TestData/DualFuelAndWHR";
		}

		[TestMethod]
		public void TestDualFuel()
		{
			var job = LoadJobFile("DemoEngineDualFuel.vep");
			SetupBackgroundWorker();

			var success = job.Run();
			Assert.IsTrue(success);

			ValidateEngineXml(job);
			CompareFileHash(job, "Dual fuel.xml", "hPls0MBANU0gSU6c0+KRZK8o0Pjnw+oZRY5d7efnB4o=");
		}

		[TestMethod]
		public void TestDualFuelWHREL()
		{
			var job = LoadJobFile("DemoEngineDualFuel_WHRel.vep");
			SetupBackgroundWorker();
			
			var success = job.Run();
			Assert.IsTrue(success);

			ValidateEngineXml(job);
			CompareFileHash(job, "Dual fuel+WHR_el.xml", "/7bo70LKMydZzSAA/BR3fwkn3GecX8kvgCYXMh1owbQ=");
		}

        [TestMethod]
        public void TestDualFuelWHRice()
        {
            var job = LoadJobFile("DemoEngineDualFuel_WHRice.vep");
            SetupBackgroundWorker();

            var success = job.Run();
            Assert.IsTrue(success);

            ValidateEngineXml(job);
            CompareFileHash(job, "Dual fuel+WHR_ice.xml", "HepwRaw7c5XyQ8LeoWprpeJLUOq7C2hAkaFZGJ6f4ck=");
        }

		[TestMethod]
		public void TestDualFuelWHRMech()
		{
			var job = LoadJobFile("DemoEngineDualFuel_WHRmech.vep");
			SetupBackgroundWorker();

			var success = job.Run();
			Assert.IsTrue(success);
			
			ValidateEngineXml(job);
			CompareFileHash(job, "Dual fuel+WHR_mech.xml", "Xr6Kpu4pUQNjr7kuQobwmvdkqhcZw6s4qs3DS242wc0=");
		}

		[TestMethod]
		public void TestSingleFuel()
		{
			var job = LoadJobFile("DemoEngineSingleFuel.vep");
			SetupBackgroundWorker();

			var success = job.Run();
			Assert.IsTrue(success);

			ValidateEngineXml(job);
			CompareFileHash(job, "Single fuel.xml", "oc5CuweTjQR5oqP9HUjoPuLJ7rHs2qj078DMp5ZP6aA=");
		}

		[TestMethod]
		public void TestSingleFuelWHRel()
		{
			var job = LoadJobFile("DemoEngineSingleFuel_WHRel.vep");
			SetupBackgroundWorker();

			var success = job.Run();
			Assert.IsTrue(success);

			ValidateEngineXml(job);
			CompareFileHash(job, "Single fuel+WHR_el.xml", "pWH69clCkszog8bofugjWn8tCWY3GSnCjW/vD3ZYR78=");
		}

        [TestMethod]
        public void TestSingleFuelWHRice()
        {
            var job = LoadJobFile("DemoEngineSingleFuel_WHRice.vep");
            SetupBackgroundWorker();

            var success = job.Run();
            Assert.IsTrue(success);

            ValidateEngineXml(job);
            CompareFileHash(job, "Single fuel+WHR_ice.xml", "dXs9hHXkSGUC0Wzg+oBfIWUhYqd7mHtYU8ub7Bepy/g=");
        }


        [TestMethod]
        public void TestSingleFuelWHMech()
        {
            var job = LoadJobFile("DemoEngineSingleFuel_WHRmech.vep");
            SetupBackgroundWorker();

            var success = job.Run();
            Assert.IsTrue(success);

            ValidateEngineXml(job);
            CompareFileHash(job, "Single fuel+WHR_mech.xml", "YxjokGvi4MQGNA9S1agJN9CEBWKpsEnoxNlGZJqnBSs=");
        }


		private void ValidateEngineXml(cJob job)
		{
			var componentFile = Path.Combine(job.OutPath, $"{job.Manufacturer}_{job.Model}.xml");
			using (var xmlReader = XmlReader.Create(componentFile))
			{
				var xmlValidator = new XMLValidator(xmlReader);
				Assert.IsTrue(xmlValidator.ValidateXML(XmlDocumentType.DeclarationComponentData));
			}
		}

		private void CompareFileHash(cJob job, string expectedXMLName, string expectedHash)
		{
			var componentFile = Path.Combine(job.OutPath, $"{job.Manufacturer}_{job.Model}.xml");
			using (var componentXmlReader = new XmlTextReader(componentFile))
			{
				using (var expectedXmlReader = new XmlTextReader(Path.Combine("TestData", "ExpectedResults", expectedXMLName)))
				{
					var xml = XDocument.Load(componentXmlReader);
					var expectedXml = XDocument.Load(expectedXmlReader);

					var idValue = expectedXml.XPathSelectElement(XMLHelper.QueryLocalName("Data")).Attribute("id").Value;
					var dateValue = expectedXml.XPathSelectElement(XMLHelper.QueryLocalName("Date")).Value;

					xml.XPathSelectElement(XMLHelper.QueryLocalName("Data")).Attribute("id").Value = idValue;
					xml.XPathSelectElement(XMLHelper.QueryLocalName("Date")).Value = dateValue;

					
					using (var stream = new MemoryStream())
					{
						using (var writer = new StreamWriter(stream))
						{
							writer.Write(xml);
							writer.Flush();
							stream.Seek(0, SeekOrigin.Begin);

							var hash = VectoHash.Load(stream);

							Assert.AreEqual(expectedHash, hash.ComputeHash(VectoComponents.Engine));
						}
					}
				}
			}
		}

		private cJob LoadJobFile(string jobFileName)
		{
			var filePath = GetFilePath(jobFileName);
			var json = new cJSON();
			var job = json.ReadJSON(filePath);
			job = AdjustInputFilePaths(job);
			job.OutPath = CreateOutputDirectory();

			return job;
		}

		private void SetupBackgroundWorker()
		{
			var worker = new BackgroundWorker { WorkerReportsProgress = true };
			worker.ProgressChanged += ProgressChangedEventHandler;
			GlobalDefinitions.Worker = worker;
		}
		
		private string GetFilePath(string fileName)
		{
			return Path.Combine(_dirPath, fileName);
		}

		private cJob AdjustInputFilePaths(cJob job)
		{
			job.MapFile =  $"{_jobBaseDirPath}/{job.MapFile}";
			job.FlcFile = $"{_jobBaseDirPath}/{job.FlcFile}";
			job.FlcParentFile = $"{_jobBaseDirPath}/{job.FlcParentFile}";
			job.DragFile = $"{_jobBaseDirPath}/{job.DragFile}";

			return job;
		}
		
		public static void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs progressChangedEventArgs)
		{
			var msg = progressChangedEventArgs.UserState as GlobalDefinitions.cWorkerMsg;
			if (msg == null)
			{
				return;
			}
			Console.WriteLine(msg.MsgType + " - " + msg.Msg);
		}

		private static string CreateOutputDirectory()
		{
			var outPath = Path.Combine("TestData", "DualFuelAndWHR", "results");
			if (!Directory.Exists(outPath))
			{
				Directory.CreateDirectory(outPath);
			}
			return outPath + Path.DirectorySeparatorChar;
		}
	}
}
