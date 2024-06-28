

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

    '   Private _validationpanel As validationpanel

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
            Return _cubedatos

        End Get

        Set(value As ICubeController)

            _cubedatos = cubedatos

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

        Dim elementMessage_ As New Dictionary(Of String, String)


        Dim message_ = ""

        Dim field_ As String = "S" &
                               SeccionesPedimento.ANS1 &
                               "." &
                               CamposPedimento.CA_DESTINO_ORIGEN.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_DESTINO_ORIGEN), "")

        If elementMessage_(field_) = "" Then

            message_ = "Falta Especificar el valor del campo DESTINO/ORIGEN"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_ADUANA_ENTRADA_SALIDA.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_ADUANA_ENTRADA_SALIDA), "")


        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta Especificar el valor del campo ADUANA E/S"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_MEDIO_TRANSPORTE.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE), "")

        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta Especificar el valor del campo MEDIO DE TRANSPORTE"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_PESO_BRUTO.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                            SeccionesPedimento.ANS1,
                            CamposPedimento.CA_PESO_BRUTO), "")

        Dim validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_PESO_BRUTO",
                                             New Dictionary(Of String, String) From {{"S1.CA_PESO_BRUTO.0",
                                                                                       elementMessage_(field_)
                                                                                    }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages Is Nothing Then

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                    Else

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$" & field_, elementMessage_(field_))

                        Else

                            message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                        End If

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_MARCAS_NUMEROS_TOTAL_BULTOS.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                            SeccionesPedimento.ANS1,
                            CamposPedimento.CA_MARCAS_NUMEROS_TOTAL_BULTOS), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_MARCAS_NUMEROS_TOTAL_BULTOS_EXPORTACION_NORMAL",
                                         New Dictionary(Of String, String) From {{"S1.CA_MARCAS_NUMEROS_TOTAL_BULTOS.0",
                                                                                    elementMessage_(field_)
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages Is Nothing Then

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                    Else

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$" & field_, elementMessage_(field_))

                        Else


                            message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                        End If

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_MARCAS_NUMEROS_TOTAL_BULTOS.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE_ARRIBO), "")

        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta Especificar el valor del campo MEDIO DE TRANSPORTE DE ARRIBO"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_MEDIO_TRANSPORTE_SALIDA.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE_SALIDA), "")

        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta Especificar el valor del campo MEDIO DE TRANSPORTE DE ARRIBO"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_MEDIO_TRANSPORTE_SALIDA.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_MEDIO_TRANSPORTE_SALIDA), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_RFC_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_RFC_IOE.0",
                                                                                    elementMessage_(field_)
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages Is Nothing Then

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                    Else

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$" & field_, elementMessage_(field_))

                        Else


                            message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                        End If

                    End If

                End If

            End If

        End If



        Dim moral_ = True

        If moral_ Then

        Else

            field_ = "S" &
                 SeccionesPedimento.ANS3 &
                 "." &
                 CamposPedimento.CA_CURP_IOE.ToString

            elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS3,
                                                         CamposPedimento.CA_CURP_IOE), "")

            validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_CURP_IOE",
                                             New Dictionary(Of String, String) From {{"S3.CA_CURP_IOE.0",
                                                                                       elementMessage_(field_)
                                                                                     }})
            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    'If validation_.result(0) = "OK" Then

                    'Else

                    '    message_ &= Chr(13) & "Falta especificar el valor del campo CURP DEL IMPORTADOR/EXPORTADOR"

                    'End If

                    If validation_.result(0) = "OK" Then

                    Else

                        If validation_.messages Is Nothing Then

                            message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$" & field_, elementMessage_(field_))

                            Else


                                message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                            End If

                        End If

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS3 &
                 "." &
                 CamposPedimento.CA_RAZON_SOCIAL_IOE.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS3,
                                                         CamposPedimento.CA_RAZON_SOCIAL_IOE), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_RAZON_SOCIAL_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_RAZON_SOCIAL_IOE.0",
                                                                                   elementMessage_(field_)
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                'If validation_.result(0) = "OK" Then

                'Else

                '    message_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACIÓN SOCIAL DEL IMPORTADOR/EXPORTADOR"

                'End If

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages Is Nothing Then

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                    Else

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$" & field_, elementMessage_(field_))

                        Else

                            message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                        End If

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS3 &
                 "." &
                 CamposPedimento.CA_DOMICILIO_IOE.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS3,
                                                         CamposPedimento.CA_DOMICILIO_IOE), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_DOMICILIO_IOE",
                                         New Dictionary(Of String, String) From {{"S3.CA_DOMICILIO_IOE.0",
                                                                                   elementMessage_(field_)
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                'If validation_.result(0) = "OK" Then

                'Else

                '    message_ &= Chr(13) & "Falta especificar el valor del campo DOMICILIO DEL IMPORTADOR/EXPORTADOR"

                'End If

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages Is Nothing Then

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                    Else

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$" & field_, elementMessage_(field_))

                        Else


                            message_ &= Chr(13) & "Falta Especificar el valor del campo " & field_

                        End If

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_TIPO_OPERACION.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_TIPO_OPERACION), "")

        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta Especificar el valor del campo TIPO DE OPERACIÓN"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_CVE_PEDIMENTO.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                             SeccionesPedimento.ANS1,
                                             CamposPedimento.CA_CVE_PEDIMENTO, False).SubString(0, 2), "")

        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta Especificar el valor del campo CVE. PEDIMENTO"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_REGIMEN.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                             SeccionesPedimento.ANS1,
                                             CamposPedimento.CA_REGIMEN, False).SubString(0, 2), "")

        'Dim cveRegimen_ = If(GetAttributeValue(pedimento_,
        '                                     SeccionesPedimento.ANS1,
        '                                     CamposPedimento.CA_REGIMEN, False).SubString(0, 2), "")
        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta Especificar el valor del campo REGIMEN"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS10 &
                 "." &
                 CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS10,
                                                         CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC), "")

        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACION O RAZON SOCIAL. DEL COMPRADOR"

        End If


        'AQUI PUEDEN IR VARIOS EVALUAR LA SECCIÓN DATOS DEL PROVEEDOR ARREGLO 12

        field_ = "S" &
                 SeccionesPedimento.ANS10 &
                 "." &
                 CamposPedimento.CA_CFDI_FACTURA.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS10,
                                                         CamposPedimento.CA_CFDI_FACTURA), "")

        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta especificar el valor del campo NUM. CFDI O DOCUMENTO EQUIVALENTE."

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS13 &
                 "." &
                 CamposPedimento.CA_FECHA_FACTURA.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS13,
                                                         CamposPedimento.CA_FECHA_FACTURA), "")


        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_FECHA_VALIDACION.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_FECHA_VALIDACION), "")
        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_FACTURA",
                                         New Dictionary(Of String, String) From {{"S13.CA_FECHA_FACTURA.0",
                                                                                    elementMessage_("S13.CA_FECHA_FACTURA")
                                                                                 },
                                                                                 {"S1.CA_FECHA_VALIDACION.0",
                                                                                    elementMessage_(field_)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                'If validation_.result(0) = "OK" Then

                'Else

                '    message_ &= Chr(13) & "Error en el campo FECHA DE FACTURA '" &
                '                           facturaDate_ &
                '                          "' debe ser menor o igual a la FECHA DE VALIDACIÓN '" &
                '                          validationDateValue_ & "'"

                'End If

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages Is Nothing Then

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & CamposPedimento.CA_FECHA_FACTURA.ToString

                    Else

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$S13.CA_FECHA_FACTURA", elementMessage_("S13.CA_FECHA_FACTURA")).Replace("$S1.CA_FECHA_VALIDACION", elementMessage_(field_))

                        Else

                            message_ &= Chr(13) & "Falta Especificar el valor del campo " & CamposPedimento.CA_FECHA_FACTURA.ToString

                        End If

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS13 &
                 "." &
                 CamposPedimento.CA_CVE_MONEDA_FACTURA.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS13,
                                                         CamposPedimento.CA_CVE_MONEDA_FACTURA), "")


        If elementMessage_(field_) = "" Then

            message_ &= Chr(13) & "Falta especificar el valor del campo CLAVE DE LA MONEDA DE LA FACTURA"

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS13 &
                 "." &
                 CamposPedimento.CA_MONTO_MONEDA_FACTURA.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS13,
                                                         CamposPedimento.CA_MONTO_MONEDA_FACTURA), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_MONTO_MONEDA_FACTURA",
                                         New Dictionary(Of String, String) From {{"S13.CA_MONTO_MONEDA_FACTURA.0",
                                                                                    elementMessage_(field_)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else


                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S13.CA_MONTO_MONEDA_FACTURA", elementMessage_("S13.CA_MONTO_MONEDA_FACTURA"))

                    Else

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & CamposPedimento.CA_MONTO_MONEDA_FACTURA.ToString

                    End If

                End If

            End If


        End If

        field_ = "S" &
                 SeccionesPedimento.ANS1 &
                 "." &
                 CamposPedimento.CA_ACUSE_ELECTRONICO_VALIDACION.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS1,
                                                         CamposPedimento.CA_ACUSE_ELECTRONICO_VALIDACION), "")


        elementMessage_("SFAC1.CP_APLICA_ENAJENACION") = "NO"

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_ACUSE_ELECTRONICO_VALIDACION_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S1.CA_ACUSE_ELECTRONICO_VALIDACION.0",
                                                                                   elementMessage_(field_)
                                                                                 },
                                                                                 {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   elementMessage_("S1.CA_CVE_PEDIMENTO")
                                                                                 },
                                                                                 {"SFAC1.CP_APLICA_ENAJENACION.0",
                                                                                  elementMessage_("SFAC1.CP_APLICA_ENAJENACION")
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S1.CA_ACUSE_ELECTRONICO_VALIDACION", elementMessage_("S1.CA_ACUSE_ELECTRONICO_VALIDACION")).
                                                                      Replace("$S1.CA_CVE_PEDIMENTO", elementMessage_("S1.CA_CVE_PEDIMENTO")).
                                                                      Replace("$SFAC1.CP_APLICA_ENAJENACION", elementMessage_("SFAC1.CP_APLICA_ENAJENACION"))

                    Else

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & CamposPedimento.CA_ACUSE_ELECTRONICO_VALIDACION.ToString

                    End If

                End If

            End If


        End If

        field_ = "S" &
                 SeccionesPedimento.ANS13 &
                 "." &
                 CamposPedimento.CA_INCOTERM.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS13,
                                                         CamposPedimento.CA_INCOTERM), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_INCOTERM",
                                         New Dictionary(Of String, String) From {{"S13.CA_INCOTERM.0",
                                                                                  elementMessage_(field_)
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S13.CA_INCOTERM", elementMessage_("S13.CA_INCOTERM"))

                    Else

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & CamposPedimento.CA_INCOTERM.ToString

                    End If

                End If

            End If


        End If

        field_ = "S" &
                 SeccionesPedimento.ANS11 &
                 "." &
                 CamposPedimento.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS11,
                                                         CamposPedimento.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO), "")

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

                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S11.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO", elementMessage_("S11.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO"))

                    Else

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & CamposPedimento.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO.ToString

                    End If

                End If

            End If


        End If

        field_ = "S" &
                 SeccionesPedimento.ANS44 &
                 "." &
                 CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS44,
                                                         CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA), "")

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

                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S44.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA", elementMessage_("S44.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA"))

                    Else

                        message_ &= Chr(13) & "Falta Especificar el valor del campo " & CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA.ToString

                    End If

                End If

            End If


        End If

        field_ = "S" &
                 SeccionesPedimento.ANS14 &
                 "." &
                 CamposPedimento.CA_FECHA_PAGO.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS14,
                                                         CamposPedimento.CA_FECHA_PAGO), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_PAGO",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_PAGO.0",
                                                                                   elementMessage_(field_)
                                                                                 },
                                                                                 {"S1.CA_FECHA_VALIDACION.0",
                                                                                   elementMessage_("S1.CA_FECHA_VALIDACION")
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If elementMessage_(field_) <> "" Then

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$S14.CA_FECHA_PAGO", elementMessage_("S14.CA_FECHA_PAGO")).
                                                                          Replace("$S1.CA_FECHA_VALIDACION", elementMessage_("S1.CA_FECHA_VALIDACION"))

                        Else

                            message_ &= Chr(13) & "Error en el valor del campo FECHA PAGO. '" &
                                               elementMessage_(field_) & "'" & Chr(13) &
                                              " debe ser igual a la fecha de validación '" &
                                              elementMessage_("S1.CA_FECHA_VALIDACION") & "'"

                        End If



                    Else

                        message_ &= Chr(13) & "Error en el valor del campo FECHA PAGO. ''"


                    End If


                End If

            End If

        End If

        'AQUI SE HARÁ UN CICLO PARA REVISAR TODA LA SECCIÓN YA QUE PUEDE HABER VARIOS IDENTIFICADOR A NIVEL PEDIMENTO

        Dim seccion_ = pedimento_.Seccion(SeccionesPedimento.ANS18)

        Dim index_ = 0

        For Each Nodo_ In seccion_.Nodos


            field_ = "S" &
                 SeccionesPedimento.ANS18 &
                 "." &
                 CamposPedimento.CA_CVE_IDENTIFICADOR.ToString &
                 "." &
                 index_

            elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(0).Nodos(0), Campo).Valor, "")

            validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_CVE_IDENTIFICADOR",
                                             New Dictionary(Of String, String) From {{"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                      elementMessage_(field_)
                                                                                     }})
            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then

                        field_ = "S" &
                                 SeccionesPedimento.ANS18 &
                                 "." &
                                 CamposPedimento.CA_COMPLEMENTO_1.ToString &
                                  "." &
                                  index_

                        elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(1).Nodos(0), Campo).Valor, "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_COMPLEMENTO_1",
                                                         New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_1.0",
                                                                                                   elementMessage_(field_)
                                                                                                 },
                                                                                                  {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                                  elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_)
                                                                                                 }})
                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    If elementMessage_(field_) = "" Then

                                        If validation_.messages(0) <> "" Then

                                            message_ &= Chr(13) & validation_.messages(0).Replace("$S18.CA_COMPLEMENTO_1", elementMessage_("S18.CA_COMPLEMENTO_1")).
                                                                                          Replace("$S18.CA_CVE_IDENTIFICADOR", elementMessage_("S18.CA_CVE_IDENTIFICADOR"))

                                        Else

                                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 1 para el identificador '" &
                                                                   elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_) &
                                                                   "' el complemento no debe estar vacío"

                                        End If


                                    Else

                                        If validation_.messages(0) <> "" Then

                                            message_ &= Chr(13) & validation_.messages(0).Replace("$S18.CA_COMPLEMENTO_1", elementMessage_("S18.CA_COMPLEMENTO_1")).
                                                                                          Replace("$S18.CA_CVE_IDENTIFICADOR", elementMessage_("S18.CA_CVE_IDENTIFICADOR"))

                                        Else
                                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 1 valor inválido '" &
                                                               elementMessage_(field_) &
                                                               "' para el identificador '" &
                                                               elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_) &
                                                               "' el complemento debe estar vacío"

                                        End If

                                    End If

                                End If

                            End If

                        End If

                        field_ = "S" &
                                 SeccionesPedimento.ANS18 &
                                 "." &
                                 CamposPedimento.CA_COMPLEMENTO_2.ToString &
                                  "." &
                                  index_

                        elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(2).Nodos(0), Campo).Valor, "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_COMPLEMENTO_2",
                                                         New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_2.0",
                                                                                                  elementMessage_(field_)
                                                                                                 },
                                                                                                 {"S18.CA_COMPLEMENTO_1.0",
                                                                                                  elementMessage_("S18.CA_COMPLEMENTO_1." & index_)
                                                                                                 },
                                                                                                  {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                                  elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_)
                                                                                                 }})
                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    If elementMessage_(field_) = "" Then

                                        If validation_.messages(0) <> "" Then

                                            message_ &= Chr(13) & validation_.messages(0).Replace("$S18.CA_COMPLEMENTO_1", elementMessage_("S18.CA_COMPLEMENTO_1")).
                                                                                          Replace("$S18.CA_COMPLEMENTO_2", elementMessage_("S18.CA_COMPLEMENTO_2")).
                                                                                          Replace("$S18.CA_CVE_IDENTIFICADOR", elementMessage_("S18.CA_CVE_IDENTIFICADOR"))

                                        Else

                                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 2 para el identificador '" &
                                                                elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_) &
                                                               "' el complemento no debe estar vacío"

                                        End If



                                    Else



                                        If validation_.messages(0) <> "" Then

                                            message_ &= Chr(13) & validation_.messages(0).Replace("$S18.CA_COMPLEMENTO_1", elementMessage_("S18.CA_COMPLEMENTO_1")).
                                                                                          Replace("$S18.CA_COMPLEMENTO_2", elementMessage_("S18.CA_COMPLEMENTO_2")).
                                                                                          Replace("$S18.CA_CVE_IDENTIFICADOR", elementMessage_("S18.CA_CVE_IDENTIFICADOR"))

                                        Else

                                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 2 valor inválido '" &
                                                                    elementMessage_(field_) &
                                                                   "' para el identificador '" &
                                                                    elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_) &
                                                                   "' el complemento debe estar vacío"

                                        End If

                                    End If

                                End If

                            End If

                        End If

                        field_ = "S" &
                                 SeccionesPedimento.ANS18 &
                                 "." &
                                 CamposPedimento.CA_COMPLEMENTO_3.ToString &
                                  "." &
                                  index_

                        elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(3).Nodos(0), Campo).Valor, "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_COMPLEMENTO_3",
                                                         New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_3.0",
                                                                                                   elementMessage_(field_)
                                                                                                 },
                                                                                                 {"S18.CA_COMPLEMENTO_1.0",
                                                                                                    elementMessage_("S18.CA_COMPLEMENTO_1." & index_)
                                                                                                 },
                                                                                                  {"S18.CA_CVE_IDENTIFICADOR.0",
                                                                                                   elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_)
                                                                                                 }})
                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else


                                    If elementMessage_(field_) = "" Then



                                        If validation_.messages(0) <> "" Then

                                            message_ &= Chr(13) & validation_.messages(0).Replace("$S18.CA_COMPLEMENTO_1", elementMessage_("S18.CA_COMPLEMENTO_1")).
                                                                                          Replace("$S18.CA_COMPLEMENTO_2", elementMessage_("S18.CA_COMPLEMENTO_2")).
                                                                                          Replace("$S18.CA_CVE_IDENTIFICADOR", elementMessage_("S18.CA_CVE_IDENTIFICADOR"))

                                        Else

                                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 3 para el identificador '" &
                                                               elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_) &
                                                               "' el complemento no debe estar vacío"

                                        End If

                                    Else



                                        If validation_.messages(0) <> "" Then

                                            message_ &= Chr(13) & validation_.messages(0).Replace("$S18.CA_COMPLEMENTO_1", elementMessage_("S18.CA_COMPLEMENTO_1")).
                                                                                          Replace("$S18.CA_COMPLEMENTO_3", elementMessage_("S18.CA_COMPLEMENTO_3")).
                                                                                          Replace("$S18.CA_CVE_IDENTIFICADOR", elementMessage_("S18.CA_CVE_IDENTIFICADOR"))

                                        Else

                                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 3 valor inválido '" &
                                                               elementMessage_(field_) &
                                                               "' para el identificador '" &
                                                               elementMessage_("S18.CA_CVE_IDENTIFICADOR." & index_) &
                                                                               "' el complemento debe estar vacío"

                                        End If

                                    End If


                                End If

                            End If

                        End If

                    Else


                        message_ &= Chr(13) & "Error en el campo CLAVE. IDENTIFICADOR. Valor inválido '" &
                                                    elementMessage_(field_) & "'"

                    End If

                End If

            End If

            index_ += 1

        Next

        field_ = "S" &
                 SeccionesPedimento.ANS14 &
                 "." &
                 CamposPedimento.CA_FECHA_PRESENTACION.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS14,
                                                         CamposPedimento.CA_FECHA_PRESENTACION), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_PRESENTACION",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_PRESENTACION.0",
                                                                                      elementMessage_(field_)
                                                                                 },
                                                                                  {"S14.CA_FECHA_PAGO.0",
                                                                                   elementMessage_("S14.CA_FECHA_PAGO")
                                                                                 }})
        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S14.CA_FECHA_PRESENTACION", elementMessage_("S14.CA_FECHA_PRESENTACION")).
                                                                                          Replace("$S14.CA_FECHA_PAGO", elementMessage_("S14.CA_FECHA_PAGO"))

                    Else

                        message_ &= Chr(13) & "Error en el valor del campo FECHA PRESENTACION '" &
                                              elementMessage_(field_) &
                                              "'" &
                                              Chr(13) &
                                              "('La fecha de presentación debe ser igual a la fecha de pago')"

                    End If


                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS14 &
                 "." &
                 CamposPedimento.CA_FECHA_EXTRACCION.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS14,
                                                         CamposPedimento.CA_FECHA_EXTRACCION), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_FECHA_EXTRACCION",
                                         New Dictionary(Of String, String) From {{"S14.CA_FECHA_EXTRACCION.0",
                                                                                   elementMessage_(field_)
                                                                                 },
                                                                                  {"S14.CA_FECHA_PAGO.0",
                                                                                   elementMessage_("S14.CA_FECHA_PAGO")
                                                                                 },
                                                                                  {"S1.CA_CVE_PEDIMENTO.0",
                                                                                  elementMessage_("S1.CA_CVE_PEDIMENTO")
                                                                                 }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If elementMessage_(field_) > elementMessage_("S14.CA_FECHA_PAGO") Then

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$S14.CA_FECHA_EXTRACCION", elementMessage_("S14.CA_FECHA_EXTRACCION")).
                                                                                          Replace("$S14.CA_FECHA_PAGO", elementMessage_("S14.CA_FECHA_PAGO")).
                                                                                          Replace("$S1.CA_CVE_PEDIMENTO", elementMessage_("S1.CA_CVE_PEDIMENTO"))

                        Else

                            message_ &= Chr(13) & "Error en el valor del campo FECHA EXTRACCIÓN '" &
                                               elementMessage_(field_) &
                                             "'" & Chr(13) & "(' debe ser menor o igual a la fecha de PAGO '" & elementMessage_("S14.CA_FECHA_PAGO") & "')"

                        End If



                    Else



                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$S14.CA_FECHA_EXTRACCION", elementMessage_("S14.CA_FECHA_EXTRACCION")).
                                                                                          Replace("$S14.CA_FECHA_PAGO", elementMessage_("S14.CA_FECHA_PAGO")).
                                                                                          Replace("$S1.CA_CVE_PEDIMENTO", elementMessage_("S1.CA_CVE_PEDIMENTO"))

                        Else

                            message_ &= Chr(13) & "Error en el valor del campo FECHA EXTRACCIÓN '" &
                                              elementMessage_(field_) &
                                             "'" & Chr(13) & "('La fecha de EXTRACCIÓN no es compatible con la clave de pedimento puesta '" &
                                             elementMessage_("S1.CA_CVE_PEDIMENTO") & "')"

                        End If

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS12 &
                 "." &
                 CamposPedimento.CA_ID_TRANSPORTE.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS12,
                                                         CamposPedimento.CA_ID_TRANSPORTE), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_ID_TRANSPORTE",
                                         New Dictionary(Of String, String) From {{"S12.CA_ID_TRANSPORTE.0",
                                                                                   elementMessage_(field_)
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S12.CA_ID_TRANSPORTE", elementMessage_("S12.CA_ID_TRANSPORTE"))

                    Else

                        message_ &= Chr(13) & "Error en el valor del campo CA_ID_TRANSPORTE '" &
                                           elementMessage_(field_)

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS12 &
                 "." &
                 CamposPedimento.CA_CVE_PAIS_TRANSPORTE.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS12,
                                                         CamposPedimento.CA_CVE_PAIS_TRANSPORTE), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CVE_PAIS_TRANSPORTE",
                                         New Dictionary(Of String, String) From {{"S12.CA_CVE_PAIS_TRANSPORTE.0",
                                                                                   elementMessage_(field_)
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S12.CA_CVE_PAIS_TRANSPORTE", elementMessage_("S12.CA_CVE_PAIS_TRANSPORTE"))

                    Else

                        message_ &= Chr(13) & "Error en el valor del campo CLAVE DEL PAIS DE TRANSPORTE '" &
                                          elementMessage_(field_)

                    End If

                End If

            End If

        End If

        'PARA EXPORTACIÓN NORMAL PUEDE SER OPCIONAL EL NÜMERO DE CANDADO

        'field_ = "S" &
        '         SeccionesPedimento.ANS15 &
        '         "." &
        '         CamposPedimento.CA_NUMERO_CANDADO.ToString

        'elementMessage_(field_) = If(GetAttributeValue(pedimento_,
        '                                                 SeccionesPedimento.ANS15,
        '                                                 CamposPedimento.CA_NUMERO_CANDADO), "")

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_NUMERO_CANDADO",
        '                                 New Dictionary(Of String, String) From {{"S15.CA_NUMERO_CANDADO.0",
        '                                                                           elementMessage_(field_)
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            If validation_.messages(0) <> "" Then

        '                message_ &= Chr(13) & validation_.messages(0).Replace("$S15.CA_NUMERO_CANDADO", elementMessage_("S15.CA_NUMERO_CANDADO"))

        '            Else

        '                message_ &= Chr(13) & "Error en el valor del campo NÚMERO DE CANDADO '" &
        '                            elementMessage_(field_) & "'"

        '            End If

        '        End If

        '    End If

        'End If
        index_ = 0

        For Each nodo_ In pedimento_.Seccion(SeccionesPedimento.ANS16).Nodos

            field_ = "S" &
                 SeccionesPedimento.ANS16 &
                 "." &
                 CamposPedimento.CA_GUIA_MANIFIESTO_BL.ToString &
                 "." &
                 index_

            elementMessage_(field_) = If(DirectCast(nodo_.Nodos(0).Nodos(0), Campo).Valor, "")

            validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_GUIA_MANIFIESTO_BL_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S16.CA_GUIA_MANIFIESTO_BL.0",
                                                                                   elementMessage_(field_)
                                                                                },
                                                                                {"S1.CA_MEDIO_TRANSPORTE.0",
                                                                                  elementMessage_("S1.CA_MEDIO_TRANSPORTE")
                                                                                 }})

            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then

                    Else

                        If elementMessage_(field_) = "" Then

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S16.CA_GUIA_MANIFIESTO_BL", elementMessage_(field_)).
                                                                          Replace("$S1.CA_MEDIO_TRANSPORTE", elementMessage_("S1.CA_MEDIO_TRANSPORTE"))

                            Else

                                message_ &= Chr(13) & "Error en el valor del campo NUMERO (GUIA/CONOCIMIENTO EMBARQUE) '' " & Chr(13) &
                                              " no debe ir vacio para el medio de transporte '" &
                                              elementMessage_("S1.CA_MEDIO_TRANSPORTE") & "'"

                            End If

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S16.CA_GUIA_MANIFIESTO_BL", elementMessage_(field_)).
                                                                          Replace("$S1.CA_MEDIO_TRANSPORTE", elementMessage_("S1.CA_MEDIO_TRANSPORTE"))

                            Else

                                message_ &= Chr(13) & "Error en el valor del campo NUMERO (GUIA/CONOCIMIENTO EMBARQUE) '" & elementMessage_(field_) & "'" & Chr(13) &
                                              "debe ir vacio para el medio de transporte '" &
                                              If(elementMessage_("S1.CA_MEDIO_TRANSPORTE"), "") & "'"

                            End If


                        End If


                    End If

                End If

            End If

            If elementMessage_(field_) <> "" Then

                field_ = "S" &
                 SeccionesPedimento.ANS16 &
                 "." &
                 CamposPedimento.CA_MASTER_HOUSE.ToString &
                 "." &
                 index_

                elementMessage_(field_) = If(DirectCast(nodo_.Nodos(1).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                          RunRoom(Of String)("A22.CA_MASTER_HOUSE",
                                             New Dictionary(Of String, String) From {{"S16.CA_MASTER_HOUSE.0",
                                                                                       elementMessage_(field_)
                                                                                    }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S16.CA_MASTER_HOUSE", elementMessage_(field_))

                            Else

                                message_ &= Chr(13) & "Error en el valor del campo MASTER HOUSE '" &
                                             elementMessage_(field_) & "'"

                            End If

                        End If

                    End If

                End If



            End If


            index_ += 1

        Next


        'PARA EXPORTACIÓN PUEDE QUE LLEVE O NO EL CONTENEDOR SIN IMPORTAR SI SE DECLARA O NO LA GUÍA

        index_ = 0

        For Each nodo_ In pedimento_.Seccion(SeccionesPedimento.ANS17).Nodos

            field_ = "S" &
            SeccionesPedimento.ANS17 &
            "." &
            CamposPedimento.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO.ToString &
            "." &
            index_

            elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                     SeccionesPedimento.ANS17,
                                                     CamposPedimento.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO), "")

            'validation_ = _cubedatos.
            '          RunRoom(Of String)("A22.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO",
            '                             New Dictionary(Of String, String) From {{"S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO.0",
            '                                                                       elementMessage_(field_)
            '                                                                    }})

            'If validation_.result IsNot Nothing Then

            '    If validation_.result.Count > 0 Then

            '        If validation_.result(0) = "OK" Then

            '        Else

            '            If validation_.messages(0) <> "" Then

            '                message_ &= Chr(13) & validation_.messages(0).Replace("$S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO", elementMessage_("S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO"))

            '            Else

            '                message_ &= Chr(13) & "Error en campo NUMERO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO. 'está vacío'"

            '            End If


            '        End If

            '    End If

            'End If

            field_ = "S" &
             SeccionesPedimento.ANS17 &
             "." &
             CamposPedimento.CA_CVE_TIPO_CONTENEDOR.ToString &
             "." &
             index_

            elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                     SeccionesPedimento.ANS17,
                                                     CamposPedimento.CA_CVE_TIPO_CONTENEDOR), "")

            validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_CVE_TIPO_CONTENEDOR",
                                         New Dictionary(Of String, String) From {{"S17.CA_CVE_TIPO_CONTENEDOR.0",
                                                                                   elementMessage_(field_)
                                                                                },
                                                                                {"S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO.0",
                                                                                   elementMessage_("S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO." & index_)
                                                                                }})

            If validation_.result IsNot Nothing Then

                If validation_.result.Count > 0 Then

                    If validation_.result(0) = "OK" Then

                    Else

                        If validation_.messages(0) <> "" Then

                            message_ &= Chr(13) & validation_.messages(0).Replace("$S17.CA_CVE_TIPO_CONTENEDOR", elementMessage_(field_)).
                                                                          Replace("$S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO", elementMessage_("S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO." & index_))

                        Else

                            message_ &= Chr(13) & "Error en campo TIPO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO. '" &
                                           elementMessage_(field_) &
                                           "' valor inválido para el contenedor '" &
                                           elementMessage_("S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO." & index_)

                        End If


                    End If

                End If

            End If

            index_ += 1

        Next



        field_ = "S" &
                 SeccionesPedimento.ANS23 &
                 "." &
                 CamposPedimento.CA_OBSERVACIONES_PEDIMENTO.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS23,
                                                         CamposPedimento.CA_OBSERVACIONES_PEDIMENTO), "")


        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_OBSERVACIONES_PEDIMENTO",
                                         New Dictionary(Of String, String) From {{"S23.CA_OBSERVACIONES_PEDIMENTO.0",
                                                                                   elementMessage_(field_)
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                Else

                    If validation_.messages(0) <> "" Then

                        message_ &= Chr(13) & validation_.messages(0).Replace("$S23.CA_OBSERVACIONES_PEDIMENTO", elementMessage_("S23.CA_OBSERVACIONES_PEDIMENTO"))

                    Else

                        message_ &= Chr(13) & "Error en campo OBSERVACIONES. '" &
                                          elementMessage_(field_) & "' valor inválido"

                    End If

                End If

            End If

        End If

        field_ = "S" &
                 SeccionesPedimento.ANS20 &
                 "." &
                 CamposPedimento.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS.ToString

        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS20,
                                                         CamposPedimento.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS), "")

        validation_ = _cubedatos.
                      RunRoom(Of String)("A22.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS_NORMAL_EXPORTACION",
                                         New Dictionary(Of String, String) From {{"S20.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS.0",
                                                                                    elementMessage_(field_)
                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                   elementMessage_("S1.CA_CVE_PEDIMENTO")
                                                                                }})

        If validation_.result IsNot Nothing Then

            If validation_.result.Count > 0 Then

                If validation_.result(0) = "OK" Then

                    If elementMessage_(field_) <> "" Then

                        field_ = "S" &
                                 SeccionesPedimento.ANS20 &
                                   "." &
                                 CamposPedimento.CA_FRACCION_ORIGINAL.ToString

                        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS20,
                                                         CamposPedimento.CA_FRACCION_ORIGINAL), "")

                        field_ = "S" &
                                 SeccionesPedimento.ANS20 &
                                   "." &
                                 CamposPedimento.CA_FECHA_PEDIMENTO_ORIGINAL.ToString

                        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS20,
                                                         CamposPedimento.CA_FECHA_PEDIMENTO_ORIGINAL), "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_FECHA_PEDIMENTO_ORIGINAL_NORMAL_EXPORTACION",
                                                         New Dictionary(Of String, String) From {{"S20.CA_FECHA_PEDIMENTO_ORIGINAL.0",
                                                                                                   elementMessage_(field_)
                                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                                   elementMessage_("S1.CA_CVE_PEDIMENTO")
                                                                                                }, {"S14.CA_FECHA_PAGO.0",
                                                                                                   elementMessage_("S14.CA_FECHA_PAGO")
                                                                                                }
                                                                                                })

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    If validation_.messages(0) <> "" Then

                                        message_ &= Chr(13) & validation_.messages(0).Replace("$S14.CA_FECHA_PAGO", elementMessage_("S14.CA_FECHA_PAGO")).
                                                                                      Replace("$S1.CA_CVE_PEDIMENTO", elementMessage_("S1.CA_CVE_PEDIMENTO")).
                                                                                      Replace("$S20.CA_FECHA_PEDIMENTO_ORIGINAL", elementMessage_("S20.CA_FECHA_PEDIMENTO_ORIGINAL"))

                                    Else

                                        message_ &= Chr(13) & "Error en campo FECHA DE OPERACIÓN ORIGINAL '" &
                                                          elementMessage_(field_) & Chr(13) &
                                                          "' relación inválida con CLAVE DE PEDIMENTO '" &
                                                          elementMessage_("S1.CA_CVE_PEDIMENTO") &
                                                          "'y FECHA DE PAGO '" &
                                                          elementMessage_("S14.CA_FECHA_PAGO")

                                    End If

                                End If

                            End If

                        End If

                        field_ = "S" &
                                 SeccionesPedimento.ANS20 &
                                   "." &
                                 CamposPedimento.CA_CVE_PEDIMENTO_ORIGINAL.ToString

                        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS20,
                                                         CamposPedimento.CA_CVE_PEDIMENTO_ORIGINAL), "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_CVE_PEDIMENTO_ORIGINAL_NORMAL_EXPORTACION",
                                                         New Dictionary(Of String, String) From {{"S20.CA_CVE_PEDIMENTO_ORIGINAL.0",
                                                                                                   elementMessage_(field_)
                                                                                                }})

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    If validation_.messages(0) <> "" Then

                                        message_ &= Chr(13) & validation_.messages(0).Replace("$S20.CA_CVE_PEDIMENTO_ORIGINAL", elementMessage_("S20.CA_CVE_PEDIMENTO_ORIGINAL"))

                                    Else

                                        message_ &= Chr(13) & "Error en campo CVE. PEDIMENTO ORIGINAL '" &
                                                          elementMessage_(field_) &
                                                          "' valor inválido"

                                    End If



                                End If

                            End If

                        End If

                        field_ = "S" &
                                 SeccionesPedimento.ANS20 &
                                   "." &
                                 CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL.ToString

                        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS20,
                                                         CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL), "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_ADUANA_DESPACHO_ORIGINAL_NORMAL_EXPORTACION",
                                                         New Dictionary(Of String, String) From {{"S20.CA_ADUANA_DESPACHO_ORIGINAL.0",
                                                                                                   elementMessage_(field_)
                                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                                   elementMessage_("S1.CA_CVE_PEDIMENTO")
                                                                                                }})

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else



                                    If validation_.messages(0) <> "" Then

                                        message_ &= Chr(13) & validation_.messages(0).Replace("$S20.CA_ADUANA_DESPACHO_ORIGINAL", elementMessage_("S20.CA_ADUANA_DESPACHO_ORIGINAL")).
                                                                                      Replace("$S1.CA_CVE_PEDIMENTO", elementMessage_("S1.CA_CVE_PEDIMENTO"))

                                    Else

                                        message_ &= Chr(13) & "Error en campo ADUANA DESPACHO ORIGINALL '" &
                                                          elementMessage_(field_) &
                                                          "' valor inválido con CLAVE DE PEDIMENTO '" &
                                                          elementMessage_("S1.CA_CVE_PEDIMENTO") & "'"

                                    End If

                                End If

                            End If

                        End If

                        field_ = "S" &
                                 SeccionesPedimento.ANS29 &
                                   "." &
                                 CamposPedimento.CA_PATENTE_ORIGINAL.ToString

                        elementMessage_(field_) = If(GetAttributeValue(pedimento_,
                                                         SeccionesPedimento.ANS29,
                                                         CamposPedimento.CA_PATENTE_ORIGINAL), "")

                        validation_ = _cubedatos.
                                      RunRoom(Of String)("A22.CA_PATENTE_ORIGINAL_NORMAL_EXPORTACION",
                                                         New Dictionary(Of String, String) From {{"S29.CA_PATENTE_ORIGINAL.0",
                                                                                                   elementMessage_(field_)
                                                                                                }, {"S1.CA_CVE_PEDIMENTO.0",
                                                                                                   elementMessage_("S1.CA_CVE_PEDIMENTO")
                                                                                                }})

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else


                                    If validation_.messages(0) <> "" Then

                                        message_ &= Chr(13) & validation_.messages(0).Replace("$S29.CA_PATENTE_ORIGINAL", elementMessage_("S29.CA_PATENTE_ORIGINAL")).
                                                                                      Replace("$S1.CA_CVE_PEDIMENTO", elementMessage_("S1.CA_CVE_PEDIMENTO"))

                                    Else

                                        message_ &= Chr(13) & "Error en campo PATENTE ORIGINAL '" &
                                                          elementMessage_(field_) &
                                                          "' valor inválido con CLAVE DE PEDIMENTO '" &
                                                          elementMessage_("S1.CA_CVE_PEDIMENTO") & "'"

                                    End If

                                End If

                            End If

                        End If


                    End If

                Else

                    message_ &= Chr(13) & "Error en campo NUMERO DE PEDIMENTO ORIGINAL 7 DIGITOS '" &
                                           elementMessage_(field_) & "' valor inválido"

                End If

            End If

        End If






        seccion_ = pedimento_.Seccion(SeccionesPedimento.ANS24)

        index_ = 0

        For Each Nodo_ In seccion_.Nodos

            If Nodo_.Nodos IsNot Nothing Then

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_FRACCION_ARANCELARIA_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(1).Nodos(0), Campo).Valor, "")


                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_NICO_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(2).Nodos(0), Campo).Valor, "")


                If elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_) = "" Then

                    message_ &= Chr(13) & "Error en campo FRACCIÓN ARANCELARIA '' valor inválido "

                Else


                    If elementMessage_(field_) = "" Then

                        message_ &= Chr(13) & "Error en campo SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL '' valor inválido "

                    Else



                        validation_ = _cubedatos.
                                                          RunRoom(Of String)("A22.CA_NICO_PARTIDA",
                                                                             New Dictionary(Of String, String) From {{"S24.CA_NICO_PARTIDA.0",
                                                                                                                       elementMessage_(field_)
                                                                                                                    }, {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                                                       elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)
                                                                                                                    }})

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    If validation_.messages(0) <> "" Then

                                        message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_FRACCION_ARANCELARIA_PARTIDA", elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA")).
                                                                                      Replace("$S24.CA_NICO_PARTIDA", elementMessage_("S24.CA_NICO_PARTIDA"))

                                    Else

                                        message_ &= Chr(13) & "Error en campo SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL '" &
                                                                   elementMessage_(field_) & "'" & Chr(13) &
                                                                   " valor inválido para FRACCIÓN '" &
                                                                   elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_) &
                                                                   "'"

                                    End If

                                End If

                            End If

                        End If


                    End If


                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_CVE_VINCULACION.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(22).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CVE_VINCULACION_NORMAL_EXPORTACION",
                                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_VINCULACION.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CVE_VINCULACION", elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA"))

                            Else

                                message_ &= Chr(13) &
                                        "Error en campo CLAVE VINCULACIÓN '" &
                                        elementMessage_(field_) &
                                        "' valor inválido"

                            End If




                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_CVE_METODO_VALORACION_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(3).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CVE_METODO_VALORACION_PARTIDA_NORMAL_EXPORTACION",
                                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_METODO_VALORACION_PARTIDA.0",
                                                                                                   elementMessage_(field_)
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CVE_METODO_VALORACION_PARTIDA", elementMessage_("S24.CA_CVE_METODO_VALORACION_PARTIDA"))

                            Else

                                message_ &= Chr(13) & "Error en campo MÉTODO DE VALORACIÓN'" &
                                                                elementMessage_(field_) & "' valor inválido"

                            End If

                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_CVE_UMC_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(4).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CVE_UMC_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_UMC_PARTIDA.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CVE_UMC_PARTIDA", elementMessage_("S24.CA_CVE_UMC_PARTIDA"))

                            Else

                                message_ &= Chr(13) &
                                           "Error en campo UNIDAD DE MEDIDA COMERCIAL'" &
                                           elementMessage_(field_) &
                                           "' valor inválido"

                            End If

                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_CANTIDAD_UMC_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(5).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CANTIDAD_UMC_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S24.CA_CANTIDAD_UMC_PARTIDA.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CANTIDAD_UMC_PARTIDA", elementMessage_("S24.CA_CANTIDAD_UMC_PARTIDA"))

                            Else

                                message_ &= Chr(13) &
                                        "Error en campo CANTIDAD UMC'" &
                                        elementMessage_(field_) &
                                        "' valor inválido"

                            End If

                        End If

                    End If

                End If


                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_CVE_UMT_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(6).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CVE_UMT_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_UMT_PARTIDA.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CVE_UMT_PARTIDA", elementMessage_("S24.CA_CVE_UMT_PARTIDA"))

                            Else

                                message_ &= Chr(13) &
                                           "Error en campo UNIDAD DE MEDIDA TARIFA'" &
                                            elementMessage_(field_) &
                                            "' valor inválido"

                            End If

                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_CANTIDAD_UMT_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(7).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CANTIDAD_UMT_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S24.CA_CANTIDAD_UMT_PARTIDA.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CANTIDAD_UMT_PARTIDA", elementMessage_("S24.CA_CANTIDAD_UMT_PARTIDA"))

                            Else

                                message_ &= Chr(13) &
                                           "Error en campo CANTIDAD UMT'" &
                                           elementMessage_(field_) &
                                           "' valor inválido"

                            End If

                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_CVE_PAIS_COMPRADOR_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(10).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CVE_PAIS_COMPRADOR_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_PAIS_COMPRADOR_PARTIDA.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CVE_PAIS_COMPRADOR_PARTIDA", elementMessage_("S24.CA_CVE_PAIS_COMPRADOR_PARTIDA"))

                            Else

                                message_ &= Chr(13) &
                                            "Error en campo CLAVE PAIS COMPRADOR PARTIDA'" &
                                            elementMessage_(field_) &
                                            "' valor inválido"

                            End If

                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_CVE_PAIS_DESTINO_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(11).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CVE_PAIS_DESTINO_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S24.CA_CVE_PAIS_DESTINO_PARTIDA.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CVE_PAIS_COMPRADOR_PARTIDA", elementMessage_("S24.CA_CVE_PAIS_COMPRADOR_PARTIDA"))

                            Else

                                message_ &= Chr(13) &
                                         "Error en campo CLAVE PAIS DESTINO PARTIDA'" &
                                         elementMessage_(field_) &
                                         "' valor inválido"

                            End If

                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_DESCRIPCION_MERCANCIA_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(12).Nodos(0), Campo).Valor, "")

                If elementMessage_(field_) = "" Then

                    message_ &= Chr(13) & "Error en campo DESCRIPCIÓN MERCANCIA PARTIDA'" &
                                                               elementMessage_(field_) & "' valor inválido"

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS24 &
                                   "." &
                                 CamposPedimento.CA_VALOR_AGREGADO_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(17).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_VALOR_AGREGADO_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S24.CA_VALOR_AGREGADO_PARTIDA.0",
                                                                                                  elementMessage_(field_)},
                                                                                                  {"S1.CA_CVE_PEDIMENTO.0",
                                                                                                  elementMessage_("S1.CA_CVE_PEDIMENTO")
                                                                                               }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_VALOR_AGREGADO_PARTIDA", elementMessage_("S24.CA_VALOR_AGREGADO_PARTIDA")).
                                                                              Replace("$S1.CA_CVE_PEDIMENTO", elementMessage_("S1.CA_CVE_PEDIMENTO"))

                            Else

                                message_ &= Chr(13) &
                                        "Error en campo VALOR AGREGADO PARTIDA'" &
                                        elementMessage_(field_) &
                                        "' valor inválido " &
                                        If(elementMessage_(field_) <> "",
                                        " Si la CVE PEDIMENTO no es RT no se llena",
                                        "")

                            End If

                        End If

                    End If

                End If

                Dim contribucionesPartida_ = Nodo_.Nodos(32).Nodos(0)

                Dim contributionIndex_ = 0

                For Each nodoContribucion_ In contribucionesPartida_.Nodos


                    If nodoContribucion_.Nodos(0).Nodos IsNot Nothing Then

                        field_ = "S" &
                                 SeccionesPedimento.ANS29 &
                                   "." &
                                 CamposPedimento.CA_CVE_CONTRIBUCION_PARTIDA.ToString &
                                 "." &
                                 contributionIndex_

                        elementMessage_(field_) = If(nodoContribucion_.Nodos(0).Nodos IsNot Nothing,
                                                     DirectCast(nodoContribucion_.Nodos(0).Nodos(0),
                                                                Campo).Valor,
                                                     "")

                        field_ = "S" &
                                 SeccionesPedimento.ANS29 &
                                   "." &
                                 CamposPedimento.CA_TASA_PARTIDA.ToString &
                                 "." &
                                 contributionIndex_

                        elementMessage_(field_) = If(nodoContribucion_.Nodos(1).Nodos IsNot Nothing,
                                                     DirectCast(nodoContribucion_.Nodos(1).Nodos(0),
                                                                Campo).Valor,
                                                     "")
                        field_ = "S" &
                                 SeccionesPedimento.ANS29 &
                                   "." &
                                 CamposPedimento.CA_CVE_TIPO_TASA_PARTIDA.ToString &
                                 "." &
                                 contributionIndex_

                        elementMessage_(field_) = If(nodoContribucion_.Nodos(2).Nodos IsNot Nothing,
                                                     DirectCast(nodoContribucion_.Nodos(1).Nodos(0),
                                                                Campo).Valor,
                                                     "")

                        field_ = "S" &
                                 SeccionesPedimento.ANS29 &
                                   "." &
                                 CamposPedimento.CA_FORMA_PAGO_PARTIDA.ToString &
                                 "." &
                                 contributionIndex_

                        elementMessage_(field_) = If(nodoContribucion_.Nodos(3).Nodos IsNot Nothing,
                                                     DirectCast(nodoContribucion_.Nodos(1).Nodos(0),
                                                                Campo).Valor,
                                                     "")


                        validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CONTRIBUCIONESPARTIDASVACIAS",
                                                        New Dictionary(Of String, String) From {{"S29.CA_CVE_CONTRIBUCION_PARTIDA.0",
                                                                                                  elementMessage_("S29.CA_CVE_CONTRIBUCION_PARTIDA." & contributionIndex_)
                                                                                               },
                                                                                               {"S29.CA_CVE_TIPO_TASA_PARTIDA.0",
                                                                                                  elementMessage_("S29.CA_CVE_TIPO_TASA_PARTIDA." & contributionIndex_)
                                                                                               },
                                                                                               {"S29.CA_TASA_PARTIDA.0",
                                                                                                  elementMessage_("S29.CA_TASA_PARTIDA." & contributionIndex_)
                                                                                               },
                                                                                               {"S29.CA_FORMA_PAGO_PARTIDA.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                        If validation_.result IsNot Nothing Then

                            If validation_.result.Count > 0 Then

                                If validation_.result(0) = "OK" Then

                                Else

                                    validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CVE_CONTRIBUCION_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S29.CA_CVE_CONTRIBUCION_PARTIDA.0",
                                                                                                  elementMessage_("S29.CA_CVE_CONTRIBUCION_PARTIDA." & contributionIndex_)
                                                                                               }})

                                    If validation_.result IsNot Nothing Then

                                        If validation_.result.Count > 0 Then

                                            If validation_.result(0) = "OK" Then

                                            Else

                                                If validation_.messages(0) <> "" Then

                                                    message_ &= Chr(13) & validation_.messages(0).Replace("$S29.CA_CVE_CONTRIBUCION_PARTIDA", elementMessage_("S29.CA_CVE_CONTRIBUCION_PARTIDA"))

                                                Else

                                                    message_ &= Chr(13) &
                                                                "Error en campo CONTRIBUCIÓN A NIVEL PARTIDA'" &
                                                                elementMessage_("S29.CA_CVE_CONTRIBUCION_PARTIDA." & contributionIndex_) &
                                                                "' valor inválido"

                                                End If

                                            End If

                                        End If

                                    End If


                                    validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_CVE_TIPO_TASA_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S29.CA_CVE_TIPO_TASA_PARTIDA.0",
                                                                                                  elementMessage_("S29.CA_CVE_TIPO_TASA_PARTIDA." & contributionIndex_)
                                                                                               }})

                                    If validation_.result IsNot Nothing Then

                                        If validation_.result.Count > 0 Then

                                            If validation_.result(0) = "OK" Then

                                            Else

                                                If validation_.messages(0) <> "" Then

                                                    message_ &= Chr(13) & validation_.messages(0).Replace("$S29.CA_CVE_TIPO_TASA_PARTIDA", elementMessage_("S29.CA_CVE_TIPO_TASA_PARTIDA"))

                                                Else

                                                    message_ &= Chr(13) &
                                                                "Error en campo CLAVE TIPO DE TASA PARTIDA'" &
                                                                elementMessage_("S29.CA_CVE_TIPO_TASA_PARTIDA." & contributionIndex_) &
                                                                "' valor inválido"

                                                End If

                                            End If

                                        End If

                                    End If


                                    validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_TASA_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S29.CA_TASA_PARTIDA.0",
                                                                                                   elementMessage_("S29.CA_TASA_PARTIDA." & contributionIndex_)
                                                                                               }})

                                    If validation_.result IsNot Nothing Then

                                        If validation_.result.Count > 0 Then

                                            If validation_.result(0) = "OK" Then

                                            Else

                                                If validation_.messages(0) <> "" Then

                                                    message_ &= Chr(13) & validation_.messages(0).Replace("$S29.CA_TASA_PARTIDA", elementMessage_("S29.CA_TASA_PARTIDA"))

                                                Else

                                                    message_ &= Chr(13) &
                                                                "Error en campo TASA PARTIDA'" &
                                                                elementMessage_("S29.CA_TASA_PARTIDA." & contributionIndex_) &
                                                                "' valor inválido"

                                                End If

                                            End If

                                        End If

                                    End If


                                    validation_ = _cubedatos.
                                     RunRoom(Of String)("A22.CA_FORMA_PAGO_PARTIDA",
                                                        New Dictionary(Of String, String) From {{"S29.CA_FORMA_PAGO_PARTIDA.0",
                                                                                                  elementMessage_(field_)
                                                                                               }})

                                    If validation_.result IsNot Nothing Then

                                        If validation_.result.Count > 0 Then

                                            If validation_.result(0) = "OK" Then

                                            Else

                                                If validation_.messages(0) <> "" Then

                                                    message_ &= Chr(13) & validation_.messages(0).Replace("$S29.CA_FORMA_PAGO_PARTIDA", elementMessage_("S29.CA_FORMA_PAGO_PARTIDA"))

                                                Else

                                                    message_ &= Chr(13) & "Error en campo FORMA DE PAGO PARTIDA'" &
                                                               elementMessage_(field_) & "' valor inválido"

                                                End If

                                            End If

                                        End If

                                    End If

                                End If

                            End If

                        End If

                    End If


                Next

                field_ = "S" &
                                 SeccionesPedimento.ANS29 &
                                   "." &
                                 CamposPedimento.CA_MARCA_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(18).Nodos(0), Campo).Valor, "")


                validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_MARCA_PARTIDA_NORMAL_EXPORTACION",
                                                    New Dictionary(Of String, String) From {{"S29.CA_MARCA_PARTIDA.0",
                                                                                              elementMessage_(field_)},
                                                                                              {"S1.CA_REGIMEN.0",
                                                                                              elementMessage_("S1.CA_REGIMEN")
                                                                                           },
                                                                                              {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                              elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)
                                                                                           }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_FRACCION_ARANCELARIA_PARTIDA", elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)).
                                                                              Replace("$S1.CA_REGIMEN", elementMessage_("S1.CA_REGIMEN")).
                                                                              Replace("$S29.CA_MARCA_PARTIDA", elementMessage_(field_))

                            Else

                                message_ &= Chr(13) & "Error en campo MARCA PARTIDA'" &
                                                            elementMessage_(field_) &
                                                            "' valor inválido pára la relación " &
                                                            Chr(13) &
                                                           "REGIMEN '" &
                                                           elementMessage_("S1.CA_REGIMEN") &
                                                           "' FRACCIÓN ARANCELARIA '" &
                                                           elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)

                            End If

                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS29 &
                                   "." &
                                 CamposPedimento.CA_MODELO_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(19).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_MODELO_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S29.CA_MODELO_PARTIDA.0",
                                                                                              elementMessage_(field_)},
                                                                                              {"S24.CA_NICO_PARTIDA.0",
                                                                                              elementMessage_("S24.CA_NICO_PARTIDA." & index_)
                                                                                           },
                                                                                              {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                              elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)
                                                                                           }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            If validation_.messages(0) <> "" Then

                                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_FRACCION_ARANCELARIA_PARTIDA", elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)).
                                                                              Replace("$S24.CA_NICO_PARTIDA", elementMessage_("S24.CA_NICO_PARTIDA." & index_)).
                                                                              Replace("$S29.CA_MODELO_PARTIDA", elementMessage_(field_))


                            Else

                                If elementMessage_(field_) = "" Then

                                    message_ &= Chr(13) & "Error en campo MODELO PARTIDA'" &
                                                               elementMessage_(field_) & "' valor inválido pára la relación " & Chr(13) &
                                                               "NICO '" & elementMessage_("S24.CA_NICO_PARTIDA." & index_) & "' FRACCIÓN ARABCELARIA '" & elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)
                                Else

                                    message_ &= Chr(13) & "Error en campo MODELO PARTIDA'" &
                                                               elementMessage_(field_) & "' valor inválido debe estar vacío pára la relación " & Chr(13) &
                                                               "NICO '" & elementMessage_("S24.CA_NICO_PARTIDA." & index_) & "' FRACCIÓN ARANCELARIA '" & elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)
                                End If

                            End If

                        End If

                    End If

                End If

                'field_ = "S" &
                '                 SeccionesPedimento.ANS24 &
                '                   "." &
                '                 CamposPedimento.CA_CODIGO_PRODUCTO_PARTIDA.ToString &
                '                 "." &
                '                 index_

                'elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(20).Nodos(0), Campo).Valor, "")

                'validation_ = _cubedatos.
                '                     RunRoom(Of String)("A22.CA_CODIGO_PRODUCTO_PARTIDA",
                '                                        New Dictionary(Of String, String) From {{"S24.CA_CODIGO_PRODUCTO_PARTIDA.0",
                '                                                                                  elementMessage_(field_)
                '                                                                               }})

                'If validation_.result IsNot Nothing Then

                '    If validation_.result.Count > 0 Then

                '        If validation_.result(0) = "OK" Then

                '        Else

                '            If validation_.messages(0) <> "" Then

                '                message_ &= Chr(13) & validation_.messages(0).Replace("$S24.CA_CODIGO_PRODUCTO_PARTIDA", elementMessage_(field_))

                '            Else

                '                message_ &= Chr(13) & "Error en campo CODIGO PRODUCTO PARTIDA'" &
                '                                               elementMessage_(field_) & "' valor inválido"

                '            End If

                '        End If

                '    End If

                'End If

                field_ = "S" &
                                 SeccionesPedimento.ANS25 &
                                   "." &
                                 CamposPedimento.CA_VINCULACION_NUMERO_SERIE_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(28).Nodos(0).Nodos(0).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_VINCULACION_NUMERO_SERIE_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S25.CA_VINCULACION_NUMERO_SERIE_PARTIDA.0",
                                                                                              elementMessage_(field_)
                                                                                           }, {"S24.CA_NICO_PARTIDA.0",
                                                                                              elementMessage_("S24.CA_NICO_PARTIDA." & index_)
                                                                                           },
                                                                                              {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                              elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)
                                                                                           }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            message_ &= Chr(13) & "Error en campo NUMERO SERIE PARTIDA'" &
                                                           elementMessage_(field_) & "' valor inválido"

                        End If

                    End If

                End If

                field_ = "S" &
                                 SeccionesPedimento.ANS25 &
                                   "." &
                                 CamposPedimento.CA_KILOMETRAJE_PARTIDA.ToString &
                                 "." &
                                 index_

                elementMessage_(field_) = If(DirectCast(Nodo_.Nodos(28).Nodos(0).Nodos(1).Nodos(0), Campo).Valor, "")


                validation_ = _cubedatos.
                                 RunRoom(Of String)("A22.CA_KILOMETRAJE_PARTIDA",
                                                    New Dictionary(Of String, String) From {{"S25.CA_KILOMETRAJE_PARTIDA.0",
                                                                                              elementMessage_(field_)
                                                                                           },
                                                                                           {"S24.CA_NICO_PARTIDA.0",
                                                                                              elementMessage_("S24.CA_NICO_PARTIDA." & index_)
                                                                                           },
                                                                                           {"S24.CA_FRACCION_ARANCELARIA_PARTIDA.0",
                                                                                              elementMessage_("S24.CA_FRACCION_ARANCELARIA_PARTIDA." & index_)
                                                                                           }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            message_ &= Chr(13) & "Error en campo KILOMETRAJE PARTIDA'" &
                                                           elementMessage_(field_) & "' valor inválido"

                        End If

                    End If

                End If

            End If

        Next

        seccion_ = pedimento_.Seccion(SeccionesPedimento.ANS26)

        For Each NodoPermiso_ In seccion_.Nodos

            Dim nodosNothing = NodoPermiso_.Nodos


            If NodoPermiso_.Nodos(0).Nodos IsNot Nothing Then

                Dim cvePermiso_ As String = If(DirectCast(NodoPermiso_.Nodos(0).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                         RunRoom(Of String)("A22.CA_CVE_PERMISO",
                                            New Dictionary(Of String, String) From {{"S26.CA_CVE_PERMISO.0",
                                                                                      cvePermiso_
                                                                                   }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            message_ &= Chr(13) & "Error en campo CVE PERMISO'" &
                                                       cvePermiso_ & "' valor inválido"

                        End If

                    End If

                End If

                Dim numeroPermiso_ As String = If(DirectCast(NodoPermiso_.Nodos(1).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                             RunRoom(Of String)("A22.CA_NUMERO_PERMISO",
                                                New Dictionary(Of String, String) From {{"S26.CA_NUMERO_PERMISO.0",
                                                                                          numeroPermiso_
                                                                                       }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            message_ &= Chr(13) & "Error en campo NUMERO DE PERMISO'" &
                                                       numeroPermiso_ & "' valor inválido"

                        End If

                    End If

                End If

                Dim firmaDescargo_ As String = If(DirectCast(NodoPermiso_.Nodos(2).Nodos(0), Campo).Valor, "")

                validation_ = _cubedatos.
                             RunRoom(Of String)("A22.CA_FIRMA_ELECTRONICA_PERMISO",
                                                New Dictionary(Of String, String) From {{"S26.CA_FIRMA_ELECTRONICA_PERMISO.0",
                                                                                          firmaDescargo_
                                                                                       }})

                If validation_.result IsNot Nothing Then

                    If validation_.result.Count > 0 Then

                        If validation_.result(0) = "OK" Then

                        Else

                            message_ &= Chr(13) & "Error en campo FIRMA DESCARGO'" &
                                                       firmaDescargo_ & "' valor inválido"

                        End If

                    End If

                End If

            End If



        Next

        seccion_ = pedimento_.Seccion(SeccionesPedimento.ANS27)

        For Each Nodo_ In seccion_.Nodos

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

                                    message_ &= Chr(13) & "Error en campo COMPLEMENTO 1 A NIVEL PARTIDA'" &
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

                                    message_ &= Chr(13) & "Error en campo COMPLEMENTO 2 A NIVEL PARTIDA'" &
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

                                    message_ &= Chr(13) & "Error en campo COMPLEMENTO 3 A NIVEL PARTIDA'" &
                                                           complemento3Partida_ & "' valor inválido"

                                End If

                            End If

                        End If

                    Else

                        message_ &= Chr(13) & "Error en campo IDENTIFICADOR A NIVEL PARTIDA'" &
                                               cveIdentificadorPartida_ & "' valor inválido"

                    End If

                End If

            End If

        Next


        Dim cvePrevalidador_ As String = GetAttributeValue(pedimento_,
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

                    message_ &= Chr(13) & "Error en campo CLAVE DEL PREVALIDADOR'" &
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

                    message_ &= Chr(13) & "Error en campo NOMBRE INSTITUCIÓN BANCARIA'" &
                                               nombreInstitucionBancaria_ & "' valor inválido"

                End If

            End If

        End If

        If message_ = "" Then

            message_ = "FELICIDADES SIN ERRORES"

        End If

        validation_.SetDetailReport(AdviceTypesReport.Information,
                                     message_,
                                     Chr(13) & "Ruta de Validación Exportación Normal Synapsis por Defecto" & Chr(13) &
                                     "Folio de Operación:" & pedimento_.FolioOperacion & Chr(13),
                                     TriggerSourceTypes.Route
                                     )

        Return validation_

    End Function

    Private Function ValidateRoute21(pedimento_ As DocumentoElectronico) As ValidatorReport


        'Dim message_ = ""


        'Dim destinoOrigen_ As String = If(GetAttributeValue(pedimento_,
        '                                                 SeccionesPedimento.ANS1,
        '                                                 CamposPedimento.CA_DESTINO_ORIGEN), "")

        'Dim validation_ = _cubedatos.
        '                  RunRoom(Of String)("A22.CA_DESTINO_ORIGEN",
        '                                     New Dictionary(Of String, String) From {{"S1.CA_DESTINO_ORIGEN.0",
        '                                                                             destinoOrigen_
        '                                                                            }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ = "Error en el campo DESTINO/ORIGEN '" & destinoOrigen_ & "' valor inválido"

        '        End If



        '    End If

        'End If

        'Dim aduanaES_ As String = If(GetAttributeValue(pedimento_,
        '                                                 SeccionesPedimento.ANS1,
        '                                                 CamposPedimento.CA_DESTINO_ORIGEN), "")

        'validation_ = _cubedatos.
        '                  RunRoom(Of String)("A22.CA_ADUANA_ENTRADA_SALIDA",
        '                                     New Dictionary(Of String, String) From {{"S1.CA_ADUANA_ENTRADA_SALIDA.0",
        '                                                                             aduanaES_
        '                                                                            }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el campo ADUANA E/S '" & aduanaES_ & "' valor inválido"

        '        End If



        '    End If

        'End If

        'Dim medioTransporte_ As String = If(GetAttributeValue(pedimento_,
        '                                                 SeccionesPedimento.ANS1,
        '                                                 CamposPedimento.CA_MEDIO_TRANSPORTE), "")

        'validation_ = _cubedatos.
        '                  RunRoom(Of String)("A22.CA_MEDIO_TRANSPORTE",
        '                                     New Dictionary(Of String, String) From {{"S1.CA_MEDIO_TRANSPORTE.0",
        '                                                                             medioTransporte_
        '                                                                            }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el campo MEDIO DE TRANSPORTE '" & medioTransporte_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'validation_ = _cubedatos.
        '                  RunRoom(Of String)("A22.CA_PESO_BRUTO",
        '                                     New Dictionary(Of String, String) From {{"S1.CA_PESO_BRUTO.0",
        '                                                                              GetAttributeValue(pedimento_,
        '                                                                                                SeccionesPedimento.ANS1,
        '                                                                                                CamposPedimento.CA_PESO_BRUTO)
        '                                                                            }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta Especificar el valor del campo Peso Bruto"

        '        End If



        '    End If

        'End If


        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_MARCAS_NUMEROS_TOTAL_BULTOS_EXPORTACION_NORMAL",
        '                                 New Dictionary(Of String, String) From {{"S1.CA_MARCAS_NUMEROS_TOTAL_BULTOS.0",
        '                                                                            GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS1,
        '                                                                                              CamposPedimento.CA_MARCAS_NUMEROS_TOTAL_BULTOS)
        '                                                                         }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo marcas, numeros y total de bultos"

        '        End If



        '    End If

        'End If

        'Dim medioTransporteArribo_ As String = If(GetAttributeValue(pedimento_,
        '                                                 SeccionesPedimento.ANS1,
        '                                                 CamposPedimento.CA_MEDIO_TRANSPORTE_ARRIBO), "")

        'validation_ = _cubedatos.
        '                  RunRoom(Of String)("A22.CA_MEDIO_TRANSPORTE_ARRIBO",
        '                                     New Dictionary(Of String, String) From {{"S1.CA_MEDIO_TRANSPORTE_ARRIBO.0",
        '                                                                             medioTransporteArribo_
        '                                                                            }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el campo MEDIO DE TRANSPORTE DE ARRIBO'" & medioTransporteArribo_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim medioTransporteSalida_ As String = If(GetAttributeValue(pedimento_,
        '                                                 SeccionesPedimento.ANS1,
        '                                                 CamposPedimento.CA_MEDIO_TRANSPORTE_SALIDA), "")

        'validation_ = _cubedatos.
        '                  RunRoom(Of String)("A22.CA_MEDIO_TRANSPORTE_SALIDA",
        '                                     New Dictionary(Of String, String) From {{"S1.CA_MEDIO_TRANSPORTE_SALIDA.0",
        '                                                                             medioTransporteSalida_
        '                                                                            }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el campo MEDIO DE TRANSPORTE DE SALIDA '" & medioTransporteSalida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_RFC_IOE",
        '                                 New Dictionary(Of String, String) From {{"S3.CA_RFC_IOE.0",
        '                                                                            GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS3,
        '                                                                                              CamposPedimento.CA_RFC_IOE)
        '                                                                         }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo RFC DEL EXPORTADOR"

        '        End If



        '    End If

        'End If

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_CURP_IOE",
        '                                 New Dictionary(Of String, String) From {{"S3.CA_CURP_IOE.0",
        '                                                                            GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS3,
        '                                                                                              CamposPedimento.CA_CURP_IOE)
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo CURP DEL IMPORTADOR/EXPORTADOR"

        '        End If



        '    End If

        'End If

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_RAZON_SOCIAL_IOE",
        '                                 New Dictionary(Of String, String) From {{"S3.CA_RAZON_SOCIAL_IOE.0",
        '                                                                            GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS3,
        '                                                                                              CamposPedimento.CA_RAZON_SOCIAL_IOE, False)
        '                                                                         }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACIÓN SOCIAL DEL IMPORTADOR/EXPORTADOR"

        '        End If



        '    End If

        'End If

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_DOMICILIO_IOE",
        '                                 New Dictionary(Of String, String) From {{"S3.CA_DOMICILIO_IOE.0",
        '                                                                           If(GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS3,
        '                                                                                              CamposPedimento.CA_DOMICILIO_IOE, False), "")
        '                                                                         }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo DOMICILIO DEL IMPORTADOR/EXPORTADOR"

        '        End If

        '    End If

        'End If

        'Dim tipoOperacion_ As String = If(GetAttributeValue(pedimento_,
        '                                                 SeccionesPedimento.ANS1,
        '                                                 CamposPedimento.CA_TIPO_OPERACION), "")

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_TIPO_OPERACION",
        '                                 New Dictionary(Of String, String) From {{"S1.CA_TIPO_OPERACION.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS1,
        '                                                                                              CamposPedimento.CA_TIPO_OPERACION, False)
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el  campo TIPO DE OPERACIÓN '" & tipoOperacion_ & " valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cvePedimento_ = If(GetAttributeValue(pedimento_,
        '                                     SeccionesPedimento.ANS1,
        '                                     CamposPedimento.CA_CVE_PEDIMENTO, False).SubString(0, 2), "")

        'validation_ = _cubedatos.
        '      RunRoom(Of String)("A22.CA_CVE_PEDIMENTO",
        '                         New Dictionary(Of String, String) From {{"S1.CA_CVE_PEDIMENTO.0",
        '                                                                   cvePedimento_
        '                                                                 }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el  campo CVE PEDIMENTO '" & cvePedimento_ & " valor inválido"

        '        End If

        '    End If

        'End If



        'Dim cveRegimen_ = If(GetAttributeValue(pedimento_,
        '                                     SeccionesPedimento.ANS1,
        '                                     CamposPedimento.CA_REGIMEN, False).SubString(0, 3), "")
        'validation_ = _cubedatos.
        '      RunRoom(Of String)("A22.CA_REGIMEN",
        '                         New Dictionary(Of String, String) From {{"S1.CA_REGIMEN.0",
        '                                                                   cveRegimen_
        '                                                                 }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el  campo REGIMEN '" & cveRegimen_ & " valor inválido"

        '        End If

        '    End If

        'End If

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC",
        '                                 New Dictionary(Of String, String) From {{"S10.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS10,
        '                                                                                              CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_POC, False)
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACION O RAZON SOCIAL. DEL COMPRADOR"

        '        End If



        '    End If

        'End If

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_CFDI_FACTURA",
        '                                 New Dictionary(Of String, String) From {{"S10.CA_CFDI_FACTURA.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS10,
        '                                                                                              CamposPedimento.CA_CFDI_FACTURA)
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo NUM. CFDI O DOCUMENTO EQUIVALENTE."

        '        End If

        '    End If

        'End If

        'Dim facturaDate_ = GetAttributeValue(pedimento_,
        '                                    SeccionesPedimento.ANS13,
        '                                    CamposPedimento.CA_FECHA_FACTURA)

        'Dim validationDateValue_ = GetAttributeValue(pedimento_,
        '                                            SeccionesPedimento.ANS13,
        '                                            CamposPedimento.CA_FECHA_FACTURA)
        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_FECHA_FACTURA",
        '                                 New Dictionary(Of String, String) From {{"S13.CA_FECHA_FACTURA.0",
        '                                                                           facturaDate_
        '                                                                         },
        '                                                                         {"S1.CA_FECHA_VALIDACION.0",
        '                                                                           validationDateValue_
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el campo FECHA DE FACTURA '" &
        '                                  If(facturaDate_ Is Nothing, "", facturaDate_) &
        '                                  "' debe ser menor o igual a la FECHA DE VALIDACIÓN '" &
        '                                  If(validationDateValue_ Is Nothing, "", validationDateValue_) & "'"

        '        End If



        '    End If

        'End If

        'Dim cveMonedaFactura = GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS13,
        '                                                                                              CamposPedimento.CA_CVE_MONEDA_FACTURA)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_CVE_MONEDA_FACTURA",
        '                                 New Dictionary(Of String, String) From {{"S13.CA_CVE_MONEDA_FACTURA.0",
        '                                                                           cveMonedaFactura
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Valor inválido para campo MONEDA FACT. ('" & cveMonedaFactura & "')"

        '        End If



        '    End If

        'End If

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_MONTO_MONEDA_FACTURA",
        '                                 New Dictionary(Of String, String) From {{"S13.CA_MONTO_MONEDA_FACTURA.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS13,
        '                                                                                              CamposPedimento.CA_MONTO_MONEDA_FACTURA)
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo VAL. MON. FACT."

        '        End If



        '    End If

        'End If



        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_ACUSE_ELECTRONICO_VALIDACION_NORMAL_EXPORTACION",
        '                                 New Dictionary(Of String, String) From {{"S1.CA_ACUSE_ELECTRONICO_VALIDACION.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS1,
        '                                                                                              CamposPedimento.CA_ACUSE_ELECTRONICO_VALIDACION)
        '                                                                         },
        '                                                                         {"S1.CA_CVE_PEDIMENTO.0",
        '                                                                           cvePedimento_
        '                                                                         },
        '                                                                         {"SFAC1.CP_APLICA_ENAJENACION.0",
        '                                                                          "SI"
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo NÚMERO DE ACUSE DE VALOR"

        '        End If

        '    End If

        'End If


        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_INCOTERM",
        '                                 New Dictionary(Of String, String) From {{"S13.CA_INCOTERM.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS13,
        '                                                                                              CamposPedimento.CA_INCOTERM)
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo INCOTERM."

        '        End If

        '    End If

        'End If

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO",
        '                                 New Dictionary(Of String, String) From {{"S11.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS11,
        '                                                                                              CamposPedimento.CA_NOMBRE_RAZON_SOCIAL_DESTINATARIO, False)
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACION O RAZON SOCIAL. DEL DESTINATARIO"

        '        End If

        '    End If

        'End If


        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA",
        '                                 New Dictionary(Of String, String) From {{"S44.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS44,
        '                                                                                              CamposPedimento.CA_NOMBRE_DENOMINACION_RAZON_SOCIAL_AA, False)
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Falta especificar el valor del campo NOMBRE, DENOMINACIÓN O RAZ. SOC.. DEL AGENTE ADUANAL"

        '        End If

        '    End If

        'End If

        'Dim fechaPago_ = GetAttributeValue(pedimento_,
        '                                   SeccionesPedimento.ANS14,
        '                                   CamposPedimento.CA_FECHA_PAGO, False)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_FECHA_PAGO",
        '                                 New Dictionary(Of String, String) From {{"S14.CA_FECHA_PAGO.0",
        '                                                                           fechaPago_
        '                                                                         },
        '                                                                         {"S1.CA_FECHA_VALIDACION.0",
        '                                                                           validationDateValue_
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            If fechaPago_ IsNot Nothing Then

        '                message_ &= Chr(13) & "Error en el valor del campo FECHA PAGO. '" &
        '                                      fechaPago_ & "'" & Chr(13) &
        '                                      " debe ser igual a la fecha de validación '" &
        '                                      validationDateValue_ & "'"

        '            Else

        '                message_ &= Chr(13) & "Error en el valor del campo FECHA PAGO. ''"


        '            End If


        '        End If

        '    End If

        'End If

        ''AQUI SE HARÁ UN CICLO PARA REVISAR TODA LA SECCIÓN YA QUE PUEDE HABER VARIOS IDENTIFICADOR A NIVEL PEDIMENTO

        'Dim cveIdentificador_ As String = GetAttributeValue(pedimento_,
        '                                          SeccionesPedimento.ANS18,
        '                                          CamposPedimento.CA_CVE_IDENTIFICADOR)

        'cveIdentificador_ = If(cveIdentificador_ Is Nothing, "", cveIdentificador_)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_CVE_IDENTIFICADOR",
        '                                 New Dictionary(Of String, String) From {{"S18.CA_CVE_IDENTIFICADOR.0",
        '                                                                          cveIdentificador_
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '            Dim complemento1_ As String = If(GetAttributeValue(pedimento_,
        '                                                  SeccionesPedimento.ANS18,
        '                                                  CamposPedimento.CA_COMPLEMENTO_1), "")


        '            validation_ = _cubedatos.
        '                          RunRoom(Of String)("A22.CA_COMPLEMENTO_1",
        '                                             New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_1.0",
        '                                                                                       complemento1_
        '                                                                                     },
        '                                                                                      {"S18.CA_CVE_IDENTIFICADOR.0",
        '                                                                                      cveIdentificador_
        '                                                                                     }})
        '            If validation_.result IsNot Nothing Then

        '                If validation_.result.Count > 0 Then

        '                    If validation_.result(0) = "OK" Then

        '                    Else

        '                        If complemento1_ = "" Then


        '                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 1 para el identificador '" &
        '                                                       cveIdentificador_ &
        '                                                       "' el complemento no debe estar vacío"


        '                        Else

        '                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 1 valor inválido '" &
        '                                                   complemento1_ &
        '                                                   "' para el identificador '" &
        '                                                   cveIdentificador_ &
        '                                                   "' el complemento debe estar vacío"

        '                        End If


        '                    End If

        '                End If

        '            End If

        '            Dim complemento2_ = If(GetAttributeValue(pedimento_,
        '                                                  SeccionesPedimento.ANS18,
        '                                                  CamposPedimento.CA_COMPLEMENTO_2), "")

        '            validation_ = _cubedatos.
        '                          RunRoom(Of String)("A22.CA_COMPLEMENTO_2",
        '                                             New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_2.0",
        '                                                                                      complemento2_
        '                                                                                     },
        '                                                                                      {"S18.CA_CVE_IDENTIFICADOR.0",
        '                                                                                      cveIdentificador_
        '                                                                                     }})
        '            If validation_.result IsNot Nothing Then

        '                If validation_.result.Count > 0 Then

        '                    If validation_.result(0) = "OK" Then

        '                    Else

        '                        If complemento2_ = "" Then

        '                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 2 para el identificador '" &
        '                                                   cveIdentificador_ &
        '                                                   "' el complemento no debe estar vacío"

        '                        Else

        '                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 2 valor inválido '" &
        '                                                       complemento2_ &
        '                                                       "' para el identificador '" &
        '                                                       cveIdentificador_ &
        '                                                       "' el complemento debe estar vacío"

        '                        End If

        '                    End If

        '                End If

        '            End If

        '            Dim complemento3_ = If(GetAttributeValue(pedimento_,
        '                                                  SeccionesPedimento.ANS18,
        '                                                  CamposPedimento.CA_COMPLEMENTO_3), "")

        '            validation_ = _cubedatos.
        '                          RunRoom(Of String)("A22.CA_COMPLEMENTO_3",
        '                                             New Dictionary(Of String, String) From {{"S18.CA_COMPLEMENTO_3.0",
        '                                                                                       complemento3_
        '                                                                                     },
        '                                                                                      {"S18.CA_CVE_IDENTIFICADOR.0",
        '                                                                                      cveIdentificador_
        '                                                                                     }})
        '            If validation_.result IsNot Nothing Then

        '                If validation_.result.Count > 0 Then

        '                    If validation_.result(0) = "OK" Then

        '                    Else


        '                        If complemento3_ = "" Then

        '                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 3 para el identificador '" &
        '                                                   cveIdentificador_ &
        '                                                   "' el complemento no debe estar vacío"

        '                        Else

        '                            message_ &= Chr(13) & "Error en el campo COMPLEMENTO 3 valor inválido '" &
        '                                                   complemento3_ &
        '                                                   "' para el identificador '" &
        '                                                   cveIdentificador_ & "' el complemento debe estar vacío"

        '                        End If


        '                    End If

        '                End If

        '            End If

        '        Else


        '            message_ &= Chr(13) & "Error en el campo CLAVE. IDENTIFICADOR. Valor inválido '" &
        '                                       cveIdentificador_ & "'"

        '        End If

        '    End If

        'End If



        'Dim fechapresentacion_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS14,
        '                                           CamposPedimento.CA_FECHA_PRESENTACION)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_FECHA_PRESENTACION",
        '                                 New Dictionary(Of String, String) From {{"S14.CA_FECHA_PRESENTACION.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                                                                              SeccionesPedimento.ANS14,
        '                                                                                              CamposPedimento.CA_FECHA_PRESENTACION)
        '                                                                         },
        '                                                                          {"S14.CA_FECHA_PAGO.0",
        '                                                                          fechaPago_
        '                                                                         }})
        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el valor del campo FECHA PRESENTACION '" & fechapresentacion_ & "'" & Chr(13) & "('La fecha de presentación debe ser igual a la fecha de pago')"

        '        End If

        '    End If

        'End If

        'Dim fechaExtraccion_ = GetAttributeValue(pedimento_,
        '                                        SeccionesPedimento.ANS14,
        '                                        CamposPedimento.CA_FECHA_PRESENTACION)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_FECHA_EXTRACCION",
        '                                 New Dictionary(Of String, String) From {{"S14.CA_FECHA_EXTRACCION.0",
        '                                                                          fechaExtraccion_
        '                                                                         },
        '                                                                          {"S14.CA_FECHA_PAGO.0",
        '                                                                          fechaPago_
        '                                                                         },
        '                                                                          {"S1.CA_CVE_PEDIMENTO.0",
        '                                                                          cvePedimento_
        '                                                                         }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            If fechaExtraccion_ > fechaPago_ Then

        '                message_ &= Chr(13) & "Error en el valor del campo FECHA EXTRACCIÓN '" &
        '                                      If(fechaExtraccion_ Is Nothing, "", fechaExtraccion_) &
        '                                     "'" & Chr(13) & "(' debe ser menor o igual a la fecha de PAGO '" & fechaPago_ & "')"

        '            Else

        '                message_ &= Chr(13) & "Error en el valor del campo FECHA EXTRACCIÓN '" &
        '                                     If(fechaExtraccion_ Is Nothing, "", fechaExtraccion_) &
        '                                     "'" & Chr(13) & "('La fecha de EXTRACCIÓN no es compatible con la clave de pedimento puesta '" &
        '                                     If(cvePedimento_ Is Nothing, "", cvePedimento_) & "')"

        '            End If

        '        End If

        '    End If

        'End If

        'Dim idTransporte_ = GetAttributeValue(pedimento_,
        '                                      SeccionesPedimento.ANS12,
        '                                      CamposPedimento.CA_ID_TRANSPORTE)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_ID_TRANSPORTE",
        '                                 New Dictionary(Of String, String) From {{"S12.CA_ID_TRANSPORTE.0",
        '                                                                           idTransporte_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el valor del campo CA_ID_TRANSPORTE '" &
        '                                   If(idTransporte_ Is Nothing, "", idTransporte_) & "')"

        '        End If

        '    End If

        'End If

        'Dim cvePaisTransporte_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS12,
        '                                           CamposPedimento.CA_CVE_PAIS_TRANSPORTE)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_CVE_PAIS_TRANSPORTE",
        '                                 New Dictionary(Of String, String) From {{"S12.CA_CVE_PAIS_TRANSPORTE.0",
        '                                                                           cvePaisTransporte_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el valor del campo CLAVE DEL PAIS DE TRANSPORTE '" &
        '                                  If(cvePaisTransporte_ Is Nothing, "", cvePaisTransporte_) & "')"

        '        End If

        '    End If

        'End If

        'Dim numeroCandado_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS15,
        '                                           CamposPedimento.CA_NUMERO_CANDADO)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_NUMERO_CANDADO",
        '                                 New Dictionary(Of String, String) From {{"S15.CA_NUMERO_CANDADO.0",
        '                                                                           numeroCandado_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el valor del campo NÚMERO DE CANDADO ''"

        '        End If

        '    End If

        'End If

        'Dim guiaManifiestoBL_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS16,
        '                                           CamposPedimento.CA_GUIA_MANIFIESTO_BL)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_GUIA_MANIFIESTO_BL_NORMAL_EXPORTACION",
        '                                 New Dictionary(Of String, String) From {{"S16.CA_GUIA_MANIFIESTO_BL.0",
        '                                                                           guiaManifiestoBL_
        '                                                                        },
        '                                                                        {"S1.CA_MEDIO_TRANSPORTE.0",
        '                                                                           medioTransporte_
        '                                                                         }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            If If(guiaManifiestoBL_ Is Nothing, "", guiaManifiestoBL_) = "" Then

        '                message_ &= Chr(13) & "Error en el valor del campo NUMERO (GUIA/CONOCIMIENTO EMBARQUE) '' " & Chr(13) &
        '                                      " no debe ir vacio para el medio de transporte '" &
        '                                      If(medioTransporte_ Is Nothing, "", medioTransporte_) & "'"

        '            Else


        '                message_ &= Chr(13) & "Error en el valor del campo NUMERO (GUIA/CONOCIMIENTO EMBARQUE) '" & guiaManifiestoBL_ & "'" & Chr(13) &
        '                                      "debe ir vacio para el medio de transporte '" &
        '                                      If(medioTransporte_ Is Nothing, "", medioTransporte_) & "'"


        '            End If


        '        End If

        '    End If

        'End If

        'Dim masterHouse_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS16,
        '                                           CamposPedimento.CA_MASTER_HOUSE)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_MASTER_HOUSE",
        '                                 New Dictionary(Of String, String) From {{"S16.CA_MASTER_HOUSE.0",
        '                                                                           masterHouse_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en el valor del campo MASTER HOUSE '" &
        '                                  If(masterHouse_ Is Nothing, "", masterHouse_) & "'"

        '        End If

        '    End If

        'End If



        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO",
        '                                 New Dictionary(Of String, String) From {{"S17.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO.0",
        '                                                                           GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS17,
        '                                           CamposPedimento.CA_NUMERO_CONTENEDOR_FERROCARRIL_NUMERO_ECONOMICO)
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo NUMERO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO. 'está vacío'"


        '        End If

        '    End If

        'End If

        'Dim cveTipoContenedor_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS17,
        '                                           CamposPedimento.CA_CVE_TIPO_CONTENEDOR)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_CVE_TIPO_CONTENEDOR",
        '                                 New Dictionary(Of String, String) From {{"S17.CA_CVE_TIPO_CONTENEDOR.0",
        '                                                                           cveTipoContenedor_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo TIPO DE CONTENEDOR/EQUIPO FERROCARRIL/NUMERO ECONOMICO DEL VEHICULO. '" &
        '                                  If(cveTipoContenedor_ Is Nothing, "", cveTipoContenedor_) & "' valor inválido"


        '        End If

        '    End If

        'End If

        'Dim observacionesPedimento_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS23,
        '                                           CamposPedimento.CA_OBSERVACIONES_PEDIMENTO)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_OBSERVACIONES_PEDIMENTO",
        '                                 New Dictionary(Of String, String) From {{"S23.CA_OBSERVACIONES_PEDIMENTO.0",
        '                                                                           observacionesPedimento_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo OBSERVACIONES. '" &
        '                                  If(observacionesPedimento_ Is Nothing, "", observacionesPedimento_) & "' valor inválido"


        '        End If

        '    End If

        'End If

        'Dim numeroPEdimentoOriginal7Digitos_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS20,
        '                                           CamposPedimento.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS_NORMAL_EXPORTACION",
        '                                 New Dictionary(Of String, String) From {{"S20.CA_NUMERO_PEDIMENTO_ORIGINAL_7_DIGITOS.0",
        '                                                                           numeroPEdimentoOriginal7Digitos_
        '                                                                        }, {"S1.CA_CVE_PEDIMENTO.0",
        '                                                                           cvePedimento_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo NUMERO DE PEDIMENTO ORIGINAL 7 DIGITOS '" &
        '                                  If(numeroPEdimentoOriginal7Digitos_ Is Nothing, "", numeroPEdimentoOriginal7Digitos_) & "' valor inválido"


        '        End If

        '    End If

        'End If

        'Dim fechaPedimentoOriginal_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS20,
        '                                           CamposPedimento.CA_FECHA_PEDIMENTO_ORIGINAL)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_FECHA_PEDIMENTO_ORIGINAL_NORMAL_EXPORTACION",
        '                                 New Dictionary(Of String, String) From {{"S20.CA_FECHA_PEDIMENTO_ORIGINAL.0",
        '                                                                           fechaPedimentoOriginal_
        '                                                                        }, {"S1.CA_CVE_PEDIMENTO.0",
        '                                                                           cvePedimento_
        '                                                                        }, {"S14.CA_FECHA_PAGO.0",
        '                                                                           fechaPago_
        '                                                                        }
        '                                                                        })

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo FECHA DE OPERACIÓN ORIGINAL '" &
        '                                  If(fechaPedimentoOriginal_ Is Nothing, "", fechaPedimentoOriginal_) & Chr(13) &
        '                                  "' relación inválida con CLAVE DE PEDIMENTO '" &
        '                                  If(cvePedimento_ Is Nothing, "", cvePedimento_) &
        '                                  "'y FECHA DE PAGO '" &
        '                                  If(fechaPago_ Is Nothing, "", fechaPago_)




        '        End If

        '    End If

        'End If

        'Dim cvePedimentoOriginal_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS20,
        '                                           CamposPedimento.CA_CVE_PEDIMENTO_ORIGINAL)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_CVE_PEDIMENTO_ORIGINAL_NORMAL_EXPORTACION",
        '                                 New Dictionary(Of String, String) From {{"S20.CA_CVE_PEDIMENTO_ORIGINAL.0",
        '                                                                           cvePedimentoOriginal_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CVE. PEDIMENTO ORIGINAL '" &
        '                                  If(cvePedimentoOriginal_ Is Nothing, "", cvePedimentoOriginal_) &
        '                                  "' valor inválido"




        '        End If

        '    End If

        'End If


        'Dim aduanaDespachoOriginal_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS20,
        '                                           CamposPedimento.CA_ADUANA_DESPACHO_ORIGINAL)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_ADUANA_DESPACHO_ORIGINAL_NORMAL_EXPORTACION",
        '                                 New Dictionary(Of String, String) From {{"S20.CA_ADUANA_DESPACHO_ORIGINAL.0",
        '                                                                           aduanaDespachoOriginal_
        '                                                                        }, {"S1.CA_CVE_PEDIMENTO.0",
        '                                                                           cvePedimento_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo ADUANA DESPACHO ORIGINALL '" &
        '                                  If(aduanaDespachoOriginal_ Is Nothing, "", aduanaDespachoOriginal_) &
        '                                  "' valor inválido con CLAVE DE PEDIMENTO '" & cvePedimento_ & "'"




        '        End If

        '    End If

        'End If

        'Dim patenteOriginal_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS29,
        '                                           CamposPedimento.CA_PATENTE_ORIGINAL)

        'validation_ = _cubedatos.
        '              RunRoom(Of String)("A22.CA_PATENTE_ORIGINAL_NORMAL_EXPORTACION",
        '                                 New Dictionary(Of String, String) From {{"S29.CA_PATENTE_ORIGINAL.0",
        '                                                                           patenteOriginal_
        '                                                                        }, {"S1.CA_CVE_PEDIMENTO.0",
        '                                                                           cvePedimento_
        '                                                                        }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo PATENTE ORIGINAL '" &
        '                                  If(patenteOriginal_ Is Nothing, "", patenteOriginal_) &
        '                                  "' valor inválido con CLAVE DE PEDIMENTO '" & cvePedimento_ & "'"




        '        End If

        '    End If

        'End If

        'Dim fraccionOriginal_ As String = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS20,
        '                                           CamposPedimento.CA_FRACCION_ORIGINAL)

        'fraccionOriginal_ = If(fraccionOriginal_ Is Nothing, "", fraccionOriginal_.Replace(".", ""))

        'Dim nicoPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_NICO_PARTIDA)


        'nicoPartida_ = If(nicoPartida_ Is Nothing, "", nicoPartida_)

        'If fraccionOriginal_ = "" Then

        '    message_ &= Chr(13) & "Error en campo FRACCIÓN ORIGINAL '' valor inválido "

        'Else

        '    validation_ = _cubedatos.
        '                  RunRoom(Of String)("A22.CA_FRACCION_ORIGINAL",
        '                                     New Dictionary(Of String, String) From {{"S20.CA_FRACCION_ORIGINAL.0",
        '                                                                               fraccionOriginal_
        '                                                                            }})

        '    If validation_.result IsNot Nothing Then

        '        If validation_.result.Count > 0 Then

        '            If validation_.result(0) = "OK" Then

        '                If If(nicoPartida_ Is Nothing, "", nicoPartida_) = "" Then

        '                    message_ &= Chr(13) & "Error en campo SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL '' valor inválido "

        '                Else



        '                    validation_ = _cubedatos.
        '                                  RunRoom(Of String)("A22.CA_NICO_PARTIDA",
        '                                                     New Dictionary(Of String, String) From {{"S24.CA_NICO_PARTIDA.0",
        '                                                                                               nicoPartida_
        '                                                                                            }, {"S20.CA_FRACCION_ORIGINAL.0",
        '                                                                                               fraccionOriginal_
        '                                                                                            }})

        '                    If validation_.result IsNot Nothing Then

        '                        If validation_.result.Count > 0 Then

        '                            If validation_.result(0) = "OK" Then

        '                            Else

        '                                message_ &= Chr(13) & "Error en campo SUBD. / NÚM. IDENTIFICACIÓN COMERCIAL '" &
        '                                                       nicoPartida_ & "'" & Chr(13) &
        '                                                       " valor inválido para FRACCIÓN '" & fraccionOriginal_ & "'"




        '                            End If

        '                        End If

        '                    End If

        '                End If

        '            Else

        '                message_ &= Chr(13) & "Error en campo FRACCIÓN ORIGINAL '" &
        '                                       fraccionOriginal_ &
        '                                      "' valor inválido "




        '            End If

        '        End If

        '    End If

        'End If





        'Dim cveVinculacion_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS10,
        '                                           CamposPedimento.CA_CVE_VINCULACION)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_VINCULACION_NORMAL_EXPORTACION",
        '                                New Dictionary(Of String, String) From {{"S10.CA_CVE_VINCULACION.0",
        '                                                                          cveVinculacion_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CLAVE VINCULACIÓN '" &
        '                                       cveVinculacion_ & "' valor inválido"




        '        End If

        '    End If

        'End If

        'Dim metodotoValoracionPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS10,
        '                                           CamposPedimento.CA_CVE_VINCULACION)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_METODO_VALORACION_PARTIDA_NORMAL_EXPORTACION",
        '                                New Dictionary(Of String, String) From {{"S24.CA_CVE_METODO_VALORACION_PARTIDA.0",
        '                                                                          metodotoValoracionPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo MÉTODO DE VALORACIÓN'" &
        '                                       metodotoValoracionPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cveUMCPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_CVE_UMC_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_UMC_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_CVE_UMC_PARTIDA.0",
        '                                                                          cveUMCPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo UNIDAD DE MEDIDA COMERCIAL'" &
        '                                       cveUMCPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cantidadUMCPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_CANTIDAD_UMC_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CANTIDAD_UMC_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_CANTIDAD_UMC_PARTIDA.0",
        '                                                                          cantidadUMCPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CANTIDAD UMC'" &
        '                                       cantidadUMCPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cveUMTPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_CVE_UMT_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_UMT_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_CVE_UMT_PARTIDA.0",
        '                                                                          cveUMTPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo UNIDAD DE MEDIDA TARIFA'" &
        '                                       cveUMTPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cantidadUMTPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_CANTIDAD_UMT_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CANTIDAD_UMT_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_CANTIDAD_UMT_PARTIDA.0",
        '                                                                          cantidadUMTPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CANTIDAD UMT'" &
        '                                       cantidadUMTPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cvePaisCompradorPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_CVE_PAIS_COMPRADOR_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_PAIS_COMPRADOR_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_CVE_PAIS_COMPRADOR_PARTIDA.0",
        '                                                                          cvePaisCompradorPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CLAVE PAIS COMPRADOR PARTIDA'" &
        '                                       cvePaisCompradorPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cvePaisDestinoPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_CVE_PAIS_DESTINO_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_PAIS_DESTINO_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_CVE_PAIS_DESTINO_PARTIDA.0",
        '                                                                          cvePaisDestinoPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CLAVE PAIS DESTINO PARTIDA'" &
        '                                       cvePaisDestinoPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim descripcionMercanciaPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_DESCRIPCION_MERCANCIA_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_DESCRIPCION_MERCANCIA_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_DESCRIPCION_MERCANCIA_PARTIDA.0",
        '                                                                          descripcionMercanciaPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo DESCRIPCIÓN MERCANCIA PARTIDA'" &
        '                                       descripcionMercanciaPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cveContribucionPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS29,
        '                                           CamposPedimento.CA_CVE_CONTRIBUCION_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_CONTRIBUCION_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S29.CA_CVE_CONTRIBUCION_PARTIDA.0",
        '                                                                          cveContribucionPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CONTRIBUCIÓN A NIVEL PARTIDA'" &
        '                                       cveContribucionPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cveTipoTasaPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS29,
        '                                           CamposPedimento.CA_CVE_TIPO_TASA_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_TIPO_TASA_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S29.CA_CVE_TIPO_TASA_PARTIDA.0",
        '                                                                          cveTipoTasaPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CLAVE TIPO DE TASA PARTIDA'" &
        '                                       cveTipoTasaPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim tasaPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS29,
        '                                           CamposPedimento.CA_TASA_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_TASA_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S29.CA_TASA_PARTIDA.0",
        '                                                                          tasaPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo TASA PARTIDA'" &
        '                                       tasaPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim formaPagoPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS29,
        '                                           CamposPedimento.CA_FORMA_PAGO_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_FORMA_PAGO_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S29.CA_FORMA_PAGO_PARTIDA.0",
        '                                                                          formaPagoPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo FORMA DE PAGO PARTIDA'" &
        '                                       formaPagoPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim valorAgregadoPartida_ As String = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_VALOR_AGREGADO_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_VALOR_AGREGADO_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_VALOR_AGREGADO_PARTIDA.0",
        '                                                                          valorAgregadoPartida_},
        '                                                                          {"S1.CA_CVE_PEDIMENTO.0",
        '                                                                          cvePedimento_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else



        '            message_ &= Chr(13) & "Error en campo VALOR AGREGADO PARTIDA'" &
        '                                       valorAgregadoPartida_ & "' valor inválido " & If(valorAgregadoPartida_ <> "", " Si la CVE PEDIMENTO no es RT no se llena", "")

        '        End If

        '    End If

        'End If

        'Dim marcaPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS29,
        '                                           CamposPedimento.CA_MARCA_PARTIDA)

        'Dim regimen_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS1,
        '                                           CamposPedimento.CA_REGIMEN)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_MARCA_PARTIDA_NORMAL_EXPORTACION",
        '                                New Dictionary(Of String, String) From {{"S29.CA_MARCA_PARTIDA.0",
        '                                                                          marcaPartida_},
        '                                                                          {"S1.CA_REGIMEN.0",
        '                                                                          regimen_
        '                                                                       },
        '                                                                          {"S20.CA_FRACCION_ORIGINAL.0",
        '                                                                          fraccionOriginal_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo MARCA PARTIDA'" &
        '                                       marcaPartida_ & "' valor inválido pára la relación " & Chr(13) &
        '                                       "REGIMEN '" & regimen_ & "' FRACCIÓN ORIGINAL '" & fraccionOriginal_

        '        End If

        '    End If

        'End If

        'Dim modeloPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS29,
        '                                           CamposPedimento.CA_MODELO_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_MODELO_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S29.CA_MODELO_PARTIDA.0",
        '                                                                          modeloPartida_},
        '                                                                          {"S24.CA_NICO_PARTIDA.0",
        '                                                                          nicoPartida_
        '                                                                       },
        '                                                                          {"S20.CA_FRACCION_ORIGINAL.0",
        '                                                                          fraccionOriginal_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            If modeloPartida_ = "" Then

        '                message_ &= Chr(13) & "Error en campo MODELO PARTIDA'" &
        '                                       modeloPartida_ & "' valor inválido pára la relación " & Chr(13) &
        '                                       "NICO '" & nicoPartida_ & "' FRACCIÓN ORIGINAL '" & fraccionOriginal_
        '            Else

        '                message_ &= Chr(13) & "Error en campo MODELO PARTIDA'" &
        '                                       modeloPartida_ & "' valor inválido dee estar vacío pára la relación " & Chr(13) &
        '                                       "NICO '" & nicoPartida_ & "' FRACCIÓN ORIGINAL '" & fraccionOriginal_
        '            End If



        '        End If

        '    End If

        'End If

        'Dim codigoProductoPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS24,
        '                                           CamposPedimento.CA_CODIGO_PRODUCTO_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CODIGO_PRODUCTO_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S24.CA_CODIGO_PRODUCTO_PARTIDA.0",
        '                                                                          codigoProductoPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CODIGO PRODUCTO PARTIDA'" &
        '                                       codigoProductoPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim numeroSeriePartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS25,
        '                                           CamposPedimento.CA_VINCULACION_NUMERO_SERIE_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_VINCULACION_NUMERO_SERIE_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S25.CA_VINCULACION_NUMERO_SERIE_PARTIDA.0",
        '                                                                          numeroSeriePartida_
        '                                                                       }, {"S24.CA_NICO_PARTIDA.0",
        '                                                                          nicoPartida_
        '                                                                       },
        '                                                                          {"S20.CA_FRACCION_ORIGINAL.0",
        '                                                                          fraccionOriginal_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo NUMERO SERIE PARTIDA'" &
        '                                       numeroSeriePartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim kilometrajePartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS25,
        '                                           CamposPedimento.CA_KILOMETRAJE_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_KILOMETRAJE_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S25.CA_KILOMETRAJE_PARTIDA.0",
        '                                                                          kilometrajePartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo KILOMETRAJE PARTIDA'" &
        '                                       kilometrajePartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cvePermiso_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS26,
        '                                           CamposPedimento.CA_CVE_PERMISO)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_PERMISO",
        '                                New Dictionary(Of String, String) From {{"S26.CA_CVE_PERMISO.0",
        '                                                                          cvePermiso_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CVE PERMISO'" &
        '                                       cvePermiso_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim numeroPermiso_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS26,
        '                                           CamposPedimento.CA_NUMERO_PERMISO)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_NUMERO_PERMISO",
        '                                New Dictionary(Of String, String) From {{"S26.CA_NUMERO_PERMISO.0",
        '                                                                          numeroPermiso_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo NUMERO DE PERMISO'" &
        '                                       numeroPermiso_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim firmaDescargo_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS26,
        '                                           CamposPedimento.CA_FIRMA_ELECTRONICA_PERMISO)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_FIRMA_ELECTRONICA_PERMISO",
        '                                New Dictionary(Of String, String) From {{"S26.CA_FIRMA_ELECTRONICA_PERMISO.0",
        '                                                                          firmaDescargo_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo FIRMA DESCARGO'" &
        '                                       firmaDescargo_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim cveIdentificadorPartida_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS27,
        '                                           CamposPedimento.CA_CVE_IDENTIFICADOR_PARTIDA)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_IDENTIFICADOR_PARTIDA",
        '                                New Dictionary(Of String, String) From {{"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
        '                                                                          cveIdentificadorPartida_
        '                                                                       }})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '            Dim complemento1Partida_ = GetAttributeValue(pedimento_,
        '                                                       SeccionesPedimento.ANS27,
        '                                                       CamposPedimento.CA_COMPLEMENTO_1_PARTIDA)

        '            validation_ = _cubedatos.
        '                         RunRoom(Of String)("A22.CA_COMPLEMENTO_1_PARTIDA",
        '                                            New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_1_PARTIDA.0",
        '                                                                                      complemento1Partida_},
        '                                                                                      {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
        '                                                                                      cveIdentificadorPartida_
        '                                                                                   }})

        '            If validation_.result IsNot Nothing Then

        '                If validation_.result.Count > 0 Then

        '                    If validation_.result(0) = "OK" Then

        '                    Else

        '                        message_ &= Chr(13) & "Error en campo COMPLEMENTO 1 A NIVEL PARTIDA'" &
        '                                                   complemento1Partida_ & "' valor inválido"

        '                    End If

        '                End If

        '            End If

        '            Dim complemento2Partida_ = GetAttributeValue(pedimento_,
        '                                                       SeccionesPedimento.ANS27,
        '                                                       CamposPedimento.CA_COMPLEMENTO_2_PARTIDA)

        '            validation_ = _cubedatos.
        '                         RunRoom(Of String)("A22.CA_COMPLEMENTO_2_PARTIDA",
        '                                            New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_2_PARTIDA.0",
        '                                                                                      complemento1Partida_},
        '                                                                                      {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
        '                                                                                      cveIdentificadorPartida_
        '                                                                                   }})

        '            If validation_.result IsNot Nothing Then

        '                If validation_.result.Count > 0 Then

        '                    If validation_.result(0) = "OK" Then

        '                    Else

        '                        message_ &= Chr(13) & "Error en campo COMPLEMENTO 2 A NIVEL PARTIDA'" &
        '                                                   complemento2Partida_ & "' valor inválido"

        '                    End If

        '                End If

        '            End If

        '            Dim complemento3Partida_ = GetAttributeValue(pedimento_,
        '                                                       SeccionesPedimento.ANS27,
        '                                                       CamposPedimento.CA_COMPLEMENTO_3_PARTIDA)

        '            validation_ = _cubedatos.
        '                         RunRoom(Of String)("A22.CA_COMPLEMENTO_3_PARTIDA",
        '                                            New Dictionary(Of String, String) From {{"S27.CA_COMPLEMENTO_3_PARTIDA.0",
        '                                                                                      complemento3Partida_},
        '                                                                                      {"S27.CA_CVE_IDENTIFICADOR_PARTIDA.0",
        '                                                                                      cveIdentificadorPartida_
        '                                                                                   }})

        '            If validation_.result IsNot Nothing Then

        '                If validation_.result.Count > 0 Then

        '                    If validation_.result(0) = "OK" Then

        '                    Else

        '                        message_ &= Chr(13) & "Error en campo COMPLEMENTO 3 A NIVEL PARTIDA'" &
        '                                                   complemento3Partida_ & "' valor inválido"

        '                    End If

        '                End If

        '            End If

        '        Else

        '            message_ &= Chr(13) & "Error en campo IDENTIFICADOR A NIVEL PARTIDA'" &
        '                                       cveIdentificadorPartida_ & "' valor inválido"

        '        End If

        '    End If

        'End If



        'Dim cvePrevalidador_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS1,
        '                                           CamposPedimento.CA_CVE_PREVALIDADOR)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_CVE_PREVALIDADOR",
        '                                New Dictionary(Of String, String) From {{"S1.CA_CVE_PREVALIDADOR.0",
        '                                                                          cvePrevalidador_}})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo CLAVE DEL PREVALIDADOR'" &
        '                                       cvePrevalidador_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'Dim nombreInstitucionBancaria_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS9,
        '                                           CamposPedimento.CA_NOMBRE_INSTITUCION_BANCARIA)

        'Dim formaPago_ = GetAttributeValue(pedimento_,
        '                                           SeccionesPedimento.ANS55,
        '                                           CamposPedimento.CA_FORMA_PAGO)

        'validation_ = _cubedatos.
        '             RunRoom(Of String)("A22.CA_NOMBRE_INSTITUCION_BANCARIA",
        '                                New Dictionary(Of String, String) From {{"S9.CA_NOMBRE_INSTITUCION_BANCARIA.0",
        '                                                                          nombreInstitucionBancaria_},
        '                                                                          {"S55.CA_FORMA_PAGO.0",
        '                                                                          formaPago_}})

        'If validation_.result IsNot Nothing Then

        '    If validation_.result.Count > 0 Then

        '        If validation_.result(0) = "OK" Then

        '        Else

        '            message_ &= Chr(13) & "Error en campo NOMBRE INSTITUCIÓN BANCARIA'" &
        '                                       nombreInstitucionBancaria_ & "' valor inválido"

        '        End If

        '    End If

        'End If

        'validation_.SetDetailReport(AdviceTypesReport.Information,
        '                             message_,
        '                             Chr(13) & "Ruta de Validación Exportación Normal por Defecto" & Chr(13) &
        '                             "Folio de Operación:" & pedimento_.FolioOperacion & Chr(13),
        '                             TriggerSourceTypes.Route
        '                             )

        Return New ValidatorReport

    End Function

    Function GetAttributeValue(pedimento_ As DocumentoElectronico, section_ As SeccionesPedimento, field_ As CamposPedimento, Optional valor_ As Boolean = True) As Object

        Dim attribute_ = pedimento_.Seccion(section_).Attribute(field_)

        'If field_ = CamposPedimento.CA_FRACCION_ORIGINAL Then

        '    Dim algo = attribute_

        'End If

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

