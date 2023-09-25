Imports gsol.krom

Namespace gsol.krom

    Public Class FormulariosFeedBack
        Inherits EntidadDatos

#Region "Enums"

        Public Enum CategoriasFormularios
            i_Cve_CategoriaFormulario
            t_Nombre
            i_Cve_Estatus
            t_Estatus
            i_Cve_Estado
        End Enum

        Public Enum SubCategoriasFormularios
            i_Cve_SubCategoriaFormulario
            t_Nombre
            i_Cve_CategoriaFormulario
            t_Categoria
            i_Cve_Estatus
            t_Estatus
            i_Cve_Estado
        End Enum

        Public Enum TiposPreguntas
            i_Cve_TipoPregunta
            t_Nombre
            t_Descripcion
            i_Cve_Estatus
            t_Estatus
            i_Cve_Estado
        End Enum

        Public Enum FormatosValidaciones
            i_Cve_FormatoValidacion
            t_Nombre
            i_TipoDato
            t_TipoDato
            t_MascaraFormato
            t_MensajeErrorFormato
            i_Cve_TipoPregunta
            t_TipoPregunta
            i_Cve_Estatus
            t_Estatus
            i_Cve_Estado
        End Enum

        Public Enum ReglasValidaciones
            i_Cve_ReglaValidacion
            t_Nombre
            t_Descripcion
            i_Cve_FormatoValidacion
            t_FormatoValidacion
            t_TipoPregunta
            i_TipoDato
            t_TipoDato
            t_MascaraFormato
            t_MensajeErrorFormato
            i_ValorInicial
            t_ValorInicial
            i_ValorFinal
            t_ValorFinal
            t_MensajeErrorValidacion
            i_Cve_Estatus
            t_Estatus
            i_Cve_Estado
        End Enum

#End Region

#Region "Constructores"

        Sub New()

            Dimension = IEnlaceDatos.TiposDimension.FormulariosFeedBack

        End Sub

#End Region

    End Class

End Namespace