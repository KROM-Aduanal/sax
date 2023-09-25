Imports System.IO
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports MongoDB.Bson.Serialization.Attributes
Imports Wma.Exceptions
Imports Rec.Globals.Utils
Imports Wma.Exceptions.TagWatcher
Imports gsol

Namespace Rec.Globals.Controllers


    ''' <summary>
    ''' 'INTERFACE EMPRESA
    ''' </summary>

#Region "Interfaces"

    Public Interface IEmpresa64 : Inherits IDisposable

#Region "Enums"

        Enum TiposEmpresas

            Nacional = 1

            Internacional = 2

        End Enum

#End Region

#Region "Propiedades"
        Property _id As ObjectId

        Property _idempresa As Int32

        <BsonIgnoreIfNull>
        Property _idempresakb As Int32

        Property razonsocial As String

        Property razonsocialcorto As String

        <BsonIgnoreIfNull>
        Property abreviatura As String

        <BsonIgnoreIfNull>
        Property razonsocialingles As String

        Property nombrecomercial As String

        Property domicilios As List(Of Domicilio64)

        <BsonIgnoreIfNull>
        Property girocomercial As String

        <BsonIgnoreIfNull>
        Property _idgrupocomercial As ObjectId

        <BsonIgnoreIfNull>
        Property contactos As List(Of Contacto64)

        Property abierto As Boolean

        Property estado As Int16

        Property archivado As Boolean

        <BsonIgnore>
        Property TipoEmpresa As TiposEmpresas

        <BsonIgnore>
        Property TagWatcher As TagWatcher

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

        Function BuscarEmpresa(ByRef empresa_ As List(Of Empresa64)) _
                               As List(Of SelectOption)

#End Region

    End Interface

    ''' <summary>
    ''' 'INTERFACE EMPRESA NACIONAL
    ''' </summary>
    ''' 


    Public Interface IEmpresaNacional64 : Inherits IDisposable

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
        Property _idcurp As ObjectId

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

    Public Interface IEmpresaInternacional64 : Inherits IDisposable

        Property _idtaxid As ObjectId

        Property taxid As String

        Property taxids As List(Of TaxId64)

        <BsonIgnoreIfNull>
        Property _idbu As ObjectId

        <BsonIgnoreIfNull>
        Property bu As String

        <BsonIgnoreIfNull>
        Property bus As List(Of Bus64)

    End Interface

#End Region



    ''' <summary>
    ''' 'CLASE EMPRESA
    ''' </summary>
    ''' 



    Public MustInherit Class Empresa64
        Implements IEmpresa64

#Region "Propiedades"

        Private disposedValue As Boolean

        Public Property _id As ObjectId _
            Implements IEmpresa64._id

        Public Property _idempresa As Integer _
            Implements IEmpresa64._idempresa

        Public Property _idempresakb As Integer _
            Implements IEmpresa64._idempresakb

        Public Property razonsocial As String _
            Implements IEmpresa64.razonsocial

        Public Property razonsocialcorto As String _
            Implements IEmpresa64.razonsocialcorto

        Public Property abreviatura As String _
            Implements IEmpresa64.abreviatura

        Public Property razonsocialingles As String _
            Implements IEmpresa64.razonsocialingles

        Public Property nombrecomercial As String _
            Implements IEmpresa64.nombrecomercial

        Public Property domicilios As List(Of Domicilio64) _
            Implements IEmpresa64.domicilios

        Public Property girocomercial As String _
            Implements IEmpresa64.girocomercial

        Public Property _idgrupocomercial As ObjectId _
            Implements IEmpresa64._idgrupocomercial

        Public Property contactos As List(Of Contacto64) _
            Implements IEmpresa64.contactos

        Public Property abierto As Boolean _
            Implements IEmpresa64.abierto

        Public Property estado As Short _
            Implements IEmpresa64.estado

        Public Property archivado As Boolean _
            Implements IEmpresa64.archivado

        Public Property TipoEmpresa As IEmpresa64.TiposEmpresas _
            Implements IEmpresa64.TipoEmpresa

        Public Property TagWatcher As TagWatcher _
            Implements IEmpresa64.TagWatcher

#End Region

#Region "Constructores"

        Sub New(Optional ByVal tiposEmpresa_ _
                As IEmpresa64.TiposEmpresas = IEmpresa64.TiposEmpresas.Nacional)

            Inicializa(tiposEmpresa_)

        End Sub

        Public Sub Inicializa(ByVal tipoEmpresa_ _
                              As IEmpresa64.TiposEmpresas)

            TipoEmpresa = tipoEmpresa_

            TagWatcher = New TagWatcher

        End Sub
#End Region

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

#Region "MÉTODOS PRIVADOS"

        Private Function GuardarEmpresa(ByRef empresa_ As EmpresaNacional64,
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

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaNacional64)("Glo007Empresas")

                session_ = operationsDB_.Database.Client.StartSession

                Dim result_ = operationsDB_.InsertOneAsync(session_, empresa_).ConfigureAwait(False)

                TagWatcher.SetOK()

            End Using

            Return TagWatcher

        End Function

        Private Function GuardarEmpresa(ByRef empresa_ As EmpresaInternacional64,
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

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional64)("Glo007Empresas")

                session_ = operationsDB_.Database.Client.StartSession

                Dim result_ = operationsDB_.InsertOneAsync(session_, empresa_).ConfigureAwait(False)

                TagWatcher.SetOK()

            End Using

            Return TagWatcher

        End Function

        Private Function ModificarEmpresa(ByRef empresa_ As EmpresaInternacional64,
                                          Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher


            Dim iespacioTrabajo_ As IEspacioTrabajo = New EspacioTrabajo _
                                                      With {
                                                            .Aplicacion = 4,
                                                            .DivisionEmpresarial = 1,
                                                            .ClaveEjecutivo = 139,
                                                            .Idioma = 1,
                                                            .ModalidadEspacio = IEspacioTrabajo.ModalidadesEspacio.Pruebas
                                                            }

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With {.EspacioTrabajo = iespacioTrabajo_}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of EmpresaInternacional64)("Glo007Empresas")

                Dim filter_ = Builders(Of EmpresaInternacional64).Filter.Eq(Function(x) x._id, empresa_._id)

                Dim setStructureOfSubs_ = Builders(Of EmpresaInternacional64).Update.
                                     Set(Function(x) x.razonsocial, empresa_.razonsocial).
                                     Set(Function(x) x.razonsocialcorto, empresa_.razonsocialcorto).
                                     Set(Function(x) x.nombrecomercial, empresa_.nombrecomercial).
                                     Set(Function(x) x._idtaxid, empresa_._idtaxid).
                                     Set(Function(x) x.taxid, empresa_.taxid).
                                     AddToSet("taxids", New TaxId64 With {.idtaxid = empresa_._idtaxid, .taxid = empresa_.taxid})


                Dim result_ = operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_).Result

                With TagWatcher

                    If result_.MatchedCount <> 0 Then

                        .SetOK()

                    ElseIf result_.UpsertedId IsNot Nothing Then

                        .SetOK()

                    Else

                        .SetError(Me, "No se generaron cambios")

                    End If

                End With

            End Using

            Return TagWatcher

        End Function

#End Region

#Region "METODOS PÚBLLICOS"
        Public Function NuevaEmpresa(empresa_ As IEmpresaNacional64,
                                 Optional session_ As IClientSessionHandle = Nothing) _
                                 As TagWatcher _
                                 Implements IEmpresa64.NuevaEmpresa

            With TagWatcher

                If TipoEmpresa = 1 Then


                    .ObjectReturned = GuardarEmpresa(DirectCast(empresa_, EmpresaNacional64))

                Else

                    .SetOKBut(Me, "No se recibió una empresa nacional")

                End If

                Return TagWatcher

            End With

        End Function

        Public Function NuevaEmpresa(empresa_ As IEmpresaInternacional64,
                                 Optional session_ As IClientSessionHandle = Nothing) _
                                 As TagWatcher _
                                 Implements IEmpresa64.NuevaEmpresa

            With TagWatcher

                If TipoEmpresa = 2 Then

                    .ObjectReturned = GuardarEmpresa(DirectCast(empresa_, EmpresaInternacional64))

                Else

                    .SetOKBut(Me, "No se recibió una empresa internacional")

                End If

                Return TagWatcher

            End With

        End Function

        Public Function NuevaEmpresa(Optional session_ As IClientSessionHandle = Nothing) _
                                As TagWatcher _
                                Implements IEmpresa64.NuevaEmpresa
            Throw New NotImplementedException()
        End Function

        Public Function ActualizaEmpresa(empresa_ As IEmpresaNacional64,
                                     Optional session_ As IClientSessionHandle = Nothing) _
                                     As TagWatcher _
                                     Implements IEmpresa64.ActualizaEmpresa
            Throw New NotImplementedException()
        End Function

        Public Function ActualizaEmpresa(empresa_ As IEmpresaInternacional64,
                                     Optional session_ As IClientSessionHandle = Nothing) _
                                     As TagWatcher _
                                     Implements IEmpresa64.ActualizaEmpresa

            ModificarEmpresa(empresa_)

        End Function

        Public Function BuscarEmpresa(ByRef empresa_ As List(Of Empresa64)) _
                                  As List(Of SelectOption) _
                                  Implements IEmpresa64.BuscarEmpresa
            Throw New NotImplementedException()
        End Function

#End Region

    End Class




    ''' <summary>
    ''' 'CLASE EMPRESA NACIONAL
    ''' </summary>
    ''' 



    Public Class EmpresaNacional64 : Inherits Empresa64
        Implements IEmpresa64, IEmpresaNacional64

#Region "Propiedades"

        Public Property _idrfc As ObjectId _
        Implements IEmpresaNacional64._idrfc

        Public Property rfc As String _
        Implements IEmpresaNacional64.rfc

        Public Property rfcs As List(Of Rfc64) _
        Implements IEmpresaNacional64.rfcs

        Public Property _idcurp As ObjectId _
        Implements IEmpresaNacional64._idcurp

        Public Property curp As String _
        Implements IEmpresaNacional64.curp

        Public Property curps As List(Of Curp64) _
        Implements IEmpresaNacional64.curps

        Public Property regimenfiscal As List(Of RegimenFiscal64) _
        Implements IEmpresaNacional64.regimenfiscal

        Public Property tipopersona As IEmpresaNacional64.TiposPersona64 _
        Implements IEmpresaNacional64.tipopersona

#End Region

#Region "Constructores"

        Sub New()

        End Sub

        Sub New(ByVal tipoEmpresa_ As IEmpresa64.TiposEmpresas)

            Inicializa(tipoEmpresa_)

        End Sub

#End Region

    End Class



    ''' <summary>
    ''' 'CLASE EMPRESA INTERNACIONAL
    ''' </summary>
    ''' 


    Public Class EmpresaInternacional64 : Inherits Empresa64
        Implements IEmpresa64, IEmpresaInternacional64

        Private disposedValue As Boolean

        Public Property _idtaxid As ObjectId _
        Implements IEmpresaInternacional64._idtaxid

        Public Property taxid As String _
        Implements IEmpresaInternacional64.taxid

        Public Property taxids As List(Of TaxId64) _
        Implements IEmpresaInternacional64.taxids

        Public Property _idbu As ObjectId _
        Implements IEmpresaInternacional64._idbu

        Public Property bu As String _
        Implements IEmpresaInternacional64.bu

        Public Property bus As List(Of Bus64) _
        Implements IEmpresaInternacional64.bus


#Region "Constructores"

        Public Sub New()


        End Sub

        Sub New(tipoEmpresa_ As IEmpresa64.TiposEmpresas)

            Inicializa(tipoEmpresa_)

        End Sub

#End Region

    End Class


    ''' <summary>
    ''' 'VEHICULOS
    ''' </summary>
    ''' 


#Region "Vehículos"
    Public Class Bus64

        Property idunidadnegocio As ObjectId

        Property sec As Integer

        Property unidadnegocio As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class

    Public Class Contacto64

        Property idcontacto As ObjectId

        Property sec As Integer

        Property nombrecompleto As String

        Property correoelectronico As String

        Property telefono As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class

    Public Class Domicilio64

        Property iddomicilio As ObjectId

        <BsonIgnoreIfNull>
        Property iddivisionkb As Integer

        Property sec As Integer

        Property calle As String

        Property numeroexterior As String

        <BsonIgnoreIfNull>
        Property numerointerior As String

        <BsonIgnoreIfNull>
        Property colonia As String

        Property codigopostal As String

        Property ciudad As String

        <BsonIgnoreIfNull>
        Property localidad As String

        <BsonIgnoreIfNull>
        Property municipio As String

        Property entidadfederativa As String

        Property pais As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class

    Public Class Rfc64

        Property idrfc As ObjectId

        Property sec As Integer

        Property rfc As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class

    Public Class Curp64

        Property idcurp As ObjectId

        Property sec As Integer

        Property curp As String

        Property estado As Integer

        Property archivado As Boolean = False

    End Class

    Public Class RegimenFiscal64

        Property idregimenfiscal As ObjectId

        Property _sec As Integer

        Property regimenfiscal As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class

    Public Class TaxId64
        Property idtaxid As ObjectId

        Property sec As Integer

        Property taxid As String

        Property estado As Int16

        Property archivado As Boolean = False

    End Class

#End Region
End Namespace