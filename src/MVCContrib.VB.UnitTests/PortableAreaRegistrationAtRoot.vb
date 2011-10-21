Imports MVCContrib.PortableAreas


<CLSCompliant(False)>
Public Class PortableAreaRegistrationAtRoot
    Inherits PortableAreaRegistration

    Public Const PortableAreaName = "FooAreaVB"

    Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext, ByVal bus As IApplicationBus)
        context.MapRoute("ResourceRoute", "fooareavb/resource/{resourceName}",
                         New With {.controller = "Resource", .action = "Index"})
        Me.RegisterAreaEmbeddedResources()
    End Sub

    Public Overrides ReadOnly Property AreaName As String
        Get
            Return PortableAreaName
        End Get
    End Property
End Class