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
Imports System.Text
Imports Microsoft.VisualBasic.FileIO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports VECTO_Engine.My.Resources


'uses JSON.NET http://json.codeplex.com/

Public Class cJSON
    Public Content As Dictionary(Of String, JToken)

    'Public ErrorMsg As String

    Public Sub New()
        Content = New Dictionary(Of String, JToken)
    End Sub


    Public sub ReadFile(ByVal path As String)
        
        Content.Clear()

        'check if file exists
        If Not IO.File.Exists(path) Then
           Throw new FileNotFoundException(path)
        End If
        
        Dim settings = New JsonSerializerSettings
        settings.NullValueHandling = NullValueHandling.Ignore
        settings.MissingMemberHandling = MissingMemberHandling.Ignore

        
        Content = JsonConvert.DeserializeObject(Of Dictionary(Of String, JToken))(File.ReadAllText(path), settings)
   End Sub


    Public Function ReadJSON(jobFile As String) As cJob

        Dim cJob = New cJob()
        Dim MsgSrc As String
        Dim JSON As New cJSON

        MsgSrc = "ENG/ReadFile"

        JSON.ReadFile(jobFile)
        'Read JSON-File and parse data
        'Try

        Dim body = JSON.Content("Body")
        If (body Is Nothing) Then Throw New Exception("Failed to read job-file - missing body")
        Dim engine = body(JSONKeys.Engine)
        If (engine Is Nothing) Then Throw New Exception("failed to read job file - no engine parameters")

        cJob.DualFuel = engine(JSONKeys.Engine_DualFuel)
        cJob.WHR_ICE = engine(JSONKeys.Engine_WHR_ICE)
        cJob.WHR_Mech = engine(JSONKeys.Engine_WHR_Mechanical)
        cJob.WHR_El = engine(JSONKeys.Engine_WHR_Electrical)
        cJob.Manufacturer = engine(JSONKeys.Engine_Manufacturer)
        cJob.Model = engine(JSONKeys.Engine_Model)
        cJob.Displacement = engine(JSONKeys.Engine_Displacement)
        cJob.RatedPower = engine(JSONKeys.Engine_RatedPower)
        cJob.RatedSpeed = engine(JSONKeys.Engine_RatedSpeed)
        cJob.MapFile = engine(JSONKeys.Engine_ParentFCMap)
        cJob.FlcParentFile = engine(JSONKeys.Engine_ParentFLC)
        cJob.FlcFile = engine(JSONKeys.Engine_FLC)
        cJob.DragFile = engine(JSONKeys.Engine_MotoringCurve)
        cJob.Idle_Parent = engine(JSONKeys.Engine_IdlingSpeed)
        cJob.Idle = engine(JSONKeys.Engine_ParentIdlingSpeed)
        cJob.CertNumber = engine(JSONKeys.Engine_CertificationNumber)

        Dim fuels = body(JSONKeys.Fuels)
        If (fuels Is Nothing) Then Throw New Exception("failed to read job file - no fuels")
        cJob.FCspecMeas_HotUrb = fuels(0)(JSONKeys.Fuel_WHTCUrban)
        cJob.FCspecMeas_HotRur = fuels(0)(JSONKeys.Fuel_WHTCRural)
        cJob.FCspecMeas_HotMw = fuels(0)(JSONKeys.Fuel_WHTCMotorway)
        cJob.FCspecMeas_ColdTot = fuels(0)(JSONKeys.Fuel_ColdStartTotal)
        cJob.FCspecMeas_HotTot = fuels(0)(JSONKeys.Fuel_HotStartTotal)
        cJob.CF_RegPer = fuels(0)(JSONKeys.Fuel_CFRegPer)
        cJob.FuelType = fuels(0)(JSONKeys.Fuel_FuelType)
        cJob.NCVfuel = fuels(0)(JSONKeys.Fuel_NCV)

        Dim whr = body(JSONKeys.WHRSystem)
        If cJob.WHR_Mech = True Then
            If (whr Is Nothing) Then Throw New Exception("failed to read job file - missing whr data")
            Dim whrMech = whr(JSONKeys.WHR_Mechanical)
            If (whrMech Is Nothing) Then Throw New Exception("failed to read job file - missing electrical whr data")
            cJob.FCspecMeas_HotUrbWHRMech = whrMech(JSONKeys.WHR_WHTCUrban)
            cJob.FCspecMeas_HotRurWHRMech = whrMech(JSONKeys.WHR_WHTCRural)
            cJob.FCspecMeas_HotMwWHRMech = whrMech(JSONKeys.WHR_WHTC_Motorway)
            cJob.FCspecMeas_ColdTotWHRMech = whrMech(JSONKeys.WHR_ColdStartTotal)
            cJob.FCspecMeas_HotTotWHRMech = whrMech(JSONKeys.WHR_HotStartTotal)
            cJob.CF_RegPerWHRMech = whrMech(JSONKeys.WHR_CFRegPer)
        End If

        If cJob.WHR_El = True Then
            If (whr Is Nothing) Then Throw New Exception("failed to read job file - missing whr data")
            Dim whrEl = whr(JSONKeys.WHR_Electrical)
            If (whrEl Is Nothing) Then Throw New Exception("failed to read job file - missing electrical whr data")
            cJob.FCspecMeas_HotUrbWHREl = whrEl(JSONKeys.WHR_WHTCUrban)
            cJob.FCspecMeas_HotRurWHREl = whrEl(JSONKeys.WHR_WHTCRural)
            cJob.FCspecMeas_HotMwWHREl = whrEl(JSONKeys.WHR_WHTC_Motorway)
            cJob.FCspecMeas_ColdTotWHREl = whrEl(JSONKeys.WHR_ColdStartTotal)
            cJob.FCspecMeas_HotTotWHREl = whrEl(JSONKeys.WHR_HotStartTotal)
            cJob.CF_RegPerWHREl = whrEl(JSONKeys.WHR_CFRegPer)
        End If

        If cJob.DualFuel = True Then
            cJob.FCspecMeas_HotUrb2 = fuels(1)(JSONKeys.Fuel_WHTCUrban)
            cJob.FCspecMeas_HotRur2 = fuels(1)(JSONKeys.Fuel_WHTCRural)
            cJob.FCspecMeas_HotMw2 = fuels(1)(JSONKeys.Fuel_WHTCMotorway)
            cJob.FCspecMeas_ColdTot2 = fuels(1)(JSONKeys.Fuel_ColdStartTotal)
            cJob.FCspecMeas_HotTot2 = fuels(1)(JSONKeys.Fuel_HotStartTotal)
            cJob.CF_RegPer2 = fuels(1)(JSONKeys.Fuel_CFRegPer)
            cJob.FuelType2 = fuels(1)(JSONKeys.Fuel_FuelType)
            cJob.NCVfuel2 = fuels(1)(JSONKeys.Fuel_NCV)
        End If

        Return cJob

    End Function


    Protected Function GetHeader() As Dictionary(Of String, Object)
        Dim header As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

        header.Add("CreatedBy", "")
        header.Add("Date", Now.ToUniversalTime().ToString("o"))
        header.Add("AppVersion", FullVersion)
        header.Add("FileVersion", "1")
        Return header
    End Function


    Protected Function GetBody(cJob As CJob, dualFuel As Boolean, whrICE As Boolean, whrMech As Boolean,
                               whrEl As Boolean, filename As String) As Dictionary(Of String, Object)
        Dim body As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
        body.Add(JSONKeys.Engine, GetEngine(cJob, dualFuel, whrICE, whrMech, whrEl, filename))
        body.Add(JSONKeys.WHRSystem, GetWHR(cJob, whrICE, whrMech, whrEl))
        body.Add(JSONKeys.Fuels, GetFuels(cJob, dualFuel))

        Return body
    End Function

    Public Function GetRelativePath(filePath As String, basePath As String) As String
        If (String.IsNullOrEmpty(filePath)) then
            Return ""
        End If
        If (string.isnullOrempty(basePath)) Then
            Return filePath
        End If
        If (Path.GetDirectoryName(filePath).StartsWith(basePath, StringComparison.OrdinalIgnoreCase)) Then
            Return Path.GetFullPath(filePath).Substring(basePath.Length + If(basePath.EndsWith("\"), 0, 1))
        End If
        Return filePath
    End Function

    Protected Function GetEngine(cJob As CJob, dualFuel As Boolean, whrICE As Boolean, whrMech As Boolean,
                                 whrEl As Boolean, filename As String) As Dictionary(Of String, Object)
        Dim _basePath = Path.GetDirectoryName(filename)
        
        Dim fuelConsumptionMapPath As String = Nothing
        Dim fullLoadCurveParentPath As String = Nothing
        Dim fullLoadCurvePath As String = Nothing
        Dim motoringCurvePath As String = Nothing

        If File.Exists(cJob.MapFile) Then
           fuelConsumptionMapPath = Path.GetDirectoryName(cJob.MapFile)
        End If

        If File.Exists(cJob.FlcParentFile) Then
            fullLoadCurveParentPath = Path.GetDirectoryName(cJob.FlcParentFile)
        End If

        If File.Exists(cJob.FlcFile) Then
            fullLoadCurvePath = Path.GetDirectoryName(cJob.FlcFile)
        End If
        
        If File.Exists(cJob.DragFile) Then
            motoringCurvePath = Path.GetDirectoryName(cJob.DragFile)
        End If

        Dim engine = New Dictionary(Of String, Object)
        engine.Add(JSONKeys.Engine_DualFuel, dualFuel)
        engine.Add(JSONKeys.Engine_WHR_ICE, whrICE)
        engine.Add(JSONKeys.Engine_WHR_Mechanical, whrMech)
        engine.Add(JSONKeys.Engine_WHR_Electrical, whrEl)
        engine.Add(JSONKeys.Engine_Manufacturer, cJob.Manufacturer)
        engine.Add(JSONKeys.Engine_Model, cJob.Model)
        engine.Add(JSONKeys.Engine_Displacement, cJob.Displacement)
        engine.Add(JSONKeys.Engine_RatedPower, cJob.RatedPower)
        engine.Add(JSONKeys.Engine_RatedSpeed, cJob.RatedSpeed)

        If(fuelConsumptionMapPath = Nothing)
            engine.Add(JSONKeys.Engine_ParentFCMap, fuelConsumptionMapPath)
        Else 
            engine.Add(JSONKeys.Engine_ParentFCMap,
                    IIf(_basePath.Equals(fuelConsumptionMapPath), GetRelativePath(cJob.MapFile, _basePath), cJob.MapFile))
        End If

        If(fullLoadCurveParentPath = Nothing)
            engine.Add(JSONKeys.Engine_ParentFLC, fullLoadCurveParentPath)
        Else
            engine.Add(JSONKeys.Engine_ParentFLC,
                       IIf(_basePath.Equals(fullLoadCurveParentPath), GetRelativePath(cJob.FlcParentFile, _basePath), cJob.FlcParentFile))
        End If

        If(fullLoadCurvePath = Nothing)
            engine.Add(JSONKeys.Engine_FLC, fullLoadCurvePath)
        Else
            engine.Add(JSONKeys.Engine_FLC,
                       IIf(_basePath.Equals(fullLoadCurvePath), GetRelativePath(cJob.FlcFile, _basePath), cJob.FlcFile))
        End If

        If(motoringCurvePath = Nothing)
            engine.Add(JSONKeys.Engine_MotoringCurve,motoringCurvePath)
        Else 
            engine.Add(JSONKeys.Engine_MotoringCurve,
                       IIf(_basePath.Equals(motoringCurvePath), GetRelativePath(cJob.DragFile, _basePath), cJob.DragFile))
        End If

        engine.Add(JSONKeys.Engine_ParentIdlingSpeed, cJob.Idle_Parent)
        engine.Add(JSONKeys.Engine_IdlingSpeed, cJob.Idle)
        engine.Add(JSONKeys.Engine_CertificationNumber, cJob.CertNumber)
        Return engine
    End Function

    Protected Function GetWHR(cJob As CJob, whrICE As Boolean, whrMech As Boolean, whrEl As Boolean) _
        As Dictionary(Of String, Object)

        Dim whrList = New Dictionary(Of String, Object)

        If whrMech = True Then
            Dim whrMechEntry As Dictionary(Of  String, Object) = New Dictionary(Of String,Object)()
            whrMechEntry.Add(JSONKeys.WHR_WHTCUrban, cJob.FCspecMeas_HotUrbWHRMech)
            whrMechEntry.Add(JSONKeys.WHR_WHTCRural, cJob.FCspecMeas_HotRurWHRMech)
            whrMechEntry.Add(JSONKeys.WHR_WHTC_Motorway, cJob.FCspecMeas_HotMwWHRMech)

            whrMechEntry.Add(JSONKeys.WHR_ColdStartTotal, cJob.FCspecMeas_ColdTotWHRMech)
            whrMechEntry.Add(JSONKeys.WHR_HotStartTotal, cJob.FCspecMeas_HotTotWHRMech)
            whrMechEntry.Add(JSONKeys.WHR_CFRegPer, cJob.CF_RegPerWHRMech)
            whrList(JSONKeys.WHR_Mechanical) = whrMechEntry
        End If

        If whrEl = True Then
            Dim whrElEntry As Dictionary(Of  String, Object) = New Dictionary(Of String,Object)()
            whrElEntry.Add(JSONKeys.WHR_WHTCUrban, cJob.FCspecMeas_HotUrbWHREl)
            whrElEntry.Add(JSONKeys.WHR_WHTCRural, cJob.FCspecMeas_HotRurWHREl)
            whrElEntry.Add(JSONKeys.WHR_WHTC_Motorway, cJob.FCspecMeas_HotMwWHREl)

            whrElEntry.Add(JSONKeys.WHR_ColdStartTotal, cJob.FCspecMeas_ColdTotWHREl)
            whrElEntry.Add(JSONKeys.WHR_HotStartTotal, cJob.FCspecMeas_HotTotWHREl)
            whrElEntry.Add(JSONKeys.WHR_CFRegPer, cJob.CF_RegPerWHREl)
            whrList(JSONKeys.WHR_Electrical) = whrElEntry
        End If
        Return whrList
    End Function


    Protected Function GetFuels(cJob As CJob, dualFuel As Boolean) As List(Of Object)

        Dim entryList As List(Of Object) = New List(Of Object)

        Dim entry As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()
        entry.Add(JSONKeys.Fuel_WHTCUrban, cJob.FCspecMeas_HotUrb)
        entry.Add(JSONKeys.Fuel_WHTCRural, cJob.FCspecMeas_HotRur)
        entry.Add(JSONKeys.Fuel_WHTCMotorway, cJob.FCspecMeas_HotMw)
        'entry.Add("WHTC-Engineering", cJob.FC)
        entry.Add(JSONKeys.Fuel_ColdStartTotal, cJob.FCspecMeas_ColdTot)
        entry.Add(JSONKeys.Fuel_HotStartTotal, cJob.FCspecMeas_HotTot)
        'entry.Add("ColdHotBalancingFactor", "")
        entry.Add(JSONKeys.Fuel_CFRegPer, cJob.CF_RegPer)
        'entry.Add("FuelMap", cJob.fi)
        'entry.Add("FuelMap", GetRelativePath(fuel.FuelConsumptionMap.Source, Path.GetDirectoryName(filename)))
        entry.Add(JSONKeys.Fuel_FuelType, cJob.FuelType)
        entry.Add(JSONKeys.Fuel_NCV, cJob.NCVfuel)

        entryList.Add(entry)

        If dualFuel = True Then
            Dim entry2 As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()
            entry2.Add(JSONKeys.Fuel_WHTCUrban, cJob.FCspecMeas_HotUrb2)
            entry2.Add(JSONKeys.Fuel_WHTCRural, cJob.FCspecMeas_HotRur2)
            entry2.Add(JSONKeys.Fuel_WHTCMotorway, cJob.FCspecMeas_HotMw2)
            'entry.Add("WHTC-Engineering", cJob.FC)
            entry2.Add(JSONKeys.Fuel_ColdStartTotal, cJob.FCspecMeas_ColdTot2)
            entry2.Add(JSONKeys.Fuel_HotStartTotal, cJob.FCspecMeas_HotTot2)
            'entry.Add("ColdHotBalancingFactor", "")
            entry2.Add(JSONKeys.Fuel_CFRegPer, cJob.CF_RegPer2)
            'entry.Add("FuelMap", cJob.fi)
            'entry.Add("FuelMap", GetRelativePath(fuel.FuelConsumptionMap.Source, Path.GetDirectoryName(filename)))
            entry2.Add(JSONKeys.Fuel_FuelType, cJob.FuelType2)
            entry2.Add(JSONKeys.Fuel_NCV, cJob.NCVfuel2)
            entryList.Add(entry2)
        End If
        Return entryList
        'Return fuels
    End Function

    Protected Shared Sub WriteFile(header As Dictionary(Of String, Object), body As Dictionary(Of String, Object),
                                   path As String)
        WriteFile(JToken.FromObject(New Dictionary(Of String, Object) From {{"Header", header}, {"Body", body}}), path)
    End Sub

    Public Shared Sub WriteFile(content As JToken, path As String)
        Dim file As StreamWriter
        Dim str As String

        If content.Count = 0 Then
            Return
        End If

        Try
            str = JsonConvert.SerializeObject(content, Formatting.Indented)
            file = My.Computer.FileSystem.OpenTextFileWriter(path, False)
        Catch ex As Exception
            Throw
        End Try

        file.Write(str)
        file.Close()
    End Sub

    Public Function SaveJob(cJob As cJob, filename As String, dualFuel As Boolean, whrICE As Boolean, whrMech As Boolean,
                            whrEl As Boolean)

        'Header
        Dim header As Dictionary(Of String, Object) = GetHeader()

        'Fuels
        Dim fuels As List(Of Object) = GetFuels(cJob, dualFuel)

        'Body
        Dim body As Dictionary(Of String, Object) = GetBody(cJob, dualFuel, whrICE, whrMech, whrEl, filename)

        'Dim engine As Dictionary(Of String, Object) = 
        WriteFile(header, body, filename)
    End Function


#Region "old self-made parser"

    Private fullfile As String

    Private Function ReadFileXXX(ByVal path As String) As Boolean
        Dim file As TextFieldParser

        Content.Clear()

        'check if file exists
        If Not IO.File.Exists(path) Then Return False

        'open file
        Try
            file = New TextFieldParser(path)
        Catch ex As Exception
            Return False
        End Try

        'Check if file is empty
        If file.EndOfData Then
            file.Close()
            Return False
        End If

        'read file
        fullfile = file.ReadToEnd

        'close file
        file.Close()

        'trim spaces
        fullfile = fullfile.Trim

        'remove line breaks
        fullfile = fullfile.Replace(vbCrLf, "")

        If Left(fullfile, 1) <> "{" Or Right(fullfile, 1) <> "}" Then Return False

        'parse JSON to Dictionary
        Try
            'Content = GetObject()
        Catch ex As Exception
            Return False
        End Try


        Return True
    End Function


    Private Function GetKeyValString(ByVal TabLvl As Integer, ByRef kv As KeyValuePair(Of String, Object)) As String
        Dim str As New StringBuilder
        Dim obj As Object
        Dim kv0 As KeyValuePair(Of String, Object)
        Dim First As Boolean

        str.Append(Tabs(TabLvl) & ChrW(34) & kv.Key & ChrW(34) & ": ")

        Select Case kv.Value.GetType

            Case GetType(Dictionary(Of String, Object))

                str.AppendLine("{")

                First = True
                For Each kv0 In kv.Value
                    If First Then
                        First = False
                    Else
                        str.AppendLine(",")
                    End If
                    str.Append(GetKeyValString(TabLvl + 1, kv0))
                Next

                str.AppendLine()
                str.Append(Tabs(TabLvl) & "}")

            Case GetType(List(Of Object))

                str.AppendLine("[")

                First = True
                For Each obj In kv.Value
                    If First Then
                        First = False
                    Else
                        str.AppendLine(",")
                    End If
                    str.Append(Tabs(TabLvl + 1) & GetObjString(TabLvl + 1, obj))
                Next

                str.AppendLine()
                str.Append(Tabs(TabLvl) & "]")

            Case Else

                str.Append(GetObjString(TabLvl + 1, kv.Value))

        End Select

        Return str.ToString
    End Function

    Private Function GetObjString(ByVal TabLvl As Integer, ByRef obj As Object) As String
        Dim kv0 As KeyValuePair(Of String, Object)
        Dim First As Boolean
        Dim str As StringBuilder

        If obj Is Nothing Then
            Return "null"
        Else
            Select Case obj.GetType

                Case GetType(Dictionary(Of String, Object))

                    str = New StringBuilder
                    str.AppendLine("{")

                    First = True
                    For Each kv0 In obj
                        If First Then
                            First = False
                        Else
                            str.AppendLine(",")
                        End If
                        str.Append(GetKeyValString(TabLvl + 1, kv0))
                    Next

                    str.AppendLine()
                    str.Append(Tabs(TabLvl) & "}")

                    Return str.ToString

                Case GetType(String)

                    Return ChrW(34) & CStr(obj) & ChrW(34)

                Case GetType(Boolean)

                    If CBool(obj) Then
                        Return "true"
                    Else
                        Return "false"
                    End If

                Case Else

                    Return CDbl(obj).ToString

            End Select
        End If
    End Function

    Private Function Tabs(ByVal l As Integer) As String
        Dim i As Integer
        Dim str As String

        str = ""
        For i = 1 To l
            str &= vbTab
        Next

        Return str
    End Function

    Private Function GetObject() As Dictionary(Of String, Object)
        Dim MyDic As Dictionary(Of String, Object)
        Dim key As String
        Dim obj As Object
        Dim i As Integer
        Dim i2 As Integer
        Dim Valstr As String
        Dim ValList As List(Of Object) = Nothing
        Dim ArrayMode As Boolean = False

        'remove {
        fullfile = (Right(fullfile, Len(fullfile) - 1)).Trim

        'new list of key/value pairs
        MyDic = New Dictionary(Of String, Object)


        'loop through key/value pairs
        lb10:
        If Left(fullfile, 1) <> ChrW(34) Then
            Throw New Exception
            Return Nothing
        End If

        'get key
        i = fullfile.IndexOf(ChrW(34), 1)
        key = Mid(fullfile, 2, i - 1)
        fullfile = (Right(fullfile, Len(fullfile) - i - 1)).Trim
        fullfile = (Right(fullfile, Len(fullfile) - 1)).Trim

        If key = "" Then
            Throw New Exception
            Return Nothing
        End If

        'get value (object, number, boolean, array)
        If Left(fullfile, 1) = "[" Then
            ArrayMode = True
            fullfile = (Right(fullfile, Len(fullfile) - 1)).Trim
            ValList = New List(Of Object)
        End If

        lb20:
        If Left(fullfile, 1) = "{" Then
            obj = GetObject()
        Else
            If Left(fullfile, 1) = ChrW(34) Then
                'string
                i = fullfile.IndexOf(ChrW(34), 1)
                obj = Mid(fullfile, 2, i - 1)
                fullfile = (Right(fullfile, Len(fullfile) - i - 1)).Trim
            Else
                'number/boolean
                i = fullfile.IndexOf(",", 1)
                i2 = fullfile.IndexOf("}", 1)

                If i = - 1 Then
                    If i2 = - 1 Then
                        Valstr = Right(fullfile, Len(fullfile) - 1)
                        fullfile = ""
                    Else
                        Valstr = Mid(fullfile, 1, i2)
                        fullfile = (Right(fullfile, Len(fullfile) - i2)).Trim
                    End If
                Else
                    If i2 = - 1 Or i < i2 Then
                        Valstr = Mid(fullfile, 1, i)
                        fullfile = (Right(fullfile, Len(fullfile) - i)).Trim
                    Else
                        Valstr = Mid(fullfile, 1, i2)
                        fullfile = (Right(fullfile, Len(fullfile) - i2)).Trim
                    End If
                End If

                If IsNumeric(Valstr) Then
                    obj = CDbl(Valstr)
                ElseIf (UCase(Valstr)).Trim = "FALSE" Then
                    obj = False
                ElseIf (UCase(Valstr)).Trim = "TRUE" Then
                    obj = True
                ElseIf (UCase(Valstr)).Trim = "NULL" Then
                    obj = Nothing
                Else
                    Throw New Exception
                    Return Nothing
                End If

            End If
        End If

        If ArrayMode Then
            ValList.Add(obj)
            If Left(fullfile, 1) = "]" Then
                ArrayMode = False
                fullfile = (Right(fullfile, Len(fullfile) - 1)).Trim
                MyDic.Add(key, ValList)
            End If
        Else
            MyDic.Add(key, obj)
        End If

        If Left(fullfile, 1) = "," Then
            fullfile = (Right(fullfile, Len(fullfile) - 1)).Trim
            If ArrayMode Then
                GoTo lb20
            Else
                GoTo lb10
            End If
        End If

        If Left(fullfile, 1) = "}" Then
            fullfile = (Right(fullfile, Len(fullfile) - 1)).Trim
        End If

        Return MyDic
    End Function


#End Region
End Class
