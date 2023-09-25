Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones
Imports Gsol.Componentes.SistemaBase.GsDialogo

Namespace Gsol.BaseDatos

    Public Class Documentos64

#Region "Atributos"

        Private _operaciones As OperacionesCatalogo

#End Region

#Region "Constructores"

        Sub New(ByVal ioperaciones_ As IOperacionesCatalogo)

            _operaciones = ioperaciones_

        End Sub

#End Region

#Region "Metodos"


#End Region

#Region "Funciones"

        Public Function RegistrarDocumento(ByVal Documento_ As Reg012Documentos) As Reg012Documentos

            If Not Documento_.TipoRegistroDocumento = Reg012Documentos.TipoRegistro.DocumentoNoIdentificado Then

                Dim OCDocumento_ = New OperacionesCatalogo

                OCDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

                Dim sistema_ = New Organismo

                OCDocumento_ = sistema_.ConsultaModulo(OCDocumento_,
                                                       "RegistroDocumentos",
                                                       Organismo.TiposOperacionEnsamblaModulo.Insercion)

                Documento_.CargarInformacion()

                If Not Documento_.TagWatcher.Status = TagWatcher.TypeStatus.Errors Then

                    With OCDocumento_

                        .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                        .CampoPorNombre("t_RutaDocumentoOrigen") = Documento_.t_RutaDocumentoOrigen

                        .CampoPorNombre("i_Cve_TipoDocumento") = Documento_.i_Cve_TipoDocumento

                        .CampoPorNombre("f_FechaRegistro") = Documento_.f_FechaRegistro.ToString("dd/MM/yyyy H:mm:ss")

                        .CampoPorNombre("i_Cve_EstatusDocumento") = Documento_.i_Cve_EstatusDocumento

                        .CampoPorNombre("t_NombreDocumento") = Documento_.t_NombreDocumento

                        .CampoPorNombre("t_FolioDocumento") = Documento_.t_FolioDocumento

                        .CampoPorNombre("t_Documento") = Documento_.t_Documento

                        .CampoPorNombre("i_Cve_RepositorioDigital") = Documento_.i_Cve_RepositorioDigital

                        .CampoPorNombre("t_VersionCFDi") = Documento_.t_VersionCFDi

                        .CampoPorNombre("i_Cve_EstatusLiquidacion") = Documento_.i_Cve_EstatusLiquidacion

                        .CampoPorNombre("i_Cve_Estado") = Documento_.i_Cve_Estado

                        .CampoPorNombre("i_Cve_DivisionMiEmpresa") = Documento_.i_Cve_DivisionMiEmpresa

                        If .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                            Documento_.i_Cve_Documento = .ValorIndice

                            Dim OCExtensionDocumento_ = New OperacionesCatalogo

                            OCExtensionDocumento_.EspacioTrabajo = _operaciones.EspacioTrabajo

                            OCExtensionDocumento_ = sistema_.ConsultaModulo(OCExtensionDocumento_,
                                                                            "ExtensionRegistroDocumentos",
                                                                             Organismo.TiposOperacionEnsamblaModulo.Insercion)

                            With OCExtensionDocumento_

                                .PreparaCatalogo(IOperacionesCatalogo.TiposOperacionSQL.Insercion)

                                .CampoPorNombre("i_Cve_Documento") = Documento_.Ext012Documentos.i_Cve_Documento

                                .CampoPorNombre("t_NombreDocumento") = Documento_.Ext012Documentos.t_NombreDocumento

                                .CampoPorNombre("t_URLPublico") = Documento_.Ext012Documentos.t_URLPublico

                                .CampoPorNombre("i_Cve_Estado") = Documento_.Ext012Documentos.i_Cve_Estado

                                .CampoPorNombre("i_Cve_Estatus") = Documento_.Ext012Documentos.i_Cve_Estatus

                                .CampoPorNombre("i_Cve_TipoArchivo") = Documento_.Ext012Documentos.i_Cve_TipoArchivo

                                .CampoPorNombre("i_Cve_DivisionMiEmpresa") = Documento_.Ext012Documentos.i_Cve_DivisionMiEmpresa

                                .CampoPorNombre("i_Cve_TipoDocumento") = Documento_.Ext012Documentos.i_Cve_TipoDocumento

                                .CampoPorNombre("i_ConsultableCliente") = Documento_.Ext012Documentos.i_ConsultableCliente

                                .CampoPorNombre("i_Cve_OrigenDocumento") = Documento_.Ext012Documentos.i_Cve_OrigenDocumento

                                If Not .Agregar() = IOperacionesCatalogo.EstadoOperacion.COk Then

                                    Documento_.TagWatcher.Status = TagWatcher.TypeStatus.Errors

                                    Documento_.TagWatcher.Errors = TagWatcher.ErrorTypes.C6_012_1004

                                End If

                            End With

                        Else

                            Documento_.TagWatcher.Status = TagWatcher.TypeStatus.Errors

                            Documento_.TagWatcher.Errors = TagWatcher.ErrorTypes.C6_012_1003

                        End If

                    End With

                Else

                    Documento_.TagWatcher.Status = TagWatcher.TypeStatus.Errors

                    Documento_.TagWatcher.Errors = TagWatcher.ErrorTypes.C6_012_1005

                End If

            End If

            Return Documento_

        End Function

        Public Function ActualizarTipoDocumento(ByVal Documento_ As Reg012Documentos) As Reg012Documentos

            'Actualizamos la tabla de documentos Reg003Documentos
            Dim Documentos_ = New OperacionesCatalogo

            Dim sistema_ = New Organismo

            Documentos_.EspacioTrabajo = _operaciones.EspacioTrabajo

            Documentos_ = sistema_.ConsultaModulo(Documentos_,
                                                  "RegistroDocumentos",
                                                   Organismo.TiposOperacionEnsamblaModulo.Insercion)

            With Documentos_

                .TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                .CampoPorNombre("i_Cve_Documento") = Documento_.i_Cve_Documento

                .CampoPorNombre("i_Cve_TipoDocumento") = Documento_.i_Cve_TipoDocumento

                .EditaCampoPorNombre("t_RutaDocumentoOrigen").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_Estado").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("f_FechaRegistro").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_EstatusDocumento").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("t_NombreDocumento").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("t_FolioDocumento").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_RepositorioDigital").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("t_Documento").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_DivisionMiEmpresa").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("t_VersionCFDi").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_EstatusLiquidacion").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                If Not (Documentos_.Modificar(Documento_.i_Cve_Documento) = IOperacionesCatalogo.EstadoOperacion.COk) Then

                    Documento_.TagWatcher.Status = TagWatcher.TypeStatus.Errors

                    Documento_.TagWatcher.Errors = TagWatcher.ErrorTypes.C6_012_1006

                End If

            End With

            'Actualizamos la tabla de documentos Ext003Documentos

            Dim ExtensionDocumentos_ = New OperacionesCatalogo

            ExtensionDocumentos_.EspacioTrabajo = _operaciones.EspacioTrabajo

            ExtensionDocumentos_ = sistema_.ConsultaModulo(ExtensionDocumentos_,
                                                           "ExtensionRegistroDocumentos",
                                                           Organismo.TiposOperacionEnsamblaModulo.Insercion)

            With ExtensionDocumentos_

                .TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                .CampoPorNombre("i_Cve_Documento") = Documento_.i_Cve_Documento

                .CampoPorNombre("i_Cve_TipoDocumento") = Documento_.i_Cve_TipoDocumento

                .EditaCampoPorNombre("t_URLPublico").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("t_NombreDocumento").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_TipoArchivo").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_ConsultableCliente").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_Estado").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_Estatus").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_DivisionMiEmpresa").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                .EditaCampoPorNombre("i_Cve_OrigenDocumento").PuedeModificar = ICaracteristica.TiposRigorDatos.No

                If Not (ExtensionDocumentos_.Modificar(Documento_.i_Cve_Documento) = IOperacionesCatalogo.EstadoOperacion.COk) Then

                    Documento_.TagWatcher.Status = TagWatcher.TypeStatus.Errors

                    Documento_.TagWatcher.Errors = TagWatcher.ErrorTypes.C6_012_1007

                End If

            End With

            Return Documento_

        End Function

#End Region

    End Class

End Namespace

