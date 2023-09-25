Imports Gsol.basedatos.Operaciones
Imports Wma.Exceptions
Imports Gsol.procesos.corporativo

Public Class AuditoriaCalidad
    Implements IAuditoriasVistosBuenos

#Region "Atributos"

    Private _sistema As Organismo

    Private _mensaje As String

    'Private iidReferencia As Integer
    'Private sReferencia As String
    'Private sStrItem As String
    'Private sImportadorExportador As String
    'Private sFacturas As String
    'Private sBLGuiaConte As String
    'Private dValorDolares As Double
    'Private dPesobruto As Double
    'Private iDTA As Integer
    'Private iCC As Integer
    'Private iIVA As Integer
    'Private iISAN As Integer
    'Private iIEPS As Integer
    'Private iIGI As Integer
    'Private iRECA As Integer
    'Private iMULT As Integer
    'Private iC303 As Integer
    'Private iRT As Integer
    'Private iBSS As Integer
    'Private iPRV As Integer
    'Private iEUR As Integer
    'Private iREU As Integer
    'Private iECI As Integer
    'Private iITV As Integer
    'Private iMT As Integer
    'Private iCNT As Integer
    'Private iIVAPRV As Integer
    'Private sEdocuments As String
    'Private sArea As String
    'Private iClaveSituacion As String
    'Private sCancelada As String
    'Private sUsuario As String
    'Private sUsuarioCancelo As String

    'Private sClavePedimento As String
    'Private sFormaPago As String
    'Private sBultos As String
    'Private sFraccion As String
    'Private sIdentificadoresPedimento As String
    'Private sIdentificadoresFraccion As String
    'Private sIdentificadoresPedimentoJur As String
    'Private sIdentificadoresFraccionJur As String
    'Private sObservaciones As String
    'Private sObservacionesJur As String

#End Region

#Region "Propiedades"

    Public ReadOnly Property MensajeError As String Implements IAuditoriasVistosBuenos.MensajeError
        Get
            Return _mensaje
        End Get
    End Property

    'Public Property idReferencia As Integer
    '    Get
    '        idReferencia = iidReferencia
    '    End Get
    '    Set(ByVal value As Integer)
    '        iidReferencia = value
    '    End Set
    'End Property

    'Public Property Referencia As String
    '    Get
    '        Referencia = sReferencia
    '    End Get
    '    Set(ByVal value As String)
    '        sReferencia = value
    '    End Set
    'End Property

    'Public Property StrItem As String
    '    Get
    '        StrItem = sStrItem
    '    End Get
    '    Set(ByVal value As String)
    '        sStrItem = value
    '    End Set
    'End Property

    'Public Property ImportadorExportador As String
    '    Get
    '        ImportadorExportador = sImportadorExportador
    '    End Get
    '    Set(ByVal value As String)
    '        sImportadorExportador = value
    '    End Set
    'End Property

    'Public Property Facturas As String
    '    Get
    '        Facturas = sFacturas
    '    End Get
    '    Set(ByVal value As String)
    '        sFacturas = value
    '    End Set
    'End Property

    'Public Property BLGuiaConte As String
    '    Get
    '        BLGuiaConte = sBLGuiaConte
    '    End Get
    '    Set(ByVal value As String)
    '        sBLGuiaConte = value
    '    End Set
    'End Property

    'Public Property ValorDolares As Decimal
    '    Get
    '        ValorDolares = dValorDolares
    '    End Get
    '    Set(ByVal value As Decimal)
    '        dValorDolares = value
    '    End Set
    'End Property

    'Public Property Pesobruto As Decimal
    '    Get
    '        Pesobruto = dPesobruto
    '    End Get
    '    Set(ByVal value As Decimal)
    '        dPesobruto = value
    '    End Set
    'End Property

    'Public Property DTA As Integer
    '    Get
    '        DTA = iDTA
    '    End Get
    '    Set(ByVal value As Integer)
    '        iDTA = value
    '    End Set
    'End Property

    'Public Property CC As Integer
    '    Get
    '        CC = iCC
    '    End Get
    '    Set(ByVal value As Integer)
    '        iCC = value
    '    End Set
    'End Property

    'Public Property IVA As Integer
    '    Get
    '        IVA = iIVA
    '    End Get
    '    Set(ByVal value As Integer)
    '        iIVA = value
    '    End Set
    'End Property

    'Public Property ISAN As Integer
    '    Get
    '        ISAN = iISAN
    '    End Get
    '    Set(ByVal value As Integer)
    '        iISAN = value
    '    End Set
    'End Property

    'Public Property IEPS As Integer
    '    Get
    '        IEPS = iIEPS
    '    End Get
    '    Set(ByVal value As Integer)
    '        iIEPS = value
    '    End Set
    'End Property

    'Public Property IGI As Integer
    '    Get
    '        IGI = iIGI
    '    End Get
    '    Set(ByVal value As Integer)
    '        iIGI = value
    '    End Set
    'End Property

    'Public Property RECA As Integer
    '    Get
    '        RECA = iRECA
    '    End Get
    '    Set(ByVal value As Integer)
    '        iRECA = value
    '    End Set
    'End Property

    'Public Property MULT As Integer
    '    Get
    '        MULT = iMULT
    '    End Get
    '    Set(ByVal value As Integer)
    '        iMULT = value
    '    End Set
    'End Property

    'Public Property C303 As Integer
    '    Get
    '        C303 = iC303
    '    End Get
    '    Set(ByVal value As Integer)
    '        iC303 = value
    '    End Set
    'End Property

    'Public Property RT As Integer
    '    Get
    '        RT = iRT
    '    End Get
    '    Set(ByVal value As Integer)
    '        iRT = value
    '    End Set
    'End Property

    'Public Property BSS As Integer
    '    Get
    '        BSS = iBSS
    '    End Get
    '    Set(ByVal value As Integer)
    '        iBSS = value
    '    End Set
    'End Property

    'Public Property PRV As Integer
    '    Get
    '        PRV = iPRV
    '    End Get
    '    Set(ByVal value As Integer)
    '        iPRV = value
    '    End Set
    'End Property

    'Public Property EUR As Integer
    '    Get
    '        EUR = iEUR
    '    End Get
    '    Set(ByVal value As Integer)
    '        iEUR = value
    '    End Set
    'End Property

    'Public Property REU As Integer
    '    Get
    '        REU = iREU
    '    End Get
    '    Set(ByVal value As Integer)
    '        iREU = value
    '    End Set
    'End Property

    'Public Property ECI As Integer
    '    Get
    '        ECI = iECI
    '    End Get
    '    Set(ByVal value As Integer)
    '        iECI = value
    '    End Set
    'End Property

    'Public Property ITV As Integer
    '    Get
    '        ITV = iITV
    '    End Get
    '    Set(ByVal value As Integer)
    '        iITV = value
    '    End Set
    'End Property

    'Public Property MT As Integer
    '    Get
    '        MT = iMT
    '    End Get
    '    Set(ByVal value As Integer)
    '        iMT = value
    '    End Set
    'End Property

    'Public Property CNT As Integer
    '    Get
    '        CNT = iCNT
    '    End Get
    '    Set(ByVal value As Integer)
    '        iCNT = value
    '    End Set
    'End Property

    'Public Property Edocuments As String
    '    Get
    '        Edocuments = sEdocuments
    '    End Get
    '    Set(ByVal value As String)
    '        sEdocuments = value
    '    End Set
    'End Property

    'Public Property IVAPRV As String
    '    Get
    '        IVAPRV = iIVAPRV
    '    End Get
    '    Set(ByVal value As String)
    '        iIVAPRV = value
    '    End Set
    'End Property

    'Public Property Area As String
    '    Get
    '        Area = sArea
    '    End Get
    '    Set(ByVal value As String)
    '        sArea = value
    '    End Set
    'End Property

    'Public Property ClaveSituacion As Integer
    '    Get
    '        ClaveSituacion = iClaveSituacion
    '    End Get
    '    Set(ByVal value As Integer)
    '        iClaveSituacion = value
    '    End Set
    'End Property
    'Public Property Cancelada As String
    '    Get
    '        Cancelada = sCancelada
    '    End Get
    '    Set(ByVal value As String)
    '        sCancelada = value
    '    End Set
    'End Property

    'Public Property Usuario As String
    '    Get
    '        Usuario = sUsuario
    '    End Get
    '    Set(ByVal value As String)
    '        sUsuario = value
    '    End Set
    'End Property

    'Public Property UsuarioCancelo As String
    '    Get
    '        UsuarioCancelo = sUsuarioCancelo
    '    End Get
    '    Set(ByVal value As String)
    '        sUsuarioCancelo = value
    '    End Set
    'End Property

    'Public Property ClavePedimento As String
    '    Get
    '        ClavePedimento = sClavePedimento
    '    End Get
    '    Set(value As String)
    '        sClavePedimento = value
    '    End Set
    'End Property

    'Public Property FormaPago As String
    '    Get
    '        FormaPago = sFormaPago
    '    End Get
    '    Set(value As String)
    '        sFormaPago = value
    '    End Set
    'End Property

    'Public Property Bultos As String
    '    Get
    '        Bultos = sBultos
    '    End Get
    '    Set(value As String)
    '        sBultos = value
    '    End Set
    'End Property
    'Public Property Fraccion As String
    '    Get
    '        Fraccion = sFraccion
    '    End Get
    '    Set(value As String)
    '        sFraccion = value
    '    End Set
    'End Property
    'Public Property IdentificadoresPedimento As String
    '    Get
    '        IdentificadoresPedimento = sIdentificadoresPedimento
    '    End Get
    '    Set(value As String)
    '        sIdentificadoresPedimento = value
    '    End Set
    'End Property

    'Public Property IdentificadoresFraccion As String
    '    Get
    '        IdentificadoresFraccion = sIdentificadoresFraccion
    '    End Get
    '    Set(value As String)
    '        sIdentificadoresFraccion = value
    '    End Set
    'End Property

    'Public Property IdentificadoresPedimentoJur As String
    '    Get
    '        IdentificadoresPedimentoJur = sIdentificadoresPedimentoJur
    '    End Get
    '    Set(value As String)
    '        sIdentificadoresPedimentoJur = value
    '    End Set
    'End Property

    'Public Property IdentificadoresFraccionJur As String
    '    Get
    '        IdentificadoresFraccionJur = sIdentificadoresFraccionJur
    '    End Get
    '    Set(value As String)
    '        sIdentificadoresFraccionJur = value
    '    End Set
    'End Property

    'Public Property Observaciones As String
    '    Get
    '        Observaciones = sObservaciones
    '    End Get
    '    Set(value As String)
    '        sObservaciones = value
    '    End Set
    'End Property

    'Public Property ObservacionesJur As String
    '    Get
    '        ObservacionesJur = sObservacionesJur
    '    End Get
    '    Set(value As String)
    '        sObservacionesJur = value
    '    End Set
    'End Property


#End Region

#Region "Metodos Publicos"

    Public Function GuardarInformacion(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                   ByVal i_Cve_Referencia As Integer, _
                                   ByVal i_Cve_Sistema As Integer) As Boolean Implements IAuditoriasVistosBuenos.GuardarInformacion

        _mensaje = Nothing

        If ExisteAuditoria(ioperacionescatalogo_, i_Cve_Referencia, i_Cve_Sistema) Then

            'tagWatcherLocal_.Status = TagWatcher.TypeStatus.Errors

            'tagWatcherLocal_.SetError(TagWatcher.ErrorTypes.C6_034_0001)

            '_sistema.GsDialogo("Ya existe un registro activo para esta auditoría", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            _mensaje = "Ya existe un registro activo para esta auditoría. "

            Return False

        End If

        Dim InformacionTrafico_ As New OperacionesCatalogo

        InformacionTrafico_ = ConsultaInformacionTrafico(ioperacionescatalogo_, i_Cve_Referencia, i_Cve_Sistema, "VistoBuenoCalidadTrafico")

        If InformacionTrafico_ Is Nothing Then

            _mensaje = "No se ha podido consultar la información de la referencia a guardar. "
            '_sistema.GsDialogo("No se ha podido consultar la información de la referencia a guardar", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return False

        End If

        If GuardarInformacion(ioperacionescatalogo_, InformacionTrafico_) = False Then

            _mensaje = "No se ha podido guardar la información de la referencia. "

            '_sistema.GsDialogo("No se ha podido guardar la información de la referencia", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return False

        End If

        Return True

    End Function

    Public Function CancelaAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                 ByVal i_Cve_Referencia_ As Integer, _
                                 ByVal i_Cve_Sistema_ As Integer, _
                                 ByVal t_Observaciones_ As String) As Boolean Implements IAuditoriasVistosBuenos.CancelaAuditoria

        _mensaje = Nothing

        Dim claveInfoAlmacenada_ As Integer = ConsultaClaveRegistroAuditoriaVivaDesdeReferencia(ioperacionescatalogo_, i_Cve_Referencia_, i_Cve_Sistema_)

        If claveInfoAlmacenada_ = 0 Then

            _mensaje = "No existe la auditoría para esa referencia. "

            '_sistema.GsDialogo("No existe la auditoría para esa referencia", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return True

        End If

        If claveInfoAlmacenada_ = -1 Then

            _mensaje = "Existe mas de una auditoría viva para esta referencia, favor de verificar. "

            '_sistema.GsDialogo("Existe mas de una auditoría viva para esta referencia, favor de verificar", Componentes.SistemaBase.GsDialogo.TipoDialogo.Aviso)

            Return False

        End If

        'Dim _operacionesAux = New OperacionesCatalogo
        Dim operacionesAux_ As IOperacionesCatalogo

        operacionesAux_ = _sistema.EnsamblaModulo("VistoBuenoCalidadInfoAlmacenada")

        operacionesAux_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        operacionesAux_.PreparaCatalogo()

        operacionesAux_.CampoPorNombre("t_Observaciones") = t_Observaciones_

        operacionesAux_.CampoPorNombre("i_Cve_UsuarioCancelo") = ioperacionescatalogo_.EspacioTrabajo.MisCredenciales.ClaveUsuario

        operacionesAux_.CampoPorNombre("f_FechaCancelacion") = Date.Now.ToString("dd/MM/yy H:mm:ss.fff")

        operacionesAux_.CampoPorNombre("i_Cve_Estatus") = "0"

        If operacionesAux_.Modificar(claveInfoAlmacenada_) = IOperacionesCatalogo.EstadoOperacion.COk Then

            'tagWatcherLocal_.Status = TagWatcher.TypeStatus.Ok
            Return True

        End If

        Return False

    End Function


    Public Function CompararInformacion() As Boolean

        Return True

    End Function

    'Revisar si ya tiene esa auditoría la referencia (viva)
    Public Function ExisteAuditoria(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                    ByVal i_Cve_Referencia As Integer, _
                                    ByVal i_Cve_Sistema As Integer) As Boolean Implements IAuditoriasVistosBuenos.ExisteAuditoria

        Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

        Dim claususalSQL_ As String

        claususalSQL_ = " and t_NombreCorto = 'VBC'" & _
            " and i_Cve_idReferencia = " & i_Cve_Referencia & _
            " and i_Cve_SistemaTrafico = " & i_Cve_Sistema & _
            " and i_Cve_Estatus = 1"

        ioperacionesLocal_ = _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, "VistoBuenoCalidadInfoAlmacenada", claususalSQL_)

        If Not ioperacionesLocal_ Is Nothing Then

            Return True

        End If

        Return False

    End Function

#End Region

#Region "Metodos privados"

    Private Function ConsultaInformacionTrafico(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                ByVal i_Cve_Referencia As Integer, _
                                                ByVal i_Cve_Sistema As Integer, _
                                                ByVal modulo_ As String) As IOperacionesCatalogo Implements IAuditoriasVistosBuenos.ConsultaInformacionTrafico

        'Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo
        'Dim informacionTrafico_ As DataTable

        Dim claususalSQL_ As String

        claususalSQL_ = " and idReferencia = " & i_Cve_Referencia & " and i_Cve_SistemaTrafico = " & i_Cve_Sistema

        Return _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, modulo_, claususalSQL_)

    End Function

    'Public Sub ConsultaInformacionInfoAlmacenada(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
    '                               ByVal i_Cve_Referencia As Integer, _
    '                               ByVal i_Cve_Sistema As Integer)

    '    Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

    '    Dim claususalSQL_ As String

    '    claususalSQL_ = " and idReferencia = " & i_Cve_Referencia & " and i_Cve_Sistema = " & i_Cve_Sistema

    '    ioperacionesLocal_ = _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, "VistoBuenoCalidadInfoAlmacenada", claususalSQL_)

    'End Sub

    Private Function GuardarInformacion(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                        ByVal datos_ As IOperacionesCatalogo) As Boolean Implements IAuditoriasVistosBuenos.GuardarInformacion



        'Dim guardado_ As Boolean = False
        'Dim tagWatcherLocal_ = New TagWatcher

        Dim operacionesAux_ As IOperacionesCatalogo

        Dim objAdministracionAuditoria_ As New AdministracionAuditorias

        operacionesAux_ = _sistema.EnsamblaModulo("VistoBuenoCalidadInfoAlmacenada")

        operacionesAux_.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        operacionesAux_.PreparaCatalogo()

        operacionesAux_.CampoPorNombre("i_Cve_SistemaTrafico") = datos_.Vista(0, "i_Cve_SistemaTrafico", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_Cve_idReferencia") = datos_.Vista(0, "idReferencia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_Referencia") = datos_.Vista(0, "Referencia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_Aduana") = datos_.Vista(0, "Aduana", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_Patente") = datos_.Vista(0, "Patente", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_Pedimento") = datos_.Vista(0, "Pedimento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_ImportadorExportadorClave") = datos_.Vista(0, "ClaveCliente", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_ImportadorExportadorRFC") = datos_.Vista(0, "RFCCliente", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_ImportadorExportadorNombre") = datos_.Vista(0, "Cliente", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_ImportadorExportadorDireccion") = datos_.Vista(0, "DireccionCliente", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("d_ValorDolares") = datos_.Vista(0, "ValorUSD", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_Bultos") = datos_.Vista(0, "Bultos", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("d_Pesobruto") = datos_.Vista(0, "PesoBruto", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_DTA") = datos_.Vista(0, "DTA", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_CC") = datos_.Vista(0, "CC", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_IVA") = datos_.Vista(0, "IVA", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_ISAN") = datos_.Vista(0, "ISAN", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_IEPS") = datos_.Vista(0, "IEPS", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_IGI") = datos_.Vista(0, "IGI", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_RECA") = datos_.Vista(0, "REC", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_MULT") = datos_.Vista(0, "MULT", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_C303") = datos_.Vista(0, "C303", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_RT") = datos_.Vista(0, "RT", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_PRV") = datos_.Vista(0, "PRV", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_EUR") = datos_.Vista(0, "EUR", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_REU") = datos_.Vista(0, "REU", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_MT") = datos_.Vista(0, "MT", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_IVAPRV") = datos_.Vista(0, "IVAPRV", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_Facturas") = datos_.Vista(0, "Facturas", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_BLGuia") = datos_.Vista(0, "Guia", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_Contenedor") = datos_.Vista(0, "Contenedor", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_Fraccion") = datos_.Vista(0, "FraccionDescCantUMCUnidadDeMedidaUMCCantTar", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_IdentificadorComplementoPedimento") = datos_.Vista(0, "IdentificadoresPedimento", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("t_IdentificadorComplementoFraccion") = datos_.Vista(0, "IdentificadoresFraccion", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

        operacionesAux_.CampoPorNombre("i_Cve_CatalogoAuditoria") = objAdministracionAuditoria_.ConsultaClaveAuditoriaDesdeNombreCorto(ioperacionescatalogo_, "VBC")

        operacionesAux_.CampoPorNombre("i_Cve_Estatus") = "1"

        operacionesAux_.CampoPorNombre("i_Cve_UsuarioRegistro") = ioperacionescatalogo_.EspacioTrabajo.MisCredenciales.ClaveUsuario

        operacionesAux_.CampoPorNombre("f_FechaRegistro") = Date.Now.ToString("dd/MM/yy H:mm:ss.fff")

        operacionesAux_.CampoPorNombre("i_Cve_Estado") = "1"

        If operacionesAux_.Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

            'tagWatcherLocal_.SetOK(0, "1") 'Regresa 1 en flag, si se guardó
            Return True
        Else

            'tagWatcherLocal_.Status = TagWatcher.TypeStatus.Errors

            'tagWatcherLocal_.SetError(TagWatcher.ErrorTypes.C6_029_0009)
            Return False
        End If

        'Return tagWatcherLocal_

    End Function

    Private Function ConsultaClaveRegistroAuditoriaVivaDesdeReferencia(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
                                                                       ByVal i_Cve_Referencia As Integer, _
                                                                       ByVal i_Cve_Sistema As Integer) Implements IAuditoriasVistosBuenos.ConsultaClaveRegistroAuditoriaVivaDesdeReferencia

        Dim claveRegistro_ As Integer = 0

        Dim ioperacionesLocal_ As IOperacionesCatalogo = New OperacionesCatalogo

        Dim claususalSQL_ As String

        claususalSQL_ = " and t_NombreCorto = 'VBC'" & _
            " and i_Cve_idReferencia = " & i_Cve_Referencia & _
            " and i_Cve_SistemaTrafico = " & i_Cve_Sistema & _
            " and i_Cve_Estatus = 1"

        ioperacionesLocal_ = _sistema.ConsultaModulo(ioperacionescatalogo_.EspacioTrabajo, "VistoBuenoCalidadInfoAlmacenada", claususalSQL_)

        If Not ioperacionesLocal_ Is Nothing Then

            If ioperacionesLocal_.Vista.Tables(0).Rows.Count > 1 Then

                claveRegistro_ = -1

            Else

                claveRegistro_ = ioperacionesLocal_.Vista(0, "i_Cve_CamposVistosBuenos", IOperacionesCatalogo.TiposAccesoCampo.NombreTecnico)

            End If

        End If

        Return claveRegistro_

    End Function

#End Region

#Region "Constructores"

    Public Sub New()

        _sistema = New Organismo

        'sReferencia = Nothing
        'sStrItem = Nothing
        'sImportadorExportador = Nothing
        'sFacturas = Nothing
        'sBLGuiaConte = Nothing
        'dValorDolares = 0
        'dPesobruto = 0
        'iDTA = 0
        'iCC = 0
        'iIVA = 0
        'iISAN = 0
        'iIEPS = 0
        'iIGI = 0
        'iRECA = 0
        'iMULT = 0
        'iC303 = 0
        'iRT = 0
        'iBSS = 0
        'iPRV = 0
        'iEUR = 0
        'iREU = 0
        'iECI = 0
        'iITV = 0
        'iMT = 0
        'iCNT = 0
        'iIVAPRV = 0
        'sEdocuments = Nothing
        'sArea = Nothing
        'sCancelada = Nothing
        'sUsuario = Nothing
        'sUsuarioCancelo = Nothing
        'sClavePedimento = Nothing
        'sBultos = Nothing
        'sFormaPago = Nothing
        'sFraccion = Nothing
        'sIdentificadoresPedimento = Nothing
        'sIdentificadoresFraccion = Nothing
        'sObservaciones = Nothing

    End Sub

#End Region

End Class
