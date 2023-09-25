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

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        _ioperacionescatalogo = New OperacionesCatalogo

        _ioperacionescatalogo = ioperacionescatalogo_

        _modalidadoperativa = tipooperacion_

        ''Control de versiones
        'VersionModulo = lblVersionModulo.Text

        'Inicializador para A1
        'InicializaFormulario(ClasesFormulario.ClaseA1,
        '                     tipooperacion_,
        '                     FuncionesTransaccionales.SinDefinir,
        '                     IOperacionesCatalogo.TiposEscritura.SinDefinir)


        Dim version_ As String = ioperacionescatalogo_.Version

        'Asignación upsert
        NombreClaveUpsert = ioperacionescatalogo_.NombreClaveUpsert

        Dim granularidad_ As String = ioperacionescatalogo_.Granularidad
        Dim entidad_ As String = ioperacionescatalogo_.Entidad
        Dim dimension_ As String = ioperacionescatalogo_.Dimension

        ioperacionescatalogo_ = _sistema.EnsamblaModulo(dimension_).Clone

        Dim listaLibrerias_ As New List(Of String)

        'Ejemplo
        'listaLibrerias_.Add("gsol.krom.Responsables64.0.0.0.0.dll")
        'listaLibrerias_.Add("gsol.krom.Mercancias64.0.0.0.0.dll")

        Me.Text = "{*} " & _ioperacionescatalogo.Nombre

        Me.Label6.Text = _ioperacionescatalogo.Nombre

        Try

            'Asignación upsert

            'NombreClaveUpsert = "i_Cve_OtrosCampos"

            Dim listaAtributos_ As Object = _ioperacionescatalogo.ObjetoRepositorio

            ConstructorContexto(ClasesFormulario.ClaseA1,
                     tipooperacion_,
                     _ioperacionescatalogo,
                     version_,
                     granularidad_,
                     dimension_,
                     entidad_,
                     Nothing,
                     listaAtributos_)

        Catch ex As Exception

            _sistema.GsDialogo("Hubo un problema al armar el formulario, asegurese de contar con la clase " & entidad_ & " compilada en IEnlace. Más detalles del error (" & ex.Message & ")")

            Me.Close()

        End Try


        '"Responsables",
        '"Vt026Responsables",
        '"Responsables")
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


    Private Sub Frm000CatPermisos_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyData
            Case Keys.Escape
                Me.Close()
        End Select
    End Sub

#End Region

End Class
