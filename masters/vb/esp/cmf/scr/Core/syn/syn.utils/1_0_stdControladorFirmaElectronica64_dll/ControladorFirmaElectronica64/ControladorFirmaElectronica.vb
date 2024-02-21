Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Wma.Exceptions
Imports System.IO
Imports System.Security.Cryptography
Imports gsol.seguridad

Public Class ControladorFirmaElectronica
    Implements IDisposable

#Region "Enums"

    Enum AccionesFirma
        Firmar = 1
        FirmarPublicar = 2
    End Enum

#End Region

#Region "Atributos"

    Private _disposedValue As Boolean

    Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance()

    Private _estado As TagWatcher

    Private _resultado As UpdateResult

    Private _documentosAsociados As List(Of DocumentoAsociado)

    Private _gruposfirmas As Dictionary(Of String, List(Of ObjectId))

    Private _errores As List(Of String)

    Private _coleccionOperaciones As IMongoCollection(Of OperacionGenerica)

    Private _filtro As FilterDefinition(Of OperacionGenerica)

    Private _resultados As List(Of OperacionGenerica)

#End Region

#Region "Propiedades"
#End Region

#Region "Constructores"

    Sub New()

        _estado = New TagWatcher()

        _documentosAsociados = New List(Of DocumentoAsociado)

        _gruposfirmas = New Dictionary(Of String, List(Of ObjectId))

        _errores = New List(Of String)

        _resultados = New List(Of OperacionGenerica)

    End Sub

#End Region

#Region "Métodos"

    Public Function Generar(ByVal iddocumento_ As ObjectId,
                            ByVal claveusuario_ As Int32) As String

        Dim firma_ = claveusuario_.ToString & iddocumento_.ToString & ObtenerFechaActual() & "v1"

        Dim cifrado_ = New Cifrado256()

        Return cifrado_.CifraCadena(firma_, ICifrado.Metodos.AES)

    End Function

    Public Function FirmarDocumento(Of T)(ByVal iddocumento_ As ObjectId,
                                          ByVal claveusuario_ As Int32,
                                          Optional ByVal accionFirma_ As AccionesFirma = AccionesFirma.FirmarPublicar,
                                          Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim firma_ = Generar(iddocumento_, claveusuario_.ToString)

            _coleccionOperaciones = iEnlace_.GetMongoCollection(Of OperacionGenerica)(GetType(T).Name)

            _filtro = Builders(Of OperacionGenerica).Filter.Eq(Of ObjectId)("_id", iddocumento_)

            Dim estructuraOperacion_ As UpdateDefinition(Of OperacionGenerica)

            If accionFirma_ = AccionesFirma.FirmarPublicar Then

                estructuraOperacion_ = Builders(Of OperacionGenerica).Update.Set(Function(x) x.FirmaElectronica, firma_).
                                                                             Set(Function(x) x.Publicado, True)
            Else

                estructuraOperacion_ = Builders(Of OperacionGenerica).Update.Set(Function(x) x.FirmaElectronica, firma_)

            End If

            If session_ IsNot Nothing Then

                _resultado = _coleccionOperaciones.UpdateOneAsync(session_, _filtro, estructuraOperacion_).Result

            Else

                _resultado = _coleccionOperaciones.UpdateOneAsync(_filtro, estructuraOperacion_).Result

            End If

            If _resultado.ModifiedCount > 0 Then

                With _estado

                    .SetOK()

                    .ObjectReturned = firma_

                End With

            Else

                _estado.SetOKBut(Me, "Firma no generada.")

            End If

            Return _estado

        End Using

    End Function

    Public Function FirmarDocumento(ByVal recurso_ As String,
                                    ByVal iddocumento_ As ObjectId,
                                    ByVal claveusuario_ As Int32,
                                    Optional ByVal accionFirma_ As AccionesFirma = AccionesFirma.FirmarPublicar,
                                    Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim firma_ = Generar(iddocumento_, claveusuario_.ToString)

            _coleccionOperaciones = iEnlace_.GetMongoCollection(Of OperacionGenerica)(recurso_)

            _filtro = Builders(Of OperacionGenerica).Filter.Eq(Of ObjectId)("_id", iddocumento_)

            Dim estructuraOperacion_ As UpdateDefinition(Of OperacionGenerica)

            If accionFirma_ = AccionesFirma.FirmarPublicar Then

                estructuraOperacion_ = Builders(Of OperacionGenerica).Update.Set(Function(x) x.FirmaElectronica, firma_).
                                                                             Set(Function(x) x.Publicado, True)
            Else

                estructuraOperacion_ = Builders(Of OperacionGenerica).Update.Set(Function(x) x.FirmaElectronica, firma_)

            End If

            If session_ IsNot Nothing Then

                _resultado = _coleccionOperaciones.UpdateOneAsync(session_, _filtro, estructuraOperacion_).Result

            Else

                _resultado = _coleccionOperaciones.UpdateOneAsync(_filtro, estructuraOperacion_).Result

            End If

            If _resultado.ModifiedCount > 0 Then

                With _estado

                    .SetOK()

                    .ObjectReturned = firma_

                End With

            Else

                _estado.SetOKBut(Me, "Firma no generada.")

            End If

            Return _estado

        End Using

    End Function

    Public Function Validar(Of T)(ByVal iddocumento_ As ObjectId,
                                                  ByVal firmaElectronica_ As String,
                                                  Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            _coleccionOperaciones = iEnlace_.GetMongoCollection(Of OperacionGenerica)(GetType(T).Name)

            _filtro = Builders(Of OperacionGenerica).Filter.Eq(Of Object)("_id", iddocumento_)

            _resultados = New List(Of OperacionGenerica)

            If session_ IsNot Nothing Then

                _resultados = _coleccionOperaciones.Find(session_, _filtro).ToList()

            Else

                _resultados = _coleccionOperaciones.Find(_filtro).ToList()

            End If

            If _resultados.Count > 0 Then

                If _resultados(0).FirmaElectronica.Equals(firmaElectronica_) Then

                    _estado.SetOK()

                Else

                    _estado.SetError(Me, "La firma electrónica no es válida.")

                End If

            Else

                _estado.SetError(Me, "Elemento no encontrado para validar firma.")

            End If

            Return _estado

        End Using

    End Function

    Public Function ValidarFirmasAsociadas(ByVal operacionGenerica_ As OperacionGenerica,
                                           Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher


        If operacionGenerica_ Is Nothing Then

            _estado.SetError(Me, "Documento no válido")

            Return _estado

        End If

        If operacionGenerica_.Borrador.Folder.DocumentosAsociados Is Nothing Then

            _estado.SetError(Me, "Este documento no contiene ningun documento asociado")

            Return _estado

        End If

        If operacionGenerica_.Borrador.Folder.DocumentosAsociados.Count = 0 Then

            _estado.SetError(Me, "Este documento no contiene ningun documento asociado")

            Return _estado

        End If

        _documentosAsociados = operacionGenerica_.Borrador.Folder.DocumentosAsociados

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            _gruposfirmas = New Dictionary(Of String, List(Of ObjectId))

            _errores = New List(Of String)

            For Each documentoasociado_ As DocumentoAsociado In _documentosAsociados

                Dim nombreColeccion_ As String = Nothing : Dim nombreBaseDatos_ As String = Nothing

                _statements.GetDatabaseAndCollectionName(nombreBaseDatos_, nombreColeccion_, documentoasociado_.identificadorrecurso, Nothing)

                _coleccionOperaciones = iEnlace_.GetMongoClient().
                                             GetDatabase(nombreBaseDatos_).
                                             GetCollection(Of OperacionGenerica)(nombreColeccion_)

                _filtro = Builders(Of OperacionGenerica).Filter.Eq(Of String)("FirmaElectronica", documentoasociado_._iddocumentoasociado)

                _resultados = New List(Of OperacionGenerica)

                If session_ IsNot Nothing Then

                    _resultados = _coleccionOperaciones.Find(session_, _filtro).ToList()

                Else

                    _resultados = _coleccionOperaciones.Find(_filtro).ToList()

                End If

                If Not _resultados.Count = 1 Then

                    _errores.Add("El documento [" & documentoasociado_._iddocumentoasociado & "] no tienen una firma válida para la colección " & documentoasociado_._iddocumentoasociado)

                End If

            Next

            If _errores.Count > 0 Then

                With _estado

                    .SetOKBut(Me, "Inconsistencia en las firmas, al menos un documento ha sido alterado.")

                    .ObjectReturned = _errores

                End With

            Else

                _estado.SetOK()

            End If

            Return _estado

        End Using

    End Function

    Private Function ObtenerFechaActual() As Long

        Dim time = (DateTime.Now.ToUniversalTime() - New DateTime(1970, 1, 1))

        Return CLng((time.TotalMilliseconds + 0.5))

    End Function

    Protected Overridable Sub Dispose(disposing_ As Boolean)

        If Not _disposedValue Then

            If disposing_ Then
                ' TODO: eliminar el estado administrado (objetos administrados)

                _estado = Nothing

                _resultado = Nothing

                _documentosAsociados = Nothing

                _gruposfirmas = Nothing

                _errores = Nothing

                _coleccionOperaciones = Nothing

                _filtro = Nothing

                _resultados = Nothing

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL
            _disposedValue = True

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
        Dispose(disposing_:=True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region

End Class
