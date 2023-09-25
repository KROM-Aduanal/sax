Imports Wma.Exceptions
Imports Gsol.BaseDatos.Operaciones

Namespace gsol.documento

    Public Interface IGeneraRepresentacionImpresa

#Region "Enum"

        Enum TiposPlantillas
            SinDefinir = 0
            FacturaCFDI33 = 1
            NotaCredito = 2
            ComplementoPago = 3
        End Enum

        Enum TiposEmisorDocumento
            Propietario = 0
            Terceros = 1
        End Enum

        Enum TiposSalida
            ExportarPDF = 1
            ExportarJPG
            ExportarHTML
            ExportarXLS
            VistaPreviaImpresion
        End Enum

#End Region

#Region "Propiedades"

        Property OperacionesCatalogo As IOperacionesCatalogo

        Property EspacioTrabajo As IEspacioTrabajo

        ReadOnly Property Estatus As TagWatcher

        ReadOnly Property CveDocumento As Int32

        Property RutaSalida As String

        Property ProcesarConHilo As Boolean

#End Region

#Region "Metodos"

        ' Procesa documento de acuerdo a una clave de documento y considerando su ruta
        Function ProcesarDocumento(ByVal i_Cve_Documento_ As Int32,
                                   ByVal t_RutaArchivo_ As String,
                                   ByVal i_Cve_DivisionMiEmpresa As Integer,
                                   ByVal i_TipoProcesable_ As IDocumento.TiposProcesable,
                                   ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                   Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = TiposEmisorDocumento.Propietario,
                                   Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = TiposSalida.ExportarPDF
                                   ) As TagWatcher

        ' Procesa documento de acuerdo a una clave de documento
        Function ProcesarDocumento(ByVal i_Cve_Documento_ As Int32,
                                  ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                  Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = TiposEmisorDocumento.Propietario,
                                  Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = TiposSalida.ExportarPDF
                                 ) As TagWatcher

        ' Procesa documento de acuerdo a una ruta 
        Function ProcesarDocumento(ByVal t_RutaArchivo_ As String,
                                  ByVal i_TipoProcesable_ As IDocumento.TiposProcesable,
                                  ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                  Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = TiposEmisorDocumento.Propietario,
                                  Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = TiposSalida.ExportarPDF
                                 ) As TagWatcher

        ' Procesa documento de acuerdo a un objeto
        Function ProcesarDocumento(ByVal o_Fuente_ As Object,
                                  ByVal i_TipoProcesable_ As IDocumento.TiposProcesable,
                                  ByVal i_TipoPlantilla_ As IGeneraRepresentacionImpresa.TiposPlantillas,
                                  Optional ByVal i_TipoEmisorDocumento_ As IGeneraRepresentacionImpresa.TiposEmisorDocumento = TiposEmisorDocumento.Propietario,
                                  Optional ByVal i_TipoSalida_ As IGeneraRepresentacionImpresa.TiposSalida = TiposSalida.ExportarPDF
                                 ) As TagWatcher

#End Region

    End Interface

End Namespace