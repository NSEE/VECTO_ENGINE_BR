using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VECTO_Engine;

namespace VectoEngineTest
{
	[TestClass]
	public class TestJsonReader
	{
		private string _dirPath;

		[TestInitialize]
		public void Init()
		{
			_dirPath = @"TestData/JsonInputFile";
		}

		[TestMethod]
		public void TestJsonReaderDualFuel()
		{
			var filePath = GetFilePath("DemoEngineDualFuel.vep");
			var json = new cJSON();
			var job = json.ReadJSON(filePath);

			TestEngineComponentData(job, "IVT at TU Graz", "Best engine ever - Dual fuel", "*1234*",
				3000, 130, 2200);

			TestEngineDataWHRSelection(job, true, false, false, false);
			TestEngineDataFiles(job, "Demo_Map_v1.5_DF.csv", "Demo_FullLoad_Parent_v1.5.csv",
				"Demo_FullLoad_Child_v1.5.csv", "Demo_Motoring_v1.5.csv");
			TestEngineFuelData01(job, "Natural Gas / CI", 48.7, 205.6, 198.2, 211.1, 201.7, 196.3, 1.0);
			TestEngineFuelData02(job, "Diesel / CI", 42.5, 23.0, 18.0, 25.6, 19.2, 15.1, 1.0);
		}


		[TestMethod]
		public void TestJsonReaderDualFuelWHRel()
		{
			var filePath = GetFilePath("DemoEngineDualFuel_WHRel.vep");
			var json = new cJSON();
			var job = json.ReadJSON(filePath);

			TestEngineComponentData(job, "IVT at TU Graz", "Best engine ever - Dual fuel+WHR_el", "*1234*",
				3000, 130, 2200);

			TestEngineDataFiles(job, "Demo_Map_v1.5_DF_WHRel.csv", "Demo_FullLoad_Parent_v1.5.csv",
				"Demo_FullLoad_Child_v1.5.csv", "Demo_Motoring_v1.5.csv");

			TestWHRElectrical(job, 23.0, 61.5, 49.2, 55.4, 75.3, 1.0);
			TestEngineFuelData01(job, "Natural Gas / CI", 48.7, 205.6, 198.2, 211.1, 201.7, 196.3, 1.0);
			TestEngineFuelData02(job, "Diesel / CI", 42.5, 23.0, 18.0, 25.6, 19.2, 15.1, 1.0);
		}

		[TestMethod]
        public void TestJsonReaderDualFuelWHRice()
        {
            var filePath = GetFilePath("DemoEngineDualFuel_WHRice.vep");
            var json = new cJSON();
            var job = json.ReadJSON(filePath);

            TestEngineComponentData(job, "IVT at TU Graz", "Best engine ever - Dual fuel+WHR_ice", "*1234*",
                3000, 130, 2200);

            TestEngineDataWHRSelection(job, true, true, false, false);
            TestEngineDataFiles(job, "Demo_Map_v1.5_DF.csv", "Demo_FullLoad_Parent_v1.5.csv",
                "Demo_FullLoad_Child_v1.5.csv", "Demo_Motoring_v1.5.csv");
            TestEngineFuelData01(job, "Natural Gas / CI", 48.7, 205.6, 198.2, 211.1, 201.7, 196.3, 1.0);
            TestEngineFuelData02(job, "Diesel / CI", 42.5, 23.0, 18.0, 25.6, 19.2, 15.1, 1.0);
		}

		[TestMethod]
		public void TestJsonReaderDualFuelWHRMech()
		{
			var filePath = GetFilePath("DemoEngineDualFuel_WHRmech.vep");
			var json = new cJSON();
			var job = json.ReadJSON(filePath);


			TestEngineComponentData(job, "IVT at TU Graz", "Best engine ever - Dual fuel+WHR_mech", "*1234*",
				3000, 130, 2200);

			TestEngineDataFiles(job, "Demo_Map_v1.5_DF_WHRmech.csv", "Demo_FullLoad_Parent_v1.5.csv",
				"Demo_FullLoad_Child_v1.5.csv", "Demo_Motoring_v1.5.csv");

			TestWHRMechanical(job, 23.0, 61.5, 49.2, 55.4, 75.3, 1.0);
			TestEngineFuelData01(job, "Natural Gas / CI", 48.7, 205.6, 198.2, 211.1, 201.7, 196.3, 1.0);
			TestEngineFuelData02(job, "Diesel / CI", 42.5, 23.0, 18.0, 25.6, 19.2, 15.1, 1.0);
		}


		[TestMethod]
		public void TestJsonReaderSingleFuel()
		{
			var filePath = GetFilePath("DemoEngineSingleFuel.vep");
			var json = new cJSON();
			var job = json.ReadJSON(filePath);
			
			TestEngineComponentData(job, "IVT at TU Graz", "Best engine ever - Single fuel", "*1234*",
				3000, 130, 2200);
			TestEngineDataFiles(job, "Demo_Map_v1.5.csv", "Demo_FullLoad_Parent_v1.5.csv",
				"Demo_FullLoad_Child_v1.5.csv", "Demo_Motoring_v1.5.csv");
			TestEngineFuelData01(job, "Diesel / CI", 42.5,205.6, 198.2, 211.1, 201.7, 196.3, 1.0);
		}


		[TestMethod]
		public void TestJsonReaderSingleFuelWHRel()
		{
			var filePath = GetFilePath("DemoEngineSingleFuel_WHRel.vep");
			var json = new cJSON();
			var job = json.ReadJSON(filePath);

			TestEngineComponentData(job, "IVT at TU Graz", "Best engine ever - Single fuel+WHR_el", "*1234*",
				3000, 130, 2200);
			TestEngineDataFiles(job, "Demo_Map_v1.5_DF_WHRel.csv", "Demo_FullLoad_Parent_v1.5.csv",
				"Demo_FullLoad_Child_v1.5.csv", "Demo_Motoring_v1.5.csv");


			TestWHRElectrical(job, 23.0, 61.5, 49.2, 55.4, 75.3, 1.0);
			TestEngineFuelData01(job, "Diesel / CI", 42.5, 205.6, 198.2, 211.1, 201.7, 196.3, 1.0);
		}

        [TestMethod]
        public void TestJsonReaderSingleFuelWHRice()
        {
            var filePath = GetFilePath("DemoEngineSingleFuel_WHRice.vep");
            var json = new cJSON();
            var job = json.ReadJSON(filePath);

            TestEngineDataWHRSelection(job, false, true, false, false);
            TestEngineComponentData(job, "IVT at TU Graz", "Best engine ever - Single fuel+WHR_ice", "*1234*",
                3000, 130, 2200);
            TestEngineDataFiles(job, "Demo_Map_v1.5.csv", "Demo_FullLoad_Parent_v1.5.csv",
                "Demo_FullLoad_Child_v1.5.csv", "Demo_Motoring_v1.5.csv");

			TestEngineFuelData01(job, "Diesel / CI", 42.5, 205.6, 198.2, 211.1, 201.7, 196.3, 1.0);
		}

        [TestMethod]
        public void TestJsonReaderSingleFuelWHRmech()
        {
            var filePath = GetFilePath("DemoEngineSingleFuel_WHRmech.vep");
            var json = new cJSON();
            var job = json.ReadJSON(filePath);

            TestEngineDataWHRSelection(job, false, false, true, false);
            TestEngineComponentData(job, "IVT at TU Graz", "Best engine ever - Single fuel+WHR_mech", "*1234*",
                3000, 130, 2200);
            TestEngineDataFiles(job, "Demo_Map_v1.5_DF_WHRmech.csv", "Demo_FullLoad_Parent_v1.5.csv",
                "Demo_FullLoad_Child_v1.5.csv", "Demo_Motoring_v1.5.csv");

            TestWHRMechanical(job, 23.0, 61.5, 49.2, 55.4, 75.3, 1.0);
            TestEngineFuelData01(job, "Natural Gas / CI", 48.7, 205.6, 198.2, 211.1, 201.7, 196.3, 1.0);
		}

        private void TestEngineComponentData(cJob job, string manufacturer, string model, string certNumber, 
			double displacement, Int16 ratedPower, int ratedSpeed)
		{
			Assert.AreEqual(manufacturer, job.Manufacturer);
			Assert.AreEqual(model, job.Model);
			Assert.AreEqual(certNumber, job.CertNumber);
			Assert.AreEqual(displacement, job.Displacement);
			Assert.AreEqual(ratedPower, job.RatedPower);
			Assert.AreEqual(ratedSpeed, job.RatedSpeed);
		}

		private void TestEngineDataWHRSelection(cJob job, bool dualFuel, bool whrICE, bool whrMech,
			bool whrEL)
		{
			Assert.AreEqual(dualFuel, job.DualFuel);
			Assert.AreEqual(whrMech, job.WHR_Mech);
			Assert.AreEqual(whrICE, job.WHR_ICE);
			Assert.AreEqual(whrEL, job.WHR_El);
		}

		private void TestEngineDataFiles(cJob job, string fcParent, string flParent, string flCurve,
			string motoringParent)
		{
			Assert.AreEqual(fcParent, job.MapFile);
			Assert.AreEqual(flParent, job.FlcParentFile);
			Assert.AreEqual(flCurve, job.FlcFile);
			Assert.AreEqual(motoringParent, job.DragFile);
		}
		
		private void TestEngineFuelData01(cJob job, string type, double ncv, double whtcCold, double whtcHot,
			double whtcUrban, double whtcRural, double whtcMotorway, double cfRegPer)
		{
			Assert.AreEqual(type, job.FuelType);
			Assert.AreEqual(ncv, job.NCVfuel);
			Assert.AreEqual(whtcCold, job.FCspecMeas_ColdTot);
			Assert.AreEqual(whtcHot, job.FCspecMeas_HotTot);
			Assert.AreEqual(whtcUrban, job.FCspecMeas_HotUrb);
			Assert.AreEqual(whtcRural, job.FCspecMeas_HotRur);
			Assert.AreEqual(whtcMotorway, job.FCspecMeas_HotMw);
			Assert.AreEqual(cfRegPer, job.CF_RegPer);
		}

		private void TestEngineFuelData02(cJob job, string type, double ncv, double whtcCold, double whtcHot,
			double whtcUrban, double whtcRural, double whtcMotorway, double cfRegPer)
		{
			Assert.AreEqual(type, job.FuelType2);
			Assert.AreEqual(ncv, job.NCVfuel2);
			Assert.AreEqual(whtcCold, job.FCspecMeas_ColdTot2);
			Assert.AreEqual(whtcHot, job.FCspecMeas_HotTot2);
			Assert.AreEqual(whtcUrban, job.FCspecMeas_HotUrb2);
			Assert.AreEqual(whtcRural, job.FCspecMeas_HotRur2);
			Assert.AreEqual(whtcMotorway, job.FCspecMeas_HotMw2);
			Assert.AreEqual(cfRegPer, job.CF_RegPer2); 
		}

		private void TestWHRElectrical(cJob job, double whtcCold, double whtcHot, double whtcUrban,
			double whtcRural, double whtcMotorway, double cfRegPer)
		{
			Assert.AreEqual(whtcCold, job.FCspecMeas_ColdTotWHREl);
			Assert.AreEqual(whtcHot, job.FCspecMeas_HotTotWHREl);
			Assert.AreEqual(whtcUrban, job.FCspecMeas_HotUrbWHREl);
			Assert.AreEqual(whtcRural, job.FCspecMeas_HotRurWHREl);
			Assert.AreEqual(whtcMotorway, job.FCspecMeas_HotMwWHREl);
			Assert.AreEqual(cfRegPer, job.CF_RegPerWHREl);
		}

		private void TestWHRMechanical(cJob job, double whtcCold, double whtcHot, double whtcUrban,
			double whtcRural, double whtcMotorway, double cfRegPer)
		{
			Assert.AreEqual(whtcCold, job.FCspecMeas_ColdTotWHRMech);
			Assert.AreEqual(whtcHot, job.FCspecMeas_HotTotWHRMech);
			Assert.AreEqual(whtcUrban, job.FCspecMeas_HotUrbWHRMech);
			Assert.AreEqual(whtcRural, job.FCspecMeas_HotRurWHRMech);
			Assert.AreEqual(whtcMotorway, job.FCspecMeas_HotMwWHRMech);
			Assert.AreEqual(cfRegPer, job.CF_RegPerWHRMech);
		}


		private string GetFilePath(string fileName)
		{
			return Path.Combine(_dirPath, fileName);
		}
	}
}
