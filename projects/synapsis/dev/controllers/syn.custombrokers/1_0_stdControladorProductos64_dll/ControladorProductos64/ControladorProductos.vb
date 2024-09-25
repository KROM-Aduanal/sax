Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Syn.Documento
Imports Wma.Exceptions
Imports gsol
Imports Busqueda = Syn.CustomBrokers.Controllers.IControladorProductos.ListaBusquedas
Imports Disponibilidad = Syn.CustomBrokers.Controllers.IControladorProductos.Disponibilidades
Imports Modalidad = Syn.CustomBrokers.Controllers.IControladorProductos.Modalidades
Imports Estatus = Syn.CustomBrokers.Controllers.IControladorProductos.Estatus
Imports Syn.Documento.Componentes
Imports Rec.Globals.Utils
Imports SeccionProducto = Syn.Nucleo.RecursosComercioExterior.SeccionesProducto
Imports CampoProducto = Syn.Nucleo.RecursosComercioExterior.CamposProducto
Imports Syn.Nucleo.RecursosComercioExterior
Imports System.Linq.Expressions
Imports MongoDB.Bson.Serialization.Attributes

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class ControladorProductos
    Implements IControladorProductos, IDisposable, ICloneable

#Region "PROPIEDADES PRIVADAS"
    Private _entorno As Int32
    Private disposedValue As Boolean
    Private _espacioTrabajo As IEspacioTrabajo
    Private _listaDataSource As List(Of SelectOption)
    Public _pageNumber As Int32
    Public _limitProductos As Int32
#End Region

#Region "PROPIEDADES PUBLICAS"
    Public Property ListaProductos As List(Of ConstructorProducto) _
        Implements IControladorProductos.ListaProductos
    Public Property Producto As Documento.ConstructorProducto _
        Implements IControladorProductos.Producto
    Public Property Estado As TagWatcher _
        Implements IControladorProductos.Estado
    Public Property ModalidadTrabajo As IControladorProductos.Modalidades _
        Implements IControladorProductos.ModalidadTrabajo
    Public Property ConservarProductos As Boolean _
        Implements IControladorProductos.ConservarProductos
    Public Property Entorno As Integer _
        Implements IControladorProductos.Entorno
        Get
            Return _entorno
        End Get
        Set(value As Integer)
            _entorno = value
            ReiniciarControlador(_entorno)
        End Set
    End Property
    Public Property DisponibilidadRecurso As IControladorProductos.Disponibilidades _
        Implements IControladorProductos.DisponibilidadRecurso
    Public Property EstatusRecurso As IControladorProductos.Estatus _
        Implements IControladorProductos.EstatusRecurso
    Public Property BusquedaRecurso As IControladorProductos.ListaBusquedas _
        Implements IControladorProductos.BusquedaRecurso
#End Region

#Region "CONSTRUCTORES"
    Sub New()
        Inicializa(Busqueda.SinDefinir,
                   Modalidad.Externo,
                   Disponibilidad.SinDefinir,
                   Estatus.SinDefinir,
                   conservarProductos_:=True)
    End Sub

    Sub New(ByVal busquedaRecurso_ As Busqueda)
        Inicializa(busquedaRecurso_,
                   Modalidad.Externo,
                   Disponibilidad.SinDefinir,
                   Estatus.SinDefinir,
                   conservarProductos_:=True)
    End Sub

    Sub New(ByVal busquedaRecurso_ As Busqueda,
            ByVal modalidadTrabajo_ As Modalidad,
            ByVal disponibilidadRecurso_ As Disponibilidad,
            ByVal estatusRecurso_ As Estatus,
            ByVal conservarProductos_ As Boolean)

        Inicializa(busquedaRecurso_,
                   modalidadTrabajo_,
                   disponibilidadRecurso_,
                   estatusRecurso_,
                   conservarProductos_)
    End Sub

    Private Sub Inicializa(ByVal busquedaRecurso_ As Busqueda,
                           ByVal modalidadTrabajo_ As Modalidad,
                           ByVal disponibilidadRecurso_ As Disponibilidad,
                           ByVal estatusRecurso_ As Estatus,
                           ByVal conservarProductos_ As Boolean)

        DisponibilidadRecurso = disponibilidadRecurso_
        EstatusRecurso = estatusRecurso_
        BusquedaRecurso = busquedaRecurso_
        ModalidadTrabajo = modalidadTrabajo_
        ConservarProductos = conservarProductos_
        ListaProductos = New List(Of ConstructorProducto)
        Producto = New ConstructorProducto
        Estado = New TagWatcher
        _entorno = 1 'OFICINA
        _pageNumber = 1
        _limitProductos = 10
    End Sub
#End Region

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: eliminar el estado administrado (objetos administrados)
                _espacioTrabajo = Nothing
                disposedValue = Nothing
                _entorno = Nothing
                _limitProductos = Nothing
                _pageNumber = Nothing
                _limitProductos = Nothing
            End If
            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            disposedValue = True
        End If
    End Sub

    Public Sub ReiniciarControlador(Optional entorno_ As Integer = 1) _
        Implements IControladorProductos.ReiniciarControlador

        Inicializa(Modalidad.Externo,
                   Disponibilidad.SinDefinir,
                   Estatus.SinDefinir,
                   Busqueda.SinDefinir,
                   conservarProductos_:=True)
        _pageNumber = 1
        _limitProductos = 10
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

#Region "CONSULTAS EXTERNAS"
    Private Function BuscarProductos(ByVal consulta_ As String,
                                     ByVal filtro_ As String) As TagWatcher
        Dim auxiliarProducto_ As List(Of AuxiliarProducto) = New List(Of AuxiliarProducto)
        Try
            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}
                Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProducto).GetType.Name)
                Dim filter_ = Builders(Of OperacionGenerica).Filter.Text(consulta_)
                Dim pipeline_ = collection_.Aggregate().
                                            Match(filter_).
                                            Project(Function(y) _
                                                        New With {
                                                        Key .id_ = y.Id,
                                                        Key .seccionEncabezado_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Encabezado")(0).Nodos(0).Nodos(1),
                                                        Key .cuerpoCliente_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(1).Nodos(0).Nodos(0),
                                                        Key .cuerpoNumeroParte_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts.Item("Cuerpo")(3).Nodos(0).Nodos(0)}).
                                            Project(Function(z) _
                                                        New With {
                                                        Key .idDocumento_ = z.id_,
                                                        Key .productoHabilitado_ = DirectCast(z.seccionEncabezado_.Nodos(0), Campo).Valor,
                                                        Key .cliente_ = DirectCast(z.cuerpoCliente_.Nodos(0).Nodos(0), Campo).Valor,
                                                        Key .numeroParte_ = DirectCast(z.cuerpoNumeroParte_.Nodos(1).Nodos(0), Campo).Valor,
                                                        Key .alias_ = DirectCast(z.cuerpoNumeroParte_.Nodos(4).Nodos(0), Campo).Valor, ''el valor del nodo debe ser 2
                                                        Key .idkrom_ = DirectCast(z.cuerpoNumeroParte_.Nodos(0).Nodos(0), Campo).Valor}).
                                                        Skip((_pageNumber - 1) * _limitProductos).
                                                        Limit(_limitProductos).ToList
                If pipeline_.Any() Then
                    pipeline_.AsEnumerable.ToList.ForEach(Sub(x)
                                                              If x.productoHabilitado_ And x.cliente_ = filtro_ Then
                                                                  Dim auxProducto_ As New AuxiliarProducto _
                                                                  With {
                                                                        .id = x.idDocumento_,
                                                                        ._idKrom = x.idkrom_,
                                                                        ._alias = x.alias_,
                                                                        ._numeroParte = x.numeroParte_,
                                                                        ._estado = x.productoHabilitado_
                                                                  }
                                                                  auxiliarProducto_.Add(auxProducto_)
                                                              End If
                                                          End Sub)
                End If

                With Estado
                    If auxiliarProducto_.Count > 0 Then
                        .ObjectReturned = auxiliarProducto_
                        .SetOK()
                    Else
                        .SetOKBut(Me, "No se encontraron resultados")
                    End If
                End With
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
        Return Estado
    End Function

    Private Function ObtenerProducto(ByVal ObjectIdProducto_ As ObjectId) _
        As TagWatcher
        Try
            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}
                Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProducto).GetType.Name)
                Dim result_ = collection_.Aggregate().
                                          Match(Function(x) x.Id.Equals(ObjectIdProducto_)).
                                          Project(Function(x) _
                                                        New With {
                                                        Key .id_ = x.Id,
                                                        Key .cuerpoEstatus_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo")(0).Nodos(0),
                                                        Key .cuerpoFraccion_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo")(2).Nodos(0).Nodos(0),
                                                        Key .cuerpoNumeroParte_ = x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo")(3).Nodos(0)
                                                        }).
                                          Project(Function(y) _
                                                        New With {
                                                        Key .idDocumento_ = y.id_,
                                                        Key .estatus_ = TryCast(y.cuerpoEstatus_.Nodos(5).Nodos(0), Campo).ValorPresentacion,
                                                        Key .fraccionArancelaria_ = TryCast(y.cuerpoFraccion_.Nodos(0).Nodos(0), Campo).Valor,
                                                        Key .nico_ = TryCast(y.cuerpoFraccion_.Nodos(1).Nodos(0), Campo).Valor,
                                                        Key .numerosParte_ = y.cuerpoNumeroParte_}).ToList
                With Estado
                    If result_.Any() Then
                        Dim listaNumerosParte_ = TryCast(result_(0).numerosParte_.Nodos, List(Of Nodo))

                        Dim auxiliarProducto_ As New AuxiliarProducto
                        With auxiliarProducto_
                            .id = result_(0).idDocumento_
                            ._fraccionArancelaria = result_(0).fraccionArancelaria_
                            ._nico = result_(0).nico_
                            ._status = result_(0).estatus_
                            ._historicoDescripciones = New List(Of HistoricoDescripciones)
                        End With

                        Dim historico_ As New HistoricoDescripciones
                        For Each numerosParte_ In listaNumerosParte_
                            For Each item_ In numerosParte_.Nodos
                                Dim nodo = TryCast(item_.Nodos(0), Campo)
                                Select Case nodo.IDUnico
                                    Case CamposProducto.CP_IDKROM
                                        historico_._idKrom = nodo.Valor
                                    Case CamposProducto.CP_NUMERO_PARTE
                                        historico_._numeroParte = nodo.Valor
                                    Case CamposProducto.CP_ALIAS
                                        historico_._alias = nodo.Valor
                                    Case CamposProducto.CP_DESCRIPCION
                                        historico_._descripcion = nodo.Valor
                                End Select
                            Next
                            auxiliarProducto_._historicoDescripciones.Add(historico_)
                        Next
                        .ObjectReturned = auxiliarProducto_
                        .SetOK()
                    Else
                        .SetOKBut(Me, "No se encontraron registros")
                    End If
                End With
            End Using

        Catch ex As Exception
            Return Nothing
        End Try
        Return Estado
    End Function

    Private Function ObtenerProductos(ByVal listaObjectId_ As List(Of ObjectId)) _
        As TagWatcher
        Try
            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = _espacioTrabajo}
                Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorProducto).GetType.Name)
                Dim result_ = collection_.Aggregate().
                                          Match(Function(x) listaObjectId_.Contains(x.Id)).
                                          Project(Function(y) New With {Key .documento_ = y.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente}).
                                          ToList
                If result_.Any() Then
                    result_.AsEnumerable.ToList.ForEach(Sub(x)
                                                            Dim producto As ConstructorProducto = DirectCast(x.documento_, ConstructorProducto)
                                                            If producto IsNot Nothing Then
                                                                ListaProductos.Add(producto)
                                                            End If
                                                        End Sub)
                End If
            End Using
            With Estado
                If ListaProductos.Count > 0 Then
                    .ObjectReturned = ListaProductos
                    .SetOK()
                Else
                    .SetOKBut(Me, "No se encontraron registros")
                End If
            End With
        Catch ex As Exception
            Return Nothing
        End Try
        Return Estado
    End Function
#End Region

#Region "METODOS PUBLICOS"
    Public Function Consultar() As TagWatcher _
        Implements IControladorProductos.Consultar
        Throw New NotImplementedException()
    End Function

    Public Function Consultar(ByVal consulta_ As String,
                              ByVal filtro_ As String) As TagWatcher _
                              Implements IControladorProductos.Consultar
        With Estado
            If _entorno > 0 Then
                If consulta_ <> Nothing And filtro_ <> Nothing Then
                    BuscarProductos(consulta_, filtro_)
                Else
                    .SetError(Me, "Consulta no recibida.")
                End If
            Else
                .SetError(Me, "Entorno no puede ser 0")
            End If
        End With
        Return Estado
    End Function

    Public Function Consultar(listaIdProductos_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorProductos.Consultar
        With Estado
            If _entorno > 0 Then
                If listaIdProductos_.Count > 0 Then
                    ObtenerProductos(listaIdProductos_)
                Else
                    .SetError(Me, "Lista de objectids de productos vacía.")
                End If
            Else
                .SetError(Me, "Entorno no puede ser 0.")
            End If
        End With
        Return Estado
    End Function

    Public Function ConsultarOne(idProducto_ As ObjectId) As TagWatcher _
        Implements IControladorProductos.ConsultarOne

        With Estado
            If _entorno > 0 Then
                If Not idProducto_ = ObjectId.Empty Then
                    ObtenerProducto(idProducto_)
                Else
                    .SetError(Me, "Lista de objectids de productos vacía.")
                End If
            Else
                .SetError(Me, "Entorno no puede ser 0.")
            End If
        End With
        Return Estado
    End Function

    Public Function Archivar(listaIdProductos_ As List(Of ObjectId)) As TagWatcher _
        Implements IControladorProductos.Archivar
        Throw New NotImplementedException()
    End Function

    Public Function ArchivarOne(idProducto_ As ObjectId) As TagWatcher _
        Implements IControladorProductos.ArchivarOne
        Throw New NotImplementedException()
    End Function

#End Region
End Class

Public Class AuxiliarProducto
    Property id As ObjectId
    <BsonIgnoreIfNull>
    Property _historicoDescripciones As List(Of HistoricoDescripciones)
    <BsonIgnoreIfNull>
    Property _idKrom As Integer
    <BsonIgnoreIfNull>
    Property _numeroParte As String
    <BsonIgnoreIfNull>
    Property _alias As String
    <BsonIgnoreIfNull>
    Property _descripcion As String
    <BsonIgnoreIfNull>
    Property _fraccionArancelaria As String
    <BsonIgnoreIfNull>
    Property _nico As String
    <BsonIgnoreIfNull>
    Property _estado As Boolean
    <BsonIgnoreIfNull>
    Property _status As String
End Class

Public Class HistoricoDescripciones
    <BsonIgnoreIfNull>
    Property _idKrom As Integer
    <BsonIgnoreIfNull>
    Property _numeroParte As String
    <BsonIgnoreIfNull>
    Property _alias As String
    <BsonIgnoreIfNull>
    Property _descripcion As String
End Class
