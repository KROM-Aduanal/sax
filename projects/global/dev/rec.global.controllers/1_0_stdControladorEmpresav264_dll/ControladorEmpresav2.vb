Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports gsol
Imports gsol.krom
'Imports IRecursosSistemas
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
        Property paisesdomicilios As List(Of PaisDomicilio)

        <BsonIgnoreIfNull>
        Property girocomercial As String

        <BsonIgnoreIfNull>
        Property _idgrupocomercial As ObjectId?

        Property contactos As List(Of Contacto64)

        Property abierto As Boolean

        Property estado As Int16

        <BsonIgnoreIfNull>
        Property estatus As Int16

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

        Property _idcurp As ObjectId?

        Property curp As String

        <BsonIgnoreIfNull>
        Property curps As List(Of Curp64)

        Property regimenesfiscales As List(Of RegimenFiscal64)

        Property tipopersona As TiposPersona64

#End Region

    End Interface



    ''' <summary>
    ''' 'INTERFACE EMPRESA INTERNACIONAL
    ''' </summary>



    Public Interface IEmpresaInternacional64 : Inherits IDisposable, IEmpresa64

        Property taxids As List(Of TaxId64)

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
        Public Property paisesdomicilios As List(Of PaisDomicilio) _
            Implements IEmpresa64.paisesdomicilios

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

        <BsonIgnoreIfNull>
        Public Property estatus As Short _
            Implements IEmpresa64.estatus

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
        Public Property regimenesfiscales As List(Of RegimenFiscal64) _
            Implements IEmpresaNacional64.regimenesfiscales

        Public Property tipopersona As IEmpresaNacional64.TiposPersona64 _
            Implements IEmpresaNacional64.tipopersona

    End Class



    ''' <summary>
    ''' 'CLASE EMPRESA INTERNACIONAL
    ''' </summary>
    ''' 


    Public Class EmpresaInternacional64 : Inherits Empresa64
        Implements IEmpresa64, IEmpresaInternacional64

        <BsonIgnoreIfNull>
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

    Public Class PaisDomicilio

        Property idpais As ObjectId

        Property sec As Integer

        <BsonIgnoreIfNull>
        Property domicilios As List(Of Domicilio64)

        Property pais As String

        Property estado As Int16 = 1

        Property archivado As Boolean = False

    End Class

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

#End Region


#Region "Propiedades"

        Property TipoEmpresa As TiposEmpresas

        Property EmpresaNacional As EmpresaNacional64

        Property EmpresaInternacional As EmpresaInternacional64

        Property ListaEmpresasNacionales As List(Of EmpresaNacional64)

        Property ListaEmpresasInternacionales As List(Of EmpresaInternacional64)

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

        Function BuscarEmpresas(ByVal token_ As String) _
                                As TagWatcher

        Function BuscarEmpresa(ByVal idEmpresa_ As ObjectId,
                               ByVal idPais_ As ObjectId,
                               ByVal idDomicilio_ As ObjectId) _
                               As TagWatcher

        Function BuscarDomicilio(ByVal cveEmpresa_ As Int32,
                                 ByVal cvePais_ As Int32,
                                 ByVal cveDomicilio_ As Int32) _
                                 As TagWatcher

        Function BuscarDomicilios(ByVal cveEmpresa_ As Int32,
                                  ByVal cvePais_ As Int32) _
                                  As TagWatcher

        Function ArchivarRegistro(ByVal objectidRegistro_ As ObjectId,
                                  ByVal tipoRegistro_ As Int32) _
                                  As TagWatcher

        Function ArchivarRegistros(ByVal objectsIdRegistros_ As List(Of ObjectId),
                                   ByVal tipoRegistro_ As Int32) _
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

        Private _listaEmpresaNacional As List(Of EmpresaNacional64)

        Private _listaEmpresaInternacional As List(Of EmpresaInternacional64)

        Private _listaPaisesDomicilios As List(Of PaisDomicilio)

        Private _listaEmpresas As List(Of SelectOption)

        Private _listaDomicilios As List(Of SelectOption)

        Private _domicilio As Domicilio64

#End Region


#Region "Propiedades públicas"

        Private disposedValue As Boolean

        Public Property TipoEmpresa As IControladorEmpresas64.TiposEmpresas _
            Implements IControladorEmpresas64.TipoEmpresa

        Public Property EmpresaNacional As EmpresaNacional64 _
            Implements IControladorEmpresas64.EmpresaNacional

        Public Property EmpresaInternacional As EmpresaInternacional64 _
            Implements IControladorEmpresas64.EmpresaInternacional

        Public Property ListaEmpresasNacionales As List(Of EmpresaNacional64) _
            Implements IControladorEmpresas64.ListaEmpresasNacionales

        Public Property ListaEmpresasInternacionales As List(Of EmpresaInternacional64) _
            Implements IControladorEmpresas64.ListaEmpresasInternacionales

        Public Property Estado As TagWatcher _
            Implements IControladorEmpresas64.Estado

#End Region


#Region "Constructores"
        Sub New(ByVal tipoEmpresa_ As IControladorEmpresas64.TiposEmpresas, Optional ByVal idPais_ As String = "MEX")

            TipoEmpresa = tipoEmpresa_

            If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                EmpresaNacional = New EmpresaNacional64

                EmpresaNacional.paisesdomicilios = New List(Of PaisDomicilio) From {(New PaisDomicilio With {.idpais = ObjectId.GenerateNewId, .sec = 1, .pais = "MEX", .domicilios = New List(Of Domicilio64), .estado = 1, .archivado = False})}

            Else

                EmpresaInternacional = New EmpresaInternacional64

            End If

            Estado = New TagWatcher

        End Sub

        Sub New(ByVal empresa_ As IEmpresaNacional64,
                ByVal tipoEmpresa_ As IControladorEmpresas64.TiposEmpresas)

            EmpresaNacional = empresa_

            TipoEmpresa = tipoEmpresa_

            Estado = New TagWatcher

        End Sub

        Sub New(ByVal empresa_ As IEmpresaInternacional64,
                ByVal tipoEmpresa_ As IControladorEmpresas64.TiposEmpresas)

            EmpresaInternacional = empresa_

            TipoEmpresa = tipoEmpresa_

            Estado = New TagWatcher

        End Sub
#End Region


#Region "Métodos privados"

        Private Function GuardarEmpresaNacional(ByVal empresaNacional_ As EmpresaNacional64,
                                                Optional ByVal session_ As IClientSessionHandle = Nothing) _
                                                As EmpresaNacional64
            With Estado

                ListaEmpresasNacionales = New List(Of EmpresaNacional64)

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


                    Dim filter_ = Builders(Of EmpresaNacional64).Filter.Eq(Function(x) x.razonsocial, empresaNacional_.razonsocial)

                    ListaEmpresasNacionales = operationsDB_.Find(filter_).Limit(1).ToList()

                    If ListaEmpresasNacionales.Count <> 0 Then

                        .SetOK()

                        EmpresaNacional = ListaEmpresasNacionales(0)

                    Else

                        .SetError(Me, "Ha ocurrido un error")

                    End If

                End Using

            End With

            Return EmpresaNacional

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

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional64)("Glo007EmpresasNacionales")

                    session_ = operationsDB_.Database.Client.StartSession

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
                                     AddToSet(Of Contacto64)("contactos", .contactos(0)).
                                     AddToSet(Of Rfc64)("rfcs", .rfcs(0)).
                                     AddToSet(Of RegimenFiscal64)("regimenesfiscales", .regimenesfiscales(0)).
                                     AddToSet(Of Curp64)("curps", .curps(0)).
                                     AddToSet("paisesdomicilios.0.domicilios", empresaNacional_.paisesdomicilios(0).domicilios(0))

                    Dim result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_, New UpdateOptions With {.IsUpsert = True}).Result

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

                    Dim setStructureOfSubs_ = Builders(Of EmpresaInternacional64).Update.
                                     Set(Function(x) x.razonsocial, .razonsocial).
                                     Set(Function(x) x.razonsocialcorto, .razonsocialcorto).
                                     Set(Function(x) x.abreviatura, .abreviatura).
                                     Set(Function(x) x.nombrecomercial, .nombrecomercial).
                                     Set(Function(x) x.girocomercial, .girocomercial).
                                     Set(Function(x) x.bu, .bu).
                                     AddToSet(Of PaisDomicilio)("paisesdomicilios", .paisesdomicilios(0)).
                                     AddToSet(Of Domicilio64)("paisesdomicilios.$.domicilios", .paisesdomicilios(0).domicilios(0)).
                                     AddToSet(Of Contacto64)("contactos", .contactos(0)).
                                     AddToSet(Of TaxId64)("taxids", .taxids(0)).
                                     AddToSet(Of Bus64)("bus", .bus(0))

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

        'Busqueda Externa
        Private Function BuscarEmpresasNacionales(ByRef token_ As String) As List(Of SelectOption)

            With Estado

                ListaEmpresasNacionales = New List(Of EmpresaNacional64)

                _listaEmpresas = New List(Of SelectOption)

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

                    Dim queryExpr = New BsonRegularExpression(New Regex(token_, RegexOptions.IgnoreCase))

                    Dim filter_ = Builders(Of EmpresaNacional64).Filter.Regex("razonsocial", queryExpr) And
                                  Builders(Of EmpresaNacional64).Filter.Eq(Function(x) x.estado, 1)

                    ListaEmpresasNacionales = operationsDB_.Find(filter_).Limit(10).ToList()

                    If ListaEmpresasNacionales.Count > 0 Then

                        For Each empresa_ In ListaEmpresasNacionales

                            _listaEmpresas.Add(New SelectOption With {.Value = empresa_._idempresa, .Text = empresa_.razonsocial})

                        Next

                        If _listaEmpresas.Count <> 0 Then

                            .SetOK()

                        Else

                            .SetError(Me, "No se generó lista de empresas nacionales")

                        End If

                    Else

                        .SetError(Me, "No se encontraron empresas nacinales")

                    End If

                End Using

            End With

            Return _listaEmpresas

        End Function

        Private Function BuscarEmpresasInternacionales(ByRef token_ As String) _
                                                       As List(Of SelectOption)

            With Estado

                ListaEmpresasInternacionales = New List(Of EmpresaInternacional64)

                _listaEmpresas = New List(Of SelectOption)

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

                    Dim queryExpr = New BsonRegularExpression(New Regex(token_, RegexOptions.IgnoreCase))

                    Dim filter_ = Builders(Of EmpresaInternacional64).Filter.Regex("razonsocial", queryExpr) And
                                  Builders(Of EmpresaInternacional64).Filter.Eq(Function(x) x.estado, 1)

                    ListaEmpresasInternacionales = operationsDB_.Find(filter_).Limit(10).ToList()

                    If ListaEmpresasInternacionales.Count > 0 Then

                        For Each empresa_ In ListaEmpresasInternacionales

                            _listaEmpresas.Add(New SelectOption With {.Value = empresa_._idempresa, .Text = empresa_.razonsocial})

                        Next

                        If _listaEmpresas.Count <> 0 Then

                            .SetOK()

                        Else

                            .SetError(Me, "No se generó lista de empresas internacionales")

                        End If

                    Else

                        .SetError(Me, "No se encontraron empresas internacionales")

                    End If

                End Using

            End With

            Return _listaEmpresas

        End Function


        Private Function BuscarEmpresaNacional(ByRef idEmpresa_ As ObjectId,
                                               ByRef idPais_ As ObjectId,
                                               ByRef idDomicilio_ As ObjectId) _
                                               As EmpresaNacional64

            With Estado

                EmpresaNacional = New EmpresaNacional64

                ListaEmpresasNacionales = New List(Of EmpresaNacional64)


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

                    Dim filter_ = Builders(Of EmpresaNacional64).Filter.Eq(Function(x) x._id, idEmpresa_)

                    ListaEmpresasNacionales = operationsDB_.Find(filter_).ToList()

                    If ListaEmpresasNacionales.Count > 0 Then

                        If Not ListaEmpresasNacionales(0) Is Nothing Then

                            _listaDomicilios = New List(Of SelectOption)

                            For Each domicilioItem_ In ListaEmpresasNacionales(0).paisesdomicilios

                                If domicilioItem_.idpais = idPais_ Then

                                    For Each domicilio_ In domicilioItem_.domicilios

                                        _listaDomicilios.Add(New SelectOption With
                                           {.Value = domicilio_.sec,
                                           .Text = domicilio_.calle & " No." &
                                                   domicilio_.numeroexterior & " " &
                                                   domicilio_.codigopostal & " " &
                                                   domicilio_.colonia & "," &
                                                   domicilio_.ciudad & ", " &
                                                   domicilio_.pais})

                                        If domicilio_._iddomicilio = idDomicilio_ Then

                                            Estado.ObjectReturned = _listaDomicilios

                                        End If


                                    Next

                                Else

                                    .SetError(Me, "No se encontró país")

                                End If

                            Next

                        End If

                    End If

                End Using

            End With

            Return EmpresaNacional

        End Function

        Private Function BuscarEmpresaInternacional(ByRef idEmpresa_ As ObjectId,
                                                    ByRef idPais_ As ObjectId,
                                                    ByRef idDomicilio_ As ObjectId) _
                                                    As EmpresaInternacional64

            With Estado

                EmpresaInternacional = New EmpresaInternacional64

                ListaEmpresasInternacionales = New List(Of EmpresaInternacional64)


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

                    Dim filter_ = Builders(Of EmpresaInternacional64).Filter.Eq(Function(x) x._id, idEmpresa_)

                    ListaEmpresasInternacionales = operationsDB_.Find(filter_).ToList()

                    If ListaEmpresasInternacionales.Count > 0 Then

                        If Not ListaEmpresasInternacionales(0) Is Nothing Then

                            _listaDomicilios = New List(Of SelectOption)

                            For Each domicilioItem_ In ListaEmpresasInternacionales(0).paisesdomicilios

                                If domicilioItem_.idpais = idPais_ Then

                                    For Each domicilio_ In domicilioItem_.domicilios

                                        _listaDomicilios.Add(New SelectOption With
                                           {.Value = domicilio_.sec,
                                           .Text = domicilio_.calle & " No." &
                                                   domicilio_.numeroexterior & " " &
                                                   domicilio_.codigopostal & " " &
                                                   domicilio_.colonia & "," &
                                                   domicilio_.ciudad & ", " &
                                                   domicilio_.pais})

                                        If domicilio_._iddomicilio = idDomicilio_ Then

                                            Estado.ObjectReturned = _listaDomicilios

                                        End If


                                    Next

                                Else

                                    .SetError(Me, "No se encontró país")

                                End If

                            Next

                        End If

                    End If

                End Using

            End With

            Return EmpresaInternacional

        End Function


        'Busqueda Interna
        Private Function BuscarDomiciliosInternos(ByVal cveEmpresa_ As Integer,
                                                  ByVal cvePais_ As Integer) _
                                                  As List(Of SelectOption)

            With Estado

                _listaPaisesDomicilios = New List(Of PaisDomicilio)

                _listaDomicilios = New List(Of SelectOption)

                If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                    _listaPaisesDomicilios = ListaEmpresasNacionales.Where(Function(t) t._idempresa = cveEmpresa_).Select(Function(t) t.paisesdomicilios)

                Else

                    _listaPaisesDomicilios = ListaEmpresasInternacionales.Where(Function(t) t._idempresa = cveEmpresa_).Select(Function(t) t.paisesdomicilios)

                End If

                If _listaPaisesDomicilios.Count > 0 Then

                    For Each domicilios_ In _listaPaisesDomicilios

                        If domicilios_.sec = cvePais_ Then

                            Dim stringdomicilio_ As String

                            For Each domicilio_ In domicilios_.domicilios

                                With domicilio_

                                    stringdomicilio_ = .calle & " " &
                                                       .numeroexterior & " " &
                                                       .codigopostal & " " &
                                                       .colonia & " " &
                                                       .ciudad & " " &
                                                       .pais

                                    stringdomicilio_ = stringdomicilio_.Replace("  ", " ")

                                    _listaDomicilios.Add(
                                                            New SelectOption With
                                                            {.Value = domicilio_.sec,
                                                                .Text = stringdomicilio_
                                                            })

                                End With

                            Next

                        Else

                            .SetError(Me, "País no encontrado")

                            Return _listaDomicilios

                        End If

                    Next

                    If _listaDomicilios.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se llenó la lista de domicilios") : End If

                Else

                    .SetError("No se encontró empresa")

                End If

            End With

            Return _listaDomicilios

        End Function

        Private Function BuscarDomicilioInterno(ByVal cveEmpresa_ As Int32,
                                                ByVal cvePais_ As Int32,
                                                ByVal cveDomicilio_ As Int32) _
                                                As Domicilio64

            With Estado

                _listaPaisesDomicilios = New List(Of PaisDomicilio)

                _listaDomicilios = New List(Of SelectOption)

                _domicilio = New Domicilio64

                If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                    _listaPaisesDomicilios = ListaEmpresasNacionales.Where(Function(t) t._idempresa = cveEmpresa_).Select(Function(t) t.paisesdomicilios)

                Else

                    _listaPaisesDomicilios = ListaEmpresasInternacionales.Where(Function(t) t._idempresa = cveEmpresa_).Select(Function(t) t.paisesdomicilios)

                End If

                If _listaPaisesDomicilios.Count > 0 Then

                    For Each domicilios_ In _listaPaisesDomicilios

                        If domicilios_.sec = cvePais_ Then

                            For Each domicilio_ In domicilios_.domicilios

                                With domicilio_

                                    If .sec = cveDomicilio_ Then

                                        _domicilio = domicilio_

                                    Else

                                        Estado.SetError(Me, "Ocurrió un error al encontrar el domicilio")

                                        Return _domicilio

                                    End If

                                End With

                            Next

                        Else

                            .SetError(Me, "País no encontrado")

                            Return _domicilio

                        End If

                    Next

                    If _listaDomicilios.Count > 0 Then : .SetOK() : Else .SetOKBut(Me, "No se encontró domicilio") : End If

                Else

                    .SetError("No se encontró empresa")

                End If

            End With

            Return _domicilio

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

        Public Function BuscarEmpresas(token_ As String) _
                                       As TagWatcher _
                                       Implements IControladorEmpresas64.BuscarEmpresas

            With Estado

                If token_ IsNot Nothing Then

                    If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                        .ObjectReturned = BuscarEmpresasNacionales(token_)


                    ElseIf TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Internacional Then

                        .ObjectReturned = BuscarEmpresasInternacionales(token_)

                    Else

                        .SetOKBut(Me, "No se recibió un tipo de empresa")

                    End If

                Else

                    .SetOKBut(Me, "No se recibió token")

                End If

            End With

            Return Estado

        End Function

        Public Function BuscarEmpresa(idEmpresa_ As ObjectId,
                                      idPais_ As ObjectId,
                                      idDomicilio_ As ObjectId) _
                                      As TagWatcher _
                                      Implements IControladorEmpresas64.BuscarEmpresa

            With Estado

                If Not idEmpresa_ = ObjectId.Empty And
                    Not idPais_ = ObjectId.Empty And
                    Not idDomicilio_ = ObjectId.Empty Then

                    If TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Nacional Then

                        .ObjectReturned = BuscarEmpresaNacional(idEmpresa_, idPais_, idDomicilio_)


                    ElseIf TipoEmpresa = IControladorEmpresas64.TiposEmpresas.Internacional Then

                        .ObjectReturned = BuscarEmpresaInternacional(idEmpresa_, idPais_, idDomicilio_)

                    Else

                        .SetOKBut(Me, "No se ha administrado un tipo de empresa")

                    End If

                Else

                    .SetOKBut(Me, "No se recibió una empresa")

                End If

            End With

            Return Estado

        End Function

        Public Function BuscarDomicilios(ByVal cveEmpresa_ As Int32,
                                         ByVal cvePais_ As Int32) _
                                         As TagWatcher _
                                         Implements IControladorEmpresas64.BuscarDomicilios

            With Estado

                If cveEmpresa_ <> 0 And
                    cvePais_ <> 0 Then

                    If ListaEmpresasNacionales.Count > 0 Or
                        ListaEmpresasInternacionales.Count > 0 Then

                        .ObjectReturned = BuscarDomiciliosInternos(cveEmpresa_, cvePais_)

                    Else

                        .SetOKBut(Me, "No tiene empresas cargadas internamente")

                    End If

                Else

                    .SetOKBut(Me, "No se envío una clave de empresa válida")

                End If

            End With

            Return Estado

        End Function

        Public Function BuscarDomicilio(cveEmpresa_ As Int32,
                                        cvePais_ As Int32,
                                        cveDomicilio_ As Int32) _
                                        As TagWatcher _
                                        Implements IControladorEmpresas64.BuscarDomicilio

            With Estado

                If cveEmpresa_ <> 0 And
                    cvePais_ <> 0 And
                    cveDomicilio_ <> 0 Then

                    If ListaEmpresasNacionales.Count > 0 Or
                       ListaEmpresasInternacionales.Count > 0 Then

                        .ObjectReturned = BuscarDomicilioInterno(cveEmpresa_, cvePais_, cveDomicilio_)

                    Else

                        .SetOKBut(Me, "No tiene empresas cargadas internamente")

                    End If

                Else

                    .SetOKBut(Me, "No se envío una clave de empresa válida")

                End If

            End With

            Return Estado

        End Function

        Public Function ArchivarRegistro(objectidRegistro_ As ObjectId,
                                         tipoRegistro_ As Integer) _
                                         As TagWatcher _
                                         Implements IControladorEmpresas64.ArchivarRegistro

            Throw New NotImplementedException()
        End Function

        Public Function ArchivarRegistros(objectsIdRegistros_ As List(Of ObjectId),
                                          tipoRegistro_ As Integer) _
                                          As TagWatcher _
                                          Implements IControladorEmpresas64.ArchivarRegistros

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