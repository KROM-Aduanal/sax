Imports gsol.BaseDatos.Operaciones
Imports gsol
Imports System.Threading
Imports Wma.Exceptions


Public Class MonitorRecolectorDocumentos

#Region "Atributos"

    Private _recolector As ControladorRecolectorDocumentos64

    Private _sistema As Organismo

    Private _claveRecolectorResumen As String

    Private _tag As TagWatcher

#End Region

#Region "Constructores"

    Sub New()

        InitializeComponent()

        _sistema = New Organismo

        _recolector = New ControladorRecolectorDocumentos64()

        _tag = New TagWatcher()

        PreparaControles()

        _recolector.IniciarRecoleccion()

        lbVersion.Text = "v1.0.0.1"

    End Sub

#End Region

#Region "Metodos"

    Private Sub PreparaControles()

        ' RecolectorDocumentos

        GsCatalogo1.CatalogoOpcional = False

        GsCatalogo1.EspacioTrabajo = _recolector.EspacioTrabajo

        _sistema.EnsamblaModulo("RecolectorDocumentos", GsCatalogo1)

        GsCatalogo1.OperacionesCatalogo.ModalidadConsulta = _recolector.OperacionesCatalogo.ModalidadConsulta

        GsCatalogo1.OperacionesCatalogo.EspacioTrabajo = _recolector.OperacionesCatalogo.EspacioTrabajo

        GsCatalogo1.OperacionesCatalogo.OrdenarResultados(1) = IOperacionesCatalogo.OrdenConsulta.ASC

        GsCatalogo1.OperacionesCatalogo.CantidadVisibleRegistros = 0

        GsCatalogo1.IdentificadorEmpresa = -1

        GsCatalogo1.TsBuscar_Click(Nothing, Nothing)


        ' BitacoraRecolectorDocumentos

        GsCatalogo2.CatalogoOpcional = False

        GsCatalogo2.EspacioTrabajo = _recolector.EspacioTrabajo

        _sistema.EnsamblaModulo("BitacoraRecolectorDocumentos", GsCatalogo2)

        GsCatalogo2.OperacionesCatalogo.ModalidadConsulta = _recolector.OperacionesCatalogo.ModalidadConsulta

        GsCatalogo2.OperacionesCatalogo.EspacioTrabajo = _recolector.OperacionesCatalogo.EspacioTrabajo

        GsCatalogo2.OperacionesCatalogo.OrdenarResultados(1) = IOperacionesCatalogo.OrdenConsulta.DESC

        GsCatalogo2.OperacionesCatalogo.CantidadVisibleRegistros = 1000

        GsCatalogo2.TsBuscar_Click(Nothing, Nothing)


        ' ErroresRecolector

        GsCatalogo3.CatalogoOpcional = False

        GsCatalogo3.EspacioTrabajo = _recolector.EspacioTrabajo

        _sistema.EnsamblaModulo("ErroresRecolector", GsCatalogo3)

        GsCatalogo3.OperacionesCatalogo.ModalidadConsulta = _recolector.OperacionesCatalogo.ModalidadConsulta

        GsCatalogo3.OperacionesCatalogo.EspacioTrabajo = _recolector.OperacionesCatalogo.EspacioTrabajo

        GsCatalogo3.OperacionesCatalogo.OrdenarResultados(1) = IOperacionesCatalogo.OrdenConsulta.DESC

        GsCatalogo3.OperacionesCatalogo.CantidadVisibleRegistros = 1000

        GsCatalogo3.TsBuscar_Click(Nothing, Nothing)

    End Sub

    Private Sub MostrarResumen()

        Dim i_Cve_Recolector_ As String = Nothing

        i_Cve_Recolector_ = GsCatalogo1.DgvCatalogo.CurrentRow.Cells.Item("Clave Recolector").Value.ToString()

        If _claveRecolectorResumen <> i_Cve_Recolector_ Then

            _claveRecolectorResumen = i_Cve_Recolector_

            DibujarControlesResumen(i_Cve_Recolector_)

        Else

            ActualizarControlesResumen(i_Cve_Recolector_)

        End If

    End Sub

    Private Sub DibujarControlesResumen(ByVal i_Cve_RecolectorDocumentos_ As String)

        TableLayoutPanel1.Controls.Clear()

        TableLayoutPanel1.RowCount = 0

        DibujarControlesGeneral(i_Cve_RecolectorDocumentos_)

        DibujarControlesTiposDocumentos(i_Cve_RecolectorDocumentos_)

        DibujarControlesCodigoError(i_Cve_RecolectorDocumentos_)

    End Sub

    Private Sub ActualizarControlesResumen(ByVal i_Cve_RecolectorDocumentos_ As String)

        Dim dataTable_ As DataTable = Nothing

        Dim sizeProgress_ As Size = New Size(211, 10)

        Dim toolTip_ As New ToolTip()

        Dim index_ As Integer = 0

        Dim pbControl_ As ProgressBar

        Dim lbControl_ As Label

        Dim t_NombreRecolector_ As String = Nothing

        Dim f_FechaUltimaRecoleccion_ As DateTime = Nothing

        Dim i_Registrados_ As Integer = 0

        Dim i_Reconocidos_ As Integer = 0

        Dim i_Recolectados_ As Integer = 0

        Dim i_Errores_ As Integer = 0

        Dim i_Cancelados_ As Integer = 0

        Dim d_PorcentajeReconocidos_ As Double = 0

        Dim d_PorcentajeRecolectados_ As Double = 0

        Dim d_PorcentajeErrores_ As Double = 0

        Dim d_PorcentajeCancelados_ As Double = 0

        Dim t_NombreDoc_ As String = Nothing

        Dim i_ReconocidosDoc_ As Integer = 0

        Dim i_RecolectadosDoc_ As Integer = 0

        Dim t_CodigoError_ As String = Nothing

        Dim i_TotalErrores_ As Integer = 0

        Dim i_ErroresCodigo_ As Integer = 0


        ' GENERAL

        dataTable_ = _recolector.ConsultarResumenGeneral("WHERE i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_)


        If Not dataTable_ Is Nothing And dataTable_.Rows.Count > 0 Then

            t_NombreRecolector_ = dataTable_.Rows(0).Item("t_NombreRecolector")

            f_FechaUltimaRecoleccion_ = dataTable_.Rows(0).Item("f_FechaRecoleccion")

            i_Registrados_ = dataTable_.Rows(0).Item("i_Registrados")

            i_Reconocidos_ = dataTable_.Rows(0).Item("i_Reconocidos")

            i_Recolectados_ = dataTable_.Rows(0).Item("i_Recolectados")

            i_Errores_ = dataTable_.Rows(0).Item("i_Errores")

            i_Cancelados_ = dataTable_.Rows(0).Item("i_Cancelados")


            d_PorcentajeReconocidos_ = 0

            d_PorcentajeRecolectados_ = 0

            d_PorcentajeErrores_ = 0

            d_PorcentajeCancelados_ = 0


            If i_Registrados_ > 0 Then

                d_PorcentajeReconocidos_ = Math.Round((i_Reconocidos_ / i_Registrados_) * 100, 2)

                d_PorcentajeErrores_ = Math.Round((i_Errores_ / i_Registrados_) * 100, 2)

                d_PorcentajeCancelados_ = Math.Round((i_Cancelados_ / i_Registrados_) * 100, 2)

            End If


            If i_Reconocidos_ > 0 Then

                d_PorcentajeRecolectados_ = Math.Round((i_Recolectados_ / i_Reconocidos_) * 100, 2)

            End If


            ' Actualizar valores en controles

            For Each control_ As Control In TableLayoutPanel1.Controls

                Select Case control_.Name

                    Case "NombreRecolector"

                        index_ = TableLayoutPanel1.GetRow(control_)

                        lbControl_ = TableLayoutPanel1.GetControlFromPosition(0, index_)

                        lbControl_.Text = t_NombreRecolector_

                    Case "FechaUltimaRecoleccion"

                        index_ = TableLayoutPanel1.GetRow(control_)

                        lbControl_ = TableLayoutPanel1.GetControlFromPosition(1, index_)

                        lbControl_.Text = f_FechaUltimaRecoleccion_.ToString()

                    Case "Reconocidos"

                        index_ = TableLayoutPanel1.GetRow(control_)

                        pbControl_ = TableLayoutPanel1.GetControlFromPosition(1, index_)

                        pbControl_.Value = d_PorcentajeReconocidos_

                        lbControl_ = TableLayoutPanel1.GetControlFromPosition(2, index_)

                        lbControl_.Text = d_PorcentajeReconocidos_ & "%"

                        toolTip_.SetToolTip(lbControl_, i_Reconocidos_ & "/" & i_Registrados_)

                    Case "Recolectados"

                        index_ = TableLayoutPanel1.GetRow(control_)

                        pbControl_ = TableLayoutPanel1.GetControlFromPosition(1, index_)

                        pbControl_.Value = d_PorcentajeRecolectados_

                        lbControl_ = TableLayoutPanel1.GetControlFromPosition(2, index_)

                        lbControl_.Text = d_PorcentajeRecolectados_ & "%"

                        toolTip_.SetToolTip(lbControl_, i_Recolectados_ & "/" & i_Reconocidos_)

                    Case "Errores"

                        index_ = TableLayoutPanel1.GetRow(control_)

                        pbControl_ = TableLayoutPanel1.GetControlFromPosition(1, index_)

                        pbControl_.Value = d_PorcentajeErrores_

                        lbControl_ = TableLayoutPanel1.GetControlFromPosition(2, index_)

                        lbControl_.Text = d_PorcentajeErrores_ & "%"

                        toolTip_.SetToolTip(lbControl_, i_Errores_ & "/" & i_Registrados_)

                    Case "Cancelados"

                        index_ = TableLayoutPanel1.GetRow(control_)

                        pbControl_ = TableLayoutPanel1.GetControlFromPosition(1, index_)

                        pbControl_.Value = d_PorcentajeCancelados_

                        lbControl_ = TableLayoutPanel1.GetControlFromPosition(2, index_)

                        lbControl_.Text = d_PorcentajeCancelados_ & "%"

                        toolTip_.SetToolTip(lbControl_, i_Cancelados_ & "/" & i_Registrados_)

                End Select

            Next

        End If


        ' TIPOS DE DOCUMENTOS

        dataTable_ = _recolector.ConsultarResumenTiposDocumentos("AND tip.i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_)


        If Not dataTable_ Is Nothing And dataTable_.Rows.Count > 0 Then

            For Each row_ As DataRow In dataTable_.Rows

                t_NombreDoc_ = row_.Item("t_Nombre")

                i_ReconocidosDoc_ = row_.Item("i_Reconocidos")

                i_RecolectadosDoc_ = row_.Item("i_Recolectados")


                Dim d_Porcentaje_ As Double = 0


                If i_ReconocidosDoc_ > 0 Then

                    d_Porcentaje_ = Math.Round((i_RecolectadosDoc_ / i_ReconocidosDoc_) * 100, 2)

                End If


                For Each control_ As Control In TableLayoutPanel1.Controls

                    If control_.Name = t_NombreDoc_ Then

                        index_ = TableLayoutPanel1.GetRow(control_)

                        pbControl_ = TableLayoutPanel1.GetControlFromPosition(1, index_)

                        pbControl_.Value = d_Porcentaje_

                        lbControl_ = TableLayoutPanel1.GetControlFromPosition(2, index_)

                        lbControl_.Text = d_Porcentaje_ & "%"

                        toolTip_.SetToolTip(lbControl_, i_RecolectadosDoc_ & "/" & i_ReconocidosDoc_)

                    End If

                Next

            Next

        End If


        ' CÓDIGOS DE ERROR

        dataTable_ = _recolector.ConsultarResumenErrores("AND err.i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_)


        If Not dataTable_ Is Nothing And dataTable_.Rows.Count > 0 Then

            For Each row_ As DataRow In dataTable_.Rows

                t_CodigoError_ = row_.Item("t_CodigoError")

                i_TotalErrores_ = row_.Item("i_TotalErrores")

                i_ErroresCodigo_ = row_.Item("i_ErroresCodigo")

                Dim d_PorcentajeError_ As Double = 0

                If i_TotalErrores_ > 0 Then

                    d_PorcentajeError_ = Math.Round((i_ErroresCodigo_ / i_TotalErrores_) * 100, 2)

                End If

                For Each control_ As Control In TableLayoutPanel1.Controls

                    If control_.Name = t_CodigoError_ Then

                        index_ = TableLayoutPanel1.GetRow(control_)

                        pbControl_ = TableLayoutPanel1.GetControlFromPosition(1, index_)

                        pbControl_.Value = d_PorcentajeError_

                        lbControl_ = TableLayoutPanel1.GetControlFromPosition(2, index_)

                        lbControl_.Text = d_PorcentajeError_ & "%"

                        toolTip_.SetToolTip(lbControl_, i_ErroresCodigo_ & "/" & i_TotalErrores_)

                    End If

                Next

            Next

        End If

    End Sub

    Private Sub DibujarControlesGeneral(ByVal i_Cve_RecolectorDocumentos_ As String)

        Dim dataTable_ As DataTable = Nothing

        Dim font_ As Font = New Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Dim fontBold_ As Font = New Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Dim fontBoldItalic_ As Font = New Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic),  _
                                                System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Dim colorGray_ As Color = Color.SlateGray

        Dim colorRed_ As Color = Color.DarkRed

        Dim sizeProgress_ As Size = New Size(211, 10)

        Dim lbGeneral As New Label()

        Dim lbNombreRecolector As New Label()

        Dim lbFechaUltimaRecoleccion As New Label()

        Dim lbReconocidos As New Label()

        Dim lbRecolectados As New Label()

        Dim lbErrores As New Label()

        Dim lbCancelados As New Label()

        Dim pbReconocidos As New ProgressBar()

        Dim pbRecolectados As New ProgressBar()

        Dim pbErrores As New ProgressBar()

        Dim pbCancelados As New ProgressBar()

        Dim lbPorcentajeReconocidos As New Label()

        Dim lbPorcentajeRecolectados As New Label()

        Dim lbPorcentajeErrores As New Label()

        Dim lbPorcentajeCancelados As New Label()

        Dim toolTip_ As New ToolTip()

        Dim t_NombreRecolector_ As String = Nothing

        Dim f_FechaUltimaRecoleccion_ As DateTime = Nothing

        Dim i_Registrados_ As Integer = 0

        Dim i_Reconocidos_ As Integer = 0

        Dim i_Recolectados_ As Integer = 0

        Dim i_Errores_ As Integer = 0

        Dim i_Cancelados_ As Integer = 0

        Dim d_PorcentajeReconocidos_ As Double = 0

        Dim d_PorcentajeRecolectados_ As Double = 0

        Dim d_PorcentajeErrores_ As Double = 0

        Dim d_PorcentajeCancelados_ As Double = 0


        dataTable_ = _recolector.ConsultarResumenGeneral("WHERE i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_)


        If Not dataTable_ Is Nothing And dataTable_.Rows.Count > 0 Then

            t_NombreRecolector_ = dataTable_.Rows(0).Item("t_NombreRecolector")

            f_FechaUltimaRecoleccion_ = dataTable_.Rows(0).Item("f_FechaRecoleccion")

            i_Registrados_ = dataTable_.Rows(0).Item("i_Registrados")

            i_Reconocidos_ = dataTable_.Rows(0).Item("i_Reconocidos")

            i_Recolectados_ = dataTable_.Rows(0).Item("i_Recolectados")

            i_Errores_ = dataTable_.Rows(0).Item("i_Errores")

            i_Cancelados_ = dataTable_.Rows(0).Item("i_Cancelados")

            d_PorcentajeReconocidos_ = 0

            d_PorcentajeRecolectados_ = 0

            d_PorcentajeErrores_ = 0

            d_PorcentajeCancelados_ = 0


            If i_Registrados_ > 0 Then

                d_PorcentajeReconocidos_ = Math.Round((i_Reconocidos_ / i_Registrados_) * 100, 2)

                d_PorcentajeErrores_ = Math.Round((i_Errores_ / i_Registrados_) * 100, 2)

                d_PorcentajeCancelados_ = Math.Round((i_Cancelados_ / i_Registrados_) * 100, 2)

            End If


            If i_Reconocidos_ > 0 Then

                d_PorcentajeRecolectados_ = Math.Round((i_Recolectados_ / i_Reconocidos_) * 100, 2)

            End If


            lbGeneral.AutoSize = True

            lbGeneral.Font = fontBoldItalic_

            lbGeneral.ForeColor = colorGray_

            lbGeneral.Text = "General"


            lbNombreRecolector.AutoSize = True

            lbNombreRecolector.Font = fontBold_

            lbNombreRecolector.ForeColor = colorRed_

            lbNombreRecolector.Text = t_NombreRecolector_

            lbNombreRecolector.Name = "NombreRecolector"


            lbFechaUltimaRecoleccion.AutoSize = True

            lbFechaUltimaRecoleccion.Font = fontBold_

            lbFechaUltimaRecoleccion.ForeColor = colorGray_

            lbFechaUltimaRecoleccion.Text = f_FechaUltimaRecoleccion_.ToString()

            lbFechaUltimaRecoleccion.Name = "FechaUltimaRecoleccion"


            lbReconocidos.AutoSize = True

            lbReconocidos.Font = fontBold_

            lbReconocidos.ForeColor = colorGray_

            lbReconocidos.Text = "Reconocidos"

            lbReconocidos.Name = "Reconocidos"

            pbReconocidos.Size = sizeProgress_

            pbReconocidos.Value = d_PorcentajeReconocidos_

            lbPorcentajeReconocidos.AutoSize = True

            lbPorcentajeReconocidos.Font = fontBold_

            lbPorcentajeReconocidos.ForeColor = colorGray_

            lbPorcentajeReconocidos.Text = d_PorcentajeReconocidos_ & "%"

            toolTip_.SetToolTip(lbPorcentajeReconocidos, i_Reconocidos_ & "/" & i_Registrados_)


            lbRecolectados.AutoSize = True

            lbRecolectados.Font = fontBold_

            lbRecolectados.ForeColor = colorGray_

            lbRecolectados.Text = "Recolectados"

            lbRecolectados.Name = "Recolectados"

            pbRecolectados.Size = sizeProgress_

            pbRecolectados.Value = d_PorcentajeRecolectados_

            lbPorcentajeRecolectados.AutoSize = True

            lbPorcentajeRecolectados.Font = fontBold_

            lbPorcentajeRecolectados.ForeColor = colorGray_

            lbPorcentajeRecolectados.Text = d_PorcentajeRecolectados_ & "%"

            toolTip_.SetToolTip(lbPorcentajeRecolectados, i_Recolectados_ & "/" & i_Reconocidos_)


            lbErrores.AutoSize = True

            lbErrores.Font = fontBold_

            lbErrores.ForeColor = colorRed_

            lbErrores.Text = "Errores"

            lbErrores.Name = "Errores"

            pbErrores.Size = sizeProgress_

            pbErrores.Value = d_PorcentajeErrores_

            lbPorcentajeErrores.AutoSize = True

            lbPorcentajeErrores.Font = fontBold_

            lbPorcentajeErrores.ForeColor = colorRed_

            lbPorcentajeErrores.Text = d_PorcentajeErrores_ & "%"

            toolTip_.SetToolTip(lbPorcentajeErrores, i_Errores_ & "/" & i_Registrados_)


            lbCancelados.AutoSize = True

            lbCancelados.Font = fontBold_

            lbCancelados.ForeColor = colorGray_

            lbCancelados.Text = "Cancelados"

            lbCancelados.Name = "Cancelados"

            pbCancelados.Size = sizeProgress_

            pbCancelados.Value = d_PorcentajeCancelados_

            lbPorcentajeCancelados.AutoSize = True

            lbPorcentajeCancelados.Font = fontBold_

            lbPorcentajeCancelados.ForeColor = colorGray_

            lbPorcentajeCancelados.Text = d_PorcentajeCancelados_ & "%"

            toolTip_.SetToolTip(lbPorcentajeCancelados, i_Cancelados_ & "/" & i_Registrados_)


            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

            TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1


            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

            TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

            TableLayoutPanel1.Controls.Add(lbNombreRecolector, 0, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(lbFechaUltimaRecoleccion, 1, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

            TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1


            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

            TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

            TableLayoutPanel1.Controls.Add(lbGeneral, 0, TableLayoutPanel1.RowCount - 1)


            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

            TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

            TableLayoutPanel1.Controls.Add(lbReconocidos, 0, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(pbReconocidos, 1, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(lbPorcentajeReconocidos, 2, TableLayoutPanel1.RowCount - 1)


            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

            TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

            TableLayoutPanel1.Controls.Add(lbRecolectados, 0, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(pbRecolectados, 1, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(lbPorcentajeRecolectados, 2, TableLayoutPanel1.RowCount - 1)


            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

            TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

            TableLayoutPanel1.Controls.Add(lbErrores, 0, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(pbErrores, 1, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(lbPorcentajeErrores, 2, TableLayoutPanel1.RowCount - 1)


            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

            TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

            TableLayoutPanel1.Controls.Add(lbCancelados, 0, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(pbCancelados, 1, TableLayoutPanel1.RowCount - 1)

            TableLayoutPanel1.Controls.Add(lbPorcentajeCancelados, 2, TableLayoutPanel1.RowCount - 1)

        End If

    End Sub

    Private Sub DibujarControlesTiposDocumentos(ByVal i_Cve_RecolectorDocumentos_ As String)

        Dim dataTable_ As DataTable = Nothing

        Dim font_ As Font = New Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Dim fontBoldItalic_ As Font = New Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic),  _
                                                System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))


        Dim colorGray_ As Color = Color.SlateGray

        Dim sizeProgress_ As Size = New Size(211, 10)

        Dim lbTiposDocumentos As New Label()

        Dim toolTip_ As New ToolTip()

        lbTiposDocumentos.AutoSize = True

        lbTiposDocumentos.Font = fontBoldItalic_

        lbTiposDocumentos.ForeColor = colorGray_

        lbTiposDocumentos.Text = "Tipos de documentos"


        TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

        TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

        TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

        TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

        TableLayoutPanel1.Controls.Add(lbTiposDocumentos, 0, TableLayoutPanel1.RowCount - 1)


        dataTable_ = _recolector.ConsultarResumenTiposDocumentos("AND tip.i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_)


        If Not dataTable_ Is Nothing And dataTable_.Rows.Count > 0 Then

            For Each row_ As DataRow In dataTable_.Rows

                Dim t_NombreDoc_ As String = row_.Item("t_Nombre")

                Dim i_ReconocidosDoc_ As Integer = row_.Item("i_Reconocidos")

                Dim i_RecolectadosDoc_ As Integer = row_.Item("i_Recolectados")


                Dim lbTipoDoc As New Label()

                Dim pbTipoDoc As New ProgressBar()

                Dim lbpTipoDoc As New Label()

                Dim d_Porcentaje_ As Double = 0


                lbTipoDoc.AutoSize = True

                lbTipoDoc.AutoEllipsis = True

                lbTipoDoc.Font = font_

                lbTipoDoc.ForeColor = colorGray_

                lbTipoDoc.Text = t_NombreDoc_

                lbTipoDoc.Name = t_NombreDoc_


                If i_ReconocidosDoc_ > 0 Then

                    d_Porcentaje_ = Math.Round((i_RecolectadosDoc_ / i_ReconocidosDoc_) * 100, 2)

                End If

                pbTipoDoc.Size = sizeProgress_

                pbTipoDoc.Value = d_Porcentaje_


                lbpTipoDoc.AutoSize = True

                lbpTipoDoc.Font = font_

                lbpTipoDoc.ForeColor = colorGray_

                lbpTipoDoc.Text = d_Porcentaje_ & "%"

                toolTip_.SetToolTip(lbpTipoDoc, i_RecolectadosDoc_ & "/" & i_ReconocidosDoc_)


                TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

                TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

                TableLayoutPanel1.Controls.Add(lbTipoDoc, 0, TableLayoutPanel1.RowCount - 1)

                TableLayoutPanel1.Controls.Add(pbTipoDoc, 1, TableLayoutPanel1.RowCount - 1)

                TableLayoutPanel1.Controls.Add(lbpTipoDoc, 2, TableLayoutPanel1.RowCount - 1)

            Next

        End If

    End Sub

    Private Sub DibujarControlesCodigoError(ByVal i_Cve_RecolectorDocumentos_ As String)

        Dim dataTable_ As DataTable = Nothing

        Dim font_ As Font = New Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Dim fontBoldItalic_ As Font = New Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic),  _
                                                System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Dim colorGray_ As Color = Color.SlateGray

        Dim sizeProgress_ As Size = New Size(211, 10)

        Dim lbCodigosError As New Label()

        Dim toolTip_ As New ToolTip()


        lbCodigosError.AutoSize = True

        lbCodigosError.Font = fontBoldItalic_

        lbCodigosError.ForeColor = colorGray_

        lbCodigosError.Text = "Códigos de error"


        TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

        TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

        TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

        TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

        TableLayoutPanel1.Controls.Add(lbCodigosError, 0, TableLayoutPanel1.RowCount - 1)


        dataTable_ = _recolector.ConsultarResumenErrores("AND err.i_Cve_RecolectorDocumentos = " & i_Cve_RecolectorDocumentos_)


        If Not dataTable_ Is Nothing And dataTable_.Rows.Count > 0 Then

            For Each row_ As DataRow In dataTable_.Rows

                Dim t_CodigoError_ As String = row_.Item("t_CodigoError")

                Dim i_TotalErrores_ As Integer = row_.Item("i_TotalErrores")

                Dim i_ErroresCodigo_ As Integer = row_.Item("i_ErroresCodigo")

                Dim lbError As New Label()

                Dim pbError As New ProgressBar()

                Dim lbpError As New Label()

                Dim d_PorcentajeError_ As Double = 0


                lbError.AutoSize = True

                lbError.AutoEllipsis = True

                lbError.Font = font_

                lbError.ForeColor = colorGray_

                lbError.Text = t_CodigoError_

                lbError.Name = t_CodigoError_


                Dim enumErrores_ As TagWatcher.ErrorTypes = DirectCast([Enum].Parse(GetType(TagWatcher.ErrorTypes), t_CodigoError_), TagWatcher.ErrorTypes)

                toolTip_.SetToolTip(lbError, _tag.GetEnumDescription(enumErrores_))


                If i_TotalErrores_ > 0 Then

                    d_PorcentajeError_ = Math.Round((i_ErroresCodigo_ / i_TotalErrores_) * 100, 2)

                End If


                pbError.Size = sizeProgress_

                pbError.Value = d_PorcentajeError_

                lbpError.AutoSize = True

                lbpError.Font = font_

                lbpError.ForeColor = colorGray_

                lbpError.Text = d_PorcentajeError_ & "%"

                toolTip_.SetToolTip(lbpError, i_ErroresCodigo_ & "/" & i_TotalErrores_)


                TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))

                TableLayoutPanel1.RowCount = TableLayoutPanel1.RowCount + 1

                TableLayoutPanel1.Controls.Add(lbError, 0, TableLayoutPanel1.RowCount - 1)

                TableLayoutPanel1.Controls.Add(pbError, 1, TableLayoutPanel1.RowCount - 1)

                TableLayoutPanel1.Controls.Add(lbpError, 2, TableLayoutPanel1.RowCount - 1)

            Next

        End If

    End Sub

    Private Sub LiberarMemoria()

        GC.Collect()

        GC.WaitForPendingFinalizers()

    End Sub

#End Region

#Region "Eventos"

    Private Sub timerResumen_Tick(sender As Object, e As EventArgs) Handles timerResumen.Tick

        lbFechaActualizacion.Text = DateTime.Now.ToString

        If TabControl1.SelectedIndex = 0 Then

            MostrarResumen()

        End If

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged

        Select Case TabControl1.SelectedIndex

            Case 0  'Resumen
                GsCatalogo1.TsBuscar_Click(Nothing, Nothing)                

            Case 1  'Bitacora
                GsCatalogo2.TsBuscar_Click(Nothing, Nothing)

            Case 2  'Errores
                GsCatalogo3.TsBuscar_Click(Nothing, Nothing)

        End Select

    End Sub

    Private Sub MonitorRecolectorDocumentos_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

        _recolector.DetenerRecoleccion()

        LiberarMemoria()

    End Sub

    Private Sub GsCatalogo1_EventoAntesTsModificar() Handles GsCatalogo1.EventoAntesTsModificar

        lbFechaActualizacion.Text = DateTime.Now.ToString

        MostrarResumen()

    End Sub

    Private Sub timerLiberarMemoria_Tick(sender As Object, e As EventArgs) Handles timerLiberarMemoria.Tick

        LiberarMemoria()

    End Sub

    Private Sub btnLiberarMemoria_Click(sender As Object, e As EventArgs) Handles btnLiberarMemoria.Click

        LiberarMemoria()

    End Sub

#End Region

End Class
