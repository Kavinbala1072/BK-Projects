Imports System.Web.Services

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
Public Class WebService1
    Inherits System.Web.Services.WebService

    <System.Web.Services.WebMethod()>
    Public Sub HandleOptionsRequest()

        Dim origin As String = Context.Request.Headers("Origin")

        If origin = "http://localhost:4200" OrElse origin = "http://localhost:3000" OrElse origin = "https://angularbilling.netlify.app" Then
            Context.Response.Headers.Add("Access-Control-Allow-Origin", origin)
            Context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS")
            Context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type")
            Context.Response.StatusCode = 200
        Else
            Context.Response.StatusCode = 403
        End If

        Context.Response.End()
    End Sub

End Class