Imports gsol
Imports gsol.BaseDatos.Operaciones
'Imports gsol.Controladores.Contabilidad
'Imports gsol.Controladores.Poliza
'Imports System.Xml
Imports System.IO
'Imports System.ComponentModel
'Imports gsol.monitoreo
'Imports gsol.Controladores
'Imports Wma.Exceptions
'Imports gsol.Nucleo.FacturaObjeto3364.FacturaObjeto
'Imports gsol


Namespace gsol.Controladores
    Public Class ControlLDA

#Region "Atributos"

        Private _sistema As New Organismo

        Private _numeroUnicogGuid As String

        Private _escritor As StreamWriter

        Private _rutaBitacora As String

        Private _nombreUsuario As String

        Private _tipoAccion As String

        Private _numeroReferencia As String

        Private _numeroLlave As String

        Private _identificadorUnico As String

        Private _claveDivisionMiEmpresa As Int32

        Private _claveUsuario As Int32

        Private _horaArchivo As String



#End Region

#Region "Constructores"

        Sub New(ByVal numeroReferencia_ As String, ByVal tipoAccion_ As String, ByVal cveUsuario_ As Integer, nombreUsuario_ As String, ByVal divisionMiEmpresa_ As Integer, Optional ByVal llave_ As String = "NULL")

            _rutaBitacora = "C:\logs\LDA"

            If Not ValidaRepositorioLocal(_rutaBitacora) Then

                _rutaBitacora = "c:\logs"

            End If

            _numeroReferencia = numeroReferencia_

            _tipoAccion = tipoAccion_

            _claveUsuario = cveUsuario_

            _nombreUsuario = nombreUsuario_

            _claveDivisionMiEmpresa = divisionMiEmpresa_

            NumeroLlave = llave_

            _identificadorUnico = Nothing

            _numeroUnicogGuid = System.Guid.NewGuid.ToString()

            _horaArchivo = Date.Now.ToString.Replace(":", "").Replace(".", "").Replace("/", "-").Replace(" ", "-")

        End Sub

#End Region

#Region "Metodos"
        Public Sub GrabarLog(ByVal contenido_ As List(Of String))

            Try

                _identificadorUnico = _rutaBitacora &
                                        "\" &
                                        _numeroReferencia & "_" &
                                        "[" & NumeroLlave & "]_" &
                                        _claveDivisionMiEmpresa & "_" &
                                        "[" & _tipoAccion & "]_" &'_nombreUsuario & "_" &
                                         _claveUsuario & "_" &
                                        "[" & _horaArchivo & "]_" &
                                         _numeroUnicogGuid &
                                        ".txt"

                _escritor = File.AppendText(_identificadorUnico)

                For Each item As String In contenido_

                    _escritor.WriteLine(item)

                Next

                _escritor.Flush()

                _escritor.Close()

            Catch excepcion_ As Exception

                '_sistema.GsDialogo("Ocurrio  un problema al grabar el respaldo de esta transacción en la bitácora de archivos, por favor notifiquelo al administrador")

            End Try

        End Sub


#End Region

#Region "Funciones"
        Private Function ValidaRepositorioLocal(ByVal directorio_ As String) As Boolean

            If Directory.Exists(directorio_) Then

                Return True

            Else

                Try
                    Directory.CreateDirectory(directorio_)

                    Return True

                Catch ex As Exception

                    '_sistema.GsDialogo("No se ha podido crear el directorio para respaldo de transacciones", Componentes.SistemaBase.GsDialogo.TipoDialogo.Err)

                    Return False

                End Try

            End If

            Return False

        End Function

        Public Function ValidaRegistro(ByVal token_ As String, ClausuLibre_ As String, espacioTrabajo_ As OperacionesCatalogo) As Dictionary(Of String, String)

            Dim operacion_ = New OperacionesCatalogo

            Dim resultado_ As System.Collections.Generic.Dictionary(Of String, String) = New System.Collections.Generic.Dictionary(Of String, String)()

            operacion_.EspacioTrabajo = espacioTrabajo_.EspacioTrabajo

            operacion_.CantidadVisibleRegistros = 1

            operacion_.ActivarLecturaSuciaSQL = False

            operacion_.ClausulasLibres = ClausuLibre_

            'operacion_.ActivarLecturaSuciaSQL = True

            operacion_ = _sistema.ConsultaModulo(operacion_,
                                              token_)

            If _sistema.TieneResultados(operacion_) Then

                For Each fila_ As Data.DataRow In operacion_.Vista.Tables(0).Rows

                    resultado_.Add("Estado", "True")

                    resultado_.Add("Indice", fila_.Item(1).ToString)

                Next

            Else

                resultado_.Add("Estado", "False")

            End If

            Return resultado_

        End Function

        Public Function MarcaArchivo(ByVal valorActual_ As String,
                                                     ByVal valorNuevo_ As String) As Boolean

            Rename(_identificadorUnico, _identificadorUnico.Replace(valorActual_, valorNuevo_))

            If File.Exists(_identificadorUnico.Replace(valorActual_, valorNuevo_)) Then

                _identificadorUnico = _identificadorUnico.Replace(valorActual_, valorNuevo_)

                Return True

            Else

                Return False

            End If

            Return False

        End Function

#End Region

#Region "Propiedades"

        Public Property NumeroLlave As String
            Get
                Return _numeroLlave
            End Get
            Set(value As String)
                _numeroLlave = value
            End Set
        End Property

#End Region

    End Class

End Namespace