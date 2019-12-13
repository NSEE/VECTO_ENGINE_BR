'
' This file is part of VECTO-Engine.
'
' Copyright © 2012-2017 European Union
'
' Developed by Graz University of Technology,
'              Institute of Internal Combustion Engines and Thermodynamics,
'              Institute of Technical Informatics
'
' VECTO is licensed under the EUPL, Version 1.1 or - as soon they will be approved
' by the European Commission - subsequent versions of the EUPL (the "Licence");
' You may not use VECTO except in compliance with the Licence.
' You may obtain a copy of the Licence at:
'
' https://joinup.ec.europa.eu/community/eupl/og_page/eupl
'
' Unless required by applicable law or agreed to in writing, VECTO
' distributed under the Licence is distributed on an "AS IS" basis,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the Licence for the specific language governing permissions and
' limitations under the Licence.
'
' Authors:
'   Stefan Hausberger, hausberger@ivt.tugraz.at, IVT, Graz University of Technology
'   Raphael Luz, luz@ivt.tugraz.at, IVT, Graz University of Technology
'   Markus Quaritsch, markus.quaritsch@tugraz.at, IVT, Graz University of Technology
'   Martin Rexeis, rexeis@ivt.tugraz.at, IVT, Graz University of Technology
'   Gérard Silberholz, silberholz@ivt.tugraz.at, IVT, Graz University of Technology
'
Imports System.IO


Public Class cJob
    Public JobFile As String
    Public MapFile As String
    Public FlcParentFile As String
    Public DragFile As String
    Public FlcFile As String

    Public DualFuel As Boolean
    Public WHR_ICE As Boolean
    Public WHR_Mech As Boolean
    Public WHR_El As Boolean

    Public Manufacturer As String
    Public Model As String
    Public CertNumber As String

    Public Displacement As Double
    Public RatedPower As Integer
    Public RatedSpeed As Integer

    Public Idle_Parent As Double
    Public Idle As Double

    Private MAP As cMAP0

    Private FlcParent As cFLD0
    Private Drag As cFLD0
    Private Flc As cFLD0

    Public WHTCsim As cWHTC
    'Public WHSCsim As cWHSC

    Public FuelType As String
    Public NCVfuel As Double
    Public FCspecMeas_ColdTot As Double
    Public FCspecMeas_HotTot As Double
    Public FCspecMeas_HotUrb As Double
    Public FCspecMeas_HotRur As Double
    Public FCspecMeas_HotMw As Double
    Public CF_RegPer As Double
    Public WHTCurbanFactor As Double
    Public WHTCruralFactor As Double
    Public WHTCmotorwayFactor As Double
    Public ColdHotBalancingFactor As Double

    Public FuelType2 As String
    Public NCVfuel2 As Double
    Public FCspecMeas_ColdTot2 As Double
    Public FCspecMeas_HotTot2 As Double
    Public FCspecMeas_HotUrb2 As Double
    Public FCspecMeas_HotRur2 As Double
    Public FCspecMeas_HotMw2 As Double
    Public CF_RegPer2 As Double
    Public WHTCurbanFactor2 As Double
    Public WHTCruralFactor2 As Double
    Public WHTCmotorwayFactor2 As Double
    Public ColdHotBalancingFactor2 As Double

    Public FCspecMeas_ColdTotWHRMech As Double
    Public FCspecMeas_HotTotWHRMech As Double
    Public FCspecMeas_HotUrbWHRMech As Double
    Public FCspecMeas_HotRurWHRMech As Double
    Public FCspecMeas_HotMwWHRMech As Double
    Public CF_RegPerWHRMech As Double
    Public WHTCurbanFactorWHRMech As Double
    Public WHTCruralFactorWHRMech As Double
    Public WHTCmotorwayFactorWHRMech As Double
    Public ColdHotBalancingFactorWHRMech As Double

    Public FCspecMeas_ColdTotWHREl As Double
    Public FCspecMeas_HotTotWHREl As Double
    Public FCspecMeas_HotUrbWHREl As Double
    Public FCspecMeas_HotRurWHREl As Double
    Public FCspecMeas_HotMwWHREl As Double
    Public CF_RegPerWHREl As Double
    Public WHTCurbanFactorWHREl As Double
    Public WHTCruralFactorWHREl As Double
    Public WHTCmotorwayFactorWHREl As Double
    Public ColdHotBalancingFactorWHREl As Double

    Public NCV_std As New Dictionary(Of String, Double)
    Public OutPath As String

    Public PT1 As cPT1

    Public Sub New()
    End Sub

    Public Sub New(dualFuel As Boolean, whrICE As Boolean, whrMech As Boolean, whrEl As Boolean)
        Me.DualFuel = dualFuel
        Me.WHR_ICE = whrICE
        Me.WHR_Mech = whrMech
        Me.WHR_El = whrEl
        NCV_std = New Dictionary(Of String, Double)
        'Fill values for standard NCVs in global dictionary
        NCV_std.Add("Diesel / CI", 42.7)
        NCV_std.Add("Ethanol / CI", 25.7)
        NCV_std.Add("Petrol / PI", 41.5)
        NCV_std.Add("Ethanol / PI", 29.1)
        NCV_std.Add("LPG / PI", 46.0)
        NCV_std.Add("Natural Gas / PI", 45.1)
        NCV_std.Add("Natural Gas / CI", 45.1)
    End Sub

    Public Function Run() As Boolean

        'Initialize Warning counter
        NumWarnings = 0

        WorkerMsg(tMsgID.Normal, "Analyzing input files")

        PT1 = New cPT1
        'PT1.Filepath = MyConfPath & "PT1.csv"
        If Not PT1.Init() Then Return False

        FlcParent = New cFLD0
        FlcParent.FilePath = FlcParentFile
        FlcParent.IdleSpeedValueForCheck = Idle_Parent
        If Not FlcParent.ReadFile(True, True) Then Return False

        Drag = New cFLD0
        Drag.FilePath = DragFile
        Drag.IdleSpeedValueForCheck = Idle_Parent
        If Not Drag.ReadFile(True, False) Then Return False

        Flc = New cFLD0
        Flc.FilePath = FlcFile
        Flc.IdleSpeedValueForCheck = Idle
        If Not Flc.ReadFile(True, True) Then Return False

        ' Calculate tolerances for fuel map torque values based on Tmax_overall (2% rule, paragraph 4.3.5.5 of technical annex)
        TqStepTol = Math.Max(TqStepTol_abs, 0.02*FlcParent.TqMax)

        MAP = New cMAP0(DualFuel, WHR_ICE, WHR_Mech, WHR_El)
        MAP.FilePath = MapFile
        MAP.FLC_Parent = FlcParent
        MAP.FLC = Flc
        MAP.Motoring = Drag

        try
            MAP.ReadFile
        Catch e As Exception
            WorkerMsg(tMsgID.Err, e.Message)
            Return False
        End Try

        If Not FlcParent.RpmCalc("CO2-parent full load") Then Return False
        If Not Flc.RpmCalc("actual engine full load") Then Return False

        ' Plausibility check of input value in GUI for P_rated and n_rated
        If Math.Abs(Flc.Pmax - RatedPower)/Flc.Pmax > 0.05 Then
            WorkerMsg(tMsgID.Warn,
                      "Rated power input in GUI deviates by more than +/- 5% from rated power calculated from engine full-load curve (" &
                      RatedPower & "[kW] vs. " &
                      Math.Round(Flc.Pmax, 2) & "[kW]).")
        End If


        'Assign char. speeds from full-load curve to Map
        MAP.Map_n_idle = FlcParent.n_idle
        MAP.Map_n_lo = FlcParent.n_lo
        MAP.Map_n_pref = FlcParent.n_pref
        MAP.Map_n_95h = FlcParent.n_95h
        MAP.Map_n_hi = FlcParent.n_hi

        'DEBUG ONLY
#If DEBUG Then
        If _
            Not _
            MAP.WriteFLD(
                Path.Combine(OutPath,
                             RemoveInvalidFileCharacters(
                                 "DEBUG_" & Manufacturer & "_" & Model & "_" & "_FLC_forMapCheck.vfld")), False) Then _
            Return False
#End If


        'Analyse FC Map
        'WorkerMsg(tMsgID.Normal, "Analysing Map")
        '  >>> now inside cMAP0
        If Not MAP.Init() Then Return False

        'Extrapolate fuel consumption map to cover knees in full load curve
        WorkerMsg(tMsgID.Normal, "Extrapolating Map")
        If Not MAP.ExtrapolateMap() Then Return False

        'Add motoring curve to Map (for FC calc around 0 Nm)
        If Not MAP.AddDrag() Then Return False

        If Not MAP.Triangulate Then Return False

        'Limit CO2-Parent full load to FC Map maximum torque
        WorkerMsg(tMsgID.Normal, "Comparing CO2-parent maximum torque to extrapolated Map maximum torque")
        If Not MAP.LimitFlcParentToMap() Then Return False
        'Limit motoring curve to interpolated values from FC Map
        If Not MAP.LimitDragtoMap() Then Return False


        WorkerMsg(tMsgID.Normal, "WHTC Initialisation")
        'Allocation of data used for calculation and reading of input files for WHTC
        WHTCsim = New cWHTC
        WHTCsim.Drag = Drag
        'NOT VALID ANYMORE: Use original full load (not limited from Map), since WHTC target torque values shall be calculated based on original full load curve
        'simulate WHTC with actual engine not parent
        WHTCsim.FullLoad = Flc
        WHTCsim.Map = MAP
        WHTCsim.PT1 = PT1
        WHTCsim.WHTC_n_idle = Flc.n_idle
        WHTCsim.WHTC_n_lo = Flc.n_lo
        WHTCsim.WHTC_n_pref = Flc.n_pref
        WHTCsim.WHTC_n_hi = Flc.n_hi
        If Not WHTCsim.InitCycle(False) Then Return False

        WorkerMsg(tMsgID.Normal, "WHTC Simulation")
        'NOT VALID ANYMORE: Use CO2-parent full load from Map, since this is limited by map maximum torque
        'NOT VALID ANYMORE: Different full load torque used in function "CalcFC of cWHTC"
        'simulate WHTC with actual engine not parent
        WHTCsim.FullLoad = Flc
        If Not WHTCsim.CalcFC() Then Return False
        If Not WHTCsim.CalcResults(False) Then Return False

        'Calculation of WHTC-Correction and Cold-Hot-Balancing-Factor
        WHTCurbanFactor = FCspecMeas_HotUrb/Math.Round(WHTCsim.Urban, 2)
        If WHTCurbanFactor < 1 Then WHTCurbanFactor = 1
        WHTCruralFactor = FCspecMeas_HotRur/Math.Round(WHTCsim.Rural, 2)
        If WHTCruralFactor < 1 Then WHTCruralFactor = 1
        WHTCmotorwayFactor = FCspecMeas_HotMw/Math.Round(WHTCsim.Motorway, 2)
        If WHTCmotorwayFactor < 1 Then WHTCmotorwayFactor = 1

        ColdHotBalancingFactor = 1 + 0.1*(FCspecMeas_ColdTot - FCspecMeas_HotTot)/FCspecMeas_HotTot
        If ColdHotBalancingFactor < 1 Then ColdHotBalancingFactor = 1


        If DualFuel Then
            'Calculation of WHTC-Correction and Cold-Hot-Balancing-Factor for second fuel
            WHTCurbanFactor2 = FCspecMeas_HotUrb2/Math.Round(WHTCsim.Urban2, 2)
            If WHTCurbanFactor2 < 1 Then WHTCurbanFactor2 = 1
            WHTCruralFactor2 = FCspecMeas_HotRur2/Math.Round(WHTCsim.Rural2, 2)
            If WHTCruralFactor2 < 1 Then WHTCruralFactor2 = 1
            WHTCmotorwayFactor2 = FCspecMeas_HotMw2/Math.Round(WHTCsim.Motorway2, 2)
            If WHTCmotorwayFactor2 < 1 Then WHTCmotorwayFactor2 = 1

            WorkerMsg(tMsgID.Normal, "WHTC Simulation Results - " & (FuelType).ToString & ":")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotFCspec).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "WHTC Simulation Results - " & (FuelType2).ToString & ":")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban2).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural2).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway2).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotFCspec2).ToString("f2") & " [g/kWh].")


            ColdHotBalancingFactor2 = 1 + 0.1*(FCspecMeas_ColdTot2 - FCspecMeas_HotTot2)/FCspecMeas_HotTot2
            If ColdHotBalancingFactor2 < 1 Then ColdHotBalancingFactor2 = 1
        Else
            WorkerMsg(tMsgID.Normal, "WHTC Simulation Results:")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotFCspec).ToString("f2") & " [g/kWh].")
        End If

        If WHR_Mech Then

            WHTCurbanFactorWHRMech = FCspecMeas_HotUrbWHRMech/Math.Round(WHTCsim.Urban3, 2)
            If WHTCurbanFactorWHRMech < 1 Then WHTCurbanFactorWHRMech = 1
            WHTCruralFactorWHRMech = FCspecMeas_HotRurWHRMech/Math.Round(WHTCsim.Rural3, 2)
            If WHTCruralFactorWHRMech < 1 Then WHTCruralFactorWHRMech = 1
            WHTCmotorwayFactorWHRMech = FCspecMeas_HotMwWHRMech/Math.Round(WHTCsim.Motorway3, 2)
            If WHTCmotorwayFactorWHRMech < 1 Then WHTCmotorwayFactorWHRMech = 1


            WorkerMsg(tMsgID.Normal, "WHRMechanical Simulation Results:")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban3).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural3).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway3).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotWHR2spec3).ToString("f2") & " [g/kWh].")

            ColdHotBalancingFactorWHRMech = 1 + 0.1*(FCspecMeas_ColdTotWHRMech - FCspecMeas_HotTotWHRMech)/FCspecMeas_HotTotWHRMech
            If ColdHotBalancingFactorWHRMech < 1 Then ColdHotBalancingFactorWHRMech = 1
        End If

        If WHR_El Then

            WHTCurbanFactorWHREl = FCspecMeas_HotUrbWHREl/Math.Round(WHTCsim.Urban2, 2)
            If WHTCurbanFactorWHREl < 1 Then WHTCurbanFactorWHREl = 1
            WHTCruralFactorWHREl = FCspecMeas_HotRurWHREl/Math.Round(WHTCsim.Rural2, 2)
            If WHTCruralFactorWHREl < 1 Then WHTCruralFactorWHREl = 1
            WHTCmotorwayFactorWHREl = FCspecMeas_HotMwWHREl/Math.Round(WHTCsim.Motorway2, 2)
            If WHTCmotorwayFactorWHREl < 1 Then WHTCmotorwayFactorWHREl = 1

            WorkerMsg(tMsgID.Normal, "WHRElectrical Simulation Results:")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban4).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural4).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway4).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotWHR3spec4).ToString("f2") & " [g/kWh].")

            ColdHotBalancingFactorWHREl = 1 + 0.1*(FCspecMeas_ColdTotWHREl - FCspecMeas_HotTotWHREl)/FCspecMeas_HotTotWHREl
            If ColdHotBalancingFactorWHREl < 1 Then ColdHotBalancingFactorWHREl = 1
        End If

        '' *** WHSC SIMULATION START
        'WorkerMsg(tMsgID.Normal, "WHSC Initialisation")
        ''Allocation of data used for calculation and reading of input files for WHSC
        'WHSCsim = New cWHSC
        'WHSCsim.Drag = Drag
        ''Use original CO2-parent full load (not limited from Map), since WHTC target torque values shall be calculated based on original full load curve
        'WHSCsim.FullLoad = FlcParent
        'WHSCsim.Map = MAP
        'WHSCsim.PT1 = PT1
        'WHSCsim.WHSC_n_idle = FlcParent.n_idle
        'WHSCsim.WHSC_n_lo = FlcParent.n_lo
        'WHSCsim.WHSC_n_pref = FlcParent.n_pref
        'WHSCsim.WHSC_n_hi = FlcParent.n_hi
        'If Not WHSCsim.InitCycle(MyConfPath & "WHSC.csv") Then Return False


        'WorkerMsg(tMsgID.Normal, "WHSC Simulation")
        ''Use CO2-parent full load from Map, since this is limited by map maximum torque
        ''Different full load torque used in function "CalcFC of cWHSC"
        'WHSCsim.FullLoad = MAP.FLC_Parent
        'If Not WHSCsim.CalcFC() Then Return False
        'If Not WHSCsim.CalcResults(False) Then Return False

        'WorkerMsg(tMsgID.Normal, "WHSC Simulation Results:")
        'WorkerMsg(tMsgID.Normal, "   Total: " & (WHSCsim.TotFCspec).ToString("0.0000") & " [g/kWh].")
        '' *** WHSC SIMULATION END


        'calculate NCV correction factor depending on fuel type
        MAP.NCV_CorrectionFactor = Math.Round(NCVfuel/NCV_std.Item(FuelType), 4)
        MAP.temp_Map_FuelType = FuelType
        If DualFuel Then
            MAP.NCV_CorrectionFactor2 = Math.Round(NCVfuel2/NCV_std.Item(FuelType2), 4)
            MAP.temp_Map_FuelType2 = FuelType2
        End If
        'MsgBox(MAP.NCV_CorrectionFactor)

        'Write output files
        WorkerMsg(tMsgID.Normal, "Writing XML output file")
        'If Not MAP.WriteMap(OutPath & fFILE(MapFile, False) & "_mod.vmap") Then Return False
#If DEBUG Then
        If _
            Not _
            MAP.WriteMap(Path.Combine(OutPath,
                                      RemoveInvalidFileCharacters(
                                          "UNOFFICIAL_OUTPUT_" & Manufacturer & "_" & Model & "_" & CertNumber &
                                          "_FCmap.vmap"))) _
            Then Return False
#End If

#If DEBUG Then
        If _
            Not _
            MAP.WriteFLD(
                Path.Combine(OutPath,
                             RemoveInvalidFileCharacters(
                                 "UNOFFICIAL_OUTPUT_" & Manufacturer & "_" & Model & "_" & CertNumber & "_FLC.vfld")),
                True) _
            Then Return False
#End If

        Dim outgenerated = MAP.WriteXmlComponentFile(
            Path.Combine(OutPath, RemoveInvalidFileCharacters(Manufacturer & "_" & Model & ".xml")), Me)

        'RpmWarnings()
        'RpmWarningsGearshifting()

        WorkerMsg(tMsgID.Normal, "Completed.")


        If NumWarnings > 0 Then
            WorkerMsg(tMsgID.Warn,
                      "ATTENTION:  " & NumWarnings &
                      " Warning(s) occured: Please check detailled descriptions in 'Message Window'!")
        End If

#If RELEASE_CANDIDATE Then
        WorkerMsg(tMsgID.Err,
                      "ATTENTION:  RELEASE CANDIDATE, NOT FOR CERTIFICATION!")
#End If

        Return outgenerated
    End Function

    Private Function RemoveInvalidFileCharacters(filename As String) As String
        return string.Join("", filename.Split(Path.GetInvalidFileNameChars()))
    End Function


    Private Sub RpmWarnings()

        'If Math.Abs(n_idle - FlcParent.n_idle) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_idle = " & FlcParent.n_idle & " [1/min]!")
        'WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_idle = " & FlcParent.n_idle & " [1/min]!")
        'If Math.Abs(n_lo - FlcParent.n_lo) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_lo = " & FlcParent.n_lo & " [1/min]!")

        'If Math.Abs(n_pref - FlcParent.n_pref) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_pref = " & FlcParent.n_pref & " [1/min]!")
        'WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_lo = " & FlcParent.n_lo & " [1/min]!")

        'If Math.Abs(n_95h - FlcParent.n_95h) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_95h = " & FlcParent.n_95h & " [1/min]!")
        'WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_pref = " & FlcParent.n_pref & " [1/min]!")

        'If Math.Abs(n_hi - FlcParent.n_hi) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_hi = " & FlcParent.n_hi & " [1/min]!")
        'WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_95h = " & FlcParent.n_95h & " [1/min]!")
    End Sub

    Private Sub RpmWarningsGearshifting()

        'If (n_idle - Flc.n_idle) >= 5 Then WorkerMsg(tMsgID.Warn, "n_idle for actual engine is lower than n_idle for Parent engine (will cause error in VECTO vehicle simulation)!")

        'If (n_95h - Flc.n_95h) <= -5 Then WorkerMsg(tMsgID.Warn, "n_95h for actual engine is higher than n_idle for Parent engine (will cause error in VECTO vehicle simulation)!")
    End Sub

    Private Function WriteTransFile(path As String) As Boolean
        Dim xml As New XDocument
        Dim xe As XElement
        Dim xe0 As XElement

        Try
            xe = New XElement("VECTO-Engine-TransferFile", New XAttribute("FileVersion", 1))

            xe0 = New XElement("Info")
            xe0.Add(New XElement("DateCreated", Now.ToString))
            xe0.Add(New XElement("MapFile", fFILE(MapFile, True)))
            xe0.Add(New XElement("FullloadFile", fFILE(FlcFile, True)))
            xe.Add(xe0)


            xe0 = New XElement("WHTCCorrectionFactors")
            xe0.Add(New XElement("Urban", WHTCurbanFactor))
            xe0.Add(New XElement("Rural", WHTCruralFactor))
            xe0.Add(New XElement("Motorway", WHTCmotorwayFactor))
            xe.Add(xe0)

            xml.Add(xe)
            xml.Save(path)

        Catch ex As Exception
            WorkerMsg(tMsgID.Err, "Failed to write VECTO transfer file! " & ex.Message)
            Return False
        End Try

        Return True
    End Function
End Class


