Imports System.Security.Cryptography
Imports gsol
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Driver
'Imports Rec.Globals.Controllers.IControladorEmpresas64
Imports Rec.Globals.Utils
Imports Wma.Exceptions

Namespace Rec.Globals.Controllers


    ''' <summary>
    ''' 'INTERFACE EMPRESA
    ''' </summary>




    Public Interface IEmpresa64 : Inherits IDisposable

#Region "Propiedades"
        Property _id As ObjectId

        Property _idempresa As Int32

        <BsonIgnoreIfNull>
        Property _idempresakb As Int32?

        Property razonsocial As String

        Property razonsocialcorto As String

        <BsonIgnoreIfNull>
        Property abreviatura As String

        Property nombrecomercial As String

        <BsonIgnoreIfNull>
        Property domicilios As List(Of Domicilio64)

        <BsonIgnoreIfNull>
        Property girocomercial As String

        <BsonIgnoreIfNull>
        Property _idgrupocomercial As ObjectId?

        <BsonIgnoreIfNull>
        Property contactos As List(Of Contacto64)

        Property abierto As Boolean

        Property estado As Int16

        Property archivado As Boolean

#End Region

    End Interface





    ''' <summary>
    ''' 'INTERFACE EMPRESA NACIONAL
    ''' </summary>
    ''' 


    Public Interface IEmpresaNacional64 : Inherits IDisposable, IEmpresa64

#Region "Enums"
        Enum TiposPersona64

            Moral = 1

            Fisica = 2

        End Enum

#End Region

#Region "Propiedades"

        Property _idrfc As ObjectId

        Property rfc As String

        Property rfcs As List(Of Rfc64)

        <BsonIgnoreIfNull>
        Property _idcurp As ObjectId?

        <BsonIgnoreIfNull>
        Property curp As String

        <BsonIgnoreIfNull>
        Property curps As List(Of Curp64)

        <BsonIgnoreIfNull>
        Property regimenfiscal As List(Of RegimenFiscal64)

        Property tipopersona As TiposPersona64

#End Region

    End Interface



    ''' <summary>
    ''' 'INTERFACE EMPRESA INTERNACIONAL
    ''' </summary>



    Public Interface IEmpresaInternacional64 : Inherits IDisposable, IEmpresa64

        Property taxids As List(Of TaxId64)

        Property idpais As ObjectId

        Property pais As String

        <BsonIgnoreIfNull>
        Property _idbu As ObjectId?

        <BsonIgnoreIfNull>
        Property bu As String

        <BsonIgnoreIfNull>
        Property bus As List(Of Bus64)

    End Interface



    ''' <summary>
    ''' 'CLASE EMPRESA
    ''' </summary>
    ''' 

    Public MustInherit Class Empresa64
        Implements IEmpresa64

        Private disposedValue As Boolean

        Public Property _id As ObjectId _
            Implements IEmpresa64._id

        Public Property _idempresa As Integer _
            Implements IEmpresa64._idempresa

        <BsonIgnoreIfNull>
        Public Property _idempresakb As Integer? _
            Implements IEmpresa64._idempresakb

        Public Property razonsocial As String _
            Implements IEmpresa64.razonsocial

        Public Property razonsocialcorto As String _
            Implements IEmpresa64.razonsocialcorto

        <BsonIgnoreIfNull>
        Public Property abreviatura As String _
            Implements IEmpresa64.abreviatura

        Public Property nombrecomercial As String _
            Implements IEmpresa64.nombrecomercial

        <BsonIgnoreIfNull>
        Public Property domicilios As List(Of Domicilio64) _
            Implements IEmpresa64.domicilios

        <BsonIgnoreIfNull>
        Public Property girocomercial As String _
            Implements IEmpresa64.girocomercial

        <BsonIgnoreIfNull>
        Public Property _idgrupocomercial As ObjectId? _
            Implements IEmpresa64._idgrupocomercial

        <BsonIgnoreIfNull>
        Public Property contactos As List(Of Contacto64) _
            Implements IEmpresa64.contactos

        Public Property abierto As Boolean _
            Implements IEmpresa64.abierto

        Public Property estado As Short _
            Implements IEmpresa64.estado

        Public Property archivado As Boolean _
            Implements IEmpresa64.archivado

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: eliminar el estado administrado (objetos administrados)
                End If

                ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                ' TODO: establecer los campos grandes como NULL
                disposedValue = True
            End If
        End Sub

        ' ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
        ' Protected Overrides Sub Finalize()
        '     ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

    End Class




    ''' <summary>
    ''' 'CLASE EMPRESA NACIONAL
    ''' </summary>
    ''' 



    Public Class EmpresaNacional64 : Inherits Empresa64
        Implements IEmpresa64, IEmpresaNacional64

        Private disposedValue _
            As Boolean

        Public Property _idrfc As ObjectId _
            Implements IEmpresaNacional64._idrfc

        Public Property rfc As String _
            Implements IEmpresaNacional64.rfc

        Public Property rfcs As List(Of Rfc64) _
            Implements IEmpresaNacional64.rfcs

        <BsonIgnoreIfNull>
        Public Property _idcurp As ObjectId? _
            Implements IEmpresaNacional64._idcurp

        <BsonIgnoreIfNull>
        Public Property curp As String _
            Implements IEmpresaNacional64.curp

        <BsonIgnoreIfNull>
        Public Property curps As List(Of Curp64) _
            Implements IEmpresaNacional64.curps

        <BsonIgnoreIfNull>
        Public Property regimenfiscal As List(Of RegimenFiscal64) _
            Implements IEmpresaNacional64.regimenfiscal

        Public Property tipopersona As IEmpresaNacional64.TiposPersona64 _
            Implements IEmpresaNacional64.tipopersona

    End Class



    ''' <summary>
    ''' 'CLASE EMPRESA INTERNACIONAL
    ''' </summary>
    ''' 


    Public Class EmpresaInternacional64 : Inherits Empresa64
        Implements IEmpresa64, IEmpresaInternacional64

        Private disposedValue _
            As Boolean

        Public Property taxids As List(Of TaxId64) _
            Implements IEmpresaInternacional64.taxids

        <BsonIgnoreIfNull>
        Public Property _idbu As ObjectId? _
            Implements IEmpresaInternacional64._idbu

        <BsonIgnoreIfNull>
        Public Property bu As String _
            Implements IEmpresaInternacional64.bu

        <BsonIgnoreIfNull>
        Public Property bus As List(Of Bus64) _
            Implements IEmpresaInternacional64.bus


        Public Property idpais As ObjectId _
            Implements IEmpresaInternacional64.idpais

        Public Property pais As String _
            Implements IEmpresaInternacional64.pais

    End Class


    ''' <summary>
    ''' 'VEHICULOS
    ''' </summary>
    ''' 


#Region "Vehículos"

    ''' <summary>
    ''' 'BUS
    ''' </summary>
    ''' 

    Public Class Bus64

        Property idunidadnegocio As ObjectId

        Property sec As Integer

        Property unidadnegocio As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class


    ''' <summary>
    ''' 'CONTACTO
    ''' </summary>
    ''' 

    Public Class Contacto64

        Property idcontacto As ObjectId

        Property sec As Integer

        Property nombrecompleto As String

        Property correoelectronico As String

        Property telefono As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class



    ''' <summary>
    ''' 'DOMICILIO
    ''' </summary>
    ''' 

    Public Class Domicilio64

        Property _iddomicilio As ObjectId

        <BsonIgnoreIfNull>
        Property iddivisionkb As Integer?

        Property sec As Integer

        Property calle As String

        Property numeroexterior As String

        <BsonIgnoreIfNull>
        Property numerointerior As String

        <BsonIgnoreIfNull>
        Property colonia As String

        <BsonIgnoreIfNull>
        Property codigopostal As String

        Property ciudad As String

        <BsonIgnoreIfNull>
        Property localidad As String

        <BsonIgnoreIfNull>
        Property municipio As String

        <BsonIgnoreIfNull>
        Property entidadfederativa As String

        <BsonIgnoreIfNull>
        Property estadorepublica As String

        Property pais As String

        Property estado As Int16 = 1

        Property archivado As Boolean = False

    End Class


    ''' <summary>
    ''' 'RFC
    ''' </summary>
    ''' 


    Public Class Rfc64

        Property idrfc As ObjectId

        Property sec As Integer

        Property rfc As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class



    ''' <summary>
    ''' 'CURP
    ''' </summary>
    ''' 


    Public Class Curp64

        Property idcurp As ObjectId

        Property sec As Integer

        Property curp As String

        Property estado As Integer

        Property archivado As Boolean = False

    End Class


    ''' <summary>
    ''' 'REGIMEN FISCAL
    ''' </summary>
    ''' 



    Public Class RegimenFiscal64

        Property idregimenfiscal As ObjectId

        Property _sec As Integer

        Property regimenfiscal As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class


    ''' <summary>
    ''' 'TAXID
    ''' </summary>
    ''' 

    Public Class TaxId64
        Property idtaxid As ObjectId

        Property sec As Integer

        Property taxid As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class



#End Region



    ''' <summary>
    ''' 'IControlador de empresas
    ''' </summary>
    ''' 

    Public Interface IControladorEmpresas64 : Inherits IDisposable

#Region "Enums"

        Enum TiposEmpresas

            Nacional = 1

            Internacional = 2

        End Enum


        Enum TipoBusqueda

            Id = 1

            RazonSocial = 2

            Taxid = 3

            Rfc = 4

            Curp = 5

        End Enum

#End Region


#Region "Propiedades"

        Property TipoEmpresa As TiposEmpresas

        Property EmpresaNacional As EmpresaNacional64

        Property EmpresaInternacional As EmpresaInternacional64

        Property Estado As TagWatcher

#End Region

#Region "Funciones"
        Function NuevaEmpresa(ByVal empresa_ As IEmpresaNacional64,
                              Optional ByVal session_ As IClientSessionHandle = Nothing) _
                              As TagWatcher

        Function NuevaEmpresa(ByVal empresa_ As IEmpresaInternacional64,
                              Optional ByVal session_ As IClientSessionHandle = Nothing) _
                              As TagWatcher

        Function NuevaEmpresa(Optional ByVal session_ As IClientSessionHandle = Nothing) _
                              As TagWatcher

        Function ActualizaEmpresa(ByVal empresa_ As IEmpresaNacional64,
                                  Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                  As TagWatcher

        Function ActualizaEmpresa(ByVal empresa_ As IEmpresaInternacional64,
                                  Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                  As TagWatcher

        Function BuscarEmpresas(ByVal objectIdEmpresa_ As ObjectId,
                                Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                As TagWatcher

        Function BuscarEmpresas(ByVal razonSocial_ As String,
                                Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                As TagWatcher

        Function BuscarDomicilios(ByVal objectIdEmpresa_ As ObjectId,
                                  Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                  As TagWatcher

        Function BuscarDomicilios(ByVal razonSocialEmpresa_ As String,
                                  ByVal paisEmpresa_ As String,
                                  Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                  As TagWatcher

#End Region

    End Interface



    ''' <summary>
    ''' 'Controlador de empresas
    ''' </summary>
    ''' 


    Public Class ControladorEmpresas64
        Implements IControladorEmpresas64, ICloneable, IDisposable

#Region "Propiedades privadas"

        Private _listaEmpresasNacionales As List(Of EmpresaNacional64)

        Private _listaEmpresasInternacionales As List(Of EmpresaInternacional64)

#End Region


#Region "Propiedades públicas"

        Private disposedValue As Boolean

        Public Property TipoEmpresa As IControladorEmpresas64.TiposEmpresas _
            Implements IControladorEmpresas64.TipoEmpresa

        Public Property EmpresaNacional As EmpresaNacional64 _
            Implements IControladorEmpresas64.EmpresaNacional

        Public Property EmpresaInternacional As EmpresaInternacional64 _
            Implements IControladorEmpresas64.EmpresaInternacional

        Public Property Estado As TagWatcher _
            Implements IControladorEmpresas64.Estado

#End Region


#Region "Constructores"
        Sub New(ByVal tipoEmpresa_ As IControladorEmpresas64.TiposEmpresas)

            TipoEmpresa = tipoEmpresa_

            Estado = New TagWatcher

        End Sub
#End Region


#Region "Métodos privados"

        Private Function GuardarEmpresaNacional(ByVal empresaNacional_ As EmpresaNacional64,
                                                Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                                As EmpresaNacional64
            With Estado

                Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                              With {
                                                                .Aplicacion = 4,
                                                                .DivisionEmpresarial = 1,
                                                                .ClaveEjecutivo = 139,
                                                                .Idioma = 1,
                                                                .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                              }

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional64)("Glo007EmpresasNacionales")

                    session_ = operationsDB_.Database.Client.StartSession

                    Dim result_ = operationsDB_.InsertOneAsync(session_, empresaNacional_).ConfigureAwait(False)

                    .SetOK()

                End Using

            End With

            Return empresaNacional_

        End Function

        Private Function GuardarEmpresaInternacional(ByVal empresaInternacional_ As EmpresaInternacional64,
                                                     Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                                     As EmpresaInternacional64
            With Estado

                Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                          With {
                                                            .Aplicacion = 4,
                                                            .DivisionEmpresarial = 1,
                                                            .ClaveEjecutivo = 139,
                                                            .Idioma = 1,
                                                            .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                          }



                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional64)("Glo007EmpresasInternacionales")

                    session_ = operationsDB_.Database.Client.StartSession

                    Dim result_ = operationsDB_.InsertOneAsync(session_, empresaInternacional_).ConfigureAwait(False)

                    .SetOK()

                End Using

            End With

            Return empresaInternacional_

        End Function

        Private Function ActualizarEmpresaNacional(ByRef empresaNacional_ As IEmpresaNacional64,
                                                        Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                                        As TagWatcher


            Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                          With {
                                                            .Aplicacion = 4,
                                                            .DivisionEmpresarial = 1,
                                                            .ClaveEjecutivo = 139,
                                                            .Idioma = 1,
                                                            .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                          }


            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                With empresaNacional_

                    Dim domicilioAux_ = Nothing

                    If .domicilios IsNot Nothing Then

                        domicilioAux_ = New Domicilio64 _
                                     With {
                                            ._iddomicilio = empresaNacional_.domicilios(0)._iddomicilio,
                                            .iddivisionkb = empresaNacional_.domicilios(0).iddivisionkb,
                                            .sec = empresaNacional_.domicilios(0).sec,
                                            .calle = empresaNacional_.domicilios(0).calle,
                                            .numeroexterior = empresaNacional_.domicilios(0).numeroexterior,
                                            .numerointerior = empresaNacional_.domicilios(0).numerointerior,
                                            .colonia = empresaNacional_.domicilios(0).colonia,
                                            .codigopostal = empresaNacional_.domicilios(0).codigopostal,
                                            .ciudad = empresaNacional_.domicilios(0).ciudad,
                                            .entidadfederativa = empresaNacional_.domicilios(0).entidadfederativa,
                                            .pais = empresaNacional_.domicilios(0).pais,
                                            .estado = empresaNacional_.domicilios(0).estado,
                                            .archivado = empresaNacional_.domicilios(0).archivado
                                        }

                    End If

                    Dim contactosAux_ = Nothing

                    If .contactos IsNot Nothing Then

                        contactosAux_ = New Contacto64 _
                                        With {
                                                .idcontacto = empresaNacional_.contactos(0).idcontacto,
                                                .sec = empresaNacional_.contactos(0).sec,
                                                .nombrecompleto = empresaNacional_.contactos(0).nombrecompleto,
                                                .correoelectronico = empresaNacional_.contactos(0).correoelectronico,
                                                .telefono = empresaNacional_.contactos(0).telefono,
                                                .estado = empresaNacional_.contactos(0).estado,
                                                .archivado = empresaNacional_.contactos(0).archivado
                                            }

                    End If

                    Dim regimenfiscalAux_ = Nothing

                    If .regimenfiscal IsNot Nothing Then

                        regimenfiscalAux_ = New RegimenFiscal64 _
                                            With {
                                                    .idregimenfiscal = empresaNacional_.regimenfiscal(0).idregimenfiscal,
                                                    ._sec = empresaNacional_.regimenfiscal(0)._sec,
                                                    .regimenfiscal = empresaNacional_.regimenfiscal(0).regimenfiscal,
                                                    .estado = empresaNacional_.regimenfiscal(0).estado,
                                                    .archivado = empresaNacional_.regimenfiscal(0).archivado
                                                  }
                    End If

                    Dim curpAux_ = Nothing

                    If .curps IsNot Nothing Then

                        curpAux_ = New Curp64 _
                                    With {
                                            .idcurp = empresaNacional_.curps(0).idcurp,
                                            .sec = empresaNacional_.curps(0).sec,
                                            .curp = empresaNacional_.curps(0).curp,
                                            .estado = empresaNacional_.curps(0).estado,
                                            .archivado = empresaNacional_.curps(0).archivado
                                        }
                    End If

                    Dim rfcAux_ = Nothing

                    If .rfcs IsNot Nothing Then

                        rfcAux_ = New Rfc64 _
                                    With {
                                            .idrfc = empresaNacional_.rfcs(0).idrfc,
                                            .sec = empresaNacional_.rfcs(0).sec,
                                            .rfc = empresaNacional_.rfcs(0).rfc,
                                            .estado = empresaNacional_.rfcs(0).estado,
                                            .archivado = empresaNacional_.rfcs(0).archivado
                                    }
                    End If

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional64)("Glo007EmpresasNacionales")

                    session_ = operationsDB_.Database.Client.StartSession


                    Dim options_ = New UpdateOptions With {.IsUpsert = True}

                    Dim filter_ = Builders(Of EmpresaNacional64).Filter.Eq(Function(x) x.razonsocial, .razonsocial) And
                                  Builders(Of EmpresaNacional64).Filter.Eq(Function(x) x.estado, 1)

                    Dim setStructureOfSubs_ = Builders(Of EmpresaNacional64).Update.
                                     Set(Function(x) x.razonsocial, .razonsocial).
                                     Set(Function(x) x._idempresakb, ._idempresakb).
                                     Set(Function(x) x._idempresa, ._idempresa).
                                     Set(Function(x) x.razonsocialcorto, .razonsocialcorto).
                                     Set(Function(x) x.abreviatura, .abreviatura).
                                     Set(Function(x) x.nombrecomercial, .nombrecomercial).
                                     Set(Function(x) x.girocomercial, .girocomercial).
                                     Set(Function(x) x._idgrupocomercial, ._idgrupocomercial).
                                     Set(Function(x) x._idcurp, ._idcurp).
                                     Set(Function(x) x.curp, .curp).
                                     Set(Function(x) x.abierto, .abierto).
                                     Set(Function(x) x.estado, .estado).
                                     Set(Function(x) x.archivado, .archivado).
                                     AddToSet("domicilios", domicilioAux_).
                                     AddToSet("contactos", contactosAux_).
                                     AddToSet("rfcs", rfcAux_).
                                     AddToSet("regimenfiscal", regimenfiscalAux_).
                                     AddToSet("curps", curpAux_)


                    Dim result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_, options_).Result

                    With Estado

                        If result_.ModifiedCount <> 0 Then

                            .SetOK()

                        ElseIf result_.UpsertedId IsNot Nothing Then

                            .SetOK()

                        Else

                            .SetError(Me, "Ha ocurrido un error")

                        End If

                    End With

                End With

            End Using

            Return Estado

        End Function


        Private Function ActualizarEmpresaInternacional(ByRef empresaInternacional_ As IEmpresaInternacional64,
                                                        Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                                        As TagWatcher


            Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                          With {
                                                            .Aplicacion = 4,
                                                            .DivisionEmpresarial = 1,
                                                            .ClaveEjecutivo = 139,
                                                            .Idioma = 1,
                                                            .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                          }


            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                With empresaInternacional_

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional64)("Glo007EmpresasInternacionales")

                    session_ = operationsDB_.Database.Client.StartSession

                    Dim options_ = New UpdateOptions With {.IsUpsert = True}

                    Dim filter_ = Builders(Of EmpresaInternacional64).Filter.Eq(Function(x) x.razonsocial, .razonsocial) And
                                  Builders(Of EmpresaInternacional64).Filter.Eq(Function(x) x.estado, 1)

                    Dim domicilioAux_ = Nothing

                    If .domicilios IsNot Nothing Then

                        domicilioAux_ = New Domicilio64 _
                                        With {
                                            ._iddomicilio = empresaInternacional_.domicilios(0)._iddomicilio,
                                            .iddivisionkb = empresaInternacional_.domicilios(0).iddivisionkb,
                                            .sec = empresaInternacional_.domicilios(0).sec,
                                            .calle = empresaInternacional_.domicilios(0).calle,
                                            .numeroexterior = empresaInternacional_.domicilios(0).numeroexterior,
                                            .numerointerior = empresaInternacional_.domicilios(0).numerointerior,
                                            .colonia = empresaInternacional_.domicilios(0).colonia,
                                            .codigopostal = empresaInternacional_.domicilios(0).codigopostal,
                                            .ciudad = empresaInternacional_.domicilios(0).ciudad,
                                            .entidadfederativa = empresaInternacional_.domicilios(0).entidadfederativa,
                                            .pais = empresaInternacional_.domicilios(0).pais,
                                            .estado = empresaInternacional_.domicilios(0).estado,
                                            .archivado = empresaInternacional_.domicilios(0).archivado
                                        }

                    End If

                    Dim contactosAux_ = Nothing

                    If .contactos IsNot Nothing Then

                        contactosAux_ = New Contacto64 _
                                        With {
                                        .idcontacto = empresaInternacional_.contactos(0).idcontacto,
                                        .sec = empresaInternacional_.contactos(0).sec,
                                        .nombrecompleto = empresaInternacional_.contactos(0).nombrecompleto,
                                        .correoelectronico = empresaInternacional_.contactos(0).correoelectronico,
                                        .telefono = empresaInternacional_.contactos(0).telefono,
                                        .estado = empresaInternacional_.contactos(0).estado,
                                        .archivado = empresaInternacional_.contactos(0).archivado
                                        }

                    End If

                    Dim taxidsAux_ = Nothing

                    If .taxids IsNot Nothing Then

                        taxidsAux_ = New TaxId64 _
                                     With {
                                        .idtaxid = empresaInternacional_.taxids(0).idtaxid,
                                        .sec = empresaInternacional_.taxids(0).sec,
                                        .taxid = empresaInternacional_.taxids(0).taxid,
                                        .estado = empresaInternacional_.taxids(0).estado,
                                        .archivado = empresaInternacional_.taxids(0).archivado
                                     }

                    End If

                    Dim busAux_ = Nothing

                    If .bus IsNot Nothing Then

                        busAux_ = New Bus64 _
                                     With {
                                        .idunidadnegocio = empresaInternacional_.bus(0).idunidadnegocio,
                                        .sec = empresaInternacional_.bus(0).sec,
                                        .unidadnegocio = empresaInternacional_.bus(0).unidadnegocio,
                                        .estado = empresaInternacional_.bus(0).estado,
                                        .archivado = empresaInternacional_.bus(0).archivado
                                     }

                    End If

                    Dim setStructureOfSubs_ = Builders(Of EmpresaInternacional64).Update.
                                     Set(Function(x) x.razonsocial, .razonsocial).
                                     Set(Function(x) x.razonsocialcorto, .razonsocialcorto).
                                     Set(Function(x) x.abreviatura, .abreviatura).
                                     Set(Function(x) x.nombrecomercial, .nombrecomercial).
                                     Set(Function(x) x.girocomercial, .girocomercial).
                                     Set(Function(x) x.idpais, .idpais).
                                     Set(Function(x) x.pais, .pais).
                                     Set(Function(x) x.bu, .bu).
                                     AddToSet("domicilios", domicilioAux_).
                                     AddToSet("contactos", contactosAux_).
                                     AddToSet("taxids", taxidsAux_).
                                     AddToSet("bus", busAux_)

                    Dim result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_, options_).Result

                    With Estado

                        If result_.ModifiedCount <> 0 Then

                            .SetOK()

                        ElseIf result_.UpsertedId IsNot Nothing Then

                            .SetOK()

                        Else

                            .SetError(Me, "Ha ocurrido un error")

                        End If

                    End With

                End With

            End Using

            Return Estado

        End Function

        Private Function BuscarEmpresaNacional(ByRef objectIdEmpresa_ As ObjectId,
                                               Optional ByRef session_ As IClientSessionHandle = Nothing) _
                                               As List(Of EmpresaNacional64)

            With Estado


                _listaEmpresasNacionales = New List(Of EmpresaNacional64)

                Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                          With {
                                                            .Aplicacion = 4,
                                                            .DivisionEmpresarial = 1,
                                                            .ClaveEjecutivo = 139,
                                                            .Idioma = 1,
                                                            .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                          }

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional64)("Glo007EmpresasInternacionales")

                    session_ = operationsDB_.Database.Client.StartSession

                    Dim filter_ = Builders(Of EmpresaNacional64).Filter.Eq(Function(x) x._id, objectIdEmpresa_)

                    Dim result_ = operationsDB_.Find(filter_).ToList()

                    If result_.Count <> 0 Then

                        _listaEmpresasNacionales.Add(result_(0))

                    Else

                        .SetError(Me, "Lista vacía")

                    End If

                End Using

            End With

            Return _listaEmpresasNacionales

        End Function

        Private Function BuscarEmpresaInternacional(ByRef objectIdEmpresa_ As ObjectId,
                                                    Optional ByRef session_ As IClientSessionHandle = Nothing) _
                                                    As List(Of EmpresaInternacional64)

            With Estado

                _listaEmpresasInternacionales = New List(Of EmpresaInternacional64)

                Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                          With {
                                                            .Aplicacion = 4,
                                                            .DivisionEmpresarial = 1,
                                                            .ClaveEjecutivo = 139,
                                                            .Idioma = 1,
                                                            .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                          }

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional64)("Glo007EmpresasInternacionales")

                    session_ = operationsDB_.Database.Client.StartSession

                    Dim filter_ = Builders(Of EmpresaInternacional64).Filter.Eq(Function(x) x._id, objectIdEmpresa_)

                    Dim result_ = operationsDB_.Find(filter_).ToList()

                    If result_.Count <> 0 Then

                        .SetOK()

                        _listaEmpresasInternacionales.Add(result_(0))

                    Else

                        .SetError(Me, "Lista vacía")

                    End If

                End Using

            End With

            Return _listaEmpresasInternacionales

        End Function

        Private Function BuscarEmpresaNacional(ByRef razonSocial_ As String,
                                               Optional ByRef session_ As IClientSessionHandle = Nothing) _
                                               As List(Of EmpresaNacional64)

            With Estado


                _listaEmpresasNacionales = New List(Of EmpresaNacional64)

                Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                          With {
                                                            .Aplicacion = 4,
                                                            .DivisionEmpresarial = 1,
                                                            .ClaveEjecutivo = 139,
                                                            .Idioma = 1,
                                                            .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                          }

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional64)("Glo007EmpresasInternacionales")

                    session_ = operationsDB_.Database.Client.StartSession

                    Dim filter_ = Builders(Of EmpresaNacional64).Filter.Regex("razonsocial", razonSocial_) And
                                  Builders(Of EmpresaNacional64).Filter.Eq(Function(x) x.estado, 1)

                    Dim result_ = operationsDB_.Find(filter_).Limit(10).ToList()

                    If result_.Count <> 0 Then

                        For Each item_ In result_

                            _listaEmpresasNacionales.Add(item_)

                        Next


                        If _listaEmpresasNacionales.Count <> 0 Then

                            .SetOK()

                        Else

                            .SetError(Me, "Lista vacía")

                        End If



                    Else

                        .SetError(Me, "No se encontraron empresas nacionales")

                    End If

                End Using

            End With

            Return _listaEmpresasNacionales

        End Function

        Private Function BuscarEmpresaInternacional(ByRef razonSocial_ As String,
                                                    Optional ByRef session_ As IClientSessionHandle = Nothing) _
                                                    As List(Of EmpresaInternacional64)

            With Estado

                _listaEmpresasInternacionales = New List(Of EmpresaInternacional64)

                Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                          With {
                                                            .Aplicacion = 4,
                                                            .DivisionEmpresarial = 1,
                                                            .ClaveEjecutivo = 139,
                                                            .Idioma = 1,
                                                            .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                          }

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional64)("Glo007EmpresasInternacionales")

                    session_ = operationsDB_.Database.Client.StartSession

                    Dim filter_ = Builders(Of EmpresaInternacional64).Filter.Eq(Function(x) x.razonsocial, razonSocial_)

                    Dim result_ = operationsDB_.Find(filter_).ToList()

                    If result_.Count <> 0 Then

                        .SetOK()

                        _listaEmpresasInternacionales.Add(result_(0))

                    Else

                        .SetError(Me, "Lista vacía")

                    End If

                End Using

            End With

            Return _listaEmpresasInternacionales

        End Function

#End Region


#Region "Métodos públicos"

        Public Function NuevaEmpresa(empresa_ As IEmpresaNacional64,
                                     Optional session_ As IClientSessionHandle = Nothing) _
                                     As TagWatcher _
                                     Implements IControladorEmpresas64.NuevaEmpresa

            With Estado

                If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                    If empresa_ IsNot Nothing Then

                        .ObjectReturned = GuardarEmpresaNacional(empresa_, session_)

                    Else

                        .SetOKBut(Me, "No existe instancia de empresa nacional")

                    End If

                Else

                    .SetOKBut(Me, "No es una empresa nacional")

                End If

            End With

            Return Estado

        End Function

        Public Function NuevaEmpresa(empresa_ As IEmpresaInternacional64,
                                     Optional session_ As IClientSessionHandle = Nothing) _
                                     As TagWatcher _
                                     Implements IControladorEmpresas64.NuevaEmpresa

            With Estado

                If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Internacional Then

                    If empresa_ IsNot Nothing Then

                        .ObjectReturned = GuardarEmpresaInternacional(empresa_, session_)

                    Else

                        .SetOKBut(Me, "No existe instancia de empresa internacional")

                    End If

                Else

                    .SetOKBut(Me, "No es una empresa internacional")

                End If

            End With

            Return Estado

        End Function

        Public Function NuevaEmpresa(Optional session_ As IClientSessionHandle = Nothing) _
                                     As TagWatcher _
                                     Implements IControladorEmpresas64.NuevaEmpresa

            Throw New NotImplementedException()
        End Function

        Public Function ActualizaEmpresa(empresa_ As IEmpresaNacional64,
                                         Optional session_ As IClientSessionHandle = Nothing) _
                                         As TagWatcher _
                                         Implements IControladorEmpresas64.ActualizaEmpresa

            With Estado

                If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                    If empresa_ IsNot Nothing Then

                        .ObjectReturned = ActualizarEmpresaNacional(empresa_, session_)

                    Else

                        .SetOKBut(Me, "No existe instancia de empresa nacional")

                    End If

                Else

                    .SetOKBut(Me, "No es una empresa nacional")

                End If

            End With

            Return Estado

        End Function

        Public Function ActualizaEmpresa(empresa_ As IEmpresaInternacional64,
                                         Optional session_ As IClientSessionHandle = Nothing) _
                                         As TagWatcher _
                                         Implements IControladorEmpresas64.ActualizaEmpresa

            With Estado

                If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Internacional Then

                    If empresa_ IsNot Nothing Then

                        .ObjectReturned = ActualizarEmpresaInternacional(empresa_, session_)

                    Else

                        .SetOKBut(Me, "No existe instancia de empresa internacional")

                    End If

                Else

                    .SetOKBut(Me, "No es una empresa internacional")

                End If

            End With

            Return Estado

        End Function

        Public Function BuscarEmpresas(objectIdEmpresa_ As ObjectId,
                                       Optional session_ As IClientSessionHandle = Nothing) _
                                       As TagWatcher _
                                       Implements IControladorEmpresas64.BuscarEmpresas

            With Estado

                If Not objectIdEmpresa_ = ObjectId.Empty Then

                    If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                        .ObjectReturned = BuscarEmpresaNacional(objectIdEmpresa_, session_)


                    ElseIf TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Internacional Then

                        .ObjectReturned = BuscarEmpresaInternacional(objectIdEmpresa_, session_)

                    Else

                        .SetOKBut(Me, "No se ha administrado un tipo de empresa")

                    End If

                Else

                    .SetOKBut(Me, "No se ha enviado un objectid de una empresa")

                End If

            End With

            Return Estado

        End Function

        Public Function BuscarEmpresas(razonSocialEmpresa_ As String,
                                       Optional session_ As IClientSessionHandle = Nothing) _
                                       As TagWatcher _
                                       Implements IControladorEmpresas64.BuscarEmpresas

            With Estado

                If razonSocialEmpresa_ IsNot Nothing Then

                    If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                        .ObjectReturned = BuscarEmpresaNacional(razonSocialEmpresa_, session_)


                    ElseIf TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Internacional Then

                        .ObjectReturned = BuscarEmpresaInternacional(razonSocialEmpresa_, session_)

                    Else

                        .SetOKBut(Me, "No se ha administrado un tipo de empresa")

                    End If

                Else

                    .SetOKBut(Me, "No se ha enviado un objectid de una empresa")

                End If

            End With

            Return Estado

        End Function

        Public Function BuscarDomicilios(objectIdEmpresa_ As ObjectId,
                                         Optional session_ As IClientSessionHandle = Nothing) _
                                         As TagWatcher _
                                         Implements IControladorEmpresas64.BuscarDomicilios

            Throw New NotImplementedException()
        End Function

        Public Function BuscarDomicilios(ByVal razonSocialEmpresa_ As String,
                                         ByVal paisEmpresa_ As String,
                                         Optional session_ As IClientSessionHandle = Nothing) _
                                         As TagWatcher _
                                         Implements IControladorEmpresas64.BuscarDomicilios

            Throw New NotImplementedException()
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Throw New NotImplementedException()
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: eliminar el estado administrado (objetos administrados)
                End If

                ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                ' TODO: establecer los campos grandes como NULL
                disposedValue = True
            End If
        End Sub

        ' ' TODO: reemplazar el finalizador solo si "Dispose(disposing As Boolean)" tiene código para liberar los recursos no administrados
        ' Protected Overrides Sub Finalize()
        '     ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

    End Class

#End Region

End Namespace