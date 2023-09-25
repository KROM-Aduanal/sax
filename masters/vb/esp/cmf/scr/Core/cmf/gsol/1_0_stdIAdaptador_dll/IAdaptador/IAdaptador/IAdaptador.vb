Namespace gsol

    Public Interface IAdaptador

#Region "Atributos"
        Enum DominiosAdptador
            Total
            Identificados
        End Enum

#End Region

#Region "Constructores"

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        WriteOnly Property EspacioTrabajo As IEspacioTrabajo
        Property Controles As ICollection
        Property DominioAdptador As DominiosAdptador

#End Region

#Region "Metodos"

        Sub HabilitaComponentes()

#End Region

    End Interface

End Namespace

