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
Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Schema
Imports TUGraz.VectoCommon.InputData
Imports TUGraz.VectoCore.Utils
Imports TUGraz.VectoHashing
Imports VECTO_Engine.My.Resources


Module Fields
    Public EngineSpeed As String = "engine speed"
    Public Torque As String = "torque"
    Public Fuel1 As String = "massflow fuel 1"
    Public Fuel2 As String = "massflow fuel 2"
    Public WHRelectric As String = "WHR electrical power"
    Public WHRmechanical As String = "WHR mechanical power"
End Module

Public Class cMAP0
    Public FilePath As String

    Private ReadOnly MapPoints As New List(Of cMapPoint)
    Private iMapDim As Integer

    Private ReadOnly RPMlists As New SortedDictionary(Of Double, cRPMlist)
    Public FLC_Parent As cFLD0
    Public FLC As cFLD0
    Public Motoring As cFLD0

    Public Map_n_idle As Double
    Public Map_n_lo As Double
    Public Map_n_pref As Double
    Public Map_n_95h As Double
    Public Map_n_hi As Double

    Public NCV_CorrectionFactor As Double
    Public temp_Map_FuelType As String
    Public NCV_CorrectionFactor2 As Double
    Public temp_Map_FuelType2 As String

    Private TqStep As Double
    Private nUStepTol As Double

    Private ReadOnly TqList As New List(Of Double)

    'For dual fuel
    Private FuelMap1 As cDelaunayMap
    Private FuelMap2 As cDelaunayMap

    'For WHR
    Private WHRMapMech As cDelaunayMap
    Private WHRMapEl As cDelaunayMap
    'z-Wert only difference by delaunay 

    Public DualFuel As Boolean
    Public WHR1 As Boolean
    Public WHRMechanicalEnabled As Boolean
    Public WHRElectricalEnabled As Boolean

    Public tableData As TableData

    Private Shared ReadOnly HeaderFilter As Regex = New Regex("\[.*?\]|\<|\>", RegexOptions.Compiled)
    Private Const Delimiter As String = ","
    Private Const Comment As String = "#"
    Private Const DigestValuePrefix As String = "#@"

    protected NumberRegex = new Regex("[0-9?]\.[0-9]{2}")

    Private ReadOnly LnU_out As New List(Of Double)
    Private ReadOnly LTq_out As New List(Of Double)
    Private ReadOnly LTqMot_out As New List(Of Double)
    Private ReadOnly LnU_outTemp As New List(Of Double)
    Private ReadOnly LTq_outTemp As New List(Of Double)
    
    Private ReadOnly tnsV10 as XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationComponent:v1.0"
    Private Readonly tnsV20 As XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationComponent:v2.0"
    Private Readonly v20 As XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationDefinitions:v2.0"
    Private ReadOnly v10 as XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationDefinitions:v1.0"
    Private Readonly v23 As XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationDefinitions:v2.3"
    Private Readonly xsi As XNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance")
    Private Readonly di As XNamespace = XNamespace.Get("http://www.w3.org/2000/09/xmldsig#")


    Public Sub New(newdualFuel As Boolean, newWHR1 As Boolean, newWhrMechanicalEnabled As Boolean,
                   newWhrElectricalEnabled As Boolean)
        DualFuel = newdualFuel
        WHR1 = newWHR1
        WHRMechanicalEnabled = newWhrMechanicalEnabled
        WHRElectricalEnabled = newWhrElectricalEnabled
    End Sub


    'Check whether the header of the file is valid
    Private Function CheckHeaderIsValid(columns As DataColumnCollection) As Boolean

        dim expectedColumns = New List(Of string)() from {
                EngineSpeed, Torque, Fuel1
                }

        if (DualFuel)
            expectedColumns.Add(Fuel2)
        End If
        if (WHRElectricalEnabled)
            expectedColumns.Add(WHRelectric)
        End If
        if (WHRMechanicalEnabled)
            expectedColumns.Add(WHRmechanical)
        End If

        if expectedColumns.Any(Function(column) Not columns.Contains(column)) Then
            If (expectedColumns.Count = 3 AndAlso columns.Count = 3) Then
                columns(0).ColumnName = EngineSpeed
                columns(1).ColumnName = Torque
                columns(2).ColumnName = Fuel1
                Return true
            end if

            Throw New FormatException(string.Format("Invalid Map-File header! expected: '{0}', given: '{1}'",
                                                    String.Join(", ", expectedColumns),
                                                    string.Join(", ",
                                                                columns.Cast (Of DataColumn).Select(
                                                                    function(c) c.ColumnName))))
        End If
    End Function


    Public Function ReadFile()

        Dim nU As Double
        Dim Tq As Double
        Dim FC1 As Double
        Dim FC2 As Double
        Dim WHRElectric As Double
        Dim WHRMechanical As Double

        'Stop if there's no file
        If FilePath = "" OrElse Not File.Exists(FilePath) Then
            WorkerMsg(tMsgID.Err, "Map file not found! (" & FilePath & ")")
            Return False
        End If

        'Read the header of the map

        tableData = VectoCSVFile.Read(FilePath)


        'Check if header is valid
        CheckHeaderIsValid(tableData.Columns)

        MapPoints.Clear()
        iMapDim = 0
        FuelMap1 = New cDelaunayMap

        If DualFuel Then FuelMap2 = New cDelaunayMap

        If WHRMechanicalEnabled Then WHRMapMech = New cDelaunayMap

        If WHRElectricalEnabled Then WHRMapEl = New cDelaunayMap

        For Each line As DataRow In tableData.Rows

            iMapDim += 1

            Try
                nU = ReadValue(line, EngineSpeed)

                Tq = ReadValue(line, Torque)
                FC1 = ReadValue(line, Fuel1)
                FC2 = if(DualFuel, ReadValue(line, Fuel2), 0)
                WHRMechanical = if(WHRMechanicalEnabled, ReadValue(line, Fields.WHRmechanical), 0)
                WHRElectric = If(WHRElectricalEnabled, ReadValue(line, Fields.WHRelectric), 0)

            Catch ex As Exception
                'WorkerMsg(tMsgID.Err,String.Format("Failed reading input data on line {0}: {1}", iMapDim + 1, ex.Message))
                Throw New Exception(String.Format("Failed reading input data on line {0}: {1}", iMapDim + 1, ex.Message))
            End Try

            Dim mapPoint As cMapPoint = New cMapPoint With {
                    .nU = nU,
                    .Tq = Tq,
                    .FC1 = FC1,
                    .FC2 = FC2,
                    .WHRMechanical = WHRMechanical,
                    .WHRElectrical = WHRElectric}

            MapPoints.Add(mapPoint)
        Next

    End Function

    Private Function ReadValue(line As DataRow, field As String) As Double
        Dim strValue = line.Field (of string)(field)
        If Not NumberRegex.IsMatch(strValue) Then
            Throw _
                New FormatException(
                    String.Format("{0} value needs to have exactly 2 digits after the decimal point: {1}", field,
                                  strValue))
        End If
        Dim value = double.Parse(strValue, CultureInfo.InvariantCulture)
        If (Not field = Fields.Torque) AndAlso value < 0 Then
            Throw New FormatException(String.Format("Negative {0} value ({1}) not allowed!", field, value))
        End If
        Return value
    End Function


    Public Function Init() As Boolean
        Dim i As Integer
        Dim nU_Double As Double
        Dim nUStep As Double
        Dim TqStepMax As Double
        Dim IntervalCount As Integer
        Dim mp0 As cMapPoint
        Dim Matched As Boolean
        Dim TargetRPMs As New List(Of Double)
        Dim rl As cRPMlist
        ' now in Declarations of Class
        'Dim TqList As New List(Of Double)
        Dim Tq As Double
        Dim TqTarget As Double
        Dim MapSpeed_nA As Double
        Dim MapSpeed_nB As Double
        Dim MapSpeed_n57 As Double
        Dim dn_idle_A_44 As Double
        Dim dn_B_95h_44 As Double
        Dim dn_idle_A_35 As Double
        Dim dn_B_95h_35 As Double
        Dim dn_idle_A_53 As Double
        Dim dn_B_95h_53 As Double
        Dim dn_44 As Double
        Dim dn_35 As Double
        Dim dn_53 As Double
        Dim IntervalTarget(1) As Integer
        Dim NumPointsBelowFL As Integer
        Dim TqToleranceEngFam As Double


        ' ADDITIONAL CHECKS for input data
        WorkerMsg(tMsgID.Normal, "Analysing validity of input data acc. to CO2-family definition")
        '**************************************
        ' Checks for family concept
        '**************************************

        ' 1. idle speed parent <= idle speed of engine to be certified
        If FLC.n_idle < FLC_Parent.n_idle Then
            WorkerMsg(tMsgID.Err,
                      "Idle speed of engine to be certified lower than idle speed of CO2-parent engine (not valid acc. to CO2-family definition).")
            Return False
        End If
        ' 2. n_95h speed of engine to be certified within +/-3% of n_95h speed of parent
        If Math.Abs(FLC.n_95h - FLC_Parent.n_95h)/FLC_Parent.n_95h > 0.03 Then
            WorkerMsg(tMsgID.Err,
                      "n_95h speed of engine to be certified deviates more than +/-3% from n_95h speed of CO2-parent engine (not valid acc. to CO2-family definition).")
            Return False
        End If
        ' 3. n_57 speed of engine to be certified within +/-3% of n_57 speed of parent
        If Math.Abs(FLC.n_57 - FLC_Parent.n_57)/FLC_Parent.n_57 > 0.03 Then
            WorkerMsg(tMsgID.Err,
                      "n_57 speed of engine to be certified deviates more than +/-3% from n_57 speed of CO2-parent engine (not valid acc. to CO2-family definition).")
            Return False
        End If
        ' 4. Tq full-load parent >= tq full-load engine to be certified
        For i = 0 To FLC_Parent.iDim_orig
            TqToleranceEngFam = Math.Max(FLC_Parent.Tq_orig(FLC_Parent.LnU_orig(i))*0.02, 20)
            If _
                (FLC_Parent.Tq_orig(FLC_Parent.LnU_orig(i)) + TqToleranceEngFam) < FLC.Tq_orig(FLC_Parent.LnU_orig(i)) AndAlso
                FLC_Parent.Tq_orig(FLC_Parent.LnU_orig(i)) > 0 Then
                WorkerMsg(tMsgID.Err,
                          "Full-load torque of CO2-parent engine (incl. tolerance) lower than full-load torque of engine to be certified at " &
                          FLC_Parent.LnU_orig(i) & "[1/min] (not valid acc. to CO2-family definition).")
                Return False
            End If
        Next


        '**************************************
        ' Checks for family concept - END
        '**************************************


        WorkerMsg(tMsgID.Normal, "Analysing Map")

        'Calculate tolerance for engine speed value of fuel map points based on highest target engine speed (paragraph 4.3.5.5 of technical annex)
        nUStepTol = 0.01*Map_n_95h

        'Define target rpms for FC Map
        TargetRPMs.Clear()

        MapSpeed_n57 = 0.565*(0.45*Map_n_lo + 0.45*Map_n_pref + 0.1*Map_n_hi - Map_n_idle)*2.0327 + Map_n_idle
        MapSpeed_nA = MapSpeed_n57 - 0.05*(Map_n_95h - Map_n_idle)
        MapSpeed_nB = MapSpeed_n57 + 0.08*(Map_n_95h - Map_n_idle)

        TargetRPMs.Add(Math.Round(Map_n_idle, 2))
        TargetRPMs.Add(Math.Round(MapSpeed_nA, 2))
        TargetRPMs.Add(Math.Round(MapSpeed_nB, 2))
        TargetRPMs.Add(Math.Round(Map_n_95h, 2))


        'Calculate speed intervals for definition of 4/4, 3/5 or 5/3 distribution
        dn_idle_A_44 = (MapSpeed_nA - Map_n_idle)/4
        dn_B_95h_44 = (Map_n_95h - MapSpeed_nB)/4
        dn_idle_A_35 = (MapSpeed_nA - Map_n_idle)/3
        dn_B_95h_35 = (Map_n_95h - MapSpeed_nB)/5
        dn_idle_A_53 = (MapSpeed_nA - Map_n_idle)/5
        dn_B_95h_53 = (Map_n_95h - MapSpeed_nB)/3

        dn_44 = Math.Abs(dn_idle_A_44 - dn_B_95h_44)
        dn_35 = Math.Abs(dn_idle_A_35 - dn_B_95h_35)
        dn_53 = Math.Abs(dn_idle_A_53 - dn_B_95h_53)

        If (dn_44 <= (dn_35 + 5)) And (dn_44 <= (dn_53 + 5)) Then
            IntervalTarget(0) = 4
            IntervalTarget(1) = 4
        ElseIf ((dn_35 + 5) < dn_44) And (dn_35 < dn_53) Then
            IntervalTarget(0) = 3
            IntervalTarget(1) = 5
        ElseIf ((dn_53 + 5) < dn_44) And (dn_53 < dn_35) Then
            IntervalTarget(0) = 5
            IntervalTarget(1) = 3
        End If


        'Check if values at zero torque are present
        If (From mp As cMapPoint In MapPoints Where Math.Abs(mp.Tq) <= TqStepTol).Count = 0 Then
            WorkerMsg(tMsgID.Err, "No values in FC map at zero torque!")
            WorkerMsg(tMsgID.Err, "  >>> Check also tolerances for torque points at zero torque!")
            Return False
        End If

        'Count points between n_idle to MapSpeed_nA at zero torque
        IntervalCount = (From mp As cMapPoint In MapPoints Where Math.Abs(mp.Tq) <= TqStepTol _
                                                                 AndAlso mp.nU > Map_n_idle + nUStepTol _
                                                                 AndAlso mp.nU < MapSpeed_nA - nUStepTol).Count + 1

        If IntervalCount <> IntervalTarget(0) Then
            WorkerMsg(tMsgID.Err,
                      "Incorrect rpm intervals between n_idle and n_A (" & IntervalCount & " intervals, but should be " &
                      IntervalTarget(0) & ")!")
            WorkerMsg(tMsgID.Err, "  >>> Check also tolerances for torque points at zero torque!")
            Return False
        End If

        'Fill in list part1
        nUStep = (MapSpeed_nA - Map_n_idle)/IntervalCount
        For i = 1 To IntervalCount - 1
            TargetRPMs.Add(Math.Round(Map_n_idle + nUStep*i, 1))
        Next

        'Count points between MapSpeed_nB and n_95h at zero torque
        IntervalCount = (From mp As cMapPoint In MapPoints Where Math.Abs(mp.Tq) <= TqStepTol _
                                                                 AndAlso mp.nU > MapSpeed_nB + nUStepTol _
                                                                 AndAlso mp.nU < Map_n_95h - nUStepTol).Count + 1

        If IntervalCount <> IntervalTarget(1) Then
            WorkerMsg(tMsgID.Err,
                      "Incorrect rpm intervals between n_B and n_95h (" & IntervalCount & " intervals, but should be " &
                      IntervalTarget(1) & ")!")
            WorkerMsg(tMsgID.Err, "  >>> Check also tolerances for torque points at zero torque!")
            Return False
        End If

        'Fill in list part2
        nUStep = (Map_n_95h - MapSpeed_nB)/IntervalCount
        For i = 1 To IntervalCount - 1
            TargetRPMs.Add(Math.Round(MapSpeed_nB + nUStep*i, 1))
        Next

        'Sort list
        TargetRPMs.Sort()

        'Feed RPM lists and check rpm tolerances
        RPMlists.Clear()

        For Each nU_Double In TargetRPMs
            RPMlists.Add(nU_Double, New cRPMlist(nU_Double))
        Next

        For Each mp0 In MapPoints

            Matched = False

            For Each nU_Double In TargetRPMs
                If Math.Abs(nU_Double - mp0.nU) <= nUStepTol Then
                    RPMlists(nU_Double).MapPoints.Add(mp0)
                    Matched = True
                    Exit For
                End If
            Next

            If Not Matched Then
                WorkerMsg(tMsgID.Err, "Invalid rpm value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
                WorkerMsg(tMsgID.Err,
                          "  >>> Calculated target rpm values are: " & String.Join(", ", TargetRPMs.ToArray) &
                          " [1/min].")
                Return False
            End If

        Next

        'Sort rpm lists by torque
        For Each rl In RPMlists.Values
            rl.MapPoints = (From mp As cMapPoint In rl.MapPoints Order By mp.Tq).ToList
        Next


        'Exact Torque step size for map (based on Parent full-load)
        TqStepMax = FLC_Parent.TqMax/10


        'Variable only needed for function "AddFld()"
        TqStep = TqStepMax


        'Check correct location of every torque value in map
        For Each rl In RPMlists.Values

            'Create list with limited amount of target torque points for specific engine speed
            ' --> only target torque points below full load are relevant!
            ' --> Different handling of points close to full-load (5% rule; combining last point below full-load acc. to par. 4.3.5.2.2 of technical annex)
            Tq = 0
            TqList.Clear()
            Do
                TqList.Add(Tq)
                Tq += TqStepMax
            Loop Until Tq > (FLC_Parent.Tq_orig(rl.TargetRPM) - 0.05 * FLC_Parent.TqMax)

            'Check number of measured map points for specific engine speed
            If rl.MapPoints.Count <> (TqList.Count + 1) Then
                WorkerMsg(tMsgID.Err, "Invalid number of measured torque points in FC map at " & mp0.nU & " [1/min]!")
                WorkerMsg(tMsgID.Err, "	 Expected number = " & (TqList.Count + 1) & ", actual number = " & rl.MapPoints.Count & ".")
                Return False
            End If


            i = -1
            For Each mp0 In rl.MapPoints
                i += 1

                'Different handling of full-load point
                ' Index start from 0, so (i-1) is the last value

                If i < TqList.Count Then

                    'Regular points
                    TqTarget = TqList(i)

                    If Math.Abs(mp0.Tq - TqTarget) > TqStepTol Then
                        WorkerMsg(tMsgID.Err,
                                      "Invalid torque value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
                        WorkerMsg(tMsgID.Err, "	 Expected torque = " & TqTarget & " [Nm].")
                        WorkerMsg(tMsgID.Err, "	 Allowed tolerance is +/- " & TqStepTol & " [Nm].")
                        Return False
                    End If



                Else

                    'Full load point has to be located at least above previously measured point
                    TqTarget = rl.MapPoints(rl.MapPoints.Count - 2).Tq

                    If mp0.Tq <= TqTarget Then
                        WorkerMsg(tMsgID.Err,
                                  "Invalid torque value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
                        WorkerMsg(tMsgID.Err, "	 Expected torque = full load.")
                        WorkerMsg(tMsgID.Err, "	 Full load torque should be >= " & TqTarget & " [Nm].")


                        Return False
                    End If

                End If

            Next

        Next

        'Create list with ALL target torque points for later use in other functions
        Tq = 0
        TqList.Clear()
        Do
            TqList.Add(Tq)
            Tq += TqStepMax
        Loop Until Tq > (FLC_Parent.TqMax + 0.1)


        'Check validity of map for family concept
        '   >>> 54 target points of the parent map must be located below the full-load curve of the engine to be certified
        NumPointsBelowFL = 0
        For Each rl In RPMlists.Values
            For Each mp0 In rl.MapPoints
                If mp0.Tq < FLC.Tq_orig(mp0.nU) Then
                    NumPointsBelowFL += 1
                End If
            Next
        Next
        If NumPointsBelowFL < 54 Then
            WorkerMsg(tMsgID.Err,
                      "FC map not valid for engine to be certified, less than 54 map points below full-load curve (not valid acc. to CO2-family definition).")
            Return False
        End If


        'Copy first rpmlist to idle -100
        rl = New cRPMlist(Map_n_idle - 100)

        For Each mp0 In RPMlists(Map_n_idle).MapPoints
            rl.MapPoints.Add(New cMapPoint() With {
                                .nU = Map_n_idle - 100,
                                .Tq = mp0.Tq,
                                .FC1 = mp0.FC1,
                                .FC2 = mp0.FC2,
                                .WHRMechanical = mp0.WHRMechanical,
                                .WHRElectrical = mp0.WHRElectrical                
                                })
        Next

        RPMlists.Add(Map_n_idle - 100, rl)

        'Copy last rpmlist to last rpm +500
        rl = New cRPMlist(TargetRPMs.Last + 500)

        For Each mp0 In RPMlists(TargetRPMs.Last).MapPoints
            rl.MapPoints.Add(New cMapPoint() With {
                                .nU = TargetRPMs.Last + 500,
                                .Tq = mp0.Tq,
                                .FC1 = mp0.FC1,
                                .FC2 = mp0.FC2,
                                .WHRMechanical = mp0.WHRMechanical,
                                .WHRElectrical = mp0.WHRElectrical                 
                                })
        Next

        RPMlists.Add(TargetRPMs.Last + 500, rl)

        'Not used any more
        MapPoints.Clear()


        Return True
    End Function

    Public Function Triangulate() As Boolean
        Dim rl As cRPMlist
        Dim mp0 As cMapPoint

        'FC Map Triangulate
        For Each rl In RPMlists.Values
            For Each mp0 In rl.MapPoints
                If DualFuel Then
                    FuelMap1.AddPoints(mp0.nU, mp0.Tq, mp0.FC1)
                    FuelMap2.AddPoints(mp0.nU, mp0.Tq, mp0.FC2)
                Else
                    FuelMap1.AddPoints(mp0.nU, mp0.Tq, mp0.FC1)
                End If

                If WHRMechanicalEnabled Then
                    WHRMapMech.AddPoints(mp0.nU, mp0.Tq, mp0.WHRMechanical)
                End If

                If WHRElectricalEnabled Then
                    WHRMapEl.AddPoints(mp0.nU, mp0.Tq, mp0.WHRElectrical)
                End If

            Next
        Next


        If Not FuelMap1.Triangulate() Then
            WorkerMsg(tMsgID.Err, "Failed to triangulate FC map 1!")
            Return False
        End If

        If DualFuel andalso Not FuelMap2.Triangulate() Then
            WorkerMsg(tMsgID.Err, "Failed to triangulate FC map 2!")
            Return False
        End If

        If WHRMechanicalEnabled andalso Not WHRMapMech.Triangulate() Then
            WorkerMsg(tMsgID.Err, "Failed to triangulate WHR map mechanical!")
            Return False
        End If

        If WHRElectricalEnabled andalso Not WHRMapEl.Triangulate() Then
            WorkerMsg(tMsgID.Err, "Failed to triangulate WHR map electrical!")
            Return False
        End If

        Return True
    End Function

    Public Function ExtrapolateMap() As Boolean
        Dim LinReg As cRegression
        Dim LinRegResult As cRegression.RegressionProcessInfo
        Dim lX As New List(Of Double)
        Dim lY As New List(Of Double)

        Dim LinReg2 As cRegression
        Dim LinRegResult2 As cRegression.RegressionProcessInfo
        Dim lX2 As New List(Of Double)
        Dim lY2 As New List(Of Double)

        Dim LinReg3 As cRegression
        Dim LinRegResult3 As cRegression.RegressionProcessInfo
        Dim lX3 As New List(Of Double)
        Dim lY3 As New List(Of Double)

        Dim LinReg4 As cRegression
        Dim LinRegResult4 As cRegression.RegressionProcessInfo
        Dim lX4 As New List(Of Double)
        Dim lY4 As New List(Of Double)

        Dim Tq As Double
        Dim Tq_FullLoad As Double
        Dim i As Integer
        Dim TqDistMax As Double
        Dim fcExtrapolation_res As Double
        Dim fcExtrapolation_res2 As Double
        Dim fcExtrapolation_whrmechanic As Double
        Dim fcExtrapolation_whrelectical As Double
        Dim tempMapPoint As cMapPoint = New cMapPoint()
        Dim MapPointMaxLim As cMapPoint = New cMapPoint()


        For Each rl In RPMlists.Values

            'Maximum torque distance for extrapolation defined as exactly one torque stepwidth for FC map
            TqDistMax = FLC_Parent.TqMax/10

            'Abort if not enough points below CO2-Parent full load
            If rl.MapPoints.Count < 3 Then
                WorkerMsg(tMsgID.Err,
                          "Extrapolation not possible: Less than 3 points in fuel map at " & rl.TargetRPM & " [1/min] !")
                Return False
            End If

            'Linear regression of last three map points
            lX.Clear()
            lY.Clear()
            LinReg = New cRegression

            For i = Math.Max(0, rl.MapPoints.Count - 3) To rl.MapPoints.Count - 1
                lX.Add(rl.MapPoints(i).Tq)
                lY.Add(rl.MapPoints(i).FC1)
            Next
            LinRegResult = LinReg.Regress(lX.ToArray, lY.ToArray)

            If DualFuel Then
                lX2.Clear()
                lY2.Clear()
                LinReg2 = New cRegression

                For i = Math.Max(0, rl.MapPoints.Count - 3) To rl.MapPoints.Count - 1
                    lX2.Add(rl.MapPoints(i).Tq)
                    lY2.Add(rl.MapPoints(i).FC2)
                Next

                LinRegResult2 = LinReg2.Regress(lX2.ToArray, lY2.ToArray)
            End If

            If WHRMechanicalEnabled Then
                lX3.Clear()
                lY3.Clear()
                LinReg3 = New cRegression

                For i = Math.Max(0, rl.MapPoints.Count - 3) To rl.MapPoints.Count - 1
                    lX3.Add(rl.MapPoints(i).Tq)
                    lY3.Add(rl.MapPoints(i).WHRMechanical)
                Next

                LinRegResult3 = LinReg3.Regress(lX3.ToArray, lY3.ToArray)
            End If

            If WHRElectricalEnabled Then
                lX4.Clear()
                lY4.Clear()
                LinReg4 = New cRegression

                For i = Math.Max(0, rl.MapPoints.Count - 3) To rl.MapPoints.Count - 1
                    lX4.Add(rl.MapPoints(i).Tq)
                    lY4.Add(rl.MapPoints(i).WHRElectrical)
                Next

                LinRegResult4 = LinReg4.Regress(lX4.ToArray, lY4.ToArray)
            End If


            'Tq_FullLoad = rl.MapPoints.Last.Tq

            ' store measured point with highest torque                   
            MapPointMaxLim = rl.MapPoints.Last

            For i = 0 To (TqList.Count - 1)
                If TqList(i) > rl.MapPoints.Last.Tq Then
                    tempMapPoint = New cMapPoint()
                    fcExtrapolation_res = Math.Max(LinRegResult.a + LinRegResult.b * TqList(i), MapPointMaxLim.FC1)
                    tempMapPoint.nU = rl.TargetRPM
                    tempMapPoint.Tq = TqList(i)
                    tempMapPoint.FC1 = fcExtrapolation_res
                    'Add extrapolated points above full load with standard stepsize up to (T_max_overall + one grid step)
                    If DualFuel Then

                        fcExtrapolation_res2 = Math.Max(LinRegResult2.a + LinRegResult2.b * TqList(i),
                                                        MapPointMaxLim.FC2)
                        'rl.MapPoints.Add(New cMapPoint(rl.TargetRPM, TqList(i), fcExtrapolation_res, fcExtrapolation_res2))

                        tempMapPoint.FC2 = fcExtrapolation_res2
                    End If

                    If WHRMechanicalEnabled Then
                        fcExtrapolation_whrmechanic = Math.Max(LinRegResult3.a + LinRegResult3.b * TqList(i),
                                                               MapPointMaxLim.WHRMechanical)
                        tempMapPoint.WHRMechanical = fcExtrapolation_whrmechanic
                    End If

                    If WHRElectricalEnabled Then
                        fcExtrapolation_whrelectical = Math.Max(LinRegResult4.a + LinRegResult4.b * TqList(i),
                                                                MapPointMaxLim.WHRElectrical)
                        tempMapPoint.WHRElectrical = fcExtrapolation_whrelectical
                    End If

                    rl.MapPoints.Add(tempMapPoint)
                End If
            Next
            'Loop Until Tq >= FLC_Parent.TqMax + TqDistMax

            'Add last point exactly at (T_max_overall + one grid step)

            Tq = FLC_Parent.TqMax + TqDistMax
            tempMapPoint = New cMapPoint()
            fcExtrapolation_res = Math.Max(LinRegResult.a + LinRegResult.b * Tq, MapPointMaxLim.FC1)

            tempMapPoint.nU = rl.TargetRPM
            tempMapPoint.Tq = Tq
            tempMapPoint.FC1 = fcExtrapolation_res

            If DualFuel Then
                fcExtrapolation_res2 = Math.Max(LinRegResult2.a + LinRegResult2.b * Tq, MapPointMaxLim.FC2)
                tempMapPoint.FC2 = fcExtrapolation_res2
            End If

            If WHRMechanicalEnabled Then
                fcExtrapolation_whrmechanic = Math.Max(LinRegResult3.a + LinRegResult3.b * Tq,
                                                       MapPointMaxLim.WHRMechanical)
                tempMapPoint.WHRMechanical = fcExtrapolation_whrmechanic
            End If

            If WHRElectricalEnabled Then
                fcExtrapolation_whrelectical = Math.Max(LinRegResult4.a + LinRegResult4.b * Tq,
                                                        MapPointMaxLim.WHRElectrical)
                tempMapPoint.WHRElectrical = fcExtrapolation_whrelectical
            End If

            rl.MapPoints.Add(tempMapPoint)

        Next

        Return True
    End Function

    Public Function LimitFlcParentToMap() As Boolean
        Dim i As Integer
        Dim nU As Double
        Dim Tq As Double
        Dim TqMaxMap As Double

        For i = 0 To FLC_Parent.iDim
            nU = FLC_Parent.LnU(i)
            Tq = FLC_Parent.LTq(i)
            TqMaxMap = TqMax(nU)
            If Tq > TqMaxMap Then
                FLC_Parent.LTq(i) = TqMaxMap
                WorkerMsg(tMsgID.Warn,
                          "   CO2-Parent Full Load torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) &
                          " [1/min] by " &
                          Math.Round(Tq - TqMaxMap, 2) & "[Nm] !")
                WorkerMsg(tMsgID.Warn,
                          "     >>> Maximum torque at " & Math.Round(nU, 2) & " [1/min] will be limited to " &
                          Math.Round(TqMaxMap, 2) & "[Nm] for WHTC simulation!")
            End If
        Next

        Return True
    End Function

    Public Function LimitDragtoMap() As Boolean
        Dim i As Integer
        Dim nU As Double
        Dim Tq As Double
        Dim TqMinMap As Double

        For i = 0 To Motoring.iDim
            nU = Motoring.LnU(i)
            Tq = Motoring.LTq(i)
            TqMinMap = TqMin(nU)
            'If Tq < TqMinMap Then
            '    Motoring.LTq(i) = TqMinMap
            'End If

            ' set all motoring torque values within fuel map speed range to interpolated torque values from map
            If nU > (Map_n_idle - 100) And nU < (Map_n_95h + 500) Then
                Motoring.LTq(i) = TqMinMap
            End If
        Next

        Return True
    End Function

    Public Function AddDrag() As Boolean
        Dim rl As cRPMlist
        'Dim lowestRpm As cRPMlist = RPMlists(RPMlists.Keys.First)
        'Dim highestRpm As cRPMlist = RPMlists(RPMlists.Keys.Last)
        Dim lTargetRpms As List(Of Double) = RPMlists.Keys.ToList()
        Dim i As Integer
        Dim CountNumVal As Integer
        Dim Tq_forLoop As Double
        Dim dnUleft_forLoop As Double
        Dim dnUright_forLoop As Double
        Dim IndexTargetRpms As Integer


        'First run: only fill regular map points (except first and last)
        'start with first entry is necessary (is anyway ignored by if statements below) in order to match "rl" with "IndexTargetRpms"
        IndexTargetRpms = 0

        For Each rl In RPMlists.Values

            If IndexTargetRpms > 1 AndAlso IndexTargetRpms < (lTargetRpms.Count - 2) Then

                dnUleft_forLoop = lTargetRpms(IndexTargetRpms) - lTargetRpms(IndexTargetRpms - 1)
                dnUright_forLoop = lTargetRpms(IndexTargetRpms + 1) - lTargetRpms(IndexTargetRpms)
                Tq_forLoop = 0
                CountNumVal = 0

                For i = 0 To Motoring.iDim
                    If _
                        Motoring.LnU(i) >= (lTargetRpms(IndexTargetRpms) - dnUleft_forLoop/2) AndAlso
                        Motoring.LnU(i) < (lTargetRpms(IndexTargetRpms) + dnUright_forLoop/2) Then
                        Tq_forLoop += Motoring.LTq(i)
                        CountNumVal += 1
                    ElseIf Motoring.LnU(i) > (lTargetRpms(IndexTargetRpms) + dnUright_forLoop/2) Then
                        Exit For
                    End If
                Next

                'Add motoring curve with zero FC
                rl.MapPoints.Insert(0, New cMapPoint() With {.nU = rl.TargetRPM, .Tq =  Tq_forLoop/CountNumVal,.FC1 = 0})
                'add additional point at (motoring torque -100Nm) with zero FC
                'rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, Motoring.Tq(rl.TargetRPM) - 100, 0))
                rl.MapPoints.Insert(0,
                                    New cMapPoint() _
                                       With {.nU = rl.TargetRPM, .Tq = (Tq_forLoop/CountNumVal) - 100, .FC1 = 0})

            ElseIf IndexTargetRpms = 1 Then
                'idle speed point (only consider values >= idle speed)

                dnUleft_forLoop = 0
                dnUright_forLoop = lTargetRpms(IndexTargetRpms + 1) - lTargetRpms(IndexTargetRpms)
                Tq_forLoop = 0
                CountNumVal = 0

                For i = 0 To Motoring.iDim
                    If _
                        Motoring.LnU(i) >= (lTargetRpms(IndexTargetRpms) - dnUleft_forLoop/2) AndAlso
                        Motoring.LnU(i) < (lTargetRpms(IndexTargetRpms) + dnUright_forLoop/2) Then
                        Tq_forLoop += Motoring.LTq(i)
                        CountNumVal += 1
                    ElseIf Motoring.LnU(i) > (lTargetRpms(IndexTargetRpms) + dnUright_forLoop/2) Then
                        Exit For
                    End If
                Next

                'Add motoring curve with zero FC
                rl.MapPoints.Insert(0,
                                    New cMapPoint() With {.nU =  rl.TargetRPM, .Tq = Tq_forLoop/CountNumVal, .FC1 = 0})
                'add additional point at (motoring torque -100Nm) with zero FC
                rl.MapPoints.Insert(0,
                                    New cMapPoint() _
                                       With {.nU = rl.TargetRPM, .Tq = (Tq_forLoop/CountNumVal) - 100, .FC1 = 0})


            ElseIf IndexTargetRpms = lTargetRpms.Count - 2 Then
                'n_95h speed point (only consider values <= n_95h speed)

                dnUleft_forLoop = lTargetRpms(IndexTargetRpms) - lTargetRpms(IndexTargetRpms - 1)
                dnUright_forLoop = 0
                Tq_forLoop = 0
                CountNumVal = 0

                For i = 0 To Motoring.iDim
                    If _
                        Motoring.LnU(i) >= (lTargetRpms(IndexTargetRpms) - dnUleft_forLoop/2) AndAlso
                        Motoring.LnU(i) <= (lTargetRpms(IndexTargetRpms) + dnUright_forLoop/2) Then
                        Tq_forLoop += Motoring.LTq(i)
                        CountNumVal += 1
                    ElseIf Motoring.LnU(i) > (lTargetRpms(IndexTargetRpms) + dnUright_forLoop/2) Then
                        Exit For
                    End If
                Next

                'Add motoring curve with zero FC
                rl.MapPoints.Insert(0,
                                    New cMapPoint() With { .nU = rl.TargetRPM, .Tq = Tq_forLoop/CountNumVal, .FC1 = 0})
                'add additional point at (motoring torque -100Nm) with zero FC
                rl.MapPoints.Insert(0,
                                    New cMapPoint() _
                                       With { .nU = rl.TargetRPM, .Tq = (Tq_forLoop/CountNumVal) - 100, .FC1 = 0})

            End If

            IndexTargetRpms += 1
        Next


        'Second run to add first (100rpm below idle point) and last (500rpm above n_95h point) value by copying of nearest neighbours

        'Special for first entry (100rpm below idle point)
        'use same motoring torque as for idle speed entry
        'Add motoring curve with zero FC
        rl = RPMlists(lTargetRpms(0))
        rl.MapPoints.Insert(0,
                            New cMapPoint() _
                               With {.nU = rl.TargetRPM, .Tq = RPMlists(lTargetRpms(1)).MapPoints(1).Tq, .FC1 = 0})
        rl.MapPoints.Insert(0,
                            New cMapPoint() _
                               With {.nU = rl.TargetRPM, .Tq = RPMlists(lTargetRpms(1)).MapPoints.First.Tq, .FC1 = 0})


        'Special for last entry (500rpm above n_95h point)
        'use same motoring torque as for n_95h speed entry
        'Add motoring curve with zero FC
        rl = RPMlists(lTargetRpms(lTargetRpms.Count - 1))
        rl.MapPoints.Insert(0,
                            New cMapPoint() _
                               With { .nU = rl.TargetRPM,
                               .Tq = RPMlists(lTargetRpms(lTargetRpms.Count - 2)).MapPoints(1).Tq, .FC1 = 0})
        rl.MapPoints.Insert(0,
                            New cMapPoint() _
                               With { .nU = rl.TargetRPM,
                               .Tq = RPMlists(lTargetRpms(lTargetRpms.Count - 2)).MapPoints.First.Tq, .FC1 = 0})


        Return True
    End Function


    Public Function WriteMap(path As String) As Boolean
        Dim file As New cFile_V3
        Dim mp As cMapPoint
        Dim rl As cRPMlist
        Dim FC_corr As Double

        If Not file.OpenWrite(path) Then
            WorkerMsg(tMsgID.Err, "Failed to write file (" & path & ") !")
            Return False
        End If

        file.WriteLine("engine speed [rpm], torque [Nm], fuel consumption [g/h]")

        For Each rl In RPMlists.Values
            For Each mp In rl.MapPoints

                'correction of measured FC to standard NCV (not performed for Diesel reference fuel B7 coded "Diesel / CI")
                If temp_Map_FuelType = "Diesel / CI" Then
                    FC_corr = mp.FC1
                Else
                    FC_corr = mp.FC1*NCV_CorrectionFactor
                End If

                'writing of output
                file.WriteLine(Math.Round(mp.nU, 2), Math.Round(mp.Tq, 2), Math.Round(FC_corr, 2))
            Next
        Next


        file.Close()

        Return True
    End Function

    Public Function WriteFLD(path As String, AveragingOver8rpm As Boolean) As Boolean
        Dim file As New cFile_V3
        Dim nU As Double
        Dim Tq As Double
        Dim DragTq As Double
        Dim i As Integer

        ' Write values in file with 8 rpm steps

        If AveragingOver8rpm Then
            'Calc 8 rpm steps
            CalcFlcOut()


            If Not file.OpenWrite(path) Then
                WorkerMsg(tMsgID.Err, "Failed to write file (" & path & ") !")
                Return False
            End If

            'Header
            file.WriteLine("engine speed [1/min], full load torque [Nm], motoring torque [Nm]")

            'Values
            For i = 0 To (LnU_out.Count - 1)

                nU = LnU_out(i)
                Tq = LTq_out(i)
                DragTq = LTqMot_out(i)

                file.WriteLine(Math.Round(nU, 2), Math.Round(Tq, 2), Math.Round(DragTq, 2))
            Next

            file.Close()

            Return True

        Else

            If Not file.OpenWrite(path) Then
                WorkerMsg(tMsgID.Err, "Failed to write file (" & path & ") !")
                Return False
            End If

            'Header
            file.WriteLine("engine speed [1/min], full load torque [Nm], motoring torque [Nm]")

            'Values
            For i = 0 To (FLC_Parent.LnU.Count - 1)

                nU = FLC_Parent.LnU(i)
                Tq = FLC_Parent.LTq(i)
                DragTq = Motoring.Tq(nU)

                file.WriteLine(Math.Round(nU, 2), Math.Round(Tq, 2), Math.Round(DragTq, 2))
            Next

            file.Close()

            Return True

        End If
    End Function

    public Function WriteXmlComponentFileV10(filename As String, job As cJob) As Boolean

#If RELEASE_CANDIDATE
        Dim schemaLocationBaseUrl = "https://webgate.ec.europa.eu/CITnet/svn/VECTO/trunk/Share/XML/XSD/DEV/"
#Else 
        Dim schemaLocationBaseUrl = $"urn:tugraz:ivt:VectoAPI:DeclarationComponent v:\VectoCore\VectoCore\Resources\XSD/VectoDeclarationComponent.xsd"
#End If
        Dim SchemaVersion = "1.0"
        Dim report = New XDocument()
        report.Add(New XElement(tnsV10 + XMLNames.VectoInputDeclaration,
                                New XAttribute("xmlns", v10),
                                New XAttribute(XNamespace.Xmlns + "tns", tnsV10),
                                New XAttribute(XNamespace.Xmlns + "di", di),
                                New XAttribute("schemaVersion", schemaVersion),
                                New XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                                New XAttribute(xsi + "schemaLocation", schemaLocationBaseUrl),
                                GetEngineElementV10(job)
                                )
                   )

        
        Try
            Dim stream = New MemoryStream()
            Dim writer = New StreamWriter(stream)
            writer.Write(report)
            writer.Flush()
            stream.Seek(0, SeekOrigin.Begin)
            Dim h As VectoHash = VectoHash.Load(stream)
            Dim finalReport = h.AddHash()

            finalReport.Save(filename)

            Dim xmlValidator =
                    New XMLValidator(XmlReader.Create(filename), Nothing, AddressOf ValidationCallBack).ValidateXML(
                        TUGraz.VectoCore.Utils.XmlDocumentType.DeclarationComponentData)


        Catch e As Exception
            WorkerMsg(tMsgID.Err, "Generated XML is not valid - no output!")
            WorkerMsg(tMsgID.Err, "     >>> " & e.Message)
            'MsgBox("Failed to generate XML: " + e.Message)
            Return False
        End Try
        Return True



        

    End Function



    Public Function WriteXmlComponentFileV23(filename As String, job As cJob) As Boolean

#If RELEASE_CANDIDATE
        Dim schemaLocationBaseUrl = "https://webgate.ec.europa.eu/CITnet/svn/VECTO/trunk/Share/XML/XSD/DEV/"
#Else 
        Dim schemaLocationBaseUrl = $"urn:tugraz:ivt:VectoAPI:DeclarationComponent V:\VectoCore\VectoCore\Resources\XSD/VectoDeclarationComponent.xsd"
#End If
        Dim schemaVersion = "2.0"
        Dim report = New XDocument()
        report.Add(New XElement(tnsV20 + XMLNames.VectoInputDeclaration,
                                New XAttribute("xmlns", tnsV20),
                                New XAttribute(XNamespace.Xmlns + "tns", tnsV20),
                                New XAttribute(XNamespace.Xmlns + "v2.0", v20),
                                New XAttribute("schemaVersion", schemaVersion),
                                New XAttribute(XNamespace.Xmlns + "v2.3", v23),
                                New XAttribute(XNamespace.Xmlns + "di", di),
                                New XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                                New XAttribute(xsi + "schemaLocation", schemaLocationBaseUrl),
                                GetEngineElementV23(job)
                                )
                   )
        Try
            Dim stream = New MemoryStream()
            Dim writer = New StreamWriter(stream)
            writer.Write(report)
            writer.Flush()
            stream.Seek(0, SeekOrigin.Begin)
            Dim h As VectoHash = VectoHash.Load(stream)
            Dim finalReport = h.AddHash()

            finalReport.Save(filename)

            Dim xmlValidator =
                    New XMLValidator(XmlReader.Create(filename), Nothing, AddressOf ValidationCallBack).ValidateXML(
                        TUGraz.VectoCore.Utils.XmlDocumentType.DeclarationComponentData)


        Catch e As Exception
            WorkerMsg(tMsgID.Err, "Generated XML is not valid - no output!")
            WorkerMsg(tMsgID.Err, "     >>> " & e.Message)
            'MsgBox("Failed to generate XML: " + e.Message)
            Return False
        End Try
        Return True
    End Function

    Private Function GetEngineElementV10(job As cJob) As XElement

#If RELEASE_CANDIDATE 
        Dim version As String = VectoEngineVersion.FullVersion + " - RELEASE CANDIDATE, NOT FOR CERTIFICATION!"
#else
        Dim version As String = FullVersion
#End If
        Dim fuelType = job.FuelType
        Dim whtcUrban = job.WHTCurbanFactor.ToString("f4")
        Dim whtcRural = job.WHTCruralFactor.ToString("f4")
        Dim whtcMotorway = job.WHTCmotorwayFactor.ToString("f4")
        Dim bfColdHot = job.ColdHotBalancingFactor.ToString("f4")
        Dim cfRegPer = job.CF_RegPer.ToString("f4")
        Dim cfncv = NCV_CorrectionFactor.ToString("f4")
        
        Return New XElement(tnsV10 + XMLNames.Engine,
                            New XElement(v10 + "Data",
                                         New XAttribute(xsi + "type", $"v1.0:EngineDataDeclarationType"),
                                         (from engineCommonField In GetEngineCommonFields(v10, job)
                                            select engineCommonField),
                                         New XElement(v10 + XMLNames.Engine_Displacement, job.Displacement),
                                         New XElement(v10 + XMLNames.Engine_IdlingSpeed, job.Idle),
                                         New XElement(v10 + XMLNames.Engine_RatedSpeed, job.RatedSpeed),
                                         New XElement(v10 + XMLNames.Engine_RatedPower, job.RatedPower*1000),
                                         New XElement(v10 + XMLNames.Engine_MaxEngineTorque, FLC.TqMax.ToString("F0")),
                                         (from fuelTypeCommonField In GetFuelTypeCommonElements(v10, whtcUrban, whtcRural, 
                                                                                                whtcMotorway, bfColdHot, cfRegPer, cfncv)
                                            Select fuelTypeCommonField),
                                         New XElement(v10 + XMLNames.Engine_Fuel_Type_Attr, GetXMLFuelTypeString(fuelType)),
                                         GetFuelConsumptionMapElement(v10, job, fuelType, true),
                                         GetFullLoadAndDragCurveElement(v10)
                                         )
                            )
    End Function



    Private Function GetEngineElementV23(job As cJob) As XElement

#If RELEASE_CANDIDATE 
        Dim version As String = VectoEngineVersion.FullVersion + " - RELEASE CANDIDATE, NOT FOR CERTIFICATION!"
#else
        Dim version As String = FullVersion
#End If
        
        Return New XElement(tnsV20 + XMLNames.Engine,
                             New XAttribute(xsi + "type", $"v2.0:EngineComponentDeclarationType"),
                             New XElement(v20 + "Data",
                                  New XAttribute(xsi + "type", $"v2.3:EngineDataDeclarationType"),
                                  New XAttribute("xmlns", v23.NamespaceName),
                                  (from engineCommonField In GetEngineCommonFields(v23, job)
                                     select engineCommonField),
                                  New XElement(v20 + XMLNames.Engine_Displacement, job.Displacement),
                                  New XElement(v20 + XMLNames.Engine_RatedSpeed, job.RatedSpeed),
                                  New XElement(v20 + XMLNames.Engine_RatedPower, job.RatedPower*1000),
                                  New XElement(v20 + XMLNames.Engine_MaxEngineTorque, FLC.TqMax.ToString("F0")), 
                                  GetWHRTypeElement(v23, job),
                                  GetModeElements(v23, job)
                                  )
                             )

    End Function
    
    Private Function GetEngineCommonFields(xmlNamespace As XNamespace, job As cJob) As List(of XElement)
        Dim commonFields = new List(Of XElement)

#If RELEASE_CANDIDATE 
        Dim version As String = VectoEngineVersion.FullVersion + " - RELEASE CANDIDATE, NOT FOR CERTIFICATION!"
#else
        Dim version As String = FullVersion
#End If
        commonFields.Add(New XElement(xmlNamespace + XMLNames.Engine_Manufacturer, job.Manufacturer))
        commonFields.Add(New XElement(xmlNamespace + XMLNames.Engine_Model, job.Model))
        commonFields.Add(New XElement(xmlNamespace + XMLNames.Engine_CertificationNumber, job.CertNumber))
        commonFields.Add(New XElement(xmlNamespace + XMLNames.Engine_Date, XmlConvert.ToString(Now, XmlDateTimeSerializationMode.Utc)))
        commonFields.Add(New XElement(xmlNamespace + XMLNames.Engine_AppVersion, version))


        Return commonFields

    End Function

    Private Function GetWHRTypeElement(xmlNamespace As XNamespace, job As cJob) As XElement
        
        Return new XElement(xmlNamespace + XMLNames.Engine_WHRType,
                            new XElement(xmlNamespace + XMLNames.Engine_WHRType_MechanicalOutputICE, job.WHR_ICE),
                            new XElement(xmlNamespace + XMLNames.Engine_WHRType_MechanicalOutputDrivetrain, job.WHR_Mech),
                            new XElement(xmlNamespace + XMLNames.Engine_WHRType_ElectricalOutput, job.WHR_El))
    End Function

    Private Function GetModeElements(xmlNamespace As XNamespace, job As cJob) As XElement

        Dim modes = New XElement(xmlNamespace + XMLNames.Engine_Mode, 
                                New XElement(xmlNamespace + XMLNames.Engine_IdlingSpeed, job.Idle), 
                                GetFullLoadAndDragCurveElement(xmlNamespace), 
                                GetFuelTypeElement(xmlNamespace, job, true))
        If job.DualFuel Then
            modes.Add(GetFuelTypeElement(xmlNamespace, job, false))
        End If

        Return modes

    End Function

    Private Function GetFullLoadAndDragCurveElement(xmlNamespace As XNamespace) As XElement
        Dim fldCurve = New XElement(xmlNamespace + XMLNames.Engine_FullLoadAndDragCurve)
        
        ' Write values in file with 8 rpm steps
        'Calc 8 rpm steps
        CalcFlcOut()
        'Values
        Dim i As Integer
        For i = 0 To (LnU_out.Count - 1)

            fldCurve.Add(New XElement(xmlNamespace + XMLNames.Engine_FullLoadAndDragCurve_Entry,
                                      New XAttribute(XMLNames.Engine_FullLoadAndDragCurve_EngineSpeed_Attr, LnU_out(i).ToString("f2")),
                                      New XAttribute(XMLNames.Engine_FullLoadAndDragCurve_MaxTorque_Attr, LTq_out(i).ToString("f2")),
                                      New XAttribute(XMLNames.Engine_FullLoadAndDragCurve_DragTorque_Attr, LTqMot_out(i).ToString("f2"))))
        Next

        Return fldCurve

    End Function
    
    Private Function GetFuelTypeElement(xmlNamespace As XNamespace, job As cJob, mainFuel As Boolean) As XElement

        Dim whrCorrectionFactors = If(mainFuel, GetWHRCorrectionFactorElements(xmlNamespace, job), nothing) 
        Dim fuelType = job.FuelType
        Dim whtcUrban = job.WHTCurbanFactor.ToString("f4")
        Dim whtcRural = job.WHTCruralFactor.ToString("f4")
        Dim whtcMotorway = job.WHTCmotorwayFactor.ToString("f4")
        Dim bfColdHot = job.ColdHotBalancingFactor.ToString("f4")
        Dim cfRegPer = job.CF_RegPer.ToString("f4")
        Dim cfncv = NCV_CorrectionFactor.ToString("f4")
        
        If(mainFuel = False)
            fuelType = job.FuelType2
            whtcUrban = job.WHTCurbanFactor2.ToString("f4")
            whtcRural = job.WHTCruralFactor2.ToString("f4")
            whtcMotorway = job.WHTCmotorwayFactor2.ToString("f4")
            bfColdHot = job.ColdHotBalancingFactor2.ToString("f4")
            cfRegPer = job.CF_RegPer2.ToString("f4")
            cfncv= NCV_CorrectionFactor2.ToString("f4")
        End If
        
        Return New XElement(xmlNamespace + XMLNames.Engine_Fuel, 
                            New XAttribute(XMLNames.Engine_Fuel_Type_Attr, GetXMLFuelTypeString(fuelType)),
                            (From commonFuelType In GetFuelTypeCommonElements(xmlNamespace, whtcUrban, whtcRural, 
                                                                              whtcMotorway, bfColdHot, cfRegPer, cfncv)
                                Select commonFuelType),
                            If(whrcorrectionfactors?.HasElements, whrcorrectionFactors, Nothing),
                            GetFuelConsumptionMapElement(xmlNamespace, job, fuelType, mainFuel))

    End Function

    Private Function GetFuelTypeCommonElements(xmlNamespace As XNamespace,
                                               whtcUrban As string, whtcRural As String, 
                                               whtcMotorway As String,  bfColdHot As String, 
                                               cfRegPer As String, cfncv As String) As List(Of XElement)
    
        Dim commonFuelTypeElements = New List(Of XElement)

        commonFuelTypeElements.Add(New XElement(xmlNamespace + XMLNames.Engine_Fuel_WHTCUrban, whtcUrban))
        commonFuelTypeElements.Add(New XElement(xmlNamespace + XMLNames.Engine_Fuel_WHTCRural, whtcRural))
        commonFuelTypeElements.Add(New XElement(xmlNamespace + XMLNames.Engine_Fuel_WHTCMotorway, whtcMotorway))
        commonFuelTypeElements.Add(New XElement(xmlNamespace + XMLNames.Engine_Fuel_BFColdHot, bfColdHot))
        commonFuelTypeElements.Add(New XElement(xmlNamespace + XMLNames.Engine_Fuel_CFRegPer, cfRegPer))
        commonFuelTypeElements.Add(New XElement(xmlNamespace + XMLNames.Engine_Fuel_CFNCV, cfncv))

        Return commonFuelTypeElements
    End Function
    Private Function GetWHRCorrectionFactorElements(xmlNamespace As XNamespace, job As cJob) As XElement

        Dim whrCorrectionFactors = new XElement(xmlNamespace + "WHRCorrectionFactors")

        If (job.WHR_El)
            whrCorrectionFactors.Add(New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_Electrical,
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_Urban, 
                                                               job.WHTCurbanFactorWHREl.ToString("f4")),
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_Rural, 
                                                               job.WHTCruralFactorWHREl.ToString("f4")),
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_Motorway,
                                                               job.WHTCmotorwayFactorWHREl.ToString("f4")),
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_BFColdHot,
                                                               job.ColdHotBalancingFactorWHREl.ToString("f4")),
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_CFRegPer,
                                                               job.CF_RegPerWHREl.ToString("f4"))))
        End If

        If (job.WHR_Mech)
            whrCorrectionFactors.Add(new XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_Mechanical,
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_Urban,
                                                               job.WHTCurbanFactorWHRMech.ToString("f4")),
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_Rural,
                                                               job.WHTCruralFactorWHRMech.ToString("f4")),
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_Motorway,
                                                               job.WHTCmotorwayFactorWHRMech.ToString("f4")),
                                                  New XElement(xmlNamespace + XMLNames.Engine_WHRCorrectionFactors_BFColdHot,
                                                               job.ColdHotBalancingFactorWHRMech.ToString("f4")),
                                                  New XElement(xmlNamespace + XMLNames.Engine_CFRegPer, job.CF_RegPerWHRMech.ToString("f4"))))
        End If
        
        Return whrCorrectionFactors

    End Function

    Private Function GetFuelConsumptionMapElement(xmlNamespace As XNamespace, job As cJob, fuelType As String, mainFuelType As Boolean) As XElement 

        Dim fcMap = New XElement(xmlNamespace +  XMLNames.Engine_FuelConsumptionMap)

        For Each rpmValues In RPMlists.Values
            For Each mp In rpmValues.MapPoints
                Dim fcCorr As Double

                If fuelType = "Diesel / CI" Then
                    If(mainFuelType)
                        fcCorr = mp.FC1
                    Else 
                        fcCorr = mp.FC2
                    End If
                Else
                    If(mainFuelType)
                        fcCorr = mp.FC1* NCV_CorrectionFactor
                    Else 
                        fcCorr = mp.FC2* NCV_CorrectionFactor
                    End If
                End If

                fcMap.Add(New XElement(xmlNamespace + XMLNames.Engine_FuelConsumptionMap_Entry,
                                       New XAttribute(XMLNames.Engine_FuelConsumptionMap_EngineSpeed_Attr, mp.nU.ToString("f2")),
                                       New XAttribute(XMLNames.Engine_FuelConsumptionMap_Torque_Attr, mp.Tq.ToString("f2")),
                                       New XAttribute(XMLNames.Engine_FuelConsumptionMap_FuelConsumption_Attr, fcCorr.ToString("f2")),
                                        If(mainFuelType,
                                            if _
                                              (job.whr_El, New XAttribute(XMLNames.Engine_FuelConsumptionMap_ElectricPower_Attr,
                                                                          mp.WHRElectrical.ToString("f2")), Nothing),Nothing),
                                        If(mainFuelType,
                                            if _
                                              (job.WHR_Mech,New XAttribute(XMLNames.Engine_FuelConsumptionMap_MechanicalPower_Attr,
                                                                           mp.WHRMechanical.ToString("f2")), Nothing),Nothing)))
            Next
        Next

        Return fcMap

    End Function

    Private Shared Sub ValidationCallBack(ByVal severity As XmlSeverityType, ByVal evt As ValidationEvent)
        Dim args = evt.ValidationEventArgs

        If severity = XmlSeverityType.[Error] Then
            Throw _
                New Exception(
                    String.Format("Validation error: {0}" & Environment.NewLine & "Line: {1}", args.Message,
                                  args.Exception.LineNumber), evt.Exception)
        Else
            Console.[Error].WriteLine("Validation warning: {0}" & Environment.NewLine & "Line: {1}", args.Message,
                                      args.Exception.LineNumber)
        End If
    End Sub

   
    Private Function GetXMLFuelTypeString(fuelType As String) As String
        ' input: "Diesel / CI", "Ethanol / CI", "Petrol / PI", "Ethanol / PI", "LPG / PI", "Natural Gas / PI", "Natural Gas / CI"
        ' output:  "Diesel CI", "Ethanol CI", "Petrol PI", "Ethanol PI", "LPG PI", "NG PI", "NG CI"
        Select Case (fuelType)
            Case "Natural Gas / PI"
                Return "NG PI"
            Case "Natural Gas / CI"
                Return "NG CI"
            Case Else
                Return fuelType.Replace("/ ", "")
        End Select
    End Function

    Public Function fFCdelaunay_Intp(nU As Double, Tq As Double, MapID As Integer) As Double
        Dim val As Double

        Select Case MapID
            Case 1
                val = FuelMap1.Intpol(nU, Tq)

                If FuelMap1.ExtrapolError Then
                    WorkerMsg(tMsgID.Err,
                              "Cannot extrapolate FC map 1! n= " & nU.ToString("0.00") & " [1/min], Tq= " &
                              Tq.ToString("0.00") & " [Nm]")
                    Return - 10000
                Else
                    Return val
                End If

            Case 2
                val = FuelMap2.Intpol(nU, Tq)

                If FuelMap2.ExtrapolError Then
                    WorkerMsg(tMsgID.Err,
                              "Cannot extrapolate FC map 2! n= " & nU.ToString("0.00") & " [1/min], Tq= " &
                              Tq.ToString("0.00") & " [Nm]")
                    Return - 10000
                Else
                    Return val
                End If
            Case 3
                val = WHRMapMech.Intpol(nU, Tq)

                If WHRMapMech.ExtrapolError Then
                    WorkerMsg(tMsgID.Err,
                              "Cannot extrapolate WHR map mechanical! n= " & nU.ToString("0.00") & " [1/min], Tq= " &
                              Tq.ToString("0.00") & " [Nm]")
                    Return - 10000
                Else
                    Return val
                End If
            Case 4
                val = WHRMapEl.Intpol(nU, Tq)

                If WHRMapEl.ExtrapolError Then
                    WorkerMsg(tMsgID.Err,
                              "Cannot extrapolate WHR map electrical! n= " & nU.ToString("0.00") & " [1/min], Tq= " &
                              Tq.ToString("0.00") & " [Nm]")
                    Return - 10000
                Else
                    Return val
                End If

        End Select
    End Function

    Public Function TqMax(nU As Double) As Double
        Dim rpm As Double
        Dim rpm0 As Double

        rpm = RPMlists.Keys(1)
        rpm0 = RPMlists.Keys(0)

        'Extrapolation for x < x(1)
        If RPMlists.First.Value.TargetRPM >= nU Then
            GoTo lbInt
        End If

        For Each rl As KeyValuePair(Of Double, cRPMlist) In RPMlists
            rpm0 = rpm
            rpm = rl.Key
            If rpm >= nU Then Exit For
        Next


        lbInt:
        'Interpolation
        Return _
            (nU - rpm0)*(RPMlists(rpm).MapPoints.Last.Tq - RPMlists(rpm0).MapPoints.Last.Tq)/(rpm - rpm0) +
            RPMlists(rpm0).MapPoints.Last.Tq
    End Function

    Public Function TqMin(nU As Double) As Double
        Dim rpm As Double
        Dim rpm0 As Double

        rpm = RPMlists.Keys(1)
        rpm0 = RPMlists.Keys(0)

        'Extrapolation for x < x(1)
        If RPMlists.First.Value.TargetRPM >= nU Then
            GoTo lbInt
        End If

        For Each rl As KeyValuePair(Of Double, cRPMlist) In RPMlists
            rpm0 = rpm
            rpm = rl.Key
            If rpm >= nU Then Exit For
        Next


        lbInt:
        'Interpolation
        '  !!!!!  ATTENTION: use second point from motoring curve, since first point is located 100 Nm below real motoring curve
        Return _
            (nU - rpm0)*(RPMlists(rpm).MapPoints(1).Tq - RPMlists(rpm0).MapPoints(1).Tq)/(rpm - rpm0) +
            RPMlists(rpm0).MapPoints(1).Tq

        ' OLD VERSION, using first point which is wrong
        '(nU - rpm0) * (RPMlists(rpm).MapPoints.First.Tq - RPMlists(rpm0).MapPoints.First.Tq) / (rpm - rpm0) +
        'RPMlists(rpm0).MapPoints.First.Tq()
    End Function

    Private Sub CalcFlcOut()
        Dim nU As Double
        Dim Tq As Double
        Dim i As Integer
        Dim TqMaxMap As Double
        Dim nEng_forLoop As Double
        Dim CountNumVal As Integer
        Dim nURange_forLoop As Double


        LnU_outTemp.Clear()
        LTq_outTemp.Clear()


        If FLC.delta_nU < 6 Then

            ' ########################################################################
            ' Start: Transform full load curve (averaging over +/-4rpm in 8 rpm steps)
            ' ########################################################################


            'keep first entry (might be exactly idle speed)
            LnU_outTemp.Add(FLC.LnU(0) - 0.01)
            LTq_outTemp.Add(FLC.LTq(0))


            nURange_forLoop = 0.5*8
            nEng_forLoop = FLC.LnU(0)

            'loop starting from (lowest speed + nURange_forLoop) in dataset
            Do While (nEng_forLoop + nURange_forLoop) < FLC.LnU(FLC.iDim)

                nURange_forLoop = 0.5*8

                Do
                    CountNumVal = 0
                    nU = 0
                    Tq = 0

                    For i = 0 To FLC.iDim
                        If _
                            (FLC.LnU(i) > (nEng_forLoop - nURange_forLoop)) And
                            (FLC.LnU(i) <= (nEng_forLoop + nURange_forLoop)) Then
                            nU += FLC.LnU(i)
                            Tq += FLC.LTq(i)
                            CountNumVal += 1
                        End If
                    Next

                    nURange_forLoop = nURange_forLoop*1.05
                Loop Until CountNumVal > 0

                LnU_outTemp.Add(nU/CountNumVal)
                LTq_outTemp.Add(Tq/CountNumVal)

                nEng_forLoop += 8
            Loop


            'last entry (might be shorter interval)
            nURange_forLoop = 0.5*8

            Do
                CountNumVal = 0
                nU = 0
                Tq = 0

                For i = 0 To FLC.iDim
                    If FLC.LnU(i) >= (nEng_forLoop - nURange_forLoop) Then
                        nU += FLC.LnU(i)
                        Tq += FLC.LTq(i)
                        CountNumVal += 1
                    End If
                Next

                nURange_forLoop = nURange_forLoop*1.05
            Loop Until CountNumVal > 0

            LnU_outTemp.Add(nU/CountNumVal)
            LTq_outTemp.Add(Tq/CountNumVal)
            'END last entry (might be shorter interval)

            'keep last entry (might be exactly n_95h speed)
            LnU_outTemp.Add(FLC.LnU(FLC.iDim) + 0.01)
            LTq_outTemp.Add(FLC.LTq(FLC.iDim))

            ' ######################################################################
            ' End: Transform full load curve (averaging over +/-4rpm in 8 rpm steps)
            ' ######################################################################


            LnU_out.Clear()
            LTq_out.Clear()
            LTqMot_out.Clear()

            'nEng_forLoop = LnU_outTemp(0)
            nEng_forLoop = FLC.n_idle - 8

            Do Until nEng_forLoop > LnU_outTemp(LnU_outTemp.Count - 1)
                'torque interpolated
                Tq = TqMax_FLC_cMAP(nEng_forLoop)
                TqMaxMap = TqMax(nEng_forLoop)

                'Check if full load torque is above max torque from map
                If Tq > TqMaxMap Then
                    WorkerMsg(tMsgID.Warn,
                              "   Actual engine Full Load torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) &
                              " [1/min] by " & Math.Round(Tq - TqMaxMap, 2) & " [Nm] !")
                    WorkerMsg(tMsgID.Warn,
                              "     >>> Maximum torque at " & Math.Round(nU, 2) & " [1/min] will be limited to " &
                              Math.Round(TqMaxMap, 2) & " [Nm] in full load output file!")
                    Tq = TqMaxMap
                End If

                LnU_out.Add(nEng_forLoop)
                LTq_out.Add(Tq)
                LTqMot_out.Add(Motoring.Tq(nEng_forLoop))

                nEng_forLoop += 8
            Loop

            If LnU_out(LnU_out.Count - 1) < FLC.n_hi Then
                'add last value (exact value might be necessary for calculation of n_hi in VECTO)
                LnU_out.Add(FLC.n_hi)
                LTq_out.Add(FLC.Tq(FLC.n_hi))
                LTqMot_out.Add(Motoring.Tq(FLC.n_hi))
            End If

        Else

            'delta_nU >= 6 rpm

            LnU_out.Clear()
            LTq_out.Clear()
            LTqMot_out.Clear()

            nEng_forLoop = FLC.n_idle - 8

            Do Until nEng_forLoop > FLC.LnU(FLC.LnU.Count - 1)
                'torque interpolated
                Tq = FLC.Tq(nEng_forLoop)
                TqMaxMap = TqMax(nEng_forLoop)

                'Check if full load torque is above max torque from map
                If Tq > TqMaxMap Then
                    WorkerMsg(tMsgID.Warn,
                              "   Actual engine Full Load torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) &
                              " [1/min] by " & Math.Round(Tq - TqMaxMap, 2) & " [Nm] !")
                    WorkerMsg(tMsgID.Warn,
                              "     >>> Maximum torque at " & Math.Round(nU, 2) & " [1/min] will be limited to " &
                              Math.Round(TqMaxMap, 2) & " [Nm] in full load output file!")
                    Tq = TqMaxMap
                End If

                LnU_out.Add(nEng_forLoop)
                LTq_out.Add(Tq)
                LTqMot_out.Add(Motoring.Tq(nEng_forLoop))

                nEng_forLoop += 8
            Loop

            If LnU_out(LnU_out.Count - 1) < FLC.n_hi Then
                'add last value (exact value might be necessary for calculation of n_hi in VECTO)
                LnU_out.Add(FLC.n_hi)
                LTq_out.Add(FLC.Tq(FLC.n_hi))
                LTqMot_out.Add(Motoring.Tq(FLC.n_hi))
            End If


        End If
    End Sub

    Private Function TqMax_FLC_cMAP(nU As Double) As Double
        Dim i As Int32

        'Extrapolation for x < x(1)
        If LnU_outTemp(0) >= nU Then
            i = 1
            GoTo lbInt
        End If

        i = 0
        Do While LnU_outTemp(i) < nU And i < (LnU_outTemp.Count - 1)
            i += 1
        Loop


        lbInt:
        'Interpolation
        Return _
            (nU - LnU_outTemp(i - 1))*(LTq_outTemp(i) - LTq_outTemp(i - 1))/(LnU_outTemp(i) - LnU_outTemp(i - 1)) +
            LTq_outTemp(i - 1)
    End Function

    Private Class cRPMlist
        Public ReadOnly TargetRPM As Double
        Public MapPoints As New List(Of cMapPoint)

        Public Sub New(rpm As Integer)
            TargetRPM = rpm
        End Sub
    End Class

    Private Class cMapPoint
        Public nU As Double
        Public Tq As Double
        Public FC1 As Double
        Public FC2 As Double
        Public WHRMechanical As Double
        Public WHRElectrical As Double
        Public MarkedForDeath As Boolean
        Public MarkedForDeathAfterR85Cut As Boolean


        Public Sub New()
            MarkedForDeath = False
            MarkedForDeathAfterR85Cut = False
        End Sub

        'Public Sub New(nU0 As Double, Tq0 As Double, FC01 As Double)
        '    nU = nU0
        '    Tq = Tq0
        '    FC1 = FC01
        '    MarkedForDeath = False
        '    MarkedForDeathAfterR85Cut = False
        'End Sub

        'Public Sub New(nU0 As Double, Tq0 As Double, FC01 As Double, FC02 As Double)
        '    nU = nU0
        '    Tq = Tq0
        '    FC1 = FC01
        '    FC2 = FC02
        '    MarkedForDeath = False
        '    MarkedForDeathAfterR85Cut = False
        'End Sub

        'Public Sub New(nU0 As Double, Tq0 As Double, FC01 As Double, FC02 As Double, WHR1 As Double)
        '    nU = nU0
        '    Tq = Tq0
        '    FC1 = FC01
        '    FC2 = FC02
        '    WHRMechanical = WHR1
        '    MarkedForDeath = False
        '    MarkedForDeathAfterR85Cut = False
        'End Sub

        'Public Sub New(nU0 As Double, Tq0 As Double, FC01 As Double, FC02 As Double, WHR1 As Double, WHR2 As Double)
        '    nU = nU0
        '    Tq = Tq0
        '    FC1 = FC01
        '    FC2 = FC02
        '    WHRMechanical = WHR1
        '    WHRElectrical = WHR2
        '    MarkedForDeath = False
        '    MarkedForDeathAfterR85Cut = False
        'End Sub
    End Class
End Class