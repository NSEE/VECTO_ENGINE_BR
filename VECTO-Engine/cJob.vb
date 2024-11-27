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

    Public Displacement? As Double = Nothing
    Public RatedPower? As Integer = Nothing
    Public RatedSpeed? As Integer = Nothing

    Public Idle_Parent? As Double = Nothing
    Public Idle? As Double = Nothing

    Private MAP As cMAP0

    Private FlcParent As cFLD0
    Private Drag As cFLD0
    Private Flc As cFLD0

    Public WHTCsim As cWHTC
    'Public WHSCsim As cWHSC

    Public FuelType As String
    Public NCVfuel? As Double = Nothing
    Public FCspecMeas_ColdTot? As Double = Nothing
    Public FCspecMeas_HotTot? As Double = Nothing
    Public FCspecMeas_HotUrb? As Double = Nothing
    Public FCspecMeas_HotRur? As Double = Nothing
    Public FCspecMeas_HotMw? As Double = Nothing
    Public CF_RegPer? As Double = Nothing
    Public WHTCurbanFactor? As Double = Nothing
    Public WHTCruralFactor? As Double = Nothing
    Public WHTCmotorwayFactor? As Double = Nothing
    Public ColdHotBalancingFactor? As Double = Nothing

    Public FuelType2 As String
    Public NCVfuel2? As Double = Nothing
    Public FCspecMeas_ColdTot2? As Double = Nothing
    Public FCspecMeas_HotTot2? As Double = Nothing
    Public FCspecMeas_HotUrb2? As Double = Nothing
    Public FCspecMeas_HotRur2? As Double = Nothing
    Public FCspecMeas_HotMw2? As Double = Nothing
    Public CF_RegPer2? As Double = Nothing
    Public WHTCurbanFactor2? As Double = Nothing
    Public WHTCruralFactor2? As Double = Nothing
    Public WHTCmotorwayFactor2? As Double = Nothing
    Public ColdHotBalancingFactor2? As Double = Nothing

    Public FCspecMeas_ColdTotWHRMech? As Double = Nothing
    Public FCspecMeas_HotTotWHRMech? As Double = Nothing
    Public FCspecMeas_HotUrbWHRMech? As Double = Nothing
    Public FCspecMeas_HotRurWHRMech? As Double = Nothing
    Public FCspecMeas_HotMwWHRMech? As Double = Nothing
    Public CF_RegPerWHRMech? As Double = Nothing
    Public WHTCurbanFactorWHRMech? As Double = Nothing
    Public WHTCruralFactorWHRMech? As Double = Nothing
    Public WHTCmotorwayFactorWHRMech? As Double = Nothing
    Public ColdHotBalancingFactorWHRMech? As Double = Nothing

    Public FCspecMeas_ColdTotWHREl? As Double = Nothing
    Public FCspecMeas_HotTotWHREl? As Double = Nothing
    Public FCspecMeas_HotUrbWHREl? As Double = Nothing
    Public FCspecMeas_HotRurWHREl? As Double = Nothing
    Public FCspecMeas_HotMwWHREl? As Double = Nothing
    Public CF_RegPerWHREl? As Double = Nothing
    Public WHTCurbanFactorWHREl? As Double = Nothing
    Public WHTCruralFactorWHREl? As Double = Nothing
    Public WHTCmotorwayFactorWHREl? As Double = Nothing
    Public ColdHotBalancingFactorWHREl? As Double = Nothing

    Public NCV_std As New Dictionary(Of String, Double) From {
        {"Diesel / CI", 42.7},
        {"Ethanol / CI", 25.7},
        {"Petrol / PI", 41.5},
        {"Ethanol / PI", 29.1},
        {"LPG / PI", 46.0},
        {"Natural Gas / PI", 45.1},
        {"Natural Gas / CI", 45.1},
        {"H2 / PI", 120},
        {"H2 / CI", 120},
        {"Diesel B100 CI", 37.2}
        }
    Public OutPath As String

    Public PT1 As cPT1

    Public SwitchFuelsFlag As Boolean


    Public Sub New()
    End Sub

    Public Sub New(dualFuel As Boolean, whrICE As Boolean, whrMech As Boolean, whrEl As Boolean)
        Me.DualFuel = dualFuel
        Me.WHR_ICE = whrICE
        Me.WHR_Mech = whrMech
        Me.WHR_El = whrEl

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
        TqStepTol = Math.Max(TqStepTol_abs, 0.02 * FlcParent.TqMax)

        MAP = New cMAP0(DualFuel, WHR_ICE, WHR_Mech, WHR_El)
        MAP.FilePath = MapFile
        MAP.FLC_Parent = FlcParent
        MAP.FLC = Flc
        MAP.Motoring = Drag

        Try
            MAP.ReadFile()
        Catch e As Exception
            WorkerMsg(tMsgID.Err, e.Message)
            Return False
        End Try

        If Not FlcParent.RpmCalc("CO2-parent full load") Then Return False
        If Not Flc.RpmCalc("actual engine full load") Then Return False

        ' Plausibility check of input value in GUI for P_rated and n_rated
        If Math.Abs(Flc.Pmax - RatedPower.Value) / Flc.Pmax > 0.05 Then
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

        WHTC_CorrFactorsAndOutput()
        CalculateWHRMech()
        CalculateWHREl()

        'calculate NCV correction factor depending on fuel type
        MAP.NCV_CorrectionFactor = Math.Round(NCVfuel.Value / NCV_std.Item(FuelType), 4)
        MAP.temp_Map_FuelType = FuelType
        If DualFuel Then
            MAP.NCV_CorrectionFactor2 = Math.Round(NCVfuel2.Value / NCV_std.Item(FuelType2), 4)
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

        Dim componentXmlGenerated As Boolean

        If FuelType.StartsWith("H2") OrElse (FuelType2 IsNot Nothing andalso FuelType2.StartsWith("H2")) _
                        orelse FuelType.Equals("Diesel B100 CI") OrElse (FuelType2 IsNot Nothing AndAlso FuelType2.Equals("Diesel B100 CI"))
            componentXmlGenerated = MAP.WriteXmlComponentFileV211_DEV(
                Path.Combine(OutPath, RemoveInvalidFileCharacters(Manufacturer & "_" & Model & ".xml")), Me)
        'Else If DualFuel = True OrElse
        '   WHR_ICE = True OrElse
        '   WHR_Mech = True OrElse
        '   WHR_El = True Then

        '    componentXmlGenerated = MAP.WriteXmlComponentFileV23(
        '        Path.Combine(OutPath, RemoveInvalidFileCharacters(Manufacturer & "_" & Model & ".xml")), Me)
        Else
            componentXmlGenerated = MAP.WriteXmlComponentFileV23(
                Path.Combine(OutPath, RemoveInvalidFileCharacters(Manufacturer & "_" & Model & ".xml")), Me)
        End If

        WorkerMsg(tMsgID.Normal, "Completed.")


        If NumWarnings > 0 Then
            WorkerMsg(tMsgID.Warn,
                      "ATTENTION:  " & NumWarnings &
                      " Warning(s) occured: Please check detailled descriptions in 'Message Window'!")
        End If

#If CERTIFICATION_RELEASE Then
#else
#If RELEASE_CANDIDATE Then
        WorkerMsg(tMsgID.Err,
                      "ATTENTION:  RELEASE CANDIDATE, NOT FOR CERTIFICATION!")
#Else
        WorkerMsg(tMsgID.Err,
                  "ATTENTION:  DEVELOPMENT VERSION, NOT FOR CERTIFICATION!")
#End If
#End If
        Return componentXmlGenerated
    End Function

    Private Sub WHTC_CorrFactorsAndOutput()
        Dim HelpEnergyShareFuel1 As Double

        If DualFuel Then

            'Calculation of WHTC-Correction and Cold-Hot-Balancing-Factor
            WHTCurbanFactor = FCspecMeas_HotUrb / Math.Round(WHTCsim.Urban, 2)
            WHTCruralFactor = FCspecMeas_HotRur / Math.Round(WHTCsim.Rural, 2)
            WHTCmotorwayFactor = FCspecMeas_HotMw / Math.Round(WHTCsim.Motorway, 2)
            ColdHotBalancingFactor = 1 + 0.1 * (FCspecMeas_ColdTot - FCspecMeas_HotTot) / FCspecMeas_HotTot

            'Calculation of WHTC-Correction and Cold-Hot-Balancing-Factor for second fuel
            WHTCurbanFactor2 = FCspecMeas_HotUrb2 / Math.Round(WHTCsim.UrbanFC2, 2)
            WHTCruralFactor2 = FCspecMeas_HotRur2 / Math.Round(WHTCsim.RuralFC2, 2)
            WHTCmotorwayFactor2 = FCspecMeas_HotMw2 / Math.Round(WHTCsim.MotorwayFC2, 2)
            ColdHotBalancingFactor2 = 1 + 0.1 * (FCspecMeas_ColdTot2 - FCspecMeas_HotTot2) / FCspecMeas_HotTot2

            'Calculation of energy share per fuel in case of dual fuel
            ' WHTCsim.EnergyFuel value is at this point an intermediate result in unit of total [g] over cycle
            ' --> intermediate result is converted to final energy
            WHTCsim.EnergyShareFuel1 = WHTCsim.EnergyShareFuel1 * NCVfuel
            WHTCsim.EnergyShareFuel2 = WHTCsim.EnergyShareFuel2 * NCVfuel2
            HelpEnergyShareFuel1 = WHTCsim.EnergyShareFuel1 / (WHTCsim.EnergyShareFuel1 + WHTCsim.EnergyShareFuel2)
            WHTCsim.EnergyShareFuel2 = WHTCsim.EnergyShareFuel2 / (WHTCsim.EnergyShareFuel1 + WHTCsim.EnergyShareFuel2)
            WHTCsim.EnergyShareFuel1 = HelpEnergyShareFuel1

            ' Check if fuel1 and fuel2 need to be switched due to main energy share
            If WHTCsim.EnergyShareFuel2 > WHTCsim.EnergyShareFuel1 Then SwitchFuelsFlag = True


            ' Special rules for limitation of combined correction factors weighted with specific fuel energy share
            ' ATTENTION: Only allow calculation of new value for fuel with higher energy share otherwise negative correction factors could occur for fuel with very low energy share
            If WHTCurbanFactor < 1 AndAlso WHTCurbanFactor2 < 1 Then
                WHTCurbanFactor = 1
                WHTCurbanFactor2 = 1
            ElseIf WHTCurbanFactor >= 1 AndAlso WHTCurbanFactor2 < 1 AndAlso WHTCsim.EnergyShareFuel2 > 0.5 Then
                WHTCurbanFactor2 = (1 - Math.Round(WHTCurbanFactor.Value, 4) * WHTCsim.EnergyShareFuel1) / WHTCsim.EnergyShareFuel2
            ElseIf WHTCurbanFactor < 1 AndAlso WHTCurbanFactor2 >= 1 AndAlso WHTCsim.EnergyShareFuel1 > 0.5 Then
                WHTCurbanFactor = (1 - Math.Round(WHTCurbanFactor2.Value, 4) * WHTCsim.EnergyShareFuel2) / WHTCsim.EnergyShareFuel1
            End If
            If WHTCruralFactor < 1 AndAlso WHTCruralFactor2 < 1 Then
                WHTCruralFactor = 1
                WHTCruralFactor2 = 1
            ElseIf WHTCruralFactor >= 1 AndAlso WHTCruralFactor2 < 1 AndAlso WHTCsim.EnergyShareFuel2 > 0.5 Then
                WHTCruralFactor2 = (1 - Math.Round(WHTCruralFactor.Value, 4) * WHTCsim.EnergyShareFuel1) / WHTCsim.EnergyShareFuel2
            ElseIf WHTCruralFactor < 1 AndAlso WHTCruralFactor2 >= 1 AndAlso WHTCsim.EnergyShareFuel1 > 0.5 Then
                WHTCruralFactor = (1 - Math.Round(WHTCruralFactor2.Value, 4) * WHTCsim.EnergyShareFuel2) / WHTCsim.EnergyShareFuel1
            End If
            If WHTCmotorwayFactor < 1 AndAlso WHTCmotorwayFactor2 < 1 Then
                WHTCmotorwayFactor = 1
                WHTCmotorwayFactor2 = 1
            ElseIf WHTCmotorwayFactor >= 1 AndAlso WHTCmotorwayFactor2 < 1 AndAlso WHTCsim.EnergyShareFuel2 > 0.5 Then
                WHTCmotorwayFactor2 = (1 - Math.Round(WHTCmotorwayFactor.Value, 4) * WHTCsim.EnergyShareFuel1) / WHTCsim.EnergyShareFuel2
            ElseIf WHTCmotorwayFactor < 1 AndAlso WHTCmotorwayFactor2 >= 1 AndAlso WHTCsim.EnergyShareFuel1 > 0.5 Then
                WHTCmotorwayFactor = (1 - Math.Round(WHTCmotorwayFactor2.Value, 4) * WHTCsim.EnergyShareFuel2) / WHTCsim.EnergyShareFuel1
            End If
            If ColdHotBalancingFactor < 1 AndAlso ColdHotBalancingFactor2 < 1 Then
                ColdHotBalancingFactor = 1
                ColdHotBalancingFactor2 = 1
            ElseIf ColdHotBalancingFactor >= 1 AndAlso ColdHotBalancingFactor2 < 1 AndAlso WHTCsim.EnergyShareFuel2 > 0.5 Then
                ColdHotBalancingFactor2 = (1 - Math.Round(ColdHotBalancingFactor.Value, 4) * WHTCsim.EnergyShareFuel1) / WHTCsim.EnergyShareFuel2
            ElseIf ColdHotBalancingFactor < 1 AndAlso ColdHotBalancingFactor2 >= 1 AndAlso WHTCsim.EnergyShareFuel1 > 0.5 Then
                ColdHotBalancingFactor = (1 - Math.Round(ColdHotBalancingFactor2.Value, 4) * WHTCsim.EnergyShareFuel2) / WHTCsim.EnergyShareFuel1
            End If

            WorkerMsg(tMsgID.Normal, "WHTC Simulation Results - " & (FuelType).ToString & ":")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotFCspec).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "WHTC Simulation Results - " & (FuelType2).ToString & ":")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.UrbanFC2).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.RuralFC2).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.MotorwayFC2).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotFCspec2).ToString("f2") & " [g/kWh].")

        Else

            'Calculation of WHTC-Correction and Cold-Hot-Balancing-Factor
            WHTCurbanFactor = FCspecMeas_HotUrb / Math.Round(WHTCsim.Urban, 2)
            If WHTCurbanFactor < 1 Then WHTCurbanFactor = 1
            WHTCruralFactor = FCspecMeas_HotRur / Math.Round(WHTCsim.Rural, 2)
            If WHTCruralFactor < 1 Then WHTCruralFactor = 1
            WHTCmotorwayFactor = FCspecMeas_HotMw / Math.Round(WHTCsim.Motorway, 2)
            If WHTCmotorwayFactor < 1 Then WHTCmotorwayFactor = 1

            ColdHotBalancingFactor = 1 + 0.1 * (FCspecMeas_ColdTot - FCspecMeas_HotTot) / FCspecMeas_HotTot
            If ColdHotBalancingFactor < 1 Then ColdHotBalancingFactor = 1

            WorkerMsg(tMsgID.Normal, "WHTC Simulation Results:")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway).ToString("f2") & " [g/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotFCspec).ToString("f2") & " [g/kWh].")

        End If

    End Sub

    Private Sub CalculateWHRMech()
        
        If WHR_Mech Then
            WHTCurbanFactorWHRMech = FCspecMeas_HotUrbWHRMech / Math.Round(WHTCsim.WHRMechUrban, 2)
            WHTCruralFactorWHRMech = FCspecMeas_HotRurWHRMech / Math.Round(WHTCsim.WHRMechRural, 2)
            WHTCmotorwayFactorWHRMech = FCspecMeas_HotMwWHRMech / Math.Round(WHTCsim.WHRMechMotorway, 2)

            WorkerMsg(tMsgID.Normal, "WHRMechanical Simulation Results:")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.WHRMechUrban).ToString("f2") & " [kJ/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.WHRMechRural).ToString("f2") & " [kJ/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.WHRMechMotorway).ToString("f2") & " [kJ/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotWHRMechSpec).ToString("f2") & " [kJ/kWh].")

            ColdHotBalancingFactorWHRMech = 1 + 0.1 * (FCspecMeas_ColdTotWHRMech - FCspecMeas_HotTotWHRMech) / FCspecMeas_HotTotWHRMech
        End If

    End Sub

    Private Sub CalculateWHREl()
        
        If WHR_El Then

            WHTCurbanFactorWHREl = FCspecMeas_HotUrbWHREl / Math.Round(WHTCsim.WHRElUrban, 2)
            WHTCruralFactorWHREl = FCspecMeas_HotRurWHREl / Math.Round(WHTCsim.WHRElRural, 2)
            WHTCmotorwayFactorWHREl = FCspecMeas_HotMwWHREl / Math.Round(WHTCsim.WHRElMotorway, 2)

            WorkerMsg(tMsgID.Normal, "WHRElectrical Simulation Results:")
            WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.WHRElUrban).ToString("f2") & " [kJ/kWh].")
            WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.WHRElRural).ToString("f2") & " [kJ/kWh].")
            WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.WHRElMotorway).ToString("f2") & " [kJ/kWh].")
            WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotWHRElSpec).ToString("f2") & " [kJ/kWh].")

            ColdHotBalancingFactorWHREl = 1 + 0.1*(FCspecMeas_ColdTotWHREl - FCspecMeas_HotTotWHREl)/FCspecMeas_HotTotWHREl
        End If

    End Sub



    Private Function RemoveInvalidFileCharacters(filename As String) As String
        return string.Join("", filename.Split(Path.GetInvalidFileNameChars()))
    End Function


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


