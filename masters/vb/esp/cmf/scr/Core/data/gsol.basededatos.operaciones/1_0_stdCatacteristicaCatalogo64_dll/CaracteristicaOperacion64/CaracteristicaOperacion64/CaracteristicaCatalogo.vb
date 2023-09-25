
Namespace Gsol.BaseDatos.Operaciones

    Public Class CaracteristicaCatalogo
        Implements ICaracteristica, ICloneable

#Region "Atributos"

        Private _llave As ICaracteristica.TipoLlave

        Private _longitud As Int32

        Private _nombre As String

        Private _nombremostrar As String

        Private _tipodato As ICaracteristica.TiposCaracteristica

        Private _visible As ICaracteristica.TiposVisible

        'Avanzada
        Private _puedeinsertar As ICaracteristica.TiposRigorDatos

        Private _puedemodificar As ICaracteristica.TiposRigorDatos

        Private _valordefault As String

        Private _valorasginado As String

        'Filtros
        Private _tipofiltro As ICaracteristica.TiposFiltro

        Private _valorfiltro As String

        Private _permisoconsulta As String

        'Especiales
        Private _nameAsKey As String

        'Tipo Interfaz
        Private _interfaz As String

        Private _reflejar As Int32

#End Region

#Region "Constructores"

        Sub New()

            _llave = ICaracteristica.TipoLlave.SinLlave

            _longitud = 0

            _nombre = Nothing

            _nombremostrar = Nothing

            _tipodato = ICaracteristica.TiposCaracteristica.cUndefined

            _visible = ICaracteristica.TiposVisible.Si

            'Avanzada
            _puedeinsertar = False

            _puedemodificar = False

            _valordefault = Nothing

            _valorasginado = Nothing

            _tipofiltro = ICaracteristica.TiposFiltro.SinDefinir

            _valorfiltro = Nothing

        End Sub

        Sub New(ByVal llave_ As ICaracteristica.TipoLlave,
                ByVal longitud_ As Int32,
                ByVal nombre_ As String,
                ByVal nombremostrar_ As String,
                ByVal tipodato_ As ICaracteristica.TiposCaracteristica,
                ByVal visible_ As Boolean
                )

            _llave = llave_

            _longitud = longitud_

            _nombre = nombre_

            _nombremostrar = nombremostrar_

            _tipodato = tipodato_

            _visible = visible_

            'Avanzada

            _puedeinsertar = False

            _puedemodificar = False

            _valordefault = Nothing

            _valorasginado = Nothing

            _tipofiltro = ICaracteristica.TiposFiltro.SinDefinir

            _valorfiltro = Nothing

        End Sub

#End Region

#Region "Destructores"

#End Region

#Region "Propiedades"

        Public Function Clone() As Object Implements ICloneable.Clone, ICaracteristica.Clone

            Dim OperacionesCatalogoClonada As ICaracteristica = New CaracteristicaCatalogo

            With OperacionesCatalogoClonada

                .Interfaz = Me.Interfaz

                .Llave = Me.Llave

                .Longitud = Me.Longitud

                .NameAsKey = Me.NameAsKey

                .Nombre = Me.Nombre

                .NombreMostrar = Me.NombreMostrar

                .PermisoConsulta = Me.PermisoConsulta

                .PuedeInsertar = Me.PuedeInsertar

                .PuedeModificar = Me.PuedeModificar

                .TipoDato = Me.TipoDato

                .TipoFiltro = Me.TipoFiltro

                .ValorAsignado = Me.ValorAsignado

                .ValorDefault = Me.ValorDefault

                .ValorFiltro = Me.ValorFiltro


                .Visible = Me.Visible

            End With

            Return OperacionesCatalogoClonada

        End Function

        Public Property NameAsKey As String _
        Implements ICaracteristica.NameAsKey
            Get

                Return _nameAsKey

            End Get

            Set(value As String)

                _nameAsKey = value

            End Set

        End Property

        Public Property Interfaz As String Implements ICaracteristica.Interfaz

            Get

                Return _interfaz

            End Get

            Set(value As String)

                _interfaz = value

            End Set

        End Property

        Public Property PermisoConsulta As String Implements ICaracteristica.PermisoConsulta

            Get

                Return _permisoconsulta

            End Get

            Set(value As String)

                _permisoconsulta = value

            End Set

        End Property

        Public Property TipoFiltro As ICaracteristica.TiposFiltro Implements ICaracteristica.TipoFiltro

            Get

                Return _tipofiltro

            End Get

            Set(value As ICaracteristica.TiposFiltro)

                _tipofiltro = value

            End Set

        End Property

        Public Property ValorFiltro As String Implements ICaracteristica.ValorFiltro

            Get

                Return _valorfiltro

            End Get

            Set(value As String)

                _valorfiltro = value

            End Set

        End Property

        Public Property ValorAsignado As String Implements ICaracteristica.ValorAsignado

            Get

                Return _valorasginado

            End Get

            Set(value As String)

                _valorasginado = value

            End Set

        End Property

        Public Property PuedeInsertar As ICaracteristica.TiposRigorDatos Implements ICaracteristica.PuedeInsertar

            Get

                Return _puedeinsertar

            End Get

            Set(value As ICaracteristica.TiposRigorDatos)

                _puedeinsertar = value

            End Set

        End Property

        Public Property PuedeModificar As ICaracteristica.TiposRigorDatos Implements ICaracteristica.PuedeModificar

            Get

                Return _puedemodificar

            End Get

            Set(value As ICaracteristica.TiposRigorDatos)

                _puedemodificar = value

            End Set

        End Property

        Public Property ValorDefault As String _
            Implements ICaracteristica.ValorDefault

            Get
                Return _valordefault
            End Get

            Set(value As String)
                _valordefault = value
            End Set

        End Property

        Public Property NombreMostrar As String Implements ICaracteristica.NombreMostrar

            Get

                Return _nombremostrar

            End Get

            Set(value As String)

                _nombremostrar = value

            End Set

        End Property

        Public Property Llave As ICaracteristica.TipoLlave Implements ICaracteristica.Llave

            Get

                Return _llave

            End Get

            Set(value As ICaracteristica.TipoLlave)

                _llave = value

            End Set

        End Property

        Public Property Longitud As Integer Implements ICaracteristica.Longitud

            Get

                Return _longitud

            End Get

            Set(value As Integer)

                _longitud = value

            End Set

        End Property

        Public Property Nombre As String Implements ICaracteristica.Nombre

            Get

                Return _nombre

            End Get

            Set(value As String)

                _nombre = value

            End Set

        End Property

        Public Property TipoDato As ICaracteristica.TiposCaracteristica Implements ICaracteristica.TipoDato

            Get

                Return _tipodato

            End Get

            Set(value As ICaracteristica.TiposCaracteristica)

                _tipodato = value

            End Set

        End Property

        Public Property Visible As ICaracteristica.TiposVisible Implements ICaracteristica.Visible

            Get

                Return _visible

            End Get

            Set(value As ICaracteristica.TiposVisible)

                _visible = value

            End Set

        End Property

        Public Property Reflejar As Int32 Implements ICaracteristica.Reflejar

            Get

                Return _reflejar

            End Get

            Set(ByVal value_ As Int32)

                _reflejar = value_

            End Set

        End Property

#End Region

#Region "Metodos"

#End Region

    End Class

End Namespace