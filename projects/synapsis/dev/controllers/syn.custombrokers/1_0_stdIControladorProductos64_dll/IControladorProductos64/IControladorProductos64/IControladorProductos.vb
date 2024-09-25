Imports System.IO
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices
Imports MongoDB.Bson
Imports Syn.Documento
Imports Wma.Exceptions

Public Interface IControladorProductos
#Region "Enum"
    Enum Disponibilidades
        SinDefinir = 0
        Habilitado = 1
        NoHabilitado = 2
    End Enum
    Enum Estatus
        SinDefinir = 0
        Autorizado = 1
        Preliminar = 2
        Clasificado = 3
        Suprimido = 4
    End Enum
    Enum Modalidades
        SinDefinir = 0
        Interno = 1
        Externo = 2
    End Enum
    Enum ListaBusquedas
        SinDefinir = 0
        Cliente = 4
        Proveedor = 5
    End Enum

#End Region

#Region "Propiedades"
    Property ListaProductos As List(Of ConstructorProducto)
    Property Producto As ConstructorProducto
    Property Estado As TagWatcher
    Property ModalidadTrabajo As Modalidades
    WriteOnly Property ConservarProductos As Boolean
    Property Entorno As Integer
    Property DisponibilidadRecurso As Disponibilidades
    Property EstatusRecurso As Estatus
    Property BusquedaRecurso As ListaBusquedas
#End Region

#Region "Métodos"
    Sub ReiniciarControlador(Optional ByVal entorno_ As Integer = 1)
    Function Consultar() As TagWatcher
    Function Consultar(ByVal consulta_ As String, ByVal filtro_ As String) As TagWatcher
    Function Consultar(ByVal listaIdProductos_ As List(Of ObjectId)) As TagWatcher
    Function ConsultarOne(ByVal idProducto_ As ObjectId) As TagWatcher
    Function Archivar(ByVal listaIdProductos_ As List(Of ObjectId)) As TagWatcher
    Function ArchivarOne(ByVal idProducto_ As ObjectId) As TagWatcher
#End Region

End Interface
