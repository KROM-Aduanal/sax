Imports System.Windows.Forms
Imports Gsol.BaseDatos.Operaciones

Namespace gsol.controles

    Public Class TipoDatos64

#Region "Constructor"

        Sub New()

        End Sub

        Sub New(ByVal control_ As Object, ByVal tipoDato_ As ICaracteristica.TiposCaracteristica, ByVal evento_ As KeyPressEventArgs)

            Select Case tipoDato_

                Case ICaracteristica.TiposCaracteristica.cReal

                    PermitirNumerosDecimales(control_, evento_)

            End Select

        End Sub

#End Region

#Region "Métodos"

        Private Sub PermitirNumerosDecimales(ByRef control_ As TextBox, ByRef evento_ As KeyPressEventArgs)

            If Char.IsDigit(evento_.KeyChar) Then

                evento_.Handled = False

            ElseIf Char.IsControl(evento_.KeyChar) Then

                evento_.Handled = False

            ElseIf evento_.KeyChar = "." And Not control_.Text.IndexOf(".") Then

                evento_.Handled = True

            ElseIf evento_.KeyChar = "." Then

                evento_.Handled = False

            Else

                evento_.Handled = True

            End If

        End Sub

#End Region

    End Class

End Namespace