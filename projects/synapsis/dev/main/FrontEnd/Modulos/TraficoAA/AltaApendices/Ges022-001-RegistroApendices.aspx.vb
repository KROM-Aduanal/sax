
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports gsol.krom
Imports gsol.Web.Components
Imports gsol.krom.Anexo22.Vt022AduanaSeccionA01

'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports Sax.Web.ControladorBackend.Datos
Imports Sax.Web.ControladorBackend.Cookies


'OBJETOS BIDIMENSIONALES (ODF's.  Dependencias Krombase/SQL Server)
Imports gsol
#End Region

Public Class Ges022_001_RegistroApendices
    Inherits ControladorBackend


    Public Overrides Sub Inicializa()

        catalogo1.DataEntity = New krom.Anexo22()

        'MsgBox("CHEQUEMOS")


        catalogo2.DataEntity = New krom.Anexo22()

        catalogo3.DataEntity = New krom.Anexo22()

        catalogo4.DataEntity = New krom.Anexo22()

        catalogo5.DataEntity = New krom.Anexo22()

        catalogo6.DataEntity = New krom.Anexo22()

        catalogo7.DataEntity = New krom.Anexo22()

        catalogo8.DataEntity = New krom.Anexo22()

        catalogo9.DataEntity = New krom.Anexo22()

        catalogo10.DataEntity = New krom.Anexo22()

        catalogo11.DataEntity = New krom.Anexo22()

        catalogo12.DataEntity = New krom.Anexo22()

        catalogo13.DataEntity = New krom.Anexo22()

        catalogo14.DataEntity = New krom.Anexo22()

        catalogo15.DataEntity = New krom.Anexo22()

        catalogo16.DataEntity = New krom.Anexo22()

        catalogo18.DataEntity = New krom.Anexo22()

        catalogo21.DataEntity = New krom.Anexo22()

    End Sub

    Public Overrides Sub InicializaBotonera()

        If Not Page.IsPostBack Then

            __SYSTEM_MODULE_FORM.Modality = FormControl.ButtonbarModality.Reading

        End If

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        GuardarApendices()

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        catalogo1.UserInteraction = True

        catalogo2.UserInteraction = True

        catalogo3.UserInteraction = True

        catalogo3.Collapsed = True

        catalogo4.UserInteraction = True

        catalogo5.UserInteraction = True

        catalogo6.UserInteraction = True

        catalogo7.UserInteraction = True

        catalogo8.UserInteraction = True

        catalogo9.UserInteraction = True

        catalogo10.UserInteraction = True

        catalogo11.UserInteraction = True

        catalogo12.UserInteraction = True

        catalogo13.UserInteraction = True

        catalogo14.UserInteraction = True

        catalogo15.UserInteraction = True

        catalogo16.UserInteraction = True

        catalogo18.UserInteraction = True

        catalogo21.UserInteraction = True

        PreparaBotonera(FormControl.ButtonbarModality.Writting)

    End Sub


    Private Sub GuardarApendices()

        catalogo1.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022AduanaSeccionA01

                              EntityDataBinding(entidad_, dataRow_, catalogo1.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo2.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022ClavesPedimentoA02

                              EntityDataBinding(entidad_, dataRow_, catalogo2.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo3.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022MediosTransporteA03

                              EntityDataBinding(entidad_, dataRow_, catalogo3.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo4.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022PaisesA04

                              EntityDataBinding(entidad_, dataRow_, catalogo4.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo5.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022MonedasA05

                              EntityDataBinding(entidad_, dataRow_, catalogo5.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo6.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022RecintosFiscalizadosA06

                              EntityDataBinding(entidad_, dataRow_, catalogo6.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo7.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022UnidadesMedidaA07

                              EntityDataBinding(entidad_, dataRow_, catalogo7.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo8.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022IdentificadoresA08

                              EntityDataBinding(entidad_, dataRow_, catalogo8.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo9.ForEach(Sub(dataRow_ As Object)

                              Dim entidad_ As IEntidadDatos = New Anexo22

                              entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022RegulacionesRestriccionesNoArancelariasA09

                              EntityDataBinding(entidad_, dataRow_, catalogo9.KeyField)

                              Me.RealizaUPSERT(entidad_:=entidad_)

                          End Sub)


        catalogo10.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022TiposContenedoresVehiculosTransporteA10

                               EntityDataBinding(entidad_, dataRow_, catalogo10.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


        catalogo11.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022MetodosValoracionA11

                               EntityDataBinding(entidad_, dataRow_, catalogo11.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


        catalogo12.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022ContribucionesA12

                               EntityDataBinding(entidad_, dataRow_, catalogo12.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


        catalogo13.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022FormasPagoA13

                               EntityDataBinding(entidad_, dataRow_, catalogo13.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


        catalogo14.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022TerminosFacturacionA14

                               EntityDataBinding(entidad_, dataRow_, catalogo14.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


        catalogo15.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022DestinosMercanciasA15

                               EntityDataBinding(entidad_, dataRow_, catalogo15.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


        catalogo16.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022RegimenesA16

                               EntityDataBinding(entidad_, dataRow_, catalogo16.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


        catalogo18.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022TiposTasasA18

                               EntityDataBinding(entidad_, dataRow_, catalogo18.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


        catalogo21.ForEach(Sub(dataRow_ As Object)

                               Dim entidad_ As IEntidadDatos = New Anexo22

                               entidad_.Dimension = IEnlaceDatos.TiposDimension.Vt022RecintosFiscalizadosEstrategicosA21

                               EntityDataBinding(entidad_, dataRow_, catalogo21.KeyField)

                               Me.RealizaUPSERT(entidad_:=entidad_)

                           End Sub)


    End Sub

End Class




