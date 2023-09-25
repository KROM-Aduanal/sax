Imports gsol.BaseDatos.Operaciones
Imports gsol



Public Class frm000Formulario
    Inherits FormularioBase64


#Region "Atributos"

#End Region

#Region "Constructores"

    Public Sub New(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
            ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        'Llamada necesaria para el diseñador.
        InitializeComponent()

        '················ Consumo del formulario mediante IEnlace ...........................

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        'ioperacionescatalogo_ = _sistema.EnsamblaModulo("Responsables").Clone

        'Dim listaLibrerias_ As New List(Of String)

        'ConstructorContexto(ClasesFormulario.ClaseA1,
        '                     tipooperacion_,
        '                     ioperacionescatalogo_,
        '                     "0.0.0.0",
        '                     "Responsables",
        '                     "Vt026Responsables",
        '                     "Responsables")


        '················ Consumo del formulario manual ...........................

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        _ioperacionescatalogo = New OperacionesCatalogo

        _ioperacionescatalogo = ioperacionescatalogo_

        _modalidadoperativa = tipooperacion_

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        _sistema = New Organismo

        Select Case tipooperacion_
            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion
                LblAccion.Text = "Nuevo registro"


            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                LblAccion.Text = "Edición"

                PreparaModificacion()

            Case Else

        End Select

    End Sub

#End Region

#Region "Métodos"

    Public Overrides Sub PreparaModificacion()

        ':::::::::::::Ejemplo de asignación directa::::::::::::::

        'TbClaveMaterial.Text = _ioperacionescatalogo.CampoPorNombre("i_Cve_Material")

        'TbDescripcion.Text = _ioperacionescatalogo.CampoPorNombre("t_Descripcion")

        'TbCantidad.Text = _ioperacionescatalogo.CampoPorNombre("i_CantidadRequerida")

    End Sub

    Public Overrides Sub RealizarInsercion()

        ':::::::::::::Ejemplo de asignación directa::::::::::::::

        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Requisicion") = _claverequisicion

        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Material") = TbClaveMaterial.Text

        '_ioperacionescatalogo.CampoPorNombre("t_Descripcion") = TbDescripcion.Text

        '_ioperacionescatalogo.CampoPorNombre("i_CantidadRequerida") = TbCantidad.Text

        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Estado") = "1"

    End Sub

    Public Overrides Sub RealizarModificacion()

        ':::::::::::::Ejemplo de asignación directa::::::::::::::

        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Material") = TbClaveMaterial.Text

        '_ioperacionescatalogo.CampoPorNombre("t_Descripcion") = TbDescripcion.Text

        '_ioperacionescatalogo.CampoPorNombre("i_CantidadRequerida") = TbCantidad.Text

        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Estado") = "1"

    End Sub

#End Region


End Class
