Namespace Gsol.BaseDatos.Operaciones
    Public Interface ICaracteristica

#Region "Atributos"

        Enum TiposFiltro
            SinDefinir = 0
            PorDefecto = 1
            Avanzado = 2
        End Enum

        Enum TiposCaracteristica
            cUndefined = -1
            cInt32 = 0
            cString = 1
            cBoolean = 2
            cReal = 3
            cDateTime = 4
        End Enum

        Enum TipoLlave
            SinLlave = 0
            Primaria = 1
        End Enum

        Enum TiposVisible
            No = 0
            Si = 1
            Acarreo = 2
            Impresion = 3
            Virtual = 4
            Informacion = 5
        End Enum

        Enum TiposRigorDatos
            No = 0
            Si = 1
            Opcional = 2
        End Enum

#End Region

#Region "Propiedades"

        Property Nombre As String

        Property NombreMostrar As String

        Property TipoDato As TiposCaracteristica

        Property Longitud As Int32

        Property Llave As TipoLlave

        Property Visible As TiposVisible

        Property PuedeInsertar As TiposRigorDatos

        Property PuedeModificar As TiposRigorDatos

        Property TipoFiltro As TiposFiltro

        Property ValorDefault As String

        Property ValorAsignado As String

        Property ValorFiltro As String

        Property PermisoConsulta As String

        Property Interfaz As String

        Property NameAsKey As String

        Property Reflejar As Int32

        Function Clone() As Object

#End Region

#Region "Metodos"

#End Region


    End Interface
End Namespace

