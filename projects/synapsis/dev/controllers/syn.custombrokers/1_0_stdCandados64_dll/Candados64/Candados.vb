﻿Imports Wma.Exceptions

Public Class Candados : Implements ICloneable

#Region "Enums"
#End Region

#Region "Propiedades"

    Property NumeroCandado As String

    Property PrimeraRevision As String

    Property SegundaRevision As String

#End Region

#Region "Metodos"

    Public Sub Add()

    End Sub

    Public Sub Delete()

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

#End Region

End Class