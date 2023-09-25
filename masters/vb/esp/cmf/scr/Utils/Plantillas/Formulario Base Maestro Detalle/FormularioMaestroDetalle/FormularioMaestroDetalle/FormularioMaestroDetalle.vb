Imports gsol.BaseDatos.Operaciones
Imports gsol

Public Class Frm000Generico
    Inherits FormularioBaseMaestroDetalle


#Region "Atributos"

    ''EJEMPLO
    'Private _claverequisicion As String

#End Region

#Region "Constructores"

    Public Sub New(ByVal ioperacionescatalogo_ As IOperacionesCatalogo, _
            ByVal tipooperacion_ As IOperacionesCatalogo.TiposOperacionSQL)

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        _ioperacionescatalogo = New OperacionesCatalogo

        _ioperacionescatalogo = ioperacionescatalogo_

        _modalidadoperativa = tipooperacion_

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        _sistema = New Organismo

        '--------IMPLEMENTACION ADICIONAL 

        Select Case tipooperacion_
            Case IOperacionesCatalogo.TiposOperacionSQL.Insercion
                LblAccion.Text = "Nuevo registro"

                'EJEMPLO
                'If _ioperacionescatalogo.TipoEscritura = IOperacionesCatalogo.TiposEscritura.MemoriaIntermedia Then

                '    _claverequisicion = "-1"

                'Else
                '    _claverequisicion = _ioperacionescatalogo.OperacionAnterior.CampoPorNombre("i_Cve_Requisicion")

                'End If

            Case IOperacionesCatalogo.TiposOperacionSQL.Modificar

                LblAccion.Text = "Edición"

                PreparaModificacion()

            Case Else

        End Select

        '---------------------------------

    End Sub

#End Region

#Region "Metodos"


    Public Overrides Sub PreparaModificacion()
        'EJEMPLO
        'TbClaveMaterial.Text = _ioperacionescatalogo.CampoPorNombre("i_Cve_Material")

        'TbDescripcion.Text = _ioperacionescatalogo.CampoPorNombre("t_Descripcion")

        'TbCantidad.Text = _ioperacionescatalogo.CampoPorNombre("i_CantidadRequerida")

    End Sub

    Public Overrides Sub RealizarInsercion()
        'EJEMPLO
        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Requisicion") = _claverequisicion

        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Material") = TbClaveMaterial.Text

        '_ioperacionescatalogo.CampoPorNombre("t_Descripcion") = TbDescripcion.Text

        '_ioperacionescatalogo.CampoPorNombre("i_CantidadRequerida") = TbCantidad.Text

        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Estado") = "1"

    End Sub

    Public Overrides Sub RealizarModificacion()
        'EJEMPLO
        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Material") = TbClaveMaterial.Text

        '_ioperacionescatalogo.CampoPorNombre("t_Descripcion") = TbDescripcion.Text

        '_ioperacionescatalogo.CampoPorNombre("i_CantidadRequerida") = TbCantidad.Text

        '_ioperacionescatalogo.CampoPorNombre("i_Cve_Estado") = "1"

    End Sub

#End Region


End Class
