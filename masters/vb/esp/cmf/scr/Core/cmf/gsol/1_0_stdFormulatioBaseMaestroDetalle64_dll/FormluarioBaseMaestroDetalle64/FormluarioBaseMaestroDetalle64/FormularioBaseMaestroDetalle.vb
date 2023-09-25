Imports gsol.BaseDatos.Operaciones
Imports gsol
Imports gsol.Componentes.SistemaBase
Imports gsol.BaseDatos
Imports Wma.Exceptions.TagWatcher

Public Class FormularioBaseMaestroDetalle
    Inherits FormularioBase64

#Region "Atributos"

    Public _llaveencabezadodefault As String

    Public _coleccionllaves As Dictionary(Of Int32, ICaracteristica)

    Public _clausulalibredetalle As String

    Private _respuestaGeneralGrabadoDetalles As Boolean

#End Region

#Region "Eventos"


    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones antes de cargar datos para edición pero despúes de realizar las asignaciones nuevas")> _
    Public Event DespuesDeRelizarNuevaAsignacionAntesCometerEdicion()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera exitosa")> _
    Shadows Event DespuesInsercionCometidaExitosa()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera exitosa")> _
    Public Event DespuesInsercionCometidaExitosaMaestroDetalle()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera exitosa en los detalles")> _
    Public Event DespuesInsercionCometidaExitosaMaestroDetalleB1()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de fallar al intentar cometer datos de inserción en el detalle")> _
    Public Event DespuesInsercionCometidaFallidaMaestroDetalleB1()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera fallida")> _
    Public Shadows Event DespuesInsercionCometidaFallida()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera exitosa")> _
    Public Shadows Event DespuesModificacionCometidaExitosa()

    <System.ComponentModel.Category("Wma")> _
<System.ComponentModel.Description("Ejecuta instrucciones despúes de cometer datos de inserción de manera fallida")> _
    Public Shadows Event DespuesModificacionCometidaFallida()

#End Region


#Region "Métodos modificables a traves de la herencia múltiple"

    Public Overridable Sub AntesTsAgregar()

    End Sub

    Public Overridable Sub DespuesTsAgregar()

    End Sub

    Public Overridable Sub AntesTsModificar()

    End Sub

    Public Overridable Sub DespuesTsModificar()

    End Sub

    Public Overloads Sub InicializaFormulario(ByVal claseformulario_ As ClasesFormulario, _
                                ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        VersionCorrecta()

        _claseformulario = claseformulario_

        _funcionTransaccional = FuncionesTransaccionales.SinDefinir

        _respuestaGeneralGrabadoDetalles = False

        Select Case claseformulario_

            Case ClasesFormulario.ClaseA1
                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseA2
                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseB1
                B1IniciaRegistrosDetalle(tipooperacion_, FuncionesTransaccionales.SinDefinir)

            Case ClasesFormulario.ClaseB2
                B2IniciaRegistrosDetalle(tipooperacion_, FuncionesTransaccionales.SinDefinir, IOperacionesCatalogo.TiposEscritura.SinDefinir)

            Case ClasesFormulario.ClaseC1
                _sistema.GsDialogo("No implementado")

            Case ClasesFormulario.ClaseUn
                _sistema.GsDialogo("No se ha asignado controlador de inicio para el formulario")

        End Select

    End Sub

    Public Overloads Sub InicializaFormulario(ByVal claseformulario_ As ClasesFormulario, _
                                    ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL, _
                                    Optional ByVal funcionTransaccional_ As FuncionesTransaccionales = FuncionesTransaccionales.SinDefinir, _
                                    Optional ByVal tipoEscrituraPrevia_ As IOperacionesCatalogo.TiposEscritura = IOperacionesCatalogo.TiposEscritura.SinDefinir, _
                                    Optional ByVal complejoTransaccional_ As IOperacionesCatalogo.ComplexTypes = IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2)

        VersionCorrecta()

        _claseformulario = claseformulario_

        _funcionTransaccional = funcionTransaccional_

        Select Case claseformulario_
            Case ClasesFormulario.ClaseA1
                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseA2
                _sistema.GsDialogo("No disponible en esta tipo de instancia")

            Case ClasesFormulario.ClaseB1
                B1IniciaRegistrosDetalle(tipooperacion_, funcionTransaccional_)

            Case ClasesFormulario.ClaseB2
                B2IniciaRegistrosDetalle(tipooperacion_, funcionTransaccional_, tipoEscrituraPrevia_, complejoTransaccional_)

            Case ClasesFormulario.ClaseC1
                _sistema.GsDialogo("No implementado")

            Case ClasesFormulario.ClaseUn
                _sistema.GsDialogo("No se ha asignado controlador de inicio para el formulario")

        End Select

    End Sub

    Public Sub ObtieneIndiceDetalleB2Temporal1(ByVal tipoOperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        Select Case tipoOperacion_

            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia Or _
                    _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then


                    If _ioperacionescatalogo.ValorIndice = "-1" Or _ioperacionescatalogo.ValorIndice Is Nothing Then

                        _valorIndice = 1

                    Else

                        _valorIndice = _ioperacionescatalogo.IndiceTablaTemporal

                    End If

                End If

            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                If _ioperacionescatalogo.ValorIndice = "-1" Or _ioperacionescatalogo.ValorIndice Is Nothing Then

                    MsgBox("No ha seleccionado ningun registro para editar...")

                    _valorIndice = 1

                Else

                    _valorIndice = _ioperacionescatalogo.ValorIndice

                End If

            Case Else

        End Select

        _ioperacionescatalogo.IndiceTablaTemporalLlamante = _ioperacionescatalogo.IndiceTablaTemporal

    End Sub

    Public Sub ObtieneLLavesInicialesB2(ByRef llavePrimaria_ As String, ByRef llaveForanea1_ As String, ByVal llavePrimariaAnterior_ As String)

        Select Case _modalidadoperativa

            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia Or _
                   _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

                    llavePrimaria_ = "-1"

                    llaveForanea1_ = "-1"

                Else

                    llavePrimaria_ = "-1"

                    llaveForanea1_ = _ioperacionescatalogo.OperacionAnterior.CampoPorNombre(llavePrimariaAnterior_)

                End If

        End Select

    End Sub


    Public Overridable Sub B2IniciaRegistrosDetalle(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL, _
                                                    Optional ByVal funcionTransaccional_ As FuncionesTransaccionales = FuncionesTransaccionales.SinDefinir, _
                                                    Optional ByVal tipoEscrituraPrevia_ As IOperacionesCatalogo.TiposEscritura = IOperacionesCatalogo.TiposEscritura.SinDefinir,
                                                    Optional ByVal complejoTransaccional_ As IOperacionesCatalogo.ComplexTypes = IOperacionesCatalogo.ComplexTypes.ComplexA2B2B2A2)

        _sistema = New Organismo

        _funcionTransaccional = funcionTransaccional_

        _ioperacionescatalogo.ComplejoTransaccional = complejoTransaccional_


        Me.CatalogosTransaccion.Add(GsCatalogo1)

        For Each catalogo_ As GsCatalogo In CatalogosTransaccion

            catalogo_.OperacionesCatalogo.OperacionAnterior = New OperacionesCatalogo

            catalogo_.OperacionesCatalogo.OperacionAnterior = _ioperacionescatalogo

            catalogo_.OperacionesCatalogo.TipoOperacionSQL = tipooperacion_

        Next

        _ioperacionescatalogo.TipoOperacionSQL = tipooperacion_

        Select Case tipooperacion_

            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                    catalogo_.OperacionesCatalogo.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

                    catalogo_.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

                    catalogo_.OperacionesCatalogo.ClausulasLibres = Nothing

                Next

                If tipoEscrituraPrevia_ <> IOperacionesCatalogo.TiposEscritura.SinDefinir Then

                    _ioperacionescatalogo.TipoEscritura = tipoEscrituraPrevia_


                    For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                        catalogo_.OperacionesCatalogo.TipoEscritura = tipoEscrituraPrevia_

                    Next

                Else

                    _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                        catalogo_.OperacionesCatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    Next

                End If

                LblAccion.Text = "Nuevo registro {BT}{" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

                    Select Case _funcionTransaccional

                        Case FuncionesTransaccionales.DetalleTemporal

                            ObtieneIndiceDetalleB2Temporal1(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                        Case FuncionesTransaccionales.CierreTransaccional

                            ObtieneIndiceCierreA2(IOperacionesCatalogo.TiposOperacionSQL.Modificar)

                        Case Else

                    End Select

                End If

                PreparaModificacion()

                If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

                    Select Case _funcionTransaccional

                        Case FuncionesTransaccionales.DetalleTemporal

                            Dim indiceCatalogo_ As Int32 = 1

                            For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                                Dim iopx_ As IOperacionesCatalogo = New OperacionesCatalogo

                                Dim myKey_ As String = Nothing
                                myKey_ = _
                                _ioperacionescatalogo.IDObjetoTransaccional & "." & _
                                _ioperacionescatalogo.IDNivelTransaccional & "." & _
                                _valorIndice & "_" & _
                                indiceCatalogo_

                                iopx_ = _ioperacionescatalogo.OperacionAnterior.ColeccionInstrucciones.Item(myKey_).ColeccionInstrucciones.Item(myKey_ & "." & _valorIndice & "")

                                catalogo_.OperacionesCatalogo = iopx_

                                indiceCatalogo_ += 1

                            Next

                        Case Else

                    End Select

                End If

                For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                    catalogo_.OperacionesCatalogo.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

                    catalogo_.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

                    catalogo_.OperacionesCatalogo.ClausulasLibres = _clausulalibredetalle

                    catalogo_.OperacionesCatalogo.GenerarVista()

                Next

                If funcionTransaccional_ = FuncionesTransaccionales.GrabarTransacciones Then

                    _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                        catalogo_.OperacionesCatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    Next

                Else

                    If tipoEscrituraPrevia_ <> IOperacionesCatalogo.TiposEscritura.SinDefinir Then

                        _ioperacionescatalogo.TipoEscritura = tipoEscrituraPrevia_

                        For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                            catalogo_.OperacionesCatalogo.TipoEscritura = tipoEscrituraPrevia_

                        Next

                    Else
                        _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                        For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                            catalogo_.OperacionesCatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                        Next

                    End If

                End If

                LblAccion.Text = "Edición {BT}{" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

                For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                    catalogo_.TsBuscar_Click(Nothing, Nothing)

                Next

            Case Else

        End Select

        Select Case _funcionTransaccional
            Case FuncionesTransaccionales.GrabarTransacciones

                _ioperacionescatalogo.RegistrosTemporales.Clear()

                _ioperacionescatalogo.RegistrosTemporalesDataTable.Clear()

                _ioperacionescatalogo.IndiceTablaTemporal = 1

                _ioperacionescatalogo.ColeccionInstrucciones.Clear()

                _ioperacionescatalogo.SQLTransaccion.Clear()

                _ioperacionescatalogo.IDObjetoTransaccional = "IOPB"

                _ioperacionescatalogo.IDNivelTransaccional = "N1"

            Case FuncionesTransaccionales.DetalleTemporal

                _ioperacionescatalogo.IDObjetoTransaccional = "IOPC"

                _ioperacionescatalogo.IDNivelTransaccional = "N2"

            Case Else
                'NOT IMPLEMENTED

        End Select

        If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda Then

            Select Case _funcionTransaccional
                Case FuncionesTransaccionales.DetalleTemporal
                    ObtieneIndiceDetalleB2Temporal1(tipooperacion_)

                Case FuncionesTransaccionales.GrabarTransacciones
                    'NOT IMPLEMENTED

            End Select

        End If

    End Sub

    Public Overridable Sub B1IniciaRegistrosDetalle(ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL, _
                                                    Optional ByVal funcionTransaccional_ As FuncionesTransaccionales = FuncionesTransaccionales.SinDefinir)

        _sistema = New Organismo

        Me.CatalogosTransaccion.Add(GsCatalogo1)

        For Each catalogo_ As GsCatalogo In CatalogosTransaccion

            catalogo_.OperacionesCatalogo.OperacionAnterior = New OperacionesCatalogo

            catalogo_.OperacionesCatalogo.OperacionAnterior = _ioperacionescatalogo

        Next

        _funcionTransaccional = funcionTransaccional_

        Select Case tipooperacion_

            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                    catalogo_.OperacionesCatalogo.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

                    catalogo_.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

                    catalogo_.OperacionesCatalogo.ClausulasLibres = Nothing

                    catalogo_.OperacionesCatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                Next

                _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                LblAccion.Text = "Nuevo registro {" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                PreparaModificacion()

                _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                LblAccion.Text = "Edición {" & _ioperacionescatalogo.TipoEscritura.ToString & "}"

                For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                    catalogo_.OperacionesCatalogo.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

                    catalogo_.EspacioTrabajo = _ioperacionescatalogo.EspacioTrabajo

                    catalogo_.OperacionesCatalogo.ClausulasLibres = _clausulalibredetalle

                    'catalogo_.OperacionesCatalogo.GenerarVista()

                    catalogo_.OperacionesCatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                    catalogo_.TsBuscar_Click(Nothing, Nothing)

                Next

            Case Else

        End Select

    End Sub

    Public Overrides Sub InvocarOperacion()

        If ContinuarOperacionActual Then

            Dim observaciones_ As String = Nothing

            Dim respuestaoperacion_ As IOperacionesCatalogo.EstadoOperacion = IOperacionesCatalogo.EstadoOperacion.CSinDefinir

            If _llaveencabezadodefault Is Nothing Then

                observaciones_ = "Indique por favor el nombre de la llave del encabezado."

            Else

                observaciones_ = _ioperacionescatalogo.ValidaValoresCampos(_modalidadoperativa)

            End If

            For Each catalogo_ As GsCatalogo In CatalogosTransaccion

                If catalogo_.CatalogoOpcional Then

                    If catalogo_.OperacionesCatalogo.Vista.Tables Is Nothing Then

                        Continue For

                    Else

                        If catalogo_.OperacionesCatalogo.Vista.Tables.Count >= 1 Then

                            If catalogo_.OperacionesCatalogo.Vista.Tables(0).Rows.Count = 0 Then

                                Continue For

                            End If

                        Else

                            Continue For

                        End If

                    End If

                End If

                If (catalogo_.OperacionesCatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia Or _
                     catalogo_.OperacionesCatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda) Then

                    If Not catalogo_.OperacionesCatalogo.RegistrosTemporalesDataTable Is Nothing Then

                        If catalogo_.OperacionesCatalogo.RegistrosTemporales Is Nothing Then

                            catalogo_.OperacionesCatalogo.RegistrosTemporales = New Dictionary(Of Int32, String)

                            catalogo_.OperacionesCatalogo.RegistrosTemporales.Add(1, "OK")

                        End If

                        If (catalogo_.OperacionesCatalogo.RegistrosTemporalesDataTable.Rows.Count = 0) Then

                            observaciones_ = "Capture por favor un item para {" & catalogo_.NombreCatalogo & "}"

                        End If

                    Else

                        observaciones_ = "Capture por favor un item para {" & catalogo_.NombreCatalogo & "}"

                    End If

                End If



                If catalogo_.OperacionesCatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata And _
                    Not catalogo_.OperacionesCatalogo.Vista Is Nothing Then

                    If catalogo_.OperacionesCatalogo.Vista.Tables.Count >= 1 Then

                        If catalogo_.OperacionesCatalogo.Vista.Tables(0).Rows.Count <= 0 Then

                            observaciones_ = "Capture por favor un item para {" & catalogo_.NombreCatalogo & "}"

                        End If

                    Else

                        observaciones_ = "Capture por favor un item para {" & catalogo_.NombreCatalogo & "}"

                    End If

                End If

            Next

            If observaciones_ Is Nothing Then

                Select Case _modalidadoperativa

                    Case IOperacionesCatalogo.TiposOperacionSQL.Insercion

                        Dim grabarPlanEjecucion_ As Boolean = False

                        If _funcionTransaccional = FuncionesTransaccionales.GrabarTransacciones Then : grabarPlanEjecucion_ = True : End If

                        respuestaoperacion_ = _ioperacionescatalogo.Agregar(grabarPlanEjecucion_)

                        If respuestaoperacion_ = IOperacionesCatalogo.EstadoOperacion.COk Then

                            Select Case GsCatalogo1.OperacionesCatalogo.TipoEscritura

                                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                                    Select Case _claseformulario
                                        Case ClasesFormulario.ClaseB1

                                        Case ClasesFormulario.ClaseB2

                                            Dim llaves_ As Dictionary(Of Int32, ICaracteristica) = New Dictionary(Of Int32, ICaracteristica)

                                            Dim llave_ As ICaracteristica = New CaracteristicaCatalogo

                                            llave_.ValorAsignado = "-0.00019810210"

                                            llave_.NombreMostrar = _llaveencabezadodefault

                                            llaves_.Add(0, llave_)

                                            _coleccionllaves = llaves_

                                            GrabarCambios(GsCatalogo1.OperacionesCatalogo.TipoEscritura)

                                            If Me._funcionTransaccional = FuncionesTransaccionales.GrabarTransacciones Then

                                                _ioperacionescatalogo.EjecutaInstrucciones()

                                                'If _ioperacionescatalogo.EjecutaInstrucciones() Then

                                                '    RaiseEvent DespuesInsercionCometidaExitosaMaestroDetalle()

                                                '    RaiseEvent DespuesInsercionCometidaExitosaMaestroDetalleB1()
                                                'Else

                                                '    RaiseEvent DespuesInsercionCometidaFallidaMaestroDetalleB1()

                                                'End If

                                            End If

                                        Case Else

                                            LblMensaje.Text = "No se ha implementado acción para {" & _claseformulario.ToString & "}"

                                    End Select

                                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                                    Select Case _claseformulario

                                        Case ClasesFormulario.ClaseB1

                                            Dim llaves_ As Dictionary(Of Int32, ICaracteristica) = New Dictionary(Of Int32, ICaracteristica)

                                            Dim llave_ As ICaracteristica = New CaracteristicaCatalogo

                                            llave_.ValorAsignado = _ioperacionescatalogo.ValorIndice

                                            llave_.NombreMostrar = _llaveencabezadodefault

                                            llaves_.Add(0, llave_)

                                            _coleccionllaves = llaves_

                                            GrabarCambios()

                                            If _respuestaGeneralGrabadoDetalles Then

                                                RaiseEvent DespuesInsercionCometidaExitosaMaestroDetalleB1()

                                            Else

                                                RaiseEvent DespuesInsercionCometidaFallidaMaestroDetalleB1()

                                            End If



                                        Case ClasesFormulario.ClaseB2

                                            Dim llaves_ As Dictionary(Of Int32, ICaracteristica) = New Dictionary(Of Int32, ICaracteristica)

                                            Dim llave_ As ICaracteristica = New CaracteristicaCatalogo

                                            llave_.ValorAsignado = _ioperacionescatalogo.ValorIndice

                                            llave_.NombreMostrar = _llaveencabezadodefault

                                            llaves_.Add(0, llave_)

                                            _coleccionllaves = llaves_

                                            GrabarCambios()

                                        Case Else

                                            LblMensaje.Text = "No se ha implementado acción para {" & _claseformulario.ToString & "}"

                                    End Select

                                Case Else

                            End Select

                            RaiseEvent DespuesInsercionCometidaExitosa()

                            RaiseEvent DespuesInsercionCometidaExitosaMaestroDetalle()

                        Else
                            'eaqndlv
                            RaiseEvent DespuesInsercionCometidaFallida()


                        End If

                    Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                        Dim grabarPlanEjecucion_ As Boolean = False

                        If _funcionTransaccional = FuncionesTransaccionales.GrabarTransacciones Then : grabarPlanEjecucion_ = True : End If

                        RaiseEvent DespuesDeRelizarNuevaAsignacionAntesCometerEdicion()

                        respuestaoperacion_ = _ioperacionescatalogo.Modificar(_ioperacionescatalogo.ValorIndice)

                        If respuestaoperacion_ = IOperacionesCatalogo.EstadoOperacion.COk Then

                            RaiseEvent DespuesModificacionCometidaExitosa()

                            Select Case GsCatalogo1.OperacionesCatalogo.TipoEscritura


                                Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia
                                    Select Case _claseformulario

                                        Case ClasesFormulario.ClaseA1

                                            GrabarCambios()

                                        Case ClasesFormulario.ClaseB1

                                            GrabarCambios()

                                        Case ClasesFormulario.ClaseA2
                                            LblMensaje.Text = "NOT IMPLEMENTED"

                                        Case ClasesFormulario.ClaseB2
                                            LblMensaje.Text = "NOT IMPLEMENTED"

                                    End Select

                                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                                    Select Case _claseformulario

                                        Case ClasesFormulario.ClaseA1
                                            LblMensaje.Text = "NOT IMPLEMENTED"

                                        Case ClasesFormulario.ClaseB1
                                            LblMensaje.Text = "NOT IMPLEMENTED"

                                        Case ClasesFormulario.ClaseA2
                                            LblMensaje.Text = "NOT IMPLEMENTED"

                                        Case ClasesFormulario.ClaseB2

                                            Dim llaves_ As Dictionary(Of Int32, ICaracteristica) = New Dictionary(Of Int32, ICaracteristica)
                                            Dim llave_ As ICaracteristica = New CaracteristicaCatalogo

                                            llave_.ValorAsignado = "-0.00019810210"

                                            llave_.NombreMostrar = _llaveencabezadodefault

                                            llaves_.Add(0, llave_)

                                            _coleccionllaves = llaves_

                                            GrabarCambios(GsCatalogo1.OperacionesCatalogo.TipoEscritura)

                                            If Me._funcionTransaccional = FuncionesTransaccionales.GrabarTransacciones Then

                                                _ioperacionescatalogo.EjecutaInstrucciones()

                                            End If

                                        Case Else

                                            LblMensaje.Text = "No se ha implementado acción para {" & _claseformulario.ToString & "}"

                                    End Select

                                Case Else

                            End Select

                        Else

                            RaiseEvent DespuesInsercionCometidaFallida()

                        End If

                End Select

                If respuestaoperacion_ = IOperacionesCatalogo.EstadoOperacion.COk Then

                    LblMensaje.ForeColor = Color.Green

                    LblMensaje.Text = "Operación exitosa!"

                    If _autoCierreFormulario Then

                        Me.Close()

                    End If

                Else

                    LblMensaje.ForeColor = Color.Red

                    LblMensaje.Text = "No se ha podido realizar la operación."

                End If

            Else

                LblMensaje.Text = observaciones_

            End If

        End If

    End Sub


    Public Overridable Sub GrabarCambios(Optional ByVal _tipoEscritura As IOperacionesCatalogo.TiposEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia)

        Dim respuesta_ As Boolean = False
        'Por default afirmamos que todo saldrá bien.
        _respuestaGeneralGrabadoDetalles = True

        For Each catalogo_ As GsCatalogo In CatalogosTransaccion

            If catalogo_.CatalogoOpcional Then

                If catalogo_.OperacionesCatalogo.Vista.Tables Is Nothing Then

                    Continue For

                Else

                    If catalogo_.OperacionesCatalogo.Vista.Tables.Count >= 1 Then

                        If catalogo_.OperacionesCatalogo.Vista.Tables(0).Rows.Count = 0 Then

                            Continue For

                        End If

                    Else

                        Continue For

                    End If

                End If

            End If


            Select Case _tipoEscritura

                Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                    respuesta_ = catalogo_.OperacionesCatalogo.GrabarDatosEnDisco(_coleccionllaves, _tipoEscritura)

                Case Else

                    If _ioperacionescatalogo.ValorIndice <> "-1" And _ioperacionescatalogo.ValorIndice <> "" And Not (_ioperacionescatalogo.ValorIndice Is Nothing) Then

                        respuesta_ = catalogo_.OperacionesCatalogo.GrabarDatosEnDisco(_coleccionllaves, _tipoEscritura)

                    Else

                        LblMensaje.Text = "El valor de la llave foránea no puede ser {" & _ioperacionescatalogo.ValorIndice & "}, verifique por favor."

                    End If

            End Select


            If respuesta_ = False And catalogo_.CatalogoOpcional = False Then
                'Cualquier cosa que no sea opcional y falle, será notificada
                _respuestaGeneralGrabadoDetalles = False

            End If

            If Not catalogo_.CatalogoOpcional Then

                If Not respuesta_ Then

                    _sistema.GsDialogo("ATENCIÓN: no se han podido registrar los detalles de uno de los catálogos, por favor notifiquelo al depto de informática", GsDialogo.TipoDialogo.Alerta)

                End If

            End If

        Next




    End Sub

    Private Sub FormularioBaseMaestroDetalle_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

        Select Case _ioperacionescatalogo.TipoEscritura

            Case IOperacionesCatalogo.TiposEscritura.TransaccionBajoDemanda

                If _funcionTransaccional = FuncionesTransaccionales.GrabarTransacciones Then

                    _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

                End If

            Case IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia

                _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.Inmediata

        End Select

        For Each catalogo_ As GsCatalogo In CatalogosTransaccion

            If catalogo_.ModalidadBusqueda <> IOperacionesCatalogo.ModalidadesBusqueda.SinDefinir Then

                If Not catalogo_.ProcesoBusqueda Is Nothing Then

                    If catalogo_.ProcesoBusqueda.IsAlive Then

                        catalogo_.ProcesoBusqueda.Interrupt()

                        catalogo_.ProcesoBusqueda.Abort()

                        While catalogo_.ProcesoBusqueda.IsAlive : End While

                    End If

                End If

            End If

        Next

    End Sub

#End Region

End Class
