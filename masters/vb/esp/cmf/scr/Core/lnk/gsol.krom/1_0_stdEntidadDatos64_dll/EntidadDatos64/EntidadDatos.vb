Imports System.Threading.Tasks
Imports Syn.Documento
Imports Wma.Exceptions

Namespace gsol.krom

    Public Class EntidadDatos
        Implements IEntidadDatos, ICloneable, IDisposable

#Region "Atributos"

        Private _dimension As IEnlaceDatos.TiposDimension

        Private _listaAtributos As List(Of CampoVirtual)

        Private _workRow As DataRow

        Private _status As TagWatcher

        Private _clausulas As String

        Private _orden As Int32 = 0

        Private _keyValues As New List(Of String)

#End Region

#Region "Constructores"

        Sub New()

            _dimension = IEnlaceDatos.TiposDimension.SinDefinir

            _listaAtributos = New List(Of CampoVirtual)

            _status = New TagWatcher

        End Sub

#End Region

#Region "Propiedades"




        'Public Property OperacionGenerica As OperacionGenerica Implements IEntidadDatos.OperacionGenerica
        '    Get

        '    End Get
        '    Set(value As OperacionGenerica)

        '    End Set
        'End Property

        Public Property UpdateOnKeyValues As List(Of String) Implements IEntidadDatos.UpdateOnKeyValues
            Get
                Return _keyValues
            End Get
            Set(value As List(Of String))
                _keyValues = value
            End Set
        End Property

        Public Property DeteleOnKeyValues As List(Of String) Implements IEntidadDatos.DeteleOnKeyValues
            Get
                Return _keyValues
            End Get
            Set(value As List(Of String))
                _keyValues = value
            End Set
        End Property

        Public Sub Array(ByVal arrayOfDataEntity_ As List(Of IEntidadDatos)) Implements IEntidadDatos.Array

        End Sub

        Public Property Attribute(ByVal nombreCampo_ As Object) As Object _
            Implements IEntidadDatos.Attribute

            Set(ByVal valor_ As Object)

                Dim campo_ As New CampoVirtual

                With campo_

                    .Atributo = nombreCampo_

                    .Orden = _orden

                    .Valor = valor_

                End With

                If campo_.IDPermiso Is Nothing Then

                    _listaAtributos.Add(campo_)

                    _orden += 1

                ElseIf VerificaPermisoPerfil(campo_.IDPermiso) Then

                    _listaAtributos.Add(campo_)

                    _orden += 1

                End If

            End Set

            Get

                Return Nothing

            End Get

        End Property

        ' Private _dimension As IEnlaceDatos.TiposDimension
        Public Property Dimension As IEnlaceDatos.TiposDimension _
            Implements IEntidadDatos.Dimension

            Get

                Return _dimension

            End Get

            Set(value As IEnlaceDatos.TiposDimension)

                _dimension = value

            End Set
        End Property

        Public ReadOnly Property CampoPorNombre(ByVal atributoDimension_ As Object) As Object _
            Implements IEntidadDatos.CampoPorNombre

            Get

                Return ObtieneAtributo(atributoDimension_)

            End Get

        End Property

        Public Property Atributos As List(Of CampoVirtual) _
            Implements IEntidadDatos.Atributos

            Get

                Return _listaAtributos

            End Get

            Set(value As List(Of CampoVirtual))

                _listaAtributos = value

            End Set

        End Property

        Private WriteOnly Property ModificaAtributo(ByVal atributo_ As Object)

            Set(value)

                For Each item_ As CampoVirtual In _listaAtributos

                    If item_.Atributo = atributo_ Then

                        item_ = value

                        Exit Property

                    End If

                Next

                'NOT FOUND!

            End Set

        End Property

        Public Property NewRow As DataRow _
            Implements IEntidadDatos.NewRow

            Get

                Return _workRow

            End Get

            Set(value As DataRow)

                _workRow = value

            End Set

        End Property

        Public Property Clausulas As String _
            Implements IEntidadDatos.Clausulas

            Get

                Return _clausulas

            End Get

            Set(value As String)

                _clausulas = value

            End Set

        End Property

        Public Property Status As Wma.Exceptions.TagWatcher _
            Implements IEntidadDatos.Status

            Get

                Return _status

            End Get

            Set(value As Wma.Exceptions.TagWatcher)

                _status = value

            End Set

        End Property

        'Public Property SubscriptionsGroup As subscriptionsgroup Implements IEntidadDatos.SubscriptionsGroup
        '    Get
        '        Throw New NotImplementedException()
        '    End Get
        '    Set(value As subscriptionsgroup)
        '        Throw New NotImplementedException()
        '    End Set
        'End Property

#End Region

#Region "Metodos"

        'EN REVISIÓN
        Private Function ObtieneAtributo(ByVal atributo_ As Object) As CampoVirtual

            For Each item_ As CampoVirtual In _listaAtributos

                If item_.Atributo = atributo_ Then

                    Return item_

                End If

            Next

            Return Nothing

        End Function

        'Sobre carga 1
        Sub cmp(ByVal nombreCampo_ As Object) Implements IEntidadDatos.cmp

            Dim campo_ As New CampoVirtual

            campo_.Atributo = nombreCampo_

            campo_.Orden = _orden

            If campo_.IDPermiso Is Nothing Then

                _listaAtributos.Add(campo_)

                _orden += 1

            ElseIf VerificaPermisoPerfil(campo_.IDPermiso) Then

                _listaAtributos.Add(campo_)

                _orden += 1

            End If


        End Sub

        'Sobre carga 2
        Public Sub cmp(ByVal nombreCampo_ As Object,
                        ByVal descripcion_ As String) _
           Implements IEntidadDatos.cmp

            Dim campo_ As New CampoVirtual

            campo_.Atributo = nombreCampo_

            campo_.Orden = _orden

            campo_.Descripcion = descripcion_

            _listaAtributos.Add(campo_)

            _orden += 1

        End Sub

        'Sobre carga 3
        Public Sub cmp(ByVal nombreCampo_ As Object,
                        Optional ByVal descripcion_ As String = Nothing,
                        Optional ByVal estiloCCS_ As IEntidadDatos.EstilossCCS = IEntidadDatos.EstilossCCS.SinDefinir,
                        Optional ByVal nombreCampoFiltro_ As Object = Nothing,
                        Optional ByVal esAgrupador_ As Boolean = False,
                        Optional ByVal funcionAgregacion_ As IEntidadDatos.TiposFuncionesAgregacion = IEntidadDatos.TiposFuncionesAgregacion.SinDefinir,
                        Optional ByVal tipoOrdenamiento_ As IEntidadDatos.TiposOrdenamiento = IEntidadDatos.TiposOrdenamiento.SinDefinir,
                        Optional ByVal mostrarCampo_ As IEntidadDatos.MostrarCampo = IEntidadDatos.MostrarCampo.Si
                    ) Implements IEntidadDatos.cmp

            Dim campo_ As New CampoVirtual

            campo_.Atributo = nombreCampo_

            campo_.Orden = _orden

            campo_.Descripcion = descripcion_

            campo_.EstiloCCS = estiloCCS_

            If Not nombreCampoFiltro_ Is Nothing Then

                Dim campoFiltro_ = New CampoVirtual

                campoFiltro_.Atributo = nombreCampoFiltro_

                campo_.AtributoFiltro = campoFiltro_

            End If

            campo_.EsAgrupador = esAgrupador_

            campo_.FuncionAgregacion = funcionAgregacion_

            campo_.TipoOrdenamiento = tipoOrdenamiento_

            'campo_.MostrarCampo = mostrarCampo_

            _listaAtributos.Add(campo_)

            _orden += 1

        End Sub

        Private Function VerificaPermisoPerfil(ByVal consultaPermiso_ As String) As Boolean

            If consultaPermiso_ = "500" Then
                Return False
            Else
                Return True
            End If

            Return False

        End Function

#End Region

#Region "Clon"

        Public Function Clone() As Object Implements ICloneable.Clone

            Dim entidadDatosClonada_ As IEntidadDatos = New EntidadDatos

            With entidadDatosClonada_

                For Each atributo_ As CampoVirtual In Me.Atributos

                    Dim atributoAuxiliar_ As New CampoVirtual

                    atributoAuxiliar_ = atributo_

                    .Atributos.Add(atributoAuxiliar_)

                Next

                .Clausulas = Me.Clausulas

                .DeteleOnKeyValues = Me.DeteleOnKeyValues

                .Dimension = Me.Dimension

                .Status = Me.Status

                .UpdateOnKeyValues = Me.UpdateOnKeyValues

            End With

            Return entidadDatosClonada_

        End Function

#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean ' Para detectar llamadas redundantes

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: eliminar estado administrado (objetos administrados).
                End If

                'Propiedades no administradas

                With Me

                    .Atributos = Nothing

                    .Status = Nothing

                End With

                ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
                ' TODO: Establecer campos grandes como Null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: invalidar Finalize() sólo si la instrucción Dispose(ByVal disposing As Boolean) anterior tiene código para liberar recursos no administrados.
        'Protected Overrides Sub Finalize()
        '    ' No cambie este código. Ponga el código de limpieza en la instrucción Dispose(ByVal disposing As Boolean) anterior.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub




#End Region

    End Class

End Namespace


