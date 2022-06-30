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
			CompareFileHash(job, "Dual fuel.xml", "4vD/x0vWYxFSIsIPc5Qee7kQA2gabnRG39TQU66kuQc=");
		}

		[TestMethod]
		public void TestDualFuelWHREL()
		{
			var job = LoadJobFile("DemoEngineDualFuel_WHRel.vep");
			SetupBackgroundWorker();
			
			var success = job.Run();
			Assert.IsTrue(success);

			ValidateEngineXml(job);
			CompareFileHash(job, "Dual fuel+WHR_el.xml", "x5LxSgJOjxlzp0OreTDHmp4GO40bPrS7x7R2wMIcC8Y=");
		}

        [TestMethod]
        public void TestDualFuelWHRice()
        {
            var job = LoadJobFile("DemoEngineDualFuel_WHRice.vep");
            SetupBackgroundWorker();

            var success = job.Run();
            Assert.IsTrue(success);

            ValidateEngineXml(job);
            CompareFileHash(job, "Dual fuel+WHR_ice.xml", "08WHN/Y3xqgXNq2mHJ8E6fKxOUuZxJwHUNy3+J9FWJc=");
        }

		[TestMethod]
		public void TestDualFuelWHRMech()
		{
			var job = LoadJobFile("DemoEngineDualFuel_WHRmech.vep");
			SetupBackgroundWorker();

			var success = job.Run();
			Assert.IsTrue(success);
			
			ValidateEngineXml(job);
			CompareFileHash(job, "Dual fuel+WHR_mech.xml", "SAZYqoSxX9yab1xa0Q3esMFd+H0bgMQod2LKha0Fx3I=");
		}

		[TestMethod]
		public void TestSingleFuel()
		{
			var job = LoadJobFile("DemoEngineSingleFuel.vep");
			SetupBackgroundWorker();

			var success = job.Run();
			Assert.IsTrue(success);

			ValidateEngineXml(job);
			CompareFileHash(job, "Single fuel.xml", "4GYctJmBruajmuSEs9H5A9OiCgWWIffi16L/NXcHD5I=");
		}

		[TestMethod]
		public void TestSingleFuelWHRel()
		{
			var job = LoadJobFile("DemoEngineSingleFuel_WHRel.vep");
			SetupBackgroundWorker();

			var success = job.Run();
			Assert.IsTrue(success);

			ValidateEngineXml(job);
			CompareFileHash(job, "Single fuel+WHR_el.xml", "qhiasu25nCtgdQthHjxic4Md68FvYdMBoEqIXl9N9Dw=");
		}

        [TestMethod]
        public void TestSingleFuelWHRice()
        {
            var job = LoadJobFile("DemoEngineSingleFuel_WHRice.vep");
            SetupBackgroundWorker();

            var success = job.Run();
            Assert.IsTrue(success);

            ValidateEngineXml(job);
            CompareFileHash(job, "Single fuel+WHR_ice.xml", "Bdl49H6waWnd1KXLbBLaZ54m/kT3u3trSfrJaFhCWV0=");
        }


        [TestMethod]
        public void TestSingleFuelWHMech()
        {
            var job = LoadJobFile("DemoEngineSingleFuel_WHRmech.vep");
            SetupBackgroundWorker();

            var success = job.Run();
            Assert.IsTrue(success);

            ValidateEngineXml(job);
            CompareFileHash(job, "Single fuel+WHR_mech.xml", "4dt2gxcuq6JLa5BbnA6RXGM+uJkYWhbtNNMnsXKpcGk=");
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
                    var appValue = expectedXml.XPathSelectElement(XMLHelper.QueryLocalName("AppVersion")).Value;

					xml.XPathSelectElement(XMLHelper.QueryLocalName("Data")).Attribute("id").Value = idValue;
					xml.XPathSelectElement(XMLHelper.QueryLocalName("Date")).Value = dateValue;
                    xml.XPathSelectElement(XMLHelper.QueryLocalName("AppVersion")).Value = appValue; 
					
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
