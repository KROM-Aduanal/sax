Imports gsol.basededatos

Namespace gsol

	Public Interface ILineaBase

#Region "Atributos"

		Enum TipoLineaBase
			ConsultaOperacionesSIR = 0
			ConsultaOperaconesSAAI
			ActualizaOperacionesSIR
			ActualizaOperacionesSAAI
			InsertaBitacoraOperacionesSIR
			InsertaBitacoraOperacionesSAAI
		End Enum
		Enum TipoComando
			Consulta = 0
			Inserta
			Borra
			Actualiza
		End Enum

#End Region

#Region "Propiedades"

		Property IConexiones As IConexiones

#End Region

#Region "Metodos"

		Sub EjecutaComando(ByVal itipolineabase_ As Integer, Optional ByVal tabla_ As String = vbNullString, Optional ByVal tupla_() As String = Nothing, Optional ByVal condiciones_() As String = Nothing)

#End Region

	End Interface

End Namespace