Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

Namespace gsol

    Friend Module Extensiones

#Region "Métodos"

        ''' <summary>
        ''' Obtiene la descripción asociada al valor de un elemento de una enumeración
        ''' </summary>
        ''' <param name="item_">Valor que representa un elemento de una enumeración</param>
        ''' <returns>Descripción asociada a un elemento de una enumeración</returns>
        <Extension()>
        Friend Function Descripcion(
            ByVal item_ As System.Enum
        ) As String
            'OBTIENE LA INFORMACIÓN (O ATRIBUTOS) ASOCIADOS AL VALOR DE UNA ENUMERACIÓN
            Dim informacionAtributo_ As FieldInfo = item_.GetType.GetField(item_.ToString)
            'OBTIENE EL VALOR DEL ATRIBUTO "Description", A PARTIR DE LA INFORMACIÓN ASOCIADA AL VALOR DE UNA ENUMERACIÓN
            Dim atributoDescripcion_ As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(informacionAtributo_, GetType(DescriptionAttribute), False), DescriptionAttribute)

            If atributoDescripcion_ IsNot Nothing Then
                Return atributoDescripcion_.Description
            End If

            Return item_.ToString
        End Function

#End Region

    End Module

End Namespace
