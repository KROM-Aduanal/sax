Public Class Customer
    Implements ICustomer
    Public Property customerid As Integer _
        Implements ICustomer.customerid
    Public Property customername As String _
        Implements ICustomer.customername
    Public Property rfc As String _
        Implements ICustomer.rfc
    Public Property address As String _
        Implements ICustomer.address
    Public Property street As String _
        Implements ICustomer.street
    Public Property externalnumber As String _
        Implements ICustomer.externalnumber
    Public Property internalnumber As String _
        Implements ICustomer.internalnumber
    Public Property zipcode As String _
        Implements ICustomer.zipcode
    Public Property city As String _
        Implements ICustomer.city
    Public Property locality As String _
        Implements ICustomer.locality
    Public Property state As String _
        Implements ICustomer.state
    Public Property country As String _
        Implements ICustomer.country
End Class
