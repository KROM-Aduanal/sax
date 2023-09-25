Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones

Namespace Gsol.BaseDatos

    Public Interface IPoliticasBaseDatos

#Region "Enum"

        Enum TiposProcedimientos
            Funcion
            ProcedimientoAlmacenado
        End Enum

        Enum VerificarCampos
            Si
            No
        End Enum

#End Region

#Region "Atributos"

#End Region

#Region "Contructores"

#End Region

#Region "Propiedades"

        ReadOnly Property GetNumeroPolitica As String

        ReadOnly Property GetNombrePolitica As String

        ReadOnly Property GetParametrosPolitica As String

        WriteOnly Property SetPermiso As Integer

        ReadOnly Property GetPermiso As Integer

        ReadOnly Property GetTagWatcher As TagWatcher

        Property IOperacionesCatalogo As IOperacionesCatalogo

        Property TipoProcedimiento As TiposProcedimientos

        Property VerificaCampos As VerificarCampos

        ReadOnly Property ObjetoRepositorio As String

#End Region

#Region "Metodos"

#End Region

    End Interface

End Namespace

