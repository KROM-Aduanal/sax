Imports System.Text.RegularExpressions
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
Imports Rec.Globals.Utils
Imports Wma.Exceptions

Public Class ControladorMonedas
    Implements IControladorMonedas, ICloneable, IDisposable

#Region "Enums"

    Public Enum CamposBusquedaSimple

        IDS = 1
        NOMBREESP = 2
        NOMBREING = 3
        CLAVES = 4
        PRESENTACION = 5
        FACTORDOLAR = 6
    End Enum

#End Region

#Region "Atributos"
    Private _monedas As List(Of MonedaGlobal)

    Private _tiposdeCambio As List(Of tipodecambioreciente)

    Private _factoresCambioRecientes As List(Of factorreciente)

    Private _consulta As List(Of String)

    Private _camposMonedas As Dictionary(Of String, List(Of String))

    Private _estado As TagWatcher

    Private _ultimoMatch As String

    Private _recursosGenerales As New Syn.Utils.Organismo

#End Region

#Region "Propiedades"

    Public Property FactoresCambioRecientes As List(Of factorreciente) Implements IControladorMonedas.FactoresCambioRecientes
        Get
            Return _factoresCambioRecientes
        End Get
        Set(value As List(Of factorreciente))
            _factoresCambioRecientes = value
        End Set
    End Property

    Public Property TiposdeCambio As List(Of tipodecambioreciente) Implements IControladorMonedas.TiposdeCambio
        Get
            Return _tiposdeCambio
        End Get
        Set(value As List(Of tipodecambioreciente))
            _tiposdeCambio = value
        End Set
    End Property

    Public Property Monedas As List(Of MonedaGlobal) Implements IControladorMonedas.Monedas
        Get
            Return _monedas
        End Get
        Set(value As List(Of MonedaGlobal))
            _monedas = value
        End Set
    End Property
    'Property factorMonedas As Dictionary(Of String, FactorMonedaPrincipal) Implements IControladorMonedas.factorMonedas
    '    Get
    '        Return _factorMonedas
    '    End Get
    '    Set(value As Dictionary(Of String, FactorMonedaPrincipal))
    '        _factorMonedas = value
    '    End Set
    'End Property

    Public Property Consulta(campo_ As [Enum],
                             Optional ByVal formato_ As String = "cvedefault",
                             Optional ByVal limite_ As Int32 = 5) As List(Of String) Implements IControladorMonedas.Consulta
        Get
            Return ConsultaMoneda(campo_, formato_, limite_)
        End Get
        Set(value As List(Of String))
            ' Falta'

        End Set
    End Property

    Property camposmonedas As Dictionary(Of String, List(Of String)) Implements IControladorMonedas.CamposMonedas
        Get
            Return _camposMonedas
        End Get
        Set(value As Dictionary(Of String, List(Of String)))
            _camposMonedas = value
        End Set
    End Property

    Property estado As TagWatcher Implements IControladorMonedas.Estado
        Get
            Return _estado
        End Get
        Set(value As TagWatcher)
            _estado = value
        End Set
    End Property

    Property UltimoMatch As String Implements IControladorMonedas.UltimoMatch
        Get
            Return _ultimoMatch
        End Get
        Set(value As String)
            _ultimoMatch = value
        End Set
    End Property
#End Region

#Region "Métodos"

    'Public Sub EstableceContextoActual(contextoactual_ As String) Implements IControladorMonedas.EstableceContextoActual
    '    _contextoactual = contextoactual_
    'End Sub


    'Public Sub ActualizaUltimaMoneda(MonedaId As ObjectId) Implements IControladorMonedas.ActualizaUltimaMoneda
    '    _ultimaMoneda = _Monedas.Find(Function(ch) ch._id = MonedaId)
    'End Sub
    Sub New()

    End Sub
    Public Sub ActualizaListMonedas(monedas_ As List(Of MonedaGlobal)) Implements IControladorMonedas.ActualizaListMonedas

        _monedas = monedas_

    End Sub

    Public Sub ActualizaListMonedasOnline(monedas_ As List(Of MonedaGlobal)) Implements IControladorMonedas.ActualizaListMonedasOnline

        _monedas = monedas_

    End Sub
#End Region

#Region "Funciones"
    Public Function BuscarMonedas(token_ As String, Optional tokenId_ As ObjectId = Nothing,
                           Optional formato_ As String = "cvedefault",
                           Optional ByVal ilimit_ As Int32 = 5) As List(Of MonedaGlobal) _
                           Implements IControladorMonedas.BuscarMonedas


        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            _camposMonedas = New Dictionary(Of String, List(Of String))

            _monedas = New List(Of MonedaGlobal)

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of MonedaGlobal)("Reg000Monedas")

            Dim aliasmonedas_ As String = "{$or:["

            If token_.Length <= 3 Then

                aliasmonedas_ = aliasmonedas_ & "{'aliasmoneda':{$elemMatch:{Valor:'" & token_.ToUpper & "', Clave:'" & formato_ & "'}}},"

            End If

            aliasmonedas_ = aliasmonedas_ & "{$and:[{$or:[" & _recursosGenerales.SeparacionPalabras(token_, "nombremonedaesp", "", "", "") & "," & _recursosGenerales.SeparacionPalabras(token_, "nombremonedaing", "", "", "") & "]},{'aliasmoneda':{$elemMatch:{Valor:{$ne:''}, Clave:'" & formato_ & "'}}}]},"

            If tokenId_ <> Nothing Then

                aliasmonedas_ = aliasmonedas_ & "{'_id':ObjectId('" & tokenId_.ToString & "')},"

            End If

            aliasmonedas_ = aliasmonedas_.Substring(0, aliasmonedas_.Length - 1) & "]}"

            If aliasmonedas_ <> _ultimoMatch Then

                _ultimoMatch = aliasmonedas_

                operationsDB_.Aggregate().Match(aliasmonedas_).SortBy(Function(ch) ch.idmoneda).Limit(ilimit_).
                    ToList().ForEach(Sub(moneda_)

                                         _monedas.Add(moneda_)

                                     End Sub)
            End If
        End Using

        Return _monedas

    End Function
    'Busca las Monedas por un token que si el token tiene un tamaño de 3 lo busca por Clave de Moneda y si es mayor lo busca por nombre

    Public Function BuscarMonedas(lttoken_ As List(Of String),
                                  Optional ltObjectId As List(Of ObjectId) = Nothing,
                                  Optional formato_ As String = "cvedefault",
                                  Optional limit_ As Int32 = 5) As List(Of MonedaGlobal) _
                                  Implements IControladorMonedas.BuscarMonedas

        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            _camposMonedas = New Dictionary(Of String, List(Of String))

            _monedas = New List(Of MonedaGlobal)

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of MonedaGlobal)("Reg000Monedas")
            Dim aliasmonedas_ As String = "{$or:["
            For Each token_ In lttoken_
                If token_.Length <= 3 Then
                    aliasmonedas_ = aliasmonedas_ & "{'aliasmoneda':{$elemMatch:{Valor:'" & token_ & "', Clave:'" & formato_ & "'}}},"
                End If

                aliasmonedas_ = aliasmonedas_ & "{$and:[{$or:[" & _recursosGenerales.SeparacionPalabras(token_, "nombremonedaesp", "", "", "") & "," & _recursosGenerales.SeparacionPalabras(token_, "nombremonedaing", "", "", "") & "]},{'aliasmoneda':{$elemMatch:{Valor:{$ne:''}, Clave:'" & formato_ & "'}}}]},"
            Next
            If ltObjectId IsNot Nothing Then
                For Each tokenId_ In ltObjectId
                    aliasmonedas_ = aliasmonedas_ & "{'_id':ObjectId('" & tokenId_.ToString & "')},"
                Next
            End If
            aliasmonedas_ = aliasmonedas_.Substring(0, aliasmonedas_.Length - 1) & "]}"

            If aliasmonedas_ <> _ultimoMatch Then

                _ultimoMatch = aliasmonedas_
                ' Dim Filter As FilterDefinition(Of MonedaGlobal) = Builders(Of MonedaGlobal).Filter.Where(Function(doc) ltObjectId.Contains(doc._id))

                operationsDB_.Aggregate().Match(aliasmonedas_).SortBy(Function(ch) ch.idmoneda).Limit(limit_).
                ToList().ForEach(Sub(moneda_)

                                     _monedas.Add(moneda_)

                                 End Sub)

            End If

        End Using

        Return _monedas
    End Function



    Public Function ObtenerTipodeCambio(clave_ As String, Optional monedaId_ As ObjectId = Nothing, Optional monedaCambio_ As String = "USD", Optional Nombre_ As String = "",
                                 Optional Inicial_ As DateTime = Nothing,
                                 Optional Final_ As DateTime = Nothing,
                                 Optional limit_ As Int32 = 10) As List(Of tipodecambioreciente) Implements IControladorMonedas.ObtenerTipodeCambio
        If monedaId_ = Nothing Then
            BuscarMonedas(monedaCambio_)
            monedaId_ = _monedas(0)._id

        End If
        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            _tiposdeCambio = New List(Of tipodecambioreciente)

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of MonedaGlobal)("Reg000Monedas")

            Dim aliasmonedas_ As String

            If Nombre_ = "" Then

                aliasmonedas_ = "{'aliasmoneda.Valor':'" & clave_ & "'}"

            Else

                aliasmonedas_ = _recursosGenerales.SeparacionPalabras(Nombre_, "nombremonedaesp", "'aliasmoneda.Valor'", clave_, "")

            End If

            If Inicial_ = Nothing And Final_ = Nothing Then

                Inicial_ = Date.Now

                Final_ = Date.Now

            End If

            If Inicial_ = Final_ Then

                Dim sinicial_ As String = Inicial_.ToShortDateString & " 00:00:00"

                Dim sfinal_ As String = Inicial_.ToShortDateString & " 23:59:59"

                Inicial_ = DateTime.Parse(sinicial_)

                Final_ = DateTime.Parse(sfinal_)

            End If

            aliasmonedas_ = "{$and:[{'aplanadorecientes.cambio':{$gte:ISODate('" & Inicial_.ToString("yyyy-MM-ddTHH:mm:ss.00Z") & "')}},{'aplanadorecientes.cambio':{$lte:ISODate('" & Final_.ToString("yyyy-MM-ddTHH:mm:ss.00Z") & "')}}," & aliasmonedas_ & "]}"

            If aliasmonedas_ <> _ultimoMatch Then

                _ultimoMatch = aliasmonedas_

                operationsDB_.Aggregate().Project(BsonDocument.Parse("{" & "aliasmoneda:1,cambiosrecientes:'$tiposdecambio.recientes'}")).
                                          Unwind("cambiosrecientes").
                                          Project(BsonDocument.Parse("{aliasmoneda:1,aplanadorecientes:{" &
                                                                      "$filter:{" &
                                                                      "input:'$cambiosrecientes'," &
                                                                      "as:'aplanadoreciente'," &
                                                                      "cond:{$and:[{$gte:['$$aplanadoreciente.cambio',ISODate('" & Inicial_.ToString("yyyy-MM-ddTHH:mm:ss") & "')]}," &
                                                                                  "{$lte:['$$aplanadoreciente.cambio',ISODate('" & Final_.ToString("yyyy-MM-ddTHH:mm:ss") & "')]}, " &
                                                                                  "{$eq:['$$aplanadoreciente.idtipodecambio',ObjectId('" & monedaId_.ToString & "')]}]}" &
                                                                      "}}}")).
                                                                          Match(BsonDocument.Parse(aliasmonedas_)).Limit(limit_).
                ToList().ForEach(Sub(TiposdeCambio_)

                                     For Each TipodeCambio_ In TiposdeCambio_.GetElement("aplanadorecientes").Value.AsBsonArray

                                         Dim CambioRecientes_ As New tipodecambioreciente

                                         Dim elementostipocamboio_ = TipodeCambio_.AsBsonDocument.ToList

                                         CambioRecientes_.idtipodecambio = elementostipocamboio_(0).Value

                                         CambioRecientes_.serie = elementostipocamboio_(1).Value

                                         CambioRecientes_.institucion = elementostipocamboio_(2).Value

                                         CambioRecientes_.tipocambio = elementostipocamboio_(3).Value.ToDouble

                                         CambioRecientes_.cambio = elementostipocamboio_(4).Value

                                         _tiposdeCambio.Add(CambioRecientes_)

                                     Next

                                 End Sub)
            End If

        End Using

        Return _tiposdeCambio

    End Function

    Function ObtenerFactorTipodeCambio(Clave_ As String, Optional FechaCambio_ As DateTime = Nothing, Optional MonedaFactor_ As String = "USD", Optional MonedaTipoCambio_ As String = "MXP", Optional Nombre_ As String = ""
                                  ) As TagWatcher Implements IControladorMonedas.ObtenerFactorTipodeCambio

        _estado = New TagWatcher
        _monedas = BuscarMonedas(MonedaFactor_)



        Dim factorMoneda_ As factorreciente
        Dim tipoCambio_ As tipodecambioreciente
        Dim factoresMoneda_ = ObtenerFactorCambio(Clave_, _monedas(0)._id, MonedaFactor_,, FechaCambio_)
        Dim tiposCambio_ = ObtenerTipodeCambio(MonedaTipoCambio_,,,, FechaCambio_, FechaCambio_, 1)

        If factoresMoneda_.Count > 0 Then
            factorMoneda_ = factoresMoneda_(0)
        Else
            factorMoneda_ = Nothing
        End If

        If tiposCambio_.Count > 0 Then
            tipoCambio_ = tiposCambio_(0)
        Else
            tipoCambio_ = Nothing
        End If

        _estado.ObjectReturned = New List(Of Object) From {factorMoneda_, tipoCambio_}

        _estado.SetOK()

        Return _estado

    End Function
    Public Function ConsultaMoneda(campos_ As List(Of [Enum]),
                                   Optional ByVal formato_ As String = "cvedefault",
                                   Optional condicion_ As String = "",
                                  Optional ByVal limite_ As Int32 = 5) As List(Of String) _
                                   Implements IControladorMonedas.ConsultaMoneda
        If _monedas.Count < limite_ Then

            limite_ = _monedas.Count

        End If

        Select Case DirectCast(campos_(0), CamposBusquedaSimple)

            Case CamposBusquedaSimple.IDS

                Return _monedas.Select(Function(ch) ch._id.ToString).ToList.GetRange(0, limite_)

            Case CamposBusquedaSimple.NOMBREESP

                Return _monedas.Select(Function(ch) ch.nombremonedaesp).ToList.GetRange(0, limite_)

            Case CamposBusquedaSimple.NOMBREING

                Return _monedas.Select(Function(ch) ch.nombremonedaing).ToList.GetRange(0, limite_)

            Case CamposBusquedaSimple.FACTORDOLAR

                Return _monedas.Select(Function(ch) "Factor: $" & ch.factoresmoneda(0).valordefault.ToString & " al " & ch.factoresmoneda(0).fecha.ToString).ToList.GetRange(0, limite_)

            Case CamposBusquedaSimple.PRESENTACION

                Return _monedas.Select(Function(ch) ch.nombremonedaesp & "|" & ch.aliasmoneda.Find(Function(sg) sg.Clave = formato_).Valor).ToList.GetRange(0, limite_)

            Case Else

                Return Nothing

        End Select

    End Function

    'Public Function DevuelveUltimaMoneda() As MonedaGlobal Implements IControladorMonedas.DevuelveUltimaMoneda
    '    Return _ultimaMoneda
    'End Function

    Public Function ConsultaMoneda(campo_ As [Enum],
                                   Optional ByVal formato_ As String = "cvedefault",
                                   Optional condicion_ As String = "",
                                   Optional ByVal ilimit_ As Int32 = 5) As List(Of String) _
                                    Implements IControladorMonedas.ConsultaMoneda

        If _monedas.Count < ilimit_ Then

            ilimit_ = _monedas.Count

        End If

        Select Case DirectCast(campo_, CamposBusquedaSimple)

            Case CamposBusquedaSimple.IDS

                If condicion_ = "" Then

                    Return _monedas.Select(Function(ch) ch._id.ToString).ToList.GetRange(0, ilimit_)

                Else

                    Return _monedas.Where(Function(sch) sch._id.ToString = condicion_).Select(Function(ch) ch._id.ToString).ToList.GetRange(0, ilimit_)

                End If

            Case CamposBusquedaSimple.NOMBREESP

                If condicion_ = "" Then

                    Return _monedas.Select(Function(ch) ch.nombremonedaesp).ToList.GetRange(0, ilimit_)

                Else

                    Return _monedas.Where(Function(sch) sch._id.ToString = condicion_).Select(Function(ch) ch.nombremonedaesp).ToList.GetRange(0, ilimit_)

                End If

            Case CamposBusquedaSimple.NOMBREING

                If condicion_ = "" Then

                    Return _monedas.Select(Function(ch) ch.nombremonedaing).ToList.GetRange(0, ilimit_)

                Else

                    Return _monedas.Where(Function(sch) sch.nombremonedaing = condicion_).Select(Function(ch) ch.nombremonedaing).ToList.GetRange(0, ilimit_)

                End If

            Case CamposBusquedaSimple.FACTORDOLAR

                If condicion_ = "" Then

                    Return _monedas.Select(Function(ch) "Factor: $" & ch.factoresmoneda(0).valordefault.ToString & " al " & ch.factoresmoneda(0).fecha.ToString).ToList.GetRange(0, ilimit_)

                Else

                    Return _monedas.Where(Function(sch) sch.factoresmoneda(0).valordefault.ToString = condicion_).Select(Function(ch) "Factor: $" & ch.factoresmoneda(0).valordefault.ToString & " al " & ch.factoresmoneda(0).fecha.ToString).ToList.GetRange(0, ilimit_)

                End If

            Case CamposBusquedaSimple.PRESENTACION

                If condicion_ = "" Then

                    Return _monedas.Select(Function(ch) ch.nombremonedaesp & "|" & ch.aliasmoneda.Find(Function(sg) sg.Clave = formato_).Valor).ToList.GetRange(0, ilimit_)

                Else

                    Return _monedas.Where(Function(sch) sch.nombremonedaing = condicion_).Select(Function(ch) ch.nombremonedaesp & "|" & ch.aliasmoneda.Find(Function(sg) sg.Clave = formato_).Valor).ToList.GetRange(0, ilimit_)

                End If

            Case Else

                Return Nothing

        End Select

    End Function

    Public Function consultaM(ltmonedasId_ As List(Of ObjectId)) As List(Of MonedaGlobal)

        Dim ltresultado As New List(Of MonedaGlobal)

        For Each idMonedas_ In ltmonedasId_

            ltresultado.Add(_monedas.Find(Function(ch) ch._id = idMonedas_))

        Next

        Return ltresultado

    End Function



    'Función para agregar unw nueva MOneda
    Public Function NuevaMoneda(Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        'Dim iEnlace_ As IEnlaceDatos = New EnlaceDatos

        'Dim operationsDB_ = iEnlace_.GetMongoCollection(Of Pais)("Reg000Paises")

        Dim tagwatcher_ As New TagWatcher

        'Dim moneda_ As New moneda With
        '                        {._idmoneda = ObjectId.GenerateNewId,
        '                         .secmoneda = 1,
        '                         .cvemonedaA05 = t_ClaveMoneda,
        '                         .claveISO = t_ClaveISO,
        '                         .codigoISO = t_CodigoISO,
        '                         .nombremoneda = t_NombreMoneda,
        '                         .estado = 1,
        '                         .archivado = False
        '                        }

        'Dim pais_ As New Pais(t_cvecomercioMX,
        '                      t_cveISOnum,
        '                      t_cveISO2,
        '                      t_cveISO3,
        '                      t_nombrepaisesp,
        '                      t_nombrepaising,
        '                      t_nombrepaiscortoesp,
        '                      t_nombrepaiscortoing,
        '                      IIf(moneda_ IsNot Nothing, moneda_, Nothing)
        '                    )

        ''Dim pais_ As New Pais With {.cvecomercioMX = t_cvecomercioMX,
        ''                            .cveISOnum = t_cveISOnum,
        ''                            .cveISO2 = t_cveISO2,
        ''                            .cveISO3 = t_cveISO3,
        ''                            .nombrepaisesp = t_nombrepaisesp,
        ''                            .nombrepaising = t_nombrepaising,
        ''                            .nombrepaiscortoesp = t_nombrepaiscortoesp,
        ''                            .nombrepaiscortoing = t_nombrepaiscortoing,
        ''                            .monedasoficiales = IIf(moneda_ IsNot Nothing, moneda_, Nothing)
        ''}

        'Dim result_ = operationsDB_.InsertOneAsync(session_, pais_).ConfigureAwait(False)

        'With tagwatcher_

        '    .SetOK()

        '    .ObjectReturned = pais_

        'End With

        Return tagwatcher_

    End Function

    'Función para actualizar un nuevo país
    Public Function ActualizaMoneda(ByVal pais_ As MonedaGlobal,
                               Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim tagwatcher_ As New TagWatcher


        Return tagwatcher_

    End Function

    Public Function ObtenerFactorCambio(clave_ As String,
                                        Optional monedaId_ As ObjectId = Nothing,
                                        Optional monedaCambio_ As String = "USD",
                                        Optional nombre_ As String = "",
                                        Optional fechaCambio_ As DateTime = Nothing) As List(Of factorreciente) _
                                        Implements IControladorMonedas.ObtenerFactorCambio
        If monedaId_ = Nothing Then

            BuscarMonedas(monedaCambio_)

            monedaId_ = _monedas(0)._id

        End If
        _factoresCambioRecientes = New List(Of factorreciente)
        Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of MonedaGlobal)("Reg000Monedas")

            Dim aliasmonedas_ As String

            If nombre_ = "" Then

                aliasmonedas_ = "{'aliasmoneda.Valor':'" & clave_ & "'}"

            Else

                aliasmonedas_ = _recursosGenerales.SeparacionPalabras(nombre_, "nombremonedaesp", "'aliasmoneda.Valor'", clave_, "")

            End If

            If fechaCambio_ = Nothing Then

                fechaCambio_ = Now.Date

            End If

            aliasmonedas_ = "{$and:[{'mes':" & fechaCambio_.Month & "},{'anio':" & fechaCambio_.Year & "}," & aliasmonedas_ & "]}"

            If aliasmonedas_ <> _ultimoMatch Then

                _ultimoMatch = aliasmonedas_

                operationsDB_.Aggregate().Project(BsonDocument.Parse("{" & "aliasmoneda:1,cambiosrecientes:'$factoresmoneda.recientes'}")).
                                          Unwind("cambiosrecientes").
                                          Project(BsonDocument.Parse("{aliasmoneda:1,aplanadorecientes:{" &
                                                                      "$filter:{" &
                                                                      "input:'$cambiosrecientes'," &
                                                                      "as:'aplanadoreciente'," &
                                                                      "cond:{$and:[{$eq:[{$month:'$$aplanadoreciente.cambio'}," & fechaCambio_.Month & "]}," &
                                                                                  "{$eq:[{$year:'$$aplanadoreciente.cambio'}," & fechaCambio_.Year & "]}, " &
                                                                                  "{$eq:['$$aplanadoreciente.idfactor',ObjectId('" & monedaId_.ToString & "')]}]}" &
                                                                      "}}}")).
                                        Project(BsonDocument.Parse("{aliasmoneda:1,aplanadorecientes:1," &
                                                                   "mes:{$month:{$arrayElemAt:['$aplanadorecientes.cambio',0]}}," &
                                                                   "anio:{$year:{$arrayElemAt:['$aplanadorecientes.cambio',0]}}}")).
                                                                   Match(BsonDocument.Parse(aliasmonedas_)).
                ToList().ForEach(Sub(tiposdeCambio_)

                                     For Each tipodeCambio_ In tiposdeCambio_.GetElement("aplanadorecientes").Value.AsBsonArray

                                         Dim cambioRecientes_ As New factorreciente

                                         Dim elementosTipoCambio_ = tipodeCambio_.AsBsonDocument.ToList

                                         cambioRecientes_.idfactor = elementosTipoCambio_(0).Value

                                         cambioRecientes_.serie = elementosTipoCambio_(1).Value

                                         cambioRecientes_.institucion = elementosTipoCambio_(2).Value

                                         cambioRecientes_.factor = elementosTipoCambio_(3).Value.ToDouble

                                         cambioRecientes_.cambio = elementosTipoCambio_(4).Value

                                         _factoresCambioRecientes.Add(cambioRecientes_)

                                     Next

                                 End Sub)
            End If

        End Using

        If _factoresCambioRecientes.Count = 0 Then
            BuscarMonedas(clave_)
            If fechaCambio_ >= _monedas(0).factoresmoneda(0).fecha Then
                Dim cambioRecientes_ As New factorreciente

                cambioRecientes_.idfactor = _monedas(0)._id

                cambioRecientes_.serie = 1

                cambioRecientes_.institucion = 1

                cambioRecientes_.factor = _monedas(0).factoresmoneda(0).valordefault

                cambioRecientes_.cambio = _monedas(0).factoresmoneda(0).fecha

                _factoresCambioRecientes.Add(cambioRecientes_)

            End If
        End If

        Return _factoresCambioRecientes

    End Function





#End Region

#Region "Clon"

    Public Function Clone() As Object Implements ICloneable.Clone

        Dim monedasClonada_ As IControladorMonedas = New ControladorMonedas

        With monedasClonada_

            .Monedas = Me._monedas

            .TiposdeCambio = Me._tiposdeCambio

            .FactoresCambioRecientes = Me.FactoresCambioRecientes

            .UltimoMatch = Me.UltimoMatch

        End With

        Return monedasClonada_

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

                .Monedas = Nothing

                .TiposdeCambio = Nothing

                .FactoresCambioRecientes = Nothing

                .UltimoMatch = Nothing


            End With

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
        End If

        Me.disposedValue = True

    End Sub


    ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
        Dispose(True)

        GC.SuppressFinalize(Me)

    End Sub







#End Region

End Class