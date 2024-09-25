Public Class Supplier
    Implements ISupplier
    Public Property supplierid As Integer _
        Implements ISupplier.supplierid
    Public Property supliername As String _
        Implements ISupplier.supliername
    Public Property taxid As String _
        Implements ISupplier.taxid
    Public Property address As String _
        Implements ISupplier.address
    Public Property street As String _
        Implements ISupplier.street
    Public Property externalnumber As String _
        Implements ISupplier.externalnumber
    Public Property internalnumber As String _
        Implements ISupplier.internalnumber
    Public Property zipcode As String _
        Implements ISupplier.zipcode
    Public Property locality As String _
        Implements ISupplier.locality
    Public Property city As String _
        Implements ISupplier.city
    Public Property state As String _
        Implements ISupplier.state
    Public Property country As String _
        Implements ISupplier.country
End Class
