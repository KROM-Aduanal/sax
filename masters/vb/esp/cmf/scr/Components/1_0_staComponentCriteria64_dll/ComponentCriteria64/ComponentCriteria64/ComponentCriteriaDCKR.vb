Imports Gsol.BaseDatos.Operaciones
Imports System.Windows.Forms


Public Class ComponentCriteriaDCKR

#Region "Enums"

    Public Enum TypesCriteria

        Undefined = 0

        EqualsTo = 1
        'Contains = 1

        GreaterThan = 2

        MinorThan = 3

        IsDifferentOf = 4

        BetweenValues = 5

        'Es igual a...
        'Es mayor que...
        'Es menor que...
        'Es diferente de...
        'Valor entre...

        StartWith = 6

        EndsWith = 7

        Contains = 8
        'EqualsTo = 8
        Void = 9

        NoEqualsTo = 10
        'Contains = 1

        NoGreaterThan = 11

        NoMinorThan = 12

        'NoIsDifferentOf = 13

        NoBetweenValues = 14

        'Es igual a...
        'Es mayor que...
        'Es menor que...
        'Es diferente de...
        'Valor entre...

        NoStartWith = 15

        NoEndsWith = 16

        NoContains = 17
        'EqualsTo = 8
        NoVoid = 18

        GreaterThanOrEquals = 19

        LessThanOrEquals = 20


    End Enum


    Public Enum TypesCriteriaSeparator
        Undefined = 0
        Adjuntion = 1
        Conjuntion = 2
    End Enum


#End Region

#Region "Attributes"

    Private _datatype As New ICaracteristica.TiposCaracteristica

    Private _criterion As TypesCriteria

    Private _icharacteristic As ICaracteristica

    'Private _value1 As String

    'Private _value2 As String

    Private _separator As TypesCriteriaSeparator

    Private _included As Boolean


    'Controls
    'Private _mtdContainer1 As MaskedTextBox
    'Private _mtdContainer2 As MaskedTextBox

    Private _mtdContainer1 As Object

    Private _mtdContainer2 As Object

    Private _cbCriterion As ComboBox

    Private _chkIncluded As CheckBox




#End Region

#Region "Builders"
    'Sub New(ByRef mtdContainer1_ As MaskedTextBox, _
    '        ByRef mtdContainer2_ As MaskedTextBox, _
    '        ByRef cbCriterion_ As ComboBox, _
    '        ByRef chkIncluded_ As CheckBox, _
    '        ByVal datatype_ As ICaracteristica.TiposCaracteristica)

    Sub New(ByRef mtdContainer1_ As Object, _
        ByRef mtdContainer2_ As Object, _
        ByRef cbCriterion_ As ComboBox, _
        ByRef chkIncluded_ As CheckBox, _
        ByVal datatype_ As ICaracteristica.TiposCaracteristica)


        _criterion = TypesCriteria.Undefined

        _icharacteristic = New CaracteristicaCatalogo


        '_value1 = Nothing

        '_value2 = Nothing

        _separator = TypesCriteriaSeparator.Undefined

        _included = False

        'Init
        _mtdContainer1 = New Object
        _mtdContainer2 = New Object
        _cbCriterion = New ComboBox
        _chkIncluded = New CheckBox

        _datatype = datatype_



        'Chatch current values
        _mtdContainer1 = mtdContainer1_
        _mtdContainer2 = mtdContainer2_
        _cbCriterion = cbCriterion_
        _chkIncluded = chkIncluded_


        'Changing types
        ChangeComboCriteria(datatype_)

    End Sub

    Sub New()

        _criterion = TypesCriteria.Undefined

        _icharacteristic = New CaracteristicaCatalogo

        '_value1 = Nothing

        '_value2 = Nothing

        _separator = TypesCriteriaSeparator.Undefined

        _included = False

        _datatype = ICaracteristica.TiposCaracteristica.cUndefined

        _mtdContainer1 = New Object
        _mtdContainer2 = New Object
        _cbCriterion = New ComboBox
        _chkIncluded = New CheckBox

    End Sub

#End Region

#Region "Propierties"

    Public Property PrepareForDataType As ICaracteristica.TiposCaracteristica
        Get
            Return _datatype
        End Get
        Set(value As ICaracteristica.TiposCaracteristica)
            _datatype = value

            PreparteEnviromentForDataType(value)

        End Set
    End Property

    Public Property IsIncluded As Boolean
        Get
            Return _chkIncluded.Checked
        End Get
        Set(value As Boolean)
            _chkIncluded.Checked = value

        End Set
    End Property

    Public Property SeparatorCriteria As TypesCriteriaSeparator
        Get
            Return _separator
        End Get
        Set(value As TypesCriteriaSeparator)
            _separator = value
        End Set
    End Property

    Public Property CharacteriticDCKR As ICaracteristica
        Get
            Return _icharacteristic
        End Get
        Set(value As ICaracteristica)
            _icharacteristic = value
        End Set
    End Property

    Public Property Criterion As TypesCriteria
        Get
            Return _criterion
        End Get
        Set(value As TypesCriteria)
            _criterion = value
        End Set
    End Property

    Private Function CorrectControlAnswer(ByVal control_ As Object) As String
        Dim answer_ As String = Nothing

        Select Case _datatype
            Case ICaracteristica.TiposCaracteristica.cBoolean

                answer_ = control_.Checked.ToString

            Case Else
                answer_ = control_.Text

        End Select

        Return answer_

    End Function


    Private Sub CorrectControlSet(ByVal control_ As Object, ByVal value_ As Object)
        Select Case _datatype
            Case ICaracteristica.TiposCaracteristica.cBoolean

                control_.Checked = Convert.ToBoolean(value_)

            Case Else
                control_.Text = value_

        End Select
    End Sub

    Public Property ControlValue1 As String
        Get
            Return CorrectControlAnswer(_mtdContainer1)
        End Get
        Set(value As String)
            '_mtdContainer1.Text = value
            CorrectControlSet(_mtdContainer1, value)
        End Set
    End Property

    Public Property ControlValue2 As String
        Get
            Return CorrectControlAnswer(_mtdContainer2)
        End Get
        Set(value As String)
            '_mtdContainer2.Text = value
            CorrectControlSet(_mtdContainer2, value)
        End Set
    End Property





#End Region

#Region "Methods"

    Private Sub ChangeComboCriteria(ByVal datatype_ As ICaracteristica.TiposCaracteristica)

        Select Case datatype_
            Case ICaracteristica.TiposCaracteristica.cString

                _cbCriterion.Items.Clear()

                _cbCriterion.Items.Add("Igual a...")
                'cbCriterion_.Items.Add("Es Mayor a...")
                'cbCriterion_.Items.Add("Es Menor a...")
                'cbCriterion_.Items.Add("Es Diferente de...")
                'cbCriterion_.Items.Add("Valor entre...")

                _cbCriterion.Items.Add("Comienza con...")
                _cbCriterion.Items.Add("Termina con...")
                _cbCriterion.Items.Add("Contiene...")


                'ChangeMaskControl()
                _mtdContainer1.Mask = Nothing
                _mtdContainer2.Mask = Nothing


            Case ICaracteristica.TiposCaracteristica.cBoolean
                _cbCriterion.Items.Clear()

                _cbCriterion.Items.Add("Igual a...")
                'cbCriterion_.Items.Add("Es Mayor a...")
                'cbCriterion_.Items.Add("Es Menor a...")
                'cbCriterion_.Items.Add("Es Diferente de...")
                'cbCriterion_.Items.Add("Valor entre...")
                'cbCriterion_.Items.Add("Comienza con...")
                'cbCriterion_.Items.Add("Termina con...")
                'cbCriterion_.Items.Add("Contiene...")

                'ChangeMaskControl()

                '_mtdContainer1.Mask = "9"
                '_mtdContainer2.Mask = "9"
                '_mtdContainer1.Checked = True
                _mtdContainer2.Checked = True



                'MsgBox("Currently does nto support radio button control, please Use RadioButtonDCKR ( Double checker component)!")


            Case ICaracteristica.TiposCaracteristica.cInt32
                _cbCriterion.Items.Clear()

                _cbCriterion.Items.Add("Igual a...")
                _cbCriterion.Items.Add("Es Mayor a...")
                _cbCriterion.Items.Add("Es Menor a...")
                _cbCriterion.Items.Add("Es Diferente de...")
                _cbCriterion.Items.Add("Valor entre...")
                'cbCriterion_.Items.Add("Comienza con...")
                'cbCriterion_.Items.Add("Termina con...")
                'cbCriterion_.Items.Add("Contiene...")

                'ChangeMaskControl()
                _mtdContainer1.Mask = "999999999"
                _mtdContainer2.Mask = "999999999"



            Case ICaracteristica.TiposCaracteristica.cReal
                _cbCriterion.Items.Clear()

                _cbCriterion.Items.Add("Igual a...")
                _cbCriterion.Items.Add("Es Mayor a...")
                _cbCriterion.Items.Add("Es Menor a...")
                _cbCriterion.Items.Add("Es Diferente de...")
                _cbCriterion.Items.Add("Valor entre...")
                'cbCriterion_.Items.Add("Comienza con...")
                'cbCriterion_.Items.Add("Termina con...")
                'cbCriterion_.Items.Add("Contiene...")

                _mtdContainer1.Mask = "9999999.99"
                _mtdContainer2.Mask = "9999999.99"

                'ChangeMaskControl()



            Case ICaracteristica.TiposCaracteristica.cDateTime
                _cbCriterion.Items.Clear()

                _cbCriterion.Items.Add("Igual a...")
                _cbCriterion.Items.Add("Es Mayor a...")
                _cbCriterion.Items.Add("Es Menor a...")
                _cbCriterion.Items.Add("Es Diferente de...")
                _cbCriterion.Items.Add("Valor entre...")

                'Not used
                '_mtdContainer1.Mask = "00/00/0000"
                '_mtdContainer2.Mask = "00/00/0000"

                'cbCriterion_.Items.Add("Comienza con...")
                'cbCriterion_.Items.Add("Termina con...")
                'cbCriterion_.Items.Add("Contiene...")

                'ChangeMaskControl()


            Case Else
                MsgBox("datatype_ unsupported")

        End Select

    End Sub



    Private Sub PreparteEnviromentForDataType(ByVal datatype_ As ICaracteristica.TiposCaracteristica)
        'Changing combobox items, adhoc for new criterion
        ChangeComboCriteria(datatype_)




    End Sub

#End Region

End Class