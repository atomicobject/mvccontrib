Imports MVCContrib.PortableAreas


Namespace PortableAreas
    <CLSCompliant(False)>
    Public Class PortableAreaRegistrationUnderSubnamespace
        Inherits PortableAreaRegistration

        Public Const PortableAreaName = "FooAreaVBUnderSubNamespace"

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext, ByVal bus As IApplicationBus)
            context.MapRoute("ResourceRoute", String.Concat(PortableAreaName.ToLower(), "/resource/{resourceName}"),
                         New With {.controller = "Resource", .action = "Index"})
            Me.RegisterAreaEmbeddedResources()
        End Sub

        Public Overrides ReadOnly Property AreaName As String
            Get
                Return PortableAreaName
            End Get
        End Property
    End Class
End Namespace