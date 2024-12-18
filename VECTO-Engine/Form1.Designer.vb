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
Imports System.ComponentModel
Imports Microsoft.VisualBasic.CompilerServices

<DesignerGenerated()> _
Partial Class Form1
	Inherits Form

	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	<DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Wird vom Windows Form-Designer benötigt.
	Private components As IContainer

	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<DebuggerStepThrough()> _
	Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.GrInput = New System.Windows.Forms.GroupBox()
        Me.Save = New System.Windows.Forms.Button()
        Me.JobFileButton = New System.Windows.Forms.Button()
        Me.JobFileLabel = New System.Windows.Forms.Label()
        Me.TbJobFilePath = New System.Windows.Forms.TextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TbFLC = New System.Windows.Forms.TextBox()
        Me.BtFLC = New System.Windows.Forms.Button()
        Me.TbMotoring = New System.Windows.Forms.TextBox()
        Me.BtMW = New System.Windows.Forms.Button()
        Me.TbFLC_Parent = New System.Windows.Forms.TextBox()
        Me.BtFLCParent = New System.Windows.Forms.Button()
        Me.TbFuelMap = New System.Windows.Forms.TextBox()
        Me.BtOpenMap = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.Fuel1 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.TbCF_RegPer1 = New System.Windows.Forms.TextBox()
        Me.GrRpm = New System.Windows.Forms.GroupBox()
        Me.TbFCspecHot1 = New System.Windows.Forms.TextBox()
        Me.TbFCspecCold1 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TbFCspecMW1 = New System.Windows.Forms.TextBox()
        Me.TbFCspecRur1 = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.TbFCspecUrb1 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SfFuelType1 = New System.Windows.Forms.ComboBox()
        Me.CbFuelType = New System.Windows.Forms.Label()
        Me.TbNCVfuel1 = New System.Windows.Forms.TextBox()
        Me.TbNCVfuel = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Fuel2 = New System.Windows.Forms.TabPage()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.TbCF_RegPer2 = New System.Windows.Forms.TextBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.TbFCspecHot2 = New System.Windows.Forms.TextBox()
        Me.TbFCspecCold2 = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.TbFCspecMW2 = New System.Windows.Forms.TextBox()
        Me.TbFCspecRur2 = New System.Windows.Forms.TextBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.TbFCspecUrb2 = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.SfFuelType2 = New System.Windows.Forms.ComboBox()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.TbNCVfuel2 = New System.Windows.Forms.TextBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.tbWHRMech = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TbCF_RegPerWHRMech = New System.Windows.Forms.TextBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.TbFCspecHotWHRMech = New System.Windows.Forms.TextBox()
        Me.TbFCspecColdWHRMech = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.TbFCspecMWWHRMech = New System.Windows.Forms.TextBox()
        Me.TbFCspecRurWHRMech = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.TbFCspecUrbWHRMech = New System.Windows.Forms.TextBox()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.tbWhrEl = New System.Windows.Forms.TabPage()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.TbCF_RegPerWHREl = New System.Windows.Forms.TextBox()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.TbFCspecHotWHREl = New System.Windows.Forms.TextBox()
        Me.TbFCspecColdWHREl = New System.Windows.Forms.TextBox()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.TbFCspecMWWHREl = New System.Windows.Forms.TextBox()
        Me.TbFCspecRurWHREl = New System.Windows.Forms.TextBox()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.TbFCspecUrbWHREl = New System.Windows.Forms.TextBox()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.TbIdle_Parent = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TbIdle = New System.Windows.Forms.TextBox()
        Me.ComponentData = New System.Windows.Forms.GroupBox()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.cbWHREl = New System.Windows.Forms.CheckBox()
        Me.cbWhrICE = New System.Windows.Forms.CheckBox()
        Me.cbWHRMech = New System.Windows.Forms.CheckBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.EngineRatedSpeedLabel = New System.Windows.Forms.Label()
        Me.TbRatedSpeed = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.EngineRatedPowerLabel = New System.Windows.Forms.Label()
        Me.cbDualFuel = New System.Windows.Forms.CheckBox()
        Me.TbRatedPower = New System.Windows.Forms.TextBox()
        Me.CertificationNumberLabel = New System.Windows.Forms.Label()
        Me.TbCertNumber = New System.Windows.Forms.TextBox()
        Me.ModelLabel = New System.Windows.Forms.Label()
        Me.TbModel = New System.Windows.Forms.TextBox()
        Me.ManufacturerLabel = New System.Windows.Forms.Label()
        Me.TbManufacturer = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.EngineDisplacementLabel = New System.Windows.Forms.Label()
        Me.TbDisplacement = New System.Windows.Forms.TextBox()
        Me.VECTOLogo = New System.Windows.Forms.PictureBox()
        Me.GrOutput = New System.Windows.Forms.GroupBox()
        Me.BtCopyToClipboard = New System.Windows.Forms.Button()
        Me.LvMsg = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.TbOutputFolder = New System.Windows.Forms.TextBox()
        Me.BtOutputDirectory = New System.Windows.Forms.Button()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.BtStart = New System.Windows.Forms.Button()
        Me.BgWorker = New System.ComponentModel.BackgroundWorker()
        Me.BtPrecalc = New System.Windows.Forms.Button()
        Me.GrInput.SuspendLayout
        Me.GroupBox5.SuspendLayout
        Me.GroupBox2.SuspendLayout
        Me.TabControl1.SuspendLayout
        Me.Fuel1.SuspendLayout
        Me.GroupBox1.SuspendLayout
        Me.GrRpm.SuspendLayout
        Me.Fuel2.SuspendLayout
        Me.GroupBox6.SuspendLayout
        Me.GroupBox7.SuspendLayout
        Me.tbWHRMech.SuspendLayout
        Me.GroupBox3.SuspendLayout
        Me.GroupBox4.SuspendLayout
        Me.tbWhrEl.SuspendLayout
        Me.GroupBox8.SuspendLayout
        Me.GroupBox9.SuspendLayout
        Me.ComponentData.SuspendLayout
        Me.GroupBox10.SuspendLayout
        CType(Me.VECTOLogo,System.ComponentModel.ISupportInitialize).BeginInit
        Me.GrOutput.SuspendLayout
        Me.SuspendLayout
        '
        'GrInput
        '
        Me.GrInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(255,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.GrInput.Controls.Add(Me.Save)
        Me.GrInput.Controls.Add(Me.JobFileButton)
        Me.GrInput.Controls.Add(Me.JobFileLabel)
        Me.GrInput.Controls.Add(Me.TbJobFilePath)
        Me.GrInput.Controls.Add(Me.GroupBox5)
        Me.GrInput.Controls.Add(Me.ComponentData)
        Me.GrInput.Controls.Add(Me.VECTOLogo)
        Me.GrInput.Location = New System.Drawing.Point(12, 12)
        Me.GrInput.Name = "GrInput"
        Me.GrInput.Size = New System.Drawing.Size(941, 632)
        Me.GrInput.TabIndex = 0
        Me.GrInput.TabStop = false
        Me.GrInput.Text = "Input"
        '
        'Save
        '
        Me.Save.Location = New System.Drawing.Point(724, 19)
        Me.Save.Name = "Save"
        Me.Save.Size = New System.Drawing.Size(206, 23)
        Me.Save.TabIndex = 5
        Me.Save.Text = "Save Job"
        Me.Save.UseVisualStyleBackColor = true
        '
        'JobFileButton
        '
        Me.JobFileButton.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.JobFileButton.Location = New System.Drawing.Point(672, 19)
        Me.JobFileButton.Name = "JobFileButton"
        Me.JobFileButton.Size = New System.Drawing.Size(28, 23)
        Me.JobFileButton.TabIndex = 20
        Me.JobFileButton.TabStop = false
        Me.JobFileButton.UseVisualStyleBackColor = true
        '
        'JobFileLabel
        '
        Me.JobFileLabel.AutoSize = true
        Me.JobFileLabel.Location = New System.Drawing.Point(6, 22)
        Me.JobFileLabel.Name = "JobFileLabel"
        Me.JobFileLabel.Size = New System.Drawing.Size(43, 13)
        Me.JobFileLabel.TabIndex = 0
        Me.JobFileLabel.Text = "Job-File"
        '
        'TbJobFilePath
        '
        Me.TbJobFilePath.Location = New System.Drawing.Point(55, 19)
        Me.TbJobFilePath.Name = "TbJobFilePath"
        Me.TbJobFilePath.Size = New System.Drawing.Size(611, 20)
        Me.TbJobFilePath.TabIndex = 0
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.GroupBox2)
        Me.GroupBox5.Controls.Add(Me.TabControl1)
        Me.GroupBox5.Controls.Add(Me.Label16)
        Me.GroupBox5.Controls.Add(Me.Label18)
        Me.GroupBox5.Controls.Add(Me.TbIdle_Parent)
        Me.GroupBox5.Controls.Add(Me.Label24)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.TbIdle)
        Me.GroupBox5.Location = New System.Drawing.Point(9, 208)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(921, 414)
        Me.GroupBox5.TabIndex = 2
        Me.GroupBox5.TabStop = false
        Me.GroupBox5.Text = "Engine test data"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.TbFLC)
        Me.GroupBox2.Controls.Add(Me.BtFLC)
        Me.GroupBox2.Controls.Add(Me.TbMotoring)
        Me.GroupBox2.Controls.Add(Me.BtMW)
        Me.GroupBox2.Controls.Add(Me.TbFLC_Parent)
        Me.GroupBox2.Controls.Add(Me.BtFLCParent)
        Me.GroupBox2.Controls.Add(Me.TbFuelMap)
        Me.GroupBox2.Controls.Add(Me.BtOpenMap)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 70)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(906, 127)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = false
        Me.GroupBox2.Text = "Data files"
        '
        'Label11
        '
        Me.Label11.AutoSize = true
        Me.Label11.Location = New System.Drawing.Point(153, 76)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(76, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Full-load curve"
        '
        'Label12
        '
        Me.Label12.AutoSize = true
        Me.Label12.Location = New System.Drawing.Point(47, 102)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(182, 13)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "Motoring curve of CO2-parent engine"
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(49, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(180, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Full-load curve of CO2-parent engine"
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(12, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(217, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Fuel consumption map of CO2-parent engine"
        '
        'TbFLC
        '
        Me.TbFLC.Location = New System.Drawing.Point(236, 73)
        Me.TbFLC.Name = "TbFLC"
        Me.TbFLC.Size = New System.Drawing.Size(626, 20)
        Me.TbFLC.TabIndex = 2
        '
        'BtFLC
        '
        Me.BtFLC.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.BtFLC.Location = New System.Drawing.Point(872, 71)
        Me.BtFLC.Name = "BtFLC"
        Me.BtFLC.Size = New System.Drawing.Size(28, 23)
        Me.BtFLC.TabIndex = 19
        Me.BtFLC.TabStop = false
        Me.BtFLC.UseVisualStyleBackColor = true
        '
        'TbMotoring
        '
        Me.TbMotoring.Location = New System.Drawing.Point(236, 99)
        Me.TbMotoring.Name = "TbMotoring"
        Me.TbMotoring.Size = New System.Drawing.Size(626, 20)
        Me.TbMotoring.TabIndex = 3
        '
        'BtMW
        '
        Me.BtMW.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.BtMW.Location = New System.Drawing.Point(872, 96)
        Me.BtMW.Name = "BtMW"
        Me.BtMW.Size = New System.Drawing.Size(28, 23)
        Me.BtMW.TabIndex = 21
        Me.BtMW.TabStop = false
        Me.BtMW.UseVisualStyleBackColor = true
        '
        'TbFLC_Parent
        '
        Me.TbFLC_Parent.Location = New System.Drawing.Point(236, 47)
        Me.TbFLC_Parent.Name = "TbFLC_Parent"
        Me.TbFLC_Parent.Size = New System.Drawing.Size(626, 20)
        Me.TbFLC_Parent.TabIndex = 1
        '
        'BtFLCParent
        '
        Me.BtFLCParent.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.BtFLCParent.Location = New System.Drawing.Point(872, 45)
        Me.BtFLCParent.Name = "BtFLCParent"
        Me.BtFLCParent.Size = New System.Drawing.Size(28, 23)
        Me.BtFLCParent.TabIndex = 17
        Me.BtFLCParent.TabStop = false
        Me.BtFLCParent.UseVisualStyleBackColor = true
        '
        'TbFuelMap
        '
        Me.TbFuelMap.Location = New System.Drawing.Point(236, 21)
        Me.TbFuelMap.Name = "TbFuelMap"
        Me.TbFuelMap.Size = New System.Drawing.Size(626, 20)
        Me.TbFuelMap.TabIndex = 0
        '
        'BtOpenMap
        '
        Me.BtOpenMap.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.BtOpenMap.Location = New System.Drawing.Point(872, 19)
        Me.BtOpenMap.Name = "BtOpenMap"
        Me.BtOpenMap.Size = New System.Drawing.Size(28, 23)
        Me.BtOpenMap.TabIndex = 15
        Me.BtOpenMap.TabStop = false
        Me.BtOpenMap.UseVisualStyleBackColor = true
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.Fuel1)
        Me.TabControl1.Controls.Add(Me.Fuel2)
        Me.TabControl1.Controls.Add(Me.tbWHRMech)
        Me.TabControl1.Controls.Add(Me.tbWhrEl)
        Me.TabControl1.Location = New System.Drawing.Point(6, 203)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(909, 215)
        Me.TabControl1.TabIndex = 3
        '
        'Fuel1
        '
        Me.Fuel1.Controls.Add(Me.GroupBox1)
        Me.Fuel1.Controls.Add(Me.GrRpm)
        Me.Fuel1.Controls.Add(Me.SfFuelType1)
        Me.Fuel1.Controls.Add(Me.CbFuelType)
        Me.Fuel1.Controls.Add(Me.TbNCVfuel1)
        Me.Fuel1.Controls.Add(Me.TbNCVfuel)
        Me.Fuel1.Controls.Add(Me.Label30)
        Me.Fuel1.Location = New System.Drawing.Point(4, 22)
        Me.Fuel1.Name = "Fuel1"
        Me.Fuel1.Padding = New System.Windows.Forms.Padding(3)
        Me.Fuel1.Size = New System.Drawing.Size(901, 189)
        Me.Fuel1.TabIndex = 0
        Me.Fuel1.Text = "Fuel 1"
        Me.Fuel1.UseVisualStyleBackColor = true
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.TbCF_RegPer1)
        Me.GroupBox1.Location = New System.Drawing.Point(663, 78)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(214, 101)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = false
        Me.GroupBox1.Text = "Correction factors"
        '
        'Label17
        '
        Me.Label17.AutoSize = true
        Me.Label17.Location = New System.Drawing.Point(6, 50)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(59, 13)
        Me.Label17.TabIndex = 3
        Me.Label17.Text = "CF-RegPer"
        '
        'TbCF_RegPer1
        '
        Me.TbCF_RegPer1.Location = New System.Drawing.Point(71, 47)
        Me.TbCF_RegPer1.Name = "TbCF_RegPer1"
        Me.TbCF_RegPer1.Size = New System.Drawing.Size(111, 20)
        Me.TbCF_RegPer1.TabIndex = 0
        '
        'GrRpm
        '
        Me.GrRpm.Controls.Add(Me.TbFCspecHot1)
        Me.GrRpm.Controls.Add(Me.TbFCspecCold1)
        Me.GrRpm.Controls.Add(Me.Label7)
        Me.GrRpm.Controls.Add(Me.Label10)
        Me.GrRpm.Controls.Add(Me.TbFCspecMW1)
        Me.GrRpm.Controls.Add(Me.TbFCspecRur1)
        Me.GrRpm.Controls.Add(Me.Label20)
        Me.GrRpm.Controls.Add(Me.Label8)
        Me.GrRpm.Controls.Add(Me.Label3)
        Me.GrRpm.Controls.Add(Me.Label9)
        Me.GrRpm.Controls.Add(Me.Label6)
        Me.GrRpm.Controls.Add(Me.Label5)
        Me.GrRpm.Controls.Add(Me.Label19)
        Me.GrRpm.Controls.Add(Me.TbFCspecUrb1)
        Me.GrRpm.Controls.Add(Me.Label4)
        Me.GrRpm.Location = New System.Drawing.Point(11, 78)
        Me.GrRpm.Name = "GrRpm"
        Me.GrRpm.Size = New System.Drawing.Size(636, 101)
        Me.GrRpm.TabIndex = 2
        Me.GrRpm.TabStop = false
        Me.GrRpm.Text = "Specific fuel consumption measured"
        '
        'TbFCspecHot1
        '
        Me.TbFCspecHot1.Location = New System.Drawing.Point(120, 75)
        Me.TbFCspecHot1.Name = "TbFCspecHot1"
        Me.TbFCspecHot1.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecHot1.TabIndex = 1
        '
        'TbFCspecCold1
        '
        Me.TbFCspecCold1.Location = New System.Drawing.Point(120, 34)
        Me.TbFCspecCold1.Name = "TbFCspecCold1"
        Me.TbFCspecCold1.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecCold1.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = true
        Me.Label7.Location = New System.Drawing.Point(395, 50)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "WHTC-Rural"
        '
        'Label10
        '
        Me.Label10.AutoSize = true
        Me.Label10.Location = New System.Drawing.Point(566, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(47, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "[g/kWh]"
        '
        'TbFCspecMW1
        '
        Me.TbFCspecMW1.Location = New System.Drawing.Point(469, 75)
        Me.TbFCspecMW1.Name = "TbFCspecMW1"
        Me.TbFCspecMW1.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecMW1.TabIndex = 4
        '
        'TbFCspecRur1
        '
        Me.TbFCspecRur1.Location = New System.Drawing.Point(469, 47)
        Me.TbFCspecRur1.Name = "TbFCspecRur1"
        Me.TbFCspecRur1.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecRur1.TabIndex = 3
        '
        'Label20
        '
        Me.Label20.AutoSize = true
        Me.Label20.Location = New System.Drawing.Point(13, 78)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(101, 13)
        Me.Label20.TabIndex = 0
        Me.Label20.Text = "WHTC hotstart total"
        '
        'Label8
        '
        Me.Label8.AutoSize = true
        Me.Label8.Location = New System.Drawing.Point(566, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "[g/kWh]"
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.Location = New System.Drawing.Point(8, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "WHTC coldstart total"
        '
        'Label9
        '
        Me.Label9.AutoSize = true
        Me.Label9.Location = New System.Drawing.Point(391, 23)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "WHTC-Urban"
        '
        'Label6
        '
        Me.Label6.AutoSize = true
        Me.Label6.Location = New System.Drawing.Point(566, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "[g/kWh]"
        '
        'Label5
        '
        Me.Label5.AutoSize = true
        Me.Label5.Location = New System.Drawing.Point(374, 78)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "WHTC-Motorway"
        '
        'Label19
        '
        Me.Label19.AutoSize = true
        Me.Label19.Location = New System.Drawing.Point(217, 78)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(47, 13)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "[g/kWh]"
        '
        'TbFCspecUrb1
        '
        Me.TbFCspecUrb1.Location = New System.Drawing.Point(469, 19)
        Me.TbFCspecUrb1.Name = "TbFCspecUrb1"
        Me.TbFCspecUrb1.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecUrb1.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = true
        Me.Label4.Location = New System.Drawing.Point(217, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "[g/kWh]"
        '
        'SfFuelType1
        '
        Me.SfFuelType1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.SfFuelType1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SfFuelType1.FormattingEnabled = true
        Me.SfFuelType1.Items.AddRange(New Object() {"Diesel / CI", "Diesel B100 CI", "Ethanol / CI", "Petrol / PI", "Ethanol / PI", "LPG / PI", "Natural Gas / PI", "Natural Gas / CI", "H2 / PI", "H2 / CI"})
        Me.SfFuelType1.Location = New System.Drawing.Point(108, 6)
        Me.SfFuelType1.Name = "SfFuelType1"
        Me.SfFuelType1.Size = New System.Drawing.Size(167, 21)
        Me.SfFuelType1.TabIndex = 0
        '
        'CbFuelType
        '
        Me.CbFuelType.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.CbFuelType.AutoSize = true
        Me.CbFuelType.Location = New System.Drawing.Point(16, 11)
        Me.CbFuelType.Name = "CbFuelType"
        Me.CbFuelType.Size = New System.Drawing.Size(83, 13)
        Me.CbFuelType.TabIndex = 24
        Me.CbFuelType.Text = "Type of test fuel"
        '
        'TbNCVfuel1
        '
        Me.TbNCVfuel1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TbNCVfuel1.Location = New System.Drawing.Point(108, 34)
        Me.TbNCVfuel1.Name = "TbNCVfuel1"
        Me.TbNCVfuel1.Size = New System.Drawing.Size(114, 20)
        Me.TbNCVfuel1.TabIndex = 1
        '
        'TbNCVfuel
        '
        Me.TbNCVfuel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TbNCVfuel.AutoSize = true
        Me.TbNCVfuel.Location = New System.Drawing.Point(18, 37)
        Me.TbNCVfuel.Name = "TbNCVfuel"
        Me.TbNCVfuel.Size = New System.Drawing.Size(81, 13)
        Me.TbNCVfuel.TabIndex = 25
        Me.TbNCVfuel.Text = "NCV of test fuel"
        '
        'Label30
        '
        Me.Label30.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label30.AutoSize = true
        Me.Label30.Location = New System.Drawing.Point(228, 37)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(44, 13)
        Me.Label30.TabIndex = 26
        Me.Label30.Text = "[MJ/kg]"
        '
        'Fuel2
        '
        Me.Fuel2.Controls.Add(Me.GroupBox6)
        Me.Fuel2.Controls.Add(Me.GroupBox7)
        Me.Fuel2.Controls.Add(Me.SfFuelType2)
        Me.Fuel2.Controls.Add(Me.Label43)
        Me.Fuel2.Controls.Add(Me.TbNCVfuel2)
        Me.Fuel2.Controls.Add(Me.Label44)
        Me.Fuel2.Controls.Add(Me.Label45)
        Me.Fuel2.Location = New System.Drawing.Point(4, 22)
        Me.Fuel2.Name = "Fuel2"
        Me.Fuel2.Padding = New System.Windows.Forms.Padding(3)
        Me.Fuel2.Size = New System.Drawing.Size(901, 189)
        Me.Fuel2.TabIndex = 1
        Me.Fuel2.Text = "Fuel 2"
        Me.Fuel2.UseVisualStyleBackColor = true
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Label28)
        Me.GroupBox6.Controls.Add(Me.TbCF_RegPer2)
        Me.GroupBox6.Location = New System.Drawing.Point(663, 78)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(214, 101)
        Me.GroupBox6.TabIndex = 3
        Me.GroupBox6.TabStop = false
        Me.GroupBox6.Text = "Correction factors"
        '
        'Label28
        '
        Me.Label28.AutoSize = true
        Me.Label28.Location = New System.Drawing.Point(6, 50)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(59, 13)
        Me.Label28.TabIndex = 3
        Me.Label28.Text = "CF-RegPer"
        '
        'TbCF_RegPer2
        '
        Me.TbCF_RegPer2.Location = New System.Drawing.Point(71, 47)
        Me.TbCF_RegPer2.Name = "TbCF_RegPer2"
        Me.TbCF_RegPer2.Size = New System.Drawing.Size(111, 20)
        Me.TbCF_RegPer2.TabIndex = 0
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.TbFCspecHot2)
        Me.GroupBox7.Controls.Add(Me.TbFCspecCold2)
        Me.GroupBox7.Controls.Add(Me.Label29)
        Me.GroupBox7.Controls.Add(Me.Label34)
        Me.GroupBox7.Controls.Add(Me.TbFCspecMW2)
        Me.GroupBox7.Controls.Add(Me.TbFCspecRur2)
        Me.GroupBox7.Controls.Add(Me.Label35)
        Me.GroupBox7.Controls.Add(Me.Label36)
        Me.GroupBox7.Controls.Add(Me.Label37)
        Me.GroupBox7.Controls.Add(Me.Label38)
        Me.GroupBox7.Controls.Add(Me.Label39)
        Me.GroupBox7.Controls.Add(Me.Label40)
        Me.GroupBox7.Controls.Add(Me.Label41)
        Me.GroupBox7.Controls.Add(Me.TbFCspecUrb2)
        Me.GroupBox7.Controls.Add(Me.Label42)
        Me.GroupBox7.Location = New System.Drawing.Point(11, 78)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(636, 101)
        Me.GroupBox7.TabIndex = 2
        Me.GroupBox7.TabStop = false
        Me.GroupBox7.Text = "Specific fuel consumption measured"
        '
        'TbFCspecHot2
        '
        Me.TbFCspecHot2.Location = New System.Drawing.Point(120, 75)
        Me.TbFCspecHot2.Name = "TbFCspecHot2"
        Me.TbFCspecHot2.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecHot2.TabIndex = 1
        '
        'TbFCspecCold2
        '
        Me.TbFCspecCold2.Location = New System.Drawing.Point(120, 34)
        Me.TbFCspecCold2.Name = "TbFCspecCold2"
        Me.TbFCspecCold2.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecCold2.TabIndex = 0
        '
        'Label29
        '
        Me.Label29.AutoSize = true
        Me.Label29.Location = New System.Drawing.Point(395, 50)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(68, 13)
        Me.Label29.TabIndex = 0
        Me.Label29.Text = "WHTC-Rural"
        '
        'Label34
        '
        Me.Label34.AutoSize = true
        Me.Label34.Location = New System.Drawing.Point(566, 22)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(47, 13)
        Me.Label34.TabIndex = 2
        Me.Label34.Text = "[g/kWh]"
        '
        'TbFCspecMW2
        '
        Me.TbFCspecMW2.Location = New System.Drawing.Point(469, 75)
        Me.TbFCspecMW2.Name = "TbFCspecMW2"
        Me.TbFCspecMW2.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecMW2.TabIndex = 4
        '
        'TbFCspecRur2
        '
        Me.TbFCspecRur2.Location = New System.Drawing.Point(469, 47)
        Me.TbFCspecRur2.Name = "TbFCspecRur2"
        Me.TbFCspecRur2.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecRur2.TabIndex = 3
        '
        'Label35
        '
        Me.Label35.AutoSize = true
        Me.Label35.Location = New System.Drawing.Point(13, 78)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(101, 13)
        Me.Label35.TabIndex = 0
        Me.Label35.Text = "WHTC hotstart total"
        '
        'Label36
        '
        Me.Label36.AutoSize = true
        Me.Label36.Location = New System.Drawing.Point(566, 50)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(47, 13)
        Me.Label36.TabIndex = 2
        Me.Label36.Text = "[g/kWh]"
        '
        'Label37
        '
        Me.Label37.AutoSize = true
        Me.Label37.Location = New System.Drawing.Point(8, 37)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(106, 13)
        Me.Label37.TabIndex = 0
        Me.Label37.Text = "WHTC coldstart total"
        '
        'Label38
        '
        Me.Label38.AutoSize = true
        Me.Label38.Location = New System.Drawing.Point(391, 23)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(72, 13)
        Me.Label38.TabIndex = 0
        Me.Label38.Text = "WHTC-Urban"
        '
        'Label39
        '
        Me.Label39.AutoSize = true
        Me.Label39.Location = New System.Drawing.Point(566, 78)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(47, 13)
        Me.Label39.TabIndex = 2
        Me.Label39.Text = "[g/kWh]"
        '
        'Label40
        '
        Me.Label40.AutoSize = true
        Me.Label40.Location = New System.Drawing.Point(374, 78)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(89, 13)
        Me.Label40.TabIndex = 0
        Me.Label40.Text = "WHTC-Motorway"
        '
        'Label41
        '
        Me.Label41.AutoSize = true
        Me.Label41.Location = New System.Drawing.Point(217, 78)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(47, 13)
        Me.Label41.TabIndex = 2
        Me.Label41.Text = "[g/kWh]"
        '
        'TbFCspecUrb2
        '
        Me.TbFCspecUrb2.Location = New System.Drawing.Point(469, 19)
        Me.TbFCspecUrb2.Name = "TbFCspecUrb2"
        Me.TbFCspecUrb2.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecUrb2.TabIndex = 2
        '
        'Label42
        '
        Me.Label42.AutoSize = true
        Me.Label42.Location = New System.Drawing.Point(217, 37)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(47, 13)
        Me.Label42.TabIndex = 2
        Me.Label42.Text = "[g/kWh]"
        '
        'SfFuelType2
        '
        Me.SfFuelType2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.SfFuelType2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SfFuelType2.FormattingEnabled = true
        Me.SfFuelType2.Items.AddRange(New Object() {"Diesel / CI", "Diesel B100 CI", "Ethanol / CI", "Petrol / PI", "Ethanol / PI", "LPG / PI", "Natural Gas / PI", "Natural Gas / CI"})
        Me.SfFuelType2.Location = New System.Drawing.Point(108, 6)
        Me.SfFuelType2.Name = "SfFuelType2"
        Me.SfFuelType2.Size = New System.Drawing.Size(167, 21)
        Me.SfFuelType2.TabIndex = 0
        '
        'Label43
        '
        Me.Label43.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label43.AutoSize = true
        Me.Label43.Location = New System.Drawing.Point(16, 11)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(83, 13)
        Me.Label43.TabIndex = 31
        Me.Label43.Text = "Type of test fuel"
        '
        'TbNCVfuel2
        '
        Me.TbNCVfuel2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TbNCVfuel2.Location = New System.Drawing.Point(108, 34)
        Me.TbNCVfuel2.Name = "TbNCVfuel2"
        Me.TbNCVfuel2.Size = New System.Drawing.Size(114, 20)
        Me.TbNCVfuel2.TabIndex = 1
        '
        'Label44
        '
        Me.Label44.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label44.AutoSize = true
        Me.Label44.Location = New System.Drawing.Point(18, 37)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(81, 13)
        Me.Label44.TabIndex = 32
        Me.Label44.Text = "NCV of test fuel"
        '
        'Label45
        '
        Me.Label45.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label45.AutoSize = true
        Me.Label45.Location = New System.Drawing.Point(228, 37)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(44, 13)
        Me.Label45.TabIndex = 33
        Me.Label45.Text = "[MJ/kg]"
        '
        'tbWHRMech
        '
        Me.tbWHRMech.Controls.Add(Me.GroupBox3)
        Me.tbWHRMech.Controls.Add(Me.GroupBox4)
        Me.tbWHRMech.Location = New System.Drawing.Point(4, 22)
        Me.tbWHRMech.Name = "tbWHRMech"
        Me.tbWHRMech.Padding = New System.Windows.Forms.Padding(3)
        Me.tbWHRMech.Size = New System.Drawing.Size(901, 189)
        Me.tbWHRMech.TabIndex = 2
        Me.tbWHRMech.Text = "WHR Mechanical"
        Me.tbWHRMech.UseVisualStyleBackColor = true
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.TbCF_RegPerWHRMech)
        Me.GroupBox3.Location = New System.Drawing.Point(663, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(214, 101)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = false
        Me.GroupBox3.Text = "Correction factors"
        '
        'Label15
        '
        Me.Label15.AutoSize = true
        Me.Label15.Location = New System.Drawing.Point(6, 50)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(59, 13)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "CF-RegPer"
        '
        'TbCF_RegPerWHRMech
        '
        Me.TbCF_RegPerWHRMech.Location = New System.Drawing.Point(71, 47)
        Me.TbCF_RegPerWHRMech.Name = "TbCF_RegPerWHRMech"
        Me.TbCF_RegPerWHRMech.Size = New System.Drawing.Size(111, 20)
        Me.TbCF_RegPerWHRMech.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.TbFCspecHotWHRMech)
        Me.GroupBox4.Controls.Add(Me.TbFCspecColdWHRMech)
        Me.GroupBox4.Controls.Add(Me.Label21)
        Me.GroupBox4.Controls.Add(Me.Label22)
        Me.GroupBox4.Controls.Add(Me.TbFCspecMWWHRMech)
        Me.GroupBox4.Controls.Add(Me.TbFCspecRurWHRMech)
        Me.GroupBox4.Controls.Add(Me.Label26)
        Me.GroupBox4.Controls.Add(Me.Label27)
        Me.GroupBox4.Controls.Add(Me.Label32)
        Me.GroupBox4.Controls.Add(Me.Label33)
        Me.GroupBox4.Controls.Add(Me.Label46)
        Me.GroupBox4.Controls.Add(Me.Label47)
        Me.GroupBox4.Controls.Add(Me.Label48)
        Me.GroupBox4.Controls.Add(Me.TbFCspecUrbWHRMech)
        Me.GroupBox4.Controls.Add(Me.Label49)
        Me.GroupBox4.Location = New System.Drawing.Point(11, 6)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(636, 101)
        Me.GroupBox4.TabIndex = 0
        Me.GroupBox4.TabStop = false
        Me.GroupBox4.Text = "Specific WHR work measured"
        '
        'TbFCspecHotWHRMech
        '
        Me.TbFCspecHotWHRMech.Location = New System.Drawing.Point(120, 75)
        Me.TbFCspecHotWHRMech.Name = "TbFCspecHotWHRMech"
        Me.TbFCspecHotWHRMech.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecHotWHRMech.TabIndex = 1
        '
        'TbFCspecColdWHRMech
        '
        Me.TbFCspecColdWHRMech.Location = New System.Drawing.Point(120, 34)
        Me.TbFCspecColdWHRMech.Name = "TbFCspecColdWHRMech"
        Me.TbFCspecColdWHRMech.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecColdWHRMech.TabIndex = 0
        '
        'Label21
        '
        Me.Label21.AutoSize = true
        Me.Label21.Location = New System.Drawing.Point(395, 50)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(68, 13)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "WHTC-Rural"
        '
        'Label22
        '
        Me.Label22.AutoSize = true
        Me.Label22.Location = New System.Drawing.Point(566, 22)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(52, 13)
        Me.Label22.TabIndex = 2
        Me.Label22.Text = "[kJ/kWh]"
        '
        'TbFCspecMWWHRMech
        '
        Me.TbFCspecMWWHRMech.Location = New System.Drawing.Point(469, 75)
        Me.TbFCspecMWWHRMech.Name = "TbFCspecMWWHRMech"
        Me.TbFCspecMWWHRMech.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecMWWHRMech.TabIndex = 4
        '
        'TbFCspecRurWHRMech
        '
        Me.TbFCspecRurWHRMech.Location = New System.Drawing.Point(469, 47)
        Me.TbFCspecRurWHRMech.Name = "TbFCspecRurWHRMech"
        Me.TbFCspecRurWHRMech.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecRurWHRMech.TabIndex = 3
        '
        'Label26
        '
        Me.Label26.AutoSize = true
        Me.Label26.Location = New System.Drawing.Point(13, 78)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(101, 13)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = "WHTC hotstart total"
        '
        'Label27
        '
        Me.Label27.AutoSize = true
        Me.Label27.Location = New System.Drawing.Point(566, 50)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(52, 13)
        Me.Label27.TabIndex = 2
        Me.Label27.Text = "[kJ/kWh]"
        '
        'Label32
        '
        Me.Label32.AutoSize = true
        Me.Label32.Location = New System.Drawing.Point(8, 37)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(106, 13)
        Me.Label32.TabIndex = 0
        Me.Label32.Text = "WHTC coldstart total"
        '
        'Label33
        '
        Me.Label33.AutoSize = true
        Me.Label33.Location = New System.Drawing.Point(391, 23)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(72, 13)
        Me.Label33.TabIndex = 0
        Me.Label33.Text = "WHTC-Urban"
        '
        'Label46
        '
        Me.Label46.AutoSize = true
        Me.Label46.Location = New System.Drawing.Point(566, 78)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(52, 13)
        Me.Label46.TabIndex = 2
        Me.Label46.Text = "[kJ/kWh]"
        '
        'Label47
        '
        Me.Label47.AutoSize = true
        Me.Label47.Location = New System.Drawing.Point(374, 78)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(89, 13)
        Me.Label47.TabIndex = 0
        Me.Label47.Text = "WHTC-Motorway"
        '
        'Label48
        '
        Me.Label48.AutoSize = true
        Me.Label48.Location = New System.Drawing.Point(217, 78)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(52, 13)
        Me.Label48.TabIndex = 2
        Me.Label48.Text = "[kJ/kWh]"
        '
        'TbFCspecUrbWHRMech
        '
        Me.TbFCspecUrbWHRMech.Location = New System.Drawing.Point(469, 19)
        Me.TbFCspecUrbWHRMech.Name = "TbFCspecUrbWHRMech"
        Me.TbFCspecUrbWHRMech.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecUrbWHRMech.TabIndex = 2
        '
        'Label49
        '
        Me.Label49.AutoSize = true
        Me.Label49.Location = New System.Drawing.Point(217, 37)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(52, 13)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "[kJ/kWh]"
        '
        'tbWhrEl
        '
        Me.tbWhrEl.Controls.Add(Me.GroupBox8)
        Me.tbWhrEl.Controls.Add(Me.GroupBox9)
        Me.tbWhrEl.Location = New System.Drawing.Point(4, 22)
        Me.tbWhrEl.Name = "tbWhrEl"
        Me.tbWhrEl.Size = New System.Drawing.Size(901, 189)
        Me.tbWhrEl.TabIndex = 3
        Me.tbWhrEl.Text = "WHR Electrical"
        Me.tbWhrEl.UseVisualStyleBackColor = true
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.Label50)
        Me.GroupBox8.Controls.Add(Me.TbCF_RegPerWHREl)
        Me.GroupBox8.Location = New System.Drawing.Point(663, 6)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(214, 101)
        Me.GroupBox8.TabIndex = 1
        Me.GroupBox8.TabStop = false
        Me.GroupBox8.Text = "Correction factors"
        '
        'Label50
        '
        Me.Label50.AutoSize = true
        Me.Label50.Location = New System.Drawing.Point(6, 50)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(59, 13)
        Me.Label50.TabIndex = 3
        Me.Label50.Text = "CF-RegPer"
        '
        'TbCF_RegPerWHREl
        '
        Me.TbCF_RegPerWHREl.Location = New System.Drawing.Point(71, 47)
        Me.TbCF_RegPerWHREl.Name = "TbCF_RegPerWHREl"
        Me.TbCF_RegPerWHREl.Size = New System.Drawing.Size(111, 20)
        Me.TbCF_RegPerWHREl.TabIndex = 0
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.TbFCspecHotWHREl)
        Me.GroupBox9.Controls.Add(Me.TbFCspecColdWHREl)
        Me.GroupBox9.Controls.Add(Me.Label51)
        Me.GroupBox9.Controls.Add(Me.Label52)
        Me.GroupBox9.Controls.Add(Me.TbFCspecMWWHREl)
        Me.GroupBox9.Controls.Add(Me.TbFCspecRurWHREl)
        Me.GroupBox9.Controls.Add(Me.Label53)
        Me.GroupBox9.Controls.Add(Me.Label54)
        Me.GroupBox9.Controls.Add(Me.Label55)
        Me.GroupBox9.Controls.Add(Me.Label56)
        Me.GroupBox9.Controls.Add(Me.Label57)
        Me.GroupBox9.Controls.Add(Me.Label58)
        Me.GroupBox9.Controls.Add(Me.Label59)
        Me.GroupBox9.Controls.Add(Me.TbFCspecUrbWHREl)
        Me.GroupBox9.Controls.Add(Me.Label60)
        Me.GroupBox9.Location = New System.Drawing.Point(11, 6)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(636, 101)
        Me.GroupBox9.TabIndex = 0
        Me.GroupBox9.TabStop = false
        Me.GroupBox9.Text = "Specific WHR work measured"
        '
        'TbFCspecHotWHREl
        '
        Me.TbFCspecHotWHREl.Location = New System.Drawing.Point(120, 75)
        Me.TbFCspecHotWHREl.Name = "TbFCspecHotWHREl"
        Me.TbFCspecHotWHREl.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecHotWHREl.TabIndex = 1
        '
        'TbFCspecColdWHREl
        '
        Me.TbFCspecColdWHREl.Location = New System.Drawing.Point(120, 34)
        Me.TbFCspecColdWHREl.Name = "TbFCspecColdWHREl"
        Me.TbFCspecColdWHREl.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecColdWHREl.TabIndex = 0
        '
        'Label51
        '
        Me.Label51.AutoSize = true
        Me.Label51.Location = New System.Drawing.Point(395, 50)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(68, 13)
        Me.Label51.TabIndex = 0
        Me.Label51.Text = "WHTC-Rural"
        '
        'Label52
        '
        Me.Label52.AutoSize = true
        Me.Label52.Location = New System.Drawing.Point(566, 22)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(52, 13)
        Me.Label52.TabIndex = 2
        Me.Label52.Text = "[kJ/kWh]"
        '
        'TbFCspecMWWHREl
        '
        Me.TbFCspecMWWHREl.Location = New System.Drawing.Point(469, 75)
        Me.TbFCspecMWWHREl.Name = "TbFCspecMWWHREl"
        Me.TbFCspecMWWHREl.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecMWWHREl.TabIndex = 4
        '
        'TbFCspecRurWHREl
        '
        Me.TbFCspecRurWHREl.Location = New System.Drawing.Point(469, 47)
        Me.TbFCspecRurWHREl.Name = "TbFCspecRurWHREl"
        Me.TbFCspecRurWHREl.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecRurWHREl.TabIndex = 3
        '
        'Label53
        '
        Me.Label53.AutoSize = true
        Me.Label53.Location = New System.Drawing.Point(13, 78)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(101, 13)
        Me.Label53.TabIndex = 0
        Me.Label53.Text = "WHTC hotstart total"
        '
        'Label54
        '
        Me.Label54.AutoSize = true
        Me.Label54.Location = New System.Drawing.Point(566, 50)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(52, 13)
        Me.Label54.TabIndex = 2
        Me.Label54.Text = "[kJ/kWh]"
        '
        'Label55
        '
        Me.Label55.AutoSize = true
        Me.Label55.Location = New System.Drawing.Point(8, 37)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(106, 13)
        Me.Label55.TabIndex = 0
        Me.Label55.Text = "WHTC coldstart total"
        '
        'Label56
        '
        Me.Label56.AutoSize = true
        Me.Label56.Location = New System.Drawing.Point(391, 23)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(72, 13)
        Me.Label56.TabIndex = 0
        Me.Label56.Text = "WHTC-Urban"
        '
        'Label57
        '
        Me.Label57.AutoSize = true
        Me.Label57.Location = New System.Drawing.Point(566, 78)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(52, 13)
        Me.Label57.TabIndex = 2
        Me.Label57.Text = "[kJ/kWh]"
        '
        'Label58
        '
        Me.Label58.AutoSize = true
        Me.Label58.Location = New System.Drawing.Point(374, 78)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(89, 13)
        Me.Label58.TabIndex = 0
        Me.Label58.Text = "WHTC-Motorway"
        '
        'Label59
        '
        Me.Label59.AutoSize = true
        Me.Label59.Location = New System.Drawing.Point(217, 78)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(52, 13)
        Me.Label59.TabIndex = 2
        Me.Label59.Text = "[kJ/kWh]"
        '
        'TbFCspecUrbWHREl
        '
        Me.TbFCspecUrbWHREl.Location = New System.Drawing.Point(469, 19)
        Me.TbFCspecUrbWHREl.Name = "TbFCspecUrbWHREl"
        Me.TbFCspecUrbWHREl.Size = New System.Drawing.Size(91, 20)
        Me.TbFCspecUrbWHREl.TabIndex = 2
        '
        'Label60
        '
        Me.Label60.AutoSize = true
        Me.Label60.Location = New System.Drawing.Point(217, 37)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(52, 13)
        Me.Label60.TabIndex = 2
        Me.Label60.Text = "[kJ/kWh]"
        '
        'Label16
        '
        Me.Label16.AutoSize = true
        Me.Label16.Location = New System.Drawing.Point(6, 26)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(160, 13)
        Me.Label16.TabIndex = 33
        Me.Label16.Text = "Idle speed of CO2-parent engine"
        '
        'Label18
        '
        Me.Label18.AutoSize = true
        Me.Label18.Location = New System.Drawing.Point(268, 53)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(40, 13)
        Me.Label18.TabIndex = 37
        Me.Label18.Text = "[1/min]"
        '
        'TbIdle_Parent
        '
        Me.TbIdle_Parent.Location = New System.Drawing.Point(171, 23)
        Me.TbIdle_Parent.Name = "TbIdle_Parent"
        Me.TbIdle_Parent.Size = New System.Drawing.Size(91, 20)
        Me.TbIdle_Parent.TabIndex = 0
        '
        'Label24
        '
        Me.Label24.AutoSize = true
        Me.Label24.Location = New System.Drawing.Point(71, 53)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(91, 13)
        Me.Label24.TabIndex = 36
        Me.Label24.Text = "Engine idle speed"
        '
        'Label13
        '
        Me.Label13.AutoSize = true
        Me.Label13.Location = New System.Drawing.Point(268, 26)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(40, 13)
        Me.Label13.TabIndex = 32
        Me.Label13.Text = "[1/min]"
        '
        'TbIdle
        '
        Me.TbIdle.Location = New System.Drawing.Point(171, 50)
        Me.TbIdle.Name = "TbIdle"
        Me.TbIdle.Size = New System.Drawing.Size(91, 20)
        Me.TbIdle.TabIndex = 1
        '
        'ComponentData
        '
        Me.ComponentData.Controls.Add(Me.GroupBox10)
        Me.ComponentData.Controls.Add(Me.Label31)
        Me.ComponentData.Controls.Add(Me.EngineRatedSpeedLabel)
        Me.ComponentData.Controls.Add(Me.TbRatedSpeed)
        Me.ComponentData.Controls.Add(Me.Label23)
        Me.ComponentData.Controls.Add(Me.EngineRatedPowerLabel)
        Me.ComponentData.Controls.Add(Me.cbDualFuel)
        Me.ComponentData.Controls.Add(Me.TbRatedPower)
        Me.ComponentData.Controls.Add(Me.CertificationNumberLabel)
        Me.ComponentData.Controls.Add(Me.TbCertNumber)
        Me.ComponentData.Controls.Add(Me.ModelLabel)
        Me.ComponentData.Controls.Add(Me.TbModel)
        Me.ComponentData.Controls.Add(Me.ManufacturerLabel)
        Me.ComponentData.Controls.Add(Me.TbManufacturer)
        Me.ComponentData.Controls.Add(Me.Label25)
        Me.ComponentData.Controls.Add(Me.EngineDisplacementLabel)
        Me.ComponentData.Controls.Add(Me.TbDisplacement)
        Me.ComponentData.Location = New System.Drawing.Point(6, 48)
        Me.ComponentData.Name = "ComponentData"
        Me.ComponentData.Size = New System.Drawing.Size(694, 154)
        Me.ComponentData.TabIndex = 1
        Me.ComponentData.TabStop = false
        Me.ComponentData.Text = "Engine component data"
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.cbWHREl)
        Me.GroupBox10.Controls.Add(Me.cbWhrICE)
        Me.GroupBox10.Controls.Add(Me.cbWHRMech)
        Me.GroupBox10.Location = New System.Drawing.Point(112, 105)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(488, 44)
        Me.GroupBox10.TabIndex = 8
        Me.GroupBox10.TabStop = false
        Me.GroupBox10.Text = "WHR data"
        '
        'cbWHREl
        '
        Me.cbWHREl.AutoSize = true
        Me.cbWHREl.Location = New System.Drawing.Point(356, 19)
        Me.cbWHREl.Name = "cbWHREl"
        Me.cbWHREl.Size = New System.Drawing.Size(104, 17)
        Me.cbWHREl.TabIndex = 2
        Me.cbWHREl.Text = "Electrical Output"
        Me.cbWHREl.UseVisualStyleBackColor = true
        '
        'cbWhrICE
        '
        Me.cbWhrICE.AutoSize = true
        Me.cbWhrICE.Location = New System.Drawing.Point(10, 19)
        Me.cbWhrICE.Name = "cbWhrICE"
        Me.cbWhrICE.Size = New System.Drawing.Size(130, 17)
        Me.cbWhrICE.TabIndex = 0
        Me.cbWhrICE.Text = "MechanicalOutputICE"
        Me.cbWhrICE.UseVisualStyleBackColor = true
        '
        'cbWHRMech
        '
        Me.cbWHRMech.AutoSize = true
        Me.cbWHRMech.Location = New System.Drawing.Point(164, 19)
        Me.cbWHRMech.Name = "cbWHRMech"
        Me.cbWHRMech.Size = New System.Drawing.Size(162, 17)
        Me.cbWHRMech.TabIndex = 1
        Me.cbWHRMech.Text = "MechanicalOutputDriveTrain"
        Me.cbWHRMech.UseVisualStyleBackColor = true
        '
        'Label31
        '
        Me.Label31.AutoSize = true
        Me.Label31.Location = New System.Drawing.Point(606, 74)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(40, 13)
        Me.Label31.TabIndex = 30
        Me.Label31.Text = "[1/min]"
        '
        'EngineRatedSpeedLabel
        '
        Me.EngineRatedSpeedLabel.AutoSize = true
        Me.EngineRatedSpeedLabel.Location = New System.Drawing.Point(401, 74)
        Me.EngineRatedSpeedLabel.Name = "EngineRatedSpeedLabel"
        Me.EngineRatedSpeedLabel.Size = New System.Drawing.Size(99, 13)
        Me.EngineRatedSpeedLabel.TabIndex = 29
        Me.EngineRatedSpeedLabel.Text = "Engine rated speed"
        '
        'TbRatedSpeed
        '
        Me.TbRatedSpeed.Location = New System.Drawing.Point(509, 71)
        Me.TbRatedSpeed.Name = "TbRatedSpeed"
        Me.TbRatedSpeed.Size = New System.Drawing.Size(91, 20)
        Me.TbRatedSpeed.TabIndex = 6
        '
        'Label23
        '
        Me.Label23.AutoSize = true
        Me.Label23.Location = New System.Drawing.Point(606, 48)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(30, 13)
        Me.Label23.TabIndex = 27
        Me.Label23.Text = "[kW]"
        '
        'EngineRatedPowerLabel
        '
        Me.EngineRatedPowerLabel.AutoSize = true
        Me.EngineRatedPowerLabel.Location = New System.Drawing.Point(401, 48)
        Me.EngineRatedPowerLabel.Name = "EngineRatedPowerLabel"
        Me.EngineRatedPowerLabel.Size = New System.Drawing.Size(99, 13)
        Me.EngineRatedPowerLabel.TabIndex = 26
        Me.EngineRatedPowerLabel.Text = "Engine rated power"
        '
        'cbDualFuel
        '
        Me.cbDualFuel.AutoSize = true
        Me.cbDualFuel.Location = New System.Drawing.Point(15, 124)
        Me.cbDualFuel.Name = "cbDualFuel"
        Me.cbDualFuel.Size = New System.Drawing.Size(71, 17)
        Me.cbDualFuel.TabIndex = 7
        Me.cbDualFuel.Text = "Dual Fuel"
        Me.cbDualFuel.UseVisualStyleBackColor = true
        '
        'TbRatedPower
        '
        Me.TbRatedPower.Location = New System.Drawing.Point(509, 45)
        Me.TbRatedPower.Name = "TbRatedPower"
        Me.TbRatedPower.Size = New System.Drawing.Size(91, 20)
        Me.TbRatedPower.TabIndex = 5
        '
        'CertificationNumberLabel
        '
        Me.CertificationNumberLabel.AutoSize = true
        Me.CertificationNumberLabel.Location = New System.Drawing.Point(12, 77)
        Me.CertificationNumberLabel.Name = "CertificationNumberLabel"
        Me.CertificationNumberLabel.Size = New System.Drawing.Size(102, 13)
        Me.CertificationNumberLabel.TabIndex = 24
        Me.CertificationNumberLabel.Text = "Certification Number"
        '
        'TbCertNumber
        '
        Me.TbCertNumber.Location = New System.Drawing.Point(120, 74)
        Me.TbCertNumber.Name = "TbCertNumber"
        Me.TbCertNumber.Size = New System.Drawing.Size(260, 20)
        Me.TbCertNumber.TabIndex = 3
        '
        'ModelLabel
        '
        Me.ModelLabel.AutoSize = true
        Me.ModelLabel.Location = New System.Drawing.Point(78, 51)
        Me.ModelLabel.Name = "ModelLabel"
        Me.ModelLabel.Size = New System.Drawing.Size(36, 13)
        Me.ModelLabel.TabIndex = 15
        Me.ModelLabel.Text = "Model"
        '
        'TbModel
        '
        Me.TbModel.Location = New System.Drawing.Point(120, 48)
        Me.TbModel.Name = "TbModel"
        Me.TbModel.Size = New System.Drawing.Size(260, 20)
        Me.TbModel.TabIndex = 2
        '
        'ManufacturerLabel
        '
        Me.ManufacturerLabel.AutoSize = true
        Me.ManufacturerLabel.Location = New System.Drawing.Point(44, 25)
        Me.ManufacturerLabel.Name = "ManufacturerLabel"
        Me.ManufacturerLabel.Size = New System.Drawing.Size(70, 13)
        Me.ManufacturerLabel.TabIndex = 8
        Me.ManufacturerLabel.Text = "Manufacturer"
        '
        'TbManufacturer
        '
        Me.TbManufacturer.Location = New System.Drawing.Point(120, 22)
        Me.TbManufacturer.Name = "TbManufacturer"
        Me.TbManufacturer.Size = New System.Drawing.Size(260, 20)
        Me.TbManufacturer.TabIndex = 1
        '
        'Label25
        '
        Me.Label25.AutoSize = true
        Me.Label25.Location = New System.Drawing.Point(606, 22)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(33, 13)
        Me.Label25.TabIndex = 10
        Me.Label25.Text = "[ccm]"
        '
        'EngineDisplacementLabel
        '
        Me.EngineDisplacementLabel.AutoSize = true
        Me.EngineDisplacementLabel.Location = New System.Drawing.Point(395, 22)
        Me.EngineDisplacementLabel.Name = "EngineDisplacementLabel"
        Me.EngineDisplacementLabel.Size = New System.Drawing.Size(105, 13)
        Me.EngineDisplacementLabel.TabIndex = 9
        Me.EngineDisplacementLabel.Text = "Engine displacement"
        '
        'TbDisplacement
        '
        Me.TbDisplacement.Location = New System.Drawing.Point(509, 19)
        Me.TbDisplacement.Name = "TbDisplacement"
        Me.TbDisplacement.Size = New System.Drawing.Size(91, 20)
        Me.TbDisplacement.TabIndex = 4
        '
        'VECTOLogo
        '
        Me.VECTOLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.VECTOLogo.Image = Global.VECTO_Engine.My.Resources.Resources.VectoEngine_6_cropV164pix
        Me.VECTOLogo.Location = New System.Drawing.Point(724, 70)
        Me.VECTOLogo.Name = "VECTOLogo"
        Me.VECTOLogo.Size = New System.Drawing.Size(206, 132)
        Me.VECTOLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.VECTOLogo.TabIndex = 11
        Me.VECTOLogo.TabStop = false
        '
        'GrOutput
        '
        Me.GrOutput.Controls.Add(Me.BtCopyToClipboard)
        Me.GrOutput.Controls.Add(Me.LvMsg)
        Me.GrOutput.Controls.Add(Me.TbOutputFolder)
        Me.GrOutput.Controls.Add(Me.BtOutputDirectory)
        Me.GrOutput.Controls.Add(Me.Label14)
        Me.GrOutput.Location = New System.Drawing.Point(12, 689)
        Me.GrOutput.Name = "GrOutput"
        Me.GrOutput.Size = New System.Drawing.Size(941, 210)
        Me.GrOutput.TabIndex = 1
        Me.GrOutput.TabStop = false
        Me.GrOutput.Text = "Output"
        '
        'BtCopyToClipboard
        '
        Me.BtCopyToClipboard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.BtCopyToClipboard.Location = New System.Drawing.Point(6, 174)
        Me.BtCopyToClipboard.Name = "BtCopyToClipboard"
        Me.BtCopyToClipboard.Size = New System.Drawing.Size(211, 27)
        Me.BtCopyToClipboard.TabIndex = 4
        Me.BtCopyToClipboard.TabStop = false
        Me.BtCopyToClipboard.Text = "Copy all messages to clipboard"
        Me.BtCopyToClipboard.UseVisualStyleBackColor = true
        '
        'LvMsg
        '
        Me.LvMsg.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.LvMsg.FullRowSelect = true
        Me.LvMsg.GridLines = true
        Me.LvMsg.HideSelection = false
        Me.LvMsg.LabelWrap = false
        Me.LvMsg.Location = New System.Drawing.Point(5, 47)
        Me.LvMsg.Name = "LvMsg"
        Me.LvMsg.Size = New System.Drawing.Size(925, 121)
        Me.LvMsg.TabIndex = 2
        Me.LvMsg.UseCompatibleStateImageBehavior = false
        Me.LvMsg.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Message window"
        Me.ColumnHeader1.Width = 921
        '
        'TbOutputFolder
        '
        Me.TbOutputFolder.Location = New System.Drawing.Point(146, 17)
        Me.TbOutputFolder.Name = "TbOutputFolder"
        Me.TbOutputFolder.Size = New System.Drawing.Size(750, 20)
        Me.TbOutputFolder.TabIndex = 0
        '
        'BtOutputDirectory
        '
        Me.BtOutputDirectory.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.BtOutputDirectory.Location = New System.Drawing.Point(902, 15)
        Me.BtOutputDirectory.Name = "BtOutputDirectory"
        Me.BtOutputDirectory.Size = New System.Drawing.Size(28, 23)
        Me.BtOutputDirectory.TabIndex = 1
        Me.BtOutputDirectory.TabStop = false
        Me.BtOutputDirectory.UseVisualStyleBackColor = true
        '
        'Label14
        '
        Me.Label14.AutoSize = true
        Me.Label14.Location = New System.Drawing.Point(56, 20)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(84, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Output Directory"
        '
        'BtStart
        '
        Me.BtStart.BackColor = System.Drawing.Color.Lime
        Me.BtStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtStart.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.BtStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.BtStart.Location = New System.Drawing.Point(12, 644)
        Me.BtStart.Name = "BtStart"
        Me.BtStart.Size = New System.Drawing.Size(639, 39)
        Me.BtStart.TabIndex = 3
        Me.BtStart.TabStop = false
        Me.BtStart.Text = "START FULL DATA EVALUATION"
        Me.BtStart.UseVisualStyleBackColor = false
        '
        'BgWorker
        '
        Me.BgWorker.WorkerReportsProgress = true
        Me.BgWorker.WorkerSupportsCancellation = true
        '
        'BtPrecalc
        '
        Me.BtPrecalc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.BtPrecalc.Location = New System.Drawing.Point(668, 644)
        Me.BtPrecalc.Name = "BtPrecalc"
        Me.BtPrecalc.Size = New System.Drawing.Size(285, 39)
        Me.BtPrecalc.TabIndex = 2
        Me.BtPrecalc.TabStop = false
        Me.BtPrecalc.Text = "Precalculate characteristic engine speeds"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"and grid for fuel map"
        Me.BtPrecalc.UseVisualStyleBackColor = true
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = true
        Me.ClientSize = New System.Drawing.Size(974, 911)
        Me.Controls.Add(Me.BtPrecalc)
        Me.Controls.Add(Me.GrOutput)
        Me.Controls.Add(Me.BtStart)
        Me.Controls.Add(Me.GrInput)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MaximizeBox = false
        Me.MaximumSize = New System.Drawing.Size(990, 950)
        Me.MinimumSize = New System.Drawing.Size(990, 500)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.GrInput.ResumeLayout(false)
        Me.GrInput.PerformLayout
        Me.GroupBox5.ResumeLayout(false)
        Me.GroupBox5.PerformLayout
        Me.GroupBox2.ResumeLayout(false)
        Me.GroupBox2.PerformLayout
        Me.TabControl1.ResumeLayout(false)
        Me.Fuel1.ResumeLayout(false)
        Me.Fuel1.PerformLayout
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        Me.GrRpm.ResumeLayout(false)
        Me.GrRpm.PerformLayout
        Me.Fuel2.ResumeLayout(false)
        Me.Fuel2.PerformLayout
        Me.GroupBox6.ResumeLayout(false)
        Me.GroupBox6.PerformLayout
        Me.GroupBox7.ResumeLayout(false)
        Me.GroupBox7.PerformLayout
        Me.tbWHRMech.ResumeLayout(false)
        Me.GroupBox3.ResumeLayout(false)
        Me.GroupBox3.PerformLayout
        Me.GroupBox4.ResumeLayout(false)
        Me.GroupBox4.PerformLayout
        Me.tbWhrEl.ResumeLayout(false)
        Me.GroupBox8.ResumeLayout(false)
        Me.GroupBox8.PerformLayout
        Me.GroupBox9.ResumeLayout(false)
        Me.GroupBox9.PerformLayout
        Me.ComponentData.ResumeLayout(false)
        Me.ComponentData.PerformLayout
        Me.GroupBox10.ResumeLayout(false)
        Me.GroupBox10.PerformLayout
        CType(Me.VECTOLogo,System.ComponentModel.ISupportInitialize).EndInit
        Me.GrOutput.ResumeLayout(false)
        Me.GrOutput.PerformLayout
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents GrInput As GroupBox
    Friend WithEvents GrOutput As GroupBox
    Friend WithEvents BtStart As Button
    Friend WithEvents BgWorker As BackgroundWorker
    Friend WithEvents LvMsg As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents TbOutputFolder As TextBox
    Friend WithEvents BtOutputDirectory As Button
    Friend WithEvents Label14 As Label
    Friend WithEvents VECTOLogo As PictureBox
    Friend WithEvents BtPrecalc As Button
    Friend WithEvents ComponentData As GroupBox
    Friend WithEvents Label25 As Label
    Friend WithEvents EngineDisplacementLabel As Label
    Friend WithEvents TbDisplacement As TextBox
    Friend WithEvents CertificationNumberLabel As Label
    Friend WithEvents TbCertNumber As TextBox
    Friend WithEvents ModelLabel As Label
    Friend WithEvents TbModel As TextBox
    Friend WithEvents ManufacturerLabel As Label
    Friend WithEvents TbManufacturer As TextBox
    Friend WithEvents Label31 As Label
    Friend WithEvents EngineRatedSpeedLabel As Label
    Friend WithEvents TbRatedSpeed As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents EngineRatedPowerLabel As Label
    Friend WithEvents TbRatedPower As TextBox
    Friend WithEvents BtCopyToClipboard As Button
    Friend WithEvents JobFileLabel As Label
    Friend WithEvents cbDualFuel As CheckBox
    Friend WithEvents JobFileButton As Button
    Friend WithEvents TbJobFilePath As TextBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents Fuel1 As TabPage
    Friend WithEvents SfFuelType1 As ComboBox
    Friend WithEvents CbFuelType As Label
    Friend WithEvents TbNCVfuel1 As TextBox
    Friend WithEvents TbNCVfuel As Label
    Friend WithEvents Label30 As Label
    Friend WithEvents Fuel2 As TabPage
    Friend WithEvents Label16 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents TbIdle_Parent As TextBox
    Friend WithEvents Label24 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents TbIdle As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents TbFLC As TextBox
    Friend WithEvents BtFLC As Button
    Friend WithEvents TbMotoring As TextBox
    Friend WithEvents BtMW As Button
    Friend WithEvents TbFLC_Parent As TextBox
    Friend WithEvents BtFLCParent As Button
    Friend WithEvents TbFuelMap As TextBox
    Friend WithEvents BtOpenMap As Button
    Friend WithEvents tbWHRMech As TabPage
    Friend WithEvents tbWhrEl As TabPage
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label17 As Label
    Friend WithEvents TbCF_RegPer1 As TextBox
    Friend WithEvents GrRpm As GroupBox
    Friend WithEvents TbFCspecHot1 As TextBox
    Friend WithEvents TbFCspecCold1 As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents TbFCspecMW1 As TextBox
    Friend WithEvents TbFCspecRur1 As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents TbFCspecUrb1 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents Label28 As Label
    Friend WithEvents TbCF_RegPer2 As TextBox
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents TbFCspecHot2 As TextBox
    Friend WithEvents TbFCspecCold2 As TextBox
    Friend WithEvents Label29 As Label
    Friend WithEvents Label34 As Label
    Friend WithEvents TbFCspecMW2 As TextBox
    Friend WithEvents TbFCspecRur2 As TextBox
    Friend WithEvents Label35 As Label
    Friend WithEvents Label36 As Label
    Friend WithEvents Label37 As Label
    Friend WithEvents Label38 As Label
    Friend WithEvents Label39 As Label
    Friend WithEvents Label40 As Label
    Friend WithEvents Label41 As Label
    Friend WithEvents TbFCspecUrb2 As TextBox
    Friend WithEvents Label42 As Label
    Friend WithEvents SfFuelType2 As ComboBox
    Friend WithEvents Label43 As Label
    Friend WithEvents TbNCVfuel2 As TextBox
    Friend WithEvents Label44 As Label
    Friend WithEvents Label45 As Label
    Friend WithEvents Save As Button
    Friend WithEvents cbWHREl As CheckBox
    Friend WithEvents cbWHRMech As CheckBox
    Friend WithEvents cbWhrICE As CheckBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label15 As Label
    Friend WithEvents TbCF_RegPerWHRMech As TextBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents TbFCspecHotWHRMech As TextBox
    Friend WithEvents TbFCspecColdWHRMech As TextBox
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents TbFCspecMWWHRMech As TextBox
    Friend WithEvents TbFCspecRurWHRMech As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label32 As Label
    Friend WithEvents Label33 As Label
    Friend WithEvents Label46 As Label
    Friend WithEvents Label47 As Label
    Friend WithEvents Label48 As Label
    Friend WithEvents TbFCspecUrbWHRMech As TextBox
    Friend WithEvents Label49 As Label
    Friend WithEvents GroupBox8 As GroupBox
    Friend WithEvents Label50 As Label
    Friend WithEvents TbCF_RegPerWHREl As TextBox
    Friend WithEvents GroupBox9 As GroupBox
    Friend WithEvents TbFCspecHotWHREl As TextBox
    Friend WithEvents TbFCspecColdWHREl As TextBox
    Friend WithEvents Label51 As Label
    Friend WithEvents Label52 As Label
    Friend WithEvents TbFCspecMWWHREl As TextBox
    Friend WithEvents TbFCspecRurWHREl As TextBox
    Friend WithEvents Label53 As Label
    Friend WithEvents Label54 As Label
    Friend WithEvents Label55 As Label
    Friend WithEvents Label56 As Label
    Friend WithEvents Label57 As Label
    Friend WithEvents Label58 As Label
    Friend WithEvents Label59 As Label
    Friend WithEvents TbFCspecUrbWHREl As TextBox
    Friend WithEvents Label60 As Label
    Friend WithEvents GroupBox10 As GroupBox
End Class
