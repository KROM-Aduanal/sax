Public Class Item
    Implements IItem
    Public Property sec As Integer _
        Implements IItem.sec
    Public Property productid As String _
        Implements IItem.productid
    Public Property sku As String _
        Implements IItem.sku
    Public Property partnumber As String _
        Implements IItem.partnumber
    Public Property quantity As Integer _
        Implements IItem.quantity
    Public Property unit As String _
        Implements IItem.unit
    Public Property description As String _
        Implements IItem.description
    Public Property total As Double _
        Implements IItem.total
    Public Property currency As String _
        Implements IItem.currency
    Public Property usdvalue As Double _
        Implements IItem.usdvalue
    Public Property value As Double _
        Implements IItem.value
    Public Property discount As Decimal _
        Implements IItem.discount
    Public Property unitprice As Double _
        Implements IItem.unitprice
    Public Property netweight As Double _
        Implements IItem.netweight
    Public Property purchaseorder As String _
        Implements IItem.purchaseorder

    Public Property destinationcountry As String _
        Implements IItem.destinationcountry
    Public Property origincountry As String _
        Implements IItem.origincountry
End Class