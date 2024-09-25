'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class Ges022_FacturaComercialExportacion

    '''<summary>
    '''Control __SYSTEM_CONTEXT_FINDER.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents __SYSTEM_CONTEXT_FINDER As Global.Gsol.Web.Components.FindbarControl

    '''<summary>
    '''Control __SYSTEM_ENVIRONMENT.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents __SYSTEM_ENVIRONMENT As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control __SYSTEM_MODULE_FORM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents __SYSTEM_MODULE_FORM As Global.Gsol.Web.Components.FormControl

    '''<summary>
    '''Control fscGenerales.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscGenerales As Global.Gsol.Web.Components.FieldsetControl

    '''<summary>
    '''Control lbModoCapturaIA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbModoCapturaIA As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lbModoCapturaManual.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbModoCapturaManual As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lbModoCapturaManualNuevo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbModoCapturaManualNuevo As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control dbcNumFacturaAcuseValor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dbcNumFacturaAcuseValor As Global.Gsol.Web.Components.DualityBarControl

    '''<summary>
    '''Control icFechaFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFechaFactura As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icFechaAcuseValor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFechaAcuseValor As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icNumeroFacturaUUID.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icNumeroFacturaUUID As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control fbcCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcCliente As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control fbcPais.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcPais As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control fbcIncoterm.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcIncoterm As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control swcEnajenacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents swcEnajenacion As Global.Gsol.Web.Components.SwitchControl

    '''<summary>
    '''Control icValorFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icValorFactura As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaFactura As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icValorMercancia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icValorMercancia As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaMercancia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaMercancia As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icPesoTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icPesoTotal As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icBultos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icBultos As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icOrdenCompra.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icOrdenCompra As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icReferenciaCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icReferenciaCliente As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control fscCompradorReceptor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscCompradorReceptor As Global.Gsol.Web.Components.FieldsetControl

    '''<summary>
    '''Control fbcCompradorReceptor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcCompradorReceptor As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control scDomicilioCompradorReceptor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scDomicilioCompradorReceptor As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control scVinculacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scVinculacion As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control scMetodoValoracion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMetodoValoracion As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control swcEsDestinatario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents swcEsDestinatario As Global.Gsol.Web.Components.SwitchControl

    '''<summary>
    '''Control fscCompradorDestinatario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscCompradorDestinatario As Global.Gsol.Web.Components.FieldsetControl

    '''<summary>
    '''Control fbcCompradorDestinatario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcCompradorDestinatario As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control scDomicilioCompradorDestinatario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scDomicilioCompradorDestinatario As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control fscItems.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscItems As Global.Gsol.Web.Components.FieldsetControl

    '''<summary>
    '''Control pbPartidasItems.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pbPartidasItems As Global.Gsol.Web.Components.PillboxControl

    '''<summary>
    '''Control lbNumero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbNumero As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control fbcProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcProducto As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control icCantidadComercial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icCantidadComercial As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scUnidadMedidaComercial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scUnidadMedidaComercial As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icValorDolaresPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icValorDolaresPartida As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaValorDolaresPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaValorDolaresPartida As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icValorUnitarioPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icValorUnitarioPartida As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaValorUnitarioPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaValorUnitarioPartida As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icPrecioUnitario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icPrecioUnitario As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaPrecioUnitarioPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaPrecioUnitarioPartida As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icPesoNeto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icPesoNeto As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control fbcPaisPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcPaisPartida As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control scMetodoValoracionPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMetodoValoracionPartida As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icOrdenCompraPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icOrdenCompraPartida As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icDescripcionPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icDescripcionPartida As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control swcAplicaCOVE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents swcAplicaCOVE As Global.Gsol.Web.Components.SwitchControl

    '''<summary>
    '''Control icDescripcionCOVE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icDescripcionCOVE As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control lbClasificacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbClasificacion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control icFraccionArancelaria.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFraccionArancelaria As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icFraccionNico.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFraccionNico As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icCantidadTarifa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icCantidadTarifa As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scUnidadMedidaTarifa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scUnidadMedidaTarifa As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control lbMercancia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbMercancia As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control icLote.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icLote As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icNumeroSerie.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icNumeroSerie As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icMarca.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icMarca As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icModelo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icModelo As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icSubmodelo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icSubmodelo As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icKilometraje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icKilometraje As Global.Gsol.Web.Components.InputControl
End Class
