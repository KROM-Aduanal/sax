Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Wma.Exceptions

Public Interface IControladorAcuseValor : Inherits IDisposable


#Region "Propiedades"

    Property AcusesValorGenerados As List(Of ConstructorAcuseValor)

    Property BulkCamposPedidos As Dictionary(Of ObjectId, List(Of Nodo))

    Property Estado As TagWatcher

#End Region

#Region "Métodos"

    Function GenerarAcuseValor(constructorAcuseValor_ As ConstructorAcuseValor,
                               Optional adendar_ As Boolean = False) As TagWatcher

    Function ConsultaAcusesValor(idAcusesValor_ As List(Of ObjectId),
                                 Optional campos_ As Dictionary(Of [Enum], List(Of [Enum])) = Nothing) As TagWatcher

    Function ConsultaAcuseValor(idAcuseValor_ As ObjectId,
                                Optional campos_ As Dictionary(Of [Enum], List(Of [Enum])) = Nothing) As TagWatcher


    Function DescargarXML(idAcuseValor_ As ObjectId) As TagWatcher

    Function DescargarXML(idAcuses_ As List(Of ObjectId)) As TagWatcher
    Function DescargarPDF(idAcuseValor_ As ObjectId) As TagWatcher

    Function DescargarPDF(idAcusesValor_ As List(Of ObjectId)) As TagWatcher

    Function ObtenerAcuseValor(idAcuseValor_ As ObjectId) As TagWatcher _


#End Region

End Interface



