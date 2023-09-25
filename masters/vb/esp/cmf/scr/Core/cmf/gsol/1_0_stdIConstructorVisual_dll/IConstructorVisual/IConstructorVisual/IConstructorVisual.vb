Imports System.ComponentModel

Namespace gsol

	Public Interface IConstructorVisual

#Region "Atributos"

		Enum EstatusConstruccion
			NoConstruido = 0
			Construido
			Cancelado
			Fallido
		End Enum
		Enum TipoAplicacion
			NoDefinido = 0
			<Description("System.Windows.Forms")> Escritorio
			<Description("System.Web.UI")> Web
		End Enum
		Enum TipoEntidad
			Independiente = 0
			Dependiente
		End Enum
		Enum TipoEvento
			<Description("Click_CustomEvent")> Click
			<Description("Click_CustomEvent")> NodeMouseClick
		End Enum
		Enum TipoModulo
			Otro
			Grafico = 8
			AccesoRapido = 12
		End Enum
        Enum TipoPropiedad
            Order
            URL
            AllowTabReorder
            Anchor
            AutoSizeContentButtons
            BackColor
            BackHighColor
            BackLowColor
            BorderStyle
            CaptionBarVisible
            Collapsed
            Color
            Dock
            Enabled
            ForeColor
            Height
            Icon
            Image
            ImageIndex
            ImageKey
            ImageList
            LargeImage
            LargeImageList
            LayoutStyle
            Location
            MaximumSize
            MaxSizeMode
            Minimized
            MinimumSize
            MinSizeMode
            Name
            Null
            OrbImage
            OrbStyle
            OrbText
            Padding
            SelectedImageIndex
            SelectedImageKey
            Size
            SmallImage
            SmallImageList
            StateImageList
            Style
            TabCloseButtonImage
            TabCloseButtonImageDisabled
            TabCloseButtonImageHot
            TabCloseButtonSize
            TabCloseButtonVisible
            TabHeight
            TabIconSize
            TabOffset
            TabsMargin
            TabSpacing
            Tag
            Text
            TextAlignment
            ThemeColor
            ToolTip
            Type
            Visible
        End Enum

#End Region

#Region "Propiedades"

		ReadOnly Property Estatus As EstatusConstruccion
		ReadOnly Property Tipo As TipoAplicacion

#End Region

#Region "Métodos"

		Function ConstruirEntornoVisual(ByVal contenedor_ As IList, ByVal manejadoresEventos_ As IList(Of System.Reflection.MethodInfo), ByVal sectoresEntorno_ As IDictionary(Of Int32, ISectorEntorno), ByVal tipoModulo_ As TipoModulo, ByVal filtro_ As String) As Object

#End Region

	End Interface

End Namespace