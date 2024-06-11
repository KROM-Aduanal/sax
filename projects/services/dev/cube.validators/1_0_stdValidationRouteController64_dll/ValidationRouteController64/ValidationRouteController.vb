

Imports Cube
Imports Cube.ValidatorReport
Imports Cube.Validators
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Wma.Exceptions

Public Class ValidationRouteController
    Implements IValidationRouteController, ICloneable, IDisposable

#Region "Attributes"

    Private _route As IValidationRouteController.ValidationRoutes

    Private _validationpanel As validationpanel

    Private _checkfields As List(Of CamposPedimento)

    Private _pedimento As DocumentoElectronico

    Private _cubedatos As ICubeController

    Private _borderfields As Dictionary(Of CamposPedimento, Boolean)

    Private _status As TagWatcher

    Private _report As ValidatorReport


#End Region


#Region "Properties"



    Public Property route As IValidationRouteController.ValidationRoutes Implements IValidationRouteController.route

        Get
            Return _route

        End Get

        Set(value As IValidationRouteController.ValidationRoutes)

            _route = value

        End Set

    End Property

    Public Property checkfields As List(Of CamposPedimento) Implements IValidationRouteController.checkfields

        Get

            Return _checkfields

        End Get

        Set(value As List(Of CamposPedimento))

            _checkfields = value

        End Set

    End Property

    Public Property pedimento As DocumentoElectronico Implements IValidationRouteController.pedimento

        Get

            Return _pedimento

        End Get

        Set(value As DocumentoElectronico)

            _pedimento = value

        End Set

    End Property

    Public Property cubedatos As ICubeController Implements IValidationRouteController.cubedatos

        Get
            Throw New NotImplementedException()

        End Get

        Set(value As ICubeController)

            Throw New NotImplementedException()

        End Set

    End Property

    Public Property borderfields As Dictionary(Of CamposPedimento, Boolean) Implements IValidationRouteController.borderfields

        Get

            Return _borderfields

        End Get

        Set(value As Dictionary(Of CamposPedimento, Boolean))

            _borderfields = value

        End Set

    End Property

    Public Property status As TagWatcher Implements IValidationRouteController.status

        Get

            Return _status

        End Get

        Set(value As TagWatcher)

            _status = value

        End Set

    End Property

    Public Property report As ValidatorReport Implements IValidationRouteController.report

        Get

            Return _report

        End Get

        Set(value As ValidatorReport)

            _report = value

        End Set

    End Property

#End Region

#Region "Builders"

    Sub New()

        _cubedatos = New CubeController


    End Sub

#End Region

#Region "Methods"



    Public Function ValidateFields(Of T)(route_ As IValidationRouteController.ValidationRoutes, checkfield_ As CamposPedimento, value_ As T) As ValidatorReport Implements IValidationRouteController.ValidateFields
        Throw New NotImplementedException()
    End Function

    Public Function ValidateFields(Of T)(route_ As IValidationRouteController.ValidationRoutes, checkfields_ As Dictionary(Of CamposPedimento, T)) As ValidatorReport Implements IValidationRouteController.ValidateFields
        Throw New NotImplementedException()
    End Function

    Public Function ValidatePedimento(Of T)(route_ As IValidationRouteController.ValidationRoutes, pedimento_ As DocumentoElectronico) As ValidatorReport Implements IValidationRouteController.ValidatePedimento

        Select Case route_

            Case IValidationRouteController.ValidationRoutes.RUVA4

                Return ValidateRoute4(pedimento_)

            Case IValidationRouteController.ValidationRoutes.RUVA21


                Return ValidateRoute21(pedimento_)

            Case Else

                Return ValidateRoute21(pedimento_)


        End Select



    End Function

    Private Function RunRoom(Of T)(roomname_ As String, params_ As Dictionary(Of String, T)) As ValidatorReport
        Throw New NotImplementedException()
    End Function

    Private Function GetReports(Of T)() As ValidatorReport
        Throw New NotImplementedException()
    End Function

    Private Function GetReports(Of T)(field_ As CamposPedimento) As ValidatorReport
        Throw New NotImplementedException()
    End Function

    Private Function GetFieldResource() As TagWatcher
        Throw New NotImplementedException()
    End Function

    Private Function ValidateRoute4(pedimento_ As DocumentoElectronico) As ValidatorReport


        Dim mensaje_ = ""

        Dim destinoOrigen_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_DESTINO_ORIGEN), "")

        If destinoOrigen_ = "" Then

            mensaje_ = "Falta Especificar el valor del campo DESTINO/ORIGEN"

        End If

        Dim aduanaES_ As String = GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_ADUANA_ENTRADA_SALIDA)

        aduanaES_ = If(aduanaES_, "")


        If aduanaES_ = "" Then

            mensaje_ &= Chr(13) & "Falta Especificar el valor del campo ADUANA E/S"

        End If

        Dim medioTransporte_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE), "")

        If medioTransporte_ = "" Then

            mensaje_ &= Chr(13) & "Falta Especificar el valor del campo MEDIO DE TRANSPORTE"

        End If

        Dim validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_PESO_BRUTO",
                                             New Dictionary(Of String, String) From {{"S1.CA_PESO_BRUTO.0",
                                                                                      GetAttributeValue(pedimento_,
                                                                                                        SeccionesPedimento.ANS1,
                                                                                                        CamposPedimento.CA_PESO_BRUTO)
                                                                                    }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta Especificar el valor del campo Peso Bruto"

                End If



            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_MARCAS_NUMEROS_TOTAL_BULTOS_EXPORTACION_NORMAL",
                                         New Dictionary(Of String, String) From {{"S1.CA_MARCAS_NUMEROS_TOTAL_BULTOS.0",
                                                                                    GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS1,
                                                                                                      CamposPedimento.CA_MARCAS_NUMEROS_TOTAL_BULTOS)
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo marcas, numeros y total de bultos"

                End If



            End If

        End If

        Dim medioTransporteArribo_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE_ARRIBO), "")

        If medioTransporteArribo_ = "" Then

            mensaje_ &= Chr(13) & "Falta Especificar el valor del campo MEDIO DE TRANSPORTE DE ARRIBO"

        End If

        Dim medioTransporteSalida_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE_SALIDA), "")

        If medioTransporteSalida_ = "" Then

            mensaje_ &= Chr(13) & "Falta Especificar el valor del campo MEDIO DE TRANSPORTE DE ARRIBO"

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_RFC_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_RFC_IOE.0",
                                                                                    If(GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS3,
                                                                                                      CamposPedimento.CA_RFC_IOE), "")
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo RFC DEL EXPORTADOR"

                End If



            End If

        End If

        Dim moral_ = True

        If moral_ Then

        Else

            validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_CURP_IOE",
                                             New Dictionary(Of String, String) From {{"S3.CA_CURP_IOE.0",
                                                                                        If(GetAttributeValue(pedimento_,
                                                                                                          SeccionesPedimento.ANS3,
                                                                                                          CamposPedimento.CA_CURP_IOE), "")
                                                                                     }})
            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then

                    Else

                        mensaje_ &= Chr(13) & "Falta especificar el valor del campo CURP DEL IMPORTADOR/EXPORTADOR"

                    End If



                End If

            End If

        End If





        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_RAZON_SOCIAL_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_RAZON_SOCIAL_IOE.0",
                                                                                   If(GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS3,
                                                                                                      CamposPedimento.CA_RAZON_SOCIAL_IOE, False), "")
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACIÓN SOCIAL DEL IMPORTADOR/EXPORTADOR"

                End If



            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_DOMICILIO_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_DOMICILIO_IOE.0",
                                                                                   If(GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS3,
                                                                                                      CamposPedimento.CA_DOMICILIO_IOE, False), "")
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo DOMICILIO DEL IMPORTADOR/EXPORTADOR"

                End If

            End If

        End If

        Dim tipoOperacion_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_TIPO_OPERACION), "")

        If tipoOperacion_ = "" Then

            mensaje_ &= Chr(13) & "Falta Especificar el valor del campo TIPO DE OPERACIÓN"

        End If

        Dim cvePedimento_ = If(GetAttributeValue(pedimento_,
                                             SeccionesPedimento.ANS1,
                                             CamposPedimento.CA_CVE_PEDIMENTO, False).SubString(0, 2), "")
        If cvePedimento_ = "" Then

            mensaje_ &= Chr(13) & "Falta Especificar el valor del campo CVE. PEDIMENTO"

        End If

        Dim cveRegimen_ = If(GetAttributeValue(pedimento_,
                                             SeccionesPedimento.ANS1,
                                             CamposPedimento.CA_REGIMEN, False).SubString(0, 2), "")
        If cvePedimento_ = "" Then

            mensaje_ &= Chr(13) & "Falta Especificar el valor del campo REGIMEN"

        End If

        Dim razonSocialPOC_ = If(GetAttributeValue(pedimento_,
                                                  SeccionesPedimento.ANS10,
                                                  CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC, False), "")

        If razonSocialPOC_ = "" Then

            mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACION O RAZON SOCIAL. DEL COMPRADOR"

        End If


        'AQUI PUEDEN IR VARIOS EVALUAR LA SECCIÓN DATOS DEL PROVEEDOR ARREGLO 12

        Dim cfdiFactura_ = If(GetAttributeValue(pedimento_,
                                                SeccionesPedimento.ANS10,
                                                CamposPedimento.CA_CFDI_FACTURA), "")

        If cfdiFactura_ = "" Then

            mensaje_ &= Chr(13) & "Falta especificar el valor del campo NUM. CFDI O DOCUMENTO EQUIVALENTE."

        End If


        Dim facturaDate_ = If(GetAttributeValue(pedimento_,
                                            SeccionesPedimento.ANS13,
                                            CamposPedimento.CA_FECHA_FACTURA), "")

        Dim validationDateValue_ = If(GetAttributeValue(pedimento_,
                                                    SeccionesPedimento.ANS1,
                                                    CamposPedimento.CA_FECHA_VALIDACION), "")
        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_FACTURA",
                                         New Dictionary(Of String, String) From {{"S13.CA_FECHA_FACTURA.0",
                                                                                   facturaDate_
                                                                                 },
                                                                                 {"S1.CA_FECHA_VALIDACION.0",
                                                                                   validationDateValue_
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el campo FECHA DE FACTURA '" &
                                           facturaDate_ &
                                          "' debe ser menor o igual a la FECHA DE VALIDACIÓN '" &
                                          validationDateValue_ & "'"

                End If



            End If

        End If

        Dim cveMonedaFactura_ = If(GetAttributeValue(pedimento_,
                                                    SeccionesPedimento.ANS13,
                                                    CamposPedimento.CA_CVE_MONEDA_FACTURA),
                                  "")

        If cveMonedaFactura_ = "" Then

            mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACION O RAZON SOCIAL. DEL COMPRADOR"

        End If


        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_MONTO_MONEDA_FACTURA",
                                         New Dictionary(Of String, String) From {{"S13.CA_MONTO_MONEDA_FACTURA.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS13,
                                                                                                      CamposPedimento.CA_MONTO_MONEDA_FACTURA)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo VAL. MON. FACT."

                End If



            End If

        End If



        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_ACUSE_ELECTRONICO_VALIDACION_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S1.CA_ACUSE_ELECTRONICO_VALIDACION.0",
                                                                                   If(GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS1,
                                                                                                      CamposPedimento.CA_ACUSE_ELECTRONICO_VALIDACION), "")
                                                                                 },
                                                                                 {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   cvePedimento_
                                                                                 },
                                                                                 {"SFAC1.CP_APLICA_ENAJENACION.0",
                                                                                  "NO"
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NÚMERO DE ACUSE DE VALOR"

                End If

            End If

        End If


        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_INCOTERM",
                                         New Dictionary(Of String, String) From {{"S13.CA_INCOTERM.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS13,
                                                                                                      CamposPedimento.CA_INCOTERM)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo INCOTERM."

                End If

            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO",
                                         New Dictionary(Of String, String) From {{"S11.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS11,
                                                                                                      CamposPedimento.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO, False)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACION O RAZON SOCIAL. DEL DESTINATARIO"

                End If

            End If

        End If


        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA",
                                         New Dictionary(Of String, String) From {{"S44.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS44,
                                                                                                      CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, False)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACIÓN O RAZ. SOC.. DEL AGENTE ADUANAL"

                End If

            End If

        End If

        Dim fechaPago_ = GetAttributeValue(pedimento_,
                                           SeccionesPedimento.ANS14,
                                           CamposPedimento.CA_FECHA_PAGO, False)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_PAGO",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_PAGO.0",
                                                                                   fechaPago_
                                                                                 },
                                                                                 {"S1.CA_FECHA_VALIDACION.0",
                                                                                   validationDateValue_
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If fechaPago_ IsNot Nothing Then

                        mensaje_ &= Chr(13) & "Error en el valor del campo FECHA PAGO. '" &
                                              fechaPago_ & "'" & Chr(13) &
                                              " debe ser igual a la fecha de validación '" &
                                              validationDateValue_ & "'"

                    Else

                        mensaje_ &= Chr(13) & "Error en el valor del campo FECHA PAGO. ''"


                    End If


                End If

            End If

        End If


        'AQUI SE HARÁ UN CICLO PARA REVISAR TODA LA SECCIÓN YA QUE PUEDE HABER VARIOS IDENTIFICADOR A NIVEL PEDIMENTO

        Dim seccion_ = pedimento_.Seccion(SeccionesPedimento.ANS18)

        For Each Nodo_ In seccion_.Nodos

            Dim cveIdentificador_ As String = If(DirectCast(Nodo_.Nodos(0).Nodos(0), Campo).Valor, "")


            validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_CVE_IDENTIFICADOR",
                                             New Dictionary(Of String, String) From {{"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                      cveIdentificador_
                                                                                     }})
            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then


                        Dim complemento1_ As String = If(DirectCast(Nodo_.Nodos(1).Nodos(0), Campo).Valor, "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_COMPLEMENTO_1",
                                                         New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_1.0",
                                                                                                   complemento1_
                                                                                                 },
                                                                                                  {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                                  cveIdentificador_
                                                                                                 }})
                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    If complemento1_ = "" Then


                                        mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 1 para el identificador '" &
                                                                   cveIdentificador_ &
                                                                   "' el complemento no debe estar vacío"


                                    Else

                                        mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 1 valor inválido '" &
                                                               complemento1_ &
                                                               "' para el identificador '" &
                                                               cveIdentificador_ &
                                                               "' el complemento debe estar vacío"

                                    End If


                                End If

                            End If

                        End If

                        Dim complemento2_ As String = If(DirectCast(Nodo_.Nodos(2).Nodos(0), Campo).Valor, "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_COMPLEMENTO_2",
                                                         New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_2.0",
                                                                                                  complemento2_
                                                                                                 },
                                                                                                 {"S18.CA_COMPLEMENTO_1.0",
                                                                                                  complemento1_
                                                                                                 },
                                                                                                  {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                                  cveIdentificador_
                                                                                                 }})
                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    If complemento2_ = "" Then

                                        mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 2 para el identificador '" &
                                                               cveIdentificador_ &
                                                               "' el complemento no debe estar vacío"

                                    Else

                                        mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 2 valor inválido '" &
                                                                   complemento2_ &
                                                                   "' para el identificador '" &
                                                                   cveIdentificador_ &
                                                                   "' el complemento debe estar vacío"

                                    End If

                                End If

                            End If

                        End If

                        Dim complemento3_ As String = If(DirectCast(Nodo_.Nodos(3).Nodos(0), Campo).Valor, "")


                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_COMPLEMENTO_3",
                                                         New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_3.0",
                                                                                                   complemento3_
                                                                                                 },
                                                                                                 {"S18.CA_COMPLEMENTO_1.0",
                                                                                                   complemento1_
                                                                                                 },
                                                                                                  {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                                  cveIdentificador_
                                                                                                 }})
                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else


                                    If complemento3_ = "" Then

                                        mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 3 para el identificador '" &
                                                               cveIdentificador_ &
                                                               "' el complemento no debe estar vacío"

                                    Else

                                        mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 3 valor inválido '" &
                                                               complemento3_ &
                                                               "' para el identificador '" &
                                                               cveIdentificador_ & "' el complemento debe estar vacío"

                                    End If


                                End If

                            End If

                        End If

                    Else


                        mensaje_ &= Chr(13) & "Error en el campo CLAVE. IDENTIFICADOR. Valor inválido '" &
                                                   cveIdentificador_ & "'"

                    End If

                End If

            End If

        Next



        Dim fechapresentacion_ = If(GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS14,
                                                   CamposPedimento.CA_FECHA_PRESENTACION), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_PRESENTACION",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_PRESENTACION.0",
                                                                                     fechapresentacion_
                                                                                 },
                                                                                  {"S14.CA_FECHA_PAGO.0",
                                                                                  fechaPago_
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo FECHA PRESENTACION '" & fechapresentacion_ & "'" & Chr(13) & "('La fecha de presentación debe ser igual a la fecha de pago')"

                End If

            End If

        End If

        Dim fechaExtraccion_ = GetAttributeValue(pedimento_,
                                                SeccionesPedimento.ANS14,
                                                CamposPedimento.CA_FECHA_EXTRACCION)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_EXTRACCION",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_EXTRACCION.0",
                                                                                  fechaExtraccion_
                                                                                 },
                                                                                  {"S14.CA_FECHA_PAGO.0",
                                                                                  fechaPago_
                                                                                 },
                                                                                  {"S1.CA_CVE_PEDIMENTO.0",
                                                                                  cvePedimento_
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If fechaExtraccion_ > fechaPago_ Then

                        mensaje_ &= Chr(13) & "Error en el valor del campo FECHA EXTRACCIÓN '" &
                                              If(fechaExtraccion_ Is Nothing, "", fechaExtraccion_) &
                                             "'" & Chr(13) & "(' debe ser menor o igual a la fecha de PAGO '" & fechaPago_ & "')"

                    Else

                        mensaje_ &= Chr(13) & "Error en el valor del campo FECHA EXTRACCIÓN '" &
                                             If(fechaExtraccion_ Is Nothing, "", fechaExtraccion_) &
                                             "'" & Chr(13) & "('La fecha de EXTRACCIÓN no es compatible con la clave de pedimento puesta '" &
                                             If(cvePedimento_ Is Nothing, "", cvePedimento_) & "')"

                    End If

                End If

            End If

        End If

        Dim idTransporte_ = If(GetAttributeValue(pedimento_,
                                              SeccionesPedimento.ANS12,
                                              CamposPedimento.CA_ID_TRANSPORTE), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_ID_TRANSPORTE",
                                         New Dictionary(Of String, String) From {{"S12.CA_ID_TRANSPORTE.0",
                                                                                   idTransporte_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo CA_ID_TRANSPORTE '" &
                                           If(idTransporte_ Is Nothing, "", idTransporte_) & "')"

                End If

            End If

        End If

        Dim cvePaisTransporte_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS12,
                                                   CamposPedimento.CA_CVE_PAIS_TRANSPORTE)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CVE_PAIS_TRANSPORTE",
                                         New Dictionary(Of String, String) From {{"S12.CA_CVE_PAIS_TRANSPORTE.0",
                                                                                   cvePaisTransporte_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo CLAVE DEL PAIS DE TRANSPORTE '" &
                                          If(cvePaisTransporte_ Is Nothing, "", cvePaisTransporte_) & "')"

                End If

            End If

        End If

        Dim numeroCandado_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS15,
                                                   CamposPedimento.CA_NUMERO_CANDADO)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NUMERO_CANDADO",
                                         New Dictionary(Of String, String) From {{"S15.CA_NUMERO_CANDADO.0",
                                                                                   "NONAME"
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo NÚMERO DE CANDADO ''"

                End If

            End If

        End If

        Dim guiaManifiestoBL_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS16,
                                                   CamposPedimento.CA_GUIA_MANIFIESTO_BL)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_GUIA_MANIFIESTO_BL_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S16.CA_GUIA_MANIFIESTO_BL.0",
                                                                                   guiaManifiestoBL_
                                                                                },
                                                                                {"S1.CA_MEDIO_TRANSPORTE.0",
                                                                                   medioTransporte_
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If If(guiaManifiestoBL_ Is Nothing, "", guiaManifiestoBL_) = "" Then

                        mensaje_ &= Chr(13) & "Error en el valor del campo NUMERO (GUIA/CONOCIMIENTO EMBARQUE) '' " & Chr(13) &
                                              " no debe ir vacio para el medio de transporte '" &
                                              If(medioTransporte_ Is Nothing, "", medioTransporte_) & "'"

                    Else


                        mensaje_ &= Chr(13) & "Error en el valor del campo NUMERO (GUIA/CONOCIMIENTO EMBARQUE) '" & guiaManifiestoBL_ & "'" & Chr(13) &
                                              "debe ir vacio para el medio de transporte '" &
                                              If(medioTransporte_ Is Nothing, "", medioTransporte_) & "'"


                    End If


                End If

            End If

        End If

        If guiaManifiestoBL_ <> "" Then

            Dim masterHouse_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS16,
                                                   CamposPedimento.CA_MASTER_HOUSE)

            validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_MASTER_HOUSE",
                                             New Dictionary(Of String, String) From {{"S16.CA_MASTER_HOUSE.0",
                                                                                       masterHouse_
                                                                                    }})

            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then

                    Else

                        mensaje_ &= Chr(13) & "Error en el valor del campo MASTER HOUSE '" &
                                              If(masterHouse_ Is Nothing, "", masterHouse_) & "'"

                    End If

                End If

            End If



            validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO",
                                             New Dictionary(Of String, String) From {{"S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO.0",
                                                                                       GetAttributeValue(pedimento_,
                                                       SeccionesPedimento.ANS17,
                                                       CamposPedimento.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO)
                                                                                    }})

            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then

                    Else

                        mensaje_ &= Chr(13) & "Error en campo NUMERO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO. 'está vacío'"


                    End If

                End If

            End If

            Dim cveTipoContenedor_ = GetAttributeValue(pedimento_,
                                                       SeccionesPedimento.ANS17,
                                                       CamposPedimento.CA_CVE_TIPO_CONTENEDOR)

            validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_CVE_TIPO_CONTENEDOR",
                                             New Dictionary(Of String, String) From {{"S17.CA_CVE_TIPO_CONTENEDOR.0",
                                                                                       cveTipoContenedor_
                                                                                    }})

            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then

                    Else

                        mensaje_ &= Chr(13) & "Error en campo TIPO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO. '" &
                                          If(cveTipoContenedor_ Is Nothing, "", cveTipoContenedor_) & "' valor inválido"


                    End If

                End If

            End If

        End If


        Dim observacionesPedimento_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS23,
                                                   CamposPedimento.CA_OBSERVACIONES_PEDIMENTO)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_OBSERVACIONES_PEDIMENTO",
                                         New Dictionary(Of String, String) From {{"S23.CA_OBSERVACIONES_PEDIMENTO.0",
                                                                                   observacionesPedimento_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo OBSERVACIONES. '" &
                                          If(observacionesPedimento_ Is Nothing, "", observacionesPedimento_) & "' valor inválido"


                End If

            End If

        End If

        Dim numeroPEdimentoOriginal7Digitos_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS20,
                                                   CamposPedimento.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S20.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS.0",
                                                                                   numeroPEdimentoOriginal7Digitos_
                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   cvePedimento_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                    If numeroPEdimentoOriginal7Digitos_ <> "" Then

                        Dim fraccionOriginal As String = If(GetAttributeValue(pedimento_,
                                                                   SeccionesPedimento.ANS20,
                                                                   CamposPedimento.CA_FRACCION_ORIGINAL), "")

                        Dim fechaPedimentoOriginal_ = GetAttributeValue(pedimento_,
                                                                    SeccionesPedimento.ANS20,
                                                                    CamposPedimento.CA_FECHA_PEDIMENTO_ORIGINAL)

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_FECHA_PEDIMENTO_ORIGINAL_NORMAL_EXPORTACION",
                                                         New Dictionary(Of String, String) From {{"S20.CA_FECHA_PEDIMENTO_ORIGINAL.0",
                                                                                                   fechaPedimentoOriginal_
                                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                                   cvePedimento_
                                                                                                }, {"S14.CA_FECHA_PAGO.0",
                                                                                                   fechaPago_
                                                                                                }
                                                                                                })

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    mensaje_ &= Chr(13) & "Error en campo FECHA DE OPERACIÓN ORIGINAL '" &
                                                          If(fechaPedimentoOriginal_ Is Nothing, "", fechaPedimentoOriginal_) & Chr(13) &
                                                          "' relación inválida con CLAVE DE PEDIMENTO '" &
                                                          If(cvePedimento_ Is Nothing, "", cvePedimento_) &
                                                          "'y FECHA DE PAGO '" &
                                                          If(fechaPago_ Is Nothing, "", fechaPago_)




                                End If

                            End If

                        End If

                        Dim cvePedimentoOriginal_ = GetAttributeValue(pedimento_,
                                                                   SeccionesPedimento.ANS20,
                                                                   CamposPedimento.CA_CVE_PEDIMENTO_ORIGINAL)

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_CVE_PEDIMENTO_ORIGINAL_NORMAL_EXPORTACION",
                                                         New Dictionary(Of String, String) From {{"S20.CA_CVE_PEDIMENTO_ORIGINAL.0",
                                                                                                   cvePedimentoOriginal_
                                                                                                }})

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    mensaje_ &= Chr(13) & "Error en campo CVE. PEDIMENTO ORIGINAL '" &
                                                          If(cvePedimentoOriginal_ Is Nothing, "", cvePedimentoOriginal_) &
                                                          "' valor inválido"




                                End If

                            End If

                        End If


                        Dim aduanaDespachoOriginal_ = GetAttributeValue(pedimento_,
                                                                   SeccionesPedimento.ANS20,
                                                                   CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL)

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_ADUANA_DESPACHO_ORIGINAL_NORMAL_EXPORTACION",
                                                         New Dictionary(Of String, String) From {{"S20.CA_ADUANA_DESPACHO_ORIGINAL.0",
                                                                                                   aduanaDespachoOriginal_
                                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                                   cvePedimento_
                                                                                                }})

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    mensaje_ &= Chr(13) & "Error en campo ADUANA DESPACHO ORIGINALL '" &
                                                          If(aduanaDespachoOriginal_ Is Nothing, "", aduanaDespachoOriginal_) &
                                                          "' valor inválido con CLAVE DE PEDIMENTO '" & cvePedimento_ & "'"




                                End If

                            End If

                        End If

                        Dim patenteOriginal_ = GetAttributeValue(pedimento_,
                                                                   SeccionesPedimento.ANS29,
                                                                   CamposPedimento.CA_PATENTE_ORIGINAL)

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_PATENTE_ORIGINAL_NORMAL_EXPORTACION",
                                                         New Dictionary(Of String, String) From {{"S29.CA_PATENTE_ORIGINAL.0",
                                                                                                   patenteOriginal_
                                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                                   cvePedimento_
                                                                                                }})

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    mensaje_ &= Chr(13) & "Error en campo PATENTE ORIGINAL '" &
                                                          If(patenteOriginal_ Is Nothing, "", patenteOriginal_) &
                                                          "' valor inválido con CLAVE DE PEDIMENTO '" & cvePedimento_ & "'"




                                End If

                            End If

                        End If


                    End If


                Else

                    mensaje_ &= Chr(13) & "Error en campo NUMERO DE PEDIMENTO ORIGINAL 7 DIGITOS '" &
                                          If(numeroPEdimentoOriginal7Digitos_ Is Nothing, "", numeroPEdimentoOriginal7Digitos_) & "' valor inválido"


                End If

            End If

        End If



        Dim fraccionArancelariaPartida_ As String = If(GetAttributeValue(pedimento_,
                                                                   SeccionesPedimento.ANS24,
                                                                   CamposPedimento.CA_FRACCION_ARANCELARIA_PARTIDA), "")

        Dim nicoPartida_ As String = If(GetAttributeValue(pedimento_,
                                                                   SeccionesPedimento.ANS24,
                                                                   CamposPedimento.CA_NICO_PARTIDA), "")


        nicoPartida_ = If(nicoPartida_ Is Nothing, "", nicoPartida_)

        If fraccionArancelariaPartida_ = "" Then

            mensaje_ &= Chr(13) & "Error en campo FRACCIÓN ARANCELARIA '' valor inválido "

        Else


            If nicoPartida_ = "" Then

                mensaje_ &= Chr(13) & "Error en campo SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL '' valor inválido "

            Else



                validation_ = _cubedatos.
                                                          RunRoom(Of String)("A22.CA_NICO_PARTIDA",
                                                                             New Dictionary(Of String, String) From {{"S24.CA_NICO_PARTIDA.0",
                                                                                                                       nicoPartida_
                                                                                                                    }, {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                                                       fraccionArancelariaPartida_
                                                                                                                    }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            mensaje_ &= Chr(13) & "Error en campo SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL '" &
                                                                   nicoPartida_ & "'" & Chr(13) &
                                                                   " valor inválido para FRACCIÓN '" & fraccionArancelariaPartida_ & "'"




                        End If

                    End If

                End If


            End If


        End If




        Dim cveVinculacion_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS10,
                                                   CamposPedimento.CA_CVE_VINCULACION)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_VINCULACION_NORMAL_EXPORTACION",
                                        New Dictionary(Of String, String) From {{"S10.CA_CVE_VINCULACION.0",
                                                                                  cveVinculacion_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE VINCULACIÓN '" &
                                               cveVinculacion_ & "' valor inválido"




                End If

            End If

        End If

        Dim metodotoValoracionPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS10,
                                                   CamposPedimento.CA_CVE_VINCULACION)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_METODO_VALORACION_PARTIDA_NORMAL_EXPORTACION",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_METODO_VALORACION_PARTIDA.0",
                                                                                  metodotoValoracionPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo MÉTODO DE VALORACIÓN'" &
                                               metodotoValoracionPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveUMCPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CVE_UMC_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_UMC_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_UMC_PARTIDA.0",
                                                                                  cveUMCPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo UNIDAD DE MEDIDA COMERCIAL'" &
                                               cveUMCPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cantidadUMCPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CANTIDAD_UMC_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CANTIDAD_UMC_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CANTIDAD_UMC_PARTIDA.0",
                                                                                  cantidadUMCPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CANTIDAD UMC'" &
                                               cantidadUMCPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveUMTPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CVE_UMT_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_UMT_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_UMT_PARTIDA.0",
                                                                                  cveUMTPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo UNIDAD DE MEDIDA TARIFA'" &
                                               cveUMTPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cantidadUMTPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CANTIDAD_UMT_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CANTIDAD_UMT_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CANTIDAD_UMT_PARTIDA.0",
                                                                                  cantidadUMTPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CANTIDAD UMT'" &
                                               cantidadUMTPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cvePaisCompradorPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CVE_PAIS_COMPRADOR_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_PAIS_COMPRADOR_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_PAIS_COMPRADOR_PARTIDA.0",
                                                                                  cvePaisCompradorPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE PAIS COMPRADOR PARTIDA'" &
                                               cvePaisCompradorPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cvePaisDestinoPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CVE_PAIS_DESTINO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_PAIS_DESTINO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_PAIS_DESTINO_PARTIDA.0",
                                                                                  cvePaisDestinoPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE PAIS DESTINO PARTIDA'" &
                                               cvePaisDestinoPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim descripcionMercanciaPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_DESCRIPCION_MERCANCIA_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_DESCRIPCION_MERCANCIA_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_DESCRIPCION_MERCANCIA_PARTIDA.0",
                                                                                  descripcionMercanciaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo DESCRIPCIÓN MERCANCIA PARTIDA'" &
                                               descripcionMercanciaPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveContribucionPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_CVE_CONTRIBUCION_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_CONTRIBUCION_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_CVE_CONTRIBUCION_PARTIDA.0",
                                                                                  cveContribucionPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CONTRIBUCIÓN A NIVEL PARTIDA'" &
                                               cveContribucionPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveTipoTasaPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_CVE_TIPO_TASA_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_TIPO_TASA_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_CVE_TIPO_TASA_PARTIDA.0",
                                                                                  cveTipoTasaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE TIPO DE TASA PARTIDA'" &
                                               cveTipoTasaPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim tasaPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_TASA_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_TASA_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_TASA_PARTIDA.0",
                                                                                  tasaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo TASA PARTIDA'" &
                                               tasaPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim formaPagoPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_FORMA_PAGO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_FORMA_PAGO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_FORMA_PAGO_PARTIDA.0",
                                                                                  formaPagoPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo FORMA DE PAGO PARTIDA'" &
                                               formaPagoPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim valorAgregadoPartida_ As String = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_VALOR_AGREGADO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_VALOR_AGREGADO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_VALOR_AGREGADO_PARTIDA.0",
                                                                                  valorAgregadoPartida_},
                                                                                  {"S1.CA_CVE_PEDIMENTO.0",
                                                                                  cvePedimento_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else



                    mensaje_ &= Chr(13) & "Error en campo VALOR AGREGADO PARTIDA'" &
                                               valorAgregadoPartida_ & "' valor inválido " & If(valorAgregadoPartida_ <> "", " Si la CVE PEDIMENTO no es RT no se llena", "")

                End If

            End If

        End If

        Dim marcaPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_MARCA_PARTIDA)

        Dim regimen_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS1,
                                                   CamposPedimento.CA_REGIMEN)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_MARCA_PARTIDA_NORMAL_EXPORTACION",
                                        New Dictionary(Of String, String) From {{"S29.CA_MARCA_PARTIDA.0",
                                                                                  marcaPartida_},
                                                                                  {"S1.CA_REGIMEN.0",
                                                                                  regimen_
                                                                               },
                                                                                  {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                  fraccionArancelariaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo MARCA PARTIDA'" &
                                               marcaPartida_ & "' valor inválido pára la relación " & Chr(13) &
                                               "REGIMEN '" & regimen_ & "' FRACCIÓN ARANCELARIA '" & fraccionArancelariaPartida_

                End If

            End If

        End If

        Dim modeloPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_MODELO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_MODELO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_MODELO_PARTIDA.0",
                                                                                  modeloPartida_},
                                                                                  {"S24.CA_NICO_PARTIDA.0",
                                                                                  nicoPartida_
                                                                               },
                                                                                  {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                  fraccionArancelariaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If modeloPartida_ = "" Then

                        mensaje_ &= Chr(13) & "Error en campo MODELO PARTIDA'" &
                                               modeloPartida_ & "' valor inválido pára la relación " & Chr(13) &
                                               "NICO '" & nicoPartida_ & "' FRACCIÓN ARABCELARIA '" & fraccionArancelariaPartida_
                    Else

                        mensaje_ &= Chr(13) & "Error en campo MODELO PARTIDA'" &
                                               modeloPartida_ & "' valor inválido dee estar vacío pára la relación " & Chr(13) &
                                               "NICO '" & nicoPartida_ & "' FRACCIÓN ARANCELARIA '" & fraccionArancelariaPartida_
                    End If



                End If

            End If

        End If

        Dim codigoProductoPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CODIGO_PRODUCTO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CODIGO_PRODUCTO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CODIGO_PRODUCTO_PARTIDA.0",
                                                                                  codigoProductoPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CODIGO PRODUCTO PARTIDA'" &
                                               codigoProductoPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim numeroSeriePartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS25,
                                                   CamposPedimento.CA_VINCULACION_NUMERO_SERIE_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_VINCULACION_NUMERO_SERIE_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S25.CA_VINCULACION_NUMERO_SERIE_PARTIDA.0",
                                                                                  numeroSeriePartida_
                                                                               }, {"S24.CA_NICO_PARTIDA.0",
                                                                                  nicoPartida_
                                                                               },
                                                                                  {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                  fraccionArancelariaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo NUMERO SERIE PARTIDA'" &
                                               numeroSeriePartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim kilometrajePartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS25,
                                                   CamposPedimento.CA_KILOMETRAJE_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_KILOMETRAJE_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S25.CA_KILOMETRAJE_PARTIDA.0",
                                                                                  kilometrajePartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo KILOMETRAJE PARTIDA'" &
                                               kilometrajePartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cvePermiso_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS26,
                                                   CamposPedimento.CA_CVE_PERMISO)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_PERMISO",
                                        New Dictionary(Of String, String) From {{"S26.CA_CVE_PERMISO.0",
                                                                                  cvePermiso_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CVE PERMISO'" &
                                               cvePermiso_ & "' valor inválido"

                End If

            End If

        End If

        Dim numeroPermiso_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS26,
                                                   CamposPedimento.CA_NUMERO_PERMISO)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_NUMERO_PERMISO",
                                        New Dictionary(Of String, String) From {{"S26.CA_NUMERO_PERMISO.0",
                                                                                  numeroPermiso_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo NUMERO DE PERMISO'" &
                                               numeroPermiso_ & "' valor inválido"

                End If

            End If

        End If

        Dim firmaDescargo_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS26,
                                                   CamposPedimento.CA_FIRMA_ELECTRONICA_PERMISO)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_FIRMA_ELECTRONICA_PERMISO",
                                        New Dictionary(Of String, String) From {{"S26.CA_FIRMA_ELECTRONICA_PERMISO.0",
                                                                                  firmaDescargo_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo FIRMA DESCARGO'" &
                                               firmaDescargo_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveIdentificadorPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS27,
                                                   CamposPedimento.CA_CVE_IDENTIFICADOR_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_IDENTIFICADOR_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
                                                                                  cveIdentificadorPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                    Dim complemento1Partida_ = GetAttributeValue(pedimento_,
                                                               SeccionesPedimento.ANS27,
                                                               CamposPedimento.CA_COMPLEMENTO_1_PARTIDA)

                    validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_COMPLEMENTO_1_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_1_PARTIDA.0",
                                                                                              complemento1Partida_},
                                                                                              {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
                                                                                              cveIdentificadorPartida_
                                                                                           }})

                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else

                                mensaje_ &= Chr(13) & "Error en campo COMPLEMENTO 1 A NIVEL PARTIDA'" &
                                                           complemento1Partida_ & "' valor inválido"

                            End If

                        End If

                    End If

                    Dim complemento2Partida_ = GetAttributeValue(pedimento_,
                                                               SeccionesPedimento.ANS27,
                                                               CamposPedimento.CA_COMPLEMENTO_2_PARTIDA)

                    validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_COMPLEMENTO_2_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_2_PARTIDA.0",
                                                                                              complemento1Partida_},
                                                                                              {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
                                                                                              cveIdentificadorPartida_
                                                                                           }})

                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else

                                mensaje_ &= Chr(13) & "Error en campo COMPLEMENTO 2 A NIVEL PARTIDA'" &
                                                           complemento2Partida_ & "' valor inválido"

                            End If

                        End If

                    End If

                    Dim complemento3Partida_ = GetAttributeValue(pedimento_,
                                                               SeccionesPedimento.ANS27,
                                                               CamposPedimento.CA_COMPLEMENTO_3_PARTIDA)

                    validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_COMPLEMENTO_3_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_3_PARTIDA.0",
                                                                                              complemento3Partida_},
                                                                                              {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
                                                                                              cveIdentificadorPartida_
                                                                                           }})

                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else

                                mensaje_ &= Chr(13) & "Error en campo COMPLEMENTO 3 A NIVEL PARTIDA'" &
                                                           complemento3Partida_ & "' valor inválido"

                            End If

                        End If

                    End If

                Else

                    mensaje_ &= Chr(13) & "Error en campo IDENTIFICADOR A NIVEL PARTIDA'" &
                                               cveIdentificadorPartida_ & "' valor inválido"

                End If

            End If

        End If



        Dim cvePrevalidador_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS1,
                                                   CamposPedimento.CA_CVE_PREVALIDADOR)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_PREVALIDADOR",
                                        New Dictionary(Of String, String) From {{"S1.CA_CVE_PREVALIDADOR.0",
                                                                                  cvePrevalidador_}})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE DEL PREVALIDADOR'" &
                                               cvePrevalidador_ & "' valor inválido"

                End If

            End If

        End If

        Dim nombreInstitucionBancaria_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS9,
                                                   CamposPedimento.CA_NOMBRE_INSTITUCION_BANCARIA)

        Dim formaPago_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS55,
                                                   CamposPedimento.CA_FORMA_PAGO)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_NOMBRE_INSTITUCION_BANCARIA",
                                        New Dictionary(Of String, String) From {{"S9.CA_NOMBRE_INSTITUCION_BANCARIA.0",
                                                                                  nombreInstitucionBancaria_},
                                                                                  {"S55.CA_FORMA_PAGO.0",
                                                                                  formaPago_}})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo NOMBRE INSTITUCIÓN BANCARIA'" &
                                               nombreInstitucionBancaria_ & "' valor inválido"

                End If

            End If

        End If

        validation_.SetDetailReport(AdviceTypesReport.Information,
                                     mensaje_,
                                     Chr(13) & "Ruta de Validación Exportación Normal Synapsis por Defecto" & Chr(13) &
                                     "Folio de Operación:" & pedimento_.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

        Return validation_

    End Function

    Private Function ValidateRoute21(pedimento_ As DocumentoElectronico) As ValidatorReport


        Dim mensaje_ = ""


        Dim destinoOrigen_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_DESTINO_ORIGEN), "")

        Dim validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_DESTINO_ORIGEN",
                                             New Dictionary(Of String, String) From {{"S1.CA_DESTINO_ORIGEN.0",
                                                                                     destinoOrigen_
                                                                                    }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ = "Error en el campo DESTINO/ORIGEN '" & destinoOrigen_ & "' valor inválido"

                End If



            End If

        End If

        Dim aduanaES_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_DESTINO_ORIGEN), "")

        validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_ADUANA_ENTRADA_SALIDA",
                                             New Dictionary(Of String, String) From {{"S1.CA_ADUANA_ENTRADA_SALIDA.0",
                                                                                     aduanaES_
                                                                                    }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el campo ADUANA E/S '" & aduanaES_ & "' valor inválido"

                End If



            End If

        End If

        Dim medioTransporte_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE), "")

        validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_MEDIO_TRANSPORTE",
                                             New Dictionary(Of String, String) From {{"S1.CA_MEDIO_TRANSPORTE.0",
                                                                                     medioTransporte_
                                                                                    }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el campo MEDIO DE TRANSPORTE '" & medioTransporte_ & "' valor inválido"

                End If

            End If

        End If

        validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_PESO_BRUTO",
                                             New Dictionary(Of String, String) From {{"S1.CA_PESO_BRUTO.0",
                                                                                      GetAttributeValue(pedimento_,
                                                                                                        SeccionesPedimento.ANS1,
                                                                                                        CamposPedimento.CA_PESO_BRUTO)
                                                                                    }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta Especificar el valor del campo Peso Bruto"

                End If



            End If

        End If


        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_MARCAS_NUMEROS_TOTAL_BULTOS_EXPORTACION_NORMAL",
                                         New Dictionary(Of String, String) From {{"S1.CA_MARCAS_NUMEROS_TOTAL_BULTOS.0",
                                                                                    GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS1,
                                                                                                      CamposPedimento.CA_MARCAS_NUMEROS_TOTAL_BULTOS)
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo marcas, numeros y total de bultos"

                End If



            End If

        End If

        Dim medioTransporteArribo_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE_ARRIBO), "")

        validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_MEDIO_TRANSPORTE_ARRIBO",
                                             New Dictionary(Of String, String) From {{"S1.CA_MEDIO_TRANSPORTE_ARRIBO.0",
                                                                                     medioTransporteArribo_
                                                                                    }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el campo MEDIO DE TRANSPORTE DE ARRIBO'" & medioTransporteArribo_ & "' valor inválido"

                End If

            End If

        End If

        Dim medioTransporteSalida_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE_SALIDA), "")

        validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_MEDIO_TRANSPORTE_SALIDA",
                                             New Dictionary(Of String, String) From {{"S1.CA_MEDIO_TRANSPORTE_SALIDA.0",
                                                                                     medioTransporteSalida_
                                                                                    }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el campo MEDIO DE TRANSPORTE DE SALIDA '" & medioTransporteSalida_ & "' valor inválido"

                End If

            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_RFC_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_RFC_IOE.0",
                                                                                    GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS3,
                                                                                                      CamposPedimento.CA_RFC_IOE)
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo RFC DEL EXPORTADOR"

                End If



            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CURP_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_CURP_IOE.0",
                                                                                    GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS3,
                                                                                                      CamposPedimento.CA_CURP_IOE)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo CURP DEL IMPORTADOR/EXPORTADOR"

                End If



            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_RAZON_SOCIAL_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_RAZON_SOCIAL_IOE.0",
                                                                                    GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS3,
                                                                                                      CamposPedimento.CA_RAZON_SOCIAL_IOE, False)
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACIÓN SOCIAL DEL IMPORTADOR/EXPORTADOR"

                End If



            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_DOMICILIO_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_DOMICILIO_IOE.0",
                                                                                   If(GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS3,
                                                                                                      CamposPedimento.CA_DOMICILIO_IOE, False), "")
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo DOMICILIO DEL IMPORTADOR/EXPORTADOR"

                End If

            End If

        End If

        Dim tipoOperacion_ As String = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_TIPO_OPERACION), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_TIPO_OPERACION",
                                         New Dictionary(Of String, String) From {{"S1.CA_TIPO_OPERACION.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS1,
                                                                                                      CamposPedimento.CA_TIPO_OPERACION, False)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el  campo TIPO DE OPERACIÓN '" & tipoOperacion_ & " valor inválido"

                End If

            End If

        End If

        Dim cvePedimento_ = If(GetAttributeValue(pedimento_,
                                             SeccionesPedimento.ANS1,
                                             CamposPedimento.CA_CVE_PEDIMENTO, False).SubString(0, 2), "")

        validation_ = _cubedatos.
              RunRoom(Of String)("A22.CA_CVE_PEDIMENTO",
                                 New Dictionary(Of String, String) From {{"S1.CA_CVE_PEDIMENTO.0",
                                                                           cvePedimento_
                                                                         }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el  campo CVE PEDIMENTO '" & cvePedimento_ & " valor inválido"

                End If

            End If

        End If



        Dim cveRegimen_ = If(GetAttributeValue(pedimento_,
                                             SeccionesPedimento.ANS1,
                                             CamposPedimento.CA_REGIMEN, False).SubString(0, 3), "")
        validation_ = _cubedatos.
              RunRoom(Of String)("A22.CA_REGIMEN",
                                 New Dictionary(Of String, String) From {{"S1.CA_REGIMEN.0",
                                                                           cveRegimen_
                                                                         }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el  campo REGIMEN '" & cveRegimen_ & " valor inválido"

                End If

            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC",
                                         New Dictionary(Of String, String) From {{"S10.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS10,
                                                                                                      CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC, False)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACION O RAZON SOCIAL. DEL COMPRADOR"

                End If



            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CFDI_FACTURA",
                                         New Dictionary(Of String, String) From {{"S10.CA_CFDI_FACTURA.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS10,
                                                                                                      CamposPedimento.CA_CFDI_FACTURA)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NUM. CFDI O DOCUMENTO EQUIVALENTE."

                End If

            End If

        End If

        Dim facturaDate_ = GetAttributeValue(pedimento_,
                                            SeccionesPedimento.ANS13,
                                            CamposPedimento.CA_FECHA_FACTURA)

        Dim validationDateValue_ = GetAttributeValue(pedimento_,
                                                    SeccionesPedimento.ANS13,
                                                    CamposPedimento.CA_FECHA_FACTURA)
        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_FACTURA",
                                         New Dictionary(Of String, String) From {{"S13.CA_FECHA_FACTURA.0",
                                                                                   facturaDate_
                                                                                 },
                                                                                 {"S1.CA_FECHA_VALIDACION.0",
                                                                                   validationDateValue_
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el campo FECHA DE FACTURA '" &
                                          If(facturaDate_ Is Nothing, "", facturaDate_) &
                                          "' debe ser menor o igual a la FECHA DE VALIDACIÓN '" &
                                          If(validationDateValue_ Is Nothing, "", validationDateValue_) & "'"

                End If



            End If

        End If

        Dim cveMonedaFactura = GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS13,
                                                                                                      CamposPedimento.CA_CVE_MONEDA_FACTURA)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CVE_MONEDA_FACTURA",
                                         New Dictionary(Of String, String) From {{"S13.CA_CVE_MONEDA_FACTURA.0",
                                                                                   cveMonedaFactura
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Valor inválido para campo MONEDA FACT. ('" & cveMonedaFactura & "')"

                End If



            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_MONTO_MONEDA_FACTURA",
                                         New Dictionary(Of String, String) From {{"S13.CA_MONTO_MONEDA_FACTURA.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS13,
                                                                                                      CamposPedimento.CA_MONTO_MONEDA_FACTURA)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo VAL. MON. FACT."

                End If



            End If

        End If



        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_ACUSE_ELECTRONICO_VALIDACION_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S1.CA_ACUSE_ELECTRONICO_VALIDACION.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS1,
                                                                                                      CamposPedimento.CA_ACUSE_ELECTRONICO_VALIDACION)
                                                                                 },
                                                                                 {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   cvePedimento_
                                                                                 },
                                                                                 {"SFAC1.CP_APLICA_ENAJENACION.0",
                                                                                  "SI"
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NÚMERO DE ACUSE DE VALOR"

                End If

            End If

        End If


        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_INCOTERM",
                                         New Dictionary(Of String, String) From {{"S13.CA_INCOTERM.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS13,
                                                                                                      CamposPedimento.CA_INCOTERM)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo INCOTERM."

                End If

            End If

        End If

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO",
                                         New Dictionary(Of String, String) From {{"S11.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS11,
                                                                                                      CamposPedimento.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO, False)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACION O RAZON SOCIAL. DEL DESTINATARIO"

                End If

            End If

        End If


        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA",
                                         New Dictionary(Of String, String) From {{"S44.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS44,
                                                                                                      CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, False)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACIÓN O RAZ. SOC.. DEL AGENTE ADUANAL"

                End If

            End If

        End If

        Dim fechaPago_ = GetAttributeValue(pedimento_,
                                           SeccionesPedimento.ANS14,
                                           CamposPedimento.CA_FECHA_PAGO, False)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_PAGO",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_PAGO.0",
                                                                                   fechaPago_
                                                                                 },
                                                                                 {"S1.CA_FECHA_VALIDACION.0",
                                                                                   validationDateValue_
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If fechaPago_ IsNot Nothing Then

                        mensaje_ &= Chr(13) & "Error en el valor del campo FECHA PAGO. '" &
                                              fechaPago_ & "'" & Chr(13) &
                                              " debe ser igual a la fecha de validación '" &
                                              validationDateValue_ & "'"

                    Else

                        mensaje_ &= Chr(13) & "Error en el valor del campo FECHA PAGO. ''"


                    End If


                End If

            End If

        End If

        'AQUI SE HARÁ UN CICLO PARA REVISAR TODA LA SECCIÓN YA QUE PUEDE HABER VARIOS IDENTIFICADOR A NIVEL PEDIMENTO

        Dim cveIdentificador_ As String = GetAttributeValue(pedimento_,
                                                  SeccionesPedimento.ANS18,
                                                  CamposPedimento.CA_CVE_IDENTIFICADOR)

        cveIdentificador_ = If(cveIdentificador_ Is Nothing, "", cveIdentificador_)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CVE_IDENTIFICADOR",
                                         New Dictionary(Of String, String) From {{"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                  cveIdentificador_
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                    Dim complemento1_ As String = If(GetAttributeValue(pedimento_,
                                                          SeccionesPedimento.ANS18,
                                                          CamposPedimento.CA_COMPLEMENTO_1), "")


                    validation_ = _cubedatos.
                                  RunRoom(Of String)("A22.CA_COMPLEMENTO_1",
                                                     New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_1.0",
                                                                                               complemento1_
                                                                                             },
                                                                                              {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                              cveIdentificador_
                                                                                             }})
                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else

                                If complemento1_ = "" Then


                                    mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 1 para el identificador '" &
                                                               cveIdentificador_ &
                                                               "' el complemento no debe estar vacío"


                                Else

                                    mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 1 valor inválido '" &
                                                           complemento1_ &
                                                           "' para el identificador '" &
                                                           cveIdentificador_ &
                                                           "' el complemento debe estar vacío"

                                End If


                            End If

                        End If

                    End If

                    Dim complemento2_ = If(GetAttributeValue(pedimento_,
                                                          SeccionesPedimento.ANS18,
                                                          CamposPedimento.CA_COMPLEMENTO_2), "")

                    validation_ = _cubedatos.
                                  RunRoom(Of String)("A22.CA_COMPLEMENTO_2",
                                                     New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_2.0",
                                                                                              complemento2_
                                                                                             },
                                                                                              {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                              cveIdentificador_
                                                                                             }})
                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else

                                If complemento2_ = "" Then

                                    mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 2 para el identificador '" &
                                                           cveIdentificador_ &
                                                           "' el complemento no debe estar vacío"

                                Else

                                    mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 2 valor inválido '" &
                                                               complemento2_ &
                                                               "' para el identificador '" &
                                                               cveIdentificador_ &
                                                               "' el complemento debe estar vacío"

                                End If

                            End If

                        End If

                    End If

                    Dim complemento3_ = If(GetAttributeValue(pedimento_,
                                                          SeccionesPedimento.ANS18,
                                                          CamposPedimento.CA_COMPLEMENTO_3), "")

                    validation_ = _cubedatos.
                                  RunRoom(Of String)("A22.CA_COMPLEMENTO_3",
                                                     New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_3.0",
                                                                                               complemento3_
                                                                                             },
                                                                                              {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                              cveIdentificador_
                                                                                             }})
                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else


                                If complemento3_ = "" Then

                                    mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 3 para el identificador '" &
                                                           cveIdentificador_ &
                                                           "' el complemento no debe estar vacío"

                                Else

                                    mensaje_ &= Chr(13) & "Error en el campo COMPLEMENTO 3 valor inválido '" &
                                                           complemento3_ &
                                                           "' para el identificador '" &
                                                           cveIdentificador_ & "' el complemento debe estar vacío"

                                End If


                            End If

                        End If

                    End If

                Else


                    mensaje_ &= Chr(13) & "Error en el campo CLAVE. IDENTIFICADOR. Valor inválido '" &
                                               cveIdentificador_ & "'"

                End If

            End If

        End If



        Dim fechapresentacion_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS14,
                                                   CamposPedimento.CA_FECHA_PRESENTACION)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_PRESENTACION",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_PRESENTACION.0",
                                                                                   GetAttributeValue(pedimento_,
                                                                                                      SeccionesPedimento.ANS14,
                                                                                                      CamposPedimento.CA_FECHA_PRESENTACION)
                                                                                 },
                                                                                  {"S14.CA_FECHA_PAGO.0",
                                                                                  fechaPago_
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo FECHA PRESENTACION '" & fechapresentacion_ & "'" & Chr(13) & "('La fecha de presentación debe ser igual a la fecha de pago')"

                End If

            End If

        End If

        Dim fechaExtraccion_ = GetAttributeValue(pedimento_,
                                                SeccionesPedimento.ANS14,
                                                CamposPedimento.CA_FECHA_PRESENTACION)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_EXTRACCION",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_EXTRACCION.0",
                                                                                  fechaExtraccion_
                                                                                 },
                                                                                  {"S14.CA_FECHA_PAGO.0",
                                                                                  fechaPago_
                                                                                 },
                                                                                  {"S1.CA_CVE_PEDIMENTO.0",
                                                                                  cvePedimento_
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If fechaExtraccion_ > fechaPago_ Then

                        mensaje_ &= Chr(13) & "Error en el valor del campo FECHA EXTRACCIÓN '" &
                                              If(fechaExtraccion_ Is Nothing, "", fechaExtraccion_) &
                                             "'" & Chr(13) & "(' debe ser menor o igual a la fecha de PAGO '" & fechaPago_ & "')"

                    Else

                        mensaje_ &= Chr(13) & "Error en el valor del campo FECHA EXTRACCIÓN '" &
                                             If(fechaExtraccion_ Is Nothing, "", fechaExtraccion_) &
                                             "'" & Chr(13) & "('La fecha de EXTRACCIÓN no es compatible con la clave de pedimento puesta '" &
                                             If(cvePedimento_ Is Nothing, "", cvePedimento_) & "')"

                    End If

                End If

            End If

        End If

        Dim idTransporte_ = GetAttributeValue(pedimento_,
                                              SeccionesPedimento.ANS12,
                                              CamposPedimento.CA_ID_TRANSPORTE)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_ID_TRANSPORTE",
                                         New Dictionary(Of String, String) From {{"S12.CA_ID_TRANSPORTE.0",
                                                                                   idTransporte_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo CA_ID_TRANSPORTE '" &
                                           If(idTransporte_ Is Nothing, "", idTransporte_) & "')"

                End If

            End If

        End If

        Dim cvePaisTransporte_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS12,
                                                   CamposPedimento.CA_CVE_PAIS_TRANSPORTE)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CVE_PAIS_TRANSPORTE",
                                         New Dictionary(Of String, String) From {{"S12.CA_CVE_PAIS_TRANSPORTE.0",
                                                                                   cvePaisTransporte_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo CLAVE DEL PAIS DE TRANSPORTE '" &
                                          If(cvePaisTransporte_ Is Nothing, "", cvePaisTransporte_) & "')"

                End If

            End If

        End If

        Dim numeroCandado_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS15,
                                                   CamposPedimento.CA_NUMERO_CANDADO)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NUMERO_CANDADO",
                                         New Dictionary(Of String, String) From {{"S15.CA_NUMERO_CANDADO.0",
                                                                                   numeroCandado_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo NÚMERO DE CANDADO ''"

                End If

            End If

        End If

        Dim guiaManifiestoBL_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS16,
                                                   CamposPedimento.CA_GUIA_MANIFIESTO_BL)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_GUIA_MANIFIESTO_BL_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S16.CA_GUIA_MANIFIESTO_BL.0",
                                                                                   guiaManifiestoBL_
                                                                                },
                                                                                {"S1.CA_MEDIO_TRANSPORTE.0",
                                                                                   medioTransporte_
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If If(guiaManifiestoBL_ Is Nothing, "", guiaManifiestoBL_) = "" Then

                        mensaje_ &= Chr(13) & "Error en el valor del campo NUMERO (GUIA/CONOCIMIENTO EMBARQUE) '' " & Chr(13) &
                                              " no debe ir vacio para el medio de transporte '" &
                                              If(medioTransporte_ Is Nothing, "", medioTransporte_) & "'"

                    Else


                        mensaje_ &= Chr(13) & "Error en el valor del campo NUMERO (GUIA/CONOCIMIENTO EMBARQUE) '" & guiaManifiestoBL_ & "'" & Chr(13) &
                                              "debe ir vacio para el medio de transporte '" &
                                              If(medioTransporte_ Is Nothing, "", medioTransporte_) & "'"


                    End If


                End If

            End If

        End If

        Dim masterHouse_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS16,
                                                   CamposPedimento.CA_MASTER_HOUSE)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_MASTER_HOUSE",
                                         New Dictionary(Of String, String) From {{"S16.CA_MASTER_HOUSE.0",
                                                                                   masterHouse_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en el valor del campo MASTER HOUSE '" &
                                          If(masterHouse_ Is Nothing, "", masterHouse_) & "'"

                End If

            End If

        End If



        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO",
                                         New Dictionary(Of String, String) From {{"S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO.0",
                                                                                   GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS17,
                                                   CamposPedimento.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO)
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo NUMERO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO. 'está vacío'"


                End If

            End If

        End If

        Dim cveTipoContenedor_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS17,
                                                   CamposPedimento.CA_CVE_TIPO_CONTENEDOR)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CVE_TIPO_CONTENEDOR",
                                         New Dictionary(Of String, String) From {{"S17.CA_CVE_TIPO_CONTENEDOR.0",
                                                                                   cveTipoContenedor_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo TIPO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO. '" &
                                          If(cveTipoContenedor_ Is Nothing, "", cveTipoContenedor_) & "' valor inválido"


                End If

            End If

        End If

        Dim observacionesPedimento_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS23,
                                                   CamposPedimento.CA_OBSERVACIONES_PEDIMENTO)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_OBSERVACIONES_PEDIMENTO",
                                         New Dictionary(Of String, String) From {{"S23.CA_OBSERVACIONES_PEDIMENTO.0",
                                                                                   observacionesPedimento_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo OBSERVACIONES. '" &
                                          If(observacionesPedimento_ Is Nothing, "", observacionesPedimento_) & "' valor inválido"


                End If

            End If

        End If

        Dim numeroPEdimentoOriginal7Digitos_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS20,
                                                   CamposPedimento.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S20.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS.0",
                                                                                   numeroPEdimentoOriginal7Digitos_
                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   cvePedimento_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo NUMERO DE PEDIMENTO ORIGINAL 7 DIGITOS '" &
                                          If(numeroPEdimentoOriginal7Digitos_ Is Nothing, "", numeroPEdimentoOriginal7Digitos_) & "' valor inválido"


                End If

            End If

        End If

        Dim fechaPedimentoOriginal_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS20,
                                                   CamposPedimento.CA_FECHA_PEDIMENTO_ORIGINAL)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_PEDIMENTO_ORIGINAL_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S20.CA_FECHA_PEDIMENTO_ORIGINAL.0",
                                                                                   fechaPedimentoOriginal_
                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   cvePedimento_
                                                                                }, {"S14.CA_FECHA_PAGO.0",
                                                                                   fechaPago_
                                                                                }
                                                                                })

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo FECHA DE OPERACIÓN ORIGINAL '" &
                                          If(fechaPedimentoOriginal_ Is Nothing, "", fechaPedimentoOriginal_) & Chr(13) &
                                          "' relación inválida con CLAVE DE PEDIMENTO '" &
                                          If(cvePedimento_ Is Nothing, "", cvePedimento_) &
                                          "'y FECHA DE PAGO '" &
                                          If(fechaPago_ Is Nothing, "", fechaPago_)




                End If

            End If

        End If

        Dim cvePedimentoOriginal_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS20,
                                                   CamposPedimento.CA_CVE_PEDIMENTO_ORIGINAL)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CVE_PEDIMENTO_ORIGINAL_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S20.CA_CVE_PEDIMENTO_ORIGINAL.0",
                                                                                   cvePedimentoOriginal_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CVE. PEDIMENTO ORIGINAL '" &
                                          If(cvePedimentoOriginal_ Is Nothing, "", cvePedimentoOriginal_) &
                                          "' valor inválido"




                End If

            End If

        End If


        Dim aduanaDespachoOriginal_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS20,
                                                   CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_ADUANA_DESPACHO_ORIGINAL_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S20.CA_ADUANA_DESPACHO_ORIGINAL.0",
                                                                                   aduanaDespachoOriginal_
                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   cvePedimento_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo ADUANA DESPACHO ORIGINALL '" &
                                          If(aduanaDespachoOriginal_ Is Nothing, "", aduanaDespachoOriginal_) &
                                          "' valor inválido con CLAVE DE PEDIMENTO '" & cvePedimento_ & "'"




                End If

            End If

        End If

        Dim patenteOriginal_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_PATENTE_ORIGINAL)

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_PATENTE_ORIGINAL_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S29.CA_PATENTE_ORIGINAL.0",
                                                                                   patenteOriginal_
                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   cvePedimento_
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo PATENTE ORIGINAL '" &
                                          If(patenteOriginal_ Is Nothing, "", patenteOriginal_) &
                                          "' valor inválido con CLAVE DE PEDIMENTO '" & cvePedimento_ & "'"




                End If

            End If

        End If

        Dim fraccionOriginal_ As String = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS20,
                                                   CamposPedimento.CA_FRACCION_ORIGINAL)

        fraccionOriginal_ = If(fraccionOriginal_ Is Nothing, "", fraccionOriginal_.Replace(".", ""))

        Dim nicoPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_NICO_PARTIDA)


        nicoPartida_ = If(nicoPartida_ Is Nothing, "", nicoPartida_)

        If fraccionOriginal_ = "" Then

            mensaje_ &= Chr(13) & "Error en campo FRACCIÓN ORIGINAL '' valor inválido "

        Else

            validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_FRACCION_ORIGINAL",
                                             New Dictionary(Of String, String) From {{"S20.CA_FRACCION_ORIGINAL.0",
                                                                                       fraccionOriginal_
                                                                                    }})

            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then

                        If If(nicoPartida_ Is Nothing, "", nicoPartida_) = "" Then

                            mensaje_ &= Chr(13) & "Error en campo SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL '' valor inválido "

                        Else



                            validation_ = _cubedatos.
                                          RunRoom(Of String)("A22.CA_NICO_PARTIDA",
                                                             New Dictionary(Of String, String) From {{"S24.CA_NICO_PARTIDA.0",
                                                                                                       nicoPartida_
                                                                                                    }, {"S20.CA_FRACCION_ORIGINAL.0",
                                                                                                       fraccionOriginal_
                                                                                                    }})

                            If validation_.result IsNot Nothing Then

                                If validation_.result.Count > 0 Then

                                    If validation_.result(0) = "OK" Then

                                    Else

                                        mensaje_ &= Chr(13) & "Error en campo SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL '" &
                                                               nicoPartida_ & "'" & Chr(13) &
                                                               " valor inválido para FRACCIÓN '" & fraccionOriginal_ & "'"




                                    End If

                                End If

                            End If

                        End If

                    Else

                        mensaje_ &= Chr(13) & "Error en campo FRACCIÓN ORIGINAL '" &
                                               fraccionOriginal_ &
                                              "' valor inválido "




                    End If

                End If

            End If

        End If





        Dim cveVinculacion_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS10,
                                                   CamposPedimento.CA_CVE_VINCULACION)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_VINCULACION_NORMAL_EXPORTACION",
                                        New Dictionary(Of String, String) From {{"S10.CA_CVE_VINCULACION.0",
                                                                                  cveVinculacion_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE VINCULACIÓN '" &
                                               cveVinculacion_ & "' valor inválido"




                End If

            End If

        End If

        Dim metodotoValoracionPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS10,
                                                   CamposPedimento.CA_CVE_VINCULACION)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_METODO_VALORACION_PARTIDA_NORMAL_EXPORTACION",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_METODO_VALORACION_PARTIDA.0",
                                                                                  metodotoValoracionPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo MÉTODO DE VALORACIÓN'" &
                                               metodotoValoracionPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveUMCPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CVE_UMC_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_UMC_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_UMC_PARTIDA.0",
                                                                                  cveUMCPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo UNIDAD DE MEDIDA COMERCIAL'" &
                                               cveUMCPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cantidadUMCPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CANTIDAD_UMC_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CANTIDAD_UMC_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CANTIDAD_UMC_PARTIDA.0",
                                                                                  cantidadUMCPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CANTIDAD UMC'" &
                                               cantidadUMCPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveUMTPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CVE_UMT_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_UMT_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_UMT_PARTIDA.0",
                                                                                  cveUMTPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo UNIDAD DE MEDIDA TARIFA'" &
                                               cveUMTPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cantidadUMTPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CANTIDAD_UMT_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CANTIDAD_UMT_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CANTIDAD_UMT_PARTIDA.0",
                                                                                  cantidadUMTPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CANTIDAD UMT'" &
                                               cantidadUMTPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cvePaisCompradorPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CVE_PAIS_COMPRADOR_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_PAIS_COMPRADOR_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_PAIS_COMPRADOR_PARTIDA.0",
                                                                                  cvePaisCompradorPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE PAIS COMPRADOR PARTIDA'" &
                                               cvePaisCompradorPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cvePaisDestinoPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CVE_PAIS_DESTINO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_PAIS_DESTINO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_PAIS_DESTINO_PARTIDA.0",
                                                                                  cvePaisDestinoPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE PAIS DESTINO PARTIDA'" &
                                               cvePaisDestinoPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim descripcionMercanciaPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_DESCRIPCION_MERCANCIA_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_DESCRIPCION_MERCANCIA_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_DESCRIPCION_MERCANCIA_PARTIDA.0",
                                                                                  descripcionMercanciaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo DESCRIPCIÓN MERCANCIA PARTIDA'" &
                                               descripcionMercanciaPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveContribucionPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_CVE_CONTRIBUCION_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_CONTRIBUCION_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_CVE_CONTRIBUCION_PARTIDA.0",
                                                                                  cveContribucionPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CONTRIBUCIÓN A NIVEL PARTIDA'" &
                                               cveContribucionPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveTipoTasaPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_CVE_TIPO_TASA_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_TIPO_TASA_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_CVE_TIPO_TASA_PARTIDA.0",
                                                                                  cveTipoTasaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE TIPO DE TASA PARTIDA'" &
                                               cveTipoTasaPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim tasaPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_TASA_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_TASA_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_TASA_PARTIDA.0",
                                                                                  tasaPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo TASA PARTIDA'" &
                                               tasaPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim formaPagoPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_FORMA_PAGO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_FORMA_PAGO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_FORMA_PAGO_PARTIDA.0",
                                                                                  formaPagoPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo FORMA DE PAGO PARTIDA'" &
                                               formaPagoPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim valorAgregadoPartida_ As String = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_VALOR_AGREGADO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_VALOR_AGREGADO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_VALOR_AGREGADO_PARTIDA.0",
                                                                                  valorAgregadoPartida_},
                                                                                  {"S1.CA_CVE_PEDIMENTO.0",
                                                                                  cvePedimento_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else



                    mensaje_ &= Chr(13) & "Error en campo VALOR AGREGADO PARTIDA'" &
                                               valorAgregadoPartida_ & "' valor inválido " & If(valorAgregadoPartida_ <> "", " Si la CVE PEDIMENTO no es RT no se llena", "")

                End If

            End If

        End If

        Dim marcaPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_MARCA_PARTIDA)

        Dim regimen_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS1,
                                                   CamposPedimento.CA_REGIMEN)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_MARCA_PARTIDA_NORMAL_EXPORTACION",
                                        New Dictionary(Of String, String) From {{"S29.CA_MARCA_PARTIDA.0",
                                                                                  marcaPartida_},
                                                                                  {"S1.CA_REGIMEN.0",
                                                                                  regimen_
                                                                               },
                                                                                  {"S20.CA_FRACCION_ORIGINAL.0",
                                                                                  fraccionOriginal_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo MARCA PARTIDA'" &
                                               marcaPartida_ & "' valor inválido pára la relación " & Chr(13) &
                                               "REGIMEN '" & regimen_ & "' FRACCIÓN ORIGINAL '" & fraccionOriginal_

                End If

            End If

        End If

        Dim modeloPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS29,
                                                   CamposPedimento.CA_MODELO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_MODELO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S29.CA_MODELO_PARTIDA.0",
                                                                                  modeloPartida_},
                                                                                  {"S24.CA_NICO_PARTIDA.0",
                                                                                  nicoPartida_
                                                                               },
                                                                                  {"S20.CA_FRACCION_ORIGINAL.0",
                                                                                  fraccionOriginal_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If modeloPartida_ = "" Then

                        mensaje_ &= Chr(13) & "Error en campo MODELO PARTIDA'" &
                                               modeloPartida_ & "' valor inválido pára la relación " & Chr(13) &
                                               "NICO '" & nicoPartida_ & "' FRACCIÓN ORIGINAL '" & fraccionOriginal_
                    Else

                        mensaje_ &= Chr(13) & "Error en campo MODELO PARTIDA'" &
                                               modeloPartida_ & "' valor inválido dee estar vacío pára la relación " & Chr(13) &
                                               "NICO '" & nicoPartida_ & "' FRACCIÓN ORIGINAL '" & fraccionOriginal_
                    End If



                End If

            End If

        End If

        Dim codigoProductoPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS24,
                                                   CamposPedimento.CA_CODIGO_PRODUCTO_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CODIGO_PRODUCTO_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S24.CA_CODIGO_PRODUCTO_PARTIDA.0",
                                                                                  codigoProductoPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CODIGO PRODUCTO PARTIDA'" &
                                               codigoProductoPartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim numeroSeriePartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS25,
                                                   CamposPedimento.CA_VINCULACION_NUMERO_SERIE_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_VINCULACION_NUMERO_SERIE_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S25.CA_VINCULACION_NUMERO_SERIE_PARTIDA.0",
                                                                                  numeroSeriePartida_
                                                                               }, {"S24.CA_NICO_PARTIDA.0",
                                                                                  nicoPartida_
                                                                               },
                                                                                  {"S20.CA_FRACCION_ORIGINAL.0",
                                                                                  fraccionOriginal_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo NUMERO SERIE PARTIDA'" &
                                               numeroSeriePartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim kilometrajePartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS25,
                                                   CamposPedimento.CA_KILOMETRAJE_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_KILOMETRAJE_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S25.CA_KILOMETRAJE_PARTIDA.0",
                                                                                  kilometrajePartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo KILOMETRAJE PARTIDA'" &
                                               kilometrajePartida_ & "' valor inválido"

                End If

            End If

        End If

        Dim cvePermiso_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS26,
                                                   CamposPedimento.CA_CVE_PERMISO)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_PERMISO",
                                        New Dictionary(Of String, String) From {{"S26.CA_CVE_PERMISO.0",
                                                                                  cvePermiso_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CVE PERMISO'" &
                                               cvePermiso_ & "' valor inválido"

                End If

            End If

        End If

        Dim numeroPermiso_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS26,
                                                   CamposPedimento.CA_NUMERO_PERMISO)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_NUMERO_PERMISO",
                                        New Dictionary(Of String, String) From {{"S26.CA_NUMERO_PERMISO.0",
                                                                                  numeroPermiso_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo NUMERO DE PERMISO'" &
                                               numeroPermiso_ & "' valor inválido"

                End If

            End If

        End If

        Dim firmaDescargo_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS26,
                                                   CamposPedimento.CA_FIRMA_ELECTRONICA_PERMISO)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_FIRMA_ELECTRONICA_PERMISO",
                                        New Dictionary(Of String, String) From {{"S26.CA_FIRMA_ELECTRONICA_PERMISO.0",
                                                                                  firmaDescargo_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo FIRMA DESCARGO'" &
                                               firmaDescargo_ & "' valor inválido"

                End If

            End If

        End If

        Dim cveIdentificadorPartida_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS27,
                                                   CamposPedimento.CA_CVE_IDENTIFICADOR_PARTIDA)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_IDENTIFICADOR_PARTIDA",
                                        New Dictionary(Of String, String) From {{"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
                                                                                  cveIdentificadorPartida_
                                                                               }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                    Dim complemento1Partida_ = GetAttributeValue(pedimento_,
                                                               SeccionesPedimento.ANS27,
                                                               CamposPedimento.CA_COMPLEMENTO_1_PARTIDA)

                    validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_COMPLEMENTO_1_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_1_PARTIDA.0",
                                                                                              complemento1Partida_},
                                                                                              {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
                                                                                              cveIdentificadorPartida_
                                                                                           }})

                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else

                                mensaje_ &= Chr(13) & "Error en campo COMPLEMENTO 1 A NIVEL PARTIDA'" &
                                                           complemento1Partida_ & "' valor inválido"

                            End If

                        End If

                    End If

                    Dim complemento2Partida_ = GetAttributeValue(pedimento_,
                                                               SeccionesPedimento.ANS27,
                                                               CamposPedimento.CA_COMPLEMENTO_2_PARTIDA)

                    validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_COMPLEMENTO_2_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_2_PARTIDA.0",
                                                                                              complemento1Partida_},
                                                                                              {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
                                                                                              cveIdentificadorPartida_
                                                                                           }})

                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else

                                mensaje_ &= Chr(13) & "Error en campo COMPLEMENTO 2 A NIVEL PARTIDA'" &
                                                           complemento2Partida_ & "' valor inválido"

                            End If

                        End If

                    End If

                    Dim complemento3Partida_ = GetAttributeValue(pedimento_,
                                                               SeccionesPedimento.ANS27,
                                                               CamposPedimento.CA_COMPLEMENTO_3_PARTIDA)

                    validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_COMPLEMENTO_3_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_3_PARTIDA.0",
                                                                                              complemento3Partida_},
                                                                                              {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
                                                                                              cveIdentificadorPartida_
                                                                                           }})

                    If validation_.result IsNot Nothing Then

                        If validation_.result.Count > 0 Then

                            If validation_.result(0) = "OK" Then

                            Else

                                mensaje_ &= Chr(13) & "Error en campo COMPLEMENTO 3 A NIVEL PARTIDA'" &
                                                           complemento3Partida_ & "' valor inválido"

                            End If

                        End If

                    End If

                Else

                    mensaje_ &= Chr(13) & "Error en campo IDENTIFICADOR A NIVEL PARTIDA'" &
                                               cveIdentificadorPartida_ & "' valor inválido"

                End If

            End If

        End If



        Dim cvePrevalidador_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS1,
                                                   CamposPedimento.CA_CVE_PREVALIDADOR)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_CVE_PREVALIDADOR",
                                        New Dictionary(Of String, String) From {{"S1.CA_CVE_PREVALIDADOR.0",
                                                                                  cvePrevalidador_}})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo CLAVE DEL PREVALIDADOR'" &
                                               cvePrevalidador_ & "' valor inválido"

                End If

            End If

        End If

        Dim nombreInstitucionBancaria_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS9,
                                                   CamposPedimento.CA_NOMBRE_INSTITUCION_BANCARIA)

        Dim formaPago_ = GetAttributeValue(pedimento_,
                                                   SeccionesPedimento.ANS55,
                                                   CamposPedimento.CA_FORMA_PAGO)

        validation_ = _cubedatos.
                     RunRoom(Of String)("A22.CA_NOMBRE_INSTITUCION_BANCARIA",
                                        New Dictionary(Of String, String) From {{"S9.CA_NOMBRE_INSTITUCION_BANCARIA.0",
                                                                                  nombreInstitucionBancaria_},
                                                                                  {"S55.CA_FORMA_PAGO.0",
                                                                                  formaPago_}})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    mensaje_ &= Chr(13) & "Error en campo NOMBRE INSTITUCIÓN BANCARIA'" &
                                               nombreInstitucionBancaria_ & "' valor inválido"

                End If

            End If

        End If

        validation_.SetDetailReport(AdviceTypesReport.Information,
                                     mensaje_,
                                     Chr(13) & "Ruta de Validación Exportación Normal por Defecto" & Chr(13) &
                                     "Folio de Operación:" & pedimento_.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

        Return validation_

    End Function

    Function GetAttributeValue(pedimento_ As DocumentoElectronico, section_ As SeccionesPedimento, field_ As CamposPedimento, Optional valor_ As Boolean = True) As Object

        Dim attribute_ = pedimento_.Seccion(section_).Attribute(field_)

        If field_ = CamposPedimento.CA_FRACCION_ORIGINAL Then

            Dim algo = attribute_

        End If

        If attribute_ Is Nothing Then

            Return Nothing

        Else

            If valor_ Then


                If attribute_.Valor Is Nothing Then

                    Return ""

                Else

                    Return attribute_.Valor.ToString

                End If

            Else

                If attribute_.ValorPresentacion Is Nothing Then

                    If attribute_.Valor Is Nothing Then

                        Return ""

                    Else

                        Return attribute_.Valor.ToString

                    End If


                Else

                    Return attribute_.ValorPresentacion.ToString

                End If



            End If

        End If


    End Function


    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

#End Region

End Class

