Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosPedimento

Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.Serialization
Imports MongoDB.Bson
Imports MongoDB.Driver

Namespace Syn.Memoria

    'Public Class MementoObject
    '    Inherits Documento

    '    'Campos especiales del expediente o legajo

    '    Private _referencia As String

    '    Private _tipoDocumentoElectronico As TiposDocumentoElectronico 'PedimentoNormal, PedimentoComplementario, Rectificación, Aviso de consolidado, Partes II Impo..Expo

    '    Private _nombreCliente As String

    '    Private _idCliente As Int32


    '    'Para evaluar si deben ser o no colecciones simplemente.

    '    Private _idCorporativo As Int32 '1

    '    Private _nombreCorporativoEmpresarial As String 'KROM Aduanal

    '    Private _idSucursal As Int32 '1 | 3 | 8 | 7 ...

    '    Private _localidad As String 'Veracruz | México | Toluca | Manzanillo | Altamira | Laredo | Nuevo Laredo | Lázaro Cárdenas | CD Juarez | Colombia | Nogales

    '    Private _nombreSucursal As String 'Grupo Reyes Kuri S.C. | Despachos Aereos Internacional S. C. ...

    '    Private _Aduanaseccion As String '430|470|650...

    '    Private _relacionInterna As String ' TiposRelacionCorporativa: SucursalKROM, Corresponsal, ClienteSAAS...


    '    <BsonElement("Referencia")>
    '    Public Property Referencia As String
    '        Get
    '            Return _referencia
    '        End Get
    '        Set(value As String)
    '            _referencia = value
    '        End Set
    '    End Property
    '    <BsonElement("TipoDocumentoElectronico")>
    '    Public Property TipoDocumentoElectronico As TiposDocumentoElectronico
    '        Get
    '            Return _tipoDocumentoElectronico
    '        End Get
    '        Set(value As TiposDocumentoElectronico)
    '            _tipoDocumentoElectronico = value
    '        End Set
    '    End Property
    '    <BsonElement("NombreCliente")>
    '    Public Property NombreCliente As String
    '        Get
    '            Return _nombreCliente
    '        End Get
    '        Set(value As String)
    '            _nombreCliente = value
    '        End Set
    '    End Property
    '    <BsonElement("IdCliente")>
    '    Public Property IdCliente As Integer
    '        Get
    '            Return _idCliente
    '        End Get
    '        Set(value As Integer)
    '            _idCliente = value
    '        End Set
    '    End Property
    '    Public Property Seccion(ByVal IDUnicoSeccion_ As TiposSeccion) As Seccion

    '        Get

    '            Return ObtenerSeccion(IDUnicoSeccion_)

    '        End Get

    '        Set(value As Seccion)

    '            ActualizarSeccion(IDUnicoSeccion_, value)

    '        End Set

    '    End Property
    '    Public Property Campo(ByVal IDUnico_ As CamposAnexo22) As CampoPedimento
    '        Get

    '            Return ObtenerCampo(IDUnico_)

    '        End Get
    '        Set(value As CampoPedimento)
    '            ActualizarCampo(IDUnico_, value)
    '        End Set
    '    End Property
    '    Sub New()

    '    End Sub
    '    Sub New(ByVal referencia_ As String, ByVal tipoDocumentoElectronico_ As TiposDocumentoElectronico, ByVal nombreCliente_ As String,
    '            ByVal idCliente_ As Int32, ByVal idCorporativo_ As Int32, ByVal nombreCorporativoEmpresarial_ As String, ByVal idSucursal_ As Int32,
    '            ByVal localidad_ As String, ByVal nombreSucursal_ As String, ByVal aduanaSeccion_ As String, ByVal relacionInterna_ As String,
    '            ByVal Id_ As String, ByVal idDocumento_ As Int32, ByVal folioDocumento_ As String, ByVal fechaCreacion_ As Date, ByVal usuarioGenerador_ As String,
    '            ByVal estatusDocumento_ As Int32, ByVal documento_ As EstructuraDocumento, ByVal listaSecciones_ As List(Of String))

    '        _referencia = referencia_

    '        _tipoDocumentoElectronico = tipoDocumentoElectronico_

    '        _nombreCliente = nombreCliente_

    '        _idCliente = idCliente_

    '        'Para evaluar si deben ser o no colecciones simplemente.

    '        _idCorporativo = idCorporativo_

    '        _nombreCorporativoEmpresarial = nombreCorporativoEmpresarial_

    '        _idSucursal = idSucursal_

    '        _localidad = localidad_

    '        _nombreSucursal = nombreSucursal_

    '        _Aduanaseccion = aduanaSeccion_

    '        _relacionInterna = relacionInterna_

    '        'Avanzado

    '        Id = Id_
    '        IdDocumento = idDocumento_
    '        FolioDocumento = folioDocumento_
    '        FechaCreacion = fechaCreacion_
    '        UsuarioGenerador = usuarioGenerador_
    '        EstatusDocumento = estatusDocumento_
    '        Documento = documento_
    '        ListaSecciones = listaSecciones_

    '    End Sub

    '    Public Overrides Sub ConstruyeEncabezado()
    '    End Sub
    '    Public Overrides Sub ConstruyeCuerpo()
    '    End Sub
    '    Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()
    '    End Sub
    '    Public Overrides Sub ConstruyePiePagina()
    '    End Sub
    '    Public Overrides Sub GeneraDocumento()
    '        'MyBase.GeneraDocumento()
    '    End Sub

    '    Private Function ObtenerSeccion(ByVal IDUnicoSeccion_ As TiposSeccion) As Seccion

    '        'For Each parDatos_ As KeyValuePair(Of String, List(Of Seccion)) In _estructuraDocumento.Parts

    '        '    For Each seccion_ As Seccion In parDatos_.Value

    '        '        If seccion_.TipoSeccion = IDUnicoSeccion_ Then

    '        '            Return seccion_

    '        '        End If

    '        '    Next

    '        'Next

    '        'Return Nothing

    '    End Function
    '    Private Sub ActualizarSeccion(ByVal IDUnicoSeccion_ As CamposAnexo22, SeccionNueva_ As Seccion)

    '        'For Each parDatos_ As KeyValuePair(Of String, List(Of Seccion)) In _estructuraDocumento.Parts

    '        '    For Each seccion_ As Seccion In parDatos_.Value

    '        '        If seccion_.TipoSeccion = IDUnicoSeccion_ Then

    '        '            seccion_ = SeccionNueva_

    '        '        End If

    '        '    Next

    '        'Next

    '    End Sub
    '    Private Sub ActualizarCampo(ByVal IDUnico_ As CamposAnexo22, campoNuevo_ As CampoPedimento)

    '        'For Each parDatos_ As KeyValuePair(Of String, List(Of Seccion)) In _estructuraDocumento.Parts

    '        '    For Each seccion_ As Seccion In parDatos_.Value

    '        '        For Each campoPedimento_ As CampoPedimento In seccion_.CamposSeccion

    '        '            If campoPedimento_.IDUnico = IDUnico_ Then

    '        '                campoPedimento_ = campoNuevo_

    '        '            End If

    '        '        Next

    '        '    Next
    '        'Next

    '    End Sub
    '    Private Function ObtenerCampo(ByVal IDUnico_ As CamposAnexo22) As CampoPedimento

    '        'For Each parDatos_ As KeyValuePair(Of String, List(Of Seccion)) In _estructuraDocumento.Parts

    '        '    For Each seccion_ As Seccion In parDatos_.Value

    '        '        For Each campoPedimento_ As CampoPedimento In seccion_.CamposSeccion

    '        '            If campoPedimento_.CampoAnexo22 = IDUnico_ Then

    '        '                Return campoPedimento_

    '        '            End If

    '        '        Next

    '        '    Next

    '        'Next


    '        Return Nothing

    '    End Function

    'End Class

End Namespace
