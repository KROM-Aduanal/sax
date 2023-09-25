Imports gsol.krom

Namespace gsol

    Public Interface IEspacioTrabajo

#Region "Atributos"

        Enum ModalidadesEspacio
            Pruebas = 0
            Produccion = 1
            PruebasExtranet = 2
            ProduccionExtranet = 3
        End Enum

        Enum TipoModulo
            Grafico = 8
            Abstracto = 10
            AccesoRapido = 12
            Reportes = 27
            'GraficoExtranet = 28
        End Enum

#End Region

#Region "Propiedades"

        Property ModalidadEspacio As ModalidadesEspacio

        WriteOnly Property Usuario As String

        Property ClaveEjecutivo As Integer

        WriteOnly Property GrupoEmpresarial As Int32

        WriteOnly Property DivisionEmpresarial As Int32

        WriteOnly Property Aplicacion As Int32

        Property Idioma As Int32

        Property SectorEntorno As Dictionary(Of Int32, ISectorEntorno)

        ReadOnly Property EstatusConstruccion As IConstructorVisual.EstatusConstruccion

        Property MisCredenciales As ICredenciales

#End Region

#Region "Metodos"

        Sub GenerarEspacioTrabajo()

        Sub ConstruirEntorno(ByVal contenedor_ As IList, ByVal manejadoresEventos_ As List(Of System.Reflection.MethodInfo))

        Function BuscaPermiso(ByVal identificador_ As Int32, ByVal tipomodulo_ As TipoModulo) As Boolean

        Function ListaPermisosModulo(ByVal tipoModulo_ As TipoModulo) As Dictionary(Of Int32, String)

#End Region

    End Interface

End Namespace



