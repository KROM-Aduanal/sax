Imports gsol.krom
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento

Namespace Syn.Documento

    <Serializable()>
    Public Class ConstructorPedimentoNormalPDF
        Inherits EntidadDatosDocumentoDigital
        Implements ICloneable

#Region "Atributes"

        Public _ControladorPedimentoPDF As New ConstructorPedimentoPDF

#End Region

#Region "Propiedades"

        Public Property _documentoElectronico As DocumentoElectronico

#End Region

#Region "Builders"

        Sub New(ByVal tipoDocumento_ As TiposDocumentoDigital,
                ByVal construir_ As Boolean,
                 ByVal documentoElectronico_ As DocumentoElectronico)

            Inicializa(documentoElectronico_, tipoDocumento_, construir_)
            _documentoElectronico = documentoElectronico_

        End Sub

        Public Sub New(ByVal tipoDocumento_ As TiposDocumentoDigital,
                       ByVal folioDocumento_ As String,
                       ByVal folioOperacion_ As String,
                       ByVal idCliente_ As Int32,
                       ByVal nombreCliente_ As String)

            Inicializa(folioDocumento_, folioOperacion_, idCliente_, nombreCliente_, tipoDocumento_)

        End Sub

#End Region

#Region "Methods"

        Public Overrides Sub ConstruyeEncabezado()

        End Sub

        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()

        End Sub

        Public Overrides Sub ConstruyeCuerpo()

        End Sub

        Public Overrides Sub ConstruyePiePagina()

        End Sub

        Public Sub CrearImpresionPedimento(ByVal documento_ As DocumentoElectronico)

            'Desde está clase se envian las Tablas conteniendo las celdas que debe pintar en cada una de estas.
            'Se debe mandar una tabla por cada línea a pintar y decirle de que tamaño deben ser cada una de las celdas.
            'Una vez realizado esto entonces se envía al controlador las tablas a dibujar.

            Dim listaTablas_ As New List(Of Tabla)

            Dim listaEncabezado_ As New List(Of Tabla)

            Dim listaEncabezadoSecundario_ As New List(Of Tabla)

            Dim listaPiePagina_ As New List(Of Tabla)

            Dim numOperacion_ As String


#Region "Encabezado"

            With listaEncabezado_

                Dim folioOperacion_ = New Tabla() With {
                    .AnchoColumna = {16.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = documento_.FolioOperacion,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(folioOperacion_)

                Dim tituloPedimento_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "PEDIMENTO",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 },
                                 New Celda With {
                                    .Contenido = "Página 1 de 2",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                }

                .Add(tituloPedimento_)

                Dim numPedimento_ = New Tabla() With {
                    .AnchoColumna = {5.0F, 5.0F, 3.0F, 2.0F, 5.0F, 1.0F, 3.0F, 2.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NUM. PEDIMENTO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                 New Celda With {
                                    .Contenido = documento_.FolioDocumento,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                 New Celda With {
                                    .Contenido = "T. OPER:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_CVE_TIPO_OPERACION).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                 New Celda With {
                                    .Contenido = "CVE. PEDIMENTO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                 New Celda With {
                                    .Contenido = documento_.Attribute(CA_CVE_PEDIMENTO).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "REGIMEN:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_REGIMEN).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "CERTIFICACIONES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(numPedimento_)

                Dim destino_ = New Tabla() With {
                    .AnchoColumna = {3.0F, 2.0F, 4.0F, 3.0F, 4.0F, 3.0F, 4.0F, 3.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "DESTINO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_DESTINO_ORIGEN).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "TIPO CAMBIO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_TIPO_CAMBIO).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "PESO BRUTO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_PESO_BRUTO).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "ADUANA E/S:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_ADUANA_E_S).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(destino_)

                Dim mediosTransporte_ = New Tabla() With {
                    .AnchoColumna = {11.0F, 9.0F, 6.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "MEDIOS DE TRANSPORTE",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "VALOR DOLARES:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_VALOR_DOLARES).Valor IsNot Nothing, documento_.Attribute(CA_VALOR_DOLARES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(mediosTransporte_)

                Dim entradaSalida_ = New Tabla() With {
                    .AnchoColumna = {5.0F, 3.0F, 3.0F, 9.0F, 6.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                            New Fila With {
                                .ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = "ENTRADA/SALIDA:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = "ARRIBO:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = "SALIDA:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = "VALOR ADUANA:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = documento_.Attribute(CA_VALOR_DOLARES).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlDerecha,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerIzq,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    }
                                }
                            }
                    }
                }

                .Add(entradaSalida_)

                Dim ValorEntradaSalida_ = New Tabla() With {
                    .AnchoColumna = {5.0F, 3.0F, 3.0F, 11.0F, 4.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_MEDIO_DE_TRANSPORTE) IsNot Nothing, documento_.Attribute(CA_MEDIO_DE_TRANSPORTE).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_MEDIO_DE_TRANSPORTE_DE_ARRIBO) IsNot Nothing, documento_.Attribute(CA_MEDIO_DE_TRANSPORTE_DE_ARRIBO).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_MEDIO_DE_TRANSPORTE_DE_SALIDA) IsNot Nothing, documento_.Attribute(CA_MEDIO_DE_TRANSPORTE_DE_SALIDA).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "PRECIO PAGADO/VALOR COMERCIAL:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_PRECIO_PAGADO_O_VALOR_COMERCIAL).Valor IsNot Nothing, documento_.Attribute(CA_PRECIO_PAGADO_O_VALOR_COMERCIAL).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(ValorEntradaSalida_)

                Dim tituloDatosImportador_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "DATOS DEL IMPORTADOR/EXPORTADOR",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(tituloDatosImportador_)

                Dim rfcImportador_ = New Tabla() With {
                    .AnchoColumna = {3.0F, 6.0F, 17.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "RFC:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_RFC_DEL_IOE).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "NOMBRE, DENOMINACION O RAZON SOCIAL:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(rfcImportador_)

                Dim curpImportador_ = New Tabla() With {
                    .AnchoColumna = {3.0F, 6.0F, 17.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "CURP:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_CURP_DEL_IOE).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_RAZON_SOCIAL_IOE).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(curpImportador_)

                Dim domicilioImportador_ = New Tabla() With {
                    .AnchoColumna = {3.0F, 23.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "DOMICILIO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_DOMICILIO_IOE).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(domicilioImportador_)

                Dim valorSeguros_ = New Tabla() With {
                    .AnchoColumna = {5.0F, 4.0F, 5.0F, 5.0F, 7.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "VAL. SEGUROS",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "SEGUROS",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "FLETES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "EMBALAJES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "OTROS INCREMENTABLES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(valorSeguros_)

                Dim datosValorSeguros_ = New Tabla() With {
                    .AnchoColumna = {5.0F, 4.0F, 5.0F, 5.0F, 7.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_VAL_SEGUROS).Valor IsNot Nothing, documento_.Attribute(CA_VAL_SEGUROS).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_SEGUROS) IsNot Nothing, documento_.Attribute(CA_SEGUROS).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_FLETES) IsNot Nothing, documento_.Attribute(CA_FLETES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_EMBALAJES) IsNot Nothing, documento_.Attribute(CA_EMBALAJES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_OTROS_INCREMENTABLES) IsNot Nothing, documento_.Attribute(CA_OTROS_INCREMENTABLES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(datosValorSeguros_)

                Dim tituloValorDecrementables_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "VALOR DECREMENTABLES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(tituloValorDecrementables_)

                Dim transporteDecrementables_ = New Tabla() With {
                    .AnchoColumna = {6.0F, 6.0F, 4.0F, 4.0F, 6.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "TRANSPORTE DECREMENTABLES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "SEGUROS DECREMENTABLES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "CARGA",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "DESCARGA",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "OTROS DECREMENTABLES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(transporteDecrementables_)

                Dim datostransporteDecrementables_ = New Tabla() With {
                    .AnchoColumna = {6.0F, 6.0F, 4.0F, 4.0F, 6.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_TRANSPORTE_DECREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_TRANSPORTE_DECREMENTABLES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_SEGURO_DECREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_SEGURO_DECREMENTABLES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_CARGA_DECREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_CARGA_DECREMENTABLES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_DESCARGA_DECREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_DESCARGA_DECREMENTABLES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_OTROS_DECREMENTABLES).Valor IsNot Nothing, documento_.Attribute(CA_OTROS_DECREMENTABLES).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(datostransporteDecrementables_)

                Dim codigoAceptacion_ = New Tabla() With {
                    .AnchoColumna = {5.0F, 16.0F, 5.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "CÓDIGO DE ACEPTACIÓN:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "CLAVE DE LA SECCION ADUANERA DE DESPACHO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(codigoAceptacion_)

                Dim valorCodigoAceptacion_ = New Tabla() With {
                    .AnchoColumna = {5.0F, 16.0F, 5.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_ACUSE_ELECTONICO_DE_VALIDACION).Valor IsNot Nothing, documento_.Attribute(CA_ACUSE_ELECTONICO_DE_VALIDACION).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_CLAVE_SAD).Valor IsNot Nothing, documento_.Attribute(CA_CLAVE_SAD).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(valorCodigoAceptacion_)

                Dim marcaNumTotalBultos_ = New Tabla() With {
                    .AnchoColumna = {33.0F, 37.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "MARCAS, NUMEROS Y TOTAL DE BULTOS:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_MARCAS_NUMEROS_TOTAL_BULTOS).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(marcaNumTotalBultos_)

                Dim fechaTituloTasas_ = New Tabla() With {
                    .AnchoColumna = {8.0F, 18.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "FECHAS",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "TASAS A NIVEL PEDIMENTO",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(fechaTituloTasas_)

                Dim fechaEntrada_ = New Tabla() With {
                    .AnchoColumna = {4.0F, 4.0F, 6.0F, 6.0F, 6.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "ENTRADA",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_FECHA_ENTRADA).Valor IsNot Nothing, CType(documento_.Attribute(CA_FECHA_ENTRADA).Valor, Date), 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "CONTRIB.",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "CVE. T. TASA",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "TASA",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(fechaEntrada_)

                Dim fechaPago_ = New Tabla() With {
                    .AnchoColumna = {4.0F, 4.0F, 6.0F, 6.0F, 6.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "PAGO",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_FECHA_PAGO) IsNot Nothing, CType(documento_.Attribute(CA_FECHA_PAGO).Valor, Date), 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_CONTRIBUCION) IsNot Nothing, documento_.Attribute(CA_CONTRIBUCION).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_CVE_T_TASA) IsNot Nothing, documento_.Attribute(CA_CVE_T_TASA).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_TASA) IsNot Nothing, documento_.Attribute(CA_TASA).Valor, 0),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(fechaPago_)

                Dim fechaOriginal_ = New Tabla() With {
                    .AnchoColumna = {4.0F, 4.0F, 6.0F, 6.0F, 6.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "ORIGINAL",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_FECHA_ORIGINAL).Valor IsNot Nothing, CType(documento_.Attribute(CA_FECHA_ORIGINAL).Valor, Date), " "),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(fechaOriginal_)

                Dim tituloLiquidaciones_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "CUADRO DE LIQUIDACION",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(tituloLiquidaciones_)

                Dim titulosConcepto_ = New Tabla() With {
                    .AnchoColumna = {3.0F, 2.0F, 3.0F, 3.0F, 2.0F, 3.0F, 10.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "CONCEPTO",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "F.P.",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "IMPORTE",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "CONCEPTO",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "F.P.",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "IMPORTE",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "TOTALES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(titulosConcepto_)

#Region "Cuadro de liquidación"

                Dim claveConcepto1_ As New Tabla()

                claveConcepto1_.AnchoColumna = {3.0F, 2.0F, 3.0F, 3.0F, 2.0F, 3.0F, 3.0F, 7.0F, 8.0F} '34

                With documento_.Seccion(SeccionesPedimento.ANS7)

                    With .Seccion(SeccionesPedimento.ANS55)

                        Dim numeroLinea_ As Integer = 0

                        Dim contadorPar_ As Integer = 0

                        For Each liaquidaciones_ As Nodo In .Nodos

                            If liaquidaciones_.TipoNodo = Nodo.TiposNodo.Partida Then

                                If numeroLinea_ = 0 Then

                                    claveConcepto1_.Filas.Add(New Fila)

                                End If

                                If contadorPar_ Mod 2 And contadorPar_ > 0 Then

                                    numeroLinea_ += 1
                                    claveConcepto1_.Filas.Add(New Fila)

                                End If

                                Dim partida_ = CType(liaquidaciones_, PartidaGenerica)

                                With claveConcepto1_

                                    Dim concepto_ = ""

                                    Dim formaPago_ = ""

                                    Dim importe_ = ""

                                    If partida_.Attribute(CA_CONCEPTO).Valor IsNot Nothing Then

                                        concepto_ = partida_.Attribute(CA_CONCEPTO).Valor

                                    End If

                                    If partida_.Attribute(CA_FP).Valor IsNot Nothing Then

                                        formaPago_ = partida_.Attribute(CA_FP).Valor

                                    End If

                                    If partida_.Attribute(CA_IMPORTE).Valor IsNot Nothing Then

                                        importe_ = partida_.Attribute(CA_IMPORTE).Valor

                                    End If

                                    .Filas(numeroLinea_).ListCeldas.Add(
                                    New Celda With {
                                                    .Contenido = concepto_,
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                    })
                                    .Filas(numeroLinea_).ListCeldas.Add(
                                    New Celda With {
                                                    .Contenido = formaPago_,
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                    })
                                    .Filas(numeroLinea_).ListCeldas.Add(
                                                    New Celda With {
                                                    .Contenido = importe_,
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                    })

                                    contadorPar_ += 1

#Region "Comentado totales liquidación"

                                    '    Dim celda7_ As New Celda

                                    '    With celda7_

                                    '        .Contenido = "EFECTIVO"

                                    '        Dim estilos_ As New Estilos

                                    '        With estilos_

                                    '            .AlineacionTexto = Alineaciones.AlCentro

                                    '            .Borde = Bordes.BordeIzqInf

                                    '            .DimensionFuente = Fuentes.Medio

                                    '            .Sombreado = False

                                    '        End With

                                    '        .EstiloCelda = estilos_

                                    '    End With

                                    '    .Add(celda7_)

                                    '    Dim celda8_ As New Celda

                                    '    With celda8_

                                    '        If documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing Then

                                    '            .Contenido = documento_.Attribute(CA_EFECTIVO).Valor.ToString

                                    '        Else

                                    '            .Contenido = " "

                                    '        End If

                                    '        Dim estilos_ As New Estilos

                                    '        With estilos_

                                    '            .AlineacionTexto = Alineaciones.AlDerecha

                                    '            .Borde = Bordes.BordeIzqInf

                                    '            .DimensionFuente = Fuentes.Medio

                                    '            .Sombreado = False

                                    '        End With

                                    '        .EstiloCelda = estilos_

                                    '    End With

                                    '    .Add(celda8_)

                                    '    Dim celda9_ As New Celda

                                    '    With celda9_

                                    '        .Contenido = " "

                                    '        Dim estilos_ As New Estilos

                                    '        With estilos_

                                    '            .AlineacionTexto = Alineaciones.AlCentro

                                    '            .Borde = Bordes.BordeDerIzq

                                    '            .DimensionFuente = Fuentes.Medio

                                    '            .Sombreado = False

                                    '        End With

                                    '        .EstiloCelda = estilos_

                                    '    End With

                                    '    .Add(celda9_)

                                    'End With

                                    'End With
#End Region
                                End With

                            End If

                        Next

                    End With

                End With

                .Add(claveConcepto1_)

#End Region

#Region "no necesario"

                'Dim claveConcepto_ As New Tabla()

                'With claveConcepto_

                '    .AnchoColumna = {3.0F, 2.0F, 3.0F, 3.0F, 2.0F, 3.0F, 3.0F, 7.0F, 8.0F} '34

                '    .Filas = New List(Of Fila) From {
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "NUM. PEDIMENTO:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeIzqInf,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.FolioDocumento,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "T. OPER:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_CVE_TIPO_OPERACION).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "CVE. PEDIMENTO:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_CVE_PEDIMENTO).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "REGIMEN:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_REGIMEN).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "CERTIFICACIONES",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlCentro,
                '                        .Borde = Bordes.Bordes,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        }
                '    }

                '    'With .Filas

                '    '        Dim listaCeldas_ As New List(Of Celda)
                '    '        .ListCeldas = listaCeldas_

                '    '        With .ListCeldas

                '    '            Dim celda_ As New Celda

                '    '            With celda_

                '    '                .Contenido = documento_.Attribute(CA_CONCEPTO).Valor

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda_)

                '    '            Dim celda2_ As New Celda

                '    '            With celda2_

                '    '                If documento_.Attribute(CA_FP) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_FP).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda2_)

                '    '            Dim celda3_ As New Celda

                '    '            With celda3_

                '    '                If documento_.Attribute(CA_IMPORTE) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_IMPORTE).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda3_)

                '    '            Dim celda4_ As New Celda

                '    '            With celda4_

                '    '                .Contenido = documento_.Attribute(CA_CONCEPTO).Valor

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda4_)

                '    '            Dim celda5_ As New Celda

                '    '            With celda5_

                '    '                If documento_.Attribute(CA_FP) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_FP).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda5_)

                '    '            Dim celda6_ As New Celda

                '    '            With celda6_

                '    '                If documento_.Attribute(CA_IMPORTE) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_IMPORTE).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda6_)

                '    '            Dim celda7_ As New Celda

                '    '            With celda7_

                '    '                .Contenido = "EFECTIVO"

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzqInf

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda7_)

                '    '            Dim celda8_ As New Celda

                '    '            With celda8_

                '    '                If documento_.Attribute(CA_EFECTIVO).Valor IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_EFECTIVO).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzqInf

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda8_)

                '    '            Dim celda9_ As New Celda

                '    '            With celda9_

                '    '                .Contenido = " "

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeDerIzq

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda9_)

                '    '        End With

                '    '    End With

                'End With

                '.Add(claveConcepto_)

                'Dim claveConcepto2_ As New Tabla()

                'With claveConcepto2_

                '    .AnchoColumna = {3.0F, 2.0F, 3.0F, 3.0F, 2.0F, 3.0F, 3.0F, 7.0F, 8.0F} '34

                '    .Filas = New List(Of Fila) From {
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "NUM. PEDIMENTO:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeIzqInf,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.FolioDocumento,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "T. OPER:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_CVE_TIPO_OPERACION).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "CVE. PEDIMENTO:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_CVE_PEDIMENTO).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "REGIMEN:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_REGIMEN).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "CERTIFICACIONES",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlCentro,
                '                        .Borde = Bordes.Bordes,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        }
                '    }

                '    'With .Filas

                '    '        Dim listaCeldas_ As New List(Of Celda)
                '    '        .ListCeldas = listaCeldas_

                '    '        With .ListCeldas

                '    '            Dim celda_ As New Celda

                '    '            With celda_

                '    '                .Contenido = documento_.Attribute(CA_CONCEPTO).Valor

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda_)

                '    '            Dim celda2_ As New Celda

                '    '            With celda2_

                '    '                If documento_.Attribute(CA_FP) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_FP).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda2_)

                '    '            Dim celda3_ As New Celda

                '    '            With celda3_

                '    '                If documento_.Attribute(CA_IMPORTE) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_IMPORTE).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda3_)

                '    '            Dim celda4_ As New Celda

                '    '            With celda4_

                '    '                .Contenido = documento_.Attribute(CA_CONCEPTO).Valor

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda4_)

                '    '            Dim celda5_ As New Celda

                '    '            With celda5_

                '    '                If documento_.Attribute(CA_FP) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_FP).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda5_)

                '    '            Dim celda6_ As New Celda

                '    '            With celda6_

                '    '                If documento_.Attribute(CA_IMPORTE) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_IMPORTE).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda6_)

                '    '            Dim celda7_ As New Celda

                '    '            With celda7_

                '    '                .Contenido = "OTROS"

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlIzquierdo

                '    '                    .Borde = Bordes.BordeIzqInf

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda7_)

                '    '            Dim celda8_ As New Celda

                '    '            With celda8_

                '    '                If documento_.Attribute(CA_OTROS).Valor IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_OTROS).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzqInf

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda8_)

                '    '            Dim celda9_ As New Celda

                '    '            With celda9_

                '    '                .Contenido = " "

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeDerIzq

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda9_)

                '    '        End With

                '    '    End With

                'End With

                '.Add(claveConcepto2_)

                'Dim claveConcepto3_ As New Tabla()

                'With claveConcepto3_

                '    .AnchoColumna = {3.0F, 2.0F, 3.0F, 3.0F, 2.0F, 3.0F, 3.0F, 7.0F, 8.0F} '34

                '    .Filas = New List(Of Fila) From {
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "NUM. PEDIMENTO:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeIzqInf,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.FolioDocumento,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "T. OPER:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_CVE_TIPO_OPERACION).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "CVE. PEDIMENTO:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_CVE_PEDIMENTO).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "REGIMEN:",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = documento_.Attribute(CA_REGIMEN).Valor,
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                '                        .Borde = Bordes.BordeInferior,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        },
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = "CERTIFICACIONES",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlCentro,
                '                        .Borde = Bordes.Bordes,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        }
                '    }

                '    'With .Filas

                '    '        Dim listaCeldas_ As New List(Of Celda)
                '    '        .ListCeldas = listaCeldas_

                '    '        With .ListCeldas

                '    '            Dim celda_ As New Celda

                '    '            With celda_

                '    '                .Contenido = documento_.Attribute(CA_CONCEPTO).Valor

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda_)

                '    '            Dim celda2_ As New Celda

                '    '            With celda2_

                '    '                If documento_.Attribute(CA_FP) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_FP).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda2_)

                '    '            Dim celda3_ As New Celda

                '    '            With celda3_

                '    '                If documento_.Attribute(CA_IMPORTE) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_IMPORTE).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda3_)

                '    '            Dim celda4_ As New Celda

                '    '            With celda4_

                '    '                .Contenido = documento_.Attribute(CA_CONCEPTO).Valor

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda4_)

                '    '            Dim celda5_ As New Celda

                '    '            With celda5_

                '    '                If documento_.Attribute(CA_FP) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_FP).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda5_)

                '    '            Dim celda6_ As New Celda

                '    '            With celda6_

                '    '                If documento_.Attribute(CA_IMPORTE) IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_IMPORTE).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda6_)

                '    '            Dim celda7_ As New Celda

                '    '            With celda7_

                '    '                .Contenido = "TOTAL"

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlIzquierdo

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda7_)

                '    '            Dim celda8_ As New Celda

                '    '            With celda8_

                '    '                If documento_.Attribute(CA_TOTAL).Valor IsNot Nothing Then

                '    '                    .Contenido = documento_.Attribute(CA_TOTAL).Valor.ToString

                '    '                Else

                '    '                    .Contenido = " "

                '    '                End If

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlDerecha

                '    '                    .Borde = Bordes.BordeIzquierdo

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda8_)

                '    '            Dim celda9_ As New Celda

                '    '            With celda9_

                '    '                .Contenido = " "

                '    '                Dim estilos_ As New Estilos

                '    '                With estilos_

                '    '                    .AlineacionTexto = Alineaciones.AlCentro

                '    '                    .Borde = Bordes.BordeDerIzqInf

                '    '                    .DimensionFuente = Fuentes.Medio

                '    '                    .Sombreado = False

                '    '                End With

                '    '                .EstiloCelda = estilos_

                '    '            End With

                '    '            .Add(celda9_)

                '    '        End With

                '    '    End With

                'End With

                '.Add(claveConcepto3_)

#End Region

            End With

#End Region

#Region "Encabezado pagina secundaria"

            listaEncabezadoSecundario_.Add(
                New Tabla() With {
                    .AnchoColumna = {16.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = documento_.FolioOperacion,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                })

            listaEncabezadoSecundario_.Add(
                New Tabla() With {
                    .AnchoColumna = {5.0F, 11.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "ANEXO DEL PEDIMENTO",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "Página 2 de 2",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                })

            listaEncabezadoSecundario_.Add(
                New Tabla() With {
                    .AnchoColumna = {5.0F, 5.0F, 3.0F, 2.0F, 5.0F, 1.0F, 3.0F, 10.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NUM. PEDIMENTO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                 New Celda With {
                                    .Contenido = documento_.FolioDocumento,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                 New Celda With {
                                    .Contenido = "T. OPER:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_CVE_TIPO_OPERACION).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                 New Celda With {
                                    .Contenido = "CVE. PEDIMENTO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                 New Celda With {
                                    .Contenido = documento_.Attribute(CA_CVE_PEDIMENTO).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "RFC:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_RFC_DEL_IOE).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                })

            listaEncabezadoSecundario_.Add(
                New Tabla() With {
                    .AnchoColumna = {21.0F, 3.0F, 10.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "CURP:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_CURP_DEL_IOE).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

#End Region

#Region "Cuerpo"

#Region "Depósito referenciado"

            With listaTablas_

                Dim tituloDepositoReferenciado_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "DEPÓSITO REFERENCIADO - LÍNEA DE CAPTURA - INFORMACIÓN DEL PAGO",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(tituloDepositoReferenciado_)

                Dim espacioEnBlanco_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco_)

                Dim espacioEnBlanco2_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco2_)

                Dim espacioEnBlanco3_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco3_)

                Dim espacioEnBlanco4_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco4_)

                Dim espacioEnBlanco5_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco5_)

                Dim espacioEnBlanco6_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco6_)

                Dim espacioEnBlanco7_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco7_)

                Dim espacioEnBlanco8_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco8_)

                Dim espacioEnBlanco9_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(espacioEnBlanco9_)

#Region "comentadito"

                'Dim espacioEnBlanco10_ = New Tabla() With {
                '    .AnchoColumna = {70.0F, 21.5F},
                '    .Filas = New List(Of Fila) From {
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = " ",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlCentro,
                '                        .Borde = Bordes.BordeIzquierdo,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 },
                '                New Celda With {
                '                    .Contenido = " ",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlCentro,
                '                        .Borde = Bordes.BordeDerIzq,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        }
                '    }
                '}

                '.Add(espacioEnBlanco10_)

                'Dim espacioEnBlanco11_ = New Tabla() With {
                '    .AnchoColumna = {70.0F, 21.5F},
                '    .Filas = New List(Of Fila) From {
                '        New Fila With {
                '            .ListCeldas = New List(Of Celda) From {
                '                New Celda With {
                '                    .Contenido = " ",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlCentro,
                '                        .Borde = Bordes.BordeIzquierdo,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 },
                '                New Celda With {
                '                    .Contenido = " ",
                '                    .EstiloCelda = New Estilos With {
                '                        .AlineacionTexto = Alineaciones.AlCentro,
                '                        .Borde = Bordes.BordeDerIzq,
                '                        .DimensionFuente = Fuentes.Medio,
                '                        .Sombreado = False
                '                    }
                '                 }
                '            }
                '        }
                '    }
                '}

                '.Add(espacioEnBlanco11_)

#End Region

                Dim tituloPagoElectronico_ = New Tabla() With {
                    .AnchoColumna = {70.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "*** PAGO ELECTRÓNICO ***",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(tituloPagoElectronico_)

                Dim pedimentoOriginal_ = ""

                If documento_.Attribute(CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS) IsNot Nothing Then

                    pedimentoOriginal_ = documento_.Attribute(CA_NUM_PEDIMENTO_ORIGINAL_7_DIGITOS).Valor

                End If

                Dim patente_ = New Tabla() With {
                    .AnchoColumna = {4.0F, 5.0F, 5.0F, 4.0F, 4.0F, 4.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                            New Fila With {
                                .ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = "PATENTE:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = documento_.Attribute(CA_PATENTE).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = "PEDIMENTO:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = pedimentoOriginal_,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = "ADUANA:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = documento_.Attribute(CA_ADUANA_DESPACHO).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerIzq,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     }
                                }
                            }
                    }
                }

                .Add(patente_)

                Dim banco_ = New Tabla() With {
                    .AnchoColumna = {6.0F, 64.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "BANCO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_NOMBRE_INST_BANCARIA).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(banco_)

                Dim lineaCaptura_ = New Tabla() With {
                    .AnchoColumna = {14.0F, 56.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "LÍNEA DE CAPTURA:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_LINEA_CAPTURA).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(lineaCaptura_)

                Dim importePagado_ = New Tabla() With {
                    .AnchoColumna = {6.0F, 5.0F, 5.0F, 10.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "IMPORTE PAGADO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_PAGO_ELECTRONICO).Valor IsNot Nothing, documento_.Attribute(CA_PAGO_ELECTRONICO).Valor, " "),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "FECHA DE PAGO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_FECHA_PAGO).Valor IsNot Nothing, CType(documento_.Attribute(CA_FECHA_PAGO).Valor, Date), " "),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(importePagado_)

                Dim numOperacionBancaria_ = New Tabla() With {
                    .AnchoColumna = {25.0F, 45.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NÚMERO DE OPERACIÓN BANCARIA:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_NUM_OPERACION_BANCARIA).Valor IsNot Nothing, documento_.Attribute(CA_NUM_OPERACION_BANCARIA).Valor, " "),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(numOperacionBancaria_)

                Dim numTransaccionSAT_ = New Tabla() With {
                    .AnchoColumna = {22.0F, 48.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                            New Fila With {
                                .ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = "NÚMERO DE TRANSACCIÓN SAT:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = IIf(documento_.Attribute(CA_NUM_TRANSACCION_SAT).Valor IsNot Nothing, documento_.Attribute(CA_NUM_TRANSACCION_SAT).Valor, " "),
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.SinBordes,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerIzq,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     }
                                }
                            }
                    }
                }

                .Add(numTransaccionSAT_)

                Dim medioPresentacion_ = New Tabla() With {
                    .AnchoColumna = {19.0F, 51.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "MEDIO DE PRESENTACIÓN:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_MEDIO_PRESENTACION).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_PRESENTACION).Valor, " "),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(medioPresentacion_)

                Dim medioRecepcionCobro_ = New Tabla() With {
                    .AnchoColumna = {22.0F, 48.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                            New Fila With {
                                .ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = "MEDIO  DE RECEPCIÓN/COBRO:",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeIzqInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = IIf(documento_.Attribute(CA_MEDIO_RECEPCION_COBRO).Valor IsNot Nothing, documento_.Attribute(CA_MEDIO_RECEPCION_COBRO).Valor, " "),
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeInferior,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerIzqInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     }
                                }
                            }
                    }
                }

                .Add(medioRecepcionCobro_)

                Dim agenteAduanal_ = New Tabla() With {
                    .AnchoColumna = {59.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                            New Fila With {
                                .ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = "AGENTE ADUANAL,AGENCIA ADUANAL,APODERADO ADUANAL O DE ALMACEN NOMBRE O RAZ. SOC.:" + documento_.Attribute(CA_RFC_AA).Valor.ToString,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = "DECLARO BAJO PROTESTA DE DECIR VERDAD, EN LOS TERMINOS DE LO DISPUESTO POR EL ARTICULO 81 DE LA LEY ADUANERA: PATENTE O AUTORIZACION: " + documento_.Attribute(CA_PATENTE).Valor.ToString,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeDerIzq,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     }
                                }
                            }
                    }
                }

                .Add(agenteAduanal_)

                Dim rfcAA_ = New Tabla() With {
                    .AnchoColumna = {30.0F, 29.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "RFC: " + documento_.Attribute(CA_RFC_AA).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = "CURP: " + documento_.Attribute(CA_CURP_AA_O_REP_LEGAL).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                }

                .Add(rfcAA_)

                Dim tituloMandatario_ = New Tabla() With {
                    .AnchoColumna = {59.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "MANDATARIO/PERSONA AUTORIZADA",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(tituloMandatario_)

                Dim nombreMandatario_ = New Tabla() With {
                    .AnchoColumna = {59.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NOMBRE: " + documento_.Attribute(CA_NOMBRE_MAND_REP_AA).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(nombreMandatario_)

                Dim rfcMandatario_ = New Tabla() With {
                    .AnchoColumna = {30.0F, 29.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "RFC: " + documento_.Attribute(CA_RFC_MAND_O_AGAD_REP_ALMACEN).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "CURP: " + documento_.Attribute(CA_CURP_MAND_O_AGAD_REP_ALMACEN).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                }

                .Add(rfcMandatario_)

                Dim nomSerieCertificado_ = New Tabla() With {
                    .AnchoColumna = {30.0F, 29.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NÚMERO DE SERIE DEL CERTIFICADO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_CERTIFICADO_FIRMA).Valor IsNot Nothing, documento_.Attribute(CA_CERTIFICADO_FIRMA).Valor, " "),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                }

                .Add(nomSerieCertificado_)

                Dim eFirma_ = New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "e.firma: ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(eFirma_)

                Dim datosEFirma_ = New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_EFIRMA).Valor IsNot Nothing, documento_.Attribute(CA_EFIRMA).Valor, " "),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                }

                .Add(datosEFirma_)

                Dim eFirmaEspacioBlanco_ = New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(eFirmaEspacioBlanco_)

                Dim cuadroFinal_ = New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(cuadroFinal_)

                Dim cuadroFinal2_ = New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                }

                .Add(cuadroFinal2_)

            End With

#End Region

#Region "Datos del proveedor o comprador"

            listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {16.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "DATOS DEL PROVEEDOR O COMPRADOR",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                }
                            }
                        }
                    }
                })

            listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {6.0F, 10.0F, 8.0F, 6.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "ID. FISCAL",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "NOMBRE, DENOMINACION O RAZON SOCIAL",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "DOMICILIO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "VINCULACION",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            Dim datosProveedor_ As New Tabla()

            datosProveedor_.AnchoColumna = {6.0F, 10.0F, 8.0F, 6.0F} '34

            With documento_.Seccion(SeccionesPedimento.ANS10)

                Dim numeroLinea_ As Integer = 0

                For Each p_ As Nodo In .Nodos

                    If p_.TipoNodo = Nodo.TiposNodo.Partida Then

                        datosProveedor_.Filas.Add(New Fila)

                        Dim proveedor_ = CType(p_, PartidaGenerica)

                        With datosProveedor_

                            .Filas(numeroLinea_).ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = proveedor_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = proveedor_.Attribute(CA_NOMBRE_DEN_RAZON_SOC_POC).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = proveedor_.Attribute(CA_DOMICILIO_POC).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = proveedor_.Attribute(CA_VINCULACION).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }

                            numeroLinea_ += 1

                        End With

                        listaTablas_.Add(datosProveedor_)

                    End If

                Next

            End With


            Dim datosFacturaProveedor_ As New Tabla()

            datosFacturaProveedor_.AnchoColumna = {8.0F, 4.0F, 4.0F, 6.0F, 6.0F, 6.0F, 6.0F} '34

            If documento_.Seccion(SeccionesPedimento.ANS13).Nodos IsNot Nothing Then

                datosFacturaProveedor_.Filas = New List(Of Fila) From {
                New Fila With {
                    .ListCeldas = New List(Of Celda) From {
                        New Celda With {
                            .Contenido = "NUM. FACTURA",
                            .EstiloCelda = New Estilos With {
                                .AlineacionTexto = Alineaciones.AlCentro,
                                .Borde = Bordes.BordeIzqInf,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = False
                            }
                        },
                        New Celda With {
                            .Contenido = "FECHA",
                            .EstiloCelda = New Estilos With {
                                .AlineacionTexto = Alineaciones.AlCentro,
                                .Borde = Bordes.BordeIzqInf,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = False
                            }
                         },
                        New Celda With {
                            .Contenido = "INCOTERM",
                            .EstiloCelda = New Estilos With {
                                .AlineacionTexto = Alineaciones.AlCentro,
                                .Borde = Bordes.BordeIzqInf,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = False
                            }
                         },
                        New Celda With {
                            .Contenido = "MONEDA FACT",
                            .EstiloCelda = New Estilos With {
                                .AlineacionTexto = Alineaciones.AlCentro,
                                .Borde = Bordes.BordeIzqInf,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = False
                            }
                        },
                        New Celda With {
                            .Contenido = "VAL. MON. FACT",
                            .EstiloCelda = New Estilos With {
                                .AlineacionTexto = Alineaciones.AlCentro,
                                .Borde = Bordes.BordeIzqInf,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = False
                            }
                         },
                        New Celda With {
                            .Contenido = "FACTOR MON. FACT",
                            .EstiloCelda = New Estilos With {
                                .AlineacionTexto = Alineaciones.AlCentro,
                                .Borde = Bordes.BordeIzqInf,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = False
                            }
                        },
                        New Celda With {
                            .Contenido = "VAL. DOLARES",
                            .EstiloCelda = New Estilos With {
                                .AlineacionTexto = Alineaciones.AlCentro,
                                .Borde = Bordes.BordeDerIzqInf,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = False
                            }
                        }
                    }
                }
            }

                With documento_.Seccion(SeccionesPedimento.ANS10)

                    With .Seccion(SeccionesPedimento.ANS13)

                        Dim numeroLinea_ As Integer = 1

                        For Each p_ As Nodo In .Nodos

                            If p_.TipoNodo = Nodo.TiposNodo.Partida Then

                                datosFacturaProveedor_.Filas.Add(New Fila)

                                Dim proveedor_ = CType(p_, PartidaGenerica)

                                With datosFacturaProveedor_

                                    .Filas(numeroLinea_).ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = proveedor_.Attribute(CA_CFDI_O_FACT).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlDerecha,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = CType(proveedor_.Attribute(CA_FECHA_FACT).Valor, Date),
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = proveedor_.Attribute(CA_INCOTERM).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = proveedor_.Attribute(CA_CVE_MONEDA_FACT).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = proveedor_.Attribute(CA_MONTO_MONEDA_FACT).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlDerecha,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                     },
                                    New Celda With {
                                        .Contenido = proveedor_.Attribute(CA_FACTOR_MONEDA).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlDerecha,
                                            .Borde = Bordes.BordeIzquierdo,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = proveedor_.Attribute(CA_MONTO_USD).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlDerecha,
                                            .Borde = Bordes.BordeDerIzq,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    }
                                }

                                    numeroLinea_ += 1

                                End With

                                listaTablas_.Add(datosFacturaProveedor_)

                            End If

                        Next

                    End With

                End With

            End If

#End Region

#Region "Datos del destinatario"

            If documento_.Seccion(SeccionesPedimento.ANS20).Nodos IsNot Nothing Then

                listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {16.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "DATOS DEL DESTINATARIO",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                }
                            }
                        }
                    }
                })

                listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {10.0F, 60.0F, 21.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "ID. FISCAL",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "NOMBRE,DENOMINACION O RAZON SOCIAL",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "DOMICILIO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        },
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_ID_FISCAL_PROVEEDOR).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_NOMBRE_DEN_RAZON_SOC_POC).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_DOMICILIO_POC).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            End If

#End Region

#Region "Acuse de valor (COVE) del proveedor o comprador"

            listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {16.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "DATOS DEL PROVEEDOR O COMPRADOR",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                }
                            }
                        }
                    }
                })

            listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {10.0F, 8.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NUMERO DE ACUSE DE VALOR",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "VINCULACION",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "INCOTERM",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })



            With documento_.Seccion(SeccionesPedimento.ANS10) '.Seccion(SeccionesPedimento.ANS13)

                With .Seccion(SeccionesPedimento.ANS13) 'Matriz facturas y numero de COVE
                    Dim proveedorAcuseValor_ As New Tabla()

                    proveedorAcuseValor_.AnchoColumna = {10.0F, 8.0F, 8.0F}

                    Dim numeroLinea_ As Integer = 0

                    For Each p_ As Nodo In .Nodos

                        If p_.TipoNodo = Nodo.TiposNodo.Partida Then

                            proveedorAcuseValor_.Filas.Add(New Fila)

                            Dim proveedor_ = CType(p_, PartidaGenerica)

                            With proveedorAcuseValor_

                                Dim acuseValor_ = " "

                                Dim vinculacion_ = " "

                                Dim incoterm_ = " "

                                If proveedor_.Attribute(CA_NUMERO_ACUSE_DE_VALOR).Valor IsNot Nothing Then

                                    acuseValor_ = proveedor_.Attribute(CA_NUMERO_ACUSE_DE_VALOR).Valor

                                End If

                                If proveedor_.Attribute(CA_VINCULACION) IsNot Nothing Then

                                    vinculacion_ = proveedor_.Attribute(CA_VINCULACION).Valor

                                End If

                                If proveedor_.Attribute(CA_INCOTERM).Valor IsNot Nothing Then

                                    incoterm_ = proveedor_.Attribute(CA_INCOTERM).Valor

                                End If

                                .Filas(numeroLinea_).ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = acuseValor_,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeIzqInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = vinculacion_,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeIzqInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = incoterm_,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerIzqInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    }
                                }

                                numeroLinea_ += 1

                            End With

                        End If

                    Next

                    listaTablas_.Add(proveedorAcuseValor_)

                End With

            End With


#End Region

#Region "Datos del transporte"

            listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {9.0F, 58.0F, 8.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "TRANSPORTE",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                },
                                New Celda With {
                                    .Contenido = "IDENTIFICACIÓN: " + documento_.Attribute(CA_ID_TRANSPORT).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "PAIS: " + documento_.Attribute(CA_CVE_PAIS_TRANSP).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

#End Region

#Region "Candados"

            Dim candados_ As New Tabla()

            candados_.AnchoColumna = {3.0F, 2.0F, 3.0F, 3.0F, 2.0F, 3.0F, 3.0F} '34

            If documento_.Seccion(SeccionesPedimento.ANS15).Nodos IsNot Nothing Then

                candados_.Filas = New List(Of Fila) From {
                New Fila With {
                    .ListCeldas = New List(Of Celda) From {
                        New Celda With {
                            .Contenido = "NÚMERO DE CANDADO",
                            .EstiloCelda = New Estilos With {
                                .AlineacionTexto = Alineaciones.AlDerecha,
                                .Borde = Bordes.BordeDerecho,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = True
                            }
                        }
                    }
                }
            }

                With documento_.Seccion(SeccionesPedimento.ANS15)

                    Dim numeroLinea_ As Integer = 0

                    For Each p_ As Nodo In .Nodos

                        If p_.TipoNodo = Nodo.TiposNodo.Partida Then

                            candados_.Filas.Add(New Fila)

                            Dim proveedor_ = CType(p_, PartidaGenerica)

                            With candados_

                                .Filas(0).ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = documento_.Attribute(CA_NUM_CANDADO).Valor,
                                        .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerecho,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                }
                            }

                                numeroLinea_ += 1

                            End With

                            listaTablas_.Add(candados_)

                        End If

                    Next

                End With

                Dim cancado1erRevision_ = " "

                Dim cancado2daRevision_ = " "

                If documento_.Attribute(CA_NUM_CANDADO_1RA).Valor IsNot Nothing Then

                    cancado1erRevision_ = documento_.Attribute(CA_NUM_CANDADO_1RA).Valor

                End If

                If documento_.Attribute(CA_NUM_CANDADO_2DA) IsNot Nothing Then

                    cancado2daRevision_ = documento_.Attribute(CA_NUM_CANDADO_2DA).Valor

                End If

                listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {5.0F, 9.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "1RA. REVISIÓN",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_NUM_CANDADO_1RA).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        },
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "2DA. REVISIÓN",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = documento_.Attribute(CA_NUM_CANDADO_2DA).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            End If

#End Region

#Region "Guias, manifiestos, conocimientos de embarque"

            Dim numGuiaEmbarque_ As New Tabla()

            numGuiaEmbarque_.AnchoColumna = {7.0F, 4.0F, 1.0F, 4.0F, 1.0F, 4.0F, 1.0F} '34

            If documento_.Seccion(SeccionesPedimento.ANS16).Nodos IsNot Nothing Then

                With documento_.Seccion(SeccionesPedimento.ANS16)

                    Dim numeroLinea_ As Integer = 0

                    For Each p_ As Nodo In .Nodos

                        If p_.TipoNodo = Nodo.TiposNodo.Partida Then

                            numGuiaEmbarque_.Filas.Add(New Fila)

                            Dim numGuia_ = CType(p_, PartidaGenerica)

                            With numGuiaEmbarque_

                                .Filas = New List(Of Fila) From {
                                New Fila With {
                                    .ListCeldas = New List(Of Celda) From {
                                        New Celda With {
                                            .Contenido = "NUMERO (GUIA/ORDEN EMBARQUE)/ID:",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlIzquierdo,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = True
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = numGuia_.Attribute(CA_GUIA_O_MANIF_O_BL).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = numGuia_.Attribute(CA_MASTER_O_HOUSE).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        }
                                    }
                                }
                            }

                                numeroLinea_ += 1

                            End With

                            listaTablas_.Add(numGuiaEmbarque_)

                        End If

                    Next

                End With

            End If

#End Region

#Region "Número y tipo Contenedores/Equipo ferrocarril/Número economico del vehiculo"

            Dim numContenedor_ As New Tabla()

            numContenedor_.AnchoColumna = {7.0F, 4.0F, 1.0F, 4.0F, 1.0F, 4.0F, 1.0F} '34

            If documento_.Seccion(SeccionesPedimento.ANS17).Nodos IsNot Nothing Then

                With documento_.Seccion(SeccionesPedimento.ANS17)

                    Dim numeroLinea_ As Integer = 0

                    For Each p_ As Nodo In .Nodos

                        If p_.TipoNodo = Nodo.TiposNodo.Partida Then

                            numContenedor_.Filas.Add(New Fila)

                            Dim contenedor_ = CType(p_, PartidaGenerica)

                            With numContenedor_

                                .Filas = New List(Of Fila) From {
                                New Fila With {
                                    .ListCeldas = New List(Of Celda) From {
                                        New Celda With {
                                            .Contenido = "NÚMERO/TIPO:",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlIzquierdo,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = True
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = contenedor_.Attribute(CA_NUM_CONTENEDOR_FERRO_NUM_ECON).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = contenedor_.Attribute(CA_CVE_TIPO_CONTENEDOR).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        }
                                    }
                                }
                            }

                            End With

                            listaTablas_.Add(numContenedor_)

                        End If

                    Next

                End With

            End If

#End Region

#Region "Identificadores"

            If documento_.Seccion(SeccionesPedimento.ANS18).Nodos IsNot Nothing Then

                listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {5.0F, 1.0F, 4.0F, 4.0F, 4.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "CLAVE / COMPL. IDENTIFICADOR",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "COMPLEMENTO 1",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "COMPLEMENTO 2",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "COMPLEMENTO 3",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

                With documento_.Seccion(SeccionesPedimento.ANS18)

                    Dim identificadores_ As New Tabla()

                    identificadores_.AnchoColumna = {5.0F, 1.0F, 4.0F, 4.0F, 4.0F}

                    For Each p_ As Nodo In .Nodos

                        If p_.TipoNodo = Nodo.TiposNodo.Partida Then

                            Dim identificador_ = CType(p_, PartidaGenerica)

                            identificadores_.Filas.Add(
                                New Fila With {
                                    .ListCeldas = New List(Of Celda) From {
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzquierdo,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = identificador_.Attribute(CA_CVE_IDENTIFICADOR_G).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzquierdo,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = identificador_.Attribute(CA_COMPL_1).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzquierdo,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = identificador_.Attribute(CA_COMPL_2).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzquierdo,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = identificador_.Attribute(CA_COMPL_3).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerIzq,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        }
                                    }
                                })

                        End If

                    Next

                    listaTablas_.Add(identificadores_)

                End With

            End If

#End Region

            'Se debe verificar si se encuentra un ejemplo del formato
#Region "Cuentas aduaneras"

#End Region

#Region "Descargos"

            If documento_.Seccion(SeccionesPedimento.ANS20).Nodos IsNot Nothing Then

                listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {16.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "DESCARGOS",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 }
                            }
                        }
                    }
                })

                listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {6.0F, 6.0F, 6.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NUM. PEDIMENTO ORIGINAL:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "FECHA DE OPERACION ORIGINAL:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "CVE. PEDIMENTO ORIGINAL:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

                Dim descargos_ As New Tabla()

                descargos_.AnchoColumna = {6.0F, 6.0F, 6.0F}

                With documento_.Seccion(SeccionesPedimento.ANS20)

                    Dim numeroLinea_ As Integer = 1

                    For Each d_ As Nodo In .Nodos

                        If d_.TipoNodo = Nodo.TiposNodo.Partida Then

                            descargos_.Filas.Add(New Fila)

                            Dim descargo_ = CType(d_, PartidaGenerica)

                            With descargos_

                                .Filas(numeroLinea_).ListCeldas = New List(Of Celda) From {
                                        New Celda With {
                                            .Contenido = descargo_.Attribute(CA_NUM_PEDIM_ORIGINAL_COMPLETO).Valor,
                                            .EstiloCelda = New Estilos With {
                                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                                .Borde = Bordes.BordeIzquierdo,
                                                                .DimensionFuente = Fuentes.Medio,
                                                                .Sombreado = False
                                                            }
                                        },
                                        New Celda With {
                                           .Contenido = CType(descargo_.Attribute(CA_FECHA_PEDIM_ORIGINAL).Valor, Date),
                                           .EstiloCelda = New Estilos With {
                                                               .AlineacionTexto = Alineaciones.AlCentro,
                                                               .Borde = Bordes.SinBordes,
                                                               .DimensionFuente = Fuentes.Medio,
                                                               .Sombreado = False
                                                           }
                                        },
                                        New Celda With {
                                            .Contenido = descargo_.Attribute(CA_CVE_PEDIM_ORIGINAL).Valor,
                                            .EstiloCelda = New Estilos With {
                                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                                .Borde = Bordes.BordeIzquierdo,
                                                                .DimensionFuente = Fuentes.Medio,
                                                                .Sombreado = False
                                                            }
                                        }
                                    }

                                numeroLinea_ += 1

                            End With

                            listaTablas_.Add(descargos_)

                        End If

                    Next

                End With

            End If

#End Region

#Region "Compensaciones"

#End Region

#Region "Formas de pago virtuales"

#End Region

#Region "Observaciones"

            listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {16.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "OBSERVACIONES",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 }
                            }
                        }
                    }
                })

            If documento_.Seccion(SeccionesPedimento.ANS23).Nodos IsNot Nothing Then

                With documento_.Seccion(SeccionesPedimento.ANS23)

                    Dim observaciones_ As New Tabla()

                    observaciones_.AnchoColumna = {16.0F}

                    For Each d_ As Nodo In .Nodos '11 items m3

                        If d_.TipoNodo = Nodo.TiposNodo.Partida Then

                            Dim observacion_ = CType(d_, PartidaGenerica)

                            Dim estiloCelda_ As New Estilos With {
                                .AlineacionTexto = Alineaciones.AlIzquierdo,
                                .Borde = Bordes.BordeDerIzq,
                                .DimensionFuente = Fuentes.Medio,
                                .Sombreado = False
                            }

                            observaciones_.Filas.Add(
                                New Fila With {
                                    .ListCeldas = New List(Of Celda) From {
                                        New Celda With {
                                            .Contenido = observacion_.Attribute(CA_OBSERV_PEDIM).Valor,
                                            .EstiloCelda = estiloCelda_
                                        }
                                    }
                                })

                        End If

                    Next

                    listaTablas_.Add(observaciones_)

                End With

            End If

#End Region

#Region "Partidas"

            listaTablas_.Add(
                New Tabla() With {
                    .AnchoColumna = {16.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "PARTIDAS",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.Bordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = True
                                    }
                                 }
                            }
                        }
                    }
                })

            If documento_.Seccion(SeccionesPedimento.ANS24).Nodos IsNot Nothing Then

                Dim partidasFraccion_ As New Tabla()

                Dim partidasDescripcion_ As New Tabla()

                Dim partidasValAduana_ As New Tabla()

                partidasFraccion_.AnchoColumna = {1.0F, 2.5F, 3.0F, 1.5F, 1.5F, 1.5F, 2.5F, 1.5F, 2.5F, 1.5F, 1.5F, 1.0F, 1.5F, 1.0F, 1.0F, 2.0F}

                partidasDescripcion_.AnchoColumna = {1.0F, 18.0F, 1.5F, 2.0F, 1.0F, 1.0F, 2.0F}

                partidasValAduana_.AnchoColumna = {1.0F, 3.0F, 3.0F, 3.0F, 3.0F, 4.0F, 1.5F, 2.0F, 1.0F, 1.0F, 2.0F}


                With documento_.Seccion(SeccionesPedimento.ANS24)

                    For Each d_ As Nodo In .Nodos

                        listaTablas_.Add(
                            New Tabla() With {
                                .AnchoColumna = {1.0F, 2.5F, 3.0F, 1.5F, 1.5F, 1.5F, 2.5F, 1.5F, 2.5F, 1.5F, 1.5F, 7.0F},
                                .Filas = New List(Of Fila) From {
                                    New Fila With {
                                        .ListCeldas = New List(Of Celda) From {
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "FRACCION",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "SUBD/NÚM.ID ENTIFICACIÓN COMERCIAL",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "VINC.",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "MET VAL",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "UMC",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "CANTIDAD UMC",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "UMT",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "CANTIDAD UMT",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "P.V/C",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "P.O/D",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            }
                                        }
                                    }
                                }
                            })

                        listaTablas_.Add(
                            New Tabla() With {
                                .AnchoColumna = {1.0F, 18.0F, 1.5F, 1.5F, 1.0F, 1.0F, 2.0F},
                                .Filas = New List(Of Fila) From {
                                    New Fila With {
                                        .ListCeldas = New List(Of Celda) From {
                                            New Celda With {
                                                .Contenido = "SEC",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "DESCRIPCION (RENGLONES VARIABLES SEGUN SE REQUIERA)",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "CON.",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "TASA",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "T.T",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "F.P",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "IMPORTE",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            }
                                        }
                                    }
                                }
                            })

                        listaTablas_.Add(
                            New Tabla() With {
                                .AnchoColumna = {1.0F, 3.0F, 4.0F, 3.0F, 3.0F, 2.0F, 1.0F, 2.0F, 1.0F, 1.0F, 2.0F},
                                .Filas = New List(Of Fila) From {
                                    New Fila With {
                                        .ListCeldas = New List(Of Celda) From {
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "VAL ADU/USD",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "IMP. PRECIO PAG.",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "PRECIO UNIT.",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "VAL. AGREG.",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            }
                                        }
                                    }
                                }
                            })

                        listaTablas_.Add(
                            New Tabla() With {
                                .AnchoColumna = {1.0F, 5.0F, 5.0F, 5.0F, 1.0F, 2.0F, 1.0F, 1.0F, 2.0F},
                                .Filas = New List(Of Fila) From {
                                    New Fila With {
                                        .ListCeldas = New List(Of Celda) From {
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "MARCA",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "MODELO",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "CODIGO PRODUCTO",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            }
                                        }
                                    }
                                }
                            })

                        If d_.TipoNodo = Nodo.TiposNodo.Partida Then

                            Dim partida_ = CType(d_, PartidaGenerica)

                            partidasFraccion_.Filas.Add(
                                New Fila With {
                                    .ListCeldas = New List(Of Celda) From {
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_NUM_SEC_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzquierdo,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_FRACC_ARANC_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_NICO_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_VINCULACION).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_CVE_MET_VALOR_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_CVE_UMC_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_CANT_UMC_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_CVE_UMT_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_CANT_UMT_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_CVE_PAIS_VEND_O_COMP_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_CVE_PAIS_ORIGEN_O_DEST_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        }
                                    }
                                })

                            partidasDescripcion_.Filas.Add(
                                New Fila With {
                                    .ListCeldas = New List(Of Celda) From {
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzquierdo,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_DESCRIP_MERC_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        }
                                    }
                                })

                            partidasValAduana_.Filas.Add(
                                New Fila With {
                                    .ListCeldas = New List(Of Celda) From {
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzquierdo,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_VAL_ADU_O_VAL_USD_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_IMP_PRECIO_PAG_O_VAL_COMER_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_MONTO_PRECIO_UNITARIO_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = partida_.Attribute(CA_MONTO_VALOR_AGREG_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeIzqInf,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        },
                                        New Celda With {
                                            .Contenido = " ",
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        }
                                    }
                                })

                        End If

                    Next

                End With

                With documento_.Seccion(SeccionesPedimento.ANS29)

                    Dim contadorContribucionPartida_ As Integer = 0

                    For Each cp_ As Nodo In .Nodos

                        If cp_.TipoNodo = Nodo.TiposNodo.Partida Then

                            Dim contribucionPartida_ = CType(cp_, PartidaGenerica)

                            Select Case contadorContribucionPartida_

                                Case 0

                                    partidasFraccion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_CVE_CONTRIBUCION_NIVEL_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasFraccion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_TASA_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasFraccion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_CVE_T_TASA_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasFraccion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_FP_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasFraccion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                Case 1

                                    partidasDescripcion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_CVE_CONTRIBUCION_NIVEL_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasDescripcion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_TASA_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasDescripcion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_CVE_T_TASA_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasDescripcion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_FP_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasDescripcion_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                Case 2

                                    partidasValAduana_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_CVE_CONTRIBUCION_NIVEL_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasValAduana_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_TASA_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasValAduana_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_CVE_T_TASA_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasValAduana_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_FP_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                                    partidasValAduana_.Filas(0).ListCeldas.Add(
                                        New Celda With {
                                            .Contenido = contribucionPartida_.Attribute(CA_IMPORTE_PARTIDA).Valor,
                                            .EstiloCelda = New Estilos With {
                                                .AlineacionTexto = Alineaciones.AlCentro,
                                                .Borde = Bordes.BordeDerecho,
                                                .DimensionFuente = Fuentes.Medio,
                                                .Sombreado = False
                                            }
                                        })

                            End Select

                            contadorContribucionPartida_ += 1

                        End If

                    Next

                    listaTablas_.Add(partidasFraccion_)
                    listaTablas_.Add(partidasDescripcion_)
                    listaTablas_.Add(partidasValAduana_)

                End With

                If documento_.Seccion(SeccionesPedimento.ANS26).Nodos IsNot Nothing Then

                    With documento_.Seccion(SeccionesPedimento.ANS26)

                        Dim partidaRegulacion_ As New Tabla()

                        partidaRegulacion_.AnchoColumna = {1.0F, 2.0F, 5.0F, 4.0F, 3.0F, 3.0F, 1.0F, 1.5F, 1.0F, 1.0F, 2.0F}

                        For Each cp_ As Nodo In .Nodos

                            listaTablas_.Add(
                                New Tabla() With {
                                    .AnchoColumna = {1.0F, 2.0F, 5.0F, 4.0F, 3.0F, 3.0F, 1.0F, 1.5F, 1.0F, 1.0F, 2.0F},
                                    .Filas = New List(Of Fila) From {
                                        New Fila With {
                                            .ListCeldas = New List(Of Celda) From {
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "CLAVE",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "NUM. PERMISO O NOM",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "FIRMA DESCARGO",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "VAL. COM. DLS.",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "CANTIDAD UMT/C",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerecho,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerecho,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerecho,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerecho,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerecho,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                }
                                            }
                                        }
                                    }
                                })

                            If cp_.TipoNodo = Nodo.TiposNodo.Partida Then

                                Dim RegulacionPartida_ = CType(cp_, PartidaGenerica)

                                partidaRegulacion_.Filas.Add(
                                    New Fila With {
                                        .ListCeldas = New List(Of Celda) From {
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzquierdo,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = RegulacionPartida_.Attribute(CA_CVE_PERMISO).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = RegulacionPartida_.Attribute(CA_NUM_PERMISO).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = RegulacionPartida_.Attribute(CA_FIRM_ELECTRON_PERMISO).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = RegulacionPartida_.Attribute(CA_MONTO_USD_VAL_COM).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = RegulacionPartida_.Attribute(CA_CANT_UMT_O_UMC).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzqInf,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.SinBordes,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            }
                                        }
                                    })

                            End If

                        Next

                        listaTablas_.Add(partidaRegulacion_)

                    End With

                End If

                If documento_.Seccion(SeccionesPedimento.ANS27).Nodos IsNot Nothing Then

                    With documento_.Seccion(SeccionesPedimento.ANS27)

                        Dim partidaIdentificadores_ As New Tabla()

                        partidaIdentificadores_.AnchoColumna = {1.0F, 3.0F, 5.0F, 5.0F, 5.0F, 1.0F, 1.5F, 1.0F, 1.0F, 2.0F}

                        listaTablas_.Add(
                                New Tabla() With {
                                    .AnchoColumna = {1.0F, 3.0F, 5.0F, 5.0F, 5.0F, 1.0F, 1.5F, 1.0F, 1.0F, 2.0F},
                                    .Filas = New List(Of Fila) From {
                                        New Fila With {
                                            .ListCeldas = New List(Of Celda) From {
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "IDENTIF.",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "COMPLEMENTO 1",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "COMPLEMENTO 2",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeIzquierdo,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = "COMPLEMENTO 3",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerIzq,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerecho,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerecho,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerecho,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.SinBordes,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                },
                                                New Celda With {
                                                    .Contenido = " ",
                                                    .EstiloCelda = New Estilos With {
                                                        .AlineacionTexto = Alineaciones.AlCentro,
                                                        .Borde = Bordes.BordeDerIzq,
                                                        .DimensionFuente = Fuentes.Medio,
                                                        .Sombreado = False
                                                    }
                                                }
                                            }
                                        }
                                    }
                                })

                        For Each iPartida_ As Nodo In .Nodos

                            If iPartida_.TipoNodo = Nodo.TiposNodo.Partida Then

                                Dim identificadorPartida_ = CType(iPartida_, PartidaGenerica)

                                partidaIdentificadores_.Filas.Add(
                                    New Fila With {
                                        .ListCeldas = New List(Of Celda) From {
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzquierdo,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = identificadorPartida_.Attribute(CA_CVE_IDENTIF_PARTIDA).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzquierdo,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = identificadorPartida_.Attribute(CA_COMPL_1_PARTIDA).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzquierdo,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = identificadorPartida_.Attribute(CA_COMPL_2_PARTIDA).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzquierdo,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = identificadorPartida_.Attribute(CA_COMPL_3_PARTIDA).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerIzq,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.SinBordes,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerIzq,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            }
                                        }
                                    })

                            End If

                        Next

                        listaTablas_.Add(partidaIdentificadores_)

                    End With

                End If

                With documento_.Seccion(SeccionesPedimento.ANS36)

                    Dim partidaObservaciones_ As New Tabla()

                    partidaObservaciones_.AnchoColumna = {1.0F, 18.0F, 1.0F, 1.5F, 1.0F, 1.0F, 2.0F}

                    listaTablas_.Add(
                            New Tabla() With {
                                .AnchoColumna = {1.0F, 18.0F, 1.0F, 1.5F, 1.0F, 1.0F, 2.0F},
                                .Filas = New List(Of Fila) From {
                                    New Fila With {
                                        .ListCeldas = New List(Of Celda) From {
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeIzquierdo,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = "OBSERVACIONES A NIVEL PARTIDA",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.Bordes,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerecho,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.SinBordes,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            },
                                            New Celda With {
                                                .Contenido = " ",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.BordeDerIzq,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            }
                                        }
                                    }
                                }
                            })

                    For Each oPartida_ As Nodo In .Nodos

                        If oPartida_.TipoNodo = Nodo.TiposNodo.Partida Then

                            Dim observacionPartida_ = CType(oPartida_, PartidaGenerica)

                            partidaObservaciones_.Filas.Add(
                            New Fila With {
                                .ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeIzqInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = observacionPartida_.Attribute(CA_OBSERV_PARTIDA).Valor,
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerIzqInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeInferior,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    },
                                    New Celda With {
                                        .Contenido = " ",
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlCentro,
                                            .Borde = Bordes.BordeDerIzqInf,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    }
                                }
                            })

                        End If

                    Next

                    listaTablas_.Add(partidaObservaciones_)

                End With

            End If


#End Region

#Region "Final del pedimento"

            listaTablas_.Add(
                New Tabla() With {
                                .AnchoColumna = {10.0F, 6.0F, 10.0F},
                                .Filas = New List(Of Fila) From {
                                    New Fila With {
                                        .ListCeldas = New List(Of Celda) From {
                                            New Celda With {
                                                .Contenido = "********* " + documento_.Attribute(CA_FIN_PEDIMENTO).Valor + " *********",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.SinBordes,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                             },
                                            New Celda With {
                                                .Contenido = "NUM. TOTAL DE PARTIDAS: " + documento_.Attribute(CA_NUMERO_TOTAL_PARTIDAS).Valor,
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.SinBordes,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                             },
                                            New Celda With {
                                                .Contenido = "**** CLAVE DEL PREVALIDADOR: " + documento_.Attribute(CA_OBSERV_PEDIM).Valor + " ****",
                                                .EstiloCelda = New Estilos With {
                                                    .AlineacionTexto = Alineaciones.AlCentro,
                                                    .Borde = Bordes.SinBordes,
                                                    .DimensionFuente = Fuentes.Medio,
                                                    .Sombreado = False
                                                }
                                            }
                                        }
                                    }
                                }
                            })

#End Region

#End Region

#Region "Pie de página"

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {59.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "AGENTE ADUANAL,AGENCIA ADUANAL,APODERADO ADUANAL O DE ALMACEN NOMBRE O RAZ. SOC.:" + documento_.Attribute(CA_RFC_AA).Valor.ToString,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "DECLARO BAJO PROTESTA DE DECIR VERDAD, EN LOS TERMINOS DE LO DISPUESTO POR EL ARTICULO 81 DE LA LEY ADUANERA: PATENTE O AUTORIZACION: " + documento_.Attribute(CA_PATENTE).Valor.ToString,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {30.0F, 29.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "RFC: " + documento_.Attribute(CA_RFC_AA).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "CURP: " + documento_.Attribute(CA_CURP_AA_O_REP_LEGAL).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.SinBordes,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {59.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "MANDATARIO/PERSONA AUTORIZADA",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {59.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NOMBRE: " + documento_.Attribute(CA_NOMBRE_MAND_REP_AA).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzquierdo,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {30.0F, 29.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "RFC: " + documento_.Attribute(CA_RFC_MAND_O_AGAD_REP_ALMACEN).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = "CURP: " + documento_.Attribute(CA_CURP_MAND_O_AGAD_REP_ALMACEN).Valor,
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {30.0F, 29.0F, 37.0F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "NÚMERO DE SERIE DEL CERTIFICADO:",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = IIf(documento_.Attribute(CA_CERTIFICADO_FIRMA).Valor IsNot Nothing, documento_.Attribute(CA_CERTIFICADO_FIRMA).Valor, " "),
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeInferior,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                },
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlCentro,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = "e.firma: ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlIzquierdo,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                            New Fila With {
                                .ListCeldas = New List(Of Celda) From {
                                    New Celda With {
                                        .Contenido = IIf(documento_.Attribute(CA_EFIRMA).Valor IsNot Nothing, documento_.Attribute(CA_EFIRMA).Valor, " "),
                                        .EstiloCelda = New Estilos With {
                                            .AlineacionTexto = Alineaciones.AlIzquierdo,
                                            .Borde = Bordes.BordeDerIzq,
                                            .DimensionFuente = Fuentes.Medio,
                                            .Sombreado = False
                                        }
                                    }
                                }
                            }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerIzq,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                })

            listaPiePagina_.Add(
                New Tabla() With {
                    .AnchoColumna = {91.5F},
                    .Filas = New List(Of Fila) From {
                        New Fila With {
                            .ListCeldas = New List(Of Celda) From {
                                New Celda With {
                                    .Contenido = " ",
                                    .EstiloCelda = New Estilos With {
                                        .AlineacionTexto = Alineaciones.AlDerecha,
                                        .Borde = Bordes.BordeDerIzqInf,
                                        .DimensionFuente = Fuentes.Medio,
                                        .Sombreado = False
                                    }
                                 }
                            }
                        }
                    }
                })

#End Region

            With documento_

                numOperacion_ = String.Format(.FolioDocumento).Replace(" ", "")

            End With

            Dim constructorPDF_ = New ControladorImpresionDocumento(numOperacion_)

            constructorPDF_.CreaTablasDocumento(listaTablas_, listaEncabezado_, listaEncabezadoSecundario_, listaPiePagina_)

        End Sub

        Public Function ImpresionEncabezado(ByVal doc_ As DocumentoElectronico) As String

            Return _ControladorPedimentoPDF.ImprimirEncabezadoPedimentoNormal(_documentoElectronico)

        End Function

#End Region

#Region "Funtions"

#End Region

    End Class

End Namespace