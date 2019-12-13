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
Imports System.Collections.Generic
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq


'uses JSON.NET http://json.codeplex.com/

Public Class cJSON
    Public Content As Dictionary(Of String, Object)
    Public ErrorMsg As String

    Public Sub New()
        Content = New Dictionary(Of String, Object)
    End Sub


    Public Function ReadFile(ByVal path As String) As Boolean
        Dim file As Microsoft.VisualBasic.FileIO.TextFieldParser
        Dim str As String


        Content.Clear()

        'check if file exists
        If Not IO.File.Exists(path) Then
            ErrorMsg = "file not found"
            Return False
        End If

        'open file
        Try
            file = New Microsoft.VisualBasic.FileIO.TextFieldParser(path)
        Catch ex As Exception
            ErrorMsg = ex.Message
            Return False
        End Try

        'Check if file is empty
        If file.EndOfData Then
            file.Close()
            ErrorMsg = "file is empty"
            Return False
        End If

        'read file
        str = file.ReadToEnd

        'close file
        file.Close()

        'parse JSON to Dictionary
        Try
            'JSONobj = JsonConvert.DeserializeObject(str)
            Content = JsonConvert.DeserializeObject(str, Content.GetType)
        Catch ex As Exception
            ErrorMsg = ex.Message
            Return False
        End Try

        Return True

    End Function


    

    Public Function ReadJSON(jobFile As String)

        Dim cJob As cJob = new cJob()
        Dim MsgSrc As String
		Dim i As Integer
		Dim JSON As New cJSON
		Dim dic As Object

		MsgSrc = "ENG/ReadFile"

		If Not JSON.ReadFile(jobFile) Then Return False
        'Read JSON-File and parse data
        'Try
            cJob.DualFuel=JSON.Content("Body")("Engine")("DualFuel")
            cJob.WHR1=JSON.Content("Body")("Engine")("WHR1")
            cJob.WHR2=JSON.Content("Body")("Engine")("WHR2")
            cJob.WHR3=JSON.Content("Body")("Engine")("WHR3")
            cJob.Manufacturer = JSON.Content("Body")("Engine")("Manufacturer")
            cJob.Model = JSON.Content("Body")("Engine")("Model")
            cJob.Displacement = JSON.Content("Body")("Engine")("Displacement")
            cJob.RatedPower = JSON.Content("Body")("Engine")("RatedPower")
            cJob.RatedSpeed = JSON.Content("Body")("Engine")("RatedSpeed")
            cJob.MapFile = JSON.Content("Body")("Engine")("ParentFullConsumption")
            cJob.FlcParentFile = JSON.Content("Body")("Engine")("ParentFullLoadCurve")
            cJob.FlcFile = JSON.Content("Body")("Engine")("FullLoadCurve")
            cJob.DragFile = JSON.Content("Body")("Engine")("MotoringCurve")
            cJob.Idle_Parent = JSON.Content("Body")("Engine")("IdlingSpeed")
            cJob.Idle = JSON.Content("Body")("Engine")("ParentIdlingSpeed")
            cJob.CertNumber = JSON.Content("Body")("Engine")("CertificationNumber")

            cJob.FCspecMeas_HotUrb = JSON.Content("Body")("Fuels")(0)("WHTCUrban")
            cJob.FCspecMeas_HotRur = JSON.Content("Body")("Fuels")(0)("WHTCRural")
            cJob.FCspecMeas_HotMw = JSON.Content("Body")("Fuels")(0)("WHTCMotorway")
            cJob.FCspecMeas_ColdTot = JSON.Content("Body")("Fuels")(0)("ColdStartTotal")
            cJob.FCspecMeas_HotTot = JSON.Content("Body")("Fuels")(0)("HotStartTotal")
            cJob.CF_RegPer = JSON.Content("Body")("Fuels")(0)("CFRegPer")
            cJob.FuelType = JSON.Content("Body")("Fuels")(0)("FuelType")
            cJob.NCVfuel = JSON.Content("Body")("Fuels")(0)("NCV")

            IF cJob.WHR2 = True Then
                cJob.FCspecMeas_HotUrbWHR1 = JSON.Content("Body")("WHRs")(0)("WHTCUrban")
                cJob.FCspecMeas_HotRurWHR1 = JSON.Content("Body")("WHRs")(0)("WHTCRural")
                cJob.FCspecMeas_HotMwWHR1 = JSON.Content("Body")("WHRs")(0)("WHTCMotorway")
                cJob.FCspecMeas_ColdTotWHR1 = JSON.Content("Body")("WHRs")(0)("ColdStartTotal")
                cJob.FCspecMeas_HotTotWHR1 = JSON.Content("Body")("WHRs")(0)("HotStartTotal")
                cJob.CF_RegPerWHR1 = JSON.Content("Body")("WHRs")(0)("CFRegPer")
            End If

            If cJob.WHR3 = True Then
                cJob.FCspecMeas_HotUrbWHR2 = JSON.Content("Body")("WHRs")(1)("WHTCUrban")
                cJob.FCspecMeas_HotRurWHR2 = JSON.Content("Body")("WHRs")(1)("WHTCRural")
                cJob.FCspecMeas_HotMwWHR2 = JSON.Content("Body")("WHRs")(1)("WHTCMotorway")
                cJob.FCspecMeas_ColdTotWHR2 = JSON.Content("Body")("WHRs")(1)("ColdStartTotal")
                cJob.FCspecMeas_HotTotWHR2 = JSON.Content("Body")("WHRs")(1)("HotStartTotal")
                cJob.CF_RegPerWHR2 = JSON.Content("Body")("WHRs")(1)("CFRegPer")
            End If

            If cJob.DualFuel = True Then 
                cJob.FCspecMeas_HotUrb2 = JSON.Content("Body")("Fuels")(1)("WHTCUrban")
                cJob.FCspecMeas_HotRur2 = JSON.Content("Body")("Fuels")(1)("WHTCRural")
                cJob.FCspecMeas_HotMw2 = JSON.Content("Body")("Fuels")(1)("WHTCMotorway")
                cJob.FCspecMeas_ColdTot2 = JSON.Content("Body")("Fuels")(1)("ColdStartTotal")
                cJob.FCspecMeas_HotTot2 = JSON.Content("Body")("Fuels")(1)("HotStartTotal")
                cJob.CF_RegPer2 = JSON.Content("Body")("Fuels")(1)("CFRegPer")
                cJob.FuelType2 = JSON.Content("Body")("Fuels")(1)("FuelType")
                cJob.NCVfuel2 = JSON.Content("Body")("Fuels")(1)("NCV")
            End If
        
            Return cJob
            'cJobI = JSON.Content("Body")("IdlingSpeed")
			'I_mot = JSON.Content("Body")("Inertia")

			'i = - 1
			'For Each dic In JSON.Content("Body")("FullLoadCurves")
		    '	i += 1
			'	fFLD.Add(New cSubPath)
			'	fFLD(i).Init(MyPath, dic("Path"))
			'	FLDgears.Add(dic("Gears"))
			'Next

			'fMAP.Init(MyPath, JSON.Content("Body")("FuelMap"))

			'If Not JSON.Content("Body")("WHTC-Urban") Is Nothing Then
			'	WHTCurban = CSng(JSON.Content("Body")("WHTC-Urban"))
			'	WHTCrural = CSng(JSON.Content("Body")("WHTC-Rural"))
			'	WHTCmw = CSng(JSON.Content("Body")("WHTC-Motorway"))
			'#End If

		'Catch ex As Exception
		'	If ShowMsg Then WorkerMsg(tMsgID.Err, "Failed to read VECTO file! " & ex.Message, MsgSrc)
		'	Return False
		'End Try

		Return True
	End Function
 

    Public Function WriteFile(ByVal path As String) As Boolean
        Dim file As System.IO.StreamWriter
        Dim str As String
        Dim First As Boolean = True

        If Content.Count = 0 Then Return False

        Try
            str = Newtonsoft.Json.JsonConvert.SerializeObject(Content, Formatting.Indented)
            file = My.Computer.FileSystem.OpenTextFileWriter(path, False)
        Catch ex As Exception
            Return False
        End Try

        file.Write(str)

        file.Close()

        Return True

    End Function


    Protected Function GetHeader() As Dictionary(Of String, Object)
        Dim header As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

        header.Add("CreatedBy", "")
        header.Add("Date", Now.ToUniversalTime().ToString("o"))
        header.Add("AppVersion", VectoEngineVersion.FullVersion)
        header.Add("FileVersion", "1")
        Return header
    End Function


    Protected Function GetBody(cJob As CJob, dualFuel As Boolean,WHR1 As Boolean, WHR2 As Boolean, WHR3 As Boolean,filename As String) As Dictionary(Of String, Object)
        Dim body As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
        body.Add("Engine",GetEngine(cJob,dualFuel,WHR1,WHR2,WHR3,filename))
        body.Add("WHRs",GetWHRs(cJob,WHR1,WHR2,WHR3))
        body.Add("Fuels",GetFuels(cJob,dualFuel))

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

    Protected Function GetEngine(cJob As CJob, dualFuel As Boolean,WHR1 As Boolean, WHR2 As Boolean, WHR3 As Boolean,filename As String) As Dictionary(Of String, Object)
        Dim _basePath = Path.GetDirectoryName(filename)
        Dim fuelConsumptionMapPath = Path.GetDirectoryName(cJob.MapFile)
        Dim fullLoadCurveParentPath = Path.GetDirectoryName(cJob.FlcParentFile)
        Dim fullLoadCurvePath = Path.GetDirectoryName(cJob.FlcFile)
        Dim motoringCurvePath = Path.GetDirectoryName(cJob.DragFile)


        Dim engine As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
        engine.Add("DualFuel",dualFuel)
        engine.Add("WHR1",WHR1)
        engine.Add("WHR2",WHR2)
        engine.Add("WHR3", WHR3)
        engine.Add("Manufacturer", cJob.Manufacturer)
        engine.Add("Model", cJob.Model)
        engine.Add("Displacement", cJob.Displacement)
        engine.Add("RatedPower", cJob.RatedPower)
        engine.Add("RatedSpeed", cJob.RatedSpeed)
        engine.Add("ParentFullConsumption", IIf(_basePath.Equals(fuelConsumptionMapPath),GetRelativePath(cJob.MapFile,_basePath),cJob.MapFile))
        engine.Add("ParentFullLoadCurve",IIf(_basePath.Equals(fullLoadCurveParentPath),GetRelativePath(cJob.FlcParentFile,_basePath),cJob.FlcParentFile))
        engine.Add("FullLoadCurve", IIf(_basePath.Equals(fullLoadCurvePath),GetRelativePath(cJob.FlcFile,_basePath),cJob.FlcFile))
        engine.Add("MotoringCurve", IIf(_basePath.Equals(motoringCurvePath),GetRelativePath(cJob.DragFile,_basePath),cJob.DragFile))
        engine.Add("ParentIdlingSpeed", cJob.Idle_Parent)
        engine.Add("IdlingSpeed", cJob.Idle)
        engine.Add("CertificationNumber", cJob.CertNumber)
        Return engine
    End Function

    Protected Function GetWHRs(cJob As CJob, WHR1 As Boolean,WHR2 As Boolean, WHR3 As Boolean) As List(Of Object)

        Dim whrList As List(Of Object) = New List(Of Object)

        If WHR2 = True Then
            Dim whr1entry As Dictionary(Of  String, Object) = New Dictionary(Of String,Object)()
            whr1entry.Add("WHTCUrban", cJob.FCspecMeas_HotUrbWHR1)
            whr1entry.Add("WHTCRural", cJob.FCspecMeas_HotRurWHR1)
            whr1entry.Add("WHTCMotorway", cJob.FCspecMeas_HotMwWHR1)
           
            whr1entry.Add("ColdStartTotal", cJob.FCspecMeas_ColdTotWHR1)
            whr1entry.Add("HotStartTotal", cJob.FCspecMeas_HotTotWHR1)
            whr1entry.Add("CFRegPer", cJob.CF_RegPerWHR1)
            whrList.Add(whr1entry)
        End If

        If WHR3 = True Then
            Dim whr2entry As Dictionary(Of  String, Object) = New Dictionary(Of String,Object)()
            whr2entry.Add("WHTCUrban", cJob.FCspecMeas_HotUrbWHR2)
            whr2entry.Add("WHTCRural", cJob.FCspecMeas_HotRurWHR2)
            whr2entry.Add("WHTCMotorway", cJob.FCspecMeas_HotMwWHR2)
           
            whr2entry.Add("ColdStartTotal", cJob.FCspecMeas_ColdTotWHR2)
            whr2entry.Add("HotStartTotal", cJob.FCspecMeas_HotTotWHR2)
            whr2entry.Add("CFRegPer", cJob.CF_RegPerWHR2)
            whrList.Add(whr2entry)
        End If
        Return whrList
    End Function


    Protected Function GetFuels(cJob As CJob, dualFuel As Boolean) As List(Of Object)

        Dim entryList As List(Of Object) = New List(Of Object)

        Dim entry As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()
        entry.Add("WHTCUrban", cJob.FCspecMeas_HotUrb)
        entry.Add("WHTCRural", cJob.FCspecMeas_HotRur)
        entry.Add("WHTCMotorway", cJob.FCspecMeas_HotMw)
        'entry.Add("WHTC-Engineering", cJob.FC)
        entry.Add("ColdStartTotal", cJob.FCspecMeas_ColdTot)
        entry.Add("HotStartTotal", cJob.FCspecMeas_HotTot)
        'entry.Add("ColdHotBalancingFactor", "")
        entry.Add("CFRegPer", cJob.CF_RegPer)
        'entry.Add("FuelMap", cJob.fi)
        'entry.Add("FuelMap", GetRelativePath(fuel.FuelConsumptionMap.Source, Path.GetDirectoryName(filename)))
        entry.Add("FuelType", cJob.FuelType)
        entry.Add("NCV", cJob.NCVfuel)

        entryList.Add(entry)

        If dualFuel = True Then
            Dim entry2 As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()
            entry2.Add("WHTCUrban", cJob.FCspecMeas_HotUrb2)
            entry2.Add("WHTCRural", cJob.FCspecMeas_HotRur2)
            entry2.Add("WHTCMotorway", cJob.FCspecMeas_HotMw2)
            'entry.Add("WHTC-Engineering", cJob.FC)
            entry2.Add("ColdStartTotal", cJob.FCspecMeas_ColdTot2)
            entry2.Add("HotStartTotal", cJob.FCspecMeas_HotTot2)
            'entry.Add("ColdHotBalancingFactor", "")
            entry2.Add("CFRegPer", cJob.CF_RegPer2)
            'entry.Add("FuelMap", cJob.fi)
            'entry.Add("FuelMap", GetRelativePath(fuel.FuelConsumptionMap.Source, Path.GetDirectoryName(filename)))
            entry2.Add("FuelType", cJob.FuelType2)
            entry2.Add("NCV", cJob.NCVfuel2)
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

    Public Function SaveJob(cJob As cJob, filename As String, dualFuel As Boolean,WHR1 As Boolean, WHR2 As Boolean, WHR3 As Boolean)

        'Header
        Dim header As Dictionary(Of String, Object) = GetHeader()

        'Fuels
        Dim fuels As List(Of Object) = GetFuels(cJob, dualFuel)
        
        'Body
        Dim body As Dictionary(Of String, Object) = GetBody(cJob, dualFuel,WHR1, WHR2, WHR3,filename)

        'Dim engine As Dictionary(Of String, Object) = 
        WriteFile(header, body, filename)
    End Function




#Region "old self-made parser"
    Private fullfile As String

    Private Function ReadFileXXX(ByVal path As String) As Boolean
        Dim file As Microsoft.VisualBasic.FileIO.TextFieldParser

        Content.Clear()

        'check if file exists
        If Not IO.File.Exists(path) Then Return False

        'open file
        Try
            file = New Microsoft.VisualBasic.FileIO.TextFieldParser(path)
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
            Content = GetObject()
        Catch ex As Exception
            Return False
        End Try


        Return True

    End Function




    Private Function WriteFileXXX(ByVal path As String) As Boolean
        Dim file As System.IO.StreamWriter
        Dim kv As KeyValuePair(Of String, Object)
        Dim str As New System.Text.StringBuilder
        Dim First As Boolean = True

        If Content.Count = 0 Then Return False

        Try
            str.AppendLine("{")
            For Each kv In Content
                If First Then
                    First = False
                Else
                    str.AppendLine(",")
                End If
                str.Append(GetKeyValString(1, kv))
            Next
            str.AppendLine()
            str.AppendLine("}")
        Catch ex As Exception
            Return False
        End Try

        Try
            file = My.Computer.FileSystem.OpenTextFileWriter(path, False)
        Catch ex As Exception
            Return False
        End Try

        file.Write(str.ToString)

        file.Close()

        Return True

    End Function

    Private Function GetKeyValString(ByVal TabLvl As Integer, ByRef kv As KeyValuePair(Of String, Object)) As String
        Dim str As New System.Text.StringBuilder
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
        Dim str As System.Text.StringBuilder

        If obj Is Nothing Then
            Return "null"
        Else
            Select Case obj.GetType

                Case GetType(Dictionary(Of String, Object))

                    str = New System.Text.StringBuilder
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

                If i = -1 Then
                    If i2 = -1 Then
                        Valstr = Right(fullfile, Len(fullfile) - 1)
                        fullfile = ""
                    Else
                        Valstr = Mid(fullfile, 1, i2)
                        fullfile = (Right(fullfile, Len(fullfile) - i2)).Trim
                    End If
                Else
                    If i2 = -1 Or i < i2 Then
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
