Imports Syn.Documento
Imports Wma.Exceptions

Public Interface IPrevalidadorSynapsis : Inherits ICloneable, IDisposable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property Pedimento As DocumentoElectronico

    Property InformePrevalidacion As InformePrevalidacion

    Property CartaInstrucciones As CartaInstruccionesSynapsis

    Property Status As TagWatcher

#End Region

#Region "Metodos"

    Function GenerarInformePrevalidacion(ByVal pedimento_ As Pedimento) As TagWatcher

    Function GuardarInformePrevalidacion(ByVal pedimento_ As Pedimento) As TagWatcher

#End Region

End Interface