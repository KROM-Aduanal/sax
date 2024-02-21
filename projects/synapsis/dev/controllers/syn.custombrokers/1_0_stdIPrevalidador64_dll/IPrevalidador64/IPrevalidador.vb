Imports Wma.Exceptions

Public Interface IPrevalidador : Inherits ICloneable, IDisposable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property TipoProcesamiento As [Enum]

    Property TipoValidacion As [Enum]

    Property Status As TagWatcher

#End Region

#Region "Metodos"

    'no se puede generar sobre carga cuando solo el tipo de devolución es diferente

    Function AnalisisValidacion(Of t)(ByVal tipoProcesamiento_ As [Enum],
                                       ByVal tipoValidacion_ As [Enum],
                                       ByVal pedimento_ As Pedimento) As t

    Function GenerarValidacion() As TagWatcher

#End Region

End Interface