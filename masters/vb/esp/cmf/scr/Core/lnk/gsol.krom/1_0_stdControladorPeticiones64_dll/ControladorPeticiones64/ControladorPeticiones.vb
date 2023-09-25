Imports gsol.krom.IControladorPeticiones64.gsol.krom
Imports gsol.krom.IControladorPeticiones64
Imports Wma.Exceptions
Imports gsol.krom
Imports gsol.krom.controladores
Imports gsol.krom.Referencia.Vt022ConsultaOperaciones

Namespace gsol.krom

    Public Class ControladorPeticiones
        Implements IControladorPeticiones

#Region "Atributos"

        Private _tagWatcher As TagWatcher

        Private _controladorWeb As ControladorWeb

        Private _espacioTrabajoExtranet As IEspacioTrabajo

#End Region

#Region "Constructores"

        Sub New()

            _tagWatcher = New TagWatcher

            _espacioTrabajoExtranet = New EspacioTrabajo

        End Sub

#End Region

#Region "Metodos"

        'Public Function RealizarPeticion(peticion_ As Peticiones, controladorWeb_ As ControladorWeb) As List(Of Object) Implements IControladorPeticiones.RealizarPeticion

        '    If Not peticion_ Is Nothing Then

        '        Dim registroNuevo_ As IEntidadDatos

        '        'Localiza el tipo de granularidad donde debe enviar la petición
        '        Select Case peticion_.Granularidad

        '            Case IEnlaceDatos.TiposDimension.CuentaGastos

        '                registroNuevo_ = New CuentaGastos

        '            Case IEnlaceDatos.TiposDimension.ExtranetGeneralOperaciones

        '                registroNuevo_ = New Referencia

        '            Case IEnlaceDatos.TiposDimension.Referencias

        '                registroNuevo_ = New Referencia

        '            Case IEnlaceDatos.TiposDimension.Contenedores
        '                'Agregar a tagwacher
        '                _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_013_00003)

        '            Case IEnlaceDatos.TiposDimension.Fracciones

        '                registroNuevo_ = New Fracciones

        '            Case IEnlaceDatos.TiposDimension.Facturas

        '                registroNuevo_ = New Facturas

        '            Case IEnlaceDatos.TiposDimension.Mercancias

        '                registroNuevo_ = New Mercancias

        '            Case Else
        '                'Agregar a tagwacher
        '                _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_013_00003)

        '        End Select

        '        'Selecciona la dimención (vista) donde se encuentra el campo requerido
        '        registroNuevo_.Dimension = peticion_.Localizacion

        '        registroNuevo_.cmp(t_NumeroReferencia)

        '        _controladorWeb = controladorWeb_

        '        With controladorWeb_

        '            .EnlaceDatos.ModalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesAutomaticas

        '            .EnlaceDatos.TipoRespuestaRequerida = IEnlaceDatos.FormatosRespuesta.IOperaciones

        '            .EnlaceDatos.TipoGestionOperativa = IEnlaceDatos.TiposGestionOperativa.AccesoOperativo

        '            .CovertRuleToScript(_tagWatcher)

        '        End With

        '        If _tagWatcher.Status = TagWatcher.TypeStatus.Ok Then

        '            Dim tag_ As TagWatcher = controladorWeb_.EnlaceDatos.ObtieneEstructuraResultados(registroNuevo_, Nothing)

        '            Dim entidadDatosEstructura_ As List(Of IEntidadDatos) = Nothing

        '            If tag_.Status = TagWatcher.TypeStatus.Ok Then

        '                entidadDatosEstructura_ = tag_.ObjectReturned

        '                controladorWeb_.ListaAtributos = entidadDatosEstructura_(0).Atributos

        '                controladorWeb_.FiltrosAvanzados = peticion_.Clausula

        '                controladorWeb_.EnlaceDatos.TipoGestionOperativa = IEnlaceDatos.TiposGestionOperativa.AccesoOperativo

        '                controladorWeb_.EnlaceDatos.GeneraTransaccion(registroNuevo_, Nothing)

        '                Return controladorWeb_.EnlaceDatos.ObtenerListaRegistros

        '            End If

        '        Else

        '            With _tagWatcher
        '                'Agregar a tagwacher
        '                .SetError(TagWatcher.ErrorTypes.C1_000_0001)

        '            End With

        '        End If

        '    End If

        'End Function

#End Region

        Public Function RealizarPeticion(ByVal peticion_ As Peticiones, ByVal controladorWeb_ As ControladorWeb) As DataTable Implements IControladorPeticiones.RealizarPeticion

            If Not controladorWeb_ Is Nothing Then

                Dim estatus_ As New TagWatcher

                Dim pckCantidadOperaciones_ As IEntidadDatos

                Select Case peticion_.Granularidad

                    Case IEnlaceDatos.TiposDimension.CuentaGastos

                        pckCantidadOperaciones_ = New CuentaGastos

                    Case IEnlaceDatos.TiposDimension.ExtranetGeneralOperaciones

                        pckCantidadOperaciones_ = New Referencia

                    Case IEnlaceDatos.TiposDimension.Referencias

                        pckCantidadOperaciones_ = New Referencia

                    Case IEnlaceDatos.TiposDimension.Contenedores
                        'Agregar a tagwacher
                        _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_013_00003)

                    Case IEnlaceDatos.TiposDimension.Fracciones

                        pckCantidadOperaciones_ = New Fracciones

                    Case IEnlaceDatos.TiposDimension.Facturas

                        pckCantidadOperaciones_ = New Facturas

                    Case IEnlaceDatos.TiposDimension.Mercancias

                        pckCantidadOperaciones_ = New Mercancias

                    Case Else
                        'Agregar a tagwacher
                        _tagWatcher.SetError(TagWatcher.ErrorTypes.C5_013_00003)

                End Select

                'Dim pckCantidadOperaciones_ As IEntidadDatos = New Referencia

                With controladorWeb_

                    .EnlaceDatos.Granularidad = peticion_.Granularidad

                    .EnlaceDatos.ModalidadPresentacion = IEnlaceDatos.ModalidadPresentacionEncabezados.DescripcionesTecnicasCampo

                    .EnlaceDatos.LimiteResultados = 0

                    .EnlaceDatos.TipoRespuestaRequerida = IEnlaceDatos.FormatosRespuesta.IOperaciones

                    .FiltrosAvanzados = peticion_.Clausula

                End With

                With pckCantidadOperaciones_

                    'Campos para grafica de lineas
                    .cmp(nombreCampo_:=peticion_.CampoBusqueda)

                End With

                pckCantidadOperaciones_.Dimension = peticion_.Localizacion

                controladorWeb_.EnlaceDatos.TipoGestionOperativa = IEnlaceDatos.TiposGestionOperativa.AccesoOperativo

                controladorWeb_.CovertRuleToScript(estatus_)

                If estatus_.Status = Wma.Exceptions.TagWatcher.TypeStatus.Ok Then

                    controladorWeb_.EnlaceDatos.GeneraTransaccion(pckCantidadOperaciones_, Nothing)

                    If controladorWeb_.EnlaceDatos.Registros.Count > 0 Then

                        '_tablaResultado = controladorWeb_.EnlaceDatos.Tabla.Copy

                        Dim tablaResultado_ As DataTable = controladorWeb_.EnlaceDatos.Tabla.Copy

                        Return tablaResultado_

                    End If

                End If

            Else


            End If

        End Function

    End Class

End Namespace
