Imports Syn.Documento

Public Interface IRepresentacionManifestacionValor

    Function EncabezadoMV(ByVal documento_ As DocumentoElectronico) As String
    Function InformacionGeneralMV(ByVal documento_ As DocumentoElectronico) As String
    Function ValorTransaccionMV(ByVal documento_ As DocumentoElectronico) As String
    Function EncabezadoHC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion1HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion2HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion3HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion4HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion5_6_7HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion8HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion9HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion10HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion11_12HC(ByVal documento_ As DocumentoElectronico) As String
    Function Seccion13HC(ByVal documento_ As DocumentoElectronico) As String

End Interface
