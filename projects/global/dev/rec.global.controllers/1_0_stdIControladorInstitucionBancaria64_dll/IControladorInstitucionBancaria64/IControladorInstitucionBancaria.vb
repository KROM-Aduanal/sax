﻿Imports System.IO
Imports System.Text.RegularExpressions
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
Imports Wma.Exceptions

Public Interface IControladorInstitucionBancaria : Inherits IDisposable

#Region "Enum"
    Enum Modalidades
        Interno = 0
        Externo = 1
    End Enum

    Enum CamposBusquedaSimple
        SinDefinir = 0
        IDS = 1
        IDBANCARIO = 2
        CLAVEUSONUEVO = 3
        CLAVEUSOBUSCAACTUALIZA = 4
        NOMBRECOMERCIALNUEVO = 5
        NOMBRECOMERCIALBUSCAACTUALIZA = 6
        RAZONSOCIAL = 7
        DOMICILIOFISCAL = 8
        METADATOS = 9
        ARCHIVADO = 10
        ESTADO = 11
    End Enum
#End Region


#Region "Propiedades"
    Property InstitucionesBancarias As List(Of InstitucionBancaria)

    Property ModalidadTrabajo As Modalidades

    ReadOnly Property Estado As TagWatcher


#End Region

#Region "Métodos"

    Function NuevoBanco(ByVal banco_ As InstitucionBancaria,
                                 Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher
    Function ActualizaBanco(ByVal banco_ As InstitucionBancaria,
                               Optional ByVal session_ As IClientSessionHandle = Nothing) As TagWatcher
    Function ActualizaBanco(idBanco As ObjectId,
                                   ByVal tokens_ As Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object),
                                   Optional session_ As IClientSessionHandle = Nothing) As TagWatcher
    Function BuscarBancos(ByVal tokens_ As Dictionary(Of CamposBusquedaSimple, Object),
                          modalidad_ As Modalidades) As TagWatcher

#End Region

End Interface

