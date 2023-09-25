Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports gsol.krom
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Nodo
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Wma.Exceptions

Namespace Syn.Documento

    <Serializable()>
    Public Class ConstructorPedimentoNormal
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

        Private _constructorCampoPedimento As ConstructorCampoPedimento

#End Region

#Region "Builders"
        Sub New()

            _tagWatcher = New TagWatcher

            IdDocumento = 0

            FolioDocumento = Nothing

            UsuarioGenerador = Nothing

            FolioOperacion = Nothing

            IdCliente = 0

            NombreCliente = Nothing

            EstatusDocumento = "0"

            FechaCreacion = Now()

            TipoDocumentoElectronico = TiposDocumentoElectronico.PedimentoNormal

            _estructuraDocumento = New EstructuraDocumento(TiposDocumentoElectronico.PedimentoNormal.ToString)

            _constructorCampoPedimento = New ConstructorCampoPedimento()

            _operacionesNodo = New OperacionesNodos()

        End Sub
        Sub New(ByVal construir_ As Boolean,
                Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            _tagWatcher = New TagWatcher

            TipoDocumentoElectronico = TiposDocumentoElectronico.PedimentoNormal

            _estructuraDocumento = New EstructuraDocumento(TiposDocumentoElectronico.PedimentoNormal.ToString)

            _constructorCampoPedimento = New ConstructorCampoPedimento()

            _operacionesNodo = New OperacionesNodos()

            If construir_ Then

                Dim ensamblador_ As EnsambladorDocumentos = New EnsambladorDocumentos

                'Autoconstruccion
                ensamblador_.Construye(Me)

                If Not documentoElectronico_ Is Nothing Then

                    With documentoElectronico_

                        Me.IdDocumento = .IdDocumento

                        Me.FolioDocumento = .FolioDocumento

                        Me.UsuarioGenerador = .UsuarioGenerador

                        Me.FolioOperacion = .FolioOperacion

                        Me.IdCliente = .IdCliente

                        Me.NombreCliente = .NombreCliente

                        Me.EstatusDocumento = .EstatusDocumento

                        Me.FechaCreacion = .FechaCreacion

                        Me.EstructuraDocumento = .EstructuraDocumento

                        Me.EstatusDocumento = .EstatusDocumento

                        Me.Id = .Id

                    End With
                Else

                    IdDocumento = 0

                    FolioDocumento = Nothing

                    UsuarioGenerador = Nothing

                    FolioOperacion = Nothing

                    IdCliente = 0

                    NombreCliente = Nothing

                    EstatusDocumento = "0"

                    FechaCreacion = Now()

                    TipoDocumentoElectronico = TiposDocumentoElectronico.PedimentoNormal

                End If

            End If

        End Sub
        Public Sub New(ByVal folioDocumento_ As String,
                       ByVal referencia_ As String,
                       ByVal idCliente_ As Int32,
                       ByVal nombreCliente_ As String
                       )

            _tagWatcher = New TagWatcher

            FolioDocumento = folioDocumento_

            FolioOperacion = referencia_

            IdCliente = idCliente_

            NombreCliente = nombreCliente_

            EstatusDocumento = "1"

            FechaCreacion = Now()

            TipoDocumentoElectronico = TiposDocumentoElectronico.PedimentoNormal

            _estructuraDocumento = New EstructuraDocumento(TiposDocumentoElectronico.PedimentoNormal.ToString)

            Dim ensamblador_ As EnsambladorDocumentos = New EnsambladorDocumentos

            _constructorCampoPedimento = New ConstructorCampoPedimento()

            _operacionesNodo = New OperacionesNodos()

            'Autoconstruccion
            ensamblador_.Construye(Me)

        End Sub
        Public Sub New(ByVal idDocumento_ As Integer,
                       ByVal folioDocumento_ As String,
                       ByVal usuarioGenerador_ As String,
                       ByVal referencia_ As String,
                       ByVal idCliente_ As Int32,
                       ByVal nombreCliente_ As String
                       )

            _tagWatcher = New TagWatcher

            IdDocumento = idDocumento_

            FolioDocumento = folioDocumento_

            UsuarioGenerador = usuarioGenerador_

            FolioOperacion = referencia_

            IdCliente = idCliente_

            NombreCliente = nombreCliente_

            EstatusDocumento = "1"

            FechaCreacion = Now()

            TipoDocumentoElectronico = TiposDocumentoElectronico.PedimentoNormal

            _estructuraDocumento = New EstructuraDocumento(TiposDocumentoElectronico.PedimentoNormal.ToString)

            _constructorCampoPedimento = New ConstructorCampoPedimento()

        End Sub
        Public Sub New(ByVal idDocumento_ As Integer,
                       ByVal folioDocumento_ As String,
                       ByVal fechaCreacion_ As Date,
                       ByVal usuarioGenerador_ As String,
                       ByVal estatusDocumento_ As Integer)

            _tagWatcher = New TagWatcher

            IdDocumento = idDocumento_

            FolioDocumento = folioDocumento_

            FechaCreacion = fechaCreacion_

            UsuarioGenerador = usuarioGenerador_

            EstatusDocumento = estatusDocumento_

            TipoDocumentoElectronico = TiposDocumentoElectronico.PedimentoNormal

            _estructuraDocumento = New EstructuraDocumento(TiposDocumentoElectronico.PedimentoNormal.ToString)

            _constructorCampoPedimento = New ConstructorCampoPedimento()

        End Sub

#End Region

#Region "Methods"
        Public Overrides Sub ConstruyeEncabezado()

            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Encabezado principal del pedimento
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()

            _estructuraDocumento(TiposBloque.EncabezadoPaginasSecundarias) = New List(Of Nodo)

            ' Encabezado para páginas secundarias del pedimento
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS2,
                             tipoBloque_:=TiposBloque.EncabezadoPaginasSecundarias,
                             conCampos_:=True)

        End Sub
        Public Overrides Sub ConstruyeCuerpo()

            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            'Datos del importador/exportador
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS3,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Tasas a nivel pedimento
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS6,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Cuadro de liquidación
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS7,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Deposito referenciado - línea de captura - información del pago
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS9,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Datos del proveedor o comprador
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS10,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Datos del destinatario
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS11,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Datos del transporte y transportista
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS12,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Fechas
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS14,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Candados
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS15,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Guias, Manifiestos, conocimientos de embarque o documentos de transporte
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS16,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Contenedores/Equipo ferrocarril/Número economico del vehiculo.
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS17,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)


            'Identificadores (Nivel pedimento)
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS18,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS19,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Descargos
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS20,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Compensaciones
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS21,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Documentos que amparan las formas de pago distintas a efectivo….
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS22,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Observaciones ( nivel pedimento)
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS23,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)

            'Partidas
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS24,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=False)

            'Fin del pedimento
            ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS43,
                             tipoBloque_:=TiposBloque.Cuerpo,
                             conCampos_:=True)



        End Sub
        Public Overrides Sub ConstruyePiePagina()

            _estructuraDocumento(TiposBloque.PiePagina) = New List(Of Nodo)

            With _estructuraDocumento(TiposBloque.PiePagina)

                'Pie de pagina del pedimento
                ConstruyeSeccion(seccionPedimento_:=SeccionesPedimento.ANS44,
                                 tipoBloque_:=TiposBloque.PiePagina,
                                 conCampos_:=True)

            End With

        End Sub
        Public Sub ConstruyeSeccion(ByVal seccionPedimento_ As SeccionesPedimento,
                                    ByVal tipoBloque_ As TiposBloque,
                                    ByVal conCampos_ As Boolean)

            If Not ExisteSeccion(seccionPedimento_,
                                 tipoBloque_) Then

                With _estructuraDocumento(tipoBloque_)

                    .Add(New NodoPedimento() With {
                                   .Nodos = EnsamblarSeccion(idSeccion_:=seccionPedimento_,
                                                             conCampos_:=conCampos_)
                         })

                End With

            End If

        End Sub

#End Region

#Region "Funciones"
        Public Function EnsamblarSeccion(ByVal idSeccion_ As Integer,
                                         Optional conCampos_ As Boolean = True) As List(Of Nodo)

            Dim seccionPedimento_ = New SeccionPedimento()

            Dim nodos_ = New List(Of Nodo)

            With seccionPedimento_

                .SeccionPedimento = idSeccion_

                .TipoDocumentoElectronico = TipoDocumentoElectronico

                If conCampos_ Then

                    .Nodos = ObtenerCamposSeccion(idSeccion_)

                End If

            End With

            nodos_.Add(seccionPedimento_)

            Return nodos_

        End Function
        Public Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                'Encabezado principal del pedimento
                Case SeccionesPedimento.ANS1

                    'NodoPedimento
                    Return New List(Of Nodo) From {
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO_COMPLETO)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_T_OPER)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PEDIMENTO)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_REGIMEN)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DESTINO_ORIGEN)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TIPO_CAMBIO)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PESO_BRUTO)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TIPO_CAMBIO)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PESO_BRUTO)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_E_S)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MEDIO_DE_TRANSPORTE)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MEDIO_DE_TRANSPORTE_DE_ARRIBO)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MEDIO_DE_TRANSPORTE_DE_SALIDA)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_VALOR_DOLARES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_VALOR_ADUANA)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PRECIO_PAGADO_O_VALOR_COMERCIAL)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_VAL_SEGUROS)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_SEGUROS)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FLETES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_EMBALAJES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_OTROS_INCREMENTABLES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TRANSPORTE_DECREMENTABLES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_SEGURO_DECREMENTABLES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CARGA_DECREMENTABLES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DESCARGA_DECREMENTABLES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_OTROS_DECREMENTABLES)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ACUSE_ELECTONICO_DE_VALIDACION)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CODIGO_DE_BARRAS)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CLAVE_SAD)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MARCAS_NUMEROS_TOTAL_BULTOS)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CERTIFICACION)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_DESPACHO)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_AÑO_VALIDACION_2)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_SIN_SECCION)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_TIPO_OPERACION)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_VALIDACION)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CP_REFERENCIA)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CP_MODALIDAD_ADUANA_PATENTE)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CP_MODALIDAD)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CP_EJECUTIVO_DE_CUENTA)},
                                   New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CP_NUMERO_CLIENTE)}
                             }

                ' Encabezado para páginas secundarias del pedimento
                Case SeccionesPedimento.ANS2

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TIPO_CAMBIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO_COMPLETO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_T_OPER)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PEDIMENTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_RFC_DEL_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CURP_DEL_IOE)}
                         }

                ' Datos del importador/exportador
                Case SeccionesPedimento.ANS3

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_RFC_DEL_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CURP_DEL_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_RAZON_SOCIAL_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DOMICILIO_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CALLE_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_INTER_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_EXTERIOR_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CODIGO_POSTAL_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MUNICIPIO_CIUDAD_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ENTIDAD_FEDERATIVA_IOE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PAIS_IOE)}
                         }

                ' Datos generales del pedimento complementario
                Case SeccionesPedimento.ANS4

                    Return New List(Of Nodo)

                        ' Prueba suficiente
                Case SeccionesPedimento.ANS5

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CONCEPTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FP)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_IMPORTE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_CONCEPTO_NIVEL_PEDIMENTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PAIS_EXPORT)}
                         }

                        ' Tasas a nivel pedimento
                Case SeccionesPedimento.ANS6

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CONTRIBUCION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_T_TASA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TASA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_CONCEPTO_NIVEL_PEDIMENTO)}
                         }

                        ' Cuadro de liquidación
                Case SeccionesPedimento.ANS7

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_EFECTIVO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_OTROS)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TOTAL)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS55, False)}
                         }

                    'Partidas del cuadro de liquidación
                Case SeccionesPedimento.ANS55

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CONCEPTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FP)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_IMPORTE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_CONCEPTO_NIVEL_PEDIMENTO)}
                         }

                        ' Informe de la industria automotriz
                Case SeccionesPedimento.ANS8

                    Return New List(Of Nodo)

                        ' Deposito referenciado - línea de captura - información del pago
                Case SeccionesPedimento.ANS9

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DEP_REFERENCIADO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PATENTE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PAGO_ELECTRONICO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_INST_BANCARIA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_LINEA_CAPTURA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_OPERACION_BANCARIA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_TRANSACCION_SAT)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MEDIO_PRESENTACION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MEDIO_RECEPCION_COBRO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_DESPACHO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_EFECTIVO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_PAGO)}
                         }

                        ' Datos del proveedor o comprador
                Case SeccionesPedimento.ANS10

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ID_FISCAL_PROVEEDOR)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_DEN_RAZON_SOC_POC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DOMICILIO_POC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_VINCULACION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_VINCULACION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CALLE_POC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUMERO_INT_POC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUMERO_EXTER_POC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CODIGO_POSTAL_POC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MUNICIPIO_CIUDAD_POC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ENTIDAD_FEDERATIVA_POC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PAIS_POC)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS13, False)}
                         }

                        ' Datos del destinatario
                Case SeccionesPedimento.ANS11

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ID_FISCAL_DESTINATARIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_RAZON_SOC_DESTINATARIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DOMICILIO_DESTINATARIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CALLE_DESTINATARIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUMERO_INT_DESTINATARIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUMERO_EXTER_DESTINATARIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CODIGO_POSTAL_DESTINATARIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MUNICIPIO_CIUDAD_DESTINATARIO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PAIS_DESTINATARIO)}
                         }

                        ' Datos del transporte y transportista
                Case SeccionesPedimento.ANS12

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ID_TRANSPORT)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PAIS_TRANSP)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_RAZON_SOC_TRANSP)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_RFC_TRANSP)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CURP_TRANSP_PERSONA_FISICA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DOMICILIO_TRANSP)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TOTAL_BULTOS)}
                         }

                        ' CFDi o ducumento equivalente
                Case SeccionesPedimento.ANS13

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CFDI_O_FACT)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_FACT)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_INCOTERM)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_MONEDA_FACT)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_MONEDA_FACT)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FACTOR_MONEDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_USD)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUMERO_ACUSE_DE_VALOR)}
                         }

                        ' Fechas
                Case SeccionesPedimento.ANS14

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_ENTRADA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_PAGO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_EXTRACCION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_PRESENTACION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_IMP_EUA_CAN)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_ORIGINAL)}
                         }

                        ' Candados
                Case SeccionesPedimento.ANS15

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_CANDADO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_CANDADO_1RA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_CANDADO_2DA)}
                         }

                        ' Guias, Manifiestos, conocimientos de embarque o documentos de transporte
                Case SeccionesPedimento.ANS16

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_GUIA_O_MANIF_O_BL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MASTER_O_HOUSE)}
                         }



                        ' Contenedores/Equipo ferrocarril/Número economico del vehiculo
                Case SeccionesPedimento.ANS17

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_CONTENEDOR_FERRO_NUM_ECON)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_TIPO_CONTENEDOR)}
                         }

                        ' Identificadores (Nivel pedimento)
                Case SeccionesPedimento.ANS18

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_IDENTIFICADOR_G)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_COMPL_1)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_COMPL_2)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_COMPL_3)}
                         }


                        ' Cuentas aduaneras y cuentas aduaneras de garantia (Nivel pedimento)
                Case SeccionesPedimento.ANS19

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_CTA_ADUANERA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_TIPO_GARANTIA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_INST_EMISORA_CTA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_CONTRATO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FOLIO_CONSTANCIA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_IMPORTE_TOTAL_CONSTANCIA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_EMISION_CONSTANCIA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CANTIDAD_UMT_PRECIO_ESTIMADO_PEDIMENTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TITULOS_ASIGNADOS_PEDIMENTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_VALOR_UNITARIO_TITULO_PEDIMENTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_INSTITUCION_EMISORA_CTA_ADUANERA_PEDIMENTO)}
                         }

                        ' Descargos
                Case SeccionesPedimento.ANS20

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIM_ORIGINAL_COMPLETO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_PEDIM_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PEDIM_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_AÑO_VALIDACION_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_AÑO_VALIDACION_2_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PATENTE_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL_2)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FRACCION_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_UM_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CANT_MERCANCIA_UMT_DESCARGO)}
                         }

                        ' Compensaciones
                Case SeccionesPedimento.ANS21

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CONTRIBUCION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIM_ORIGINAL_COMPLETO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_PAGO_ORIG_PARA_COMPENSAC)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_IMPORTE_GRAVAMEN_COMPENSACION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_CONCEPTO_COMPENSACION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_AÑO_VALIDACION_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_AÑO_VALIDACION_2_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PATENTE_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL_2)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS)}
                         }


                        ' Documentos que amparan las formas de pago distintas a efectivo
                Case SeccionesPedimento.ANS22

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FP)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_INST_EMISORA_DOCTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_DOCTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_EXP_DOCTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_DOCTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_SALDO_DISP_DOCTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_PAG_PEDIM)}
                         }

                        ' Observaciones ( nivel pedimento)
                Case SeccionesPedimento.ANS23

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_OBSERV_PEDIM)}
                         }

                        ' Partidas
                Case SeccionesPedimento.ANS24

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_SEC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FRACC_ARANC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NICO_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_MET_VALOR_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_UMC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CANT_UMC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_UMT_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CANT_UMT_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PAIS_VEND_O_COMP_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PAIS_ORIGEN_O_DEST_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DESCRIP_MERC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_VAL_ADU_O_VAL_USD_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_USD)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_IMP_PRECIO_PAG_O_VAL_COMER_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_PRECIO_UNITARIO_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_VALOR_AGREG_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_MARCA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_MODELO_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CODIGO_PRODUCTO_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_VINCULACION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_VINCULACION)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ENTIDAD_FEDER_ORIGEN)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ENTIDAD_FEDER_DESTINO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ENTIDAD_FEDER_COMPRADOR)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ENTIDAD_FEDER_VENDEDOR)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_USD)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS25, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS26, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS27, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS28, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS29, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS32, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS33, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS34, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS35, False)},
                               New NodoPedimento With {.Nodos = EnsamblarSeccion(SeccionesPedimento.ANS36, False)}
                         }

                        ' Mercancias
                Case SeccionesPedimento.ANS25

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_VIN_O_NUM_SERIE_MERCA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_KILOM)}
                         }

                        ' Regulaciones y restricciones no arancelarias
                Case SeccionesPedimento.ANS26

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PERMISO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PERMISO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FIRM_ELECTRON_PERMISO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_USD_VAL_COM)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CANT_UMT_O_UMC)}
                         }

                        ' Identificadores ( Nivel partida )
                Case SeccionesPedimento.ANS27

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_IDENTIF_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_COMPL_1_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_COMPL_2_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_COMPL_3_PARTIDA)}
                         }

                        ' Cuentas aduaneras de garantia ( Nivel partida)
                Case SeccionesPedimento.ANS28

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_TIPO_GARANTIA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_INST_EMISORA_GARANTIA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_EXP_CONSTANCIA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_CTA_GARANTIA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FOLIO_CONSTANCIA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_TOTAL_CONSTANCIA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PRECIO_ESTIMADO_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CANT_UMT_PRECIO_ESTIMADO_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_INSTITUCION_EMISORA_CTA_ADUANERA_PARTIDA)}
                         }

                        ' Tasas y contribuciones a nivel partida
                Case SeccionesPedimento.ANS29

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CONTRIBUCION_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TASA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_T_TASA_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FP_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_IMPORTE_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_CONTRIBUCION_NIVEL_PARTIDA)}
                         }

                        ' Contribuciones a nivel partida
                Case SeccionesPedimento.ANS30

                    Return New List(Of Nodo)

                        ' Partidas del informe de la industria automotriz
                Case SeccionesPedimento.ANS31

                    Return New List(Of Nodo)

                        ' Determinación de contribuciones a nivel partida al amparo del Art 2.5 del T-MEC
                Case SeccionesPedimento.ANS32

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_MERC_NO_ORIGIN_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_IGI_PARTIDA)}
                         }

                        ' Detalle de importación a EUA/CAN al amparo del Art. 2.5 del T-MEC
                Case SeccionesPedimento.ANS33

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_SEC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FRACC_ARANC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PAIS_EXPORT)}
                         }

                        ' Determinación y/o pago de contribuciones por aplicación del art 2.5 del TMEC en el pedimento de exporación(Retorno)
                Case SeccionesPedimento.ANS34

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_SEC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FRACC_ARANC_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_MERC_NO_ORIGIN_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_MONTO_IGI_PARTIDA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO)}
                         }

                        ' Pago de contribuciones a nivel partida por aplicación del Art. 2.5 del T-MEC
                Case SeccionesPedimento.ANS35

                    Return New List(Of Nodo) From {
                                New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_SEC_PARTIDA)},
                                New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FRACC_ARANC_PARTIDA)},
                                New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CONTRIBUCION_PARTIDA)},
                                New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FP_PARTIDA)},
                                New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_IMPORTE_PARTIDA)},
                                New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO)},
                                New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PAIS_EXPORT)}
                        }

                         ' Observaciones ( Nivel partida )
                Case SeccionesPedimento.ANS36

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_OBSERV_PARTIDA)}
                         }

                        ' Rectificaciones
                Case SeccionesPedimento.ANS37

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_PEDIM_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PEDIM_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PATENTE_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_AÑO_VALIDACION_2_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_AÑO_VALIDACION_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL_2)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PEDIM_RECTIF)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FECHA_PAG_PEDIM_RECTIFICACION)}
                    }

                        ' Diferencias de contribuciones ( Nivel pedimento )
                Case SeccionesPedimento.ANS38

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DIFERENCIA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DIFERENCIA_EFECTIVO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DIFERENCIA_OTROS)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_DIFERENCIA_TOTAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CONCEPTO_DIF_CONTRIB)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_CONCEPTO_DIF_CONTRIB)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FORMA_PAGO_DIFERENCIAS)}
                    }

                        ' Prueba suficiente
                Case SeccionesPedimento.ANS39

                    Return New List(Of Nodo)

                        ' Encabezado para determinacion de contribuciones a nivel partdida para pedimentos complementarios al amparo del art. T-Mec
                Case SeccionesPedimento.ANS40

                    Return New List(Of Nodo)

                        ' Encabezado para determinación de contribuciones a nivel partida para pedimentos complementarios al amparo del los articulos 14 de la decision o 15 del TLCAELC
                Case SeccionesPedimento.ANS41

                    Return New List(Of Nodo)

                        ' Instructivo de llenado del pedimento de tránsito para el transbordo
                Case SeccionesPedimento.ANS42

                    Return New List(Of Nodo)

                        ' Fin de pedimento
                Case SeccionesPedimento.ANS43

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_FIN_PEDIMENTO)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NUMERO_TOTAL_PARTIDAS)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CVE_PREVAL)}
                         }

                        ' Pie de pagina del pedimento
                Case SeccionesPedimento.ANS44

                    Return New List(Of Nodo) From {
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_RFC_AA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CURP_AA_O_REP_LEGAL)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_NOMBRE_MAND_REP_AA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_RFC_MAND_O_AGAD_REP_ALMACEN)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CURP_MAND_O_AGAD_REP_ALMACEN)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_PATENTE)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_EFIRMA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_CERTIFICADO_FIRMA)},
                               New NodoPedimento With {.Nodos = _constructorCampoPedimento.EnsamblarCampo(CamposPedimento.CA_TIPO_FIGURA)}
                         }


                Case Else

                    _tagWatcher.SetError(Me, "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function
        Public Function Clone() As Object Implements ICloneable.Clone

            Dim ms_ As New MemoryStream

            Dim objResult_ As Object = Nothing

            Try

                Dim bf As New BinaryFormatter

                bf.Serialize(ms_, Me)

                ms_.Position = 0

                objResult_ = bf.Deserialize(ms_)

            Catch ex As Exception

                _tagWatcher.SetError(Me, "Alguna clase no esta serializada, a continuación el error de excepción:" & ex.Message)

            Finally

                ms_.Close()

            End Try

            Return objResult_

        End Function
        Public Function Clone(ByVal documentoClonado_ As Object) As Object

            documentoClonado_ = Me.Clone()

            Return documentoClonado_

        End Function

#End Region


    End Class

End Namespace