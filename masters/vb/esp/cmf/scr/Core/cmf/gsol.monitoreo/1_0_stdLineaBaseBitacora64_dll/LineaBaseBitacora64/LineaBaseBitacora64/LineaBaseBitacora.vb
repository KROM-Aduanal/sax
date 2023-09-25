Imports gsol.basededatos
Imports gsol.basedatos.Operaciones
Imports Wma.Exceptions
Imports gsol.seguridad

Namespace gsol.monitoreo

    Public Class LineaBaseBitacora
        Inherits Organismo

#Region "Atributos"

        Private _disparo As String
        Private _usuario As String
        Private _grupo As Int32
        Private _division As Int32
        Private _aplicacion As Int32
        Private _permiso As Int32
        Private _mensaje As String
        Private _parametro As String
        Private _tipoBitacora As IBitacoras.TiposBitacora
        Private _tipoSuceso As IBitacoras.TiposSucesos
        Private _estatusInsercion As IBitacoras.EstatusInsercion

        Public _f_FechaHoraInicio As New DateTime
        Public _f_FechaHoraFinal As New DateTime

        Public _i_DiaSemana As Int16
        Public _i_Mes As Int16
        Public _i_Anio As Int32
        Public _i_Cve_Usuario As Int32

        Public _i_Cve_RecursoSolicitante As Int32

        Public _i_Cve_DivisionMiEmpresa As Int32


        Public _i_Cve_Aplicacion As IBitacoras.ClaveTiposAplicacion

        Public _i_Instrumentacion As IBitacoras.TiposInstrumentacion
        Public _modalidadConsulta As IOperacionesCatalogo.ModalidadesConsulta

        Public _t_IP As String
        Public _t_Pais As String
        Public _t_EstadoCiudad As String
        Public _i_MemoriaRAMDisponibleGB As Double
        Public _i_MemoriaRAMTotalGB As Double

        Public _i_TiempoRespuestaTotal As Double

        Public _tagWatcherMensaje As TagWatcher

        Public _idTransaccion As String

        Public _t_NombreEnsamblado As String
        Public _t_NombreVT As String
        Public _t_NombreUsuario As String
        Public _t_CuentaUsuario As String

        Private _nombreBaseDatosBitacora As String

#End Region

#Region "Constructores"
        Sub New()
            _estatusInsercion = IBitacoras.EstatusInsercion.SinEstatus

            _f_FechaHoraInicio = Now
            _i_DiaSemana = Now.DayOfWeek
            _i_Mes = Now.Month
            _i_Anio = Now.Year
            _i_Cve_Usuario = 0

            _i_Cve_RecursoSolicitante = 0

            _i_Cve_DivisionMiEmpresa = 0


            _i_Cve_Aplicacion = IBitacoras.ClaveTiposAplicacion.SinDefinir

            _i_Instrumentacion = IBitacoras.TiposInstrumentacion.GestorIOperaciones
            _modalidadConsulta = IOperacionesCatalogo.ModalidadesConsulta.Singleton

            _t_IP = "0.0.0.0"
            _t_Pais = Nothing
            _t_EstadoCiudad = Nothing
            _i_MemoriaRAMDisponibleGB = 0
            _i_MemoriaRAMTotalGB = 0
            _i_TiempoRespuestaTotal = 0

            _tagWatcherMensaje = New TagWatcher

            _idTransaccion = Nothing

            Configuracion.ObtenerInstancia()

            _nombreBaseDatosBitacora = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.B1NombreSQLServer)

            ' _nombreBaseDatosBitacora = Configuracion.ConstanteGlobal(Configuracion.DatosGlobalesSistema.BaseDatosSQLServerProduccion)

        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Property Disparo As String
            Get
                Return _disparo
            End Get
            Set(ByVal value As String)
                _disparo = value
            End Set
        End Property

        Property Usuario As String
            Get
                Return _usuario
            End Get
            Set(ByVal value As String)
                _usuario = value
            End Set
        End Property

        Property Grupo As Int32
            Get
                Return _grupo
            End Get
            Set(ByVal value As Int32)
                _grupo = value
            End Set
        End Property

        Property Division As Int32
            Get
                Return _division
            End Get
            Set(ByVal value As Int32)
                _division = value
            End Set
        End Property

        Property Aplicacion As Int32
            Get
                Return _aplicacion
            End Get
            Set(ByVal value As Int32)
                _aplicacion = value
            End Set
        End Property

        Property Permiso As Int32
            Get
                Return _permiso
            End Get
            Set(value As Int32)
                _permiso = value
            End Set
        End Property

        Property Mensaje As String
            Get
                Return _mensaje
            End Get
            Set(value As String)
                _mensaje = value
            End Set
        End Property

        Property Parametros As String
            Get
                Return _parametro
            End Get
            Set(value As String)
                _parametro = value
            End Set
        End Property

        Property TipoBitacora As IBitacoras.TiposBitacora
            Get
                Return _tipoBitacora
            End Get
            Set(value As IBitacoras.TiposBitacora)
                _tipoBitacora = value
            End Set
        End Property

        Property TipoSuceso As IBitacoras.TiposSucesos
            Get
                Return _tipoSuceso
            End Get
            Set(value As IBitacoras.TiposSucesos)
                _tipoSuceso = value
            End Set
        End Property

        Property EstatusInsercion As IBitacoras.EstatusInsercion
            Get
                Return _estatusInsercion
            End Get
            Set(value As IBitacoras.EstatusInsercion)
                _estatusInsercion = value
            End Set
        End Property

#End Region

#Region "Metodos"

        Public Sub InsertaBitacora()

            Dim respuesta_ As String
            Dim CadenaSql_ As String

            Try

                ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Reset()
                ConexionSingleton.EstaConectado.ToString()

                '    ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(
                '    "sp011insertabitacora '" & _disparo & _
                '    "', '" & _usuario & _
                '    "', " & _grupo & _
                '    ", " & _division & _
                '    ", " & _tipoSuceso & _
                '    ", " & _permiso & _
                '    ", '" & _mensaje & _
                '    "', '" & _parametro & _
                '    "', " & _aplicacion & _
                '    ", " & _tipoBitacora & _
                '    ";"
                ')

                '    'If ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows.Count >= 1 Then
                '    respuesta_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)("t_EstadoInsercion").ToString
                '    If respuesta_ = 0 Then
                '        _estatusInsercion = IBitacoras.EstatusInsercion.Incorrecto
                '    End If
                '    If respuesta_ = 1 Then
                '        _estatusInsercion = IBitacoras.EstatusInsercion.Correcto
                '    End If

                '-----------------

                CadenaSql_ = Trim(_nombreBaseDatosBitacora) & ".dbo.sp011insertabitacora '" & _disparo & _
                            "', '" & _usuario & _
                            "', " & _grupo & _
                            ", " & _division & _
                            ", " & _tipoSuceso & _
                            ", " & _permiso & _
                            ", '" & _mensaje & _
                            "', '" & _parametro & _
                            "', " & _aplicacion & _
                            ", " & _tipoBitacora & _
                            ";"

                If ConexionSingleton.SQLServerSingletonConexion.EjecutaConsulta(CadenaSql_) Then
                    respuesta_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)("t_EstadoInsercion").ToString
                    If respuesta_ = 0 Then
                        _estatusInsercion = IBitacoras.EstatusInsercion.Incorrecto
                    End If
                    If respuesta_ = 1 Then
                        _estatusInsercion = IBitacoras.EstatusInsercion.Correcto
                    End If
                Else
                    _estatusInsercion = IBitacoras.EstatusInsercion.Incorrecto
                End If

                'Else
                ' implementar generacion de log local
                'End If

            Catch ex As Exception
                _estatusInsercion = IBitacoras.EstatusInsercion.Incorrecto
            End Try

        End Sub


        Public Sub InsertaBitacoraAvanzada(ByVal consulta_ As String,
                                           Optional ByVal EsOpenClose_ As Boolean = True)

            Dim respuesta_ As String = Nothing
            Dim CadenaSql_ As String = Nothing

            consulta_ = consulta_.Replace("'", "'+char(39) +'")

            _disparo = consulta_ ' "'Select * from campo1 as '+char(39) +'campo'+ char(39) +', campo2 as '+ char(39) + 'campo2' + char(39)"

            ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

            Dim t_Fechainicio_ As String = Nothing

            t_Fechainicio_ = _f_FechaHoraInicio.ToString("dd/MM/yy H:mm:ss.fff")

            Dim f_FechaFinal_ As String = Nothing

            CadenaSql_ =
                "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & Chr(13) & _
                "SET ARITHABORT ON; " & Chr(13) & _
                "BEGIN TRANSACTION;  " & Chr(13) & _
                " BEGIN TRY  " & Chr(13) & _
                "    -- Generate a constraint violation error.  " & Chr(13) & _
                "	SET NOCOUNT on;" & Chr(13) & _
                "	declare @IDBitacora integer = null;" & Chr(13) & _
                "	declare @IDInformacionSQL integer = null;" & Chr(13) & _
                "	insert into " & Trim(_nombreBaseDatosBitacora) & ".dbo.Reg011InformacionSQLAvanzadaConsultas" & Chr(13) & _
                "	(t_DisparoSQL,i_Cve_Estado)" & Chr(13) & _
                "                values " & Chr(13) & _
                "	('" & _disparo & "'" & Chr(13) & _
                "	,1);" & Chr(13) & _
                "	set @IDInformacionSQL =(select @@IDENTITY as IDInformacionSQL);" & Chr(13) & _
                "--Disparo para proceso" & Chr(13) & _
                "	insert into " & Trim(_nombreBaseDatosBitacora) & ".dbo.Reg011BitacoraAvanzadaConsultas" & Chr(13) & _
                "	(" & Chr(13) & _
                "	i_Cve_Usuario," & Chr(13) & _
 _
               "	t_NombreEnsamblado," & Chr(13) & _
                "	t_NombreVT," & Chr(13) & _
                "	t_NombreUsuario," & Chr(13) & _
                "	t_CuentaUsuario," & Chr(13) & _
 _
                "	i_Cve_DivisionMiEmpresa," & Chr(13) & _
                "	i_Cve_Suceso," & Chr(13) & _
                "	i_DiaSemana," & Chr(13) & _
                "	i_Mes," & Chr(13) & _
                "	i_Anio," & Chr(13) & _
                "	f_FechaHoraInicio," & Chr(13) & _
                "	i_Cve_InformacionSQL," & Chr(13) & _
                "	i_Cve_Aplicacion," & Chr(13) & _
                "	i_Cve_Estado," & Chr(13) & _
                "	i_Cve_Estatus," & Chr(13) & _
                "	i_Cve_TipoMensaje," & Chr(13) & _
                "	i_TiempoRespuestaTotal," & Chr(13) & _
                "	i_Instrumentacion," & Chr(13) & _
                "	i_Conectividad," & Chr(13) & _
                "	i_MemoriaRAMDisponibleGB," & Chr(13) & _
                "	i_MemoriaRAMTotalGB," & Chr(13) & _
                "	i_Cve_RecursoSolicitante," & Chr(13) & _
                "	t_IP," & Chr(13) & _
                "	t_Pais," & Chr(13) & _
                "	t_EstadoCiudad)" & Chr(13) & _
                " values " & Chr(13) & _
                "	(" & Chr(13) & _
                "	" & _i_Cve_Usuario & "," & Chr(13) & _
 _
                 "	'" & _t_NombreEnsamblado & "'," & Chr(13) & _
                 "	'" & _t_NombreVT & "'," & Chr(13) & _
                 "	'" & _t_NombreUsuario & "'," & Chr(13) & _
                 "	'" & _t_CuentaUsuario & "'," & Chr(13) & _
 _
               "	" & _i_Cve_DivisionMiEmpresa & "," & Chr(13) & _
                "	" & _tipoSuceso & "," & Chr(13) & _
                "	" & _i_DiaSemana & "," & Chr(13) & _
                "	" & _i_Mes & "," & Chr(13) & _
                "	" & _i_Anio & "," & Chr(13) & _
                "   '" & t_Fechainicio_ & "'," & Chr(13) & _
                "	@IDInformacionSQL," & Chr(13) & _
                "	" & _i_Cve_Aplicacion & "," & Chr(13) & _
                "	1," & Chr(13) & _
                "	0," & Chr(13) & _
                "	null," & Chr(13) & _
                "	null," & Chr(13) & _
                "	" & _i_Instrumentacion & "," & Chr(13) & _
                "	" & _modalidadConsulta & "," & Chr(13) & _
                "	" & (Math.Round((((_i_MemoriaRAMDisponibleGB / 1024) / 1024) / 1024), 2, MidpointRounding.ToEven)).ToString & ", " & Chr(13) & _
                "	" & (Math.Round((((_i_MemoriaRAMTotalGB / 1024) / 1024) / 1024), 2, MidpointRounding.ToEven)).ToString & ", " & Chr(13) & _
                "	" & _i_Cve_RecursoSolicitante & "," & Chr(13) & _
                "   '" & _t_IP & "'," & Chr(13) & _
                "   '" & _t_Pais & "'," & Chr(13) & _
                "   '" & _t_EstadoCiudad & "');" & Chr(13) & _
                "   Set @IDBitacora =(select @@IDENTITY as IDBitacora);" & Chr(13) & _
                "	--Select @IDBitacora;" & Chr(13) & _
                "	SELECT   " & Chr(13) & _
                "       'OK' as StatusTransaction," & Chr(13) & _
                "		@IDBitacora as LasIDInserted," & Chr(13) & _
                "        null AS ErrorNumber  " & Chr(13) & _
                "        ,null AS ErrorSeverity  " & Chr(13) & _
                "        ,null AS ErrorState  " & Chr(13) & _
                "        ,null AS ErrorProcedure  " & Chr(13) & _
                "        ,null AS ErrorLine  " & Chr(13) & _
                "        ,null AS ErrorMessage;  " & Chr(13) & _
                "       End Try " & Chr(13) & _
                " BEGIN CATCH  " & Chr(13) & _
                "    SELECT   " & Chr(13) & _
                "      'NO' as StatusTransaction," & Chr(13) & _
                "		null as LasIDInserted," & Chr(13) & _
                "        ERROR_NUMBER() AS ErrorNumber  " & Chr(13) & _
                "        ,ERROR_SEVERITY() AS ErrorSeverity  " & Chr(13) & _
                "        ,ERROR_STATE() AS ErrorState  " & Chr(13) & _
                "        ,ERROR_PROCEDURE() AS ErrorProcedure  " & Chr(13) & _
                "        ,ERROR_LINE() AS ErrorLine  " & Chr(13) & _
                "        ,ERROR_MESSAGE() AS ErrorMessage;  " & Chr(13) & _
                "    IF @@TRANCOUNT > 0  " & Chr(13) & _
                "        ROLLBACK TRANSACTION;  " & Chr(13) & _
                " END CATCH;  " & Chr(13) & _
                " IF @@TRANCOUNT > 0  " & Chr(13) & _
                "  COMMIT TRANSACTION  " '& Chr(13) & _
            '" GO "

            '/////////////////////////////REVISION 2020, Hay que Actualizar ///////////////////////
            If EsOpenClose_ Then

                _tagWatcherMensaje = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenClose(CadenaSql_)

                '_tagWatcherMensaje.SetError()
            Else

                _tagWatcherMensaje = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizada(CadenaSql_)

                '_tagWatcherMensaje.SetError()
            End If





            If _tagWatcherMensaje.Status = TagWatcher.TypeStatus.Ok Then

                '                respuesta_ = ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables(0).Rows(0)("StatusTransaction").ToString

                If Not _tagWatcherMensaje.ObjectReturned Is Nothing Then

                    If _tagWatcherMensaje.ObjectReturned.Tables.Count >= 1 Then

                        respuesta_ = _tagWatcherMensaje.ObjectReturned.tables(0).rows(0)("StatusTransaction").ToString

                        If respuesta_ = "NO" Then

                            _estatusInsercion = IBitacoras.EstatusInsercion.Incorrecto

                            _idTransaccion = Nothing

                        ElseIf respuesta_ = "OK" Then

                            _estatusInsercion = IBitacoras.EstatusInsercion.Correcto

                            _idTransaccion = _tagWatcherMensaje.ObjectReturned.tables(0).rows(0)("LasIDInserted").ToString

                        End If

                    End If

                End If

            Else

                _estatusInsercion = IBitacoras.EstatusInsercion.Incorrecto

                _idTransaccion = Nothing

            End If

        End Sub


        Public Sub ActualizaBitacoraAvanzada(ByVal idTransaccionBitacora_ As Int32,
                                             Optional ByVal EsOpenClose_ As Boolean = True)

            If Not _idTransaccion Is Nothing Then

                Dim t_FechaFinal_ As String = Nothing

                t_FechaFinal_ = _f_FechaHoraFinal.ToString("dd/MM/yy H:mm:ss.fff")

                Dim respuesta_ As String = Nothing

                Dim CadenaSql_ As String = Nothing

                Dim idTagWatcher_ As String = "0"

                If _permiso = 0 Then : _permiso = 118 : End If

                'ConexionSingleton.SQLServerSingletonConexion.DataSetReciente.Tables.Clear()

                CadenaSql_ = _
              "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; " & Chr(13) & _
              "SET ARITHABORT ON;" & Chr(13) & _
              "BEGIN TRANSACTION;  " & Chr(13) & _
              " BEGIN TRY  " & Chr(13) & _
              " SET NOCOUNT on;" & Chr(13) & _
              " declare @IDBitacora integer = null;" & Chr(13) & _
              " update " & Trim(_nombreBaseDatosBitacora) & ".dbo.Reg011BitacoraAvanzadaConsultas " & Chr(13) & _
              " set " & Chr(13) & _
              " i_Cve_Estatus = 1," & Chr(13) & _
              " f_FechaHoraFinal ='" & t_FechaFinal_ & "'," & Chr(13) & _
              " i_TiempoRespuestaTotal = " & (_i_TiempoRespuestaTotal / 1000).ToString & "," & Chr(13) & _
              " i_Cve_TagWatcher = " & idTagWatcher_ & "," & Chr(13) & _
              " i_Cve_Permiso = " & _permiso & "," & Chr(13) & _
              " i_cve_tipoMensaje = " & _tipoSuceso & "," & Chr(13) & _
              " t_Mensaje = '" & _tagWatcherMensaje.ErrorDescription & "'" & Chr(13) & _
              "  where i_Cve_BitacoraAvanzadaConsultas = " & _idTransaccion & Chr(13) & _
              "	 SELECT   " & Chr(13) & _
              "     'OK' as StatusTransaction," & Chr(13) & _
              "      @IDBitacora as LasIDInserted," & Chr(13) & _
              "      null AS ErrorNumber  " & Chr(13) & _
              "      ,null AS ErrorSeverity  " & Chr(13) & _
              "      ,null AS ErrorState  " & Chr(13) & _
              "      ,null AS ErrorProcedure  " & Chr(13) & _
              "      ,null AS ErrorLine  " & Chr(13) & _
              "      ,null AS ErrorMessage; " & Chr(13) & _
              " END TRY  " & Chr(13) & _
              "  BEGIN CATCH  " & Chr(13) & _
              "    SELECT   " & Chr(13) & _
              "         'NO' as StatusTransaction," & Chr(13) & _
              " 		   null as LasIDInserted," & Chr(13) & _
              "         ERROR_NUMBER() AS ErrorNumber  " & Chr(13) & _
              "         ,ERROR_SEVERITY() AS ErrorSeverity  " & Chr(13) & _
              "         ,ERROR_STATE() AS ErrorState  " & Chr(13) & _
              "         ,ERROR_PROCEDURE() AS ErrorProcedure  " & Chr(13) & _
              "         ,ERROR_LINE() AS ErrorLine  " & Chr(13) & _
              "         ,ERROR_MESSAGE() AS ErrorMessage;  " & Chr(13) & _
              "     IF @@TRANCOUNT > 0  " & Chr(13) & _
              "         ROLLBACK TRANSACTION;  " & Chr(13) & _
              " END CATCH;  " & Chr(13) & _
              " IF @@TRANCOUNT > 0  " & Chr(13) & _
              "     COMMIT TRANSACTION;  "
                '"                 GO "

                If EsOpenClose_ Then

                    _tagWatcherMensaje = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizadaOpenClose(CadenaSql_)
                Else

                    _tagWatcherMensaje = ConexionSingleton.SQLServerSingletonConexion.EjecutaConsultaDirectaEstandarizada(CadenaSql_)

                End If


                If _tagWatcherMensaje.Status = TagWatcher.TypeStatus.Ok Then

                    respuesta_ = _tagWatcherMensaje.ObjectReturned.tables(0).rows(0)("StatusTransaction").ToString

                    If respuesta_ = "NO" Then

                        _estatusInsercion = IBitacoras.EstatusInsercion.Incorrecto

                    ElseIf respuesta_ = "OK" Then

                        _estatusInsercion = IBitacoras.EstatusInsercion.Correcto

                    End If

                Else

                    _estatusInsercion = IBitacoras.EstatusInsercion.Incorrecto

                    _idTransaccion = Nothing

                End If

            Else

                'NOT IMPLEMENTED

            End If

        End Sub

#End Region

    End Class

End Namespace
