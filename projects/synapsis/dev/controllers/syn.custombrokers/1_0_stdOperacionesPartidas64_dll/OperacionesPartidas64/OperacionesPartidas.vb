Imports Wma.Exceptions

Public MustInherit Class OperacionesPartidas

    Public MustOverride Function Agregar() As TagWatcher

    Public MustOverride Function Actualizar() As TagWatcher

    Public MustOverride Function Eliminar() As TagWatcher

    Public MustOverride Function Archivar() As TagWatcher

End Class