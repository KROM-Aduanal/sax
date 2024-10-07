Imports Ia.Analysis
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

<Serializable()>
Public Class CommercialInvoiceAnalysis
    Implements ICommercialInvoice, IAnalysisDocument
    Public Property _id As ObjectId _
        Implements ICommercialInvoice._id
    Public Property invoicenumber As String _
        Implements ICommercialInvoice.invoicenumber
    Public Property invoicedate As Date _
        Implements ICommercialInvoice.invoicedate
    Public Property invoiceseries As String _
        Implements ICommercialInvoice.invoiceseries
    Public Property customername As String _
        Implements ICommercialInvoice.customername
    Public Property suppliername As String _
        Implements ICommercialInvoice.suppliername
    Public Property invoicecountry As String _
        Implements ICommercialInvoice.invoicecountry
    Public Property totalinvoice As Double _
        Implements ICommercialInvoice.totalinvoice
    Public Property invoicecurrency As String _
        Implements ICommercialInvoice.invoicecurrency
    Public Property customer As Controllers.Customer _
        Implements ICommercialInvoice.customer
    Public Property supplier As Controllers.Supplier _
        Implements ICommercialInvoice.supplier
    Public Property items As List(Of Controllers.Item) _
        Implements ICommercialInvoice.items
    Public Property additionaldetails As Controllers.AdditionalDetails _
        Implements ICommercialInvoice.additionaldetails
    Public Property consigneedetails As Controllers.ConsigneeDetails _
        Implements ICommercialInvoice.consigneedetails
    Public Property processdate As Date _
        Implements IAnalysisDocument.processdate
    Public Property environmentid As Integer _
        Implements IAnalysisDocument.environmentid
    Public Property confidence As Double _
        Implements IAnalysisDocument.confidence
    Public Property info As String _
        Implements IAnalysisDocument.info
    Public Property state As String _
        Implements IAnalysisDocument.state
    Public Property analysis As Analysis _
        Implements IAnalysisDocument.analysis
End Class
