Imports System.Web.Http
Imports System.Web.Http.Cors

Public Class WebApiConfig
    Public Shared Sub Register(ByVal config As HttpConfiguration)

        Dim cors As New EnableCorsAttribute("http://localhost:4200", "*", "*")
        config.EnableCors(cors)

        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )
    End Sub
End Class
