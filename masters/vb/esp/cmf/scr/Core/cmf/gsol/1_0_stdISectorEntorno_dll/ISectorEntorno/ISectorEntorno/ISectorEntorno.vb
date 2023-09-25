Namespace gsol

    Public Interface ISectorEntorno

#Region "Atributos"

        Enum TiposSector
            SinDefinir = 0
            SectorGrafico = 1
            SectorPermisosLibres = 2
        End Enum

#End Region

#Region "Propiedades"

        Property Identitificador As Integer
        ReadOnly Property NombreSector As String
        Property Permisos As Dictionary(Of Int32, IPermisos)


        Property Usuario As String
        Property Password As String
        Property FechaBinario As Date
        Property DivisionEmpresarial As Int32
        Property Idioma As Int32

#End Region

#Region "Metodos"

#End Region

    End Interface

End Namespace


