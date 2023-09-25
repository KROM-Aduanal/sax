Imports Gsol.BaseDatos.Operaciones



Public Class TextBoxDCKR


#Region "Attributes"

    Private _controler As ComponentCriteriaDCKR

    Private _controlTitle As String = Nothing

#End Region

#Region "Properties"

    <System.ComponentModel.Category("DoubleChecker")> _
    <System.ComponentModel.Description("This controler let you access to control resources")> _
    Public Property Controler As ComponentCriteriaDCKR
        Get
            Return _controler
        End Get
        Set(value As ComponentCriteriaDCKR)
            _controler = value
        End Set
    End Property

    Public Property ControlTitle As String
        Get
            Return _controlTitle
        End Get
        Set(value As String)
            _controlTitle = value
            lblTitle.Text = _controlTitle
        End Set
    End Property

#End Region

#Region "Builders"


    Sub New()

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        _controler = New ComponentCriteriaDCKR(mtbValue1,
                                               mtbValue2,
                                               cbCriteria,
                                               chkIncluding,
                                               ICaracteristica.TiposCaracteristica.cString)

        rbtnSi.Visible = False
        rbtnNo.Visible = False

        lblTitle.Text = _controlTitle



        'Me.DataType = ICaracteristica.TiposCaracteristica.cBoolean

        '_controler.PrepareForDataType = ICaracteristica.TiposCaracteristica.cInt32

    End Sub

    Sub New(ByVal datatype_ As ICaracteristica.TiposCaracteristica)

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        Select Case datatype_

            Case ICaracteristica.TiposCaracteristica.cBoolean

                _controler = New ComponentCriteriaDCKR(rbtnSi,
                                                      rbtnNo,
                                                      cbCriteria,
                                                      chkIncluding,
                                                      datatype_)
                VerifyAlternativeRadios(datatype_)

            Case Else
                _controler = New ComponentCriteriaDCKR(mtbValue1,
                                                       mtbValue2,
                                                       cbCriteria,
                                                       chkIncluding,
                                                       datatype_)

        End Select

        'Me.DataType = ICaracteristica.TiposCaracteristica.cBoolean

        '_controler.PrepareForDataType = ICaracteristica.TiposCaracteristica.cInt32

    End Sub

#End Region

#Region "Properties"

    <System.ComponentModel.Category("DoubleChecker")> _
    <System.ComponentModel.Description("Change DataType, this component currently do not support boolean type")> _
    Public Property DataType As ICaracteristica.TiposCaracteristica
        Get
            Return _controler.PrepareForDataType
        End Get
        Set(value As ICaracteristica.TiposCaracteristica)

            _controler.PrepareForDataType = value
            VerifyAlternativeRadios(value)

        End Set
    End Property

#End Region

#Region "Methods"

    Private Sub ChangeControlerCriterion(ByVal iuoption_ As String)

        Select Case iuoption_

            Case "Igual a..." : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.EqualsTo
            Case "Es Mayor a..." : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.GreaterThan
            Case "Es Menor a..." : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.MinorThan
            Case "Es Diferente de..." : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.IsDifferentOf
            Case "Valor entre..." : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.BetweenValues
            Case "Comienza con..." : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.StartWith
            Case "Termina con..." : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.EndsWith
            Case "Contiene..." : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.Contains
            Case Else : _controler.Criterion = ComponentCriteriaDCKR.TypesCriteria.Undefined

        End Select

    End Sub


    Private Sub cbCriteria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCriteria.SelectedIndexChanged

        Select Case cbCriteria.SelectedIndex

            Case 4
                mtbValue2.Visible = True
                lblSeparator.Visible = True
                mtbValue1.Width = 76
            Case Else
                mtbValue2.Visible = False
                lblSeparator.Visible = False
                mtbValue1.Width = 171
        End Select

        ChangeControlerCriterion(cbCriteria.SelectedItem)

        'MsgBox("criteria:" & _controler.Criterion.ToString & ", incl:" & _controler.IsIncluded.ToString)
        ' MsgBox(_controler.ControlValue1 & "," & _controler.ControlValue2)

    End Sub

    Private Sub chkIncluding_CheckedChanged(sender As Object, e As EventArgs) Handles chkIncluding.CheckedChanged
        If chkIncluding.Checked Then
            mtbValue1.Enabled = True
            mtbValue2.Enabled = True
            cbCriteria.Enabled = True

            rbtnSi.Enabled = True
            rbtnNo.Enabled = True
            cbCriteria.SelectedIndex = 0
            Me.BackColor = Color.LightYellow

        Else
            mtbValue1.Enabled = False
            mtbValue2.Enabled = False
            cbCriteria.Enabled = False

            rbtnSi.Enabled = False
            rbtnNo.Enabled = False

            cbCriteria.SelectedIndex = -1
            Me.BackColor = Color.White
            Me.BorderStyle = Windows.Forms.BorderStyle.None

        End If

        _controler.IsIncluded = chkIncluding.Checked

        'MsgBox("criteria:" & _controler.Criterion.ToString & ", incl:" & _controler.IsIncluded.ToString)

    End Sub

    Private Sub VerifyAlternativeRadios(ByVal datatype_ As ICaracteristica.TiposCaracteristica)
        Select Case datatype_
            Case ICaracteristica.TiposCaracteristica.cBoolean
                mtbValue1.Enabled = False
                mtbValue2.Enabled = False
                mtbValue1.Visible = False
                mtbValue2.Visible = False
                lblSeparator.Visible = False

                rbtnSi.Visible = True
                rbtnNo.Visible = True
            Case Else

                rbtnSi.Visible = False
                rbtnNo.Visible = False

        End Select

    End Sub

#End Region

End Class


