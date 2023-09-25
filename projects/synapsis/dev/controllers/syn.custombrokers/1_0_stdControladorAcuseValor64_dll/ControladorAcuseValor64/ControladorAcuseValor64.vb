
Imports gsol.krom
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports MongoDB.Bson
Imports MongoDB.Driver

Imports Wma.Exceptions
Imports gsol.Web.Components

Imports Rec.Globals.Controllers
Imports Syn.Utils
Imports MongoDB.Driver.Builders

Public Class ControladorAcuseValor
    Implements IControladorAcuseValor, ICloneable, IDisposable

#Region "Enums"

    Public Enum TipoOperacion

        Importacion = 1
        Exportacion = 2

    End Enum



#End Region

#Region "Atributos"

    Private _organismo As New Organismo

    Private _acusesValorGenerados As List(Of ConstructorAcuseValor)

    Private _documentos As List(Of DocumentoElectronico)

    Private _tipoOperacion As TipoOperacion

    Private _bulkCamposPedidos As Dictionary(Of ObjectId, List(Of Nodo))

    Private _estado As TagWatcher

#End Region

#Region "Propiedades"

    Public Property AcusesValorGenerados As List(Of ConstructorAcuseValor) _
                        Implements IControladorAcuseValor.AcusesValorGenerados
        Get

            Return _acusesValorGenerados

        End Get

        Set(value As List(Of ConstructorAcuseValor))

            _acusesValorGenerados = value

        End Set

    End Property


    Public Property BulkCamposPedidos As Dictionary(Of ObjectId, List(Of Nodo)) _
        Implements IControladorAcuseValor.BulkCamposPedidos
        Get

            Return _bulkCamposPedidos

        End Get

        Set(value As Dictionary(Of ObjectId, List(Of Nodo)))

            _bulkCamposPedidos = value

        End Set

    End Property

    Public Property Estado As TagWatcher _
        Implements IControladorAcuseValor.Estado
        Get

            Return _estado

        End Get

        Set(value As TagWatcher)

            _estado = value

        End Set

    End Property



#End Region

#Region "Constructores"

    Sub New()
        'Dim ConstructorCove_ As New ConstructorCOVE
        '' Dim Algo = ConstructorCove_.ObtenerCamposSeccion(SeccionesCOVE.SCOVE1)
        '' MsgBox(_ctrlRecursosGenerales.ObtenerRutaCampoB(ConstructorCove_, SeccionesCOVE.SCOVE1, CamposFacturaComercial.CA_MONEDA_FACTURACION))

        ''MsgBox(_ctrlRecursosGenerales.ObtenerRutaCampoB(ConstructorCove_, SeccionesCOVE.SCOVE1, CamposFacturaComercial.CA_MONEDA_FACTURACION) & Chr(13) &
        ''       _ctrlRecursosGenerales.ObtenerRutaCampoB(ConstructorCove_, SeccionesCOVE.SCOVE1, CamposCOVE.CA_NUMERO_EXPORTADOR_COVE) & Chr(13) &
        ''       _ctrlRecursosGenerales.ObtenerRutaCampoB(ConstructorCove_, SeccionesCOVE.SCOVE3, CamposDomicilio.CA_DOMICILIO_FISCAL) & Chr(13) &
        ''       _ctrlRecursosGenerales.ObtenerRutaCampoB(ConstructorCove_, SeccionesCOVE.SCOVE4, 0))
        '' MsgBox(_ctrlRecursosGenerales.RutillaAMongo(_ctrlRecursosGenerales.ObtenerRutaCampoB(ConstructorCove_, SeccionesCOVE.SCOVE1, CamposFacturaComercial.CA_MONEDA_FACTURACION)))
        'Dim ltSeccion_ As New Dictionary(Of Int32, List(Of Int32))
        'ltSeccion_.Add(SeccionesClientes.SCS1, New List(Of Int32) From {CamposClientes.CA_RAZON_SOCIAL, CamposDomicilio.CA_DOMICILIO_FISCAL, CamposClientes.CA_RFC_CLIENTE})
        'Dim Algo = New ConstructorCliente

        '_CtrlRecursosGenerales.RemplazaParaUpdate(New ObjectId("64c058f72e0949dc43c8ba07"), New ConstructorProveedoresOperativos, SeccionesProvedorOperativo.SPRO4)
        'MsgBox("USADO")
        'MsgBox(DirectCast(Algodon("43").Item(0), Campo).Valor & Chr(13) & DirectCast(Algodon("43").Item(1), Campo).Valor & Chr(13) & DirectCast(Algodon("43").Item(2), Campo).ValorPresentacion)
        'ConsultaCOVE(New ObjectId("649f10dc1f4172e06fd23acb"), New Dictionary(Of [Enum], List(Of [Enum])) From {{SeccionesAcuseValor.SCOVE1, New List(Of [Enum]) From {CamposFacturaComercial.CA_MONEDA_FACTURACION}}})

        '6419c3805354a4068f25cdb8
        _acusesValorGenerados = New List(Of ConstructorAcuseValor)

        _estado = New TagWatcher

    End Sub

#End Region


#Region "Métodos"


    Public Function ConsultaAcusesValor(idCoves_ As List(Of ObjectId),
                    Optional campos_ As Dictionary(Of [Enum], List(Of [Enum])) = Nothing) As TagWatcher _
                    Implements IControladorAcuseValor.ConsultaAcusesValor


        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            If campos_ Is Nothing Then

                _acusesValorGenerados = New List(Of ConstructorAcuseValor)

                Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) =
                    _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorAcuseValor).Name)

                operationsDB_.Aggregate().Project(Function(r) New With {
                                                        Key .ids = r.Id,
                                                        Key .documento = r.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                                      }).
                                                      Match(Function(chi) idCoves_.Contains(chi.ids)).
                                                      ToList().ForEach(Sub(items)
                                                                           items.documento.Id = items.ids.ToString
                                                                           _acusesValorGenerados.Add(New ConstructorAcuseValor(True, items.documento))
                                                                       End Sub)

                _estado.ObjectReturned = _acusesValorGenerados

            Else

                _bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(idCoves_, New ConstructorAcuseValor, campos_)

                _estado.ObjectReturned = _bulkCamposPedidos

            End If

        End Using

        _estado.SetOK()

        Return _estado

    End Function


    Public Function ConsultaAcuseValor(idAcuseValor_ As ObjectId,
                                 Optional campos_ As Dictionary(Of [Enum], List(Of [Enum])) = Nothing) As TagWatcher _
                                 Implements IControladorAcuseValor.ConsultaAcuseValor

        _estado = New TagWatcher

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            If campos_ Is Nothing Then

                Dim consulta_ As String = ""

                _acusesValorGenerados = New List(Of ConstructorAcuseValor)

                Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) = _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(GetType(ConstructorAcuseValor).Name)

                operationsDB_.Aggregate().Project(Function(r) New With {
                                                        Key .ids = r.Id,
                                                        Key .documento = r.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente
                                                      }).
                                                      Match(Function(ch) ch.ids = idAcuseValor_).
                                                      ToList().ForEach(Sub(items)

                                                                           items.documento.Id = items.ids.ToString

                                                                           _acusesValorGenerados.Add(items.documento)

                                                                       End Sub)

                _estado.ObjectReturned = _acusesValorGenerados

            Else

                _bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {idAcuseValor_}, New ConstructorAcuseValor, campos_)

                _estado.ObjectReturned = _bulkCamposPedidos

            End If

        End Using

        _estado.SetOK()

        Return _estado

    End Function



    Public Function filtracomparacion(sTaxId_ As String, sRFC As String, stoken_ As String) As Boolean

        If sTaxId_ = "" Then

            If sRFC = stoken_ Then

                Return True

            Else

                Return False

            End If

        Else

            If sTaxId_ = stoken_ Then

                Return True

            Else

                Return False

            End If

        End If

    End Function


    Public Function GenerarAcuseValor(constructorAcuseValor_ As ConstructorAcuseValor,
                                      Optional adendar_ As Boolean = False) As TagWatcher _
                                      Implements IControladorAcuseValor.GenerarAcuseValor

        Dim acuseValor_ As String = "COVE237"

        Randomize()
        ' Generate random value between 1 and 6.
        acuseValor_ = acuseValor_ + Chr(Int((25 * Rnd()) + 65)) + Chr(Int((25 * Rnd()) + 65))

        For indice_ = 1 To 4

            Dim aleatorio_ As Int32 = Int((1 * Rnd()) + 1) = 1

            If aleatorio_ = 1 Then

                aleatorio_ = Int((9 * Rnd()) + 48)
            Else
                aleatorio_ = Int((25 * Rnd()) + 65)

            End If
            acuseValor_ = acuseValor_ + Chr(aleatorio_)
        Next
        _estado = New TagWatcher
        With _estado

            .SetOK()

            .ObjectReturned = acuseValor_

        End With

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            'Dim operationsDB_ As IMongoCollection(Of ConstructorAcuseValor) = _enlaceDatos.GetMongoCollection(Of ConstructorAcuseValor)(constructorAcuseValor_.GetType.Name)
            'Dim update_ = Builders(Of ConstructorAcuseValor).Update.Set(Of String)(Function(e) DirectCast(e.EstructuraDocumento.Item("Encabezado")(0).Nodos(0).Nodos(3).Nodos(0), Campo).Valor, acuseValor_)
            '' update_ = update_.Set(Of DateTime)(Function(e) e.Seccion(SeccionesAcuseValor.SAcuseValor1).Attribute(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor, DateTime.Now)
            'operationsDB_.UpdateOne(Function(e) e.Id.Equals(constructorAcuseValor_.Id), update_)



            Dim operationsDB_ As IMongoCollection(Of OperacionGenerica) =
                _enlaceDatos.GetMongoCollection(Of OperacionGenerica)(constructorAcuseValor_.GetType.Name)

            Dim ruta_ = _organismo.ObtenerRutaCampo(constructorAcuseValor_,
                                                    SeccionesAcuseValor.SAcuseValor1,
                                                    CamposAcuseValor.CA_NUMERO_ACUSEVALOR)

            ruta_ = ruta_.Substring(0, ruta_.Length - 2)

            Dim puntosNumeroCove_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." & ruta_.Replace("(", ".").Replace(")", "") & ".Valor"

            Dim puntosNumeroCove2_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." & ruta_.Replace("(", ".").Replace(")", "") & ".ValorPresentacion"

            ruta_ = _organismo.ObtenerRutaCampo(constructorAcuseValor_, SeccionesAcuseValor.SAcuseValor1, CamposAcuseValor.CA_FECHA_ACUSEVALOR)

            ruta_ = ruta_.Substring(0, ruta_.Length - 2)

            Dim puntosFechaCove_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." & ruta_.Replace("(", ".").Replace(")", "") & ".Valor"

            '' Crear el objeto de actualización
            Dim fechaaux_ = DateTime.Now

            Dim update_ As BsonDocument = IIf(Not adendar_,
                                             BsonDocument.Parse("{$set:{'" & puntosNumeroCove_ & "':'" & acuseValor_ &
                                                                    "', '" & puntosFechaCove_ & "':ISODate('" &
                                                                    fechaaux_.ToString("yyyy-MM-ddTHH:mm:ss.00Z") & "')}}"),
                                             BsonDocument.Parse("{$set:{'" & puntosNumeroCove_ & "':'" & acuseValor_ &
                                                                "','" & puntosNumeroCove2_ & "':'" &
                                                                constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).
                                                                Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor &
                                                                "', '" & puntosFechaCove_ & "':ISODate('" &
                                                                (New DateTime).ToString("yyyy-MM-ddTHH:mm:ss.00Z") & "')}}"))


            '' Realizar la actualización
            Dim acuseValorId_ = New ObjectId(constructorAcuseValor_.Id)

            constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).Campo(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor = fechaaux_

            operationsDB_.UpdateOne(Function(e) e.Id = acuseValorId_, update_)

        End Using

        Dim controladorFacturaComercial_ As New ControladorFacturaComercial(1, True)

        controladorFacturaComercial_.ActualizarDatosAcuseValor(constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).Attribute(CamposAcuseValor.CP_ID_FACTURA_ACUSEVALOR).Valor, New List(Of String) From {acuseValor_, constructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).Attribute(CamposAcuseValor.CA_FECHA_ACUSEVALOR).Valor})

        Return _estado

    End Function
    Public Sub Dispose() Implements IDisposable.Dispose

        Throw New NotImplementedException()

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function



    'Public Function ChecaFormatos(ByVal format_ As InputControl.InputFormat, ByVal valorAsignado_ As String)

    '    Select Case format_

    '        Case InputControl.InputFormat.Calendar

    '            If IsDate(valorAsignado_) Then

    '                Return Convert.ToDateTime(valorAsignado_).Date.ToString("yyyy-MM-dd")

    '            End If

    '        Case InputControl.InputFormat.Money

    '            Return FormatCurrency(valorAsignado_)

    '        Case Else

    '            Return valorAsignado_

    '    End Select

    '    Return Nothing

    'End Function

    Public Function ObtenerAcuseValor(idAcuseValor_ As ObjectId) As TagWatcher _
                                 Implements IControladorAcuseValor.ObtenerAcuseValor
        Dim ConstructorAcuseValor_ = _acusesValorGenerados.Find(Function(e) e.Id = idAcuseValor_.ToString)

        If ConstructorAcuseValor_ Is Nothing Then

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                _bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {idAcuseValor_},
                                                                             New ConstructorAcuseValor, New Dictionary(Of [Enum],
                                                                             List(Of [Enum])) From {{SeccionesAcuseValor.SAcuseValor1,
                                                                             New List(Of [Enum]) From {CamposAcuseValor.CA_NUMERO_ACUSEVALOR}}})

                If _bulkCamposPedidos Is Nothing Then

                    _estado.ObjectReturned = ""

                Else

                    _estado.ObjectReturned = DirectCast(_bulkCamposPedidos(idAcuseValor_).Item(0), Campo).Valor

                End If

            End Using

        Else

            _estado.ObjectReturned = ConstructorAcuseValor_.Seccion(SeccionesAcuseValor.SAcuseValor1).Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor

        End If

        _estado.SetOK()

        Return _estado

    End Function

    Public Function DescargarXML(idCOVE As ObjectId) As TagWatcher Implements IControladorAcuseValor.DescargarXML

        Throw New NotImplementedException()

    End Function

    Public Function DescargarXML(idCOVEs As List(Of ObjectId)) As TagWatcher Implements IControladorAcuseValor.DescargarXML

        Throw New NotImplementedException()

    End Function

    Public Function DescargarPDF(idCOVE As ObjectId) As TagWatcher Implements IControladorAcuseValor.DescargarPDF
        Throw New NotImplementedException()
    End Function

    Public Function DescargarPDF(idCOVEs As List(Of ObjectId)) As TagWatcher Implements IControladorAcuseValor.DescargarPDF

        Throw New NotImplementedException()

    End Function
#End Region

End Class












