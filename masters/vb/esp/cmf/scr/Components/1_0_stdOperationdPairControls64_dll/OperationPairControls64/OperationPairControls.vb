Imports Gsol.BaseDatos.Operaciones
Imports System.Text
Imports System.Reflection.Emit

Namespace Wma.Components


    Public Class OperationdPairControls

#Region "Attributes"

        'Type
        Private _caracteristic As ICaracteristica

        'Auxiliares....

        'Título
        Private _controltitle As Object

        'Control contenido
        Private _controlcontent As Object

        'Tipo
        Private _typecharacteristic As ICaracteristica.TiposCaracteristica

        'Nombre técnico
        Private _technicalname As String


        'Caracteristicas válidas VE 09112020 MOP
        'VALORES dentro de "_caracteristic"
        '
        'Nombre,
        'Llave,
        'Longitud,
        'TipoDato,
        'Visible,
        'NombreColumna,
        'PuedeInsertar,
        'PuedeModificar,
        'ValorDefault,
        'TipoFiltro,
        'NameAsKey,
        'Interfaz


#End Region

#Region "Builders"

        Sub New()



            _controltitle = New Object

            _controlcontent = New Object

            _typecharacteristic = ICaracteristica.TiposCaracteristica.cUndefined

            _caracteristic = New CaracteristicaCatalogo

            _technicalname = Nothing

        End Sub

#End Region

#Region "Properties"

        Public Property TechnicalName As String
            Get
                Return _technicalname
            End Get
            Set(value As String)
                _technicalname = value
            End Set
        End Property

        Public Property Characteristic As ICaracteristica
            Get
                Return _caracteristic
            End Get
            Set(value As ICaracteristica)
                _caracteristic = value
            End Set
        End Property

        Public Property TypeCharacteristic As ICaracteristica.TiposCaracteristica
            Get
                Return _typecharacteristic
            End Get
            Set(value As ICaracteristica.TiposCaracteristica)
                _typecharacteristic = value
            End Set
        End Property
        Public Property ControlTitle As Object
            Get
                Return _controltitle
            End Get
            Set(value As Object)
                _controltitle = value

                '_controlcontent.ControlTitle = _controltitle.

                'DirectCast(_controlcontent.ControlTitle, Label).text = DirectCast(ControlTitle.ControlTitle, Label).Text




            End Set
        End Property

        Public Property ControlTitleDirect As String
            Get
                Return _controlcontent.ControlTitle
            End Get
            Set(value As String)
                _controlcontent.ControlTitle = value

            End Set
        End Property

        Public Property ControlContent As Object

            Get

                Return _controlcontent

            End Get

            Set(value As Object)

                _controlcontent = value

            End Set

        End Property

#End Region

    End Class


End Namespace