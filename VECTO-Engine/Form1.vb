Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports Microsoft.WindowsAPICodePack.Dialogs

Public Class Form1
    Private Job As cJob
    Private JobSuccess As Boolean

    Private JobPrecalc As cJobPrecalc
    
    private Fuel2Tab as TabPage
    Private WHREl as TabPage
    Private WHRMech as TabPage


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Disable the tab control
        'TabControl1.TabPages("Fuel2").Enabled = False
        'TabControl1.TabPages("WHR1").Enabled = False
        'TabControl1.TabPages("WHR2").Enabled = False

        Fuel2Tab = TabControl1.TabPages(1)
        WHRMech = TabControl1.TabPages(2)
        WHREl = TabControl1.TabPages(3)

        UpdateTabs()
       
        Me.Text = FullVersion

        Worker = Me.BgWorker
    End Sub


    Private Function InitJob()
        Job = New cJob(cbDualFuel.Checked,cbWhrICE.Checked,cbWHRMech.Checked,cbWHREl.Checked)
        Job.Manufacturer = Me.TbManufacturer.Text
        Job.Model = Me.TbModel.Text
        Job.CertNumber = Me.TbCertNumber.Text
        Job.Idle_Parent = Me.TbIdle_Parent.Text
        Job.Idle = Me.TbIdle.Text
        Job.Displacement = Me.TbDisplacement.Text
        Job.RatedPower = Me.TbRatedPower.Text
        Job.RatedSpeed = Me.TbRatedSpeed.Text

        Job.MapFile = Me.TbFuelMap.Text
        Job.FlcFile = Me.TbFLC.Text
        Job.FlcParentFile = Me.TbFLC_Parent.Text
        Job.DragFile = Me.TbMotoring.Text

        Job.FuelType = Me.SfFuelType1.Text
        Job.NCVfuel = Me.TbNCVfuel1.Text
        Job.FCspecMeas_ColdTot = Me.TbFCspecCold1.Text
        Job.FCspecMeas_HotTot =  Me.TbFCspecHot1.Text
        Job.FCspecMeas_HotUrb = Me.TbFCspecUrb1.Text
        Job.FCspecMeas_HotRur = Me.TbFCspecRur1.Text
        Job.FCspecMeas_HotMw =  Me.TbFCspecMW1.Text
        Job.CF_RegPer =  Me.TbCF_RegPer1.Text

        If cbDualFuel.Checked = True Then
            Job.FuelType2 = Me.SfFuelType2.Text
            Job.NCVfuel2 =  Me.TbNCVfuel2.Text
            Job.FCspecMeas_ColdTot2 =  Me.TbFCspecCold2.Text
            Job.FCspecMeas_HotTot2 = Me.TbFCspecHot2.Text
            Job.FCspecMeas_HotUrb2 = Me.TbFCspecUrb2.Text
            Job.FCspecMeas_HotRur2 = Me.TbFCspecRur2.Text
            Job.FCspecMeas_HotMw2 =  Me.TbFCspecMW2.Text
            Job.CF_RegPer2 = Me.TbCF_RegPer2.Text
           
        End If

       If cbWHRMech.Checked = True Then
            Job.FCspecMeas_ColdTotWHRMech=Me.TbFCspecColdWHRMech.Text
            Job.FCspecMeas_HotTotWHRMech = Me.TbFCspecHotWHRMech.Text
            Job.FCspecMeas_HotUrbWHRMech = Me.TbFCspecUrbWHRMech.Text
            Job.FCspecMeas_HotRurWHRMech = Me.TbFCspecRurWHRMech.Text
            Job.FCspecMeas_HotMwWHRMech = Me.TbFCspecMWWHRMech.Text
            Job.CF_RegPerWHRMech = Me.TbCF_RegPerWHRMech.Text
        End If

        If cbWHREl.Checked = True Then

            Job.FCspecMeas_ColdTotWHREl= Me.TbFCspecColdWHREl.Text
            Job.FCspecMeas_HotTotWHREl = Me.TbFCspecHotWHREl.Text
            Job.FCspecMeas_HotUrbWHREl = Me.TbFCspecUrbWHREl.Text
            Job.FCspecMeas_HotRurWHREl = Me.TbFCspecRurWHREl.Text
            Job.FCspecMeas_HotMwWHREl = Me.TbFCspecMWWHREl.Text
            Job.CF_RegPerWHREl = Me.TbCF_RegPerWHREl.Text

        End If

        Job.OutPath = Me.TbOutputFolder.Text & "\"
    End Function



    Private Sub BtStart_Click(sender As Object, e As EventArgs) Handles BtStart.Click

        Job = New cJob(cbDualFuel.Checked,cbWhrICE.Checked,cbWHRMech.Checked,cbWHREl.Checked)
        JobSuccess = False

        Try

            '**********************************************
            'REAL PART FOR INPUT DATA ASSIGNMENT
            '**********************************************
            InitJob()

            JobInit()

            If Not CheckInput() Then
                JobEnd()
                Exit Sub
            End If

            Job.Manufacturer = Me.TbManufacturer.Text
            Job.Model = Me.TbModel.Text
            Job.CertNumber = Me.TbCertNumber.Text
            Job.Idle_Parent = Me.TbIdle_Parent.Text
            Job.Idle = Me.TbIdle.Text
            Job.Displacement = Me.TbDisplacement.Text
            Job.RatedPower = Me.TbRatedPower.Text
            Job.RatedSpeed = Me.TbRatedSpeed.Text

            Job.MapFile = Me.TbFuelMap.Text
            Job.FlcFile = Me.TbFLC.Text
            Job.FlcParentFile = Me.TbFLC_Parent.Text
            Job.DragFile = Me.TbMotoring.Text

            If cbDualFuel.Checked = True Then
                Job.FuelType = Me.SfFuelType1.Text
                Job.NCVfuel = Me.TbNCVfuel1.Text
                Job.FCspecMeas_ColdTot = Me.TbFCspecCold1.Text
                Job.FCspecMeas_HotTot = Me.TbFCspecHot1.Text
                Job.FCspecMeas_HotUrb = Me.TbFCspecUrb1.Text
                Job.FCspecMeas_HotRur = Me.TbFCspecRur1.Text
                Job.FCspecMeas_HotMw = Me.TbFCspecMW1.Text
                Job.CF_RegPer = Me.TbCF_RegPer1.Text

                Job.FuelType2 = Me.SfFuelType2.Text
                Job.NCVfuel2 = Me.TbNCVfuel2.Text
                Job.FCspecMeas_ColdTot2 = Me.TbFCspecCold2.Text
                Job.FCspecMeas_HotTot2 = Me.TbFCspecHot2.Text
                Job.FCspecMeas_HotUrb2 = Me.TbFCspecUrb2.Text
                Job.FCspecMeas_HotRur2 = Me.TbFCspecRur2.Text
                Job.FCspecMeas_HotMw2 = Me.TbFCspecMW2.Text
                Job.CF_RegPer2 = Me.TbCF_RegPer2.Text
            Else
                Job.FuelType = Me.SfFuelType1.Text
                Job.NCVfuel = Me.TbNCVfuel1.Text
                Job.FCspecMeas_ColdTot = Me.TbFCspecCold1.Text
                Job.FCspecMeas_HotTot = Me.TbFCspecHot1.Text
                Job.FCspecMeas_HotUrb = Me.TbFCspecUrb1.Text
                Job.FCspecMeas_HotRur = Me.TbFCspecRur1.Text
                Job.FCspecMeas_HotMw = Me.TbFCspecMW1.Text
                Job.CF_RegPer = Me.TbCF_RegPer1.Text
            End If




            Job.OutPath = Me.TbOutputFolder.Text & "\"

            '**********************************************
            'REAL PART FOR INPUT DATA ASSIGNMENT - END
            '**********************************************


            '**********************************************
            '**********************************************
            '**********************************************
            '      TEST DATA  START
            '**********************************************
            '**********************************************
            '**********************************************


            'Test DataSet 1

            'JobInit()


            'Job.Manufacturer = "TUG"
            'Job.Model = "Testengine"
            'Job.CertNumber = "Engine0815"
            'Job.Idle_Parent = 600
            'Job.Idle = 600
            'Job.Displacement = 7700
            'Job.RatedPower = 130
            'Job.RatedSpeed = 2200
            'Job.FuelType = "Ethanol / CI"
            'Job.NCVfuel = 42.3

            'Job.MapFile = "E:\ve_test\VECTO-Engine_1.4.4.xxx-DEV\Demo input data\Demo_Map_v1.4.csv"
            'Job.FlcFile = "E:\ve_test\VECTO-Engine_1.4.4.xxx-DEV\Demo input data\Demo_FullLoad_Child_v1.4.csv"
            'Job.FlcParentFile = "E:\ve_test\VECTO-Engine_1.4.4.xxx-DEV\Demo input data\Demo_FullLoad_Parent_v1.4.csv"
            'Job.DragFile = "E:\ve_test\VECTO-Engine_1.4.4.xxx-DEV\Demo input data\Demo_Motoring_v1.4.csv"

            'Job.FCspecMeas_ColdTot = 200.0
            'Job.FCspecMeas_HotTot = 200.0
            'Job.FCspecMeas_HotUrb = 200.0
            'Job.FCspecMeas_HotRur = 200.0
            'Job.FCspecMeas_HotMw = 200.0
            'Job.CF_RegPer = 1.0


            'Job.OutPath = "E:\ve_test\VECTO-Engine_1.4.4.xxx-DEV\output"


            '**********************************************
            '**********************************************
            '**********************************************
            '      TEST DATA  END
            '**********************************************
            '**********************************************
            '**********************************************


        Catch ex As Exception
            ShowMsgDirect(ex.Message, tMsgID.Err)
            Exit Sub
        End Try

        Worker.RunWorkerAsync()
    End Sub

    Private Sub JobInit()

        Me.GrInput.Enabled = False
        Me.LvMsg.Items.Clear()
        'Me.TbUrbanFactor.Clear()
        'Me.TbRuralFactor.Clear()
        'Me.TbMotorwayFactor.Clear()
    End Sub

    Private Sub JobEnd()

        'Set calc mode back again to normal from Precalc (=1)
        CalcMode = 0

        Me.GrInput.Enabled = True
    End Sub

    Private Sub BgWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BgWorker.DoWork

        If SetCulture Then
            Try
                Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
            Catch ex As Exception
                ShowMsgDirect(
                    "Failed to set thread culture 'en-US'! Check system decimal- and group- separators and restart application!",
                    tMsgID.Err)
                Exit Sub
            End Try
        End If

        'Calc mode 1 = only precalculation
        If CalcMode = 1 Then
            JobPrecalc.Run()
        Else
            JobSuccess = Job.Run()
            If JobSuccess Then
                'Messagebox: VALID DESPITE OF WARNINGS
                Dim newline = Environment.NewLine
                MsgBox(
                    "DATA EVALUATION IS COMPLETED." & newline & newline &
                    "The results produced are valid for certification despite any warnings displayed in the message window!" &
                    newline &
                    newline &
                    "Nevertheless, causes for warnings shall be analyzed together with the Technical Service or Type Approval Authority.",
                    MsgBoxStyle.Information, MsgBoxStyle.OkOnly)

            End If
        End If
    End Sub

    Private Sub BgWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) _
        Handles BgWorker.ProgressChanged
        Dim WorkerMsg As cWorkerMsg

        WorkerMsg = e.UserState

        ShowMsgDirect(WorkerMsg.Msg, WorkerMsg.MsgType)
    End Sub

    Private Sub BgWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles BgWorker.RunWorkerCompleted
        JobEnd()

        'If JobSuccess Then
        '    Me.TbUrbanFactor.Text = Job.WHTCurbanFactor.ToString("0.0000")
        '    Me.TbRuralFactor.Text = Job.WHTCruralFactor.ToString("0.0000")
        '    Me.TbMotorwayFactor.Text = Job.WHTCmotorwayFactor.ToString("0.0000")
        '    Me.TbColdHotFactor.Text = Job.ColdHotBalancingFactor.ToString("0.0000")
        'End If
    End Sub

    Private Function CheckInput() As Boolean
        Dim Result As Boolean
        Dim StringForSplit As String
        Dim StringsAfterSplit As String()

        Result = True

        'Check if numbers are numbers and files do exist

        'check all file references

        If Trim(Me.TbOutputFolder.Text) = "" OrElse Not Directory.Exists(Me.TbOutputFolder.Text) Then
            ShowMsgDirect("Output folder not found!", tMsgID.Err)
            Result = False
        End If


        'check all input fields, either string or numeric value
        If Trim(Me.TbManufacturer.Text) = "" Then
            ShowMsgDirect("Field ""Manufacturer"" is empty!", tMsgID.Err)
            Result = False
        End If

        If Trim(Me.TbModel.Text) = "" Then
            ShowMsgDirect("Field ""Model"" is empty!", tMsgID.Err)
            Result = False
        End If

        If Trim(Me.TbCertNumber.Text) = "" Then
            ShowMsgDirect("Field ""Certification Number"" is empty!", tMsgID.Err)
            Result = False
        End If

        If Not IsNumeric(Me.TbDisplacement.Text) Then
            ShowMsgDirect("Engine displacement is not valid!", tMsgID.Err)
            Result = False
        Else
            If Not IsInteger(Me.TbDisplacement.Text) Then
                ShowMsgDirect("Engine displacement needs to be rounded to the nearest whole number!", tMsgID.Err)
                Result = False
            End If
        End If

        If Not IsNumeric(Me.TbRatedPower.Text) Then
            ShowMsgDirect("Engine rated power is not valid!", tMsgID.Err)
            Result = False
        Else
            If Not IsInteger(Me.TbRatedPower.Text) Then
                ShowMsgDirect("Engine rated power needs to be rounded to the nearest whole number!", tMsgID.Err)
                Result = False
            End If
        End If
        If Not IsNumeric(Me.TbRatedSpeed.Text) Then
            ShowMsgDirect("Engine rated speed is not valid!", tMsgID.Err)
            Result = False
        Else
            If Not IsInteger(Me.TbRatedSpeed.Text) Then
                ShowMsgDirect("Engine rated speed needs to be rounded to the nearest whole number!", tMsgID.Err)
                Result = False
            End If
        End If


        If Not IsNumeric(Me.TbIdle_Parent.Text) Then
            ShowMsgDirect("Idle speed of CO2-parent engine is not valid!", tMsgID.Err)
            Result = False
        Else
            If Not IsInteger(Me.TbIdle_Parent.Text) Then
                ShowMsgDirect("Idle speed of CO2-parent engine needs to be rounded to the nearest whole number!",
                              tMsgID.Err)
                Result = False
            End If
        End If

        If Not IsNumeric(Me.TbIdle.Text) Then
            ShowMsgDirect("Engine idle speed is not valid!", tMsgID.Err)
            Result = False
        Else
            If Not IsInteger(Me.TbIdle.Text) Then
                ShowMsgDirect("Engine idle speed needs to be rounded to the nearest whole number!", tMsgID.Err)
                Result = False
            End If
        End If

        If Trim(Me.TbFuelMap.Text) = "" OrElse Not File.Exists(Me.TbFuelMap.Text) Then
            ShowMsgDirect("File for fuel consumption map of CO2-parent engine not found!", tMsgID.Err)
            Result = False
        End If

        If Trim(Me.TbFLC_Parent.Text) = "" OrElse Not File.Exists(Me.TbFLC_Parent.Text) Then
            ShowMsgDirect("File for full-Load curve of CO2-parent engine not found!", tMsgID.Err)
            Result = False
        End If

        If Trim(Me.TbFLC.Text) = "" OrElse Not File.Exists(Me.TbFLC.Text) Then
            ShowMsgDirect("File for full-Load curve of actual engine not found!", tMsgID.Err)
            Result = False
        End If

        'If Trim(Me.TbWHTC.Text) = "" OrElse Not IO.File.Exists(Me.TbWHTC.Text) Then
        '    ShowMsgDirect("WHTC file not found!", tMsgID.Err)
        '    Result = False
        'End If

        If Trim(Me.TbMotoring.Text) = "" OrElse Not File.Exists(Me.TbMotoring.Text) Then
            ShowMsgDirect("File for motoring curve of CO2-parent engine not found!", tMsgID.Err)
            Result = False
        End If


        If Trim(Me.SfFuelType1.Text) = "" Then
            ShowMsgDirect("Fuel1: Field ""Type of test fuel"" is empty!", tMsgID.Err)
            Result = False
        End If

        If Not IsNumeric(Me.TbNCVfuel1.Text) Then
            ShowMsgDirect("Fuel1: NCV of test fuel is not valid!", tMsgID.Err)
            Result = False
        Else
            StringForSplit = Me.TbNCVfuel1.Text
            If StringForSplit.Contains(".") Then
                StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                If StringsAfterSplit(1).Length <> 2 Then
                    ShowMsgDirect("Fuel1: NCV of test fuel needs to have exactly 2 digits after the decimal point!",
                                  tMsgID.Err)
                    Result = False
                End If
            Else
                ShowMsgDirect("Fuel1: NCV of test fuel does not contain a valid decimal separator!", tMsgID.Err)
                Result = False
            End If
        End If

        If Not IsNumeric(Me.TbFCspecCold1.Text) Then
            ShowMsgDirect("Fuel1: Specific FC of WHTC coldstart is not valid!", tMsgID.Err)
            Result = False
        Else
            StringForSplit = Me.TbFCspecCold1.Text
            If StringForSplit.Contains(".") Then
                StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                If StringsAfterSplit(1).Length <> 2 Then
                    ShowMsgDirect(
                        "Fuel1: Specific FC of WHTC coldstart needs to have exactly 2 digits after the decimal point!",
                        tMsgID.Err)
                    Result = False
                End If
            Else
                ShowMsgDirect("Fuel1: Specific FC of WHTC coldstart does not contain a valid decimal separator!",
                              tMsgID.Err)
                Result = False
            End If
        End If


        If Not IsNumeric(Me.TbFCspecHot1.Text) Then
            ShowMsgDirect("Fuel1: Specific FC of WHTC hotstart is not valid!", tMsgID.Err)
            Result = False
        Else
            StringForSplit = Me.TbFCspecHot1.Text
            If StringForSplit.Contains(".") Then
                StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                If StringsAfterSplit(1).Length <> 2 Then
                    ShowMsgDirect(
                        "Fuel1: Specific FC of WHTC hotstart needs to have exactly 2 digits after the decimal point!",
                        tMsgID.Err)
                    Result = False
                End If
            Else
                ShowMsgDirect("Fuel1: Specific FC of WHTC hotstart does not contain a valid decimal separator!", tMsgID.Err)
                Result = False
            End If
        End If


        If Not IsNumeric(Me.TbFCspecUrb1.Text) Then
            ShowMsgDirect("Fuel1: Specific FC of WHTC-Urban is not valid!", tMsgID.Err)
            Result = False
        Else
            StringForSplit = Me.TbFCspecUrb1.Text
            If StringForSplit.Contains(".") Then
                StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                If StringsAfterSplit(1).Length <> 2 Then
                    ShowMsgDirect(
                        "Fuel1: Specific FC of WHTC-Urban needs to have exactly 2 digits after the decimal point!",
                        tMsgID.Err)
                    Result = False
                End If
            Else
                ShowMsgDirect("Fuel1: Specific FC of WHTC-Urban does not contain a valid decimal separator!", tMsgID.Err)
                Result = False
            End If
        End If


        If Not IsNumeric(Me.TbFCspecRur1.Text) Then
            ShowMsgDirect("Fuel1: Specific FC of WHTC-Rural is not valid!", tMsgID.Err)
            Result = False
        Else
            StringForSplit = Me.TbFCspecRur1.Text
            If StringForSplit.Contains(".") Then
                StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                If StringsAfterSplit(1).Length <> 2 Then
                    ShowMsgDirect(
                        "Fuel1: Specific FC of WHTC-Rural needs to have exactly 2 digits after the decimal point!",
                        tMsgID.Err)
                    Result = False
                End If
            Else
                ShowMsgDirect("Fuel1: Specific FC of WHTC-Rural does not contain a valid decimal separator!", tMsgID.Err)
                Result = False
            End If
        End If


        If Not IsNumeric(Me.TbFCspecMW1.Text) Then
            ShowMsgDirect("Fuel1: Specific FC of WHTC-Motorway is not valid!", tMsgID.Err)
            Result = False
        Else
            StringForSplit = Me.TbFCspecMW1.Text
            If StringForSplit.Contains(".") Then
                StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                If StringsAfterSplit(1).Length <> 2 Then
                    ShowMsgDirect(
                        "Fuel1: Specific FC of WHTC-Motorway needs to have exactly 2 digits after the decimal point!",
                        tMsgID.Err)
                    Result = False
                End If
            Else
                ShowMsgDirect("Fuel1: Specific FC of WHTC-Motorway does not contain a valid decimal separator!", tMsgID.Err)
                Result = False
            End If
        End If

        If Not IsNumeric(Me.TbCF_RegPer1.Text) Then
            ShowMsgDirect("Fuel1: CF-RegPer is not valid!", tMsgID.Err)
            Result = False
        Else
            StringForSplit = Me.TbCF_RegPer1.Text
            If StringForSplit.Contains(".") Then
                StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                If StringsAfterSplit(1).Length <> 2 Then
                    ShowMsgDirect("Fuel1: CF-RegPer needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
                    Result = False
                End If
            Else
                ShowMsgDirect("Fuel1: CF-RegPer does not contain a valid decimal separator!", tMsgID.Err)
                Result = False
            End If
        End If


        'Check if dualfuel is selected if selected check fuel2
        If cbDualFuel.Checked = True Then

            If Trim(Me.SfFuelType2.Text) = "" Then
                ShowMsgDirect("Fuel2: Field ""Type of test fuel2"" is empty!", tMsgID.Err)
                Result = False
            End If

            If Not IsNumeric(Me.TbNCVfuel2.Text) Then
                ShowMsgDirect("Fuel2: NCV of test fuel is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbNCVfuel2.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect("Fuel2: NCV of test fuel needs to have exactly 2 digits after the decimal point!",
                                      tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("Fuel2: NCV of test fuel does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If

            If Not IsNumeric(Me.TbFCspecCold2.Text) Then
                ShowMsgDirect("Fuel2: Specific FC of WHTC coldstart is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecCold2.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "Fuel2: Specific FC of WHTC coldstart needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("Fuel2: Specific FC of WHTC coldstart does not contain a valid decimal separator!",
                                  tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecHot2.Text) Then
                ShowMsgDirect("Fuel2: Specific FC of WHTC hotstart is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecHot2.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "Fuel2: Specific FC of WHTC hotstart needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("Fuel2: Specific FC of WHTC hotstart does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecUrb2.Text) Then
                ShowMsgDirect("Fuel2: Specific FC of WHTC-Urban is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecUrb2.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "Fuel2: Specific FC of WHTC-Urban needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("Fuel2: Specific FC of WHTC-Urban does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecRur2.Text) Then
                ShowMsgDirect("Fuel2: Specific FC of WHTC-Rural is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecRur2.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "Fuel2: Specific FC of WHTC-Rural needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("Fuel2: Specific FC of WHTC-Rural does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecMW2.Text) Then
                ShowMsgDirect("Fuel2: Specific FC of WHTC-Motorway is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecMW2.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "Fuel2: Specific FC of WHTC-Motorway needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("Fuel2: Specific FC of WHTC-Motorway does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If

            If Not IsNumeric(Me.TbCF_RegPer2.Text) Then
                ShowMsgDirect("Fuel2: CF-RegPer is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbCF_RegPer2.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect("Fuel2: CF-RegPer needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("Fuel2: CF-RegPer does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If

        End If


        'WHR mechanical
        If cbWHRMech.Checked = True Then

            If Not IsNumeric(Me.TbFCspecColdWHRMech.Text) Then
                ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC coldstart is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecColdWHRMech.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC coldstart needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC coldstart does not contain a valid decimal separator!",
                                  tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecHotWHRMech.Text) Then
                ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC hotstart is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecHotWHRMech.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "WHR mechanical: Specific WHR work of WHTC hotstart needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC hotstart does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecUrbWHRMech.Text) Then
                ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC-Urban is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecUrbWHRMech.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "WHR mechanical: Specific WHR work of WHTC-Urban needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC-Urban does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecRurWHRMech.Text) Then
                ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC-Rural is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecRurWHRMech.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "WHR mechanical: Specific WHR work of WHTC-Rural needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC-Rural does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecMWWHRMech.Text) Then
                ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC-Motorway is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecMWWHRMech.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "WHR mechanical: Specific WHR work of WHTC-Motorway needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR mechanical: Specific WHR work of WHTC-Motorway does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If

            If Not IsNumeric(Me.TbCF_RegPerWHRMech.Text) Then
                ShowMsgDirect("WHR mechanical: CF-RegPer is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbCF_RegPerWHRMech.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect("WHR mechanical: CF-RegPer needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR mechanical: CF-RegPer does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If

        End If

        'WHR2

        If cbWHREl.Checked = True Then

            If Not IsNumeric(Me.TbFCspecColdWHREl.Text) Then
                ShowMsgDirect("WHR2: Specific FC of WHTC coldstart is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecColdWHREl.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect("WHR2: Specific FC of WHTC coldstart needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR2: Specific FC of WHTC coldstart does not contain a valid decimal separator!",
                                  tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecHotWHREl.Text) Then
                ShowMsgDirect("WHR2: Specific FC of WHTC hotstart is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecHotWHREl.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "WHR2: Specific FC of WHTC hotstart needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR2: Specific FC of WHTC hotstart does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecUrbWHREl.Text) Then
                ShowMsgDirect("WHR2: Specific FC of WHTC-Urban is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecUrbWHREl.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "WHR2: Specific FC of WHTC-Urban needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR2: Specific FC of WHTC-Urban does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecRurWHREl.Text) Then
                ShowMsgDirect("Fuel2: Specific FC of WHTC-Rural is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecRurWHREl.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "WHR2: Specific FC of WHTC-Rural needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR2: Specific FC of WHTC-Rural does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If


            If Not IsNumeric(Me.TbFCspecMWWHREl.Text) Then
                ShowMsgDirect("WHR2: Specific FC of WHTC-Motorway is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbFCspecMWWHREl.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect(
                            "WHR2: Specific FC of WHTC-Motorway needs to have exactly 2 digits after the decimal point!",
                            tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR2: Specific FC of WHTC-Motorway does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If

            If Not IsNumeric(Me.TbCF_RegPerWHREl.Text) Then
                ShowMsgDirect("WHR2: CF-RegPer is not valid!", tMsgID.Err)
                Result = False
            Else
                StringForSplit = Me.TbCF_RegPerWHREl.Text
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        ShowMsgDirect("WHR2: CF-RegPer needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
                        Result = False
                    End If
                Else
                    ShowMsgDirect("WHR2: CF-RegPer does not contain a valid decimal separator!", tMsgID.Err)
                    Result = False
                End If
            End If

        End If



        Return Result
    End Function

    Private Sub ShowMsgDirect(Msg As String, MsgType As tMsgID)
        Dim lv0 As ListViewItem

        lv0 = New ListViewItem
        lv0.Text = Msg

        Select Case MsgType
            Case tMsgID.Err
                lv0.ForeColor = Color.Red
            Case tMsgID.Warn
                lv0.ForeColor = Color.DarkOrange
        End Select

        Me.LvMsg.Items.Add(lv0)

        lv0.EnsureVisible()
    End Sub


    Private Sub BtOpenMap_Click(sender As Object, e As EventArgs) Handles BtOpenMap.Click
        Dim dlog As New OpenFileDialog
        dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
        If dlog.ShowDialog() = DialogResult.OK Then Me.TbFuelMap.Text = dlog.FileName
    End Sub

    Private Sub TbFuelMap_TextChanged(sender As Object, e As EventArgs)
        If Me.TbOutputFolder.Text = "" AndAlso Me.TbFuelMap.Text <> "" AndAlso File.Exists(Me.TbFuelMap.Text) Then
            Me.TbOutputFolder.Text = Path.GetDirectoryName(Me.TbFuelMap.Text)
        End If
    End Sub

    Private Sub BtFLCParent_Click(sender As Object, e As EventArgs) Handles BtFLCParent.Click
        Dim dlog As New OpenFileDialog
        dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
        If dlog.ShowDialog() = DialogResult.OK Then Me.TbFLC_Parent.Text = dlog.FileName
    End Sub

    Private Sub BtFLC_Click(sender As Object, e As EventArgs) Handles BtFLC.Click
        Dim dlog As New OpenFileDialog
        dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
        If dlog.ShowDialog() = DialogResult.OK Then Me.TbFLC.Text = dlog.FileName
    End Sub


    Private Sub BtMW_Click(sender As Object, e As EventArgs) Handles BtMW.Click
        Dim dlog As New OpenFileDialog
        dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
        If dlog.ShowDialog() = DialogResult.OK Then Me.TbMotoring.Text = dlog.FileName
    End Sub


    Private Sub BtOutputDirectory_Click(sender As Object, e As EventArgs) Handles BtOutputDirectory.Click
        Dim directoryDialog As new CommonOpenFileDialog()
        directoryDialog.Multiselect=False
        directoryDialog.EnsurePathExists = True
        directoryDialog.IsFolderPicker = True
        directoryDialog.Title = "Select Folder"

        If directoryDialog.ShowDialog = DialogResult.OK Then Me.TbOutputFolder.Text = directoryDialog.FileName
    End Sub

    'Text-to-number
    Public Function fTextboxToNumString(txt As String) As String
        If Not IsNumeric(txt) Then
            Return "0"
        Else
            Return txt
        End If
    End Function


    ' Precalculation of WHTC test speeds and grid for fuel map
    Private Sub BtPrecalc_Click(sender As Object, e As EventArgs) Handles BtPrecalc.Click

        JobPrecalc = New cJobPrecalc
        CalcMode = 1

        Try
            JobInit()

            If Not CheckInputPrecalc() Then
                JobEnd()
                Exit Sub
            End If

            JobPrecalc.FlcParentFile = Me.TbFLC_Parent.Text
            JobPrecalc.Idle_Parent = Me.TbIdle_Parent.Text

        Catch ex As Exception
            ShowMsgDirect(ex.Message, tMsgID.Err)
            Exit Sub
        End Try

        Worker.RunWorkerAsync()
    End Sub


    Private Function CheckInputPrecalc() As Boolean
        Dim Result As Boolean

        Result = True

        'Check if files do exist       
        If Trim(Me.TbFLC_Parent.Text) = "" OrElse Not File.Exists(Me.TbFLC_Parent.Text) Then
            ShowMsgDirect("File for full-Load curve of CO2-parent engine not found!", tMsgID.Err)
            Result = False
        End If

        If Not IsNumeric(Me.TbIdle_Parent.Text) Then
            ShowMsgDirect("Idle speed of CO2-parent engine is not valid!", tMsgID.Err)
            Result = False
        Else
            If Not IsInteger(Me.TbIdle_Parent.Text) Then
                ShowMsgDirect("Idle speed of CO2-parent engine needs to be rounded to the nearest whole number!",
                              tMsgID.Err)
                Result = False
            End If
        End If


        Return Result
    End Function

    Private Sub BtCopyToClipboard_Click(sender As Object, e As EventArgs) Handles BtCopyToClipboard.Click
        Dim iCopyLvMsg As Integer
        Dim strText As String

        Clipboard.Clear()
        strText = ""

        With LvMsg
            For iCopyLvMsg = 0 To .Items.Count - 1
                strText = strText & .Items(iCopyLvMsg).Text
                strText = strText & vbNewLine
            Next iCopyLvMsg
        End With
        Clipboard.SetText(strText)
    End Sub

    Private Sub JobFileButton_Click(sender As Object, e As EventArgs) Handles JobFileButton.Click

        Dim dlog As New OpenFileDialog
        dlog.Filter = "Vecto Engine Job (*.vep)|*.vep"
        If dlog.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Me.TbJobFilePath.Text = dlog.FileName
       Dim jobFilePath = Path.GetDirectoryName(dlog.FileName)

        Dim json As New cJSON()
        If Me.TbJobFilePath.Text.Length > 0 Then
            try
            Job = json.ReadJSON(Me.TbJobFilePath.Text)
                Catch ex As Exception
                WorkerMsg(tMsgID.Err, String.Format("failed to open file: {0}", ex.Message))
                return
                end try
            Me.cbDualFuel.Checked=Job.DualFuel
            Me.cbWHRMech.Checked=Job.WHR_Mech
            Me.cbWHREl.Checked=Job.WHR_El
            Me.TbManufacturer.Text = Job.Manufacturer
            Me.TbModel.Text = Job.Model
            Me.TbCertNumber.Text = Job.CertNumber
            Me.TbIdle_Parent.Text = Job.Idle_Parent
            Me.TbIdle.Text = Job.Idle
            Me.TbDisplacement.Text = Job.Displacement
            Me.TbRatedPower.Text = Job.RatedPower
            Me.TbRatedSpeed.Text = Job.RatedSpeed

            If String.IsNullOrEmpty(Path.GetDirectoryName(Job.MapFile)) Then
                Me.TbFuelMap.Text=jobFilePath+"\"+Job.MapFile
            Else
                Me.TbFuelMap.Text=Job.MapFile
            End If

            If String.IsNullOrEmpty(Path.GetDirectoryName(Job.FlcFile)) Then
                Me.TbFLC.Text=jobFilePath+"\"+Job.FlcFile
            Else
                Me.TbFLC.Text=Job.FlcFile
            End If

            If String.IsNullOrEmpty(Path.GetDirectoryName(Job.FlcParentFile)) Then
                Me.TbFLC_Parent.Text=jobFilePath+"\"+Job.FlcParentFile
            Else
                Me.TbFLC_Parent.Text=Job.FlcParentFile
            End If

            If String.IsNullOrEmpty(Path.GetDirectoryName(Job.DragFile)) Then
                Me.TbMotoring.Text=jobFilePath+"\"+Job.DragFile
            Else
                Me.TbMotoring.Text=Job.DragFile
            End If

            Me.SfFuelType1.Text =Job.FuelType
            Me.TbNCVfuel1.Text = Job.NCVfuel.ToString("#,##0.00")
            Me.TbFCspecCold1.Text =   Job.FCspecMeas_ColdTot.ToString("#,##0.00")
            Me.TbFCspecHot1.Text = Job.FCspecMeas_HotTot.ToString("#,##0.00")
            Me.TbFCspecUrb1.Text = Job.FCspecMeas_HotUrb.ToString("#,##0.00")
            Me.TbFCspecRur1.Text = Job.FCspecMeas_HotRur.ToString("#,##0.00")
            Me.TbFCspecMW1.Text = Job.FCspecMeas_HotMw.ToString("#,##0.00")
            Me.TbCF_RegPer1.Text = Job.CF_RegPer.ToString("#,##0.00")

            Me.SfFuelType2.Text =Job.FuelType2
            Me.TbNCVfuel2.Text = Job.NCVfuel2.ToString("#,##0.00")
            Me.TbFCspecCold2.Text =   Job.FCspecMeas_ColdTot2.ToString("#,##0.00")
            Me.TbFCspecHot2.Text = Job.FCspecMeas_HotTot2.ToString("#,##0.00")
            Me.TbFCspecUrb2.Text = Job.FCspecMeas_HotUrb2.ToString("#,##0.00")
            Me.TbFCspecRur2.Text = Job.FCspecMeas_HotRur2.ToString("#,##0.00")
            Me.TbFCspecMW2.Text = Job.FCspecMeas_HotMw2.ToString("#,##0.00")
            Me.TbCF_RegPer2.Text = Job.CF_RegPer2.ToString("#,##0.00")

            Me.TbFCspecColdWHRMech.Text =   Job.FCspecMeas_ColdTotWHRMech.ToString("#,##0.00")
            Me.TbFCspecHotWHRMech.Text = Job.FCspecMeas_HotTotWHRMech.ToString("#,##0.00")
            Me.TbFCspecUrbWHRMech.Text = Job.FCspecMeas_HotUrbWHRMech.ToString("#,##0.00")
            Me.TbFCspecRurWHRMech.Text = Job.FCspecMeas_HotRurWHRMech.ToString("#,##0.00")
            Me.TbFCspecMWWHRMech.Text = Job.FCspecMeas_HotMwWHRMech.ToString("#,##0.00")
            Me.TbCF_RegPerWHRMech.Text = Job.CF_RegPerWHRMech.ToString("#,##0.00")

            Me.TbFCspecColdWHREl.Text =   Job.FCspecMeas_ColdTotWHREl.ToString("#,##0.00")
            Me.TbFCspecHotWHREl.Text = Job.FCspecMeas_HotTotWHREl.ToString("#,##0.00")
            Me.TbFCspecUrbWHREl.Text = Job.FCspecMeas_HotUrbWHREl.ToString("#,##0.00")
            Me.TbFCspecRurWHREl.Text = Job.FCspecMeas_HotRurWHREl.ToString("#,##0.00")
            Me.TbFCspecMWWHREl.Text = Job.FCspecMeas_HotMwWHREl.ToString("#,##0.00")
            Me.TbCF_RegPerWHREl.Text = Job.CF_RegPerWHREl.ToString("#,##0.00")

        End If

      

        
    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs)
    End Sub

    Private Sub DualFuelCheckBox_CheckedChanged(sender As Object, e As EventArgs) _
        Handles cbDualFuel.CheckedChanged

        UpdateTabs()
      
    End Sub

    Private Sub BtOpenMap_Click_1(sender As Object, e As EventArgs) Handles BtOpenMap.Click
    End Sub

    Private Sub Save_Click(sender As Object, e As EventArgs) Handles Save.Click

        Dim json As New cJSON()
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Filter ="Vecto Engine Job (*.vep)|*.vep"
        saveFileDialog.FilterIndex=1
        saveFileDialog.RestoreDirectory=True

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            'If CheckInput() Then 
                InitJob()
                If saveFileDialog.FileName <> "" Then
                json.SaveJob(Job, saveFileDialog.FileName, cbDualFuel.Checked,cbWhrICE.Checked,cbWHRMech.Checked,cbWHREl.Checked)
                End If
           
            
                MsgBox("The file was saved successfully", 0, "Save JobFile")
            'Else
            '    MsgBox("Please fill out all parameters", 0, "Save JobFile")
            'End If

            'MsgBox(saveFileDialog.FileName)
        End If

        
    End Sub

    Private Sub WHR1CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles cbWhrICE.CheckedChanged
       
    End Sub

    Private Sub WHR2CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles cbWHRMech.CheckedChanged
       UpdateTabs()
    End Sub

    Private Sub WHR3CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles cbWHREl.CheckedChanged
       UpdateTabs()
    End Sub

   
   private function UpdateTabs()
        TabControl1.TabPages.Remove(Fuel2Tab)
        TabControl1.TabPages.Remove(WHRMech)
        TabControl1.TabPages.Remove(WHREl)

        if (cbDualFuel.Checked) Then
            TabControl1.TabPages.Add(Fuel2Tab)
        End If
        if (cbWHRMech.checked) Then
            TabControl1.TabPages.Add(WHRMech)
        End If
        if (cbWHREl.Checked) Then
            TabControl1.TabPages.Add(WHREl)
        End If
    End function

    Private Sub Label49_Click(sender As Object, e As EventArgs) Handles Label49.Click

    End Sub

End Class

