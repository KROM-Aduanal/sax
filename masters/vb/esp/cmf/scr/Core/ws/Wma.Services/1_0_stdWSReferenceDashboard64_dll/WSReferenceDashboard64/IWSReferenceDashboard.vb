' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
<ServiceContract()>
Public Interface IWSReferenceDashboard

    '<OperationContract()>
    'Function GetData(ByVal value As Integer) As String

    '<OperationContract()>
    'Function GetDataUsingDataContract(ByVal composite As CompositeType) As CompositeType

    <OperationContract()>
    Function TotalOperaciones(ByVal oficina_ As String) As Int32

    Function EjecutivoCuentaResposable(ByVal referencia_ As Int32) As String



    ' TODO: agregue aquí sus operaciones de servicio

End Interface

' Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.

<DataContract()>
Public Class CompositeType

    <DataMember()>
    Public Property BoolValue() As Boolean

    <DataMember()>
    Public Property StringValue() As String

End Class
