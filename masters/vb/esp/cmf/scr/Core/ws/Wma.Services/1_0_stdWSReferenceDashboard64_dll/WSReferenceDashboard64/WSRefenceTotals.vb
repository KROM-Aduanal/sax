' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
Public Class WSRefenceTotals
    Implements IWSReferenceDashboard


    'Public Function GetData(ByVal value As Integer) As String _
    '    Implements IWSReferenceDashboard.GetData

    '    Return String.Format("You entered: {0}", value)

    'End Function

    'Public Function GetDataUsingDataContract(ByVal composite As CompositeType) As CompositeType _
    '    Implements IWSReferenceDashboard.GetDataUsingDataContract

    '    If composite Is Nothing Then
    '        Throw New ArgumentNullException("composite")
    '    End If

    '    If composite.BoolValue Then
    '        composite.StringValue &= "Suffix"
    '    End If
    '    Return composite

    'End Function

    Public Function EjecutivoCuentaResposable(ByVal referencia_ As Integer) As String _
        Implements IWSReferenceDashboard.EjecutivoCuentaResposable

        Return "Pedro Bautista"

    End Function

    Public Function TotalOperaciones(ByVal oficina_ As String) As Integer _
        Implements IWSReferenceDashboard.TotalOperaciones
        Return 23

    End Function
End Class
