Imports Syn.Documento
Imports Wma.Exceptions

Public Interface IPrevalidadorSynapsis : Inherits ICloneable, IDisposable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property PedimentoSynapsis As DocumentoElectronico

    Property InformePrevalidacion As InformePrevalidacion

    Property EstatusSynapsis As TagWatcher

#End Region

#Region "Metodos"

    Function CargaDocumentoElectronico(ByVal pedimentoSynpasis_ As DocumentoElectronico) As TagWatcher

    Function GenerarInformePrevalidacion(ByVal pedimento_ As DocumentoElectronico) As TagWatcher

#End Region

End Interface