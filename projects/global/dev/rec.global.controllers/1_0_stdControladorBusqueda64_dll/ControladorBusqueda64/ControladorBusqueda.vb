Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Wma.Exceptions

Public Class Filtro
    Property IdCampo As Integer
    Property IdSeccion As Integer

End Class

Public Class ControladorBusqueda(Of T As {New})
    Implements IDisposable

#Region "Enums"

    'Enum 'Public Property FormatoRespuesta As FormatosRespuesta
    '    Collection = 1
    '    Dropdown = 2
    '    JSON = 3
    '    XML = 4
    'End Enum

    Enum CampoDetalles
        SinDefinir = 0
        FolioOperacion = 1
        Propietario = 2
    End Enum

#End Region

#Region "Atributos"

    Private _filtrosBusqueda As List(Of Filtro)

    Private _resultadosBusqueda As Dictionary(Of Integer, List(Of SelectOption))

    Private disposedValue As Boolean

#End Region

#Region "Propiedades"

    'Public Property FormatoRespuesta As FormatosRespuesta

    Public Property Limit As Integer

    'Public Property CampoDetalle As CampoDetalles = CampoDetalles.FolioOperacion

#End Region

#Region "Constructores"

    Sub New()

        _filtrosBusqueda = New List(Of Filtro)

        _resultadosBusqueda = New Dictionary(Of Integer, List(Of SelectOption))

        'FormatoRespuesta = FormatosRespuesta.Collection

        Limit = 100

    End Sub

#End Region



#Region "Métodos"

    Public Sub AgregarFiltro(IdSeccion_ As Integer, IdCampo_ As Integer)

        _filtrosBusqueda.Add(New Filtro With {.IdSeccion = IdSeccion_, .IdCampo = IdCampo_})

    End Sub

    Public Sub Buscar(ByVal texto_ As String, Optional ByVal campoDetalle_ As CampoDetalles = CampoDetalles.FolioOperacion)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Using _entidadDatos As IEntidadDatos = New T

                Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                For Each filtro_ As Filtro In _filtrosBusqueda

                    Dim listaElemento_ = New List(Of SelectOption)

                    _enlaceDatos.LimiteResultados = Limit

                    Dim status_ = _enlaceDatos.BusquedaGeneralDocumento(documentoElectronico_, filtro_.IdSeccion, filtro_.IdCampo, texto_)

                    If status_.Status = TagWatcher.TypeStatus.Ok Then

                        For Each item_ As Dictionary(Of Object, Object) In status_.ObjectReturned

                            Dim itemTitle_ = item_.Item("valorOperacion")

                            If campoDetalle_ = CampoDetalles.FolioOperacion Then

                                itemTitle_ = itemTitle_ & " | " & item_.Item("folioOperacion")

                            ElseIf campoDetalle_ = CampoDetalles.Propietario Then

                                itemTitle_ = itemTitle_ & " | " & item_.Item("propietario")

                            End If

                            Dim dataSourceItem_ = New SelectOption With {
                                .Text = itemTitle_,
                                .Value = item_.Item("ID")
                            }

                            listaElemento_.Add(dataSourceItem_)

                        Next

                        If listaElemento_.Count Then

                            _resultadosBusqueda.Add(filtro_.IdCampo, listaElemento_)

                        End If

                    End If

                Next

            End Using

        End Using

    End Sub

    Public Function Buscar(ByVal texto_ As String, filtro_ As Filtro, Optional ByVal campoDetalle_ As CampoDetalles = CampoDetalles.FolioOperacion) As List(Of SelectOption)

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Using _entidadDatos As IEntidadDatos = New T

                Dim documentoElectronico_ As DocumentoElectronico = _entidadDatos

                Dim listaElemento_ = New List(Of SelectOption)

                _enlaceDatos.LimiteResultados = Limit

                Dim status_ = _enlaceDatos.BusquedaGeneralDocumento(documentoElectronico_, filtro_.IdSeccion, filtro_.IdCampo, texto_)

                If status_.Status = TagWatcher.TypeStatus.Ok Then
                    For Each item_ As Dictionary(Of Object, Object) In status_.ObjectReturned

                        Dim itemTitle_ = item_.Item("valorOperacion")

                        If campoDetalle_ = CampoDetalles.FolioOperacion Then

                            itemTitle_ = itemTitle_ & " | " & item_.Item("folioOperacion")

                        ElseIf campoDetalle_ = CampoDetalles.Propietario Then

                            itemTitle_ = itemTitle_ & " | " & item_.Item("propietario")

                        End If

                        Dim dataSourceItem_ = New SelectOption With {
                                .Text = itemTitle_,
                                .Value = item_.Item("ID")
                        }

                        listaElemento_.Add(dataSourceItem_)

                    Next

                    If listaElemento_.Count > 0 Then

                        Return listaElemento_

                    End If

                End If

            End Using

        End Using

        Return Nothing

    End Function

    Public Function ObtenerResultadosBusqueda() As Dictionary(Of Integer, List(Of SelectOption))

        If Not _resultadosBusqueda Is Nothing Then

            If _resultadosBusqueda.Count > 0 Then

                Return _resultadosBusqueda

            End If

        End If

        Return Nothing

    End Function

    Public Function ObtenerResultadosBusqueda(ByVal IdSeccion_ As Integer) As List(Of SelectOption)

        If Not _resultadosBusqueda Is Nothing Then

            If Not _resultadosBusqueda(IdSeccion_) Is Nothing Then

                If _resultadosBusqueda(IdSeccion_).Count > 0 Then

                    Return _resultadosBusqueda(IdSeccion_)

                End If

            End If

        End If

        Return Nothing

    End Function

    Public Function ObtenerDocumento(ByVal objectId_ As String) As TagWatcher

        If objectId_ Is Nothing Or objectId_ = "" Or objectId_ = "-1" Then

            Return New TagWatcher(0, Me, "No se encontró el Id :'(")

        End If

        Dim tipo_ As String = GetType(T).Name

        If Not String.IsNullOrEmpty(objectId_) Then

            Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operacionesDB_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(tipo_)

                Dim resultadoDocumentos_ As New List(Of OperacionGenerica)

                Dim filtro_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, New ObjectId(objectId_))

                resultadoDocumentos_ = operacionesDB_.Find(filtro_).Limit(1).ToList

                If resultadoDocumentos_.Count Then

                    Dim operacionGenerica_ As OperacionGenerica = resultadoDocumentos_(0)

                    Return New TagWatcher(1) With {.ObjectReturned = operacionGenerica_}

                Else

                    Return New TagWatcher(0, Me, "No se encontró ningún valor para esta consulta")

                End If

            End Using

        End If

        Return New TagWatcher(0, Me, "Sin resultados")

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)

                _filtrosBusqueda = Nothing

                _resultadosBusqueda = Nothing

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True
        End If
    End Sub

    ' ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
    'Protected Overrides Sub Finalize()
    '    ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
    '    Dispose(disposing:=False)
    '    MyBase.Finalize()
    'End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
